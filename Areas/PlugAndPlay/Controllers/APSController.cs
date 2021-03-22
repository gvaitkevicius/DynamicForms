using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CromulentBisgetti.ContainerPacking;
using CromulentBisgetti.ContainerPacking.Entities;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using OptMiddleware;
using OptShered;
using OptTransport;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class APSController : BaseController
    {
        public DateTime DataInicial;
        public T_Usuario usuario_logado;
        private static List<ExpandoObject> listaMP;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public APSController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        // GET: PlugAndPlay/FilaProducao
        public ActionResult Index()
        {
            usuario_logado = ObterUsuarioLogado(); //Obtem somente informações básicas
            ViewBag.UserName = usuario_logado.USE_NOME;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(usuario_logado, typeof(APSController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //ViewBag.m = db.SqlQuery<T_MAQUINAS>("select MAQ_ID ,MAQ_DESCRICAO from T_MAQUINA ").ToList();
                //ViewBag.m = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID.Substring(0, 3) != "SRV").Select(x => new Maquina { MAQ_ID = x.MAQ_ID, MAQ_DESCRICAO = x.MAQ_DESCRICAO }).ToList();
                List<Maquina> maquinas = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID == "PLAYSIS" || (x.CAL_ID != null && x.MAQ_ID.Substring(0, 3) != "SRV")).Select(x => new Maquina { MAQ_ID = x.MAQ_ID, MAQ_DESCRICAO = x.MAQ_DESCRICAO, MAQ_HIERARQUIA_SEQ_TRANSFORMACAO = x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO }).ToList();
                List<Maquina> equipes = db.Equipe.AsNoTracking().OrderBy(x => x.EQU_ID).Select(x => new Maquina { EQU_ID = x.EQU_ID, MAQ_HIERARQUIA_SEQ_TRANSFORMACAO = x.EQU_HIERARQUIA_SEQ_TRANSFORMACAO }).ToList();

                List<Maquina> maquinas_equipes = maquinas;
                maquinas_equipes.AddRange(equipes);
                maquinas_equipes = maquinas_equipes.OrderBy(x => x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO).ToList();

                ViewBag.m = maquinas_equipes;
                ViewBag.json = null;

            }
            return View();
        }



        //public JsonResult ObterFilaExpedicao(string recalculaFila, int privilegiar, DateTime dataFiltro, bool execInterface)
        [HttpPost]
        public JsonResult ObterFilaExpedicao(int? MaxDiasAntecipacao, string diasAfrente, string recalculaFila, string cargaId, string dtEmbarque, string status, string query_json)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    Exception e = null;
                    if (recalculaFila == "sim")
                    {
                        //e = OptFunctions.FilaExpedicao(db, MaxDiasAntecipacao, diasAfrente,true, 0);
                    }
                    if (e == null)
                    {
                        db.Database.SetCommandTimeout(120000);
                        DateTime DtIni = DateTime.Now;
                        //1 - ESTUDO;2 - CONSOLIDADA;3 - AGENCIADA;4 - PIKING;5 - CARREGANDO ;6 - DESPACHADA;7 - ENTREGUE PARCIAL;8 - ENTREGUE TOTAL. 
                        IOrderedEnumerable<CargasWeb> cargas;
                        IEnumerable<CargasWeb> cargasWhere = null; //caso seja preciso realizar um segundo where nas consultas, se utiliza essa variavel
                        List<CargasWeb> cargasPlanilha = null;
                        string st;
                        //query_json tarz um filtro personalizado pelo usuario para selecionar as cargas
                        if (query_json != "undefined" && query_json != null)
                        {
                            MasterController mc = new MasterController();
                            StringBuilder str_query = new StringBuilder();

                            EstruturaQuery estruturaQuery = JsonConvert.DeserializeObject<EstruturaQuery>(query_json);
                            //Teste
                            Type type = Type.GetType(estruturaQuery.ClassName);
                            IEntityType entityType = db.Model.FindEntityType(estruturaQuery.ClassName);
                            string tableName = entityType.Relational().TableName;
                            string namespaceClass = estruturaQuery.ClassName;

                            if (estruturaQuery.Filters != null && estruturaQuery.Filters.Count > 0)
                            {
                                string str_where = mc.GetCondicaoWhere(estruturaQuery.Filters, estruturaQuery.Operators, entityType, type);
                                str_query.Append($"select * from {tableName} where(CAR_ID_JUNTADA = '' AND {str_where}) ");
                            }
                            else
                            {
                                //double dias = db.Param.AsNoTracking().Where(p => p.PAR_ID == "FILA_EXPEDICAO_NUMERO_DIAS_TELA").FirstOrDefault().PAR_VALOR_N;
                                double dias = 30;
                                str_query.Append($"select * from {tableName}  where CAR_STATUS < 6 " +
                                    $" AND DTEMBARQUE < '" + DateTime.Now.AddDays(dias).ToString("yyyyMMdd HH:mm:ss") + "'");
                            }

                            cargas = db.CargasWeb
                                .AsNoTracking()
                                .FromSql(str_query.ToString())
                                .ToList()
                                .OrderBy(o => o.EMBARQUE).ThenBy(oo => oo.CAR_ID).ThenByDescending(ooo => ooo.FIMPREVISTO)
                                ;
                            //cargasPlanilha = new List<CargasWeb>(cargas);
                            cargasWhere = cargas;

                            foreach (var item in cargasWhere)
                            {
                                item.ORD_PESO_UNITARIO = Convert.ToDouble(item.ORD_PESO_UNITARIO) / 1000;
                            }

                            //cargasPlanilha.OrderBy(o => o.EMBARQUE).ThenBy(x => x.UF).ThenBy(xx => xx.PON_DESCRICAO).ThenBy(xxx => xxx.ORD_REGIAO_ENTREGA).ThenBy(xxx => xxx.CLI_NOME).ThenBy(xxx => xxx.CAR_ID).ThenByDescending(xxx => xxx.FIMPREVISTO);
                            //cargasWhere.OrderBy(o => o.EMBARQUE).ThenBy(x => x.UF).ThenBy(xx => xx.PON_DESCRICAO).ThenBy(xxx => xxx.ORD_REGIAO_ENTREGA).ThenBy(xxx => xxx.CLI_NOME).ThenBy(xxx => xxx.CAR_ID).ThenByDescending(xxx => xxx.FIMPREVISTO);

                            cargasWhere = cargasWhere
                                .OrderBy(xxx => xxx.EMBARQUE)
                                .ThenBy(xxx => xxx.CAR_ID)
                                .ThenBy(xxx => xxx.UF)
                                .ThenBy(xxx => xxx.PON_DESCRICAO)
                                .ThenBy(xxx => xxx.ORD_REGIAO_ENTREGA)
                                .ThenBy(xxx => xxx.CAR_DATA_INICIO_PREVISTO)
                                .ThenBy(xxx => xxx.CLI_NOME)
                                ;

                            //necessário, pois ao trazer as planilhas sem o where, ele coloca as cargas planilhas erradas
                            cargasPlanilha = cargasWhere.ToList();

                            db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                            st = "OK";
                        }
                        else
                        {
                            st = "ERROR";
                        }

                        var j = Json(new { st, cargasPlanilha, cargasWhere });
                        return j;
                    }
                    else
                    {
                        var st = UtilPlay.getErro(e);
                        return Json(new { st });
                    }
                }
                catch (Exception e)
                {
                    var st = UtilPlay.getErro(e);
                    return Json(new { st });
                    throw;
                }
            }
        }

        [HttpGet]
        public IActionResult ObterCargaMaquina(string nivel, string _data)
        {
            //string st = "OK";
            //return Json(new { st });

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    if (nivel == "n1")
                    {
                        /* MODELO ANTIGO
                         var cargaMaquinaDia = db.CargaMaquina.AsNoTracking().GroupBy(x => new { x.DIA_FIM_PREVISTO, x.STATUS, x.TEMPO })
                            .Select(l => new CargaMaquina { DIA_FIM_PREVISTO = l.Key.DIA_FIM_PREVISTO, STATUS = l.Key.STATUS, TEMPO = l.Sum(i => i.TEMPO) })
                            .ToList();
                        */

                        var cargaMaquinaDia2 = db.CargaMaquina.AsNoTracking().GroupBy(x => new { x.MED_DATA })
                            .Select(l => new CargaMaquina
                            {
                                MED_DATA = l.Key.MED_DATA,
                                ATRASO = Math.Round((double)l.Sum(i => i.ATRASO), 2),
                                NA_DATA = Math.Round((double)l.Sum(i => i.NA_DATA), 2),
                                ADIANTADO = Math.Round((double)l.Sum(i => i.ADIANTADO), 2),
                                OCIOSO = Math.Round((double)l.Sum(i => i.OCIOSO), 2),
                                DISPONIVEL = Math.Round((double)l.Sum(i => i.DISPONIVEL), 2),
                                M2_MAQUINA_DIA = Math.Round((double)l.Sum(i => i.M2_MAQUINA_DIA)),
                                M2_ACABADO_DIA = Math.Round((double)l.Sum(i => i.M2_ACABADO_DIA)),
                                PESO_MAQUINA_DIA = Math.Round((double)l.Sum(i => i.PESO_MAQUINA_DIA)),
                                PESO_ACABADO_DIA = Math.Round((double)l.Sum(i => i.PESO_ACABADO_DIA))

                            }).ToList();


                        return Json(new { cargaMaquinaDia2 });
                    }
                    if (nivel == "n2")
                    {
                        /* MODELO ANTIGO
                            db.Database.SetCommandTimeout(120000);
                            DateTime DtIni = DateTime.Now;
                            var cargaMaquinaDia = db.CargaMaquina.GroupBy(x => new { x.DIA_FIM_PREVISTO, x.GRP_ID, x.GRP_DESCRICAO, x.STATUS })
                                .Select(l => new { l.Key.DIA_FIM_PREVISTO, l.Key.GRP_ID, l.Key.GRP_DESCRICAO, l.Key.STATUS, TEMPO = l.Sum(i => i.TEMPO) })
                                .OrderBy(x => x.DIA_FIM_PREVISTO).ThenBy(x => x.GRP_ID).ToList();
                         */

                        var cargaMaquinaDia2 = db.CargaMaquina.AsNoTracking().GroupBy(x => new { x.TIPO, x.DIM_ID, x.MAQ_DESCRICAO, x.MED_DATA, x.GMA_ID, x.ATRASO, x.NA_DATA, x.ADIANTADO, x.OCIOSO, x.DISPONIVEL, x.MAQ_ID, x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO, x.M2_ACABADO_DIA, x.M2_MAQUINA_DIA, x.PESO_ACABADO_DIA, x.PESO_MAQUINA_DIA })
                            .Select(l => new CargaMaquina
                            {
                                DIM_ID = l.Key.DIM_ID,
                                MAQ_DESCRICAO = l.Key.MAQ_DESCRICAO,
                                MED_DATA = l.Key.MED_DATA,
                                GMA_ID = l.Key.GMA_ID,
                                ATRASO = l.Key.ATRASO,
                                NA_DATA = l.Key.NA_DATA,
                                ADIANTADO = l.Key.ADIANTADO,
                                DISPONIVEL = l.Key.DISPONIVEL,
                                OCIOSO = l.Key.OCIOSO,
                                TIPO = l.Key.TIPO,
                                MAQ_HIERARQUIA_SEQ_TRANSFORMACAO = l.Key.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO,
                                MAQ_ID = l.Key.MAQ_ID,
                                M2_ACABADO_DIA = l.Key.M2_ACABADO_DIA,
                                M2_MAQUINA_DIA = l.Key.M2_MAQUINA_DIA,
                                PESO_ACABADO_DIA = l.Key.PESO_ACABADO_DIA,
                                PESO_MAQUINA_DIA = l.Key.PESO_MAQUINA_DIA
                            }).OrderByDescending(x => x.TIPO).ThenBy(x => x.MAQ_HIERARQUIA_SEQ_TRANSFORMACAO).ThenBy(x => x.MAQ_ID).ToList();


                        return Json(new { cargaMaquinaDia2 });
                    }
                    if (nivel == "n3")
                    {
                        /*
                        db.Database.SetCommandTimeout(120000);
                        DateTime DtIni = DateTime.Now;
                        var cargaMaquinaDia = db.CargaMaquina.Where(x => x.DIA_FIM_PREVISTO == data && x.GRP_ID == grupo)
                            .OrderBy(x => x.DIA_FIM_PREVISTO).ThenBy(x => x.STATUS).ToList();
                        */

                        string[] data = _data.Split("-")[0].Split("_");
                        DateTime datetime = new DateTime(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));

                        string tipo = _data.Split("-")[2];
                        string maquina = _data.Split("-")[1];
                        if (tipo.Equals("E"))
                        {
                            var cargaMaquinaPedidos = db.ViewCargaMaquinasPedidos.AsNoTracking()
                                .Where(
                                    x => (x.FPR_DATA_INICIO_PREVISTA.Date.Equals(datetime) ||
                                    x.FPR_DATA_FIM_PREVISTA.Date.Equals(datetime)) &&
                                    x.EQU_ID == maquina
                                    )
                                .OrderBy(i => i.FPR_DATA_INICIO_PREVISTA)
                                .ToList();

                            var somatorio = cargaMaquinaPedidos.Sum(x => x.ORD_M2_TOTAL).Value;
                            return Json(new { cargaMaquinaPedidos, somatorio });
                        }
                        else// if(tipo.Equals("M"))
                        {
                            var cargaMaquinaPedidos = db.ViewCargaMaquinasPedidos.AsNoTracking()
                                .Where(
                                    x => (x.FPR_DATA_INICIO_PREVISTA.Date.Equals(datetime) ||
                                    x.FPR_DATA_FIM_PREVISTA.Date.Equals(datetime)) &&
                                    x.ROT_MAQ_ID == maquina
                                    )
                                .OrderBy(i => i.FPR_DATA_INICIO_PREVISTA)
                                .ToList();

                            var somatorio = cargaMaquinaPedidos.Sum(x => x.ORD_M2_TOTAL).Value;
                            return Json(new { cargaMaquinaPedidos, somatorio });
                        }
                        //else// if(tipo.Equals("M"))

                        //j.MaxJsonLength = 900000000;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    var fila = UtilPlay.getErro(e);
                    return Json(fila);
                    throw;
                }
            }
        }

        public JsonResult AlterarStatusCarga(int status, string cargas)
        {
            string st = "";
            List<LogPlay> Logs = new List<LogPlay>();
            List<string> listaCarid = new List<string>();
            Carga _cargaAux = new Carga();
            var tempList = cargas.Split(",");
            foreach (var item in tempList)
            {
                listaCarid.Add(item);
            }

            if (listaCarid.Count > 0)
            {
                _cargaAux.AlterarStatusCarga(listaCarid, status, ref Logs);
                return Json(new { status, cargas });
            }
            else
            {
                //Deu Bosta
                return Json(new { status, cargas });
            }

        }


        [HttpGet]
        public JsonResult SimularPedidoCargaMaquina(string pro_id, string data_entrega_str, int quantidade, string cli_id)
        {
            string status = "ADIAR"; //"ADIAR" //"ERRO"
            string msgRetorno = "";

            List<V_OPS_A_PLANEJAR> logs = new List<V_OPS_A_PLANEJAR>();

            //Conversão da data_fim que vem em formato string em um DateTime;
            string[] data_aux1 = data_entrega_str.Split("-");
            string[] data_aux2 = data_aux1[2].Split("T");
            //string[] hora_aux = data_aux2[1].Split(":");
            DateTime data_fim = new DateTime(
                Convert.ToInt32(data_aux1[0]), //Year
                Convert.ToInt32(data_aux1[1]), //Month
                Convert.ToInt32(data_aux2[0]), //Day
                23, 55, 00);
            DateTime prazoMinimo = DateTime.Now;

            List<object[]> embarque = new List<object[]>();
            DateTime inicioJanelaEmbarque = DateTime.Now;
            DateTime fimJanelaEmbarque = DateTime.Now;
            DateTime embarqueAlvo = DateTime.Now;

            bool flag = UtilPlay.SimularPedido(pro_id, quantidade, cli_id, ref status, ref prazoMinimo, ref msgRetorno, ref data_fim, ref logs, ref embarque);


            inicioJanelaEmbarque = Convert.ToDateTime(embarque[0][0]);
            fimJanelaEmbarque = Convert.ToDateTime(embarque[0][1]);
            embarqueAlvo = Convert.ToDateTime(embarque[0][2]);
            return Json(new { status, prazoMinimo, msgRetorno, data_fim, logs, inicioJanelaEmbarque, fimJanelaEmbarque, embarqueAlvo });
        }

        [HttpPost]
        public JsonResult setReserva(string lc_estoque_lote, string lc_pedidos_futuros)
        {
            #region Convertendo Lotes em formato Json retornados do formulario
            List<SaldosEmEstoquePorLote> lotesParaAproveitar = JsonConvert.DeserializeObject<List<SaldosEmEstoquePorLote>>(lc_estoque_lote);
            List<ViewPedidosFuturos> pedidosFuturos = JsonConvert.DeserializeObject<List<ViewPedidosFuturos>>(lc_pedidos_futuros);
            #endregion Convertendo Lotes em formato Json retornados do formulario

            string msg = "";

            #region Validando tipo do pedido
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                bool EhPedidoKanban = db.Order.AsNoTracking().Any(x => x.ORD_ID.Equals(pedidosFuturos.First().ORD_ID) && x.ORD_TIPO == 2);
                if (EhPedidoKanban)
                {
                    msg = "Você não pode realizar uma reserva para um pedido do tipo (2) Kanban.\n";
                    return Json(new { msg });
                }
            }
            #endregion Validando tipo do pedido

            MovimentoEstoqueReservaDeEstoque auxReserva = new MovimentoEstoqueReservaDeEstoque();
            var reservasdoAproveitamento = auxReserva.TransferirLotesReservados(lotesParaAproveitar, pedidosFuturos.First().ORD_ID);

            #region Persistindo alteraçoes das reservas
            var aux = new List<object>();
            aux.AddRange(reservasdoAproveitamento);

            var objetosParaPersistir = new List<List<object>>() { aux };

            MasterController mc = new MasterController() { UsuarioLogado = ObterUsuarioLogado() };
            List<LogPlay> logs = mc.UpdateData(objetosParaPersistir, 0, true);
            #endregion

            #region Gerar etiqueta dos lotes
            Etiqueta auxEtiqueta = new Etiqueta();
            List<Etiqueta> etiquetas = new List<Etiqueta>();
            if (!logs.Any(x => x.Status.Equals("ERRO")))
            {
                for (int i = 0; i < lotesParaAproveitar.Count; i++)
                {
                    var etiqueta = auxEtiqueta.GerarEtiquetaLoteProduto(lotesParaAproveitar[i].PRO_ID, lotesParaAproveitar[i].MOV_LOTE,
                        lotesParaAproveitar[i].MOV_SUB_LOTE, 1, logs, ObterUsuarioLogado().USE_ID);

                    etiquetas.Add(etiqueta);
                }
            }
            var idEtiquetas = String.Join(",", etiquetas.Select(x => x.ETI_ID).ToList());
            #endregion

            msg = (logs.Any(x => x.Status.Equals("ERRO"))) ? msg += String.Join("\n", logs.Where(x => x.Status.Equals("ERRO")).Select(x => x.MsgErro).ToList()) : "Aproveitamento Realizado com sucesso!";
            return Json(new { msg, idEtiquetas });
        }

        [HttpGet]
        public JsonResult ObterLotesPedFutPA(string pro_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //var estoque_lote = db.SaldosEmEstoquePorLote.Where(x => x.PRO_ID == pro_id && x.MOV_APROVEITAMENTO != "S" && x.SALDO > 0).ToList();
                var estoque_lote = db.V_PEDIDOS_COM_LOTES_DISPONIVEIS.Where(x => x.PRO_ID == pro_id).ToList();
                var pedidos_futuros = db.ViewPedidosFuturos.Where(x => x.PRO_ID.Equals(pro_id)).ToList();
                return Json(new { estoque_lote, pedidos_futuros });
            }
        }

        [HttpGet]
        public JsonResult ObterLotesPedFutMP(string pro_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //var TIPO_GRP_PROD = db.Produto.Include(x => x.GrupoProduto).AsNoTracking().Where(x => x.PRO_ID == pro_id).Select(x => x.GrupoProduto.GRP_TIPO).FirstOrDefault();
                var estoque_lote = db.SaldosEmEstoquePorLote.Where(x => x.PRO_ID == pro_id && x.MOV_APROVEITAMENTO != "S").ToList();
                var pedidos_futuros = db.ViewPedidosFuturos.Where(x => x.PRO_ID.Equals(pro_id)).ToList();
                return Json(new { estoque_lote, pedidos_futuros });
            }
        }

        [HttpGet]
        public JsonResult OPT()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    DateTime Agora = DateTime.Now;
                    List<Mensagem> Menssagens = new List<Mensagem>();
                    OptQueueTransport t_Otimizado = null;
                    sheOptQueueBox b = new sheOptQueueBox();
                    sheOptQueueTransport t = new sheOptQueueTransport();
                    OPTMiddleware run = new OPTMiddleware(Agora, 1, "", 0, b, t, null, null, ref t_Otimizado);

                    return Json(new { st = "OK", result = Menssagens });
                }
                catch (Exception e)
                {
                    return Json(new { erro = e.Message });
                    throw;
                }
            }
        }

        [HttpGet]
        public JsonResult ObterLotesPedFutPI(string pro_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var estoque_lote = db.SaldosEmEstoquePorLote.AsNoTracking().Where(x => x.PRO_ID == pro_id && x.MOV_APROVEITAMENTO != "S").ToList();
                var pedidos_futuros = db.ViewPedidosFuturosPI.Where(x => x.PRO_ID.Equals(pro_id)).ToList();

                return Json(new { estoque_lote, pedidos_futuros });
            }
        }

        [HttpGet]
        public JsonResult ObterEstoquePA(string nivel, string grupo, string grp_id)
        {
            try
            {
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    DateTime oDate = DateTime.Now;
                    string st = "Erro ao consultar estoques";
                    //Grupos TIPO: CAIXA / CHAPA
                    var grp = grupo.Equals("CX") ? new List<double> { 3, 4 } : new List<double> { 1, 2, 5 };
                    switch (nivel)
                    {
                        case "N1":
                            var listaN1 = db.ViewEstoquePA.GroupBy(x => new { GRP_TIPO = x.GRP_TIPO == 2 ? "CHAPA" : "CAIXAS" })
                                .Select(c => new
                                {
                                    GRP_TIPO = c.Key.GRP_TIPO,
                                    DISPONIVEL = Math.Round(c.Sum(z => z.DISPONIVEL)),
                                    COMPROMISSADO = Math.Round(c.Sum(z => z.COMPROMISSADO)),
                                    SOBRA_PRODUCAO = Math.Round(c.Sum(z => z.SOBRA_PRODUCAO)),
                                    SOBRA_EXPEDICAO = Math.Round(c.Sum(z => z.SOBRA_EXPEDICAO)),
                                    DEVOLUCAO = Math.Round(c.Sum(z => z.DEVOLUCAO)),
                                    PEDIDOS_FUTUROS = Math.Round(c.Sum(z => z.PEDIDOS_FUTUROS)),
                                    SALDO_RETIDO = Math.Round(c.Sum(z => z.SALDO_RETIDO))
                                }).ToList();

                            return Json(new { listaN1 });
                        case "N2":
                            var listaN2 = db.ViewEstoquePA.Where(x => x.GRP_ID != grp_id && grp.Contains(x.GRP_TIPO)).GroupBy(x => new { GRP_ID = x.GRP_ID, GRP_DESCRICAO = x.GRP_DESCRICAO })
                                .Select(c => new
                                {
                                    GRP_ID = c.Key.GRP_ID,
                                    GRP_DESCRICAO = c.Key.GRP_DESCRICAO,
                                    DISPONIVEL = Math.Round(c.Sum(z => z.DISPONIVEL)),
                                    COMPROMISSADO = Math.Round(c.Sum(z => z.COMPROMISSADO)),
                                    SOBRA_PRODUCAO = Math.Round(c.Sum(z => z.SOBRA_PRODUCAO)),
                                    SOBRA_EXPEDICAO = Math.Round(c.Sum(z => z.SOBRA_EXPEDICAO)),
                                    DEVOLUCAO = Math.Round(c.Sum(z => z.DEVOLUCAO)),
                                    PEDIDOS_FUTUROS = Math.Round(c.Sum(z => z.PEDIDOS_FUTUROS)),
                                    SALDO_RETIDO = Math.Round(c.Sum(z => z.SALDO_RETIDO))
                                }).ToList();

                            return Json(new { listaN2 });
                        case "N3":
                            var listaN3 = db.ViewEstoquePA.Where(x => x.GRP_ID == grp_id).GroupBy(x => new { GRP_ID = x.GRP_ID, PRO_ID = x.PRO_ID, PRO_DESCRICAO = x.PRO_DESCRICAO })
                                .Select(c => new
                                {
                                    GRP_ID = c.Key.GRP_ID,
                                    PRO_ID = c.Key.PRO_ID,
                                    PRO_DESCRICAO = c.Key.PRO_DESCRICAO,
                                    DISPONIVEL = Math.Round(c.Sum(z => z.DISPONIVEL)),
                                    COMPROMISSADO = Math.Round(c.Sum(z => z.COMPROMISSADO)),
                                    SOBRA_PRODUCAO = Math.Round(c.Sum(z => z.SOBRA_PRODUCAO)),
                                    SOBRA_EXPEDICAO = Math.Round(c.Sum(z => z.SOBRA_EXPEDICAO)),
                                    DEVOLUCAO = Math.Round(c.Sum(z => z.DEVOLUCAO)),
                                    PEDIDOS_FUTUROS = Math.Round(c.Sum(z => z.PEDIDOS_FUTUROS)),
                                    SALDO_RETIDO = Math.Round(c.Sum(z => z.SALDO_RETIDO))
                                }).ToList();

                            //var listaPedidos = db.ViewPedidosFuturos.Where(x => x.PRO_ID == listaN3[0].PRO_ID).ToList();

                            return Json(new { listaN3 });
                        default:
                            return Json(new { st });
                    }
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
        [HttpGet]
        public JsonResult ObterEstoqueMP(string dataDe, string dataAte)
        {
            string dtDe = dataDe != null ? dataDe :
                "1900-01-01 00:00:00.000";

            string dtAte = dataAte != null ? dataAte :
                "2200-01-01 00:00:00.000";

            string sql = "SELECT " +
                    "I.GRP_ID " +
                    ",PRO_ID_COMPONENTE " +
                    ",I.PRO_DESCRICAO " +
                    ",SUM(CASE WHEN  FPR_DATA_INICIO_PREVISTA >= '" + dtDe + "' AND FPR_DATA_FIM_PREVISTA <= '" + dtAte + "' THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_DO_PERIODO " +
                    ",SUM(CASE WHEN    CONVERT(DATE, FPR_DATA_INICIO_PREVISTA,112) = CONVERT(DATE, GETDATE(),112) OR CONVERT(DATE, FPR_DATA_FIM_PREVISTA,112) = CONVERT(DATE, GETDATE(),112) THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_HOJE " +
                    ",SUM(CASE WHEN    CONVERT(DATE, FPR_DATA_INICIO_PREVISTA,112) = CONVERT(DATE, GETDATE()+1,112) OR CONVERT(DATE, FPR_DATA_FIM_PREVISTA,112) = CONVERT(DATE, GETDATE()+1,112) THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_AMANHA " +
                    ",SUM(CASE WHEN  FPR_DATA_INICIO_PREVISTA <= CONVERT(DATE, GETDATE()+5,112)  OR  FPR_DATA_FIM_PREVISTA <= CONVERT(DATE, GETDATE()+5,112) THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_CINCO " +
                    ",SUM(CASE WHEN  FPR_DATA_INICIO_PREVISTA <= CONVERT(DATE, GETDATE()+10,112)  OR  FPR_DATA_FIM_PREVISTA <= CONVERT(DATE, GETDATE()+10,112) THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_DEZ " +
                    ",SUM(CASE WHEN  FPR_DATA_INICIO_PREVISTA <= CONVERT(DATE, GETDATE()+15,112)  OR  FPR_DATA_FIM_PREVISTA <= CONVERT(DATE, GETDATE()+15,112) THEN EST_QUANT * FPR_QUANTIDADE_PREVISTA ELSE 0 END )CONSUMO_QUINZE " +
                    ",SUM(EST_QUANT * FPR_QUANTIDADE_PREVISTA) CONSUMO_PREVISTO " +
                "FROM V_FILA_PRODUCAO F " +
                "INNER JOIN T_ESTRUTURA_PRODUTO E ON F.PC_PRO_ID = PRO_ID_PRODUTO " +
                "INNER JOIN T_PRODUTOS I ON I.PRO_ID = PRO_ID_COMPONENTE " +
                "GROUP BY I.GRP_ID, PRO_ID_COMPONENTE, I.PRO_DESCRICAO ";

            List<object[]> lista_estoque = new List<object[]>();
            object[] temp_obj;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;

                        db.Database.OpenConnection();

                        using (DbDataReader result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                temp_obj = new object[10];
                                for (int i = 0; i < 10; i++)
                                {
                                    var valor = result.GetValue(i);
                                    temp_obj[i] = valor;
                                }
                                lista_estoque.Add(temp_obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }

            listaMP = new List<ExpandoObject>();
            foreach (var item in lista_estoque)
            {
                dynamic objeto = new ExpandoObject();
                objeto.GRP_ID = item[0];
                objeto.PRO_ID_COMPONENTE = item[1];
                objeto.PRO_DESCRICAO = item[2];
                objeto.CONSUMO_DO_PERIODO = item[3];
                objeto.CONSUMO_HOJE = item[4];
                objeto.CONSUMO_AMANHA = item[5];
                objeto.CONSUMO_CINCO = item[6];
                objeto.CONSUMO_DEZ = item[7];
                objeto.CONSUMO_QUINZE = item[8];
                objeto.CONSUMO_PREVISTO = item[9];

                listaMP.Add(objeto);
            }

            return Json(new { listaMP });
        }

        [HttpGet]
        public IActionResult CriarArquivoMP()
        {
            string downloadUrl = "";
            string msg = "";
            List<List<object>> resultadoQuery = new List<List<object>>();
            //private List<ExpandoObject> listaMP;

            //https://www.c-sharpcorner.com/article/import-and-export-data-using-epplus-core/
            try
            {
                string rootFolder = _hostingEnvironment.WebRootPath;
                string fileName = $"ExportQuery-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                downloadUrl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);

                FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(rootFolder, fileName));
                }

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    ExpandoObject linha = listaMP[0];
                    for (int j = 0; j < linha.Count(); j++)
                    {
                        object coluna = linha.ElementAt(j).Key;
                        worksheet.Cells[1, j + 1].Value = coluna;
                    }

                    for (int i = 0; i < listaMP.Count(); i++)
                    {
                        linha = listaMP[i];
                        for (int j = 0; j < linha.Count(); j++)
                        {
                            object coluna = linha.ElementAt(j).Value;
                            worksheet.Cells[i + 2, j + 1].Value = coluna;
                        }
                    }
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                msg = "Erro ao exportar os dados.";
            }

            return Json(new { msg, downloadUrl });
        }

        [HttpGet]
        public JsonResult ObterEstoqueInt(string nivel, string grupo, string grp_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime oDate = DateTime.Now;
                string st = "Erro ao consultar estoques";
                //Mudar daqui para baixo depois.
                switch (nivel)
                {
                    case "N1":
                        //var listaN1 = db.ViewEstoquePA.GroupBy(x => new { GRP_TIPO = x.GRP_TIPO == 2 ? "CHAPA" : "CAIXAS" })
                        var listaN1 = db.ViewEstoqueIntermediario.
                            //Where(c => c.PRO_ID.StartsWith("CH")).
                            GroupBy(x => new { GRP_TIPO = x.GRP_TIPO == 2 ? "CHAPA" : "CAIXAS" }).
                            AsNoTracking().
                             Select(c => new
                             {
                                 GRP_TIPO = c.Key.GRP_TIPO,
                                 DISPONIVEL = Math.Round(Convert.ToDouble(c.Sum(z => z.DISPONIVEL))),
                                 COMPROMISSADO = Math.Round(Convert.ToDouble(c.Sum(z => z.COMPROMISSADO))),
                                 SOBRA_PRODUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_PRODUCAO))),
                                 SOBRA_EXPEDICAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_EXPEDICAO))),
                                 DEVOLUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.DEVOLUCAO))),
                                 PEDIDOS_FUTUROS = Math.Round(Convert.ToDouble(c.Sum(z => z.PEDIDOS_FUTUROS))),
                                 SALDO_RETIDO = Math.Round(Convert.ToDouble(c.Sum(z => z.SALDO_RETIDO)))
                             }).ToList();

                        return Json(new { listaN1 });
                    case "N2":
                        var listaN2 = db.ViewEstoqueIntermediario.
                            Where(x => x.GRP_ID != grp_id).
                            GroupBy(x => new { GRP_ID = x.GRP_ID, GRP_DESCRICAO = x.GRP_DESCRICAO })
                            .Select(c => new
                            {
                                GRP_ID = c.Key.GRP_ID,
                                GRP_DESCRICAO = c.Key.GRP_DESCRICAO,
                                DISPONIVEL = Math.Round(Convert.ToDouble(c.Sum(z => z.DISPONIVEL))),
                                COMPROMISSADO = Math.Round(Convert.ToDouble(c.Sum(z => z.COMPROMISSADO))),
                                SOBRA_PRODUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_PRODUCAO))),
                                SOBRA_EXPEDICAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_EXPEDICAO))),
                                DEVOLUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.DEVOLUCAO))),
                                PEDIDOS_FUTUROS = Math.Round(Convert.ToDouble(c.Sum(z => z.PEDIDOS_FUTUROS))),
                                SALDO_RETIDO = Math.Round(Convert.ToDouble(c.Sum(z => z.SALDO_RETIDO)))
                            }).ToList();

                        return Json(new { listaN2 });
                    case "N3":
                        var listaN3 = db.ViewEstoqueIntermediario.Where(x => x.GRP_ID == grp_id).GroupBy(x => new { GRP_ID = x.GRP_ID, PRO_ID = x.PRO_ID, PRO_DESCRICAO = x.PRO_DESCRICAO })
                            .Select(c => new
                            {
                                GRP_ID = c.Key.GRP_ID,
                                PRO_ID = c.Key.PRO_ID,
                                PRO_DESCRICAO = c.Key.PRO_DESCRICAO,
                                DISPONIVEL = Math.Round(Convert.ToDouble(c.Sum(z => z.DISPONIVEL))),
                                COMPROMISSADO = Math.Round(Convert.ToDouble(c.Sum(z => z.COMPROMISSADO))),
                                SOBRA_PRODUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_PRODUCAO))),
                                SOBRA_EXPEDICAO = Math.Round(Convert.ToDouble(c.Sum(z => z.SOBRA_EXPEDICAO))),
                                DEVOLUCAO = Math.Round(Convert.ToDouble(c.Sum(z => z.DEVOLUCAO))),
                                PEDIDOS_FUTUROS = Math.Round(Convert.ToDouble(c.Sum(z => z.PEDIDOS_FUTUROS))),
                                SALDO_RETIDO = Math.Round(Convert.ToDouble(c.Sum(z => z.SALDO_RETIDO)))
                            }).ToList();

                        //var listaPedidos = db.ViewPedidosFuturos.Where(x => x.PRO_ID == listaN3[0].PRO_ID).ToList();

                        return Json(new { listaN3 });
                    default:
                        return Json(new { st });
                }
            }
        }

        [HttpGet]
        public IActionResult ObterGruposDeProdutos()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                var Db_Grupos = db.GrupoProduto.AsNoTracking().ToList();
                return Json(new { Db_Grupos });
            }
        }

        public JsonResult ObterFila(string recalculaFila, int pagina, bool visualizacaoMaquinaEquipe)
        {
            /*
               file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/ui-accordion.html
                file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/blo-search.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/blo-mail-important.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/charts-flot-extensions.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/charts-flot-combined.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/charts-rickshaw-extensions.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/ui-tabs.html
                 file:///C:/drives/PlaySis/playsis/Partilheira/Exemplos%20HTML%20Complte%20Admin%20-%20v2.2-Package/Complete%20Admin%20-%20HTML%20-%20v2.2/Full%20Page%20Menu%20-%20v2.2/ui-googlemaps.html
             */

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    Exception e = null;
                    ////if (recalculaFila == "sim")
                    ////{
                    //    OptFunctions.tipoHeuristicaFilaProducao = 5;
                    //    OptFunctions.SetCMD("M");
                    //    OptFunctions.Debug("D");
                    //    List<OptFunctions.Menssagens> Menssagens = new List<OptFunctions.Menssagens>();
                    //    e = OptFunctions.FilaExpedicao(0, 0, "FULL", true, 0, null, ref Menssagens);
                    ////}
                    if (e == null)
                    {
                        db.Database.SetCommandTimeout(120000);
                        DateTime DtIni = new DateTime(1990, 01, 01); //Data muito pequena para não ter possibilidade de um item da fila ser menor que esta data.
                        DateTime df = DateTime.Now.AddDays(
                            db.Param.AsNoTracking().Where(p => p.PAR_ID == "FILA_PRODUCAO_NUMERO_DIAS_TELA").Select(x => x.PAR_VALOR_N).FirstOrDefault());



                        //Parte referente ao modelo com paginação.
                        /*DateTime d = DateTime.Now.AddHours(168);
                        if (pagina != 0)
                        {
                            DtIni = DateTime.Now.AddHours(168 * pagina);
                            d = DtIni.AddHours(168);
                        }
                        else
                        {
                            DtIni = new DateTime(1990, 01, 01); //Data muito pequena para não ter possibilidade de um item da fila ser menor que esta data.
                            d = DateTime.Now.AddHours(168);
                        }

                        var fila = db.ViewFilaProducao.Where(v => v.FprDataInicioPrevista >= DtIni && v.FprDataFimPrevista < d)*/
                        UsuarioSingleton usuarioSingleton = UsuarioSingleton.Instance;
                        T_Usuario usuario = usuarioSingleton.ObterUsuario(ObterUsuarioLogado().USE_ID);

                        List<T_PREFERENCIAS> pref = usuario.T_PREFERENCIAS.ToList();
                        string un_tempo = "SEGUNDO";
                        string fat_tempo = "SEGUNDO";
                        string usuario_logado = usuario.USE_NOME + "_" + usuario.USE_ID;
                        int i = 0;
                        while (i < pref.Count && !pref[i].PRE_TIPO.Equals("UN_TEMPO"))
                            i++;

                        if (i < pref.Count)
                            un_tempo = pref[i].PRE_VALOR;

                        i = 0;
                        while (i < pref.Count && !pref[i].PRE_TIPO.Equals("FAT_TEMPO"))
                            i++;

                        if (i < pref.Count)
                            fat_tempo = pref[i].PRE_VALOR;


                        var fila = db.ViewFilaProducao.Where(v => v.MAQ_TIPO_PLANEJAMENTO != "SERVICO_COMPRAS" && v.FprDataInicioPrevista >= DtIni && v.FprDataFimPrevista <= df)
                            //.OrderBy(f => new { f.MaqDescricao, f.RotMaqId, f.OrdemDaFila, f.RotSeqTransformacao }).ToList()
                            .Select(f => new
                            {
                                orderby_1 = (visualizacaoMaquinaEquipe && f.EQU_ID != "" && f.EQU_ID != null) ? f.EQU_ID : f.MaqDescricao, //variavel que sera o primeiro campo de referencia a ser ordenado
                                orderby_2 = (visualizacaoMaquinaEquipe && f.EQU_ID != "" && f.EQU_ID != null) ? f.EQU_ID : f.RotMaqId,//variavel que sera o segundo campo de referencia a ser ordenado
                                maquina = f.MaqDescricao,
                                maquinaId = f.RotMaqId,
                                grupoMaquinaId = f.GMA_ID,
                                produtoId = f.PaProId,
                                pedidoId = f.OrdId,
                                produto = f.PaProDescricao,
                                grpDescricao = f.GRP_DESCRICAO,
                                inicioPrevisto = f.FprDataInicioPrevista,//.ToString("dd/MM/yyyy HH:mm:ss"),
                                fimPrevisto = f.FprDataFimPrevista,//.ToString("dd/MM/yyyy HH:mm:ss"),
                                equipeId = f.EQU_ID,
                                EntregaDe = f.OrdDataEntregaDe,
                                EntregaAte = f.OrdDataEntregaAte,
                                seqTransform = f.RotSeqTransformacao,
                                seqRepet = f.FprSeqRepeticao,
                                truncado = f.Truncado.Trim(),
                                dataInicioTrunc = f.DataInicioTrunc,
                                dataFimTrunc = f.DataFimTrunc,
                                qtd = f.FprQuantidadePrevista,
                                CorFila = f.CorFila,
                                CorOrd = f.CorOrd,
                                // conceito de escala de tempo a partir de agora em minutos
                                MinutoIni = 0,
                                MinutoFim = 0,
                                MinutoIniTrunc = 0,
                                MinutoFimTrunc = 0,
                                Id = f.Id,// id fila
                                CongelaFila = f.CongelaFila,
                                OrdemDaFila = f.OrdemDaFila,
                                PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                                InicioJanelaEmbarque = f.OrdInicioJanelaEmbarque,
                                FimJanelaEmbarque = f.OrdFimJanelaEmbarque,
                                EmbarqueAlvo = f.OrdEmbarqueAlvo,
                                GrupoProdutivo = f.FPR_GRUPO_PRODUTIVO,
                                InicioGrupoProdutivo = f.OrdInicioGrupoProdutivo,
                                FimGrupoProdutivo = f.OrdFimGrupoProdutivo,
                                DataNecessidadeInicioProducao = f.DataHoraNecessidadeInicioProducao,
                                DataNecessidadeFimProducao = f.DataHoraNecessidadeFimProducao,
                                TipoCarregamento = f.OrdTipoCarregamento,
                                OrdTipo = f.OrdTipo,
                                Status = f.ORD_STATUS,
                                OrdOpIntegracao = f.ORD_OP_INTEGRACAO,
                                CliId = f.CliId,
                                CliNome = f.CliNome,
                                PecasPorPulso = f.RotQuantPecasPulso,
                                M2Unitario = f.ORD_M2_UNITARIO,
                                M2Total = f.ORD_M2_TOTAL,
                                Performance = f.ROT_PERFORMANCE,
                                TempoSetup = f.FPR_META_SETUP,
                                TempoSetupA = 0,
                                CorBico1 = f.CorBico1,
                                CorBico2 = f.CorBico2,
                                CorBico3 = f.CorBico3,
                                CorBico4 = f.CorBico4,
                                CorBico5 = f.CorBico5,
                                hieranquiaSeqTransf = f.FPR_HIERARQUIA_SEQ_TRANSFORMACAO
                            })
                            .OrderBy(f => f.orderby_1) //pode ter o valor de equipeId ou maquinaDDescricao
                            .ThenBy(f => f.orderby_2) //pode ter o valor de equipeId ou maquinaId
                            .ThenBy(f => f.inicioPrevisto)
                            .ThenBy(f => f.seqTransform)
                            .ToList(); //Pendencia - Retirar o Take(100)

                        db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                        if (fila.Count() <= 0)
                        {
                            //var st = "Não existem pedidos programados na fila de produção.";
                            return Json(fila);
                        }

                        var mimDtIni = fila.Min(m => m.inicioPrevisto);
                        var DtFim = fila.Max(m => m.fimPrevisto);

                        if (DtIni > mimDtIni || DateTime.Compare(DtIni, new DateTime(1990, 01, 01)) == 0) //Data muito pequena declarada anteriormente
                        {
                            DtIni = mimDtIni;
                        }



                        var fila2 = fila.Select(f => new
                        {
                            orderby_1 = f.orderby_1, //mantem os campos para ordernar novamente
                            orderby_2 = f.orderby_2,
                            maquina = f.maquina,
                            maquinaId = f.maquinaId,
                            grupoMaquinaId = f.grupoMaquinaId,
                            produtoId = f.produtoId,
                            pedidoId = f.pedidoId,
                            produto = f.produto,
                            grpDescricao = f.grpDescricao,
                            inicioPrevisto = f.inicioPrevisto,//.ToString("dd/MM/yyyy HH:mm:ss"),
                            fimPrevisto = f.fimPrevisto,//.ToString("dd/MM/yyyy HH:mm:ss"),
                            equipeId = f.equipeId,
                            EntregaDe = f.EntregaDe,
                            EntregaAte = f.EntregaAte,
                            seqTransform = f.seqTransform,
                            seqRepet = f.seqRepet,
                            truncado = f.truncado.Trim(),
                            dataInicioTrunc = f.dataInicioTrunc,
                            dataFimTrunc = f.dataFimTrunc,
                            qtd = f.qtd,
                            CorFila = f.CorFila,
                            CorOrd = f.CorOrd,
                            // conceito de escala de tempo a partir de agora em minutos
                            MinutoIni = (f.inicioPrevisto - DtIni).TotalMinutes,
                            MinutoFim = (f.fimPrevisto - DtIni).TotalMinutes,
                            MinutoIniTrunc = (f.truncado != "") ? (f.dataInicioTrunc - DtIni).TotalMinutes : 0,
                            MinutoFimTrunc = (f.truncado != "") ? (f.dataFimTrunc - DtIni).TotalMinutes : 0,
                            Id = f.Id,// id fila
                            CongelaFila = f.CongelaFila,
                            OrdemDaFila = f.OrdemDaFila,
                            PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                            InicioJanelaEmbarque = f.InicioJanelaEmbarque,
                            FimJanelaEmbarque = f.FimJanelaEmbarque,
                            EmbarqueAlvo = f.EmbarqueAlvo,
                            GrupoProdutivo = f.GrupoProdutivo,
                            InicioGrupoProdutivo = f.InicioGrupoProdutivo,
                            FimGrupoProdutivo = f.FimGrupoProdutivo,
                            DataNecessidadeInicioProducao = f.DataNecessidadeInicioProducao,
                            DataNecessidadeFimProducao = f.DataNecessidadeFimProducao,
                            TipoCarregamento = f.TipoCarregamento,
                            OrdTipo = f.OrdTipo,
                            Status = f.Status,
                            OrdOpIntegracao = f.OrdOpIntegracao,
                            CliId = f.CliId,
                            CliNome = f.CliNome,
                            PecasPorPulso = f.PecasPorPulso,
                            M2Unitario = f.M2Unitario,
                            M2Total = f.M2Total,
                            Performance = f.Performance,
                            PerformanceString = BaseController.ConversorUnidades(Convert.ToDouble(f.Performance), fat_tempo),
                            TempoSetup = f.TempoSetup,
                            TempoSetupString = BaseController.ConversorTempo(Convert.ToDouble(f.TempoSetup), un_tempo),
                            TempoSetupA = f.TempoSetupA,
                            TempoSetupAString = BaseController.ConversorTempo(Convert.ToDouble(f.TempoSetupA), un_tempo),
                            CorBico1 = f.CorBico1,
                            CorBico2 = f.CorBico2,
                            CorBico3 = f.CorBico3,
                            CorBico4 = f.CorBico4,
                            CorBico5 = f.CorBico5,
                            hieranquiaSeqTransf = f.hieranquiaSeqTransf,
                            estado_op = ((f.equipeId == null || f.equipeId == "") && f.maquinaId != "" && f.maquinaId != null) ? 0 : // 0 = op somente com maquina
                                (f.equipeId != null && f.equipeId != "" && f.maquinaId != null && f.maquinaId != "") ? 1 :  //1 = op com maquina e equipe, 
                                (f.equipeId != null && f.equipeId != "" && (f.maquinaId == null || f.maquinaId == "")) ? 2 : 0 //2 = somente equipe
                        })
                        .OrderBy(f => f.orderby_1)
                        .ThenBy(f => f.orderby_2)
                        .ThenBy(f => f.inicioPrevisto)
                        .ThenBy(f => f.seqTransform)
                        .ToList(); //Pendencia - Retirar o Take(100)

                        var QtdHorasLinhaTempo = Convert.ToInt32(Math.Floor((DtFim - DtIni).TotalHours));
                        TimeSpan hora = new TimeSpan((DtIni).Ticks);
                        var horaIni = hora.Hours;
                        var MinutoIni = hora.Minutes;
                        DataInicial = DtIni;

                        //Pegando o minuto inicial da Onduladeira.
                        //var contador = db.CorridasOnduladeira.AsNoTracking().ToList().Count();
                        var contador = db.CorridasOnduladeira.AsNoTracking().Select(c => c.COR_ID).Count();
                        var menorIniOndu = DtIni;
                        if (contador > 0)
                            menorIniOndu = db.CorridasOnduladeira.AsNoTracking().Min(x => x.COR_INICIO_PREVISTO);

                        var minIniOndu = (menorIniOndu - DtIni).TotalMinutes;
                        if (minIniOndu < 0)
                        {
                            DtIni = menorIniOndu;
                        }


                        return Json(new { fila2, DtIni, DtFim, QtdHorasLinhaTempo, horaIni, MinutoIni, minIniOndu, usuario_logado });
                    }
                    else
                    {
                        var fila = UtilPlay.getErro(e);
                        return Json(fila);
                    }
                }
                catch (Exception e)
                {
                    var fila = UtilPlay.getErro(e);
                    return Json(fila);
                    throw;
                }
            }
        }

        public JsonResult ObterQuantidadePaginas()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    db.Database.SetCommandTimeout(120000);
                    /*"select DATEDIFF(day, (select min(FPR_DATA_FIM_PREVISTA)), (select max(FPR_DATA_FIM_PREVISTA))) as qtd_dias from V_FILA_PRODUCAO";*/
                    DateTime max = db.ViewFilaProducao.Max(x => x.FprDataFimPrevista);
                    DateTime min = DateTime.Now;
                    //DateTime min = db.ViewFilaProducao.Min(x => x.FprDataFimPrevista);
                    int diferenca = (max - min).Days;
                    decimal semanas = Math.Ceiling((decimal)diferenca / 7);

                    db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                    return Json((int)semanas);
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                    throw;
                }
            }
        }

        /*Pendencia - Procurar views correspondentes - A V_PENDENCIAS_PROGRAMACAO retorna teste?*/
        public JsonResult ObterPendenciasCadastrais()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    db.Database.SetCommandTimeout(120000);
                    string sql = "SELECT * FROM V_PENDENCIAS_PROGRAMACAO"; // ESTA VIEW JA IMPLEMENTA TOP 10 PARA SER MAIS RAPIDA 
                    var pendencias = db.Query<V_PENDENCIAS_PROGRAMACAO>().FromSql(sql).ToList();
                    db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                    return Json(pendencias);

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                    throw;
                }
            }
        }

        public JsonResult ObterOpsParciais()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    db.Database.SetCommandTimeout(120000);
                    var pendencias = db.V_SALDO_PRODUCAO_DE_OPS.AsNoTracking().Where(x => x.ENCERRADO_NORMAL == "S" && x.SALDO_A_PRODUZIR > 0 && x.RESIDUO_ELIMINADO == "N").
                        GroupBy(a => new { a.FASE, a.ORD_ID, a.PRO_ID, a.PRO_DESCRICAO, a.ORD_QUANTIDADE, a.ORD_TOLERANCIA_MENOS, a.ORD_OP_INTEGRACAO, a.CLI_NOME, a.QTD_PECAS_BOAS, a.QTD_PERDAS }).
                        Select(g => new
                        {
                            FASE = g.Key.FASE,
                            ORD_ID = g.Key.ORD_ID,
                            PRO_ID = g.Key.PRO_ID,
                            PRO_DESCRICAO = g.Key.PRO_DESCRICAO,
                            ORD_QUANTIDADE = g.Key.ORD_QUANTIDADE,
                            ORD_TOLERANCIA_MENOS = g.Key.ORD_TOLERANCIA_MENOS,
                            ORD_OP_INTEGRACAO = g.Key.ORD_OP_INTEGRACAO,
                            CLI_NOME = g.Key.CLI_NOME,
                            ROT_PRO_ID = "",
                            ENCERRADO_NORMAL = "",
                            RESIDUO_ELIMINADO = "",
                            ROT_SEQ_TRANFORMACAO = g.Min(x => x.ROT_SEQ_TRANFORMACAO),
                            FPR_SEQ_REPETICAO = g.Max(x => x.FPR_SEQ_REPETICAO),
                            FPR_PREVISAO_MATERIA_PRIMA = g.Min(x => x.FPR_PREVISAO_MATERIA_PRIMA),
                            SALDO_A_PRODUZIR = g.Max(x => x.SALDO_A_PRODUZIR),
                            DATA_PRODUCAO = g.Max(x => x.DATA_PRODUCAO),
                            QTD_PRODUZIDA = g.Key.QTD_PECAS_BOAS
                        }).OrderBy(x => x.DATA_PRODUCAO).ToList();

                    //var pendencias = db.V_PEDIDOS_PARCIALMENTE_PRODUZIDOS.AsNoTracking().OrderBy(x => x.DATA_PRODUCAO).ToList();
                    db.Database.SetCommandTimeout(ParametrosSingleton.Instance.TimOut);
                    return Json(pendencias);
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                    throw;
                }
            }
        }

        [HttpPost]
        public JsonResult ResiduosReprogramacaoOP(string ORD_ID, string FASE)
        {

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string st = "OK";
                StringBuilder sb;
                V_MOTIVOS_DE_REPROGRAMACAO aux;
                try
                {
                    var Db_Itens = db.V_SALDO_PRODUCAO_DE_OPS.AsNoTracking().Where(x => x.ORD_ID.Equals(ORD_ID) && x.FASE.Equals(FASE, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.ROT_SEQ_TRANFORMACAO)
                    .Select(x => new V_SALDO_PRODUCAO_DE_OPS() { ROT_SEQ_TRANFORMACAO = x.ROT_SEQ_TRANFORMACAO, ROT_PRO_ID = x.ROT_PRO_ID, SALDO_A_PRODUZIR = x.SALDO_A_PRODUZIR, QTD_PECAS_BOAS = x.QTD_PECAS_BOAS, QTD_PERDAS = x.QTD_PERDAS, MAQUINAS = "", OBS = "", MAQ_ID = x.MAQ_ID }).ToList();
                    var Produtos = Db_Itens.Select(x => x.ROT_PRO_ID).Distinct().ToList();
                    var Db_Roteiro = db.Roteiro.Where(r => Produtos.Contains(r.PRO_ID)).Select(r => new { r.PRO_ID, r.MAQ_ID, r.ROT_SEQ_TRANFORMACAO }).ToList();
                    var _ObsProdParcial = db.V_MOTIVOS_DE_REPROGRAMACAO.AsNoTracking().Where(x => x.ORD_ID.Equals(ORD_ID)).ToList();

                    foreach (var item in Db_Itens)
                    {
                        foreach (var _roteiro in Db_Roteiro)
                        {
                            if (item.ROT_PRO_ID.Equals(_roteiro.PRO_ID) && item.ROT_SEQ_TRANFORMACAO == _roteiro.ROT_SEQ_TRANFORMACAO)
                            {
                                item.MAQUINAS += _roteiro.MAQ_ID + " ";
                            }
                            else if (item.ROT_SEQ_TRANFORMACAO == 0)
                            {
                                item.MAQUINAS += item.MAQ_ID + " ";
                            }
                        }
                        aux = _ObsProdParcial.Where(x => x.PRO_ID == item.ROT_PRO_ID && x.FPR_SEQ_TRANFORMACAO == item.ROT_SEQ_TRANFORMACAO).FirstOrDefault();
                        if (aux != null && aux.MOV_OCO_ID_OP_PARCIAL != "-1")
                        {
                            sb = new StringBuilder();
                            sb.Append("CODIGO OCO.PARCIAL:[ ");
                            sb.Append(aux.MOV_OCO_ID_OP_PARCIAL);
                            sb.Append(" ] - ");
                            sb.Append("DESCRIÇÃO:[ ");
                            sb.Append(aux.OCO_DESCRICAO);
                            sb.Append(" ] - ");
                            sb.Append("OBS:[ ");
                            sb.Append(aux.MOV_OBS_OP_PARCIAL);
                            sb.Append(" ]");
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.Append("N/A");
                        }
                        item.OBS = sb.ToString();
                    }
                    var m = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    return Json(new { st, Db_Itens, m });
                }
                catch (Exception e)
                {
                    //return Json(st = e.Message);

                    throw e;
                }
            }
        }
        [HttpPost]
        public JsonResult EliminarResiduosOP(string Itens, string Cab)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                StringBuilder st = new StringBuilder("");
                try
                {
                    dynamic oCab = JsonConvert.DeserializeObject<dynamic>(Cab);
                    if (oCab != null)
                    {
                        string sql = "EXEC SP_PLUG_OPS_ADD_SEQ_REPETICAO '" + oCab.Pedido + "','" + oCab.Produto + "',0," + oCab.SeqRep + ",0,'" + oCab.PrevMat.ToString("yyyy-MM-dd hh:mm:ss") + "'";
                        int result = db.Database.ExecuteSqlCommand(sql);
                        st.Append("OK");
                    }
                    return Json(new { st = st.ToString() });
                }
                catch (Exception e)
                {
                    st.Append(UtilPlay.getErro(e));
                    return Json(new { st = st.ToString() });
                }
            }
        }
        [HttpPost]
        public JsonResult ReprogramarOP(string Itens, string Cab)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Int64 _qtd = 0;
                StringBuilder st = new StringBuilder("");
                try
                {
                    //O Atributo do objeto oCab (oCab.seqRep) na verdade é a sequencia de Transformação, trocar o nome no JS e no Controller --- Pendência 06-04-20
                    dynamic oCab = JsonConvert.DeserializeObject<dynamic>(Cab);
                    dynamic oItens = JsonConvert.DeserializeObject<dynamic>(Itens);
                    bool erro = false;

                    string data = oCab.PrevMat.ToString().Substring(0, 16);
                    DateTime dt = DateTime.Parse(data);

                    erro = true;
                    foreach (var item in oItens)
                    {
                        _qtd = Convert.ToInt64(item.SaldoRep);
                        if (_qtd > 0)
                        {
                            erro = false;
                        }
                    }
                    if (erro)
                    {
                        st.Append("Nunhuma quantidade a reprogramar foi preenchida.");
                        return Json(new { st = st.ToString() });
                    }
                    else
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in oItens)
                                {
                                    _qtd = Convert.ToInt64(item.SaldoRep);
                                    if (_qtd > 0)
                                    {
                                        string sql = "EXEC SP_PLUG_OPS_ADD_SEQ_REPETICAO '" + oCab.Pedido + "','" + item.produto + "'," + item.SaldoRep + "," + oCab.SeqRep + "," + item.SeqTrans + ",'" + dt.ToString("yyyyMMdd HH:mm:ss") + "'"; // 0 recria todas   se passar diferente de zero vai criar as sequencia que esta sendo passada
                                        db.Database.ExecuteSqlCommand(sql);
                                    }
                                }
                                st.Append("OK");
                                transaction.Commit();
                                return Json(new { st = st.ToString() });
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                st.Append(UtilPlay.getErro(e));
                                return Json(new { st = st.ToString() });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    st.Append(UtilPlay.getErro(e));
                    return Json(new { st = st.ToString() });
                }
                //}
            }
        }

        [HttpGet]
        public JsonResult ObterPedidosSuspensos()
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var _lista = db.Order.Include(o => o.Cliente).AsNoTracking()
                    .Where(o => o.ORD_STATUS.Substring(1, 1).Equals("S"))
                    .Select(o => new { o.ORD_ID, o.PRO_ID, o.ORD_QUANTIDADE, o.ORD_DATA_ENTREGA_ATE, o.Cliente.CLI_NOME })
                    .ToList();
                return Json(_lista);
            }

        }

        public JsonResult AlteraFila(string ordem, string maqManual, string idFila, string dtprevMat)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    double ordemfila = (Convert.ToDouble(ordem) - 0.5);
                    int idF = Convert.ToInt32(idFila);
                    DateTime dt = Convert.ToDateTime(dtprevMat);

                    if (maqManual != "" && maqManual != null)
                    {
                        db.Database.ExecuteSqlCommand(@" UPDATE T_FILA_PRODUCAO  SET MAQ_ID_MANUAL = {0}
                     WHERE FPR_ID = {1} ",
                        maqManual, idF);

                    }
                    db.Database.ExecuteSqlCommand(@" UPDATE T_FILA_PRODUCAO  SET 
                FPR_ORDEM_NA_FILA = {0},FPR_PREVISAO_MATERIA_PRIMA = {1}  WHERE ORD_ID IN
                (SELECT ORD_ID FROM T_FILA_PRODUCAO WHERE FPR_ID = {2}  )",
                    ordemfila, dt, idF);

                    return Json("OK");
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }
            }

        }

        [HttpPost]
        public JsonResult ObterTiposVeiculos()
        {
            List<TipoVeiculo> listaVeiculos = new List<TipoVeiculo>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                listaVeiculos = db.TipoVeiculo.AsNoTracking().ToList();
            }

            return Json(listaVeiculos);
        }

        public JsonResult AtualizarTiposVeiculos(string tipId, string listaIdsCargas)
        {
            string[] carIds = listaIdsCargas.Split(",");
            List<Carga> listaCargas = new List<Carga>();
            List<object> tempList = new List<object>();
            List<List<object>> listaFinal = new List<List<object>>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                listaCargas = db.Carga.AsNoTracking().Where(x => listaIdsCargas.Contains(x.CAR_ID)).ToList();
            }

            foreach (var item in listaCargas)
            {
                item.TIP_ID = Convert.ToInt32(tipId);
                item.PlayAction = "UPDATE";

                tempList.Add(item);
            }

            listaFinal.Add(tempList);
            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(listaFinal, 1, true);


            string temp = "OK";
            return Json(new { temp });
        }

        [HttpPost]
        public JsonResult BuscarMaquinas(string proId)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var maquinas = db.Roteiro.AsNoTracking().Include(r => r.Maquina).Where(r => r.PRO_ID == proId).Select(r => new { descricao = r.Maquina.MAQ_DESCRICAO, id = r.Maquina.MAQ_ID }).Distinct();
                return Json(maquinas);
            }
        }
        [HttpPost]
        public JsonResult BuscarSequenciasTranformacao(string maqId, string proId)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var seqsTransf = db.Roteiro.AsNoTracking().Where(r => r.MAQ_ID == maqId && r.PRO_ID == proId).Select(r => new { SequanciaTransformacao = r.ROT_SEQ_TRANFORMACAO }).Distinct();
                return Json(seqsTransf);
            }
        }

        /// <summary>
        /// Une duas ou mais Cargas em uma nova carga
        /// </summary>
        [HttpPost]
        public JsonResult UnirCargas(string CargasIds, string CargaSelecionada, string CidadesIds)
        {
            string st = "BAD";
            string msg = "Você deve selecionar duas ou mais cragas para realizar a união\n ou selecionar uma unica carga para desfazer a união.";
            if (!String.IsNullOrEmpty(CargasIds))
            {
                var _idsCargas = CargasIds.Split(";");
                if (_idsCargas.Length >= 1)
                {
                    string idAux = "";
                    List<LogPlay> Logs = new List<LogPlay>();
                    LogPlay auxLog = new LogPlay();
                    using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                    {

                        if (!String.IsNullOrEmpty(CidadesIds))
                        {
                            var _idsCidades = CidadesIds.Split(";");
                            var Db_cargaSelecionada = db.Carga.Include(x => x.Transportadora).AsNoTracking().Where(c => c.CAR_ID.Equals(CargaSelecionada)).ToList();
                            var Db_todasCargas = db.Carga.Include(x => x.Transportadora).AsNoTracking().Where(c => _idsCargas.Contains(c.CAR_ID)).ToList();
                            /*var Db_ItensCargas = db.ItenCarga
                                .Include(x => x.Oredr)
                                .Include(c => c.Oredr.Municipio)
                                .AsNoTracking()
                                .Where(w => _idsCargas.Contains(w.CAR_ID) && _idsCidades.Contains(w.Oredr.Municipio.MUN_NOME))
                                .ToList();
                            */


                            if (Db_todasCargas.Count > 1)
                            {
                                var aux_Db_ItensCargas = db.ItenCarga
                                    .Include(x => x.Oredr)
                                    .Include(c => c.Oredr.Municipio)
                                    .AsNoTracking()
                                    .Where(w => _idsCargas.Contains(w.CAR_ID))
                                    .ToList();

                                List<ItenCarga> Db_ItensCargas = new List<ItenCarga>();
                                for (int i = 0; i < aux_Db_ItensCargas.Count; i++)
                                {
                                    int j = 0;
                                    while (j < _idsCargas.Length &&
                                        !(aux_Db_ItensCargas[i].CAR_ID == _idsCargas[j] &&
                                        aux_Db_ItensCargas[i].Oredr.Municipio.MUN_NOME == _idsCidades[j]))
                                    {
                                        j++;
                                    }

                                    if (j < _idsCargas.Length)
                                    {
                                        Db_ItensCargas.Add(aux_Db_ItensCargas[i]);
                                    }
                                }

                                foreach (var item in Db_todasCargas)
                                {
                                    Db_cargaSelecionada.Add(item);
                                }

                                if (!_idsCargas.Contains(CargaSelecionada))
                                {
                                    var itensAux = db.ItenCarga
                                        .Include(x => x.Oredr)
                                        .Include(c => c.Oredr.Municipio)
                                        .AsNoTracking()
                                        .Where(w => CargaSelecionada.Equals(w.CAR_ID))
                                        .ToList();

                                    foreach (var item in itensAux)
                                    {
                                        Db_ItensCargas.Add(item);
                                    }

                                }
                                Carga _cargaAux = new Carga();
                                idAux = _cargaAux.UnirCargas(Db_todasCargas, CargaSelecionada, ref Logs, Db_ItensCargas);
                            }
                            else
                            {
                                //Erro, tentando unir a mesma carga
                                st = "BAD";
                                msg = "Impossível unir apenas uma carga!";
                                return Json(new { st, msg });
                            }
                        }
                        else
                        {
                            var Db_CargasUniao = db.Carga.Include(x => x.Transportadora).AsNoTracking().Where(c => _idsCargas.Contains(c.CAR_ID)).ToList();

                            if (Db_CargasUniao != null && Db_CargasUniao.Count > 0)
                            {
                                Carga _cargaAux = new Carga();
                                idAux = _cargaAux.UnirCargas(Db_CargasUniao, CargaSelecionada, ref Logs, null);
                            }
                        }
                    }
                    var _erros = auxLog.GetLogsErro(Logs);
                    st = (_erros.Count > 0) ? "BAD" : "OK";
                    msg = idAux;
                    return Json(new { st, msg });
                }
            }
            return Json(new
            {
                st,
                msg
            });
        }


        public ActionResult OtimizarCarga(string strCargas, string carIdPrincipal)
        {
            string lstIds = strCargas.Replace(",", "_");
            PontosMapa pontoMapa;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string idPontoORigem = db.Param.AsNoTracking().Where(p => p.PAR_ID.Equals("EXPEDICAO_ID_PONTO_MAPA_ORIGEM")).Select(p => p.PAR_VALOR_S).FirstOrDefault();
                pontoMapa = db.PontosMapa.AsNoTracking().Where(c => c.PON_ID.Equals(idPontoORigem)).FirstOrDefault();
            }

            ViewBag.cargaPrincipal = carIdPrincipal;
            ViewBag.listaCargas = lstIds;
            ViewBag.pontoMapa = pontoMapa;

            return View();
        }

        public JsonResult BuscarCidadesPontoMapa(string listaCidades)
        {
            string[] vetCidades = listaCidades.Split(",");
            List<Municipio> cidades = new List<Municipio>();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                cidades = db.Municipio.AsNoTracking().Where(x => vetCidades.Contains(x.MUN_ID)).ToList();
            }

            return Json(new { cidades });
        }

        public JsonResult obterCargasAPS(string strCargas, string carIdPrincipal)
        {
            string[] lstIds = strCargas.Split("_");
            string carId = carIdPrincipal.Split("_")[1];
            List<CargasWeb> listaCargas = new List<CargasWeb>();
            List<CargasWeb> cargaPrincipal = new List<CargasWeb>();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                listaCargas = db.CargasWeb.AsNoTracking().Where(x => lstIds.Contains(x.CAR_ID)).ToList();
                listaCargas = listaCargas.OrderBy(x => x.EMBARQUE)
                    .ThenBy(x => x.CAR_ID)
                    .ThenBy(x => x.UF)
                    .ThenBy(x => x.PON_DESCRICAO)
                    .ThenBy(x => x.ORD_REGIAO_ENTREGA)
                    .ThenBy(x => x.ORD_DATA_ENTREGA_DE)
                    .ThenBy(x => x.CLI_NOME)
                    .ToList();
            }

            if (carId != null)
            {
                cargaPrincipal = listaCargas.Where(x => x.CAR_ID == carId).ToList();
                while (listaCargas.Remove(listaCargas.Where(x => x.CAR_ID == carId).FirstOrDefault())) ;
            }

            return Json(new { cargaPrincipal, listaCargas });
        }

        public JsonResult obterPedidosFuturosAPS(string cargas, string raio, string ufs, string municipios, string clientes, string diasAntecipacao, string latitude, string longitude)
        {
            cargas = cargas == null ? "" : cargas + ",";
            raio = raio == null ? "0" : raio;
            ufs = ufs == null ? "" : ufs + ",";
            municipios = municipios == null ? "" : municipios + ",";
            clientes = clientes == null ? "" : clientes + ",";
            diasAntecipacao = diasAntecipacao == null ? "365" : diasAntecipacao + ",";

            string[] lstCargas = cargas.Split(",");
            string[] lstUfs = ufs.Split(",");
            string[] lstMunicipios = municipios.Split(",");
            string[] lstClientes = clientes.Split(",");
            DateTime dataAntecipacao = DateTime.Now.AddDays(Convert.ToInt16(diasAntecipacao));

            //V_PEDIDOS_A_PLANEJAR_EXPEDICAO
            List<OrderOpt> listaPedFut = new List<OrderOpt>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                //listaPedFut = db.OrderOpt.AsNoTracking().Where(x => lstCargas.Contains(x.CAR_ID)).ToList();
                string sql = "select * from V_PEDIDOS_A_PLANEJAR_EXPEDICAO where isnull(CAR_STATUS,1) <= 1";
                listaPedFut = db.OrderOpt.AsNoTracking().FromSql(sql).ToList();

                if (lstCargas.Length > 1)
                {
                    listaPedFut = listaPedFut.Where(x => lstCargas.Contains(x.CAR_ID)).ToList();
                }
                if (lstUfs.Length > 1)
                {
                    listaPedFut = listaPedFut.Where(x => lstUfs.Contains(x.UF_COD)).ToList();
                }
                if (lstMunicipios.Length > 1)
                {
                    listaPedFut = listaPedFut.Where(x => lstMunicipios.Contains(x.MUN_NOME)).ToList();
                }
                if (lstClientes.Length > 1)
                {
                    listaPedFut = listaPedFut.Where(x => lstClientes.Contains(x.CLI_NOME)).ToList();
                }
                if (!raio.Equals("0"))
                {
                    string sqlRaio = "SELECT MUN_ID, " +
                        "(6371 * acos(cos( radians(" + latitude + ") ) " +
                        "* cos( radians( MUN_LATITUDE ) ) " +
                        "* cos( radians( MUN_LONGITUDE ) - radians(" + longitude + ") ) " +
                        "+ sin( radians(" + latitude + ") ) * sin( radians( MUN_LATITUDE )))) " +
                        "AS distancia, MUN_NOME FROM T_MUNICIPIOS " +
                        "where" +
                        "(6371 * acos(cos(radians(" + latitude + "))" +
                        "* cos(radians(MUN_LATITUDE)) " +
                        "* cos(radians(MUN_LONGITUDE) - radians(" + longitude + ")) " +
                        "+ sin(radians(" + latitude + ")) * sin(radians(MUN_LATITUDE)))) < " + raio + " ORDER BY distancia ASC";
                    List<object[]> listaMunIdsObj = new List<object[]>();
                    object[] temp_obj;

                    using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                    {
                        try
                        {
                            command.CommandText = sqlRaio;
                            command.CommandType = CommandType.Text;

                            db.Database.OpenConnection();

                            using (DbDataReader result = command.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    temp_obj = new object[1];
                                    for (int i = 0; i < 1; i++)
                                    {
                                        var valor = result.GetValue(i);
                                        temp_obj[i] = valor;
                                    }
                                    listaMunIdsObj.Add(temp_obj);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    if (listaMunIdsObj.Count > 0)
                    {
                        List<string> listaMunIds = new List<string>();
                        foreach (var munId in listaMunIdsObj)
                        {
                            listaMunIds.Add(munId[0].ToString());
                        }

                        listaPedFut = listaPedFut.Where(x => listaMunIds.Contains(x.MUN_ID)).ToList();
                    }
                }
            }

            return Json(new { listaPedFut });
        }

        public void calcularDisponibilidadeCargaMaquina(ref List<List<CargasWeb>> resultCargasWeb)
        {
            //Lista com todos os produtos;
            List<string> listaProdutos = new List<string>();
            foreach (var carga in resultCargasWeb)
            {
                foreach (var itemCarga in carga)
                {
                    if (!listaProdutos.Contains(itemCarga.PRO_ID))
                    {
                        listaProdutos.Add(itemCarga.PRO_ID);
                    }
                }
            }

            //Lista com todos os roteiros possíveis de todos os produtos;
            List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> ListaRoteiros = new List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                ListaRoteiros = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsNoTracking()
                    .Include(x => x.Maquina)
                    .Include(x => x.Maquina.Calendario)
                    .Include(x => x.Produto)
                    .OrderBy(x => x.ROT_SEQ_TRANFORMACAO)
                    .Where(x => listaProdutos.Contains(x.PRO_ID))
                    .ToList();
            }

            List<V_OPS_A_PLANEJAR> listaVirtual = new List<V_OPS_A_PLANEJAR>();
            List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> tempRoteiros = new List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>();

            //Objeto com dois campos
            //1 - MaqId
            //2 - TempoTotalProducao
            object[] objeto;
            List<object[]> listaMaquinasTempo = new List<object[]>();
            double tempoProducao = 0;
            DateTime dataInicio;
            DateTime dataFim;
            DateTime prazoMinimo;
            List<string> erros = new List<string>();
            List<V_OPS_A_PLANEJAR> logs = new List<V_OPS_A_PLANEJAR>();

            foreach (var carga in resultCargasWeb)
            {
                listaMaquinasTempo = new List<object[]>();
                foreach (var itemCarga in carga)
                {
                    dataInicio = DateTime.Now;
                    dataFim = itemCarga.DTEMBARQUE;

                    tempRoteiros = ListaRoteiros.Where(x => x.PRO_ID == itemCarga.PRO_ID).OrderBy(x => x.ROT_SEQ_TRANFORMACAO).ToList();

                    listaVirtual = gerarListaVirtual(itemCarga, tempRoteiros); //É a lista virtual que serve como entrada para o algoritmo que simula o pedido na fila;

                    foreach (var item in listaVirtual)
                    {
                        if (item.GRP_TIPO == 2) //Onduladeira
                            tempoProducao = Convert.ToDouble(((((item.QuantidadePrevista * (item.PRO_LARGURA_PECA / 1000.0 * item.PRO_COMPRIMENTO_PECA / 1000.0)) / item.MAQ_LARGURA_UTIL) / item.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO) + item.TempoSetup + item.TempoSetupAjuste));
                        else
                            tempoProducao = ((item.QuantidadePrevista / (item.PecasPorPulso * item.Performance)) + item.TempoSetup + item.TempoSetupAjuste);


                        int i = 0;
                        while (i < listaMaquinasTempo.Count && item.MaquinaId != listaMaquinasTempo[i][0].ToString())
                        {
                            i++;
                        }

                        if (i < listaMaquinasTempo.Count)
                        {
                            item.TempoSetup += Convert.ToDouble(listaMaquinasTempo[i][1]);
                            listaMaquinasTempo[i][1] = tempoProducao + Convert.ToDouble(listaMaquinasTempo[i][1]);
                        }
                        else
                        {
                            objeto = new object[2];
                            objeto[0] = item.MaquinaId;
                            objeto[1] = tempoProducao;

                            listaMaquinasTempo.Add(objeto);
                        }
                    }

                    prazoMinimo = UtilPlay.calcularDataProdutiva(ref listaVirtual, dataInicio, dataFim, ref erros, ref logs);

                    if (erros.Count > 0 || prazoMinimo > dataFim) //Deu erro, então reverte os tempos;
                    {
                        foreach (var item in listaVirtual)
                        {
                            int i = 0;
                            while (i < listaMaquinasTempo.Count && item.MaquinaId != listaMaquinasTempo[i][0].ToString())
                                i++;

                            if (i < listaMaquinasTempo.Count)
                                listaMaquinasTempo[i][1] = Convert.ToDouble(listaMaquinasTempo[i][1]) - item.TempoProducao;
                        }
                        itemCarga.PlayAction = "ERRO";
                    }
                    else
                    {
                        itemCarga.PlayAction = "OK";
                    }

                    erros = new List<string>();

                }
            }
        }

        public List<V_OPS_A_PLANEJAR> gerarListaVirtual(CargasWeb item, List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> roteiros)
        {
            List<V_OPS_A_PLANEJAR> lista = new List<V_OPS_A_PLANEJAR>();
            V_OPS_A_PLANEJAR temp;

            DateTime data_aux_1970 = new DateTime(1970, 1, 1); //Datas auxiliares só para prencher a fila virtual;
            DateTime data_aux_2200 = new DateTime(2200, 1, 1); //Datas auxiliares só para prencher a fila virtual;
            DateTime data_inicio = DateTime.Now; //Nisso também
            DateTime data_fim = DateTime.Now.AddDays(30); //Mexer nisso depois
            foreach (var roteiro in roteiros)
            {
                temp = new V_OPS_A_PLANEJAR();

                temp.OrderId = "S0000";// + index;
                temp.ProdutoId = item.PRO_ID;
                temp.SequenciaTransformacao = roteiro.ROT_SEQ_TRANFORMACAO;
                temp.SequenciaRepeticao = 1;
                temp.FPR_PRIORIDADE = 0;
                temp.ORD_STATUS = "";
                temp.FPR_STATUS = "";
                temp.MaquinaId = roteiro.MAQ_ID;
                temp.ORD_PRO_ID = item.PRO_ID;
                temp.DataInicioPrevista = data_inicio;
                temp.DataFimPrevista = data_fim;

                temp.DataFimMaxima = data_aux_1970;
                temp.PrevisaoMateriaPrima = data_aux_1970;
                temp.ObservacaoProducao = "";
                temp.QuantidadePrevista = Convert.ToDouble(item.ORD_QUANTIDADE);
                temp.Status = "";
                temp.Produzindo = 0;
                temp.IdIntegracao = "";
                temp.QuantidadeProduzida = 0;
                temp.QuantidadeRestante = 0;
                temp.TempoRestanteTotal = 0;

                temp.ORD_DATA_ENTREGA_DE = data_aux_2200;
                temp.ORD_DATA_ENTREGA_ATE = data_aux_2200;
                temp.CLI_TRANSLADO = 1;
                temp.TempoProducao = 0;
                temp.Performance = Convert.ToDouble(roteiro.ROT_PERFORMANCE) > 0 ? Convert.ToDouble(roteiro.ROT_PERFORMANCE) : 0;
                temp.TempoSetup = Convert.ToDouble(roteiro.ROT_TEMPO_SETUP) > 0 ? Convert.ToDouble(roteiro.ROT_TEMPO_SETUP) : 0;
                temp.TempoSetupAjuste = Convert.ToDouble(roteiro.ROT_TEMPO_SETUP_AJUSTE) > 0 ? Convert.ToDouble(roteiro.ROT_TEMPO_SETUP_AJUSTE) : 0;
                temp.PecasPorPulso = Convert.ToDouble(roteiro.ROT_PECAS_POR_PULSO) > 0 ? Convert.ToDouble(roteiro.ROT_PECAS_POR_PULSO) : 0;
                //v_op_aux.HIERARQUIA_SEQ_TRANSFORMACAO = Convert.ToDouble(item.ROT_HIERARQUIA_SEQ_TRANSFORMACAO) > 0 ? Convert.ToDouble(item.ROT_HIERARQUIA_SEQ_TRANSFORMACAO) : 0;
                //v_op_aux.AVALIA_CUSTO = Convert.ToInt32(item.ROT_AVALIA_CUSTO) > 0 ? Convert.ToInt32(item.ROT_AVALIA_CUSTO) : 0;
                temp.HIERARQUIA_SEQ_TRANSFORMACAO = 0;
                temp.AVALIA_CUSTO = 0;
                temp.Truncado = "";
                temp.DataInicioTrunc = data_aux_1970;
                temp.DataFimTrunc = data_aux_1970;
                temp.OrdemDaFila = 0;

                temp.Id = 8000;// + index; //Zuado
                temp.ORD_LOTE_PILOTO = 0;
                temp.FPR_INICIO_GRUPO_PRODUTIVO = data_aux_1970;
                temp.FPR_FIM_GRUPO_PRODUTIVO = data_aux_1970;
                temp.DataHoraNecessidadeInicioProducao = data_aux_1970;
                temp.DataHoraNecessidadeFimProducao = data_aux_1970;
                temp.TempoDecorridoSetup = 1;
                temp.TempoDecorridoSetupAjuste = 1;
                temp.TempoDecorridoPerformacace = 1;
                temp.QuantidadePerformace = 1;
                temp.QuantidadeSetup = 1;
                temp.TempoTeoricoPerformace = 1;
                temp.VelocidadeAtingirMeta = 1;
                temp.VeloAtuPcSegundo = 1;
                temp.PerformaceProjetada = 1;
                temp.TempoDecorridoPequenasParadas = 1;
                temp.AlocadaEmMaquina = 0;
                temp.FPR_GRUPO_PRODUTIVO = 0;

                temp.CAR_INICIO_JANELA_EMBARQUE = data_aux_2200;
                temp.CAR_FIM_JANELA_EMBARQUE = data_aux_2200;
                temp.EMBARQUE_ALVO = data_fim;//.AddDays(-1); // data_aux_2200;

                temp.CLI_EXIGENTE_NA_IMPRESSAO = -1;
                temp.FPR_COR_BICO1 = "";
                temp.FPR_COR_BICO2 = "";
                temp.FPR_COR_BICO3 = "";
                temp.FPR_COR_BICO4 = "";
                temp.FPR_COR_BICO5 = "";

                //v_op_aux.GRP_ID = item.GrupoMaquina != null ? item.GrupoMaquina.GMA_ID : "";
                temp.GRP_ID = "";
                temp.GRP_TIPO = roteiro.GRP_TIPO;
                temp.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO = Convert.ToDouble(roteiro.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO);
                temp.MAQ_LARGURA_UTIL = Convert.ToDouble(roteiro.MAQ_LARGURA_UTIL);

                temp.GRP_PAP_ONDA = null;
                temp.ORD_TIPO = 1;
                temp.PrevisaoMateriaPrima = data_aux_1970;
                temp.FPR_ID_ORIGEM = 0;
                temp.FPR_DATA_ENTREGA = data_aux_1970;
                temp.EQU_ID = null;
                temp.PRO_COMPRIMENTO_PECA = roteiro.Produto.PRO_COMPRIMENTO_PECA;
                temp.PRO_LARGURA_PECA = roteiro.Produto.PRO_LARGURA_PECA;
                //v_op_aux.PRO_COMPRIMENTO_PECA = 0;
                //v_op_aux.PRO_LARGURA_PECA = 0;

                temp.Maquina = roteiro.Maquina;

                lista.Add(temp);
                //index++;
            }


            return lista;
        }

        public JsonResult OtimizarPedidosFuturos(string cargaPrincipal, List<string> listaIds)
        {
            List<OrderOpt> listaPedidos = new List<OrderOpt>();
            List<string> listaIdsPedidos = new List<string>();
            int carIdTNumero = 0;
            string nextCarId = "";
            foreach (var item in listaIds)
            {
                string temp = item.Split("_")[2];
                listaIdsPedidos.Add(temp);
            }

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                listaPedidos = db.OrderOpt.AsNoTracking().OrderBy(x => x.Id).Where(x => listaIdsPedidos.Contains(x.Id) || x.CAR_ID == cargaPrincipal).ToList();

                //Next Id do T;
                nextCarId = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Substring(0, 1).Equals("T")).Max(c => c.CAR_ID);
                carIdTNumero = (String.IsNullOrEmpty(nextCarId)) ? 1 : Convert.ToInt32(nextCarId.Substring(1, nextCarId.Length - 1));
                if (carIdTNumero < 1)
                {
                    nextCarId = "T00001";
                }
                else
                {
                    carIdTNumero++;
                    nextCarId = "T";
                    nextCarId += $"{carIdTNumero:D5}";
                }


                foreach (var P in listaPedidos)
                {
                    if (P.CAR_ID != cargaPrincipal)
                        P.IDsCargasPotenciaisParaAntecipacoes = cargaPrincipal;

                    else
                        P.CAR_STATUS = 1.2;
                }
                sheOptQueueBox b = null;
                sheOptQueueTransport t = new sheOptQueueTransport();
                OptQueueTransport t_Otimizado = null;

                OPTMiddleware run = new OPTMiddleware(DateTime.Now, 1, "", 2, b, OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport, listaPedidos, db, ref t_Otimizado);

                var result = t_Otimizado.sheOptQueueTransport.Cargas.Where(a => a.Opt == "").ToList();

                List<List<CargasWeb>> resultCargasWeb = new List<List<CargasWeb>>();
                List<CargasWeb> tempListCargasWeb;
                CargasWeb tempCarga;

                foreach (var carga in result)
                {
                    tempListCargasWeb = new List<CargasWeb>();
                    for (int i = 0; i < carga.ItensCarga.Count; i++)
                    {
                        sheItenCarga item = carga.ItensCarga.ElementAt(i);

                        tempCarga = new CargasWeb();
                        tempCarga.CAR_ID = nextCarId;
                        tempCarga.CAR_DATA_INICIO_PREVISTO = carga.CAR_DATA_INICIO_PREVISTO;
                        tempCarga.CAR_DATA_FIM_PREVISTO = carga.CAR_DATA_FIM_PREVISTO;
                        tempCarga.CAR_ID_DOCA = carga.CAR_ID_DOCA;
                        tempCarga.CAR_STATUS = carga.CAR_STATUS;
                        tempCarga.CLI_ID = item.OrderOpt.ClienteId;
                        tempCarga.CLI_NOME = item.OrderOpt.CLI_NOME;
                        tempCarga.COR_OTIF = item.OrderOpt.VIR_COR_OTIF;
                        tempCarga.DTEMBARQUE = carga.EmbarqueAlvo;
                        tempCarga.FIMPREVISTO = carga.CAR_DATA_FIM_REALIZADO;
                        tempCarga.FPR_COR_FILA = item.OrderOpt.FPR_COR_FILA;
                        tempCarga.ITC_ORDEM_ENTREGA = item.ITC_ORDEM_ENTREGA;
                        tempCarga.ITC_QTD_PLANEJADA = item.ITC_QTD_PLANEJADA;
                        tempCarga.M3_PLANEJADO = item.OrderOpt.ITC_QTD_PLANEJADA / item.OrderOpt.VIR_PECAS_POR_UE * item.OrderOpt.VIR_M3_UE;
                        tempCarga.M3_UE = item.OrderOpt.VIR_M3_UE;
                        tempCarga.OCUPACAO = Convert.ToDouble(carga.CLP_Vencedor.PercentContainerVolumePacked);
                        tempCarga.ORD_DATA_ENTREGA_DE = carga.MinEntrega;
                        tempCarga.ORD_DATA_ENTREGA_ATE = carga.MaxEntrega;
                        tempCarga.ORD_ID = item.ORD_ID;
                        tempCarga.ORD_PESO_UNITARIO = item.OrderOpt.Peso_Unitario;
                        tempCarga.ORD_QUANTIDADE = item.OrderOpt.Quantidade;
                        tempCarga.PECENT_ESTOQUE_PRONTO = item.OrderOpt.PECENT_ESTOQUE_PRONTO;
                        tempCarga.PON_ID = item.OrderOpt.PON_ID_MUN;
                        tempCarga.PON_DESCRICAO = item.OrderOpt.PON_DESCRICAO;
                        tempCarga.PRO_ALTURA_EMBALADA = item.OrderOpt.PRO_ALTURA_EMBALADA;
                        tempCarga.PRO_COMPRIMENTO_EMBALADA = item.OrderOpt.PRO_COMPRIMENTO_EMBALADA;
                        tempCarga.PRO_LARGURA_EMBALADA = item.OrderOpt.PRO_LARGURA_EMBALADA;
                        tempCarga.PRO_DESCRICAO = item.OrderOpt.PRO_DESCRICAO;
                        tempCarga.PRO_ID = item.OrderOpt.ProdutoId;
                        tempCarga.QTD_UE = carga.PedidosParaExpedicao[i].Ordens[0].VIR_QTD_SALDO_UE;
                        //tempCarga.QTD_UE = item.OrderOpt.ITC_QTD_PLANEJADA / item.OrderOpt.VIR_PECAS_POR_UE * item.OrderOpt.VIR_M3_UE;
                        tempCarga.SALDO_ESTOQUE = item.OrderOpt.SALDO_ESTOQUE;
                        tempCarga.TIP_ID = carga.TIP_ID;
                        tempCarga.TRA_NOME = "";
                        tempCarga.VEI_PLACA = carga.VEI_PLACA;

                        tempListCargasWeb.Add(tempCarga);


                    }

                    carIdTNumero++;
                    nextCarId = "T";
                    nextCarId += $"{carIdTNumero:D5}";
                    resultCargasWeb.Add(tempListCargasWeb);

                }
                //Calcular a data produtiva de cada um;
                OrderOpt tempCargaPrincipal = listaPedidos.Where(x => x.CAR_ID == cargaPrincipal).FirstOrDefault();
                DateTime dataInicio = DateTime.Now;
                DateTime dataFim = tempCargaPrincipal.DataEntregaAte;

                calcularDisponibilidadeCargaMaquina(ref resultCargasWeb);

                return Json(new { result, resultCargasWeb });
            }
        }

        [HttpPost]
        public JsonResult gravarCargasSimuladas(string listaStrCargas, string cargaPrincipal)
        {
            List<string> listaOrders = new List<string>();
            List<object> listaTemp = new List<object>();
            List<List<CargasWeb>> resultCargasWeb = JsonConvert.DeserializeObject<List<List<CargasWeb>>>(listaStrCargas);

            foreach (var lista in resultCargasWeb)
            {
                Carga carga = new Carga();
                carga.CAR_ID = lista[0].CAR_ID;
                //carga.CAR_PREVISAO_MATERIA_PRIMA = lista[0]
                carga.CAR_DATA_INICIO_PREVISTO = lista[0].CAR_DATA_INICIO_PREVISTO;
                carga.CAR_DATA_INICIO_REALIZADO = lista[0].CAR_DATA_INICIO_REALIZADO;
                carga.CAR_DATA_FIM_PREVISTO = lista[0].CAR_DATA_FIM_PREVISTO;
                //carga.CAR_DATA_FIM_REALIZADO = lista[0]
                //carga.CAR_INICIO_JANELA_EMBARQUE = lista[0]
                //carga.CAR_FIM_JANELA_EMBARQUE = lista[0]
                carga.CAR_EMBARQUE_ALVO = lista[0].CAR_EMBARQUE_ALVO;
                carga.CAR_STATUS = lista[0].CAR_STATUS;
                carga.CAR_PESO_TEORICO = lista[0].CAR_PESO_TEORICO;
                carga.CAR_VOLUME_TEORICO = lista[0].CAR_VOLUME_TEORICO;
                carga.CAR_PESO_REAL = lista[0].CAR_PESO_REAL;
                carga.CAR_VOLUME_REAL = lista[0].CAR_VOLUME_REAL;
                carga.CAR_PESO_EMBALAGEM = lista[0].CAR_PESO_EMBALAGEM;
                carga.CAR_PESO_ENTRADA = lista[0].CAR_PESO_ENTRADA;
                carga.CAR_PESO_SAIDA = lista[0].CAR_PESO_SAIDA;
                carga.CAR_ID_DOCA = lista[0].CAR_ID_DOCA;
                carga.VEI_PLACA = lista[0].VEI_PLACA;
                carga.TIP_ID = lista[0].TIP_ID;
                //carga.TRA_ID = lista[0]
                carga.CAR_GRUPO_PRODUTIVO = lista[0].GRP_TIPO;
                //carga.ROT_ID = lista[0]
                //carga.CAR_OBSERVACAO_DE_TRANSPORTE = lista[0]
                //carga.CAR_JUSTIFICATIVA_DE_CARREGAMENTO = lista[0]
                //carga.OCO_ID = lista[0].oco
                carga.CAR_ID_JUNTADA = lista[0].CAR_ID_JUNTADA;
                //carga.CAR_ID_INTEGRACAO_BALANCA = lista[0]
                //carga.CAR_OBS_LIBERACAO = lista[0]
                //carga.CAR_PESAGEM_LIBERADA = lista[0]
                carga.PlayAction = "CARGA_ANTECIPADA";


                listaTemp.Add(carga);


                foreach (var item in lista)
                {
                    listaOrders.Add(item.ORD_ID);
                    ItenCarga itenCarga = new ItenCarga();
                    itenCarga.CAR_ID = item.CAR_ID;
                    itenCarga.ORD_ID = item.ORD_ID;
                    itenCarga.ITC_ENTREGA_PLANEJADA = DateTime.Now;
                    itenCarga.ITC_ENTREGA_REALIZADA = new DateTime(1970, 01, 01);
                    itenCarga.ITC_ORDEM_ENTREGA = item.ITC_ORDEM_ENTREGA;
                    itenCarga.ITC_QTD_PLANEJADA = item.ITC_QTD_PLANEJADA;
                    itenCarga.ITC_QTD_REALIZADA = item.ITC_QTD;
                    itenCarga.PlayAction = "insert";


                    listaTemp.Add(itenCarga);
                }
            }

            List<List<object>> listaObjetos = new List<List<object>>();
            listaObjetos.Add(listaTemp);

            MasterController mc = new MasterController();
            List<LogPlay> logs = mc.UpdateData(listaObjetos, 3, true);

            int i = 0;
            string msgRetorno = resultCargasWeb[0][0].CAR_ID;
            while (i < logs.Count && logs[i].Status == "OK")
                i++;

            if (i >= logs.Count)
            {
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    string sqlExclusao = "DELETE T_ITENS_CARGA FROM T_ITENS_CARGA I INNER JOIN T_CARGA C ON C.CAR_ID = I.CAR_ID WHERE(CAR_STATUS <= 1 AND ORD_ID IN(SELECT ORD_ID FROM T_ITENS_CARGA WHERE CAR_ID = '" + resultCargasWeb[0][0].CAR_ID + "')) OR C.CAR_ID = '" + cargaPrincipal + "'";
                    db.Database.ExecuteSqlCommand(sqlExclusao);
                }
            }
            else
            {
                msgRetorno = "Erro";
            }
            string status = "OK";
            return Json(new { status, msgRetorno });
        }

        public JsonResult AvaliarCargaTotal(string idCarga)
        {








            #region
            //List<Item> itensToPack = new List<Item>();
            //Container ct = null;
            //decimal PCVP = Decimal.Zero;
            //int TTINE = 0;
            //int TOTItens = 0;
            //var db = new ContextFactory().CreateDbContext(new string[] { });
            //List<int> Algoritimo = new List<int> { 1 };
            ////string sql = "SELECT CAR_ID=I.CAR_ID, P.*,C.* FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO P (NOLOCK) ";
            ////sql += " INNER JOIN T_ITENS_CARGA I (NOLOCK) ON I.ORD_ID = Id ";
            ////sql += " INNER JOIN T_CARGA C (NOLOCK) ON C.CAR_ID = I.CAR_ID ";
            ////sql += " WHERE I.CAR_ID IN('" + idCarga + "')";
            ////sql += " ORDER BY DataEntregaDe,PRO_GRUPO_PALETIZACAO,MIT ";
            //string sql = "SELECT * FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO P (NOLOCK) ";
            //sql += " WHERE CAR_ID IN('" + idCarga + "')";
            //sql += " ORDER BY DataEntregaDe,PRO_GRUPO_PALETIZACAO,MIT ";

            //List<OrderOpt> Ordens = null;
            //Ordens = db.OrderOpt.FromSql(sql).ToList();
            //OptQueueTransport Opt = new OptQueueTransport();

            //List<sheOrderOpt> sheOrderOpt = new List<sheOrderOpt>();
            //foreach (var de in Ordens)
            //    sheOrderOpt.Add((sheOrderOpt)UtilPlay.ConverterObjetos(de, new sheOrderOpt()));


            //int idT = -1;
            //dynamic _cargaAux = null;
            //itensToPack = Opt.GetitemsToPack(null, null, null, sheOrderOpt,null);
            //if (Ordens.Count() > 0)
            //{
            //    string aux = Ordens.ElementAt(0).CAR_ID;
            //    _cargaAux = db.Carga.Where(x => x.CAR_ID == aux).Select(xx => new { xx.TIP_ID, xx.TRA_ID, xx.CAR_ID_DOCA, xx.VEI_PLACA, xx.CAR_STATUS }).FirstOrDefault();
            //    idT = _cargaAux.TIP_ID;
            //}
            //int IndexMelhorOcupacao = 0;
            //var cl = Opt.GetContainerList(DateTime.Now, 1, 30, ref IndexMelhorOcupacao);

            //List<ContainerPackingResult> CLP = PackingService.Pack(cl, itensToPack, Algoritimo);
            //AlgorithmPackingResult vencedor = Opt.GetVencedor(CLP);

            ////var NaoEmbarcadas = vencedor.UnpackedItems.OrderBy(x => x.ORD_ID).GroupBy(x => x.ORD_ID).ToList();
            ////TOTItens = vencedor.PackedItems.Count();
            ////TTINE = NaoEmbarcadas.Count();
            ////PCVP = vencedor.PercentContainerVolumePacked;
            ////itensToPack = vencedor.PackedItems;
            ////string Carga = strIds;
            ////ct = cl.Where(x => x.ID == vencedor.IDContainerVencedor).FirstOrDefault();
            ////string st = "PK";
            ////string TRA = _cargaAux.TRA_ID;
            ////string DOCA = _cargaAux.CAR_ID_DOCA;
            ////string PLACA = _cargaAux.VEI_PLACA;
            ////string STATUS = "" + _cargaAux.CAR_STATUS;

            //return Json(new { });
            #endregion

            List<Item> itensToPack = new List<Item>();
            Container ct = null;
            decimal PCVP = Decimal.Zero;
            int TTINE = 0;
            int TOTItens = 0;
            var db = new ContextFactory().CreateDbContext(new string[] { });
            List<int> Algoritimo = new List<int> { 1 };

            string sql__ = "SELECT * FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO P (NOLOCK) ";
            sql__ += " WHERE CAR_ID IN('A02311')";
            sql__ += " ORDER BY DataEntregaDe,PRO_GRUPO_PALETIZACAO,MIT ";
            List<OrderOpt> Ordens__ = null;
            Ordens__ = db.OrderOpt.FromSql(sql__).ToList();

            int idT = -1;
            dynamic _cargaAux = null;
            if (Ordens__.Count() > 0)
            {
                string aux = Ordens__.ElementAt(0).CAR_ID;
                _cargaAux = db.Carga.Where(x => x.CAR_ID == aux).Select(xx => new { xx.TIP_ID, xx.TRA_ID, xx.CAR_ID_DOCA, xx.VEI_PLACA, xx.CAR_STATUS }).FirstOrDefault();
                idT = _cargaAux.TIP_ID;
            }

            foreach (var item in Ordens__)
            {
                item.QTD_TOTAL_PLANEJADO_E_EXPEDIDO = 0;
                item.CAR_ID = "";
                if (item.ITC_QTD_PLANEJADA > 0)
                {
                    item.Quantidade = item.ITC_QTD_PLANEJADA;
                }
            }

            OptQueueTransport Opt = new OptQueueTransport();
            List<sheTipoVeiculo> sheTipoVeiculo__ = new List<sheTipoVeiculo>();

            // lista de disponibilidades de caminhao 
            string sql = " select * from V_DISPONIBILIDADE_VEICULO ";
            List<V_DISPONIBILIDADE_VEICULO> V_DISPONIBILIDADE_VEICULO = db.V_DISPONIBILIDADE_VEICULO.FromSql(sql).ToList();
            int y = 0;
            foreach (var item in V_DISPONIBILIDADE_VEICULO)
            {
                item.listIndex = y;
                y++;
            }
            foreach (var de in V_DISPONIBILIDADE_VEICULO)
                sheTipoVeiculo__.Add((sheTipoVeiculo)UtilPlay.ConverterObjetos(de, new sheTipoVeiculo()));

            Opt.sheOptQueueTransport.VeiculosDisponiveis = new List<sheTipoVeiculo>();
            Opt.sheOptQueueTransport.VeiculosDisponiveis = sheTipoVeiculo__;

            List<sheOrderOpt> __sheOrderOpt = new List<sheOrderOpt>();
            foreach (var de in Ordens__)
                __sheOrderOpt.Add((sheOrderOpt)UtilPlay.ConverterObjetos(de, new sheOrderOpt()));

            itensToPack = Opt.GetitemsToPack(null, null, null, __sheOrderOpt, null);

            int IndexMelhorOcupacao = 0;
            var cl = Opt.GetContainerList(DateTime.Now, 1, 30, ref IndexMelhorOcupacao);
            List<ContainerPackingResult> CLP = CromulentBisgetti.ContainerPacking.PackingService.Pack(cl, itensToPack, Algoritimo);
            AlgorithmPackingResult vencedor = Opt.GetVencedor(CLP);
            //var NaoEmbarcadas = vencedor.UnpackedItems.OrderBy(x => x.ORD_ID).GroupBy(x => x.ORD_ID).ToList();
            //TOTItens = vencedor.PackedItems.Count();
            //TTINE = NaoEmbarcadas.Count();
            //PCVP = vencedor.PercentContainerVolumePacked;
            //itensToPack = vencedor.PackedItems;
            //string Carga = strIds;
            //ct = cl.Where(x => x.ID == vencedor.IDContainerVencedor).FirstOrDefault();
            //string st = "PK";
            //string TRA = _cargaAux.TRA_ID;
            //string DOCA = _cargaAux.CAR_ID_DOCA;
            //string PLACA = _cargaAux.VEI_PLACA;
            //string STATUS = "" + _cargaAux.CAR_STATUS;
            return Json(new { });
        }

        [HttpGet]
        public JsonResult setAvaliarCargaTotal(string strIds)
        {

            JSgi db = new ContextFactory().CreateDbContext(new string[] { });

            List<Item> itensToPack = new List<Item>();
            Container ct = null;
            decimal PCVP = Decimal.Zero;
            int TTINE = 0;
            int TOTItens = 0;
            List<int> Algoritimo = new List<int> { 1 };

            string sql__ = "SELECT top 1 * FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO P (NOLOCK) ";
            sql__ += " WHERE CAR_ID <> ''";
            sql__ += " ORDER BY DataEntregaDe,PRO_GRUPO_PALETIZACAO,MIT ";
            List<OrderOpt> Ordens__ = null;
            Ordens__ = db.OrderOpt.FromSql(sql__).ToList();

            int idT = -1;
            dynamic _cargaAux = null;
            if (Ordens__.Count() > 0)
            {
                string aux = Ordens__.ElementAt(0).CAR_ID;
                _cargaAux = db.Carga.Where(x => x.CAR_ID == aux).Select(xx => new { xx.TIP_ID, xx.TRA_ID, xx.CAR_ID_DOCA, xx.VEI_PLACA, xx.CAR_STATUS }).FirstOrDefault();
                idT = _cargaAux.TIP_ID;
            }

            foreach (var item in Ordens__)
            {
                item.QTD_TOTAL_PLANEJADO_E_EXPEDIDO = 0;
                item.CAR_ID = "";
                if (item.ITC_QTD_PLANEJADA > 0)
                {
                    item.Quantidade = item.ITC_QTD_PLANEJADA;
                }
            }

            OptQueueTransport Opt = new OptQueueTransport();
            List<sheTipoVeiculo> sheTipoVeiculo__ = new List<sheTipoVeiculo>();

            // lista de disponibilidades de caminhao 
            string sql = " select * from V_DISPONIBILIDADE_VEICULO ";
            List<V_DISPONIBILIDADE_VEICULO> V_DISPONIBILIDADE_VEICULO = db.V_DISPONIBILIDADE_VEICULO.FromSql(sql).ToList();
            int y = 0;
            foreach (var item in V_DISPONIBILIDADE_VEICULO)
            {
                item.listIndex = y;
                y++;
            }
            foreach (var de in V_DISPONIBILIDADE_VEICULO)
                sheTipoVeiculo__.Add((sheTipoVeiculo)UtilPlay.ConverterObjetos(de, new sheTipoVeiculo()));

            Opt.sheOptQueueTransport.VeiculosDisponiveis = new List<sheTipoVeiculo>();
            Opt.sheOptQueueTransport.VeiculosDisponiveis = sheTipoVeiculo__;

            List<sheOrderOpt> __sheOrderOpt = new List<sheOrderOpt>();
            foreach (var de in Ordens__)
                __sheOrderOpt.Add((sheOrderOpt)UtilPlay.ConverterObjetos(de, new sheOrderOpt()));

            itensToPack = Opt.GetitemsToPack(null, null, null, __sheOrderOpt, null);

            int IndexMelhorOcupacao = 0;
            var cl = Opt.GetContainerList(DateTime.Now, 1, 30, ref IndexMelhorOcupacao);
            List<ContainerPackingResult> CLP = CromulentBisgetti.ContainerPacking.PackingService.Pack(cl, itensToPack, Algoritimo);
            AlgorithmPackingResult vencedor = Opt.GetVencedor(CLP);
            //var NaoEmbarcadas = vencedor.UnpackedItems.OrderBy(x => x.ORD_ID).GroupBy(x => x.ORD_ID).ToList();
            //TOTItens = vencedor.PackedItems.Count();
            //TTINE = NaoEmbarcadas.Count();
            //PCVP = vencedor.PercentContainerVolumePacked;
            //itensToPack = vencedor.PackedItems;
            //string Carga = strIds;
            //ct = cl.Where(x => x.ID == vencedor.IDContainerVencedor).FirstOrDefault();
            //string st = "PK";
            //string TRA = _cargaAux.TRA_ID;
            //string DOCA = _cargaAux.CAR_ID_DOCA;
            //string PLACA = _cargaAux.VEI_PLACA;
            //string STATUS = "" + _cargaAux.CAR_STATUS;




            var NaoEmbarcadas = vencedor.UnpackedItems.OrderBy(x => x.ORD_ID).GroupBy(x => x.ORD_ID).ToList();
            TOTItens = vencedor.PackedItems.Count();
            TTINE = NaoEmbarcadas.Count();
            PCVP = vencedor.PercentContainerVolumePacked;
            itensToPack = vencedor.PackedItems;
            string Carga = strIds;
            ct = cl.Where(x => x.ID == vencedor.IDContainerVencedor).FirstOrDefault();
            string st = "PK";
            string TRA = _cargaAux.TRA_ID;
            string DOCA = _cargaAux.CAR_ID_DOCA;
            string PLACA = _cargaAux.VEI_PLACA;
            string STATUS = "" + _cargaAux.CAR_STATUS;

            return Json(new { st, itensToPack, ct, PCVP, TTINE, TOTItens, NaoEmbarcadas, strIds, idT, TRA, DOCA, PLACA, STATUS });
        }

        [HttpPost]
        public JsonResult GerenciarCargas(string pedidoId, string old_carga, string new_carga, string old_ord_entrega, string new_ord_entrega, string old_itc_qtd_plan, string new_itc_qtd_plan, string old_tipo_caminhao, string new_tipo_caminhao)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                MasterController mc = new MasterController();
                List<object> _ItensCarga = new List<object>();
                List<List<object>> ListOfListObjects;
                int.TryParse(new_itc_qtd_plan, out int qtdRetirar);
                int.TryParse(new_ord_entrega, out int ordenEntrega);
                int.TryParse(new_tipo_caminhao, out int tipoCaminhao);
                string msg = "OK";
                //Carga Destino
                Carga Db_CargaDestino = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Equals(new_carga)).FirstOrDefault();
                Carga Db_CargaOrigem;
                if (new_carga.Equals(old_carga) && old_tipo_caminhao != new_tipo_caminhao)
                {
                    Db_CargaOrigem = db.Carga.AsNoTracking().Where(c => c.CAR_ID.Equals(new_carga)).FirstOrDefault();
                    Db_CargaOrigem.TIP_ID = tipoCaminhao;
                    Db_CargaOrigem.PlayAction = "UPDATE";
                    List<object> list_Carga = new List<object>() { Db_CargaOrigem };
                    ListOfListObjects = new List<List<object>> { list_Carga };

                    List<LogPlay> logs3 = mc.UpdateData(ListOfListObjects, 0, true);
                    var resp2 = new LogPlay().GetLogsErro(logs3);
                    if (resp2.Count > 0)
                    {
                        msg = "";
                        foreach (var item in resp2)
                        {
                            msg += item.MsgErro;
                        }
                    }
                    return Json(new { msg });
                }
                else if (new_carga.Equals(old_carga))
                {
                    msg = "A carga informada [" + new_carga + "] é a mesma de origem, verifique a informação!";
                    return Json(new { msg });
                }
                if (qtdRetirar <= 0)
                {
                    msg = "A quantidade informada[" + qtdRetirar + "] deve ser maior que 0!";
                    return Json(new { msg });
                }

                if (Db_CargaDestino == null)
                {
                    msg = "A carga informada [" + new_carga + "] como destino não foi encontrada, verifique o codigo informado!";
                    return Json(new { msg });
                }
                Db_CargaDestino.PlayAction = "update";
                Db_CargaDestino.CAR_STATUS = 1;
                if (Db_CargaDestino.TIP_ID != tipoCaminhao)
                {
                    Db_CargaDestino.TIP_ID = tipoCaminhao;
                }
                //--Novo item de Carga criado
                ItenCarga _NovoItem = new ItenCarga() { CAR_ID = new_carga, PlayAction = "OK", ORD_ID = pedidoId, ITC_ORDEM_ENTREGA = ordenEntrega, PlayMsgErroValidacao = "" };

                //Consultando se o pedido já se encontra dividido
                var Db_CargasComPedidoAtual = db.ItenCarga.AsNoTracking().Where(ic => ic.ORD_ID.Equals(pedidoId)).Select(o => o.CAR_ID).Distinct().ToList();
                //Consultando se o Pedido já consta na Carga de destino
                var Db_ItensCargaDestino = db.ItenCarga.AsNoTracking().Where(ic => ic.ORD_ID.Equals(pedidoId) && ic.CAR_ID.Equals(new_carga)).Select(o => o.CAR_ID).Distinct().ToList();
                //Itens de carga a serem movidos da origem
                var Db_ItenRetirarOrigem = db.ItenCarga.Include(i => i.Oredr).Where(ic => ic.CAR_ID.Equals(old_carga) && ic.ORD_ID.Equals(pedidoId)).FirstOrDefault();

                // O Pedido já está dividido em outras duas cargas?
                //if (!Db_ItensCargaDestino.Contains(new_carga) && Db_CargasComPedidoAtual.Count == 2)
                //{
                //msg = "O Pedido [" + pedidoId + "] já se encontra dividido em duas cargas, exclua-o de uma das cargas para realizar esta operação.";
                //return Json(new { msg });
                //}
                var Db_ICDestino = db.ItenCarga.AsNoTracking().Where(ic => ic.ORD_ID.Equals(pedidoId) && ic.CAR_ID.Equals(new_carga)).FirstOrDefault();
                // Se Pedido está contido na carga destino
                if (Db_ItensCargaDestino.Contains(new_carga) && Db_CargasComPedidoAtual.Count == 2)
                {
                    var _IsRomaneada = db.MovimentoEstoque.Where(mv => mv.CAR_ID == Db_ICDestino.CAR_ID && mv.ORD_ID == Db_ICDestino.ORD_ID && mv.TIP_ID.Equals("998")).Count();
                    if (qtdRetirar > Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA)
                    {
                        msg = "A quantidade informada[" + qtdRetirar + "] deve ser menor ou igual a [" + Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA + "]!";
                        return Json(new { msg });
                    }
                    //Se a quantidade  a retirar é igual ao total da origem
                    if (_IsRomaneada == 0)
                    {
                        if (qtdRetirar == Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA)
                        {
                            Db_ICDestino.ITC_QTD_PLANEJADA = Db_ICDestino.ITC_QTD_PLANEJADA + qtdRetirar;
                            Db_ItenRetirarOrigem.PlayAction = "delete";
                            Db_ICDestino.PlayAction = "update";
                        }
                        else
                        {
                            Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA = Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA - qtdRetirar;
                            Db_ItenRetirarOrigem.PlayAction = "update";
                            Db_ICDestino.ITC_QTD_PLANEJADA = Db_ICDestino.ITC_QTD_PLANEJADA + qtdRetirar;
                            Db_ICDestino.PlayAction = "update";
                        }
                    }
                    else
                    {
                        msg = "Operação inválida. A Carga [" + old_carga + "] já foi Romaneada";
                        return Json(new { msg });
                    }

                }
                else// O Pedido não está contido na carga de destino
                {
                    var _IsRomaneada = db.MovimentoEstoque.Where(mv => mv.CAR_ID == Db_CargaDestino.CAR_ID && mv.ORD_ID == pedidoId && mv.TIP_ID.Equals("998")).Count();

                    //A quantidade informada pelo usuário é igual a da op 
                    if (qtdRetirar == Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA)
                    {
                        //Se a carga nao foi romaneada
                        if (_IsRomaneada == 0)
                        {
                            Db_ItenRetirarOrigem.PlayAction = "delete";
                            _NovoItem.ITC_QTD_PLANEJADA = qtdRetirar;
                            _NovoItem.PlayAction = "insert";
                        }
                        else
                        {
                            msg = "Operação inválida. A Carga [" + old_carga + "] já foi Romaneada";
                            return Json(new { msg });
                        }
                    }
                    else
                    {
                        //Se a carga nao foi romaneada
                        if (_IsRomaneada == 0)
                        {
                            Db_ItenRetirarOrigem.PlayAction = "update";
                            Db_ItenRetirarOrigem.ITC_QTD_PLANEJADA -= qtdRetirar;
                            _NovoItem.ITC_QTD_PLANEJADA = qtdRetirar;
                            _NovoItem.PlayAction = "insert";
                        }
                        else
                        {
                            msg = "Operação inválida. A Carga [" + old_carga + "] já foi Romaneada";
                            return Json(new { msg });
                        }
                    }

                }
                //Adicionando objetos na lista para o UpdateData()
                if (Db_ICDestino != null)
                    _ItensCarga.Add(Db_ICDestino);
                _ItensCarga.Add(Db_ItenRetirarOrigem);
                _ItensCarga.Add(_NovoItem);
                _ItensCarga.Add(Db_CargaDestino);
                ListOfListObjects = new List<List<object>> { _ItensCarga };
                List<LogPlay> logs2 = mc.UpdateData(ListOfListObjects, 0, true);
                var resp = new LogPlay().GetLogsErro(logs2);
                if (resp.Count > 0)
                {
                    msg = "";
                    foreach (var item in resp)
                    {
                        msg += item.MsgErro;
                    }
                }
                return Json(new { msg });
            }
        }

        public JsonResult CarregarOnduladeira()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var onduladeira = db.CorridasOnduladeira.AsNoTracking().
                    OrderBy(x => x.BOL_ID).
                    ThenBy(x => x.COR_INICIO_PREVISTO).
                    ToList();

                if (onduladeira.Count > 0)
                {

                    //Fazer função para pegar o valor mínimo de todas as onduladeiras.
                    DateTime DtIni = new DateTime(1990, 01, 01); //Data muito pequena para não ter possibilidade de um item da fila ser menor que esta data.
                    var mimDtIni = onduladeira.Min(m => m.COR_INICIO_PREVISTO);
                    var DtFim = onduladeira.Max(m => m.COR_FIM_PREVISTO);

                    if (DtIni > mimDtIni || DateTime.Compare(DtIni, new DateTime(1990, 01, 01)) == 0) //Data muito pequena declarada anteriormente
                    {
                        DtIni = mimDtIni;
                    }

                    for (int i = 0; i < onduladeira.Count; i++)
                    {
                        onduladeira[i].MinutoIni = (onduladeira[i].COR_INICIO_PREVISTO - DtIni).TotalMinutes;
                        onduladeira[i].MinutoFim = (onduladeira[i].COR_FIM_PREVISTO - DtIni).TotalMinutes;
                    }


                    double tamanhoTotal = (DtFim - DtIni).TotalMinutes;


                    Console.WriteLine(DataInicial);
                    DateTime fila_ini = new DateTime(1990, 01, 01); //Data muito pequena para não ter possibilidade de um item da fila ser menor que esta data.
                    DateTime fila_fim = DateTime.Now.AddMonths(1);

                    DateTime fila_inicio = db.ViewFilaProducao
                        .Where(v => v.MAQ_TIPO_PLANEJAMENTO != "SERVICO_COMPRAS" &&
                            v.FprDataInicioPrevista >= fila_ini &&
                            v.FprDataFimPrevista <= fila_fim)
                        .OrderBy(f => f.FprDataInicioPrevista)
                        .Select(f => f.FprDataInicioPrevista)
                        .FirstOrDefault();

                    double tamanhoInicial = (DtIni - fila_inicio).TotalMinutes;

                    return Json(new { tamanhoTotal, tamanhoInicial, DtIni, DtFim, onduladeira });
                }

                return Json(new { onduladeira });
            }
        }

        public JsonResult obterFilaOndu(string ord_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime DtIni = new DateTime(1990, 01, 01);
                //OrdId
                var fila = db.ViewFilaProducao.Where(v => v.OrdId == ord_id)
                    //.OrderBy(f => new { f.MaqDescricao, f.RotMaqId, f.OrdemDaFila, f.RotSeqTransformacao }).ToList()
                    .OrderBy(f => f.MaqDescricao)
                    .ThenBy(f => f.RotMaqId)
                    .ThenBy(f => f.FprDataInicioPrevista)
                    .ThenBy(f => f.RotSeqTransformacao)
                    .Select(f => new FilaWeb
                    {
                        maquina = f.MaqDescricao,
                        maquinaId = f.RotMaqId,
                        grupoMaquinaId = f.GMA_ID,
                        produtoId = f.PaProId,
                        pedidoId = f.OrdId,
                        produto = f.PaProDescricao,
                        grpDescricao = f.GRP_DESCRICAO,
                        inicioPrevisto = f.FprDataInicioPrevista,//.ToString("dd/MM/yyyy HH:mm:ss"),
                        fimPrevisto = f.FprDataFimPrevista,//.ToString("dd/MM/yyyy HH:mm:ss"),
                        EntregaDe = f.OrdDataEntregaDe,
                        EntregaAte = f.OrdDataEntregaAte,
                        seqTransform = f.RotSeqTransformacao,
                        seqRepet = f.FprSeqRepeticao,
                        truncado = f.Truncado.Trim(),
                        dataInicioTrunc = f.DataInicioTrunc,
                        dataFimTrunc = f.DataFimTrunc,
                        qtd = f.FprQuantidadePrevista,
                        CorFila = f.CorFila,
                        CorOrd = f.CorOrd,
                        // conceito de escala de tempo a partir de agora em minutos
                        MinutoIni = 0,
                        MinutoFim = 0,
                        MinutoIniTrunc = 0,
                        MinutoFimTrunc = 0,
                        Id = f.Id,// id fila
                        CongelaFila = f.CongelaFila,
                        OrdemDaFila = f.OrdemDaFila,
                        PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                        InicioJanelaEmbarque = f.OrdInicioJanelaEmbarque,
                        FimJanelaEmbarque = f.OrdFimJanelaEmbarque,
                        EmbarqueAlvo = f.OrdEmbarqueAlvo,
                        GrupoProdutivo = f.FPR_GRUPO_PRODUTIVO,
                        InicioGrupoProdutivo = f.OrdInicioGrupoProdutivo,
                        FimGrupoProdutivo = f.OrdFimGrupoProdutivo,
                        DataNecessidadeInicioProducao = f.DataHoraNecessidadeInicioProducao,
                        DataNecessidadeFimProducao = f.DataHoraNecessidadeFimProducao,
                        TipoCarregamento = f.OrdTipoCarregamento,
                        Status = f.ORD_STATUS,
                        OrdOpIntegracao = f.ORD_OP_INTEGRACAO,
                        CliId = f.CliId,
                        CliNome = f.CliNome,
                        PecasPorPulso = f.RotQuantPecasPulso,
                        M2Unitario = f.ORD_M2_UNITARIO,
                        M2Total = f.ORD_M2_TOTAL,
                        Performance = f.ROT_PERFORMANCE,
                        TempoSetup = f.FPR_META_SETUP,
                        TempoSetupA = 0,
                        CorBico1 = f.CorBico1,
                        CorBico2 = f.CorBico2,
                        CorBico3 = f.CorBico3,
                        CorBico4 = f.CorBico4,
                        CorBico5 = f.CorBico5,
                        hieranquiaSeqTransf = f.FPR_HIERARQUIA_SEQ_TRANSFORMACAO

                    }).ToList();


                var fila2 = fila.Select(f => new FilaWeb
                {
                    maquina = f.maquina,
                    maquinaId = f.maquinaId,
                    grupoMaquinaId = f.grupoMaquinaId,
                    produtoId = f.produtoId,
                    pedidoId = f.pedidoId,
                    produto = f.produto,
                    grpDescricao = f.grpDescricao,
                    inicioPrevisto = f.inicioPrevisto,//.ToString("dd/MM/yyyy HH:mm:ss"),
                    fimPrevisto = f.fimPrevisto,//.ToString("dd/MM/yyyy HH:mm:ss"),
                    EntregaDe = f.EntregaDe,
                    EntregaAte = f.EntregaAte,
                    seqTransform = f.seqTransform,
                    seqRepet = f.seqRepet,
                    truncado = f.truncado.Trim(),
                    dataInicioTrunc = f.dataInicioTrunc,
                    dataFimTrunc = f.dataFimTrunc,
                    qtd = f.qtd,
                    CorFila = f.CorFila,
                    CorOrd = f.CorOrd,
                    // conceito de escala de tempo a partir de agora em minutos
                    MinutoIni = (f.inicioPrevisto - DtIni).TotalMinutes,
                    MinutoFim = (f.fimPrevisto - DtIni).TotalMinutes,
                    MinutoIniTrunc = (f.truncado != "") ? (f.dataInicioTrunc - DtIni).TotalMinutes : 0,
                    MinutoFimTrunc = (f.truncado != "") ? (f.dataFimTrunc - DtIni).TotalMinutes : 0,
                    Id = f.Id,// id fila
                    CongelaFila = f.CongelaFila,
                    OrdemDaFila = f.OrdemDaFila,
                    PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                    InicioJanelaEmbarque = f.InicioJanelaEmbarque,
                    FimJanelaEmbarque = f.FimJanelaEmbarque,
                    EmbarqueAlvo = f.EmbarqueAlvo,
                    GrupoProdutivo = f.GrupoProdutivo,
                    InicioGrupoProdutivo = f.InicioGrupoProdutivo,
                    FimGrupoProdutivo = f.FimGrupoProdutivo,
                    DataNecessidadeInicioProducao = f.DataNecessidadeInicioProducao,
                    DataNecessidadeFimProducao = f.DataNecessidadeFimProducao,
                    TipoCarregamento = f.TipoCarregamento,
                    Status = f.Status,
                    OrdOpIntegracao = f.OrdOpIntegracao,
                    CliId = f.CliId,
                    CliNome = f.CliNome,
                    PecasPorPulso = f.PecasPorPulso,
                    M2Unitario = f.M2Unitario,
                    M2Total = f.M2Total,
                    Performance = f.Performance,
                    TempoSetup = f.TempoSetup,
                    TempoSetupA = f.TempoSetupA,
                    CorBico1 = f.CorBico1,
                    CorBico2 = f.CorBico2,
                    CorBico3 = f.CorBico3,
                    CorBico4 = f.CorBico4,
                    CorBico5 = f.CorBico5,
                    hieranquiaSeqTransf = f.hieranquiaSeqTransf

                }).ToList();

                return Json(fila2);
            }
        }

        /// <summary>
        /// Recebe um objeto json para retornar um protocolo que irá abrir a tela de desmontagem de lotes.
        /// </summary>
        /// <param name="item_json"></param>
        /// <returns></returns>
        [HttpPost]
        public List<LogPlay> DesmontarLote(string item_json)
        {
            List<LogPlay> logs = new List<LogPlay>();
            dynamic item = JsonConvert.DeserializeObject<dynamic>(item_json);

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string mov_lote = item.MOV_LOTE == null ? "" : item.MOV_LOTE;
                string mov_sub_lote = item.MOV_SUB_LOTE == null ? "" : item.MOV_SUB_LOTE;
                string pro_id = item.PRO_ID == null ? "" : item.PRO_ID;
                string ord_id = item.ORD_ID == null ? "" : item.ORD_ID;
                string mov_endereco = item.MOV_ENDERECO == null ? "" : item.MOV_ENDERECO;

                var max_sub_lote = db.MovimentoEstoque.AsNoTracking()
                    .Where(x => x.MOV_LOTE == mov_lote && x.PRO_ID == pro_id)
                    .Select(x => Int32.Parse(x.MOV_SUB_LOTE)).Max() + 1;

                string name_space = "DynamicForms.Areas.PlugAndPlay.Models.InterfaceTelaTransferenciaSimples";

                InterfaceTelaTransferenciaSimples parametros = new InterfaceTelaTransferenciaSimples()
                {
                    MOV_LOTE_ORIGEM = mov_lote,
                    MOV_SUB_LOTE_ORIGEM = mov_sub_lote,
                    MOV_LOTE_DESTINO = mov_lote,
                    MOV_SUB_LOTE_DESTINO = max_sub_lote.ToString(),
                    PRO_ID_DESTINO = pro_id,
                    ORD_ID = ord_id,
                    MOV_ENDERECO = mov_endereco
                };

                //transforma o objeto em uma string JSON para enviar por GET quando o protocolo for executado
                string string_parametros = JsonConvert.SerializeObject(parametros);

                LogPlay protocolo = new LogPlay() { Status = "LINK", MsgErro = $"/DynamicWeb/LinkSGI?str_namespace={name_space}&ArrayDeValoresDefault={string_parametros}", NomeAtributo = "PROTOCOLO" };
                logs.Add(protocolo);
            }

            return logs;
        }
    }

}


