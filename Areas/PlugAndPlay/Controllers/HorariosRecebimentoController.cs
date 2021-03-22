using System;
using System.Collections.Generic;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class HorariosRecebimentoController : BaseController
    {
        private JSgi db;

        public HorariosRecebimentoController()
        {
            this.db = new ContextFactory().CreateDbContext(new string[] { });
        }

        public ActionResult Create(string idCliente)
        {
            ViewBag.Horarios = db.T_HORARIO_RECEBIMENTO.AsNoTracking().Where(x => x.CLI_ID == idCliente).ToList();
            ViewBag.idCliente = idCliente;
            ViewBag.horaInicio = "00:00";
            ViewBag.horaFim = "00:01";
            ViewBag.diaSemana = "1";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("")] T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO, IFormCollection fc)
        {
            //Domingo: 1 - Sababdo 7
            var auxDias = new List<string>() { "", "Domingo", "Segunda", "Terca", "Quarta", "Quinta", "Sexta", "Sabado" };

            List<string> dias = new List<string>();
            List<T_HORARIO_RECEBIMENTO> Db_HorariosCadastrados = db.T_HORARIO_RECEBIMENTO.AsNoTracking().Where(x => x.CLI_ID == t_HORARIO_RECEBIMENTO.CLI_ID).ToList();
            string horaInicio_1 = Convert.ToString(fc["horaUm"]);
            string horaFim_1 = Convert.ToString(fc["horaDois"]);
            string horaInicio_2 = Convert.ToString(fc["horaTres"]);
            string horaFim_2 = Convert.ToString(fc["horaQuatro"]);
            var auxDiasF = new List<string>() { "" };
            var _DiasNoFormulario = auxDiasF;
            _DiasNoFormulario.AddRange(fc.Keys.Where(f => auxDias.Contains(f)).ToList());
            foreach (var item in _DiasNoFormulario)
            {
                dias.Add((Convert.ToString(fc[item])));
            }
            dias[0] = "false";

            if (ModelState.IsValid)
            {
                List<T_HORARIO_RECEBIMENTO> lista = new List<T_HORARIO_RECEBIMENTO>();
                for (int i = 2; i < 8; i++)
                {
                    T_HORARIO_RECEBIMENTO item = new T_HORARIO_RECEBIMENTO();
                    string[] statusDia = dias.ElementAt(i).Split(',');
                    DateTime auxHoraInicial;
                    DateTime auxHoraInicial2;
                    DateTime auxHoraFinal;
                    DateTime auxHoraFinal2;
                    if (statusDia.Count() > 1)
                    {
                        if (!String.IsNullOrWhiteSpace(horaInicio_1) && !String.IsNullOrWhiteSpace(horaInicio_2))
                        {
                            item.CLI_ID = t_HORARIO_RECEBIMENTO.CLI_ID;
                            item.HRE_DIA_DA_SEMANA = i;
                            DateTime.TryParse(horaInicio_1, out auxHoraInicial);
                            DateTime.TryParse(horaFim_1, out auxHoraFinal);
                            item.HRE_HORA_INICIAL = auxHoraInicial;
                            item.HRE_HORA_FINAL = auxHoraFinal;
                            lista.Add(item);
                            item = new T_HORARIO_RECEBIMENTO
                            {
                                CLI_ID = t_HORARIO_RECEBIMENTO.CLI_ID,
                                HRE_DIA_DA_SEMANA = i
                            };
                            DateTime.TryParse(horaInicio_2, out auxHoraInicial2);
                            DateTime.TryParse(horaFim_2, out auxHoraFinal2);
                            item.HRE_HORA_INICIAL = auxHoraInicial2;
                            item.HRE_HORA_FINAL = auxHoraFinal2;
                            lista.Add(item);
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(horaInicio_1))
                            {
                                item.CLI_ID = t_HORARIO_RECEBIMENTO.CLI_ID;
                                item.HRE_DIA_DA_SEMANA = i;
                                DateTime.TryParse(horaInicio_1, out auxHoraInicial);
                                DateTime.TryParse(horaFim_1, out auxHoraFinal);
                                item.HRE_HORA_INICIAL = auxHoraInicial;
                                item.HRE_HORA_FINAL = auxHoraFinal;
                            }
                            if (!String.IsNullOrWhiteSpace(horaInicio_2))
                            {
                                DateTime.TryParse(horaInicio_2, out auxHoraInicial2);
                                DateTime.TryParse(horaFim_2, out auxHoraFinal2);
                                item.CLI_ID = t_HORARIO_RECEBIMENTO.CLI_ID;
                                item.HRE_DIA_DA_SEMANA = i;
                                item.HRE_HORA_INICIAL = auxHoraInicial2;
                                item.HRE_HORA_FINAL = auxHoraFinal2;
                            }
                            lista.Add(item);
                        }
                    }
                }
                foreach (T_HORARIO_RECEBIMENTO lhCad in Db_HorariosCadastrados)
                {
                    foreach (T_HORARIO_RECEBIMENTO horario in lista)
                    {
                        string auxLhcHInicio = lhCad.HRE_HORA_INICIAL.Hour.ToString() + ":" + lhCad.HRE_HORA_INICIAL.Minute.ToString();
                        string auxHInicio = horario.HRE_HORA_INICIAL.Hour.ToString() + ":" + horario.HRE_HORA_INICIAL.Minute.ToString();
                        if (horario.HRE_DIA_DA_SEMANA == lhCad.HRE_DIA_DA_SEMANA && auxHInicio.Equals(auxLhcHInicio))
                        {
                            lista.Remove(horario);
                        }
                    }
                }
                foreach (T_HORARIO_RECEBIMENTO horario in lista)
                {
                    db.T_HORARIO_RECEBIMENTO.Add(horario);
                    db.SaveChanges();
                }
                return RedirectToAction("Create", "HorariosRecebimento", new { area = "PlugAndPlay", idCliente = t_HORARIO_RECEBIMENTO.CLI_ID });
            }

            return View(t_HORARIO_RECEBIMENTO);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO = db.T_HORARIO_RECEBIMENTO.Where(t => t.HRE_ID == id).FirstOrDefault();
            if (t_HORARIO_RECEBIMENTO == null)
            {
                return NotFound();
            }
            return View(t_HORARIO_RECEBIMENTO);
        }

        // POST: PlugAndPlay/T_HORARIO_RECEBIMENTO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_HORARIO_RECEBIMENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "HorariosRecebimento", new { area = "PlugAndPlay", idCliente = t_HORARIO_RECEBIMENTO.CLI_ID });

            }
            return View(t_HORARIO_RECEBIMENTO);
        }

        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO = db.T_HORARIO_RECEBIMENTO.Where(t => t.HRE_ID == id).FirstOrDefault();
            if (t_HORARIO_RECEBIMENTO == null)
            {
                return NotFound();
            }
            return View(t_HORARIO_RECEBIMENTO);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO = db.T_HORARIO_RECEBIMENTO.Where(t => t.HRE_ID == id).FirstOrDefault();
            string auxIdCliente = t_HORARIO_RECEBIMENTO.CLI_ID;
            db.T_HORARIO_RECEBIMENTO.Remove(t_HORARIO_RECEBIMENTO);
            db.SaveChanges();
            return RedirectToAction("Index", "HorariosRecebimento", new { area = "PlugAndPlay", idCliente = t_HORARIO_RECEBIMENTO.CLI_ID });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            T_HORARIO_RECEBIMENTO t_HORARIO_RECEBIMENTO = db.T_HORARIO_RECEBIMENTO.Where(h => h.HRE_ID == id).FirstOrDefault();
            if (t_HORARIO_RECEBIMENTO == null)
            {
                return NotFound();
            }
            return View(t_HORARIO_RECEBIMENTO);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
