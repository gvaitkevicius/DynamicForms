using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class PontoMapaController : BaseController
    {
        private readonly JSgi db;
        private List<PontosMapa> Db_PontosMapa;
        public PontoMapaController()
        {
            this.db = new ContextFactory().CreateDbContext(new string[] { });
            //var teste = db.Municipio.AsNoTracking().Where(m => m.UF_COD == "RS").Select(x => x.MUN_NOME).ToList();
            //.Where(pm=> teste.Contains(pm.PON_DESCRICAO))
            this.Db_PontosMapa = db.PontosMapa.AsNoTracking().Take(100).ToList();

        }
        public IActionResult Index(string searchString)
        {
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                this.Db_PontosMapa = Db_PontosMapa.Where(c => (c.PON_DESCRICAO).ToUpper().Contains(searchString.ToUpper()) || c.PON_DESCRICAO.Contains(searchString.ToUpper())).ToList();
                if (Db_PontosMapa.Count == 0)
                {
                    this.Db_PontosMapa = db.PontosMapa.AsNoTracking().Where(c => (c.PON_DESCRICAO).ToUpper().Contains(searchString.ToUpper()) || c.PON_DESCRICAO.Contains(searchString.ToUpper())).ToList();
                }
            }
            return View(Db_PontosMapa);
        }
        public ActionResult MapUI(string id)
        {
            if (id == null)
            {
                return new StatusCodeResult(404);
            }
            PontosMapa pontoMapa = db.PontosMapa.AsNoTracking().Where(c => c.PON_ID.Equals(id)).FirstOrDefault();
            if (pontoMapa == null)
            {
                return NotFound();
            }
            return View(pontoMapa);
        }
        public IActionResult PontosEntrega(string data, string carga, string modo)
        //public JsonResult PontosEntrega(string data, string carga,string modo) //Sexual Hands - Krupck (MAPAS)
        {
            string idPontoORigem = db.Param.AsNoTracking().Where(p => p.PAR_ID.Equals("EXPEDICAO_ID_PONTO_MAPA_ORIGEM")).Select(p => p.PAR_VALOR_S).FirstOrDefault();
            PontosMapa pontoMapa = db.PontosMapa.AsNoTracking().Where(c => c.PON_ID.Equals(idPontoORigem)).FirstOrDefault();

            if (pontoMapa == null)
            {
                return NotFound();
                //var msg = "Erro, erro, erro";
                //return Json(msg);
            }
            if (data.Equals("undefined"))
            {
                data = DateTime.Now.ToShortDateString();
            }
            DateTime aux = DateTime.ParseExact(data, @"dd/MM/yyyy", CultureInfo.InvariantCulture);
            PontosEntrega pontosEntrega = new PontosEntrega()
            {
                PON_ID = pontoMapa.PON_ID,
                PON_DESCRICAO = pontoMapa.PON_DESCRICAO,
                PON_LATITUDE = pontoMapa.PON_LATITUDE,
                PON_LONGITUDE = pontoMapa.PON_LONGITUDE,
                CAR_ID = carga,
                CAR_EMBARQUE_ALVO = aux,
                Modo = modo
            };
            //return Json(pontosEntrega);
            return View(pontosEntrega);
        }
        /// <summary>
        /// Obtem todos os pontos de entrega para uma data ou uma carga específica.
        /// </summary>
        /// <param name="dataAtual"></param> Data prevista das cargas
        /// <param name="idCarga"></param> carga desejada  
        /// <param name="modo"></param> 1 somente data, dois somente carga
        /// <returns></returns>

        public JsonResult ObterPontosEntrega(string dataAtual, string idCarga, string modo)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime dateAux = Convert.ToDateTime(dataAtual);

                if (modo.Equals("1"))
                {
                    var Db_pontosEntrega = (from pe in db.PontosEntrega where (pe.CAR_EMBARQUE_ALVO >= dateAux && pe.CAR_EMBARQUE_ALVO < dateAux.AddDays(1)) select pe).ToList();
                    return Json(new { Db_pontosEntrega });
                }
                else if(modo.Equals("2"))
                {
                    var Db_pontosEntrega = db.PontosEntrega.AsNoTracking().Where(p => p.CAR_ID.Equals(idCarga)).ToList();
                    return Json(new { Db_pontosEntrega });
                }
                else //Vai pesquisar por PON_ID
                {
                    string st = "OK";
                    return Json(new { st });
                }

            }
        }
        public JsonResult BuscarMapasDoPonto(string ponId)
        {
            List<string> Db_IdsPontos = db.Mapa.AsNoTracking().Where(m => m.PON_ID.Equals(ponId)).Select(m => m.PON_ID_VIZINHO).ToList();
            //Consultando os Ids de PontosMapa dos Vizinhos 
            var pontos = db.PontosMapa.AsNoTracking().Where(pm => Db_IdsPontos.Contains(pm.PON_ID)).Select(pm => pm.PON_ID).ToList();
            //Obtendo os vizinhos do ponto de origem
            var vizinhos = db.PontosMapa.Where(p => pontos.Contains(p.PON_ID)).ToList();

            var jSon = Json(new { vizinhos });
            return jSon;
        }
    }
}