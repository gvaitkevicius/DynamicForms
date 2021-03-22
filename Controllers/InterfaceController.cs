using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Interfaces;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicForms.Controllers
{
    public class InterfaceController : BaseController
    {

        //public async Task<IActionResult> IndexAsync(int id)
        //{
        //    try
        //    {
        //        JSgi _context = new ContextFactory().CreateDbContext(new string[] { });
        //        List<LogPlay> logInterface = new List<LogPlay>();
        //        var stopwatch = new Stopwatch();
        //        double tempo = 0;
        //        LogPlay log = new LogPlay();
        //        InterfaceTotal InterfaceTotal = new InterfaceTotal();
        //        stopwatch.Start();

        //        //chama interface
        //        logInterface = InterfaceTotal.InterfaceStart(id);
        //        var _rusumes = Task.Run(() => log.GetResumeErrorList(logInterface).Take(50).ToList());
        //        var _pendencias = Task.Run(() => _context.GetPendenciasGerais().Result.ToList());
        //        await Task.WhenAll(_pendencias, _rusumes);
        //        stopwatch.Stop();
        //        tempo = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());

        //        ViewBag.TempoTotal = tempo;
        //        ViewBag.Pendencias = _pendencias.Status == TaskStatus.RanToCompletion ? _pendencias.Result : null;
        //        ViewBag.LogsDoSistemaBad = _rusumes.Status == TaskStatus.RanToCompletion ? _rusumes.Result[0] : null;
        //        ViewBag.LogsDoSistemaOk = _rusumes.Status == TaskStatus.RanToCompletion ? _rusumes.Result[1] : null;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        var st = UtilPlay.getErro(ex);
        //        return Json(new { st });
        //    }
        //}

        public IActionResult RenderizarBodyMaxTimeOut(string url)
        {
            if (String.IsNullOrEmpty(url))
                return View("Index");

            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(user, typeof(InterfaceController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            ViewBag.Url = url;

            return View();
        }

        [Authorize]
        public IActionResult IndexAsync(int id)
        {
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(user, typeof(InterfaceController).FullName))
                return StatusCode(403);

            try
            {
                string st = "";
                if (ParametrosSingleton.Instance.semaforoInterface != "USING")
                {
                    // IMPLEMENTAR WORKER SERVICE
                    JSgi _context = new ContextFactory().CreateDbContext(new string[] { });
                    List<LogPlay> logInterface = new List<LogPlay>();
                    var stopwatch = new Stopwatch();
                    double tempo = 0;
                    LogPlay log = new LogPlay();
                    InterfaceTotal InterfaceTotal = new InterfaceTotal();
                    stopwatch.Start();

                    //chama interface
                    logInterface = InterfaceTotal.InterfaceStart(id);
                    var _rusumes = log.GetResumeErrorList(logInterface).Take(50).ToList();
                    log.GetResumeErrorList(logInterface).Take(50).ToList();

                    //try
                    //{
                    //    ViewBag.Pendencias = _context.GetPendenciasGerais().Result.ToList();
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    stopwatch.Stop();
                    tempo = Convert.ToDouble(stopwatch.Elapsed.TotalSeconds.ToString());
                    ViewBag.TempoTotal = tempo;
                    ViewBag.LogsDoSistemaBad = _rusumes[0].Take(50).ToList();
                    ViewBag.LogsDoSistemaOk = _rusumes[1].Take(50).ToList();
                }
                else
                {
                    st = "INTERFACE EM USO.";
                    return Json(new { st });
                }
                return View("IndexAsync");
            }
            catch (Exception ex)
            {
                var st = UtilPlay.getErro(ex);
                return Json(new { st });
            }
        }

        public IActionResult OPT(int id)
        {
            try
            {


                //var a = Opt.OptFunctions.FilaExpedicao(db, 0, "FULL", true, 0, null, ref Menssagens);
                //if (a != null)
                //{
                //    System.Console.WriteLine(GlobalFunctons.retErro(a));
                //    foreach (var item in Menssagens)
                //    {
                //        System.Console.WriteLine(item);
                //    }

                //}



                var st = "OK";
                return Json(new { st });


            }
            catch (Exception ex)
            {
                var st = UtilPlay.getErro(ex);
                return Json(new { st });
            }
        }




        private IActionResult Json(object p, object allowGet)
        {
            throw new NotImplementedException();
        }

        public ActionResult StartInterface(int forceInterface, int reterros)
        {
            try
            {
                if (ParametrosSingleton.Instance.semaforoInterface != "USING")
                {
                    //chama interface
                    List<LogPlay> logResult = new List<LogPlay>();
                    InterfaceTotal InterfaceTotal = new InterfaceTotal();
                    logResult = InterfaceTotal.InterfaceStart(forceInterface);
                    if (reterros == 0)
                    {// retorna somente erros
                        var log = logResult.Where(x => x.Status != "OK").ToList();
                        return Json(new { log });
                    }
                    if (reterros == 1)
                    {// retorna tudo 
                        return Json(new { logResult });
                    }
                    return Json(new { logResult });
                }
                else
                {
                    var st = "INTERFACE EM USO.";
                    return Json(new { st });
                }
            }
            catch (Exception ex)
            {
                var st = UtilPlay.getErro(ex);
                return Json(new { st });
            }
        }

        public ActionResult testeVariavelGlobal()
        {
            try
            {
                int st = ParametrosSingleton.Instance.TimOut;
                ParametrosSingleton.Instance.TimOut = ParametrosSingleton.Instance.TimOut + 1;
                List<LogPlay> log = new List<LogPlay>();
                Carga _CargaTeste = new Carga();
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    List<object> carList = new List<object>();
                    var Db_Cargas = db.Carga.Where(C => C.CAR_ID == "M000011").FirstOrDefault();
                    carList.Add(Db_Cargas);
                    _CargaTeste.ConsolidarRomaneio(carList, ref log);
                }
                return Json(new { st, log });
            }
            catch (Exception ex)
            {
                var st = UtilPlay.getErro(ex);
                return Json(new { st });
            }
        }
    }
}
