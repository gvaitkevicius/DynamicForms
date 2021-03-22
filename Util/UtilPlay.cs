using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using OptMiddleware;
using OptShered;
using OptTransport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DynamicForms.Util
{
    public static class UtilPlay
    {
        private static object listaPedidos;

        /// <summary>
        /// Verifica os horários de recebimento do cliente. Utilizado nos relatórios de Plano de Carga e de Cargas Consolidadas.
        /// </summary>
        /// <param name="clientes">clientes.</param>
        /// <returns>Lista de objetos contendo o Código do Cliente e uma string com os horários de recebimento.</returns>
        public static List<object> ObterHorariosRecebimentoDoCliente(List<Cliente> clientes)
        {
            List<object> horariosRecebimento = new List<object>();
            foreach (var cliente in clientes)
            {
                List<T_HORARIO_RECEBIMENTO> horariosRepetidos = new List<T_HORARIO_RECEBIMENTO>();
                List<T_HORARIO_RECEBIMENTO> diasRepetidos = new List<T_HORARIO_RECEBIMENTO>();

                List<T_HORARIO_RECEBIMENTO> horariosNaoRepetidos = new List<T_HORARIO_RECEBIMENTO>();

                T_HORARIO_RECEBIMENTO horarioAtual;
                T_HORARIO_RECEBIMENTO aux;
                for (int i = 0; i < cliente.HorariosRecebimentos.Count; i++)
                {
                    horarioAtual = cliente.HorariosRecebimentos.ElementAt(i);

                    aux = cliente.HorariosRecebimentos
                                .Where(x => x.HRE_DIA_DA_SEMANA != horarioAtual.HRE_DIA_DA_SEMANA &&
                                    x.HRE_HORA_INICIAL.TimeOfDay == horarioAtual.HRE_HORA_INICIAL.TimeOfDay &&
                                    x.HRE_HORA_FINAL.TimeOfDay == horarioAtual.HRE_HORA_FINAL.TimeOfDay).FirstOrDefault();
                    if (aux == null)
                    {// horarioAtual não se repete
                        horariosNaoRepetidos.Add(horarioAtual);
                    }
                    else
                    {// horarioAtual se repete
                        aux = horariosRepetidos.Where(x => x.HRE_HORA_INICIAL.TimeOfDay == horarioAtual.HRE_HORA_INICIAL.TimeOfDay &&
                                                    x.HRE_HORA_FINAL.TimeOfDay == horarioAtual.HRE_HORA_FINAL.TimeOfDay).FirstOrDefault();
                        if (aux == null)
                        {// Este horario não está gravado na lista de horários repetidos
                            horariosRepetidos.Add(new T_HORARIO_RECEBIMENTO
                            {
                                HRE_HORA_INICIAL = horarioAtual.HRE_HORA_INICIAL,
                                HRE_HORA_FINAL = horarioAtual.HRE_HORA_FINAL
                            });
                        }

                        diasRepetidos.Add(new T_HORARIO_RECEBIMENTO
                        {
                            HRE_DIA_DA_SEMANA = horarioAtual.HRE_DIA_DA_SEMANA,
                            HRE_HORA_INICIAL = horarioAtual.HRE_HORA_INICIAL,
                            HRE_HORA_FINAL = horarioAtual.HRE_HORA_FINAL
                        });
                    }
                }

                StringBuilder sbHorario = new StringBuilder();
                List<T_HORARIO_RECEBIMENTO> listHorarios;
                string diaSemana;
                foreach (var horario in horariosRepetidos)
                {
                    listHorarios = diasRepetidos
                                    .Where(x => x.HRE_HORA_INICIAL.TimeOfDay == horario.HRE_HORA_INICIAL.TimeOfDay &&
                                        x.HRE_HORA_FINAL.TimeOfDay == horario.HRE_HORA_FINAL.TimeOfDay).ToList();

                    for (int j = 0; j < listHorarios.Count; j++)
                    {
                        diaSemana = ObterDiaDaSemana(listHorarios[j].HRE_DIA_DA_SEMANA);

                        if (j < (listHorarios.Count - 1))
                        {// Não é o ultimo elemento
                            if (j == (listHorarios.Count - 2))
                                sbHorario.Append($"{diaSemana} e ");
                            else
                                sbHorario.Append($"{diaSemana}, ");
                        }
                        else
                        {// É ultimo elemento
                            sbHorario.Append($"{diaSemana} ");
                        }
                    }
                    sbHorario.Append($"({horario.HRE_HORA_INICIAL.TimeOfDay.ToString(@"hh\:mm")} - " +
                        $"{horario.HRE_HORA_FINAL.TimeOfDay.ToString(@"hh\:mm")}) | ");
                }

                for (int j = 0; j < horariosNaoRepetidos.Count; j++)
                {
                    diaSemana = ObterDiaDaSemana(horariosNaoRepetidos[j].HRE_DIA_DA_SEMANA);
                    sbHorario.Append($"{diaSemana} ");
                    if (j < (horariosNaoRepetidos.Count - 1))
                    {// Não é o ultimo elemento
                        sbHorario.Append($"({horariosNaoRepetidos[j].HRE_HORA_INICIAL.TimeOfDay.ToString(@"hh\:mm")} - " +
                            $"{horariosNaoRepetidos[j].HRE_HORA_FINAL.TimeOfDay.ToString(@"hh\:mm")}) | ");
                    }
                    else
                    {
                        sbHorario.Append($"({horariosNaoRepetidos[j].HRE_HORA_INICIAL.TimeOfDay.ToString(@"hh\:mm")} - " +
                            $"{horariosNaoRepetidos[j].HRE_HORA_FINAL.TimeOfDay.ToString(@"hh\:mm")})");
                    }
                }

                string strHorario = sbHorario.ToString();
                if (horariosNaoRepetidos.Count == 0)
                    strHorario = strHorario.Remove(strHorario.Length - 3);

                var objeto = new
                {
                    CodCliente = cliente.CLI_ID,
                    HorariosRecebimento = strHorario
                };

                horariosRecebimento.Add(objeto);
            }

            return horariosRecebimento;
        }

        /// <summary>
        /// Avalia o dia da semana recebido por parâmetros e retorna o dia abreviado.
        /// </summary>
        /// <param name="dia">dia da semana (de 1[dom] até 7[sab]).</param>
        /// <returns>dia da semana abreviado.</returns>
        public static string ObterDiaDaSemana(int dia)
        {
            string diaSemana;
            switch (dia)
            {
                case 1:
                    diaSemana = "dom";
                    break;
                case 2:
                    diaSemana = "seg";
                    break;
                case 3:
                    diaSemana = "ter";
                    break;
                case 4:
                    diaSemana = "qua";
                    break;
                case 5:
                    diaSemana = "qui";
                    break;
                case 6:
                    diaSemana = "sex";
                    break;
                case 7:
                    diaSemana = "sab";
                    break;
                default:
                    diaSemana = "";
                    break;

            }
            return diaSemana;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Para">Objeto que receberá os valores do obj2</param>
        /// <param name="De">Objeto que que será utilizado para atribuir os valores no obj1</param>
        public static object ConverterObjetos(object De, object Para)
        {
            PropertyInfo[] propertiesObj1 = Para.GetType().GetProperties();
            PropertyInfo[] propertiesObj2 = De.GetType().GetProperties();
            foreach (PropertyInfo property in propertiesObj1)
            {
                PropertyInfo prop = propertiesObj2.Where(p => p.Name == property.Name).FirstOrDefault();
                if (prop != null)// && property.PropertyType == prop.PropertyType) //QTD_TOTAL_PLANEJADO_E_EXPEDIDO
                {
                    object valuePropObj2 = prop.GetValue(De);
                    property.SetValue(Para, valuePropObj2);
                }
            }

            return Para;
        }

        public static DateTime ProximaDataProdutiva(bool ParaFrente, DateTime DTDe, DateTime DTAte, List<ItensCalendario> itensCalendario)
        {
            if (ParaFrente)
            {
                var calend = itensCalendario
                    .Where(c => c.ICA_TIPO == 1 && c.ICA_DATA_DE <= DTDe && c.ICA_DATA_ATE > DTAte).FirstOrDefault();
                if (calend == null)
                {
                    calend = itensCalendario
                        .Where(c => c.ICA_TIPO == 1 && c.ICA_DATA_DE > DTDe).OrderBy(c => c.ICA_DATA_DE).FirstOrDefault();
                    if (calend == null)
                    {
                        return DTDe;
                    }
                    return calend.ICA_DATA_DE;
                }
                else
                {
                    return DTDe;
                }
            }
            else
            {
                // retroagindo 
                double TempoTotal = (DTAte - DTDe).TotalSeconds;
                var calend = itensCalendario
                    .Where(c => c.ICA_TIPO == 1 && c.ICA_DATA_DE <= DTAte).OrderByDescending(x => x.ICA_DATA_ATE).Take(30).ToList();
                foreach (var c in calend)
                {
                    if (DTAte > c.ICA_DATA_ATE)
                    {
                        DTAte = c.ICA_DATA_ATE;
                    }
                    if (DTAte.AddSeconds(TempoTotal * -1) >= c.ICA_DATA_DE)
                    {// cabe nesta intervalo de calendario 
                        return DTAte.AddSeconds(TempoTotal * -1);
                    }
                    TempoTotal = Math.Abs(TempoTotal - Math.Abs((DTAte - c.ICA_DATA_DE).TotalSeconds));
                }
                return DTAte;
            }
        }

        public static void DefinirValoresPadroesSaida(object objeto)
        {
            Type type = Type.GetType(objeto.ToString());
            PropertyInfo[] properties = type.GetProperties();

            string tipo;
            dynamic value;
            foreach (PropertyInfo property in properties)
            {
                tipo = property.PropertyType.ToString();
                if (!tipo.StartsWith("System.Collections") && !tipo.StartsWith("DynamicForms") && !property.Name.Equals("IndexClone"))
                {
                    value = property.GetValue(objeto);
                    if (tipo.Contains("Int"))
                    {
                        if (value == null)
                            property.SetValue(objeto, 0);
                    }
                    else if (tipo.Contains("Double"))
                    {
                        if (value == null)
                            property.SetValue(objeto, new Double());
                    }
                    else if (tipo.Contains("Single")) // float
                    {
                        if (value == null)
                            property.SetValue(objeto, new Single());
                    }
                    else if (tipo.Contains("Decimal"))
                    {
                        if (value == null)
                            property.SetValue(objeto, new Decimal());
                    }
                    else if (tipo.Contains("String"))
                    {
                        if (value == null)
                            property.SetValue(objeto, "");
                    }
                    else if (tipo.Contains("DateTime"))
                    {
                        if (value == null || DateTime.Compare(value, new DateTime()) == 0)
                            property.SetValue(objeto, DateTime.Now);
                    }
                }

            }
        }

        public static void DefinirValoresPadroesEntrada(ref List<object> objects)
        {
            using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (object objeto in objects)
                {
                    dynamic dynamicObj = objeto;
                    if (dynamicObj.PlayAction.ToUpper() == "INSERT" ||
                        dynamicObj.PlayAction.ToUpper() == "UPDATE")
                    {
                        Type type = Type.GetType(objeto.ToString());
                        PropertyInfo[] properties = type.GetProperties();

                        IEntityType entityType = _context.Model.FindEntityType(type);

                        string tipo;
                        dynamic value;
                        foreach (PropertyInfo property in properties)
                        {
                            IProperty iProperty = entityType.FindProperty(property);
                            if (iProperty != null && iProperty.IsForeignKey()) // É chave estrangeira
                                continue;

                            if (property.Name == "IndexClone")
                                continue;

                            tipo = property.PropertyType.ToString();

                            if (tipo.StartsWith("System.Collections") || tipo.StartsWith("DynamicForms")) // É uma Collections ou uma classe do Projeto
                                continue;

                            value = property.GetValue(objeto);
                            if (tipo.Contains("Int"))
                            {
                                if (value == null)
                                    property.SetValue(objeto, 0);
                            }
                            else if (tipo.Contains("Double"))
                            {
                                if (value == null)
                                    property.SetValue(objeto, new Double());
                            }
                            else if (tipo.Contains("Single")) // float
                            {
                                if (value == null)
                                    property.SetValue(objeto, new Single());
                            }
                            else if (tipo.Contains("Decimal"))
                            {
                                if (value == null)
                                    property.SetValue(objeto, new Decimal());
                            }
                            else if (tipo.Contains("String"))
                            {
                                if (value == null)
                                    property.SetValue(objeto, "");
                                else
                                {
                                    if (type.Name == nameof(Consultas))
                                        property.SetValue(objeto, value.Replace("\"", "").ToUpper().Trim());
                                    else if (type.Name == nameof(T_AGENDA_SCHEDULE))
                                        property.SetValue(objeto, value.ToUpper().Trim());
                                    else
                                        property.SetValue(objeto, value.Replace("\"", "").Replace("'", "").ToUpper().Trim());
                                }

                            }
                            else if (tipo.Contains("DateTime"))
                            {
                                if (value == null || DateTime.Compare(value, new DateTime()) == 0)
                                    property.SetValue(objeto, DateTime.Now);
                            }
                        }
                    }
                }
            }
        }

        public static void VerificarValorForeignKey(ref List<object> objects)
        {
            using (JSgi _context = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (object objeto in objects)
                {
                    dynamic dynamicObj = objeto;
                    if (dynamicObj.PlayAction.ToUpper() == "INSERT" || dynamicObj.PlayAction.ToUpper() == "UPDATE")
                    {
                        Type type = Type.GetType(objeto.ToString());
                        IEntityType entityType = _context.Model.FindEntityType(type);
                        if (entityType != null)
                        {
                            List<IForeignKey> foreignkeys = entityType.GetForeignKeys().ToList();

                            int i = 0;
                            for (; i < foreignkeys.Count; i++)
                            {
                                if (foreignkeys[i].Properties.Count > 1) // É chave composta
                                    continue;

                                string nameForeignKey = foreignkeys[i].ToString().Split("{'", 2)[1].Split("'}", 2)[0];
                                PropertyInfo property = type.GetProperty(nameForeignKey);

                                string tipo = property.PropertyType.ToString();
                                dynamic value = property.GetValue(objeto);
                                if (tipo.Contains("Int"))
                                {
                                    if (value == 0)
                                        property.SetValue(objeto, null);
                                }
                                else if (tipo.Contains("String"))
                                {
                                    if (value == "")
                                        property.SetValue(objeto, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static string[] GetPrimaryKey(string nome_classe, JSgi context)
        {
            var entity = context.Model.FindEntityType(nome_classe);

            if (entity == null)
                return new string[] { };

            string pk = entity.FindPrimaryKey().ToString();
            pk = pk.Replace("Key:", "").Replace("PK", "").Replace(nome_classe + ".", "").Replace(" ", "");
            return pk.Split(",");
        }

        public static string GetSha1(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }

        /// <summary>
        /// Retorna uma lista de objetos convertida para o tipo da classe especificada
        /// </summary>
        /// <param name="objJSON">Lista de objetos no formato JSON</param>
        /// <param name="name_class">Nome da classe dos objetos da lista</param>
        /// <returns></returns>
        public static object ConvertJsonToListObjects(string objJSON, string name_class)
        {
            Type listType = typeof(List<>);
            Type type = Type.GetType(name_class);
            Type constructedListType = listType.MakeGenericType(type);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            object objects = JsonConvert.DeserializeObject(objJSON, constructedListType, settings);

            return objects;
        }


        public static object ConvertParamToObjects(string parametros)
        {
            string[] vetor = parametros.Split(";");


            object objects = vetor;

            return objects;
        }

        /// <summary>
        /// Sua princial funcao é gravar uma lista de objetos em um arquivo.
        /// Esta funcao foi criada com o propósito de verificacao de logs do UpdateData
        /// de uma maneira mais fácil durante algum tipo de debug.
        /// A lista de objetos pode sofrer qualquer tipo de alteracao dentro desta funcao 
        /// de modo que fique mais fácil mais de visualizar o que se deseja,
        /// ela tambem pode ser chamada de qualquer lugar do projeto para fins de testes. 
        /// </summary>
        /// <param name="objects">Lista de objetos que será persistida no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static void GravarArquivoJson(dynamic objects, string nomeArquivo)
        {
            #region Regiao personalizavel

            if (objects.Count < 1)
                return;

            string data = DateTime.Now.TimeOfDay.ToString();
            string newDate = data.Replace(".", "_").Replace(":", "");
            nomeArquivo = $"{newDate}-{nomeArquivo}";

            #endregion Regiao personalizavel

            JsonSerializer serializer = new JsonSerializer();
            //using (StreamWriter sw = new StreamWriter(@"C:\Users\intepp\Source\Workspaces\DynamicForm\DynamicForms\wwwroot\Reports\Logs\" + nomeArquivo))
            using (StreamWriter sw = new StreamWriter(@"D:\PlaySistemas\DynamicForms\DynamicForms\wwwroot\Reports\Logs\" + nomeArquivo))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, objects);
            }
        }

        public static object ConvertJsonToObject(string objJSON, string name_class)
        {
            Type type = Type.GetType(name_class);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            object objects = JsonConvert.DeserializeObject(objJSON, type, settings);

            return objects;
        }


        public static string getErro(Exception e)
        {
            string sLine = "";
            try
            {
                var i = (e.InnerException == null) ? "" : e.InnerException.Message;
                var s = (e.StackTrace == null) ? "" : e.StackTrace.ToString();
                var AlertasErros = e.Source;
                //sLine = e.StackTrace.Substring(e.StackTrace.IndexOf("line"));
                //sLine = sLine.Substring(0, sLine.IndexOf("at"));
                return e.Message + " - " + i + " - " + sLine + " - " + s + AlertasErros;
            }
            catch (Exception)
            {
                return e.Message;
            }
        }

        public static string getNameClass(string namespaceOfClass)
        {
            string[] vet = namespaceOfClass.Split(".");
            return vet[vet.Length - 1];
        }

        public static void SetProduzindo(string maquina, string produto, string order, int seqRep, int seqTran)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    int linhasAlteradas = 0;

                    linhasAlteradas = db.Database.ExecuteSqlCommand(
                        "UPDATE T_FILA_PRODUCAO SET FPR_PRODUZINDO = 0 WHERE ROT_MAQ_ID = @ROT_MAQ_ID AND FPR_PRODUZINDO = 1;",
                        new SqlParameter("@ROT_MAQ_ID", maquina)
                    );

                    linhasAlteradas = db.Database.ExecuteSqlCommand(
                        "UPDATE T_FILA_PRODUCAO SET FPR_PRODUZINDO = 1 " +
                        "WHERE ORD_ID = @ORD_ID AND ROT_PRO_ID = @ROT_PRO_ID AND ROT_MAQ_ID = @ROT_MAQ_ID AND " +
                        "ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO AND FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;",
                        new SqlParameter("@ORD_ID", order),
                        new SqlParameter("@ROT_PRO_ID", produto),
                        new SqlParameter("@ROT_MAQ_ID", maquina),
                        new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                        new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                    );

                    linhasAlteradas = db.Database.ExecuteSqlCommand(
                        "UPDATE T_MAQUINA SET FPR_ID_OP_PRODUZINDO = " +
                        "ISNULL((SELECT TOP 1 FPR_ID FROM T_FILA_PRODUCAO (NOLOCK) WHERE ROT_MAQ_ID = @ROT_MAQ_ID ORDER BY FPR_ORDEM_NA_FILA), 0) " +
                        "WHERE MAQ_ID = @ROT_MAQ_ID;",
                        new SqlParameter("@ROT_MAQ_ID", maquina)
                    );

                    linhasAlteradas = db.Database.ExecuteSqlCommand(
                        "UPDATE T_FILA_PRODUCAO SET FPR_ORDEM_NA_FILA = " +
                        "(select MIN(FPR_ORDEM_NA_FILA)-1 FROM V_FILA_PRODUCAO WHERE ROT_MAQ_ID = @ROT_MAQ_ID) " +
                        "WHERE ORD_ID = @ORD_ID AND ROT_PRO_ID = @ROT_PRO_ID AND ROT_MAQ_ID = @ROT_MAQ_ID AND " +
                        "ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO AND FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;",
                        new SqlParameter("@ORD_ID", order),
                        new SqlParameter("@ROT_PRO_ID", produto),
                        new SqlParameter("@ROT_MAQ_ID", maquina),
                        new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                        new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                    );

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao executar SQL:" + ex);
                }
            }
        }

        public static void SetPrimeiraProducao(string Maquina)
        {
            // DEFINI OPE PRODUZINDO CASO NAO ENCONTRE NENHUM PEDIDO {Produzindo}
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (db.ViewFilaProducao.Count(f => f.RotMaqId == Maquina && f.Produzindo == 1) < 1)
                {
                    var Op = db.ViewFilaProducao.Where(f => f.RotMaqId == Maquina).OrderBy(f => f.OrdemDaFila).Take(1).FirstOrDefault();
                    if (Op != null)
                    {
                        // PRA FICAR PADRAO ENTYTY BASTA ADICIONAR O OBJETO 
                        //db.Fila.Add(ocorrencia);
                        //db.SaveChanges();

                        string sql = $"UPDATE T_FILA_PRODUCAO SET FPR_PRODUZINDO = 1 " +
                                     $"WHERE ORD_ID = '{Op.OrdId}' AND ROT_PRO_ID = '{Op.PaProId}' AND " +
                                     $"ROT_MAQ_ID = '{Maquina}' AND ROT_SEQ_TRANFORMACAO = {Op.RotSeqTransformacao} AND " +
                                     $"FPR_SEQ_REPETICAO = {Op.FprSeqRepeticao}";
                        db.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
        }

        public static DateTime DiaTurmaD(JSgi db, string dataOriginal)
        {
            DateTime dataAux = DateTime.Parse(dataOriginal);
            double tempoAdicional = db.Param.Find("HORA_DIA_TURMA").PAR_VALOR_N;
            dataAux = dataAux.AddHours(tempoAdicional);
            return dataAux;
        }

        public static Boolean SendMensagem(JSgi db, string msgid, string send, string status, string tipo)
        {
            Boolean ret = true;
            try
            {
                Mensagem Men = null;
                Men = db.Mensagem.Find(msgid);
                if (Men == null)
                {
                    Mensagem m = new Mensagem();
                    m.MEN_ID = msgid;
                    m.MEN_EMISSION = DateTime.Now;
                    m.MEN_SEND = send;
                    m.MEN_TYPE = tipo;
                    m.MEN_RECEIVE = "";
                    m.MEN_STATUS = status;
                    m.MEN_DATE_TRY_SEND = DateTime.Now;
                    m.MEN_QTD_TRY_SEND = 0;
                    db.Mensagem.Add(m);
                }
                else
                {
                    db.Entry(Men).State = EntityState.Modified;
                    Men.MEN_EMISSION = DateTime.Now;
                    Men.MEN_SEND = send;
                    Men.MEN_TYPE = tipo;
                    Men.MEN_RECEIVE = "";
                    Men.MEN_STATUS = status;
                }
                db.SaveChanges();
                Men = null;

                if (tipo.ToUpper() == "ONLINE_PROCEDURE")
                {
                    db.Database.ExecuteSqlCommand(send);

                    var m = db.Mensagem.AsNoTracking()
                    .Where(f => f.MEN_ID == msgid)
                    .Take(1)
                    .Select(x => new
                    {
                        x.MEN_STATUS
                    }).FirstOrDefault();

                    // Men = db.Mensagens.Find(msgid);  pegou valor antigo 
                    if (m != null)
                    {
                        if (m.MEN_STATUS == "OK")// executado com sucesso 
                            ret = true;
                        else
                            ret = false;
                    }
                    else
                    {
                        ret = false;
                    }
                }

            }
            catch (Exception e)
            {
                return false;
                throw;
            }
            return ret;
        }

        public static string GetMensagem(JSgi db, string msgid)
        {
            Mensagem Men = db.Mensagem.AsNoTracking().Where(m => m.MEN_ID == msgid).FirstOrDefault();
            if (Men == null)
            {
                return "";
            }
            else
            {
                return Men.MEN_STATUS.Trim();
            }
        }

        /// <summary>
        /// Injeta o usuário nas classes que possuem esta propriedade.
        /// </summary>
        /// <param name="objects">Lista de Objetos</param>
        /// <param name="UsuarioLogado">Usuário (INSTANCIADO)</param>
        public static void InjetarUsuarioLogado(ref List<object> objects, T_Usuario UsuarioLogado)
        {
            foreach (object obj in objects)
            {
                Type type = Type.GetType(obj.ToString());
                PropertyInfo property = type.GetProperty(nameof(UsuarioLogado));
                if (property != null)
                {
                    property.SetValue(obj, UsuarioLogado);
                }
            }
        }


        public static bool SimularPedido(string pro_id, int quantidade, string cli_id, ref string status, ref DateTime prazoMinimo, ref string msgRetorno, ref DateTime data_fim, ref List<V_OPS_A_PLANEJAR> logs, ref List<object[]> embarque)
        {
            if (!(pro_id != null && data_fim != null && quantidade > 0))
            {
                status = "ERRO";
                msgRetorno = "Informe todos os campos: (pro id, data entrega e quantidade maior que zero).";
                //return Json(new { status, msgRetorno });
                return false;
            }

            DateTime data_inicio = DateTime.Now;
            List<V_OPS_A_PLANEJAR> v_ops_planejar = new List<V_OPS_A_PLANEJAR>();

            V_OPS_A_PLANEJAR v_op_aux;
            List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> roteiros;
            List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> roteirosComponentes;
            List<string> listaString = new List<string>();
            List<string> erros = new List<string>();
            Cliente cliente;

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                roteiros = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsNoTracking()
                    .Include(x => x.Maquina)
                    .Include(x => x.Maquina.Calendario)
                    .Include(x => x.Produto)
                    .OrderBy(x => x.ROT_SEQ_TRANFORMACAO)
                    .Where(x => x.PRO_ID == pro_id)
                    .ToList();

                cliente = db.Cliente.AsNoTracking().Where(c => c.CLI_ID == cli_id).FirstOrDefault();


                var lista = db.EstruturaProduto.AsNoTracking().Where(x => x.PRO_ID_PRODUTO == pro_id).Select(x => new { PRO_ID_COMPONENTE = x.PRO_ID_COMPONENTE }).ToList();
                foreach (var item in lista)
                {
                    listaString.Add(item.PRO_ID_COMPONENTE);
                }

                roteirosComponentes = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsNoTracking()
                    .Include(x => x.Maquina)
                    .Include(x => x.Maquina.Calendario)
                    .Include(x => x.Produto)
                    .OrderBy(x => x.ROT_SEQ_TRANFORMACAO)
                    .Where(x => listaString.Contains(x.PRO_ID))
                    .ToList();


                foreach (var roteiro in roteiros)
                {
                    roteirosComponentes.Add(roteiro);
                }
                roteiros = roteirosComponentes;
            }

            //Erros previstos
            #region Tratamento de erros previstos
            if (roteiros.Count == 0)
            {

                status = "ERRO";
                msgRetorno = "O produto não existe ou não passa por nenhuma máquina.";
                //return Json(new { status, msgRetorno });
                return false;
            }
            if (cliente == null)
            {
                status = "ERRO";
                msgRetorno = "O cliente não existe, verifique o valor digitado!";
                //return Json(new { status, msgRetorno });
                return false;
            }
            if (data_fim < DateTime.Now)
            {
                status = "ERRO";
                msgRetorno = "A data de necessidade de entrega não pode ser inferior a data atual.";
                //return Json(new { status, msgRetorno });
                return false;
            }
            #endregion

            DateTime data_aux_1970 = new DateTime(1970, 1, 1); //Datas auxiliares só para prencher a fila virtual;
            DateTime data_aux_2200 = new DateTime(2200, 1, 1); //Datas auxiliares só para prencher a fila virtual;
            foreach (var item in roteiros)
            {
                v_op_aux = new V_OPS_A_PLANEJAR();
                v_op_aux.OrderId = "S0000";// + index;
                v_op_aux.ProdutoId = item.PRO_ID;
                v_op_aux.SequenciaTransformacao = item.ROT_SEQ_TRANFORMACAO;
                v_op_aux.SequenciaRepeticao = 1;
                v_op_aux.FPR_PRIORIDADE = 0;
                v_op_aux.ORD_STATUS = "";
                v_op_aux.FPR_STATUS = "";
                v_op_aux.MaquinaId = item.MAQ_ID;
                v_op_aux.ORD_PRO_ID = item.PRO_ID;
                v_op_aux.DataInicioPrevista = data_inicio;
                v_op_aux.DataFimPrevista = data_fim;

                v_op_aux.DataFimMaxima = data_aux_1970;
                v_op_aux.PrevisaoMateriaPrima = data_aux_1970;
                v_op_aux.ObservacaoProducao = "";
                v_op_aux.QuantidadePrevista = quantidade;
                v_op_aux.Status = "";
                v_op_aux.Produzindo = 0;
                v_op_aux.IdIntegracao = "";
                v_op_aux.QuantidadeProduzida = 0;
                v_op_aux.QuantidadeRestante = 0;
                v_op_aux.TempoRestanteTotal = 0;

                v_op_aux.ORD_DATA_ENTREGA_DE = data_aux_2200;
                v_op_aux.ORD_DATA_ENTREGA_ATE = data_aux_2200;
                v_op_aux.CLI_TRANSLADO = 1;
                v_op_aux.TempoProducao = 0;
                v_op_aux.Performance = Convert.ToDouble(item.ROT_PERFORMANCE) > 0 ? Convert.ToDouble(item.ROT_PERFORMANCE) : 0;
                v_op_aux.TempoSetup = Convert.ToDouble(item.ROT_TEMPO_SETUP) > 0 ? Convert.ToDouble(item.ROT_TEMPO_SETUP) : 0;
                v_op_aux.TempoSetupAjuste = Convert.ToDouble(item.ROT_TEMPO_SETUP_AJUSTE) > 0 ? Convert.ToDouble(item.ROT_TEMPO_SETUP_AJUSTE) : 0;
                v_op_aux.PecasPorPulso = Convert.ToDouble(item.ROT_PECAS_POR_PULSO) > 0 ? Convert.ToDouble(item.ROT_PECAS_POR_PULSO) : 0;
                //v_op_aux.HIERARQUIA_SEQ_TRANSFORMACAO = Convert.ToDouble(item.ROT_HIERARQUIA_SEQ_TRANSFORMACAO) > 0 ? Convert.ToDouble(item.ROT_HIERARQUIA_SEQ_TRANSFORMACAO) : 0;
                //v_op_aux.AVALIA_CUSTO = Convert.ToInt32(item.ROT_AVALIA_CUSTO) > 0 ? Convert.ToInt32(item.ROT_AVALIA_CUSTO) : 0;
                v_op_aux.HIERARQUIA_SEQ_TRANSFORMACAO = 0;
                v_op_aux.AVALIA_CUSTO = 0;
                v_op_aux.Truncado = "";
                v_op_aux.DataInicioTrunc = data_aux_1970;
                v_op_aux.DataFimTrunc = data_aux_1970;
                v_op_aux.OrdemDaFila = 0;

                v_op_aux.Id = 8000;// + index; //Zuado
                v_op_aux.ORD_LOTE_PILOTO = 0;
                v_op_aux.FPR_INICIO_GRUPO_PRODUTIVO = data_aux_1970;
                v_op_aux.FPR_FIM_GRUPO_PRODUTIVO = data_aux_1970;
                v_op_aux.DataHoraNecessidadeInicioProducao = data_aux_1970;
                v_op_aux.DataHoraNecessidadeFimProducao = data_aux_1970;
                v_op_aux.TempoDecorridoSetup = 1;
                v_op_aux.TempoDecorridoSetupAjuste = 1;
                v_op_aux.TempoDecorridoPerformacace = 1;
                v_op_aux.QuantidadePerformace = 1;
                v_op_aux.QuantidadeSetup = 1;
                v_op_aux.TempoTeoricoPerformace = 1;
                v_op_aux.VelocidadeAtingirMeta = 1;
                v_op_aux.VeloAtuPcSegundo = 1;
                v_op_aux.PerformaceProjetada = 1;
                v_op_aux.TempoDecorridoPequenasParadas = 1;
                v_op_aux.AlocadaEmMaquina = 0;
                v_op_aux.FPR_GRUPO_PRODUTIVO = 0;

                v_op_aux.CAR_INICIO_JANELA_EMBARQUE = data_aux_2200;
                v_op_aux.CAR_FIM_JANELA_EMBARQUE = data_aux_2200;
                v_op_aux.EMBARQUE_ALVO = data_fim;//.AddDays(-1); // data_aux_2200;

                v_op_aux.CLI_EXIGENTE_NA_IMPRESSAO = -1;
                v_op_aux.FPR_COR_BICO1 = "";
                v_op_aux.FPR_COR_BICO2 = "";
                v_op_aux.FPR_COR_BICO3 = "";
                v_op_aux.FPR_COR_BICO4 = "";
                v_op_aux.FPR_COR_BICO5 = "";

                //v_op_aux.GRP_ID = item.GrupoMaquina != null ? item.GrupoMaquina.GMA_ID : "";
                v_op_aux.GRP_ID = "";
                v_op_aux.GRP_TIPO = item.GRP_TIPO;
                v_op_aux.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO = Convert.ToDouble(item.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO);
                v_op_aux.MAQ_LARGURA_UTIL = Convert.ToDouble(item.MAQ_LARGURA_UTIL);

                v_op_aux.GRP_PAP_ONDA = null;
                v_op_aux.ORD_TIPO = 1;
                v_op_aux.PrevisaoMateriaPrima = data_aux_1970;
                v_op_aux.FPR_ID_ORIGEM = 0;
                v_op_aux.FPR_DATA_ENTREGA = data_aux_1970;
                v_op_aux.EQU_ID = null;
                v_op_aux.PRO_COMPRIMENTO_PECA = item.Produto.PRO_COMPRIMENTO_PECA;
                v_op_aux.PRO_LARGURA_PECA = item.Produto.PRO_LARGURA_PECA;
                //v_op_aux.PRO_COMPRIMENTO_PECA = 0;
                //v_op_aux.PRO_LARGURA_PECA = 0;

                v_op_aux.Maquina = item.Maquina;

                v_ops_planejar.Add(v_op_aux);
                //index++;
            }

            data_inicio = calcularPreProducao(ref v_ops_planejar, data_inicio, data_fim);
            prazoMinimo = calcularDataProdutiva(ref v_ops_planejar, data_inicio, data_fim, ref erros, ref logs);

            ParametrosSingleton.LoadOPTSingleton();
            List<object[]> listaEmbarque = gerarOrderOpt(v_ops_planejar, quantidade, data_fim, cliente, ref erros);
            DateTime inicioJanelaEmbarque = DateTime.Now;
            DateTime fimJanelaEmbarque = DateTime.Now;
            DateTime embarqueAlvo = DateTime.Now;
            embarque = listaEmbarque;

            if (erros.Count > 0)
            {
                status = "ERRO";
                foreach (var erro in erros)
                {
                    msgRetorno += "\n" + erro;
                }
                msgRetorno += ".";
                //return Json(new { status, msgRetorno, logs });
                return false;
            }
            else
            {
                inicioJanelaEmbarque = Convert.ToDateTime(listaEmbarque[0][0]);
                fimJanelaEmbarque = Convert.ToDateTime(listaEmbarque[0][1]);
                embarqueAlvo = Convert.ToDateTime(listaEmbarque[0][2]);
            }

            if (fimJanelaEmbarque < data_fim)
            {
                status = "OK";
                msgRetorno = "É possível produzir e entregar para essa data.";
                //return Json(new { status, prazoMinimo, msgRetorno, data_fim, logs });
                return true;
            }
            else if (prazoMinimo < data_fim && fimJanelaEmbarque >= data_fim)
            {
                status = "ADIAR";
                msgRetorno = "É possível produzir para esta data, porém o embarque será concluído somente após a Data de entrega";
                return true;
            }
            else
            {
                status = "ADIAR";
                msgRetorno = "A data mínima para entrega é: " + prazoMinimo.Day + "/" + prazoMinimo.Month + "/" + prazoMinimo.Year + ".";
                //return Json(new { status, prazoMinimo, msgRetorno, data_fim, logs });
                return true;
            }

        }

        public static List<object[]> gerarOrderOpt(List<V_OPS_A_PLANEJAR> lista, int quantidadePlanejada, DateTime dataEntrega, Cliente cliente, ref List<string> erros)
        {
            OrderOpt produto = new OrderOpt();
            //Produto produto = new Produto();


            string sql = "select " +
                "PRO_PECAS_POR_FARDO,PRO_CAMADAS_POR_PALETE ,PRO_FARDOS_POR_CAMADA ,PON_DISTANCIA_KM, PRO_ID, P.TMP_TIPO_CARGA, " +
                "200000.0 / (P.PRO_PECAS_POR_FARDO * P.PRO_CAMADAS_POR_PALETE * P.PRO_FARDOS_POR_CAMADA)   VIR_QTD_SALDO_UE ," +
                "CASE WHEN PRO_TEMPO_CARREGAMENTO_UNITARIO > 0 THEN PRO_TEMPO_CARREGAMENTO_UNITARIO ELSE " +
                "ISNULL(CAR.TMP_TEMPO_MEDIO_UNITARIO,0) END PRO_TEMPO_CARREGAMENTO_UNITARIO ," +
                "" +
                "CASE WHEN PRO_TEMPO_DESCARREGAMENTO_UNITARIO > 0 THEN PRO_TEMPO_CARREGAMENTO_UNITARIO ELSE " +
                "CASE WHEN CLI_TEMPO_DESCARREGAMENTO_UNITARIO > 0 THEN CLI_TEMPO_DESCARREGAMENTO_UNITARIO ELSE " +
                "ISNULL(D.TMP_TEMPO_MEDIO_UNITARIO,0) END " +
                "END PRO_TEMPO_DESCARREGAMENTO_UNITARIO ," +
                "" +
                "CASE WHEN PRO_PERCENTUAL_JANELA_EMBARQUE > 0 THEN PRO_PERCENTUAL_JANELA_EMBARQUE ELSE " +
                "CASE WHEN CLI_PERCENTUAL_JANELA_EMBARQUE > 0 THEN CLI_PERCENTUAL_JANELA_EMBARQUE ELSE ISNULL(PER.TMP_TEMPO_MEDIO_UNITARIO,0) END " +
                "END PRO_PERCENTUAL_JANELA_EMBARQUE " +
                "" +
                "from T_PRODUTOS P(NOLOCK) " +
                "INNER JOIN T_GRUPO_PRODUTO GP(NOLOCK) ON GP.GRP_ID = P.GRP_ID " +
                "LEFT JOIN T_CLIENTES C(NOLOCK) ON C.CLI_ID = '" + cliente.CLI_ID + "' " +
                "LEFT JOIN T_PONTOS_MAPA M ON M.PON_ID = CASE WHEN CLI_REGIAO_ENTREGA = '' OR CLI_REGIAO_ENTREGA IS NULL THEN MUN_ID_ENTREGA ELSE CLI_REGIAO_ENTREGA END " +
                "LEFT JOIN T_TEMPOS_LOGISTICA D (NOLOCK) on D.TMP_TIPO_CARGA = P.TMP_TIPO_CARGA  AND D.TMP_TIPO_TEMPO = 'DESCARREGAMENTO' " +
                "LEFT JOIN T_TEMPOS_LOGISTICA CAR (NOLOCK) on CAR.TMP_TIPO_CARGA = P.TMP_TIPO_CARGA  AND CAR.TMP_TIPO_TEMPO = 'CARREGAMENTO' " +
                "LEFT JOIN T_TEMPOS_LOGISTICA E (NOLOCK) on E.TMP_TIPO_CARGA = ''  AND E.TMP_TIPO_TEMPO = 'TEMPO_ESPERA_CLIENTE' " +
                "LEFT JOIN T_TEMPOS_LOGISTICA PER (NOLOCK) on PER.TMP_TIPO_CARGA = ''  AND PER.TMP_TIPO_TEMPO = 'PERCENTUAL_JANELA_EMBARQUE' WHERE PRO_ID = '" + lista[lista.Count - 1].ProdutoId + "'";


            List<object[]> listaOrder = new List<object[]>();
            object[] temp_obj;

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {

                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;

                        db.Database.OpenConnection();

                        using (DbDataReader result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                temp_obj = new object[10];
                                for (int i = 0; i < 10; i++)
                                {
                                    var valor = result.GetValue(i);
                                    temp_obj[i] = valor;
                                }
                                listaOrder.Add(temp_obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        erros.Add(ex.Message);
                    }

                }

            }

            List<object[]> saida = new List<object[]>();
            if (erros.Count == 0)
            {
                double TemposInternos = 0;
                List<string> logsCalculaEmbarque = new List<string>();
                shePedidosParaExpedicao PE = new shePedidosParaExpedicao();
                sheOrderOpt order = new sheOrderOpt();
                sheRota Rot = new sheRota();

                order.CAR_ID = "S00000";
                order.PRO_PECAS_POR_FARDO = Convert.ToDouble(listaOrder[0][0]);
                order.PRO_CAMADAS_POR_PALETE = Convert.ToDouble(listaOrder[0][1]);
                order.PRO_FARDOS_POR_CAMADA = Convert.ToDouble(listaOrder[0][2]);
                order.PON_DISTANCIA_KM_MUN = Convert.ToDouble(listaOrder[0][3]);
                order.ProdutoId = listaOrder[0][4].ToString();
                order.TMP_TIPO_CARGA = listaOrder[0][5].ToString();
                order.VIR_QTD_SALDO_UE = Convert.ToDouble(listaOrder[0][6]);
                order.PRO_TEMPO_CARREGAMENTO_UNITARIO = Convert.ToDouble(listaOrder[0][7]);
                order.PRO_TEMPO_DESCARREGAMENTO_UNITARIO = Convert.ToDouble(listaOrder[0][8]);
                order.PRO_PERCENTUAL_JANELA_EMBARQUE = Convert.ToDouble(listaOrder[0][9]);
                order.InicioJanelaEmbarque = DateTime.Now;
                order.FimJanelaEmbarque = DateTime.Now;
                order.EmbarqueAlvo = DateTime.Now;
                order.ClienteId = cliente.CLI_ID;
                order.MUN_ID = cliente.MUN_ID;
                order.DataEntregaDe = dataEntrega;
                order.DataEntregaAte = dataEntrega;

                PE.AddOrdens(order);

                Rot.Distancia = order.PON_DISTANCIA_KM_MUN;
                PE.RotasFactiveis.Add(Rot);
                PE.EntregaDe = order.DataEntregaDe;
                PE.EntregaAte = order.DataEntregaAte;


                DateTime InicioJanelaEmbarque = DateTime.Now;
                DateTime FimJanelaEmbarque = DateTime.Now;
                DateTime EmbarqueAlvo = DateTime.Now;
                OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport.calculaJanelaEmbarque(PE, null, null, PE.RotasFactiveis.ElementAt(0), -1, ref EmbarqueAlvo, ref InicioJanelaEmbarque, ref FimJanelaEmbarque, ref TemposInternos, ref logsCalculaEmbarque);


                object[] temp = new object[3];
                temp[0] = InicioJanelaEmbarque;
                temp[1] = FimJanelaEmbarque;
                temp[2] = EmbarqueAlvo;
                saida.Add(temp);
            }
            return saida;
        }

        public static DateTime calcularPreProducao(ref List<V_OPS_A_PLANEJAR> v_ops_planejar, DateTime data_inicio, DateTime data_fim)
        {
            //Faz calculos para definir um inicio baseado em cada empresa;
            //Na cartrom são até dois dias antes da entrega;
            //Na Paraíbuna são 7 dias para produzir a matéria prima.

            return data_inicio;
        }

        public static double pegarFatorMaquina(List<CargaMaquina> listaMAquina, double tempoOciosoSec)
        {
            //Esse método indica a quantidade de tempo em porcentagem da máquina que pode ficar ociosa;

            //A máquina será a mesma, mas pode ter vários itens por conta das datas;
            //A mesma máquina em um ou mais dias diferentes;

            double fator = 1; //100% //Valor fixo por enquanto, futuramente vai ser modificado 
            return tempoOciosoSec * fator;
        }


        public static DateTime calcularDataProdutiva(ref List<V_OPS_A_PLANEJAR> fila, DateTime dataInicio, DateTime dataFim, ref List<string> erros, ref List<V_OPS_A_PLANEJAR> logs)
        {
            DateTime auxDataInicio = dataInicio;
            double tempoProducao = 0;
            bool flagProximo = false;
            bool buscarNoBanco = true;
            bool adiarTodos = false;
            int cal_id = 1;
            DateTime dataInicioCM = new DateTime(dataInicio.Year, dataInicio.Month, dataInicio.Day);
            DateTime dataFimCM = new DateTime(dataFim.Year, dataFim.Month, dataFim.Day, 23, 59, 59).AddDays(30);
            List<CargaMaquina> cargaMaquina = new List<CargaMaquina>();
            int contador_break = 0;

            for (int i = 0; i < fila.Count; i++)
            {

                if (!adiarTodos) //Variável de controlle que indica se todos os roteiros em questão precisaram serem adiados, se precisaram, nem executa os comandos nesse trecho, pois não será necessário.
                {
                    cal_id = 1;
                    if (fila[i].Maquina != null)
                    {
                        if (fila[i].Maquina.CAL_ID != null)
                        {
                            cal_id = (int)fila[i].Maquina.CAL_ID;
                        }


                    }
                    if (fila[i].Performance <= 0)
                    {
                        //Erros("ERRO Performance esta igual e zero. Verifique Roteiro." + l.ProdutoId);
                        String aux_erro = "PRODUTO NA MÁQUINA " + fila[i].MaquinaId + " COM PERFORMANCE 0";
                        erros.Add(aux_erro);

                        tempoProducao = 0;
                    }
                    if (fila[i].Performance > 0)
                    {
                        if (fila[i].GRP_TIPO == 2) //Onduladeira
                        {
                            tempoProducao = Convert.ToDouble(((((fila[i].QuantidadePrevista * (fila[i].PRO_LARGURA_PECA / 1000.0 * fila[i].PRO_COMPRIMENTO_PECA / 1000.0)) / fila[i].MAQ_LARGURA_UTIL) / fila[i].GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO) + fila[i].TempoSetup + fila[i].TempoSetupAjuste));
                        }
                        else
                        {

                            tempoProducao = ((fila[i].QuantidadePrevista / (fila[i].PecasPorPulso * fila[i].Performance)) + fila[i].TempoSetup + fila[i].TempoSetupAjuste);
                        }
                    }

                    if (fila[i].MaquinaId == "SRV_COMPRAS_01")
                    {
                        String aux_erro = "PRODUTO REFERENTE AO SERVIÇO DE COMPRAS";
                        erros.Add(aux_erro);

                        tempoProducao = 0;
                        return dataInicio;
                    }
                    fila[i].TempoProducao = tempoProducao;


                    #region Parte inicio e fim previsto
                    //Trecho que calcula o fim previsto considerando os calendários;
                    fila[i].DataHoraNecessidadeInicioProducao = auxDataInicio;
                    fila[i].DataHoraNecessidadeInicioProducao = ProximaDataProdutiva(true, auxDataInicio, auxDataInicio, cal_id, 1);
                    fila[i].DataHoraNecessidadeFimProducao = ProximaDataProdutiva(true,
                            fila[i].DataHoraNecessidadeInicioProducao,
                            fila[i].DataHoraNecessidadeInicioProducao.AddSeconds(tempoProducao), cal_id, tempoProducao);

                    //Trecho que pega o inicio previsto e o fim previsto sem considerar o horário;
                    DateTime auxData = fila[i].DataHoraNecessidadeInicioProducao;
                    fila[i].DataInicioPrevista = new DateTime(auxData.Year, auxData.Month, auxData.Day);
                    auxData = fila[i].DataHoraNecessidadeFimProducao;
                    fila[i].DataFimPrevista = new DateTime(auxData.Year, auxData.Month, auxData.Day, 23, 59, 59);
                    #endregion
                }

                //Trecho que encontra horário livre no cargaMaquina
                #region Parte de encontrar horário livre no cargaMaquina
                flagProximo = false;

                while (!flagProximo)
                {
                    if (buscarNoBanco)
                    {
                        using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                        {
                            cargaMaquina = db.CargaMaquina.AsNoTracking()
                                .Where(
                                    x => x.MED_DATA >= dataInicioCM &&
                                    x.MED_DATA <= dataFimCM)
                                .ToList();
                        }
                        buscarNoBanco = false;
                    }

                    DateTime auxDtInicio = fila[i].DataInicioPrevista;
                    DateTime auxDtFim = fila[i].DataFimPrevista;
                    String maqId = fila[i].MaquinaId;

                    #region Pegando a máquina responsável
                    var maquina = cargaMaquina
                        .Where(x =>
                        x.MED_DATA >= auxDtInicio &&
                        x.MED_DATA < auxDtFim &&
                        x.MAQ_ID == maqId
                    ).ToList();

                    if (maquina.Count == 0) //Provavelmente a máquina está associada a uma equipe
                    {
                        T_MAQUINAS_EQUIPES maquinaEquipe;
                        using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                        {
                            maquinaEquipe = db.T_MAQUINAS_EQUIPES.AsNoTracking().Where(x => x.MAQ_ID == maqId).FirstOrDefault();
                        }

                        if (maquinaEquipe != null)
                        {
                            maquina = cargaMaquina.Where(x =>
                                    x.MED_DATA >= auxDtInicio &&
                                    x.MED_DATA < auxDtFim && x.DIM_ID == maquinaEquipe.EQU_ID)
                                .ToList();
                        }
                    }
                    #endregion
                    /*if (maquina.Count == 0)
                    {
                        string erro = "Não existe CargaMaquina ou MaquinaEquipe associado para esta máquina.";
                        erros.Add(erro);
                        return dataInicio;
                    }*/

                    contador_break++;
                    if (contador_break > 1000)
                    {
                        string erro = "Limite máximo de iterações! Há algum erro nos tempos das máquinas ou no algoritmo";
                        erros.Add(erro);
                        return dataInicio;
                    }

                    double tempoOciosoSec = Math.Abs(maquina.Sum(x => x.DISPONIVEL).Value);
                    //double tempoOciosoSec = Math.Abs(maquina.Sum(x => x.OCIOSO).Value + maquina.Sum(x => x.ADIANTADO).Value); //Forma antiga
                    tempoOciosoSec = tempoOciosoSec * 3600; //Convertendo horas em segundos 60(min) * 60(sec) = 3600;
                    tempoOciosoSec = pegarFatorMaquina(maquina, tempoOciosoSec); //Esse método indica a porcentagem da máquina que pode ficar ociosa;
                    if (fila[i].TempoProducao <= tempoOciosoSec)
                    {
                        //continua
                        fila[i].StatusCargaMaquina = true;
                        flagProximo = true;
                        adiarTodos = false;
                        auxDataInicio = fila[i].DataHoraNecessidadeFimProducao;
                        //fila[i].TempoTotal = tempoOciosoSec + fila[i]
                        logs.Add(fila[i]);

                        int x = i;
                        while (i < fila.Count && fila[x].SequenciaTransformacao == fila[i].SequenciaTransformacao)
                        {
                            i++;
                        }
                        i--;
                    }
                    else
                    {
                        //A máquina não vai conseguir produzir nessa data;
                        fila[i].StatusCargaMaquina = false;

                        int w = i + 1;
                        if (w < fila.Count && fila[i].SequenciaTransformacao == fila[w].SequenciaTransformacao)
                        {
                            //A próxima sequencia é a mesma e tem chance de dar certo
                            flagProximo = true;
                        }
                        else
                        {
                            //Iteração para ver se existem roteiros anteriores iguais ao atual que possam produzir sem a necessidade de adiar;
                            w = i - 1;
                            while (w >= 0 && fila[w].SequenciaTransformacao == fila[i].SequenciaTransformacao && fila[w].StatusCargaMaquina == false)
                            {
                                w--;
                            }

                            if (w >= 0 && fila[w].SequenciaTransformacao == fila[i].SequenciaTransformacao && fila[w].StatusCargaMaquina == true)
                            {
                                //Existe roteiros iguais anteriores e que podem produzir essa sequencia de transformação
                                flagProximo = true;
                            }
                            else
                            {
                                flagProximo = false;
                                //Realmente vai ter que adiar o dia de produção do roteiro em questão;
                                if (fila[i].DataFimPrevista >= dataFimCM)
                                {
                                    dataFimCM = dataFimCM.AddDays(30);
                                    buscarNoBanco = true;
                                }
                                else
                                {
                                    fila[i].DataHoraNecessidadeFimProducao = fila[i].DataHoraNecessidadeFimProducao.AddDays(1);
                                    fila[i].DataFimPrevista = fila[i].DataFimPrevista.AddDays(1);
                                }

                                //Essa parte vai voltando para todos os roteiros que não deram certo e vai adiando todos eles;
                                //Caso exista N>1 roteiros iguais e que também não deram certo;
                                w = i - 1;
                                while (w >= 0 && fila[i].SequenciaTransformacao == fila[w].SequenciaTransformacao)
                                {
                                    //Realmente vai ter que adiar o dia de produção do roteiro em questão;
                                    if (fila[w].DataFimPrevista >= dataFimCM)
                                    {
                                        dataFimCM = dataFimCM.AddDays(30);
                                        buscarNoBanco = true;
                                    }
                                    else
                                    {
                                        fila[w].DataHoraNecessidadeFimProducao = fila[w].DataHoraNecessidadeFimProducao.AddDays(1);
                                        fila[w].DataFimPrevista = fila[w].DataFimPrevista.AddDays(1);
                                    }

                                    adiarTodos = true;


                                    w--;
                                }
                                i = w + 1;
                                auxDataInicio = fila[i].DataHoraNecessidadeFimProducao;
                            }
                        }
                    }
                }



                #endregion
            }

            return auxDataInicio;
        }

        public static GrupoProdutivo retroageGrupoProdutivo(DateTime DT, int NumeroDeGrupos)
        {
            using (var pi = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //int grp = pi.GrupoProdutivo.Where(x => x.Inicio <= DT && x.Fim > DT).FirstOrDefault().Index - NumeroDeGrupos;
                //return pi.GrupoProdutivo.ElementAt(grp);
                /*
                int grp = pi.GruposProdutivosExpedicao.Where(x => x.Inicio <= DT && x.Fim > DT).FirstOrDefault().Index - NumeroDeGrupos;
                return pi.GruposProdutivosExpedicao.ElementAt(grp);
                */

                return new GrupoProdutivo(DT, DT);
            }
        }

        public static DateTime ProximaDataProdutiva(bool ParaFrente, DateTime DTDe, DateTime DTAte, int cal_id, double TempoProducao)
        {
            //Essa função calcula o tempo de produção dos pedidos considerando os calendários de cada máquina;

            using (var pi = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (ParaFrente)
                {
                    //int cal_id = 1; //Aux
                    DateTime auxDTDe = new DateTime(DTDe.Year, DTDe.Month, DTDe.Day);
                    DateTime auxDataInicial = DTDe.AddSeconds(TempoProducao);
                    int addDays = 10;

                    var calend = pi.ItensCalendario.Where
                    //(c => IDsCalendarios.Contains(c.CAL_ID) && c.ICA_TIPO == 1 && c.ICA_DATA_DE >= auxDTDe && c.ICA_DATA_ATE <= DTAte.AddDays(addDays)).OrderBy(x => x.ICA_DATA_ATE).Take(30).ToList();
                    (c => c.CAL_ID == cal_id && c.ICA_TIPO == 1 && c.ICA_DATA_DE >= auxDTDe && c.ICA_DATA_ATE <= DTAte.AddDays(addDays)).OrderBy(x => x.ICA_DATA_ATE).Take(30).ToList();

                    int i = 0;
                    ItensCalendario ic;
                    while (TempoProducao > 0 && i < calend.Count)
                    {
                        ic = calend[i];
                        if (DTDe >= ic.ICA_DATA_DE && DTDe <= ic.ICA_DATA_ATE)
                        {
                            DTDe = DTDe.AddSeconds(TempoProducao);
                            if (DTDe > ic.ICA_DATA_ATE)
                            {
                                TempoProducao = (DTDe - ic.ICA_DATA_ATE).TotalSeconds;
                                i++;
                                while (i >= calend.Count)
                                {
                                    if (addDays > 100)
                                        return auxDataInicial;

                                    addDays *= 2;
                                    calend = pi.ItensCalendario.Where
                                        (c => c.CAL_ID == cal_id && c.ICA_TIPO == 1 && c.ICA_DATA_DE >= auxDTDe && c.ICA_DATA_ATE <= DTAte.AddDays(addDays)).OrderBy(x => x.ICA_DATA_ATE).Take(30).ToList();
                                }
                                DTDe = calend[i].ICA_DATA_DE;
                            }
                            else
                                TempoProducao = 0;
                        }
                        else if (DTDe <= ic.ICA_DATA_DE)
                        {
                            DTDe = ic.ICA_DATA_DE;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    return DTDe;
                }
                else
                {// retroagindo 

                    double TempoTotal = (DTAte - DTDe).TotalSeconds;
                    var calend = pi.ItensCalendario.Where
                    (c => c.CAL_ID == cal_id && c.ICA_TIPO == 1 && c.ICA_DATA_DE <= DTAte).OrderByDescending(x => x.ICA_DATA_ATE).Take(30).ToList();
                    foreach (var c in calend)
                    {
                        if (DTAte > c.ICA_DATA_ATE)
                        {
                            DTAte = c.ICA_DATA_ATE;
                        }
                        if (DTAte.AddSeconds(TempoTotal * -1) >= c.ICA_DATA_DE)
                        {// cabe nesta intervalo de calendario 
                            return DTAte.AddSeconds(TempoTotal * -1);
                        }
                        TempoTotal = Math.Abs(TempoTotal - Math.Abs((DTAte - c.ICA_DATA_DE).TotalSeconds));
                    }
                    return DTAte;
                }
            }
        }

        public static DateTime ConvertStringToDate(string str_data)
        {
            string[] temp = str_data.Split("-");
            DateTime data = new DateTime(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));

            return data;
        }

        public static bool IncluirPedidoFila(string cli_id, string mun_id, string ord_status, string ord_id, string pro_id, int quantidade, List<V_OPS_A_PLANEJAR> v_ops_a_planejar, DateTime data, ref string status, ref string msgRetorno)
        {
            //DateTime data = ConvertStringDate(data_entrega); //Converte a string em datetime;
            //List<V_OPS_A_PLANEJAR> v_ops_a_planejar = JsonConvert.DeserializeObject<List<V_OPS_A_PLANEJAR>>(fila); //Converte a string no formato List<V_OPS_A_PLANEJAR>
            //string ord_id = pegarOrdIdCM(); //Pega o próximo ORD_ID para inserir na fila; //Edit 1: Como o ID já vem por parâmetro essa função se tornou desnecessária
            Order order = preencherPedidosCM(ord_id, cli_id, mun_id, pro_id, quantidade, data, ord_status); //Preenche os dados do t_order;

            DateTime dtIni = v_ops_a_planejar[0].DataInicioPrevista;
            DateTime dtFim = v_ops_a_planejar[v_ops_a_planejar.Count - 1].DataFimPrevista;

            List<T_Medicoes> medicoes;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                medicoes = db.T_Medicoes.AsNoTracking().Where(x => dtIni >= x.MED_DATA && x.MED_DATA <= dtFim).ToList();
            }

            List<T_Medicoes> listaMed = new List<T_Medicoes>();
            T_Medicoes medTemp;

            for (int i = 0; i < v_ops_a_planejar.Count; i++)
            {
                int cont_dias = Convert.ToInt32((v_ops_a_planejar[i].DataFimPrevista - v_ops_a_planejar[i].DataInicioPrevista).TotalDays);
                DateTime tempDtIni = v_ops_a_planejar[i].DataInicioPrevista;
                DateTime tempDtFim = v_ops_a_planejar[i].DataFimPrevista;
                double somatorioTempo = 0;
                int auxCont = 0;
                while (cont_dias > 1)
                {
                    /*  
                     *  Entra nesse while quando acontecer de um roteiro de uma op durar mais de um dia na mesma máquina;
                     *  Por exemplo, durar os dias 17, 18 e 19 na mesma máquina;
                     *  Isso implica que nos dias 17 e 18 eu preciso zerar o tempo de antecipado e ocioso na máquina, isso vai acontecer ao inserir um registro na tabela de
                     *  medições com o FAT_ID "R" de reserva;
                     *  O somatório de tempo serve para quando chegar no dia último dia do roteiro fazer a diferença do tempoTotal de produção com os tempos somados 
                     *  da antecipação e ociosidade dos dias anteriores;
                     */

                    tempDtIni = tempDtIni.AddDays(auxCont);

                    var temp = medicoes
                        .Where(x =>
                            x.MED_DATA == tempDtIni &&
                            x.DIM_ID == v_ops_a_planejar[i].MaquinaId &&
                            (x.FAT_ID == "A" || x.FAT_ID == "O" || x.FAT_ID == "N")
                        ).ToList();


                    T_Medicoes medO = temp.Where(x => x.FAT_ID == "O").FirstOrDefault();
                    T_Medicoes medA = temp.Where(x => x.FAT_ID == "A").FirstOrDefault();
                    T_Medicoes medN = temp.Where(x => x.FAT_ID == "N").FirstOrDefault();


                    double valMedO = Convert.ToDouble(medO.MED_VALOR);
                    double valMedA = Convert.ToDouble(medA.MED_VALOR);
                    double valInsert = Math.Round((valMedO + valMedA) * -1);
                    somatorioTempo += valMedO + valMedA;

                    T_Medicoes medInsert = new T_Medicoes();
                    medInsert.DIM_ID = v_ops_a_planejar[i].MaquinaId;
                    medInsert.DIM_DESCRICAO = v_ops_a_planejar[i].MaquinaId;
                    medInsert.IND_ID = 85;
                    medInsert.MET_ID = 0;
                    medInsert.UNI_ID = 16;
                    medInsert.MED_DATA = v_ops_a_planejar[i].DataInicioPrevista;
                    medInsert.MED_VALOR = Convert.ToString(valInsert).Replace(",", ".");
                    medInsert.MED_AC_ANO = "0";
                    medInsert.MED_DATAMEDICAO = v_ops_a_planejar[i].DataInicioPrevista.Year.ToString() + v_ops_a_planejar[i].DataInicioPrevista.Month.ToString() + v_ops_a_planejar[i].DataInicioPrevista.Day.ToString();
                    medInsert.FAT_ID = "R";
                    medInsert.FAT_DESCRICAO = "RESERVA";
                    medInsert.MED_SQL = "MED_SQL";
                    medInsert.PlayAction = "INSERT";

                    listaMed.Add(medInsert); //São as medições dos dias anteriores;

                    cont_dias--;
                    auxCont++;
                }

                somatorioTempo = Math.Round((Convert.ToDouble(v_ops_a_planejar[i].TempoProducao) - somatorioTempo) * -1);

                medTemp = new T_Medicoes();
                medTemp.DIM_ID = v_ops_a_planejar[i].MaquinaId;
                medTemp.DIM_DESCRICAO = v_ops_a_planejar[i].MaquinaId;
                medTemp.IND_ID = 85;
                medTemp.MET_ID = 0;
                medTemp.UNI_ID = 16;
                medTemp.MED_DATA = v_ops_a_planejar[i].DataInicioPrevista;
                medTemp.MED_VALOR = Convert.ToString(somatorioTempo).Replace(",", ".");
                medTemp.MED_AC_ANO = "0";
                medTemp.MED_DATAMEDICAO = v_ops_a_planejar[i].DataInicioPrevista.Year.ToString() + v_ops_a_planejar[i].DataInicioPrevista.Month.ToString() + v_ops_a_planejar[i].DataInicioPrevista.Day.ToString();
                medTemp.FAT_ID = "R";
                medTemp.FAT_DESCRICAO = "RESERVA";
                medTemp.MED_SQL = "MED_SQL";
                medTemp.PlayAction = "INSERT";

                listaMed.Add(medTemp);

            }

            MasterController mc = new MasterController();
            List<object> listaInsert = new List<object>();
            listaInsert.Add(order);
            foreach (var item in listaMed)
            {
                listaInsert.Add(item);
            }

            List<LogPlay> logs = mc.UpdateData(new List<List<object>> { listaInsert }, 0, true);



            int j = 0;
            while (j < logs.Count && logs[j].Status.Equals("OK"))
            {
                j++;
            }

            if (j < logs.Count)
            {
                status = "Erro";
                msgRetorno = "Ocorreu erro ao inserir o pedido na fila. Erro: " + logs[j].MsgErro;
            }
            else
            {
                status = "OK";
                msgRetorno = "O pedido foi inserido na fila!";
            }

            return true;
        }


        private static Order preencherPedidosCM(string ord_id, string cli_id, string mun_id, string pro_id, int quantidade, DateTime data, string ord_status)
        {
            Order order = new Order();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (mun_id != null && !mun_id.Equals(""))
                {
                    var municipio = db.Municipio.Where(x => x.MUN_ID == mun_id).FirstOrDefault();
                    if (municipio == null)
                        mun_id = "";
                }

                if (mun_id == null || mun_id.Equals(""))
                {
                    var cliente = db.Cliente.AsNoTracking().Where(x => x.CLI_ID == cli_id).FirstOrDefault();
                    mun_id = cliente.MUN_ID;
                }
            }

            order.ORD_ID = ord_id;
            order.CLI_ID = cli_id;
            order.MUN_ID_ENTREGA = mun_id;
            order.PRO_ID = pro_id;
            order.ORD_QUANTIDADE = quantidade;
            order.ORD_DATA_ENTREGA_DE = data;
            order.ORD_DATA_ENTREGA_ATE = data;
            order.ORD_STATUS = ord_status;
            order.ORD_EMISSAO = DateTime.Now;

            //Campos obrigatórios
            order.PlayAction = "INSERT";
            order.ORD_TIPO = 0;
            order.ORD_TOLERANCIA_MAIS = 0;
            order.ORD_TOLERANCIA_MENOS = 0;
            order.ORD_INICIO_JANELA_EMBARQUE = data;
            order.ORD_FIM_JANELA_EMBARQUE = data;
            order.ORD_EMBARQUE_ALVO = data;
            order.ORD_INICIO_GRUPO_PRODUTIVO = data;
            order.ORD_FIM_GRUPO_PRODUTIVO = data;
            order.ORD_PESO_UNITARIO = 0;
            order.ORD_M2_UNITARIO = 0;
            order.ORD_LARGURA = 0;
            order.ORD_COMPRIMENTO = 0;
            order.ORD_GRAMATURA = 0;
            order.ORD_PRIORIDADE = 0;
            order.ORD_LOTE_PILOTO = 0;

            return order;
        }

        public static bool calcularSaldoRepresentante(int rep_id, double tempoTotal, DateTime dataDe, DateTime dataAte, string ord_status, ref string msgRetorno)
        {
            Representantes representante;
            List<Cotas> cotas = new List<Cotas>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                representante = db.Representantes.AsNoTracking().Where(x => x.REP_ID == rep_id).FirstOrDefault();
                cotas = db.Cotas.AsNoTracking().Where(x => x.REP_ID == rep_id &&
                    ((x.COT_DATA_DE < dataDe && x.COT_DATA_ATE > dataDe) ||
                    (x.COT_DATA_DE > dataDe && x.COT_DATA_DE < dataAte)))
                    .ToList();
            }

            if (cotas.Count == 0)
            {
                msgRetorno += " Porém, o representante não possui cotas para esse período!";
            }
            else if (representante == null)
            {
                msgRetorno += " Porém, este representante não existe!";
            }
            else
            {
                double totalTempoCotas = Convert.ToDouble(cotas.Sum(x => x.COT_VALOR) - cotas.Sum(x => x.COT_OCUPADO));
                double auxTempoTotal = tempoTotal;
                int i = 0;
                while (auxTempoTotal > 0)
                {

                    if (i < cotas.Count)
                    {
                        double disponivel = Convert.ToDouble(cotas[i].COT_VALOR - cotas[i].COT_OCUPADO);

                        if (disponivel > auxTempoTotal)
                        {
                            cotas[i].COT_OCUPADO += Convert.ToDecimal(auxTempoTotal);
                            auxTempoTotal = 0;
                        }
                        else
                        {
                            cotas[i].COT_OCUPADO = cotas[i].COT_VALOR;
                            auxTempoTotal -= disponivel;
                        }
                    }
                    else
                    {
                        msgRetorno += " Porém, o Representante: " + representante.REP_ID + " - " + representante.REP_NOME + " ocupou todas as cotas disponíveis para ele nesse período e ainda ficou faltando:" + auxTempoTotal + " horas para completar o pedido.";
                        cotas[i - 1].COT_OCUPADO += Convert.ToDecimal(auxTempoTotal);
                        auxTempoTotal = 0;
                    }

                    i++;
                }

                if (tempoTotal < totalTempoCotas)
                {
                    msgRetorno += " O pedido pode ser reservado por esse representante.";
                }

                if (ord_status != null && ord_status.Contains("R"))
                {
                    MasterController mc = new MasterController();
                    List<object> listaInsert = new List<object>();

                    foreach (var item in cotas)
                    {
                        item.PlayAction = "update";
                        listaInsert.Add(item);
                    }

                    List<LogPlay> logs = mc.UpdateData(new List<List<object>> { listaInsert }, 0, true);

                    return true;
                }
            }
            return false;
        }
    }
}
