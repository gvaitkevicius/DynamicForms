using System;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class ClpMedicoesController : BaseController
    {
        // GET: PlugAndPlay/ClpMedicoes
        public ActionResult Index()
        {
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                return View(db.ClpMedicoes.ToList());
            }
        }

        // GET: PlugAndPlay/ClpMedicoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/Shared/ErrorPlay.cshtml");
            }

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                ClpMedicoes clpMedicoes = db.ClpMedicoes.Find(id);
                if (clpMedicoes == null)
                {
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
                return View(clpMedicoes);
            }
        }

        // GET: PlugAndPlay/ClpMedicoes/Create
        public ActionResult Create(string MaqId, string EquId)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime dtAux = db.ClpMedicoes.AsNoTracking()
                    .Where(c => c.MaquinaId == MaqId).Select(c => c.DataFim)
                    .DefaultIfEmpty(new DateTime(1900, 1, 1, 7, 0, 0)).Max();
                ViewBag.FalgDate = (dtAux.Equals(new DateTime(1900, 1, 1, 7, 0, 0)) ? 0 : 1);
                Maquina maqAux = db.Maquina.Find(MaqId);
                ViewBag.Maq = maqAux.MAQ_ID;
                ViewBag.Equ = EquId;
                ViewBag.DataInicio = dtAux.ToString();
                ViewBag.DataEmissao = DateTime.Now.ToString(); ;
                ViewBag.Grupo = (db.ClpMedicoes.Where(c => c.MaquinaId == MaqId).Select(c => c.Grupo).DefaultIfEmpty(0).Max()) + 1;
                ViewBag.Origem = 'M';

                return View();
            }
        }

        // POST: PlugAndPlay/ClpMedicoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,MaquinaId,DataInicio,DataFim,Quantidade,Grupo,Status,TurnoId,TurmaId,OcorrenciaId,IdLoteClp,Fase,Emissao,ClpOrigem")] ClpMedicoes clpMedicoes, string MaqId, string EquId)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime dtAux = db.ClpMedicoes.AsNoTracking()
                    .Where(c => c.MaquinaId == MaqId).Select(c => c.DataFim)
                    .DefaultIfEmpty(new DateTime(1900, 1, 1, 7, 0, 0)).Max();
                ViewBag.FalgDate = (dtAux.Equals(new DateTime(1900, 1, 1, 7, 0, 0)) ? 0 : 1);
                var maqAux = db.Maquina.Find(MaqId);
                ViewBag.Maq = maqAux.MAQ_ID;
                ViewBag.DataInicio = dtAux.ToString();
                ViewBag.DataEmissao = DateTime.Now.ToString();

                double? grupoCLP = db.ClpMedicoes.AsNoTracking()
                                        .Where(c => c.MaquinaId == MaqId).Select(c => c.Grupo).DefaultIfEmpty(0).Max();
                double grupoFeedBack = db.Feedback.AsNoTracking()
                                        .Where(f => f.MaquinaId == MaqId).Select(f => f.Grupo).DefaultIfEmpty(0).Max();

                double? grupo = (grupoCLP >= grupoFeedBack) ? grupoCLP : grupoFeedBack;

                ViewBag.Grupo = grupo + 1;
                ViewBag.Origem = 'M';

                if (clpMedicoes.DataInicio > DateTime.Now)
                    ModelState.AddModelError("DataInicio", "A data/hora de início não pode ser maior que a data/hora atual");

                if (clpMedicoes.DataFim > DateTime.Now)
                    ModelState.AddModelError("DataFim", "A data/hora de fim não pode ser maior que a data/hora atual");

                if (clpMedicoes.DataFim < clpMedicoes.DataInicio)
                    ModelState.AddModelError("DataFim", "A data/hora de fim não pode ser menor que a data/hora de início");

                if (ModelState.IsValid)
                {
                    if (clpMedicoes.Quantidade <= 0) // setup
                        clpMedicoes.Fase = 1;
                    else // produzindo
                        clpMedicoes.Fase = 3;
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.ClpMedicoes.Add(clpMedicoes);
                            db.SaveChanges();

                            Maquina maquina = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID == MaqId).FirstOrDefault();
                            if (maquina != null && maquina.MAQ_TIPO_CONTADOR == 3)
                            {
                                //Executar Procedure que atualiza o painel de monitoramento caso a máquina seja de apontamento manual
                                float proximaMetaPerformance = -1;
                                string sql = "EXEC SP_PLUG_CALCULA_PERFORMANCE_FILA -1, '" + MaqId + "','','',0,'','',0," + proximaMetaPerformance.ToString() + ", '',''," + clpMedicoes.Quantidade.ToString() + " ," + clpMedicoes.Fase.ToString();
                                db.Database.ExecuteSqlCommand(sql);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro:" + ex);
                            transaction.Rollback();
                            throw;
                        }


                    }
                    return RedirectToAction("Index", "Medicoes", new { area = "PlugAndPlay", id = MaqId, idEquipe = EquId });
                }

                return View(clpMedicoes);
            }
        }

        // GET: PlugAndPlay/ClpMedicoes/Edit/5
        public ActionResult Edit(int? id)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (id == null)
                {
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
                ClpMedicoes clpMedicoes = db.ClpMedicoes.Find(id);
                if (clpMedicoes == null)
                {
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
                return View(clpMedicoes);
            }
        }

        // POST: PlugAndPlay/ClpMedicoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,MaquinaId,DataInicio,DataFim,Quantidade,Grupo,Status,TurnoId,TurmaId,OcorrenciaId,IdLoteClp,Fase,Emissao,ClpOrigem")] ClpMedicoes clpMedicoes)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(clpMedicoes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(clpMedicoes);
            }
        }

        // GET: PlugAndPlay/ClpMedicoes/Delete/5
        public ActionResult Delete(int? id)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (id == null)
                {
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
                ClpMedicoes clpMedicoes = db.ClpMedicoes.Find(id);
                if (clpMedicoes == null)
                {
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
                return View(clpMedicoes);
            }
        }

        // POST: PlugAndPlay/ClpMedicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                ClpMedicoes clpMedicoes = db.ClpMedicoes.Find(id);
                db.ClpMedicoes.Remove(clpMedicoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}