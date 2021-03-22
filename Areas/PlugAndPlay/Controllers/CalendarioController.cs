using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class CalendarioController : BaseController
    {
        private readonly JSgi db;
        public CalendarioController()
        {
            this.db = new ContextFactory().CreateDbContext(new string[] { });
        }
        public IActionResult Index()
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(CalendarioController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            return View(db.Calendario.AsNoTracking().ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Calendario calendario = db.Calendario.Where(c => c.CAL_ID == id).FirstOrDefault();
            if (calendario == null)
            {
                return NotFound();
            }
            return View(calendario);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Calendario calendario)
        {
            if (ModelState.IsValid)
            {
                db.Calendario.Add(calendario);
                db.SaveChanges();
                return RedirectToAction("Index", "Calendario", new { area = "PlugAndPlay" });
            }
            return View(calendario);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Calendario calendario = db.Calendario.Where(c => c.CAL_ID == id).FirstOrDefault();
            if (calendario == null)
            {
                return NotFound();
            }
            return View(calendario);
        }


        [HttpPost]
        public ActionResult Edit(Calendario calendario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Calendario", new { area = "PlugAndPlay" });

            }
            return View(calendario);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            Calendario calendario = db.Calendario.Where(c => c.CAL_ID == id).FirstOrDefault();

            if (calendario == null)
            {
                return NotFound();
            }
            return View(calendario);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Calendario calendario = db.Calendario.Where(c => c.CAL_ID == id).FirstOrDefault();
            var Db_itensRemover = db.ItensCalendario.Where(ic => ic.CAL_ID == id).ToList();
            db.ItensCalendario.RemoveRange(Db_itensRemover);
            db.Calendario.Remove(calendario);
            db.SaveChangesAsync();
            return RedirectToAction("Index", "Calendario", new { area = "PlugAndPlay" });
        }

    }

}
