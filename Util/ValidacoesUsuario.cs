using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Util
{
    public static class ValidacoesUsuario
    {
        /// <summary>
        /// Verifica se o usuário, ou se um dos perfis cadastros tem permissão para acessar determinada tela do sistema.
        /// </summary>
        /// <param name="usuarioLogado">Usuário logado no sistema.</param>
        /// <param name="class_namespace">Namespace da tela que está sendo acessada.</param>
        /// <returns>True, se permitido. Do contrário False.</returns>
        public static bool ValidarAcessoTela(T_Usuario usuarioLogado, string class_namespace)
        {
            if (usuarioLogado.USE_NOME.Equals("adm", StringComparison.OrdinalIgnoreCase))
                return true; // por padrao o usuário adm tem todas as permissões

            bool usuarioTemPermissao = usuarioLogado.T_USUARIO_OBJETO_CONTROLAVEL
                .Any(o => o.OBJ_ID.Equals(class_namespace, StringComparison.OrdinalIgnoreCase));

            if (usuarioTemPermissao)
                return true;

            var perfisUsuario = usuarioLogado.T_Usuario_Perfil.Select(up => up.Perfil);
            bool perfilTemPermissao = false;
            int i = 0;
            while (i < perfisUsuario.Count() && !perfilTemPermissao)
            {
                var perfil = perfisUsuario.ElementAt(i);
                perfilTemPermissao = perfil.T_PERFIL_OBJETO_CONTROLAVEL
                    .Any(poc => poc.OBJ_ID.Equals(class_namespace, StringComparison.OrdinalIgnoreCase));

                i++;
            }

            if (perfilTemPermissao)
                return true;

            return false;
        }


        public static bool ValidarOperacoesPorUsuario(T_Usuario UsuarioLogado, dynamic objeto, string namespaceClasse, IQueryable<Object> query)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<T_USUARIO_OBJETO_CONTROLAVEL> listaValidacao = getListaUsuariosObjetos(UsuarioLogado, namespaceClasse);
                //listaValidacao = db.T_USUARIO_OBJETO_CONTROLAVEL.Where(x => x.USE_ID == UsuarioLogado.USE_ID && x.OBJ_ID.Contains(namespaceClasse)).ToList();

                string[] tempClasse = namespaceClasse.Split("."); //Pegando o nome da classe.
                string nomeClasse = tempClasse[tempClasse.Length - 1];

                if (listaValidacao.Count > 0)
                {
                    string PlayAction = objeto.PlayAction.ToString().ToUpper();
                    if (PlayAction == "UPDATE")
                    {

                        #region Buscando o objeto original
                        //Este primeiro trecho de código busca o objeto original no banco de dados para poder comparar com o que foi modificado.
                        string[] pk = UtilPlay.GetPrimaryKey(namespaceClasse, db); //Pega todas as chaves primárias do objeto.
                        string clausula_where = " WHERE ";

                        for (int i = 0; i < pk.Length; i++)
                        {
                            string campo = pk[i].ToString().Split(".")[1]; //A chave vem assim: CALENDARIO.CAL_ID, por isso o split é necessário.
                            var valor = objeto.GetType().GetProperty(campo).GetValue(objeto); //Pega o valor do objeto original.

                            clausula_where += campo + " = '" + valor + "' "; //Monta a clausula where.
                            if (i + 1 < pk.Length)
                                clausula_where += "AND "; //Se for chave primária composta precisa por o AND.

                        }
                        //Esse trecho de código busca o nome da Tabela no banco de dados.
                        IEntityType entityType = db.Model.FindEntityType(namespaceClasse);
                        string tableName = entityType.Relational().TableName;
                        string consultaSQL = "SELECT * FROM " + tableName + clausula_where; //Monta a consulta SQL.


                        var objetoOriginal = query.AsNoTracking().FromSql(consultaSQL).FirstOrDefault(); //Faz a consulta SQL.
                        #endregion

                        #region Comparando objeto original com objeto modificado.
                        int cont_val = 0;
                        while (cont_val < listaValidacao.Count) //Vai percorrer todos os objeto da classe.
                        {

                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacao[cont_val].USO_ACAO = listaValidacao[cont_val].USO_ACAO != null && !listaValidacao[cont_val].USO_ACAO.Equals("") ?
                                listaValidacao[cont_val].USO_ACAO.ToUpper() : listaValidacao[cont_val].USO_ACAO = "CRUD";

                            if (!listaValidacao[cont_val].USO_ACAO.Contains("U")) //Se a ação do usuário USO_ACAO não conter U (Update), então o usuário não pode fazer alteração neste campo
                            {
                                string[] temp = listaValidacao[cont_val].OBJ_ID.Split(".");
                                string campo = temp[temp.Length - 1]; //Pega o nome do campo

                                if (campo == nomeClasse) //Se o campo na verdade for a própria classe, entra nessa condição.
                                {
                                    //Nesse caso, o usuário não pode sequer alterar nada na classe.
                                    return false;
                                }
                                else
                                {
                                    if (objeto.GetType().GetProperty(campo) == null) //Se trata de um método, então essa validação não tem sentido
                                        return true;

                                    var valorAtual = objeto.GetType().GetProperty(campo).GetValue(objeto); //Pega o valor atual no objeto modificado
                                    var valorOriginal = objetoOriginal.GetType().GetProperty(campo).GetValue(objetoOriginal); //Pega o valor no objeto original

                                    if (valorAtual.ToString() != valorOriginal.ToString()) //Faz a comparação, se for diferente, o usuário modificou o que não devia.
                                    {
                                        //O usuário foi pilantra e alterou um campo que não devia.
                                        return false;
                                    }
                                }
                            }
                            cont_val++;
                        }
                        #endregion
                    }
                    else if (PlayAction == "DELETE" || PlayAction == "INSERT")
                    {
                        //Caso o PlayActiom seja DELETE significa a listaValidacao precisa ter o OBJ_ID classe.
                        int cont_val = 0;
                        while (cont_val < listaValidacao.Count) //Vai percorrer todos os objeto da classe.
                        {
                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacao[cont_val].USO_ACAO = listaValidacao[cont_val].USO_ACAO != null && !listaValidacao[cont_val].USO_ACAO.Equals("") ?
                                listaValidacao[cont_val].USO_ACAO.ToUpper() : listaValidacao[cont_val].USO_ACAO = "CRUD";

                            if ((!listaValidacao[cont_val].USO_ACAO.Contains("D") && PlayAction == "DELETE") || (!listaValidacao[cont_val].USO_ACAO.Contains("C") && PlayAction == "INSERT")) //Se a ação do usuário USO_ACAO não conter U (Update), então o usuário não pode fazer alteração neste campo
                            {
                                string[] temp = listaValidacao[cont_val].OBJ_ID.Split(".");
                                string campo = temp[temp.Length - 1]; //Pega o nome do campo
                               

                                if (campo == nomeClasse) //Se o campo na verdade for a própria classe, entra nessa condição.
                                {
                                    //Nesse caso, o usuário não pode excluir caso o playaction seja Delete ou inserir nenhum registro caso o playaction seja Insert
                                    return false;
                                }
                                else if (objeto.GetType().GetProperty(campo) == null) //Se trata de um método, então essa validação não tem sentido
                                    return true;
                            }
                            cont_val++;
                        }
                    }

                }

            }
            return true;
        }

        public static List<T_USUARIO_OBJETO_CONTROLAVEL> getListaUsuariosObjetos(T_Usuario UsuarioLogado, string namespaceClasse)
        {
            List<T_USUARIO_OBJETO_CONTROLAVEL> listaValidacaoUse = new List<T_USUARIO_OBJETO_CONTROLAVEL>();
            foreach (T_USUARIO_OBJETO_CONTROLAVEL usuarioObjeto in UsuarioLogado.T_USUARIO_OBJETO_CONTROLAVEL)
            {
                if (usuarioObjeto.OBJ_ID.Contains(namespaceClasse))
                    listaValidacaoUse.Add(usuarioObjeto);
            }

            return listaValidacaoUse;
        }

        public static List<T_Perfil_Objeto_Controlavel> getListaPerfisObjetos(T_Usuario UsuarioLogado, string namespaceClasse)
        {
            List<T_Perfil_Objeto_Controlavel> listaValidacaoPer = new List<T_Perfil_Objeto_Controlavel>();
            foreach (T_Usuario_Perfil usuarioPerfil in UsuarioLogado.T_Usuario_Perfil)
            {
                foreach (var perfilObjeto in usuarioPerfil.Perfil.T_PERFIL_OBJETO_CONTROLAVEL)
                {
                    if (perfilObjeto.OBJ_ID.Contains(namespaceClasse))
                        listaValidacaoPer.Add(perfilObjeto);
                }
            }

            return listaValidacaoPer;
        }

        public static bool ValidarOperacoesPorPerfil(T_Usuario UsuarioLogado, dynamic objeto, string namespaceClasse, IQueryable<Object> query)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<T_Perfil_Objeto_Controlavel> listaValidacao = getListaPerfisObjetos(UsuarioLogado, namespaceClasse);
                List<T_USUARIO_OBJETO_CONTROLAVEL> listaValidacaoUse = getListaUsuariosObjetos(UsuarioLogado, namespaceClasse);

                //Remove todos os perfisObjetos que são iguais aos usuariosObjetos
                foreach (var item in listaValidacaoUse)
                {
                    int i = 0;
                    while (i < listaValidacao.Count && listaValidacao[i].OBJ_ID != item.OBJ_ID)
                    {
                        i++;
                    }

                    if (i < listaValidacao.Count)
                    {
                        listaValidacao.RemoveAt(i);
                    }
                }


                string[] tempClasse = namespaceClasse.Split("."); //Pegando o nome da classe.
                string nomeClasse = tempClasse[tempClasse.Length - 1];

                if (listaValidacao.Count > 0)
                {
                    string PlayAction = objeto.PlayAction.ToString().ToUpper();
                    if (PlayAction == "UPDATE")
                    {

                        #region Buscando o objeto original
                        //Este primeiro trecho de código busca o objeto original no banco de dados para poder comparar com o que foi modificado.
                        string[] pk = UtilPlay.GetPrimaryKey(namespaceClasse, db); //Pega todas as chaves primárias do objeto.
                        string clausula_where = " WHERE ";


                        for (int i = 0; i < pk.Length; i++)
                        {
                            string campo = pk[i].ToString().Split(".")[1]; //A chave vem assim: CALENDARIO.CAL_ID, por isso o split é necessário.
                            var valor = objeto.GetType().GetProperty(campo).GetValue(objeto); //Pega o valor do objeto original.

                            clausula_where += campo + " = '" + valor + "' "; //Monta a clausula where.
                            if (i + 1 < pk.Length)
                                clausula_where += "AND "; //Se for chave primária composta precisa por o AND.

                        }

                        //Esse trecho de código busca o nome da Tabela no banco de dados.
                        IEntityType entityType = db.Model.FindEntityType(namespaceClasse);
                        string tableName = entityType.Relational().TableName;
                        string consultaSQL = "SELECT * FROM " + tableName + clausula_where; //Monta a consulta SQL.


                        var objetoOriginal = query.AsNoTracking().FromSql(consultaSQL).FirstOrDefault(); //Faz a consulta SQL.
                        #endregion

                        #region Comparando objeto original com objeto modificado.
                        int cont_val = 0;
                        while (cont_val < listaValidacao.Count) //Vai percorrer todos os objeto da classe.
                        {

                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacao[cont_val].PEO_ACAO = listaValidacao[cont_val].PEO_ACAO != null && !listaValidacao[cont_val].PEO_ACAO.Equals("") ?
                                listaValidacao[cont_val].PEO_ACAO.ToUpper() : listaValidacao[cont_val].PEO_ACAO = "CRUD";

                            if (!listaValidacao[cont_val].PEO_ACAO.Contains("U")) //Se a ação do usuário USO_ACAO não conter U (Update), então o usuário não pode fazer alteração neste campo
                            {
                                string[] temp = listaValidacao[cont_val].OBJ_ID.Split(".");
                                string campo = temp[temp.Length - 1]; //Pega o nome do campo


                                if (campo == nomeClasse) //Se o campo na verdade for a própria classe, entra nessa condição.
                                {
                                    //Nesse caso, o usuário não pode sequer alterar nada na classe.
                                    return false;
                                }
                                else
                                {
                                    if (objeto.GetType().GetProperty(campo) == null) //Se trata de um método, então essa validação não tem sentido
                                        return true;


                                    var valorAtual = objeto.GetType().GetProperty(campo).GetValue(objeto); //Pega o valor atual no objeto modificado
                                    var valorOriginal = objetoOriginal.GetType().GetProperty(campo).GetValue(objetoOriginal); //Pega o valor no objeto original

                                    if (valorAtual.ToString() != valorOriginal.ToString()) //Faz a comparação, se for diferente, o usuário modificou o que não devia.
                                    {
                                        //O usuário foi pilantra e alterou um campo que não devia.
                                        return false;
                                    }
                                }
                            }
                            cont_val++;
                        }
                        #endregion
                    }
                    else if (PlayAction == "DELETE" || PlayAction == "INSERT")
                    {
                        //Caso o PlayActiom seja DELETE significa a listaValidacao precisa ter o OBJ_ID classe.
                        int cont_val = 0;
                        while (cont_val < listaValidacao.Count) //Vai percorrer todos os objeto da classe.
                        {
                            //Se a ação for vazia, então o usuário pode tudo (CRUD), se não recebe a própria ação do usário em upper case para evitar problema de validações
                            listaValidacao[cont_val].PEO_ACAO = listaValidacao[cont_val].PEO_ACAO != null && !listaValidacao[cont_val].PEO_ACAO.Equals("") ?
                                listaValidacao[cont_val].PEO_ACAO.ToUpper() : listaValidacao[cont_val].PEO_ACAO = "CRUD";

                            if ((!listaValidacao[cont_val].PEO_ACAO.Contains("D") && PlayAction == "DELETE") || (!listaValidacao[cont_val].PEO_ACAO.Contains("C") && PlayAction == "INSERT")) //Se a ação do usuário USO_ACAO não conter U (Update), então o usuário não pode fazer alteração neste campo
                            {
                                string[] temp = listaValidacao[cont_val].OBJ_ID.Split(".");
                                string campo = temp[temp.Length - 1]; //Pega o nome do campo


                                if (campo == nomeClasse) //Se o campo na verdade for a própria classe, entra nessa condição.
                                {
                                    //Nesse caso, o usuário não pode excluir caso o playaction seja Delete ou inserir nenhum registro caso o playaction seja Insert
                                    return false;
                                }
                                else if (objeto.GetType().GetProperty(campo) == null) //Se trata de um método, então essa validação não tem sentido
                                    return true;
                            }
                            cont_val++;
                        }
                    }

                }

            }
            return true;
        }

        public static bool validarMetodoUsuarioPerfil(string namespaceClasse, string nameMethod, T_Usuario UsuarioLogado)
        {
            List<T_USUARIO_OBJETO_CONTROLAVEL> listaValidacaoUse = getListaUsuariosObjetos(UsuarioLogado, namespaceClasse);
            string nameTotal = namespaceClasse + "." + nameMethod;
            int cont = 0;
            bool flag = false;

            //Faz a validação de usuário
            while (cont < listaValidacaoUse.Count)
            {
                if (listaValidacaoUse[cont].OBJ_ID == nameTotal)
                {
                    if (listaValidacaoUse[cont].USO_ACAO.Contains("N"))
                    {
                        return false;
                    }
                    flag = true; //Achou o usuárioObjeto referente a este método e ele pode entrar no método.
                }

                cont++;
            }

            if (!flag) //Quer dizer que o usuário não tem aquele objeto controlável, então vai tentar achar no perfil.
            {
                List<T_Perfil_Objeto_Controlavel> listaValidacaoPer = getListaPerfisObjetos(UsuarioLogado, namespaceClasse);
                cont = 0;
                while (cont < listaValidacaoPer.Count)
                {
                    if (listaValidacaoPer[cont].OBJ_ID == nameTotal && listaValidacaoPer[cont].PEO_ACAO.Contains("N"))
                        return false;

                    cont++;
                }
            }

            return true;
        }


        /// <summary>
        /// Verifica se o usuário, ou algum de seus perfis possui permissão para efetuar determinada operação na base de dados.
        /// </summary>
        /// <param name="usuario">Usuário logado no sistema.</param>
        /// <param name="playAction">Operação que será realizada na base de dados.</param>
        /// <param name="namespaceClasse">Namespace da classe.</param>
        /// <returns>True, se permitido. Do contrário False.</returns>
        public static bool ValidarOperacoesNaBaseDeDados(T_Usuario UsuarioLogado, dynamic objeto, string namespaceClasse, IQueryable<Object> query)
        {
            if (ValidarOperacoesPorUsuario(UsuarioLogado, objeto, namespaceClasse, query))
            {
                return ValidarOperacoesPorPerfil(UsuarioLogado, objeto, namespaceClasse, query);
            }
            return false;

        }

        /// <summary>
        /// Verifica a operação na base de dados correspondente ao CRUD.
        /// </summary>
        /// <param name="playAction">Operação que será realizada na base de dados.</param>
        /// <returns>Index que corresponde a operação CRUD que será realizada C(0) R(1) U(2) D(3)</returns>
        private static string ObterValorAcao(string playAction)
        {
            string retorno;

            /* insert => i
             * update => u
             * delete => d
             */

            switch (playAction.ToLower())
            {
                case "insert":
                    retorno = "i";
                    break;
                case "update":
                    retorno = "u";
                    break;
                case "delete":
                    retorno = "d";
                    break;
                default:
                    retorno = null;
                    break;
            }
            return retorno;
        }
    }
}
