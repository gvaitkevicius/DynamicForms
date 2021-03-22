using System.Collections.Generic;
using System.Linq;
using DynamicForms.Areas.PlugAndPlay.Util;
using DynamicForms.Context;
using DynamicForms.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class RoteirizadorController : BaseController
    {
        private readonly JSgi db;
        private MapUtil _mp;
        public RoteirizadorController()
        {
            this.db = new ContextFactory().CreateDbContext(new string[] { });
            this._mp = new MapUtil();
            //this._mp.LoadRouter();
        }
        public IActionResult Index()
        {
            return View();
        }

        public string GerarMapa(string Origem, string Destino)
        {
            List<string> startEnd = new List<string>() { Origem, Destino };
            var pontos = db.PontosMapa.AsNoTracking().Where(pm => startEnd.Contains(pm.PON_ID)).ToList();
            //var rotaGerada=_mp.GerarRota(pontos.First(), pontos.Last());
            //var rotaGerada1 = _mp.RotaGeoGson(pontos.First(), pontos.Last());
            //_mp.GravarRotaEmArquivo(rotaGerada, "PP_PV.geojson");
            //var jSon = Json(new { rotaGerada });
            TesteRota();
            return "";
        }
        public void TesteRota()
        {
            string ponId = "4305108L";
            List<string> Db_IdsPontos = db.Mapa.AsNoTracking().Where(m => m.PON_ID.Equals(ponId)).Select(m => m.PON_ID_VIZINHO).ToList();
            //Consultando os Ids de PontosMapa dos Vizinhos 
            var pontos = db.PontosMapa.AsNoTracking().Where(pm => Db_IdsPontos.Contains(pm.PON_ID)).Select(pm => pm.PON_ID).ToList();
            //Obtendo os vizinhos do ponto de origem
            var vizinhos = db.PontosMapa.Where(p => pontos.Contains(p.PON_ID)).ToList();
            var teste = _mp.RotaGeoGsonLSM(vizinhos.First(), vizinhos);
            _mp.GravarRotaEmArquivo(teste, "TSP_ROUTE.geojson");
        }
        public void GerarNovoMapa()
        {
            _mp.OSMtoItineroFomart();
        }

    }
}