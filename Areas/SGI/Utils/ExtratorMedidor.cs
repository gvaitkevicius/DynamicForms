using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using P.Pager;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Areas.SGI.Utils
{
    public static class ExtratorMedidor
    {
        public static T_Usuario T_Usuario { get; set; }

        /// <summary>
        /// Metódo responsavel por retornar agrupado os indicadores para serem utilizados na páginação
        /// </summary>
        /// <param name="nPageSize">Registros por página</param>
        /// <param name="page">Página a ser exibida</param>
        /// <param name="idNegocio">Código do negócio</param>
        /// <param name="pAno">Ano a ser filtrado</param>
        /// <returns>Irá retornar lista de indicadores com ano,id negocio e id meta</returns>
        public static IPager<MedicoesInd> GetIndicadores(int? nPageSize, int? page, int? idNegocio, int? idGrupo, int? idUnidade, int? idDepartamento, string pAno, string search, List<T_Favoritos> Favoritos)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int _PageNumber = page ?? 1;
                int _PageSize = nPageSize ?? 10;
                List<MedicoesInd> agrupado = new List<MedicoesInd>();
                //Agrupa dados por mês, Indicador e meta

                var dbIndicadores = db.VW_SGI_PARAMETRO_RELMEDICOES
                    .Select(x => new { Ano = x.Mes.Substring(0, 4), x.IND_DESCRICAO, x.IND_ID, x.NEG_ID, UNI_ID = x.UNID }).ToList();

                var indicadores = dbIndicadores.GroupBy(x => new { x.IND_ID, x.IND_DESCRICAO, x.NEG_ID, x.Ano, x.UNI_ID }).OrderByDescending(x => x.Key.Ano).ToList();
                #region Filtros

                //Filtra Ano
                if (pAno != "" && pAno != null)
                    indicadores = indicadores.Where(x => x.Key.Ano == pAno).ToList();
                //Filtra Negócio
                if (idNegocio > 0)
                    indicadores = indicadores.Where(x => x.Key.NEG_ID == idNegocio).ToList();

                //Filtra Grupo
                if (idGrupo > 0)
                {
                    List<T_Grupo_Indicador> grupos = db.T_Grupo_Indicador.Where(x => x.GRU_ID == idGrupo).ToList();
                    indicadores = indicadores.Where(x => grupos.Any(g => g.IND_ID == x.Key.IND_ID)).ToList();
                }

                //Filtra Departamento
                if (idDepartamento > 0)
                {
                    List<T_Indicadores_Departamentos> departamentos = db.T_Indicadores_Departamentos.Where(x => x.DEP_ID == idDepartamento).ToList();
                    indicadores = indicadores.Where(x => departamentos.Any(d => d.IND_ID == x.Key.IND_ID)).ToList();
                }

                //Filtra por unidade
                if (idUnidade > 0)
                    indicadores = indicadores.Where(x => x.Key.UNI_ID == idUnidade).ToList();

                //Filtra por pesquisa de usuário
                if (!string.IsNullOrEmpty(search))
                    indicadores = indicadores.Where(x => (x.Key.IND_DESCRICAO).ToUpper().Contains(search.ToUpper())).ToList();

                #endregion Filtros

                indicadores = indicadores.OrderBy(x => x.Key.IND_DESCRICAO).ToList();

                //Percorre registros recuperados
                foreach (var item in indicadores)
                {
                    if (agrupado.Count(x => x.IND_ID == item.Key.IND_ID) <= 0)
                    {
                        //busca detalhes do indicador
                        T_Indicadores indicador = db.T_Indicadores.Where(x => x.IND_ID == item.Key.IND_ID).FirstOrDefault();
                        //busca as dimensoes do indicador
                        indicador.Dimensoes = db.T_Medicoes
                            .Where(x => x.IND_ID == indicador.IND_ID && x.DIM_ID != null)
                            .Select(x => new Dimensao
                            {
                                Id = x.DIM_ID,
                                Descricao = x.DIM_DESCRICAO
                            }).Distinct().ToList();

                        //busca as sub dimensoes do indicador
                        var SubDimensoes = db.T_Medicoes
                            .Where(x => x.IND_ID == indicador.IND_ID && x.DIM_SUBDIMENSAO_ID != null)
                            .Select(x => new SubDimensao
                            {
                                Id = x.DIM_SUBDIMENSAO_ID,
                                Descricao = x.DIM_SUB_DESCRICAO
                            }).Distinct().ToList();

                        indicador.Dimensoes.ElementAt(0).SubDimensao = SubDimensoes;

                        var dimId = indicador.Dimensoes.ElementAt(0).Id;
                        //busca os periodos de um indicador baseado na primeira dimensão recuperada

                        var periodos = db.T_Medicoes
                            .Where(x => x.IND_ID == indicador.IND_ID
                                && x.DIM_ID != null && x.DIM_ID == dimId
                                && x.PER_ID != null && x.PER_ID.Trim().ToUpper() != "MAC" && x.PER_ID.Trim().ToUpper() != "DAC")
                            .Select(x => new Periodo { Id = x.PER_ID, Descricao = x.PER_DESCRICAO })
                            .Distinct().ToList();

                        //adciona os periodos na primeira posicao da lista de dimensoes do indicador
                        indicador.Dimensoes.ElementAt(0).Periodos = periodos;
                        agrupado.Add(new MedicoesInd()
                        {
                            ID_FAVORITO = (Favoritos.Count(x => x.ID_INDICADOR == item.Key.IND_ID) > 0 ? item.Key.IND_ID : 0),
                            IND_ID = item.Key.IND_ID,
                            Ano = item.Key.Ano,
                            Indicador = indicador,
                            DESC_CALCULO = indicador.DESC_CALCULO,
                            TIPO_COMPARADOR = indicador.IND_TIPOCOMPARADOR.ToString(),
                            IND_CONEXAO = indicador.IND_CONEXAO,
                            IND_GRAFICO = indicador.IND_GRAFICO,
                            DIM_ID = indicador.DIM_ID,
                            PER_ID = indicador.PER_ID
                        });
                    }
                }

                IPager<MedicoesInd> retorno = agrupado.OrderBy(x => x.Indicador.IND_DESCRICAO).OrderByDescending(x => x.ID_FAVORITO).ToPagerList(_PageNumber, _PageSize);
                return retorno;
            }
        }

        private static bool ValidarPerfil(T_Usuario t_usuario)
        {
            int i = 0;
            //Sexual Hands Krupck -> Dar uma olhada na consulta, nessa condição, etc.
            //1 é o perfil de administrador
            while (i < t_usuario.T_Usuario_Perfil.Count && t_usuario.T_Usuario_Perfil.ElementAt(i).PER_ID != 1)
            {
                i++;
            }

            if (i < t_usuario.T_Usuario_Perfil.Count)
                return true;


            return false;
        }

        /// <summary>
        /// Metódo responsavel por retornar agrupado os indicadores para serem utilizados na páginação
        /// </summary>
        /// <param name="nPageSize">Registros por página</param>
        /// <param name="page">Página a ser exibida</param>
        /// <param name="idNegocio">Código do negócio</param>
        /// <param name="pAno">Ano a ser filtrado</param>
        /// <returns>Irá retornar lista de indicadores com ano,id negocio e id meta</returns>
        public static IPager<MedicoesInd> GetIndicadores(string pesquisa, int? nPageSize, int? page, int? idNegocio, int? idGrupo, int? idUnidade, int? idDepartamento, string pAno)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int _PageNumber = page ?? 1;
                int _PageSize = nPageSize ?? 10;
                List<MedicoesInd> agrupado = new List<MedicoesInd>();
                //Agrupa dados por mês, Indicador e meta
                var dbIndicadoes = db.VW_SGI_PARAMETRO_RELMEDICOES.Select(x => new { Ano = x.Mes.Substring(0, 4), IND_ID = x.IND_ID, NEG_ID = x.NEG_ID, UNI_ID = x.UNID }).ToList();
                var indicadores = dbIndicadoes.GroupBy(x => new { x.IND_ID, x.NEG_ID, x.Ano, x.UNI_ID }).ToList();
                #region Filtros
                //Filtra grupos por usuário
                bool validar_perfil = ValidarPerfil(T_Usuario);
                if (validar_perfil)
                {
                    T_Usuario usuario = db.T_Usuario.First(x => x.USE_EMAIL == T_Usuario.USE_EMAIL);
                    List<T_USER_GRUPO> grupos = db.T_USER_GRUPO.Where(x => x.ID_USUARIO == usuario.USE_ID).ToList();
                    List<T_Grupo_Indicador> gruposLiberados = db.T_Grupo_Indicador.Where(x => grupos.Any(g => g.GRU_ID == x.GRU_ID)).ToList();
                    indicadores = indicadores.Where(x => gruposLiberados.Any(g => x.Key.IND_ID == g.IND_ID)).ToList();
                }
                //Filtra Ano
                if (pAno != "" && pAno != null)
                    indicadores = indicadores.Where(x => x.Key.Ano == pAno).ToList();
                //Filtra Negócio
                if (idNegocio > 0)
                    indicadores = indicadores.Where(x => x.Key.NEG_ID == idNegocio).ToList();
                //Filtra Grupo
                if (idGrupo > 0)
                {
                    List<T_Grupo_Indicador> grupos = db.T_Grupo_Indicador.Where(x => x.GRU_ID == idGrupo).ToList();
                    indicadores = indicadores.Where(x => grupos.Any(g => g.IND_ID == x.Key.IND_ID)).ToList();
                }
                //Filtra por unidade
                if (idUnidade > 0)
                    indicadores = indicadores.Where(x => x.Key.UNI_ID == idUnidade).ToList();
                //Filtra Departamento
                if (idDepartamento > 0)
                {
                    List<T_Indicadores_Departamentos> departamentos = db.T_Indicadores_Departamentos.Where(x => x.DEP_ID == idDepartamento).ToList();
                    indicadores = indicadores.Where(x => departamentos.Any(d => d.IND_ID == x.Key.IND_ID)).ToList();
                }
                #endregion Filtros
                //Percorre registros recuperados
                foreach (var item in indicadores)
                {
                    if (agrupado.Count(x => x.IND_ID == item.Key.IND_ID) <= 0)
                    {
                        agrupado.Add(new MedicoesInd()
                        {
                            IND_ID = item.Key.IND_ID,
                            Ano = item.Key.Ano,
                            Indicador = db.T_Indicadores.Find(item.Key.IND_ID)
                        });
                    }
                }
                return agrupado.OrderBy(x => x.MET_ID).ToPagerList(_PageNumber, _PageSize);
            }
        }

        /// <summary>
        /// Metódo para processar as medições filtrando por indicadores
        /// </summary>
        /// <param name="indicadores">Objeto com dados dos indicadores para serem filtrados.</param>
        /// <param name="pAno">Ano a ser filtrado</param>
        /// <returns></returns>
        public static List<vw_SGI_PARAMETRO_RELMEDICOES> GetMedicoes(List<MedicoesInd> indicadores, string pAno)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<vw_SGI_PARAMETRO_RELMEDICOES> medicoes = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                List<vw_SGI_PARAMETRO_RELMEDICOES> medicao = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                foreach (var item in indicadores)
                {
                    //Busca dados no banco filtrando por indicador e meta
                    medicao = db.VW_SGI_PARAMETRO_RELMEDICOES.AsNoTracking().Where(x => x.IND_ID == item.IND_ID).ToList();

                    //Appenda dados no objeto lista medições
                    if (pAno != "" && pAno != null)
                        medicao = medicao.Where(x => x.Mes.Substring(0, 4) == pAno).ToList();
                    medicoes.AddRange(medicao);
                }
                return medicoes;
            }
        }

        /// <summary>
        /// Metódo para processar as medições filtrando por indicadores
        /// </summary>
        /// <param name="indicadores">Objeto com dados dos indicadores para serem filtrados.</param>
        /// <param name="pAno">Ano a ser filtrado</param>
        /// <returns>Objeto do tipo T_Medicoes</returns>
        public static List<T_Medicoes> GetMedicao(List<MedicoesInd> indicadores, string pAno)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<T_Medicoes> medicoes = new List<T_Medicoes>();
                foreach (var item in indicadores)
                {
                    //Busca dados no banco filtrando por indicador e meta
                    List<T_Medicoes> medicao = db.T_Medicoes.Where(x => x.IND_ID == item.IND_ID && x.MET_ID == item.MET_ID).ToList();
                    //Appenda dados no objeto lista medições
                    if (pAno != "" && pAno != null)
                        medicao = medicao.Where(x => x.MED_DATAMEDICAO.Substring(0, 4) == pAno).ToList();
                    medicoes.AddRange(medicao);
                }
                return medicoes;
            }
        }
    }
}
