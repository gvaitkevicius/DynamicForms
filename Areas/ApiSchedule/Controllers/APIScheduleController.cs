using DynamicForms.Areas.ApiSchedule.Models;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OptShered;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.ApiSchedule.Controllers
{
    [Area("ApiSchedule")]
    [Route("apischedule/[controller]")]
    [ApiController]
    public class APIScheduleController : ControllerBase
    {
        #region Schedule

        [HttpGet(Name = "Start Schedule")]
        [Route("StartSchedule/{ordemInterface}/{ordemOtimizador}/{id_interface}/{type_otimizador}/{proxima_dt_interface}/{proxima_dt_otimizador}")]
        public IEnumerable<object> StartSchedule(int ordemInterface, int ordemOtimizador, int id_interface, int type_otimizador, string proxima_dt_interface, string proxima_dt_otimizador)
        {

            //Se o otimizador e a interface estiverem em uso, aborta a função
            if (ObterStatusOtimizador().MEN_SEND.ToLower() == "true" && ObterStatusInterface().MEN_SEND.ToLower() == "true")
                return null;

            List<Mensagem> mensagens = new List<Mensagem>();

            if (ordemInterface == 0 && ordemOtimizador == 0) //Se não for para executar nem interface e nem o otimizador, aborta o método
                return mensagens;

            //Se for maior que 0, irá executar, então, adiciona a ultima data de execução
            if ((int)ordemInterface > 0)
                ParametrosSingleton.Instance.ultima_exec_interface = DateTime.Now;
            if ((int)ordemOtimizador > 0)
                ParametrosSingleton.Instance.ultima_exec_otimizador = DateTime.Now;
            try
            {
                //Salva as próximas execuções
                if (!String.IsNullOrEmpty(proxima_dt_otimizador))
                {
                    DateTime nova_data = DateTime.ParseExact(proxima_dt_otimizador, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                    if (nova_data != DateTime.MinValue)
                        ParametrosSingleton.Instance.proxima_exec_otimizador = nova_data;
                }
                if (!String.IsNullOrEmpty(proxima_dt_interface))
                {
                    DateTime nova_data = DateTime.ParseExact(proxima_dt_interface, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                    if (nova_data != DateTime.MinValue)
                        ParametrosSingleton.Instance.proxima_exec_interface = nova_data;
                }


                //Cria um dicionário numerico de ordem ascendente
                //O primeiro a ser executado será o com Key = 1, em seguida o com Key = 2, se Key = 0 não será executado
                Dictionary<int, string> ordem_execucao = new Dictionary<int, string>()
                {
                    { ordemInterface, "INTERFACE" },
                    { ordemOtimizador, "OTIMIZADOR" }
                };
                ordem_execucao = ordem_execucao.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

                //Dá um foreach nas keys que são maior que 0, e executa a interface ou o otimizador
                foreach (KeyValuePair<int, string> element in ordem_execucao)
                {
                    if (element.Key > 0)
                    {
                        try
                        {
                            if (element.Value == "INTERFACE")
                            {
                                //Interface _interface = new Interface();
                                //IEnumerable<object> retorno_interface = _interface.StartInterface(id_interface);
                                //List<Mensagem> retorno_convertido = retorno_interface.Cast<dynamic>().Select(x => new Mensagem
                                //{
                                //    MEN_TYPE = "ERRO_INTERFACE",
                                //    MEN_EMISSION = DateTime.Now,
                                //    MEN_SEND = $"{x.NomeClasse} | {x.PrimaryKey} | {x.Status} | {x.MsgErro}"
                                //}).ToList();

                                //mensagens.AddRange(retorno_convertido);
                            }
                            else if (element.Value == "OTIMIZADOR")
                            {
                                Otimizador _otimizador = new Otimizador();
                                List<Mensagem> retorno_otimizador = _otimizador.StartOtimizador(type_otimizador);

                                mensagens.AddRange(retorno_otimizador);
                            }
                        }
                        catch (Exception ex)
                        {
                            mensagens.Add(new Mensagem() { MEN_SEND = ex.Message, MEN_EMISSION = DateTime.Now, MEN_TYPE = "ERRO_SCHEDULE" });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Mensagem men = new Mensagem()
                {
                    MEN_SEND = ex.Message,
                    MEN_EMISSION = DateTime.Now,
                    MEN_TYPE = "ERRO_SCHEDULE"
                };
                    
                mensagens.Add(men);
            }

            return mensagens;
        }

        [HttpGet(Name = "Cancelar Execucao")]
        [Route("CancelarExecucao")]
        public List<Mensagem> CancelarExecucao() 
        {
            List<Mensagem> result = new List<Mensagem>();

            try
            {
                OPTParametrosSingleton.Instance.Cancelar = true;

                result.Add(new Mensagem { MEN_TYPE = "SUCESSO_SCHEDULE", MEN_SEND = "Sucesso ao cancelar as rotinas que estão executando.", MEN_EMISSION = DateTime.Now });
            }
            catch(Exception e)
            {
                result.Add(new Mensagem { MEN_TYPE = "ERRO_SCHEDULE", MEN_SEND = e.Message, MEN_EMISSION = DateTime.Now });
            }

            return result;
        }

        #endregion

        #region Interface

        /// <summary>
        /// Obtem as mensagens da interface do ParametrosSingleton, e adiciona mensagens para demonstrar o status e a próxima execução
        /// </summary>
        /// <returns></returns>
        // GET: apischedule/Schedule/ObterMensagensInterface
        [HttpGet(Name = "Obter Mensagens Interface")]
        [Route("ObterMensagensInterface")]
        public List<Mensagem> ObterMensagensInterface()
        {
            List<Mensagem> msgInterface = new List<Mensagem>();
            msgInterface.AddRange(ParametrosSingleton.Instance.Menssagens);

            Mensagem men_status_interface = ObterStatusInterface();
            Mensagem men_proxima_exec_interface = ObterProximaExecInterface();
            Mensagem men_ultima_exec_interface = ObterUltimaExecInterface();

            if (men_status_interface != null)
                msgInterface.Add(men_status_interface);

            if(men_proxima_exec_interface != null)
                msgInterface.Add(men_proxima_exec_interface);

            if (men_ultima_exec_interface != null)
                msgInterface.Add(men_ultima_exec_interface);

            return msgInterface;
        }

        /// <summary>
        /// Insere na tabela T_AGENDA_SCHEDULE um registro para execução imediata.
        /// </summary>
        /// <returns></returns>
        // POST: apischedule/Schedule/ExecutarAgoraInterface
        [HttpPost(Name = "Executar Agora Interface")]
        [Route("ExecutarAgoraInterface")]
        public List<Mensagem> ExecutarAgoraInterface()
        {
            Interface _interface = new Interface();
            return _interface.ExecutarAgora();
        }

        // POST: apischedule/Schedule/ExecutarProcedureInterface
        [HttpPost(Name = "Executar Procedure Interface")]
        [Route("ExecProcedureInterface")]
        public List<Mensagem> ExecProcedureInterface()
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            try
            {
                using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
                {
                    db.Database.ExecuteSqlCommand("EXEC SP_PLUG_INTERFACE_AFTER_OPT_CUSTON");
                }
            }
            catch(Exception e)
            {
                Mensagem men = new Mensagem { MEN_SEND = e.Message, MEN_TYPE = "ERRO_INTERFACE", MEN_EMISSION = DateTime.Now };
                mensagens.Add(men);
            }

            return mensagens;

        }

        /// <summary>
        /// Verifica no ParametrosSingleton se a interface esta == "USING" retornando uma mensagem.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterStatusInterface()
        {
            bool interfaceRodando = ParametrosSingleton.Instance.semaforoInterface == "USING";
            var msg = new Mensagem
            {
                MEN_TYPE = "RUN_INTERFACE",
                MEN_SEND = interfaceRodando.ToString(),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        /// <summary>
        /// Verifica no ParametrosSingleton qual a próxima execução da interface, retornando uma mensagem com a data formatada.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterProximaExecInterface()
        {
            if (ParametrosSingleton.Instance.proxima_exec_interface == DateTime.MinValue)
                return null;

            var msg = new Mensagem
            {
                MEN_TYPE = "PROX_EXEC_INTERFACE",
                MEN_SEND = ParametrosSingleton.Instance.proxima_exec_interface.ToString("dd/MM/yyyy HH:mm"),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        /// <summary>
        /// Verifica no ParametrosSingleton qual a próxima execução da interface, retornando uma mensagem com a data formatada.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterUltimaExecInterface()
        {
            if (ParametrosSingleton.Instance.ultima_exec_interface == DateTime.MinValue)
                return null;

            var msg = new Mensagem
            {
                MEN_TYPE = "ULTIMA_EXEC_INTERFACE",
                MEN_SEND = ParametrosSingleton.Instance.ultima_exec_interface.ToString("dd/MM/yyyy HH:mm"),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        #endregion

        #region Otimizador

        /// <summary>
        /// Obtem as mensagens do OPTParametrosSingleton, e tambem adiciona mensagens de status e proxima execução do otimizador.
        /// </summary>
        /// <returns></returns>
        // GET: ApiOtimizador/Otimizador/ObterMensagensOtimizador
        [HttpGet(Name = "Obter Mensagens Otimizador")]
        [Route("ObterMensagensOtimizador")]
        public List<Mensagem> ObterMensagensOtimizador()
        {
            List<sheMensagem> she_msgOtimizador = OPTParametrosSingleton.Instance.Menssagens.Take(10).OrderByDescending(x=>x.MEN_EMISSION).ToList();
            List<Mensagem> msgOtimizador = she_msgOtimizador.Select(x => new Mensagem
            {
                MEN_SEND = x.MEN_SEND,
                MEN_TYPE = x.MEN_TYPE,
                MEN_EMISSION = x.MEN_EMISSION
            }).ToList();

            Mensagem men_status_otimizador = ObterStatusOtimizador();
            Mensagem men_ultima_exec_otimizador = ObterUltimaExecOtimizador();
            Mensagem men_proximo_exec_otimizador = ObterProximaExecOtimizador();

            if(men_status_otimizador != null)
                msgOtimizador.Add(men_status_otimizador);

            if(men_proximo_exec_otimizador != null)
                msgOtimizador.Add(men_proximo_exec_otimizador);

            if (men_ultima_exec_otimizador != null)
                msgOtimizador.Add(men_ultima_exec_otimizador);

            return msgOtimizador;
        }

        /// <summary>
        /// Verifica no OPTParametrosSingleton se o otimizador == "USING", retornando uma mensagem de acordo.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterStatusOtimizador()
        {
            bool otimizadorRodando = OPTParametrosSingleton.Instance.semaforoOtimizador == "USING";
            var msg = new Mensagem
            {
                MEN_TYPE = "RUN_OTIMIZADOR",
                MEN_SEND = otimizadorRodando.ToString(),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        /// <summary>
        /// Obtem do ParametrosSingleton a proxima execução do otimizador, retornando uma mensagem com a data formatada.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterProximaExecOtimizador()
        {
            if (ParametrosSingleton.Instance.proxima_exec_otimizador == DateTime.MinValue)
                return null;

            var msg = new Mensagem
            {
                MEN_TYPE = "PROX_EXEC_OTIMIZADOR",
                MEN_SEND = ParametrosSingleton.Instance.proxima_exec_otimizador.ToString("dd/MM/yyyy HH:mm"),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }


        /// <summary>
        /// Obtem do ParametrosSingleton a ultima execução do otimizador, retornando uma mensagem com a data formatada.
        /// </summary>
        /// <returns></returns>
        private Mensagem ObterUltimaExecOtimizador()
        {
            if (ParametrosSingleton.Instance.ultima_exec_otimizador == DateTime.MinValue)
                return null;

            var msg = new Mensagem
            {
                MEN_TYPE = "ULTIMA_EXEC_OTIMIZADOR",
                MEN_SEND = ParametrosSingleton.Instance.ultima_exec_otimizador.ToString("dd/MM/yyyy HH:mm"),
                MEN_EMISSION = DateTime.Now
            };
            return msg;
        }

        /// <summary>
        /// Adiciona na tabela T_AGENDA_SCHEDULE um registro de execução imediata.
        /// </summary>
        /// <param name="type_otimizador"></param>
        /// <returns></returns>
        // POST: apischedule/Schedule/ExecutarAgoraOtimizador
        [HttpPost(Name = "Executar Agora Otimizador")]
        [Route("ExecutarAgoraOtimizador")]
        public List<Mensagem> ExecutarAgoraOtimizador(string type_otimizador)
        {
            Otimizador _otimizador = new Otimizador();
            return _otimizador.ExecutarAgora(type_otimizador);
        }

        #endregion
    }
}
