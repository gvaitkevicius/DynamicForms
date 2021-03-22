//using DynamicForms.Models;
//using DynamicForms.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using DynamicForms.Util;
//using System.Collections.Generic;
//using System.Linq;
//using System;
//using System.Threading;
//using DynamicForms.Areas.PlugAndPlay.Models;

//namespace DynamicForms.Areas.ApiInterface.Controllers
//{
//    [Area("ApiInterface")]
//    [Route("apiInterface/[controller]")]
//    [ApiController]
//    public class InterfaceController : ControllerBase
//    {
//        // GET: ApiInterface/Interface/StartInterface/2
//        [HttpGet(Name = "Start Interface")]
//        [Route("StartInterface/{id}")]
//        public IEnumerable<object> StartInterface(int id)
//        {
//            var retorno = new List<object>();
//            List<LogPlay> logInterface = new List<LogPlay>();

//            try
//            {
//                if (ParametrosSingleton.Instance.semaforoInterface != "USING")
//                {
//                    InterfaceTotal InterfaceTotal = new InterfaceTotal();
//                    //chama interface
//                    logInterface = InterfaceTotal.InterfaceStart(id);
//                }
//                else
//                {
//                    logInterface.Add(new LogPlay(nameof(InterfaceController), "ERRO", "INTERFACE EM USO"));
//                }
//            }
//            catch (Exception ex)
//            {
//                string msgErro = UtilPlay.getErro(ex);
//                logInterface.Add(new LogPlay(nameof(InterfaceController), "ERRO", msgErro));
//            }

//            var logsErro = logInterface.Where(l => l.Status != "OK")
//                                    .Select(l => new
//                                    {
//                                        l.NomeClasse,
//                                        l.Status,
//                                        l.MsgErro,
//                                        l.PrimaryKey
//                                    });

//            retorno.AddRange(logsErro);

//            var msgInterface = new List<Mensagem>();
//            foreach (var item in logsErro)
//            {
//                var msg = new Mensagem
//                {
//                    MEN_TYPE = "ERRO_INTERFACE",
//                    MEN_SEND = $"{item.NomeClasse} | {item.PrimaryKey} | {item.Status} | {item.MsgErro}",
//                    MEN_EMISSION = DateTime.Now
//                };
//                msgInterface.Add(msg);
//            }

//            ParametrosSingleton.Instance.Menssagens.AddRange(msgInterface);

//            return retorno;
//        }

//        // GET: ApiInterface/Interface/TempoParaRodarInterface?time=2020-08-28 05:21:43
//        [HttpGet(Name = "Tempo Interface")]
//        [Route("TempoParaRodarInterface")]
//        public string TempoParaRodarInterface(string time)
//        {
//            string st;
//            DateTime dateTime;
//            bool dataValida = DateTime.TryParse(time, out dateTime);

//            if (!dataValida)
//                st = "ERRO";
//            else
//            {
//                var msgInterface = new Mensagem
//                {
//                    MEN_TYPE = "TIME_INTERFACE",
//                    MEN_SEND = time,
//                    MEN_EMISSION = DateTime.Now
//                };

//                var msgOtimizador = new Mensagem
//                {
//                    MEN_TYPE = "TIME_OTIMIZADOR",
//                    MEN_SEND = time,
//                    MEN_EMISSION = DateTime.Now
//                };

//                ParametrosSingleton.Instance.Menssagens.Add(msgInterface);
//                ParametrosSingleton.Instance.Menssagens.Add(msgOtimizador);

//                st = "OK";
//            }

//            return st;
//        }

//        // GET: ApiInterface/Interface/GetMensagens
//        [HttpGet(Name = "Test Mensagens")]
//        [Route("GetMensagens")]
//        public object GetMensagens()
//        {
//            int total = ParametrosSingleton.Instance.Menssagens.Count;
//            return new { totalMensagens = total, mensagens = ParametrosSingleton.Instance.Menssagens };
//        }

//        // GET: ApiInterface/Interface/TesteTimeOut
//        [HttpGet(Name = "Test Timeout")]
//        [Route("TesteTimeOut")]
//        public string TesteTimeOut()
//        {
//            try
//            {
//                int minutos = 3;
//                int tempoDeEspera = (minutos * 60) * 1000; // 30 minutos em milisegundos
//                Thread.Sleep(tempoDeEspera);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
//    }
//}
