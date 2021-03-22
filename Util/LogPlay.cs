using DynamicForms.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicForms.Util
{
    public class LogPlay
    {
        public readonly JSgi _context;
        public LogPlay() { }

        public LogPlay(object objeto, string status, string msg_erro)
        {
            _context = new ContextFactory().CreateDbContext(new string[] { });
            BuildLog(objeto, status, msg_erro);
            _context = null;

        }


        public LogPlay(string status, string msg_erro)
        {
            this.NomeClasse = "";
            this.Status = status;
            this.MsgErro = msg_erro;
        }

        public LogPlay(object objeto, string status)
        {
            this.NomeClasse = GetNameClass(objeto);
            this.Status = status;
            this.MsgErro = "";
        }

        public LogPlay(string NomeClasse, string NomeAtributo, string Status, string MsgErro, string PrimaryKey)
        {
            this.NomeClasse = NomeClasse;
            this.NomeAtributo = NomeAtributo;
            this.Status = Status;
            this.MsgErro = MsgErro;
            this.PrimaryKey = PrimaryKey;
        }

        public LogPlay(string NomeClasse, string Status, string MsgErro)
        {
            this.NomeClasse = NomeClasse;
            this.Status = Status;
            this.MsgErro = MsgErro;
        }

        public string NomeClasse { get; set; }
        public string NomeAtributo { get; set; }
        public string Status { get; set; }
        public string MsgErro { get; set; }
        public string PrimaryKey { get; set; }
        public List<Property> Properties { get; set; }

        public List<LogPlay> GetLogsErro(List<LogPlay> logs)
        {
            return logs.Where(l => l.Status == "ERRO").ToList();
        }


        /// <summary>
        /// Atribui as propriedades do objeto seu devidos valores
        /// </summary>
        /// <param name="objeto">Objeto que possui os dados do log</param>
        /// <param name="status">Erro - se o objeto tiver algum problema, OK caso contrário</param>
        /// <param name="msg_erro">Mensagem de erro que será atribuída do log.
        /// Se o objeto não tiver erros, passe uma string vazia</param>
        private void BuildLog(object objeto, string status, string msg_erro)
        {
            this.NomeClasse = GetNameClass(objeto);
            this.Status = status;
            this.MsgErro = msg_erro;
            this.SetPrimaryKey(objeto.ToString(), objeto);
            this.Properties = GetInfoProperties(objeto, msg_erro);
        }

        /// <summary>
        /// Retorna o nome da classe do objeto
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        private string GetNameClass(object objeto)
        {
            string str_namespace = objeto.ToString();
            string[] vet_class = null;
            if (str_namespace.Contains("+"))
                vet_class = str_namespace.Split("+");
            else
                vet_class = str_namespace.Split(".");
            return vet_class[vet_class.Length - 1];
        }

        /// <summary>
        /// Verifica cada propriedade do objeto e grava no log o seu nome, valor, e mensagem de erro.
        /// </summary>
        /// <param name="objeto">Objeto com as com as propriedades para gravar no log</param>
        /// <param name="msg_erro">Mensagem de erro do objeto</param>
        /// <returns></returns>
        private List<Property> GetInfoProperties(object objeto, string msg_erro)
        {
            List<Property> properties = new List<Property>();

            Type type_class = Type.GetType(objeto.ToString());
            PropertyInfo[] propertyInfos = type_class.GetProperties();


            string[] erros_validacao = this.GetVetorErrorsValidation(msg_erro);
            string[] vet_properties = null;
            string[] vet_msg_erros = null;

            if (erros_validacao != null)
            {
                vet_properties = new string[erros_validacao.Length];
                vet_msg_erros = new string[erros_validacao.Length];

                for (int count = 0; count < erros_validacao.Length; count++)
                {
                    if (erros_validacao[count].Contains(":"))
                    {
                        string[] vet_aux = erros_validacao[count].Split(":", 2);
                        vet_properties[count] = vet_aux[0];
                        vet_msg_erros[count] = vet_aux[1];
                    }
                    else
                    {
                        vet_properties[count] = "undefined";
                        vet_msg_erros[count] = "undefined";
                    }
                }
            }

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo property = propertyInfos[i];
                Property prop = new Property();
                prop.Name = property.Name;
                try
                {
                    prop.Value = string.Format("{0}", property.GetValue(objeto));
                }
                catch (Exception)
                {
                    prop.Value = "null";
                }

                if (vet_msg_erros != null)
                {
                    for (int count = 0; count < vet_msg_erros.Length; count++)
                    {
                        if (prop.Name.ToLower() == vet_properties[count].ToLower())
                        {
                            prop.MsgErro = vet_msg_erros[count];
                            break;
                        }
                    }
                }

                properties.Add(prop);
            }

            return properties;
        }

        /// <summary>
        /// Separa a string de erro por ';' para retornar as propriedades dos objetos que estão com erro
        /// </summary>
        /// <param name="msg_erros">Mensagem de erro do objeto</param>
        /// <returns></returns>
        private string[] GetVetorErrorsValidation(string msg_erros)
        {
            string[] retorno = null;
            if (msg_erros != null && msg_erros != "" && msg_erros.Contains(";"))
                retorno = msg_erros.Split(";");

            return retorno;
        }
        /// <summary>
        /// Resume a lista de erros de log por agrupamentos em uma lista de logs
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        /// Task<ActionResult<List<List<Resume>>>>
        public List<List<Resume>> GetResumeErrorList(List<LogPlay> logs)
        {
            List<List<Resume>> _rusumes = new List<List<Resume>>();
            //Obtendo uma lista de logs agrupados por Nome de classe e Status de importação
            var logBad = logs.Where(ll => ll.Status != "OK").GroupBy(l => new { l.NomeClasse, l.MsgErro }).Select(log => new Resume { NomeClasse = log.Key.NomeClasse, Erros = log.ToList(), Total = log.ToList().Count() }).ToList();
            var logOk = logs.Where(ll => ll.Status == "OK").GroupBy(l => new { l.NomeClasse, l.MsgErro }).Select(log => new Resume { NomeClasse = log.Key.NomeClasse, Erros = log.ToList(), Total = log.ToList().Count() }).ToList();

            _rusumes.Add(logBad);
            _rusumes.Add(logOk);

            return _rusumes;
        }


        public List<LogPlay> ConcatenateLogs(List<LogPlay> Oldlogs, List<LogPlay> NewLogs)
        {
            foreach (LogPlay oldLog in Oldlogs)
            {
                for (int i = 0; i < NewLogs.Count; i++)
                {
                    LogPlay newLog = NewLogs[i];
                    if (oldLog.NomeClasse == newLog.NomeClasse && oldLog.PrimaryKey.Equals(newLog.PrimaryKey))
                    {
                        oldLog.MsgErro += " " + newLog.MsgErro;
                        if (newLog.Status.StartsWith("ERRO"))
                        {
                            string[] _status_split = oldLog.Status.Split('_');
                            if (_status_split.Length > 1)
                            {
                                oldLog.Status = newLog.Status + "_" + _status_split[1];
                            }
                            else
                            {
                                oldLog.Status = newLog.Status;
                            }
                        }
                        NewLogs.RemoveAt(i);
                        i = NewLogs.Count;
                    }
                }
            }
            return Oldlogs; ;
        }


        /// <summary>
        /// Encontra as propriedades e seus respectivos valores que são primary key do objeto e grava no log
        /// </summary>
        /// <param name="nome_classe">Nome da classe que possui as primary keys</param>
        /// <param name="obj">Objeto instanciado referente ao nome da classe</param>
        public void SetPrimaryKey(string nome_classe, object obj)
        {


            string[] vet_pk = UtilPlay.GetPrimaryKey(nome_classe, _context);
            Type type = Type.GetType(nome_classe);
            PropertyInfo[] propertyInfos = type.GetProperties();
            StringBuilder pk = new StringBuilder();

            for (int i = 0; i < vet_pk.Length; i++)
            {
                for (int j = 0; j < propertyInfos.Length; j++)
                {
                    string name_property = propertyInfos[j].Name;
                    if (vet_pk[i].Contains(name_property))
                    {
                        try
                        {
                            string strIdValue = "NULL";
                            if (propertyInfos[j].GetValue(obj) != null)
                                strIdValue = string.Format("{0}", propertyInfos[j].GetValue(obj));

                            if (i == 0)
                                pk.Append(string.Format("{0}:{1}", vet_pk[i], strIdValue));
                            else
                                pk.Append(string.Format(";{0}:{1}", vet_pk[i], strIdValue));
                        }
                        catch (Exception)
                        {
                            if (i == 0)
                                pk.Append(string.Format("{0}:null", vet_pk[i]));
                            else
                                pk.Append(string.Format(";{0}:null", vet_pk[i]));
                        }
                        break;
                    }
                }
            }


            #region propriedades de integração

            var propIntegracao = propertyInfos.Where(x => x.Name.Contains("integracao", StringComparison.OrdinalIgnoreCase));
            for (int i = 0; i < propIntegracao.Count(); i++)
            {
                var prop = propIntegracao.ElementAt(i);

                string strValue = "NULL";
                if (prop.GetValue(obj) != null)
                    strValue = string.Format("{0}", prop.GetValue(obj));

                pk.Append(string.Format(";{0}:{1}", prop.Name, strValue));
            }

            #endregion propriedades de integração

            this.PrimaryKey = pk.ToString();
        }


        [Obsolete]
        public string GetLog(object obj)
        {
            string retorno = "";
            if (obj != null)
            {
                string str_namespace = obj.ToString();

                string[] vet_class = null;

                if (str_namespace.Contains("+"))
                    vet_class = str_namespace.Split("+");
                else
                    vet_class = str_namespace.Split(".");

                if (vet_class != null)
                {
                    string class_name = vet_class[vet_class.Length - 1];
                    retorno = string.Format("Classe:{0}", class_name);

                    Type type_class = Type.GetType(obj.ToString());
                    PropertyInfo[] propertyInfos = type_class.GetProperties();

                    int i = 0;
                    for (; i < propertyInfos.Length; i++)
                    {
                        PropertyInfo property = propertyInfos[i];
                        retorno += ",";
                        retorno += property.Name.ToString();
                        try
                        {
                            retorno += ":" + property.GetValue(obj).ToString();
                        }
                        catch (Exception)
                        {
                            retorno += ":null";
                        }
                    }
                }
            }

            return retorno;
        }


    }
    public class Resume
    {
        public string NomeClasse { get; set; }
        public List<LogPlay> Erros { get; set; }
        public int Total { get; set; }
        public Resume()
        {

        }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string MsgErro { get; set; }
    }
}
