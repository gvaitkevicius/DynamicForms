using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Newtonsoft.Json;
using OptMiddleware;
using OptShered;
using OptTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.ApiSchedule.Models
{
    /// <summary>
    /// Classe que concentra os métodos relacionados a interface.
    /// Ela é utilizada pela ApiSchedule
    /// </summary>
    public class Otimizador
    {
        public List<Mensagem> StartOtimizador(int type_otimizador)
        {
            List<sheMensagem> retorno = new List<sheMensagem>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                OPTMiddleware run = null;
                OptQueueTransport t_Otimizado = null;
                try
                {
                    if (OPTParametrosSingleton.Instance.semaforoOtimizador != "USING")
                    {
                        OPTParametrosSingleton.Instance.semaforoOtimizador = "USING";
                        OPTParametrosSingleton.Instance.Menssagens = new List<sheMensagem>();

                        string logDebug = "";
                        DateTime ini = DateTime.Now;
                        DateTime agra = DateTime.Now;
                        sheOptQueueBox b = new sheOptQueueBox();
                        sheOptQueueTransport t = new sheOptQueueTransport();
                        run = new OPTMiddleware(agra, 1, "", 0, b, t, null, null, ref t_Otimizado);
                        List<sheMenssagens> msgOtimizador = run.Menssagens;
                        msgOtimizador.ForEach(m =>
                        {
                            var msg = new sheMensagem
                            {
                                MEN_ID = m.MEN_ID,
                                MEN_TYPE = $"{m.MEN_TYPE}_OTIMIZADOR",
                                MEN_SEND = m.MEN_SEND,
                                MEN_EMISSION = m.MEN_EMISSION
                            };
                            retorno.Add(msg);
                        });
                    }
                    else
                    {
                        retorno.Add(new sheMensagem
                        {
                            MEN_TYPE = "ERRO_OTIMIZADOR",
                            MEN_SEND = "OTIMIZADOR EM USO",
                            MEN_EMISSION = DateTime.Now
                        });
                    }

                }
                catch (Exception e)
                {
                    var msgErro = new sheMensagem
                    {
                        MEN_TYPE = "ERRO_OTIMIZADOR",
                        MEN_SEND = e.Message,
                        MEN_EMISSION = DateTime.Now
                    };
                    retorno.Add(msgErro);
                }
            }

            OPTParametrosSingleton.Instance.Menssagens.AddRange(retorno);

            return retorno.Cast<Mensagem>().ToList();
        }

        public string TempoParaRodarOtimizador(string time)
        {
            string st;
            DateTime dateTime;
            bool dataValida = DateTime.TryParse(time, out dateTime);

            if (!dataValida)
                st = "ERRO";
            else
            {
                var mensagem = new sheMensagem
                {
                    MEN_TYPE = "TIME_OTIMIZADOR",
                    MEN_SEND = time,
                    MEN_EMISSION = DateTime.Now
                };

                OPTParametrosSingleton.Instance.Menssagens.Add(mensagem);

                st = "OK";
            }

            return st;
        }

        public string MensagemProgesso(string mensagem)
        {
            string st = "";

            if (string.IsNullOrEmpty(mensagem))
                st = "ERRO: O valor da mensagem é vazio ou nulo.";
            else
            {
                try
                {
                    var msg = JsonConvert.DeserializeObject<sheMensagem>(mensagem);
                    OPTParametrosSingleton.Instance.Menssagens.Add(msg);
                    st = "OK";
                }
                catch (Exception)
                {
                    st = "ERRO: Não foi possível converter o objeto Mensagem.";
                }
            }

            return st;
        }

        public List<Mensagem> ObterMensagensOtimizador()
        {
            List<Mensagem> msgOtimizador = new List<Mensagem>();
            //msgOtimizador = ParametrosSingleton.Instance.Menssagens.Where(m => m.MEN_TYPE.EndsWith("OTIMIZADOR")).ToList(); ;

            var data = DateTime.Now;
            for (int i = 0; i < 3; i++)
            {
                // Mensagem de tempo restante para rodar o otimizador
                //var msg = new Mensagem
                //{
                //    MEN_TYPE = "TIME_OTIMIZADOR",
                //    MEN_SEND = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"),
                //    MEN_EMISSION = data.AddSeconds(1)
                //};
                //msgOtimizador.Insert(0, msg);

                // Mensagem de progresso do otimizador
                var msg = new Mensagem
                {
                    MEN_TYPE = "PROGRESS_OTIMIZADOR",
                    MEN_SEND = 30 * (i + 1) + "%",
                    MEN_EMISSION = data.AddSeconds(1)
                };
                msgOtimizador.Add(msg);
            }

            msgOtimizador.Add(ObterStatusOtimizador());

            return msgOtimizador;
        }

        private Mensagem ObterStatusOtimizador()
        {

            bool otimizadorRodando = OPTParametrosSingleton.Instance.semaforoOtimizador == "USING";
            var msg = new Mensagem
            {
                MEN_TYPE = "RUN_OTIMIZADOR",
                //MEN_SEND = otimizadorRodando.ToString(),
                MEN_SEND = true.ToString(),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        /// <summary>
        /// Insere um registro na T_AGENDA_SCHEDULE apontando para executar o otimizador imediatamente.
        /// </summary>
        /// <returns></returns>
        public List<Mensagem> ExecutarAgora(string type_otimizador)
        {
            T_AGENDA_SCHEDULE executar_agora = new T_AGENDA_SCHEDULE();

            executar_agora.AGE_ORDEM_EXECUCAO = "OTIMIZADOR";
            executar_agora.AGE_DATA_ESPECIFICA = DateTime.Now.Add(new TimeSpan(0, 1, 0));
            executar_agora.AGE_HORARIO_INICIO = DateTime.Now.TimeOfDay + new TimeSpan(0, 1, 0);
            executar_agora.AGE_HORARIO_FIM = DateTime.Now.TimeOfDay + new TimeSpan(0, 1, 0);
            executar_agora.AGE_PARAMETROS = "[{NOME_PARAMETRO:\"type_otimizador\", VALOR_PARAMETRO:\"" + type_otimizador + "\"}, {NOME_PARAMETRO:\"id_interface\", VALOR_PARAMETRO:\"1\"}]";
            executar_agora.AGE_DESCRICAO = "EXECUCAO_IMEDIATA";
            executar_agora.PlayAction = "INSERT";

            List<Mensagem> mensagens = new List<Mensagem>();
            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(new List<List<object>>() { new List<object>() { executar_agora } }, 0, true);

            foreach(LogPlay log in logs)
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
