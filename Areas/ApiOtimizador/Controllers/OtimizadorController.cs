//using System;
//using System.Collections.Generic;
//using DynamicForms.Areas.PlugAndPlay.Models;
//using DynamicForms.Context;
//using DynamicForms.Models;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using OptMiddleware;
//using OptShered;
//using OptTransport;

//namespace DynamicForms.Areas.ApiOtimizador.Controllers
//{
//    [Area("ApiOtimizador")]
//    [Route("ApiOtimizador/[controller]")]
//    [ApiController]
//    public class OtimizadorController : ControllerBase
//    {
//        // GET: ApiOtimizador/Otimizador/StartOtimizador
//        [HttpGet(Name = "Start Otimizador")]
//        [Route("StartOtimizador")]
//        public List<Mensagem> StartOtimizador()
//        {
//            List<Mensagem> retorno = new List<Mensagem>();
//            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
//            {
//                try
//                {
//                    if (ParametrosSingleton.Instance.semaforoOtimizador != "USING")
//                    {
//                        ParametrosSingleton.Instance.Menssagens.RemoveAll(x => x.MEN_TYPE.EndsWith("OTIMIZADOR"));

//                        DateTime Agora = DateTime.Now;
//                        OptQueueTransport t_Otimizado = null;
//                        sheOptQueueBox b = new sheOptQueueBox();
//                        sheOptQueueTransport t = new sheOptQueueTransport();
//                        OPTMiddleware run = new OPTMiddleware(Agora, 1, "", 0, b, t, null, null, ref t_Otimizado);

//                        List<sheMenssagens> msgOtimizador = run.Menssagens;
//                        msgOtimizador.ForEach(m =>
//                        {
//                            var msg = new Mensagem
//                            {
//                                MEN_ID = m.MEN_ID,
//                                MEN_TYPE = $"{m.MEN_TYPE}_OTIMIZADOR",
//                                MEN_SEND = m.MEN_SEND,
//                                MEN_EMISSION = m.MEN_EMISSION
//                            };
//                            retorno.Add(msg);
//                        });
//                    }
//                    else
//                    {
//                        retorno.Add(new Mensagem
//                        {
//                            MEN_TYPE = "ERRO_OTIMIZADOR",
//                            MEN_SEND = "OTIMIZADOR EM USO",
//                            MEN_EMISSION = DateTime.Now
//                        });
//                    }

//                }
//                catch (Exception e)
//                {
//                    var msgErro = new Mensagem
//                    {
//                        MEN_TYPE = "ERRO_OTIMIZADOR",
//                        MEN_SEND = e.Message,
//                        MEN_EMISSION = DateTime.Now
//                    };
//                    retorno.Add(msgErro);
//                }
//            }

//            ParametrosSingleton.Instance.Menssagens.AddRange(retorno);
//            return retorno;
//        }

//        // GET: ApiOtimizador/Otimizador/TempoParaRodarOtimizador?time=2020-08-28 05:21:43
//        [HttpGet(Name = "Tempo Otimizador")]
//        [Route("TempoParaRodarOtimizador")]
//        public string TempoParaRodarOtimizador(string time)
//        {
//            string st;
//            DateTime dateTime;
//            bool dataValida = DateTime.TryParse(time, out dateTime);

//            if (!dataValida)
//                st = "ERRO";
//            else
//            {
//                var mensagem = new Mensagem
//                {
//                    MEN_TYPE = "TIME_OTIMIZADOR",
//                    MEN_SEND = time,
//                    MEN_EMISSION = DateTime.Now
//                };

//                ParametrosSingleton.Instance.Menssagens.Add(mensagem);

//                st = "OK";
//            }

//            return st;
//        }

//        // GET: ApiOtimizador/Otimizador/MensagemProgesso?mensagem=
//        [HttpGet(Name = "Mensagem Progesso")]
//        [Route("MensagemProgesso")]
//        public string MensagemProgesso(string mensagem)
//        {
//            string st = "";

//            if (string.IsNullOrEmpty(mensagem))
//                st = "ERRO: O valor da mensagem é vazio ou nulo.";
//            else
//            {
//                try
//                {
//                    var msg = JsonConvert.DeserializeObject<Mensagem>(mensagem);
//                    ParametrosSingleton.Instance.Menssagens.Add(msg);
//                    st = "OK";
//                }
//                catch (Exception)
//                {
//                    st = "ERRO: Não foi possível converter o objeto Mensagem.";
//                }
//            }

//            return st;
//        }

//        // GET: ApiOtimizador/Otimizador/ObterMensagensOtimizador
//        [HttpGet(Name = "Obter Mensagens Otimizador")]
//        [Route("ObterMensagensOtimizador")]
//        public List<Mensagem> ObterMensagensOtimizador()
//        {
//            List<Mensagem> msgOtimizador = new List<Mensagem>();
//            //msgOtimizador = ParametrosSingleton.Instance.Menssagens.Where(m => m.MEN_TYPE.EndsWith("OTIMIZADOR")).ToList(); ;

//            var data = DateTime.Now;
//            for (int i = 0; i < 3; i++)
//            {
//                // Mensagem de tempo restante para rodar o otimizador
//                //var msg = new Mensagem
//                //{
//                //    MEN_TYPE = "TIME_OTIMIZADOR",
//                //    MEN_SEND = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss"),
//                //    MEN_EMISSION = data.AddSeconds(1)
//                //};
//                //msgOtimizador.Insert(0, msg);

//                // Mensagem de progresso do otimizador
//                var msg = new Mensagem
//                {
//                    MEN_TYPE = "PROGRESS_OTIMIZADOR",
//                    MEN_SEND = 30 * (i + 1) + "%",
//                    MEN_EMISSION = data.AddSeconds(1)
//                };
//                msgOtimizador.Add(msg);
//            }

//            msgOtimizador.Add(ObterStatusOtimizador());

//            return msgOtimizador;
//        }

//        private Mensagem ObterStatusOtimizador()
//        {

//            bool otimizadorRodando = ParametrosSingleton.Instance.semaforoOtimizador == "USING";
//            var msg = new Mensagem
//            {
//                MEN_TYPE = "RUN_OTIMIZADOR",
//                //MEN_SEND = otimizadorRodando.ToString(),
//                MEN_SEND = true.ToString(),
//                MEN_EMISSION = DateTime.Now
//            };
//            return msg;
//        }
//    }
//}
