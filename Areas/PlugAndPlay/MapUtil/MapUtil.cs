using DynamicForms.Areas.PlugAndPlay.Models;
using Itinero;
using Itinero.Optimization;
using Itinero.IO.Osm;
using Itinero.LocalGeo;
using Itinero.Osm.Vehicles;
using KdTree;
using KdTree.Math;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Util
{
    public class MapUtil
    {
        private Router _router = null;
        public Router Router { get { return _router; } }
        private KdTree<double, string> _KDTree;
        private List<PontosMapa> _listMunicipios;
        public MapUtil()
        {
            //Criando nova instância da Arvore KDTree para armazenar os municípios e suas coordenadas(Lat, Lon)
            //Uma (lat,lon) - str como id;nome-uf como tag de identificação
            _KDTree = new KdTree<double, string>(2, new DoubleMath());
            _listMunicipios = new List<PontosMapa>();
        }
        public List<PontosMapa> ListMunicipios
        {
            get { return _listMunicipios; }
            set
            {
                _listMunicipios = value;
                string strCidade = "";
                //Adicionando municípios da base de dados para a Árvore
                _KDTree = new KdTree<double, string>(2, new DoubleMath());
                foreach (var cidade in _listMunicipios)
                {
                    strCidade = cidade.PON_DESCRICAO + ";" + cidade.PON_ID;
                    _KDTree.Add(new[] { Convert.ToDouble(cidade.PON_LATITUDE.ToString()), Convert.ToDouble(cidade.PON_LONGITUDE.ToString()) }, strCidade);
                    strCidade = "";
                }
            }
        }
        public JsonResult ObterVizinhos(String Raio, PontosMapa cidadeOrigem)
        {
            string[] dados;
            double _raioBusca;
            try
            {
                Raio = Raio.Replace(".", ",");
                _raioBusca = (Convert.ToDouble(Raio)) / 100;
            }
            catch
            {
                _raioBusca = 0.20;
            }
            //Busca Radial na árvore a partir da origem pelo parâmetro _raioBusca
            List<PontosMapa> lista = new List<PontosMapa>();
            var vizinhosNoRaio = _KDTree.RadialSearch(new double[] { Convert.ToDouble(cidadeOrigem.PON_LATITUDE.ToString()), Convert.ToDouble(cidadeOrigem.PON_LONGITUDE.ToString()) }, _raioBusca);
            //Extraindo Dados da Estrutura dos Nós da KdTree
            foreach (var item in vizinhosNoRaio)
            {
                dados = item.Value.ToString().Split(';');
                PontosMapa p = new PontosMapa(dados[1], dados[0], "CID", Convert.ToDecimal(item.Point[0].ToString()), Convert.ToDecimal(item.Point[1]));
                lista.Add(p);
            }

            //Adicionando aos pontos a Distancia da Origem como Facilitador
            var _listAux = lista.Select(m => new
            {
                Id = m.PON_ID,
                Nome = m.PON_DESCRICAO,
                Latitude = m.PON_LATITUDE,
                Longitude = m.PON_LONGITUDE,
                DistFo = GeoUtils.Distance(Convert.ToDouble(cidadeOrigem.PON_LATITUDE.ToString()), Convert.ToDouble(cidadeOrigem.PON_LONGITUDE.ToString()), Convert.ToDouble(m.PON_LATITUDE.ToString()), Convert.ToDouble(m.PON_LONGITUDE.ToString()), 'K'),
            }).ToList();
            //Removendo primeiro item da lista pois o ponto de Origem é inclusivo na busca
            _listAux.RemoveAt(0);


            return new JsonResult(_listAux.Select(m => new
            {
                m.Id,
                m.Nome,
                m.Latitude,
                m.Longitude,
                m.DistFo,
            }).OrderBy(o => o.Nome).ToList());
        }
        public void OSMtoItineroFomart()
        {
            var routerDb = new RouterDb();
            try
            {
                string _nomeArquivo = "brazil-latest.osm.pbf";
                string _pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Mapas\", _nomeArquivo);
                using (var stream = new FileInfo(_pathFile).OpenRead())
                {
                    // create the network for cars.
                    routerDb.LoadOsmData(stream, Vehicle.BigTruck);
                }
                // write the routerdb to disk.
                _nomeArquivo = "brazil.routerdb1.2";
                _pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Mapas\", _nomeArquivo);
                using (var stream = new FileInfo(_pathFile).Open(FileMode.Create))
                {
                    routerDb.Serialize(stream);
                }
            }
            catch (Exception ex)
            {

            }

        }
        public void LoadRouter()
        {
            RouterDb routerDb = null;
            string _nomeArquivo = "brazil.routerdb";
            string _pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Mapas\", _nomeArquivo);
            using (var stream = new FileInfo(_pathFile).OpenRead())
            {
                routerDb = RouterDb.Deserialize(stream);
            }
            _router = new Router(routerDb);
        }

        public Route GerarRota(PontosMapa origem, PontosMapa destino)
        {
            // get a profile.
            var profile = Vehicle.BigTruck.Fastest(); // the default OSM car profile.

            // create a routerpoint from a location.
            // snaps the given location to the nearest routable edge.

            var start = _router.Resolve(profile, Convert.ToSingle(origem.PON_LATITUDE), Convert.ToSingle(origem.PON_LONGITUDE));
            var end = _router.Resolve(profile, Convert.ToSingle(destino.PON_LATITUDE), Convert.ToSingle(destino.PON_LONGITUDE));

            // calculate a route.
            Route route = _router.Calculate(profile, start, end);
            return route;
        }
        public string RotaGeoGson(PontosMapa origem, PontosMapa destino)
        {
            var profile = Vehicle.BigTruck.Fastest(); // the default OSM car profile.

            var start = _router.Resolve(profile, Convert.ToSingle(origem.PON_LATITUDE), Convert.ToSingle(origem.PON_LONGITUDE));
            var end = _router.Resolve(profile, Convert.ToSingle(destino.PON_LATITUDE), Convert.ToSingle(destino.PON_LONGITUDE));

            // calculate a route.
            Route route = _router.Calculate(profile, start, end);

            return route.ToGeoJson();
        }
        public Route RotaGeoGsonLSM(PontosMapa origem, List<PontosMapa> clientes)
        {
            var profile = Vehicle.BigTruck.Fastest(); // the default OSM car profile.
            List<PontosMapa> novaLista = new List<PontosMapa>();
            novaLista.Add(origem);
            novaLista.AddRange(clientes);

            Coordinate[] locations = new Coordinate[novaLista.Count()];
            for (int i = 0; i < novaLista.Count; i++)
            {
                locations[i] = new Coordinate() { Latitude = Convert.ToSingle(novaLista[i].PON_LATITUDE), Longitude = Convert.ToSingle(novaLista[i].PON_LONGITUDE) };
            }
            //Route route = _router.Calculate(profile, locations);
            Route route = _router.CalculateTSP(profile, locations);
            return route;
        }
        public void GravarRotaEmArquivo(Route route, string fileName)
        {
            try
            {
                string _pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Mapas\", fileName);
                using (var writer = new StreamWriter(_pathFile))
                {
                    route.WriteGeoJson(writer);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao GravarArquivo de Rotas:\n" + ex.Message);
            }
        }
    }
}
