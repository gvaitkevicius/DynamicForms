using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicForms.Interfaces;
using DynamicForms.Areas.PlugAndPlay.Models;
using System.Threading;
using DynamicForms.Controllers;

namespace DynamicForms.Areas.ApiSchedule.Models
{

    /// <summary>
    /// Classe que concentra os métodos relacionados a interface.
    /// Ela é utilizada pela ApiSchedule
    /// </summary>
    public class Interface
    {

        public IEnumerable<object> StartInterface(int id = 1)
        {
            var retorno = new List<object>();
            List<LogPlay> logInterface = new List<LogPlay>();

            try
            {
                if (ParametrosSingleton.Instance.semaforoInterface != "USING")
                {
                    ParametrosSingleton.Instance.semaforoInterface = "USING";
                    InterfaceTotal InterfaceTotal = new InterfaceTotal();
                    logInterface = InterfaceTotal.InterfaceStart(id);
                }
                else
                {
                    logInterface.Add(new LogPlay(nameof(Interface), "ERRO", "INTERFACE EM USO"));
                }
            }
            catch (Exception ex)
            {
                string msgErro = UtilPlay.getErro(ex);
                logInterface.Add(new LogPlay(nameof(Interface), "ERRO", msgErro));
            }
            finally {
                ParametrosSingleton.Instance.semaforoInterface = "FIM";
            }

            var logsErro = logInterface.Where(l => l.Status != "OK")
                                    .Select(l => new
                                    {
                                        l.NomeClasse,
                                        l.Status,
                                        l.MsgErro,
                                        l.PrimaryKey
                                    });

            retorno.AddRange(logsErro);

            var msgInterface = new List<Mensagem>();
            foreach (var item in logsErro)
            {
                var msg = new Mensagem
                {
                    MEN_TYPE = "ERRO_INTERFACE",
                    MEN_SEND = $"{item.NomeClasse} | {item.PrimaryKey} | {item.Status} | {item.MsgErro}",
                    MEN_EMISSION = DateTime.Now
                };
                msgInterface.Add(msg);
            }

            ParametrosSingleton.Instance.Menssagens.AddRange(msgInterface);

            Thread.Sleep(120000);

            return retorno;
        }

        public string TempoParaRodarInterface(string time)
        {
            string st;
            DateTime dateTime;
            bool dataValida = DateTime.TryParse(time, out dateTime);

            if (!dataValida)
                st = "ERRO";
            else
            {
                var msgInterface = new Mensagem
                {
                    MEN_TYPE = "TIME_INTERFACE",
                    MEN_SEND = time,
                    MEN_EMISSION = DateTime.Now
                };

                var msgOtimizador = new Mensagem
                {
                    MEN_TYPE = "TIME_OTIMIZADOR",
                    MEN_SEND = time,
                    MEN_EMISSION = DateTime.Now
                };

                ParametrosSingleton.Instance.Menssagens.Add(msgInterface);
                ParametrosSingleton.Instance.Menssagens.Add(msgOtimizador);

                st = "OK";
            }

            return st;
        }

        public object GetMensagens()
        {
            int total = ParametrosSingleton.Instance.Menssagens.Count;
            return new { totalMensagens = total, mensagens = ParametrosSingleton.Instance.Menssagens };
        }

        public string TesteTimeOut()
        {
            try
            {
                int minutos = 3;
                int tempoDeEspera = (minutos * 60) * 1000; // 30 minutos em milisegundos
                Thread.Sleep(tempoDeEspera);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Insere um registro na T_AGENDA_SCHEDULE apontando para executar a interface imediatamente.
        /// </summary>
        /// <returns></returns>
        public List<Mensagem> ExecutarAgora()
        {
            T_AGENDA_SCHEDULE executar_agora = new T_AGENDA_SCHEDULE();

            executar_agora.AGE_ORDEM_EXECUCAO = "INTERFACE";
            executar_agora.AGE_DATA_ESPECIFICA = DateTime.Now.Add(new TimeSpan(0, 1, 0));
            executar_agora.AGE_HORARIO_INICIO = DateTime.Now.TimeOfDay + new TimeSpan(0, 1, 0);
            executar_agora.AGE_HORARIO_FIM = DateTime.Now.TimeOfDay + new TimeSpan(0, 1, 0);
            executar_agora.AGE_PARAMETROS = "[{NOME_PARAMETRO:\"type_otimizador\", VALOR_PARAMETRO:\"2\"}, {NOME_PARAMETRO:\"id_interface\", VALOR_PARAMETRO:\"1\"}]";
            executar_agora.AGE_DESCRICAO = "EXECUCAO_IMEDIATA";
            executar_agora.PlayAction = "INSERT";

            List<Mensagem> mensagens = new List<Mensagem>();
            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(new List<List<object>>() { new List<object>() { executar_agora } }, 0, true);

            foreach (LogPlay log in logs)
            {
                Mensagem men = new Mensagem();
                men.MEN_SEND = log.MsgErro;
                men.MEN_TYPE = log.Status;
                men.MEN_EMISSION = DateTime.Now;

                mensagens.Add(men);
            }

            return mensagens;
        }
    }
}
