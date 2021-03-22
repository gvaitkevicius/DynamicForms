using CromulentBisgetti.ContainerPacking.Entities;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using OptShered;
using System;
using System.Collections.Generic;
using System.Linq;
using OptTransport;
using System.Text;
using DynamicForms.Util;
using GeoCoordinatePortable;
using OptBox;
using System.Reflection;
using System.Threading.Tasks;
using DynamicForms.Models;

namespace OptMiddleware
{
    public class OPTMiddleware
    {
        public DateTime Total = DateTime.Now;
        public DateTime SubtotalTotal = DateTime.Now;
        public DateTime tempo = DateTime.Now;
        public List<sheMenssagens> Menssagens = new List<sheMenssagens>();
        public JSgi db = null;//  new JSgi();
        public List<int> IDsCalendarioExpedicao = new List<int>();
        public List<Param> Parametros;
        private DateTime Agora { get; set; }
        public sheNodo ori { get; set; }
        public GeoCoordinate GeoNodoOri { get; set; }
        //sheParametros middleware
        public void AddManssagem(sheMenssagens msg, double RunType)
        {
            this.Menssagens.Add(msg);
            OPTParametrosSingleton.MsgSingleton(msg.MEN_TYPE, msg.MEN_SEND, RunType);
        }

        private void CheckSun(sheOptQueueBox Box)
        {
            string maquina = "";
            double GRP = -1;
            string TIPO_COLOCACAO_FILA = "";
            sheFilaDoSchedule OPAnterior = new sheFilaDoSchedule();
            foreach (var item in Box.filaOrdenada.OrderBy(x => x.MaquinaId).ThenBy(x => x.DataInicioPrevista))
            {
                if (item.OPAnterior != null && item.DataInicioPrevista < item.OPAnterior.DataInicioPrevista && item.ProdutoId == item.ORD_PRO_ID)
                {
                    this.AddManssagem(new sheMenssagens("", "ATENCAO", "<ATENCAO> Inicio previsto da sequencia atual menor que inicio da sequencia anterior " + item.OrderId + "/" + item.SequenciaTransformacao + "/" + item.SequenciaRepeticao + " - " + item.ProdutoId), 0);
                }
                if (item.OPAnterior != null && item.DataFimPrevista < item.OPAnterior.DataFimPrevista && item.ProdutoId == item.ORD_PRO_ID)
                {
                    this.AddManssagem(new sheMenssagens("", "ERRO", "<ERRO> Fim previsto da sequencia atual maior que Fim da sequencia anterior " + item.OrderId + "/" + item.SequenciaTransformacao + "/" + item.SequenciaRepeticao + " - " + item.ProdutoId), 0);
                }

                if (maquina != item.MaquinaId)
                {
                    GRP = -1;
                }
                // grupo produtivo maior que anterior 
                if (GRP != -1 && GRP > item.FPR_GRUPO_PRODUTIVO && TIPO_COLOCACAO_FILA != "E" && item.PrevisaoMateriaPrima <= OPAnterior.DataInicioPrevista)
                {
                    this.AddManssagem(new sheMenssagens("", "ERRO", "<ERRO> Grupo produtivo OP anterior maior. OP atual: " + item.OrderId + "/" + item.SequenciaTransformacao + "/" + item.SequenciaRepeticao + " - " + item.ProdutoId + " OP integracao:" + item.IdIntegracao), 0);
                }

                OPAnterior = item;
                GRP = item.FPR_GRUPO_PRODUTIVO;
                maquina = item.MaquinaId;
                TIPO_COLOCACAO_FILA = item.TIPO_COLOCACAO_FILA;
            }
        }
        private void StartOpt()
        {
            try
            {
                // ATUALIZA PARAMETROS DE INICIO DO PROCESSO 
                //string sql = " UPDATE T_PARAMETROS SET PAR_VALOR_N = PAR_VALOR_N+1 WHERE PAR_ID = 'SCHEDULE_ID_LOTE';UPDATE T_PARAMETROS SET PAR_VALOR_D = GETDATE() WHERE PAR_ID = 'SCHEDULE_DATA_INICIO';UPDATE T_PARAMETROS SET PAR_VALOR_S = 'RUN - TRANSPORTE - '+CONVERT(varchar, getdate(), 100) WHERE PAR_ID = 'SCHEDULE_STATUS'; ";
                //this.db.Database.ExecuteSqlCommand(sql);

            }
            catch (Exception e)
            {
                // pendencia encio de menssagens 
                throw;
            }


        }
        private void StartInterface()
        {
            // TEMPORARIO PENDENCIA
            //db.Database.ExecuteSqlCommand("EXEC SP_PLUG_INTERFACE_BEFORE_INPUTS ");
            //db.Database.ExecuteSqlCommand("EXEC SP_PLUG_INTERFACE_AFTER_INPUTS ''	, '','',0,0 ");
        }
        // transporte 
        public DateTime FiltroOtimizador(int diasAfrente)
        {
            DateTime d = Agora.AddDays(diasAfrente);
            //if (d.DayOfWeek == DayOfWeek.Friday)
            //    d = d.AddDays(2);
            //else if (d.DayOfWeek == DayOfWeek.Saturday)
            //    d = d.AddDays(1);
            return d;
        }
        public static object ConverterObjetos(object De, object Para)
        {
            PropertyInfo[] propertiesObj1 = Para.GetType().GetProperties();
            PropertyInfo[] propertiesObj2 = De.GetType().GetProperties();
            foreach (PropertyInfo property in propertiesObj1)
            {
                PropertyInfo prop = propertiesObj2.Where(p => p.Name == property.Name).FirstOrDefault();
                if (prop != null && property.Name.ToUpper() != "MAQUINA")
                {
                    object valuePropObj2 = prop.GetValue(De);
                    property.SetValue(Para, valuePropObj2);
                }
            }

            return Para;
        }



        // construtor FULL model
        public OPTMiddleware(DateTime Agora, int TipoHeuristica, string DebugLog, double RunType,
            sheOptQueueBox Parametro_sheOptQueueBox,
            sheOptQueueTransport Parametro_sheOptQueueTransport,
            List<OrderOpt> Parametro_Ordens_V_PEDIDOS_A_PLANEJAR_EXPEDICAO,
            JSgi Parametro_db, ref OptQueueTransport Parametro_Ref_OptQueueTransport) // RunType    0 completo produção e expedição  1 somente produção    2 expedição Customizada 
        {

            try
            {
                if (RunType < 2)
                {
                    OPTParametrosSingleton.Instance.Cancelar = false;
                    OPTParametrosSingleton.Instance.semaforoOtimizador = "USING";
                }
                if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "0", RunType))
                    return;

                if (Parametro_db == null)
                {
                    if (this.db == null)
                        this.db = new ContextFactory().CreateDbContext(new string[] { });
                }
                else
                {
                    this.db = Parametro_db;
                }

                if (1 == 2)
                {// teste  para rodar na web
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
                }


                string sqlLog = "";
                int diasAfrente = 15;

                this.Agora = Agora;
                this.Menssagens = new List<sheMenssagens>();
                if (true)
                {
                    Console.WriteLine("Database: " + this.db.Database.GetDbConnection().Database);
                    this.StartInterface();
                    this.StartOpt();

                    #region transporte
                    int timeout = (int)this.db.Database.GetCommandTimeout();
                    this.db.Database.SetCommandTimeout(10000);

                    if (RunType == 0)
                    {
                        // DELETA OPS SIMULADAS 
                        db.Database.ExecuteSqlCommand(" DELETE FROM T_FILA_PRODUCAO WHERE ORD_ID IN (SELECT ORD_ID FROM T_ORDENS WHERE ORD_STATUS = 'S1' ) ");

                        /*DELETE FROM T_ITENS_CARGA WHERE 
                            ORD_ID IN (SELECT I.ORD_ID FROM T_CARGA C 
                            INNER JOIN T_ITENS_CARGA I ON C.CAR_ID = I.CAR_ID 
                            LEFT JOIN T_ORDENS O ON O.ORD_ID = I.ORD_ID AND LEFT(ORD_STATUS,1)<> 'E'
                            WHERE  CAR_STATUS > 1 and CAR_STATUS < 6 
                            AND O.ORD_ID IS NULL )

                            DELETE FROM T_CARGA WHERE 
                            CAR_ID IN (SELECT C.CAR_ID FROM T_CARGA C 
                            LEFT JOIN T_ITENS_CARGA I ON C.CAR_ID = I.CAR_ID 
                            WHERE  CAR_STATUS > 1 and CAR_STATUS < 6 
                            AND I.CAR_ID IS NULL )

                            DELETE FROM T_FILA_PRODUCAO WHERE ORD_ID IN (
                            SELECT ORD_ID FROM T_ORDENS 
                            LEFT JOIN T_CARGA C ON C.CAR_ID = ORD_ID 
                            WHERE ORD_TIPO = '100' AND  C.CAR_ID IS NULL )

                            DELETE FROM T_ORDENS WHERE ORD_ID IN (
                            SELECT ORD_ID FROM T_ORDENS 
                            LEFT JOIN T_CARGA C ON C.CAR_ID = ORD_ID 
                            WHERE ORD_TIPO = '100' AND  C.CAR_ID IS NULL )
                        */
                        db.Database.ExecuteSqlCommand(" DELETE FROM T_FILA_PRODUCAO WHERE ORD_ID IN (SELECT ORD_ID FROM T_ORDENS WHERE ORD_TIPO = 100 AND  ORD_ID IN (SELECT CAR_ID  FROM T_CARGA WHERE CAR_STATUS <= 1)  ) ");
                        db.Database.ExecuteSqlCommand(" delete from T_ORDENS WHERE ORD_TIPO = 100  AND  ORD_ID IN (SELECT CAR_ID  FROM T_CARGA WHERE CAR_STATUS <= 1)   ");
                        db.Database.ExecuteSqlCommand(" delete from T_ITENS_PACKED WHERE CAR_ID IN (SELECT CAR_ID  FROM T_CARGA WHERE CAR_STATUS <= 1) ");
                        db.Database.ExecuteSqlCommand(" DELETE FROM T_ITENS_CARGA WHERE CAR_ID IN (" +
                                                      " SELECT I.CAR_ID FROM T_ITENS_CARGA I " +
                                                      " INNER JOIN T_CARGA C ON I.CAR_ID = C.CAR_ID " +
                                                      " WHERE CAR_STATUS <= 1 ) ");
                        db.Database.ExecuteSqlCommand(" DELETE FROM T_CARGA WHERE CAR_STATUS <= 1 ");
                    }
                    //db.Database.ExecuteSqlCommand(" SP_PLUG_SET_LOG '" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " INICIO FILAS EXPEDICAO', 'FILAS_EXPED_START','SCHEDULE_FILAS_EXPED' ");


                    // lista de disponibilidades de caminhao 
                    string sql = " select * from V_DISPONIBILIDADE_VEICULO ";// " WHERE CDV_DATA_DE >= '" + Agora.AddDays(-1).ToString("yyyyMMdd HH:mm:ss") + "'    ";
                    List<V_DISPONIBILIDADE_VEICULO> V_DISPONIBILIDADE_VEICULO = null;
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.VeiculosDisponiveis == null || Parametro_sheOptQueueTransport.VeiculosDisponiveis.Count() == 0)
                    {
                        V_DISPONIBILIDADE_VEICULO = db.V_DISPONIBILIDADE_VEICULO.FromSql(sql).ToList();
                        int y = 0;
                        foreach (var item in V_DISPONIBILIDADE_VEICULO)
                        {
                            item.listIndex = y;
                            y++;
                        }
                    }

                    //BUSCA CALENDARIO DE EXPEDIÇÃO 
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.IDsCalendariosExpedicao == null || Parametro_sheOptQueueTransport.IDsCalendariosExpedicao.Count() == 0)
                    {
                        var Docas = db.Maquina.Where(x => x.GMA_ID == "SRV_EXPEDICAO" && x.CAL_ID != null).Select(z => new { ID = (int)z.CAL_ID }).Distinct().ToList();
                        foreach (var c in Docas)
                        {
                            IDsCalendarioExpedicao.Add(c.ID);
                        }
                    }

                    string MUN_ID_ENTREGA = "";
                    // pendencia singoton 
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.Parametros == null || Parametro_sheOptQueueTransport.Parametros.Count() == 0)
                    {
                        if (Parametros == null)
                        {
                            Parametros = db.Param.ToList();
                        }
                        diasAfrente = (int)Parametros.Where(x => x.PAR_ID == "OTIMIZA_NUMERO_DIAS").FirstOrDefault().PAR_VALOR_N;
                        MUN_ID_ENTREGA = Parametros.Where(x => x.PAR_ID == "EXPEDICAO_MUN_ID_ORIGEM").FirstOrDefault().PAR_VALOR_S;
                    }
                    else
                    {
                        diasAfrente = (int)Parametro_sheOptQueueTransport.Parametros.Where(x => x.PAR_ID == "OTIMIZA_NUMERO_DIAS").FirstOrDefault().PAR_VALOR_N;
                        MUN_ID_ENTREGA = Parametro_sheOptQueueTransport.Parametros.Where(x => x.PAR_ID == "EXPEDICAO_MUN_ID_ORIGEM").FirstOrDefault().PAR_VALOR_S;
                    }


                    sql = " select * FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO P ";
                    if (diasAfrente > 0)
                        sql += " WHERE convert(varchar, DataEntregaDe  ,112)  <= '" + FiltroOtimizador(diasAfrente).ToString("yyyyMMdd") + "'";
                    sql += "  ORDER BY Id ";
                    List<OrderOpt> Ordens = null;
                    Ordens = Parametro_Ordens_V_PEDIDOS_A_PLANEJAR_EXPEDICAO;
                    if (Ordens == null)
                        Ordens = db.OrderOpt.FromSql(sql).ToList();


                    List<Mapa> Mapas = null;
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.Mapa == null || Parametro_sheOptQueueTransport.Mapa.Count() == 0)
                    {
                        if (Mapas == null)
                            Mapas = db.Mapa.ToList();
                    }

                    sql = " select P.PON_DISTANCIA_KM,P.PON_ID,PON_DESCRICAO,PON_TIPO,PON_LATITUDE,PON_LONGITUDE FROM T_PONTOS_MAPA  P (NOLOCK) " +
                    " INNER JOIN T_MAPA M (NOLOCK) ON M.PON_ID =  P.PON_ID  " +
                    " GROUP BY P.PON_DISTANCIA_KM,P.PON_ID,PON_DESCRICAO,PON_TIPO,PON_LATITUDE,PON_LONGITUDE  ORDER BY P.PON_ID ";

                    List<PontosMapa> PontosMapa = null;
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.PontosMapa == null || Parametro_sheOptQueueTransport.PontosMapa.Count() == 0)
                    {
                        if (PontosMapa == null)
                            PontosMapa = db.PontosMapa.FromSql(sql).ToList();
                    }

                    List<TemposLogisticos> TemposLogistica = null;
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.TemposLogistica == null || Parametro_sheOptQueueTransport.TemposLogistica.Count() == 0)
                    {
                        if (TemposLogistica == null)
                            TemposLogistica = db.TemposLogisticos.ToList();
                    }

                    DateTime dtCalDe = Ordens.Min(x => x.DataEntregaDe).AddDays(-50);
                    DateTime dtCalAte = Ordens.Max(x => x.DataEntregaDe).AddDays(365);

                    List<ItensCalendario> calend = null;
                    if (Parametro_sheOptQueueTransport == null || Parametro_sheOptQueueTransport.ItensCalendario == null || Parametro_sheOptQueueTransport.ItensCalendario.Count() == 0)
                    {
                        calend = db.ItensCalendario.Where(c => c.ICA_DATA_DE >= dtCalDe && c.ICA_DATA_ATE <= dtCalAte).OrderBy(c => c.ICA_DATA_DE).ToList();
                    }


                    //converções 
                    List<sheItensCalendario> sheCalend = Parametro_sheOptQueueTransport.ItensCalendario;
                    if (sheCalend == null || sheCalend.Count() == 0)
                    {
                        sheCalend = new List<sheItensCalendario>();
                        foreach (var de in calend)
                            sheCalend.Add((sheItensCalendario)UtilPlay.ConverterObjetos(de, new sheItensCalendario()));
                    }

                    List<sheTemposLogistica> sheTemposLogistica = Parametro_sheOptQueueTransport.TemposLogistica;
                    if (sheTemposLogistica == null || sheTemposLogistica.Count() == 0)
                    {
                        sheTemposLogistica = new List<sheTemposLogistica>();
                        foreach (var de in TemposLogistica)
                            sheTemposLogistica.Add((sheTemposLogistica)UtilPlay.ConverterObjetos(de, new sheTemposLogistica()));
                    }

                    List<shePontosMapa> shePontosMapa = Parametro_sheOptQueueTransport.PontosMapa;
                    if (shePontosMapa == null || shePontosMapa.Count() == 0)
                    {
                        shePontosMapa = new List<shePontosMapa>();
                        foreach (var de in PontosMapa)
                            shePontosMapa.Add((shePontosMapa)UtilPlay.ConverterObjetos(de, new shePontosMapa()));
                    }

                    List<sheMapa> sheMapas = Parametro_sheOptQueueTransport.Mapa;
                    if (sheMapas == null || sheMapas.Count() == 0)
                    {
                        sheMapas = new List<sheMapa>();
                        foreach (var de in Mapas)
                            sheMapas.Add((sheMapa)UtilPlay.ConverterObjetos(de, new sheMapa()));
                    }


                    List<sheOrderOpt> sheOrderOpt = null;//Parametro_sheOptQueueTransport.Ordens;
                    if (sheOrderOpt == null || sheOrderOpt.Count() == 0)
                    {
                        sheOrderOpt = new List<sheOrderOpt>();
                        foreach (var de in Ordens)
                            sheOrderOpt.Add((sheOrderOpt)UtilPlay.ConverterObjetos(de, new sheOrderOpt()));
                    }

                    List<sheParametros> sheParametros = Parametro_sheOptQueueTransport.Parametros;
                    if (sheParametros == null || sheParametros.Count() == 0)
                    {
                        sheParametros = new List<sheParametros>();
                        foreach (var de in Parametros)
                            sheParametros.Add((sheParametros)UtilPlay.ConverterObjetos(de, new sheParametros()));
                    }

                    List<sheTipoVeiculo> sheTipoVeiculo = Parametro_sheOptQueueTransport.VeiculosDisponiveis;
                    if (sheTipoVeiculo == null || sheTipoVeiculo.Count() == 0)
                    {
                        sheTipoVeiculo = new List<sheTipoVeiculo>();
                        foreach (var de in V_DISPONIBILIDADE_VEICULO)
                            sheTipoVeiculo.Add((sheTipoVeiculo)UtilPlay.ConverterObjetos(de, new sheTipoVeiculo()));
                    }
                    if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "5", RunType))
                        return;
                    sheOptQueueTransport sheOptQueueTransport = null;
                    OptQueueTransport OptQueueTransport = null;
                    try
                    {
                        if (Parametro_sheOptQueueTransport.MatrizMapa == null)
                        {
                            sheOptQueueTransport = new sheOptQueueTransport(Agora, sheCalend, sheTemposLogistica, shePontosMapa, sheMapas, sheOrderOpt, sheParametros, diasAfrente, IDsCalendarioExpedicao, sheTipoVeiculo, null, RunType);
                        }
                        else
                        {
                            if (RunType == 2)
                                Parametro_sheOptQueueTransport.Cargas = new List<sheCarga>();
                            Parametro_sheOptQueueTransport.Ordens = sheOrderOpt;
                            sheOptQueueTransport = Parametro_sheOptQueueTransport;
                        }
                        OPTParametrosSingleton.Instance.Parametro_sheOptQueueTransport = sheOptQueueTransport;
                        OptQueueTransport = new OptQueueTransport(sheOptQueueTransport);
                        foreach (var item in sheOptQueueTransport.Menssagens)
                        {
                            AddManssagem(item, 0);
                        }
                        Parametro_Ref_OptQueueTransport = OptQueueTransport;
                    }

                    catch (Exception e)
                    {
                        System.Console.WriteLine("#################################### ERRO ######################################");
                        System.Console.WriteLine("ERRO: " + UtilPlay.getErro(e));
                        System.Console.WriteLine("SQL: " + sqlLog);
                        System.Console.WriteLine("#################################### ERRO ######################################");

                        foreach (var item in sheOptQueueTransport.Menssagens.Where(x => x.MEN_TYPE == "SQL"))
                        {
                            System.Console.WriteLine("Menssagens: " + item.MEN_SEND);
                        }
                        foreach (var item in sheOptQueueTransport.Menssagens.Where(x => x.MEN_TYPE != "SQL"))
                        {
                            System.Console.WriteLine("Menssagens: " + item.MEN_SEND);
                            this.AddManssagem(item, 0);
                        }
                        //foreach (var item in sheOptQueueTransport.Menssagens)
                        //{
                        //    System.Console.WriteLine("Menssagens: " + item.MEN_SEND);
                        //}
                    }


                    // temporario atedevolver as variaveis corretas 
                    if (RunType == 2)
                    {
                        foreach (var c in Parametro_Ref_OptQueueTransport.sheOptQueueTransport.Cargas.Where(a => a.Opt == "").ToList())
                        {
                            if (c.CLP_Vencedor == null)
                            {
                                int IndexMelhorOcupacao = 0;
                                List<int> A = new List<int>();
                                A.Add(1);
                                var cl = Parametro_Ref_OptQueueTransport.GetContainerList(DateTime.Now, 1, 30, ref IndexMelhorOcupacao);
                                List<ContainerPackingResult> CLP = CromulentBisgetti.ContainerPacking.PackingService.Pack(cl, Parametro_Ref_OptQueueTransport.GetitemsToPack(c, null, null, null, null), A);
                                AlgorithmPackingResult vencedor = Parametro_Ref_OptQueueTransport.GetVencedor(CLP);
                                c.CLP_Vencedor = vencedor;
                            }
                        }
                    }

                    if (RunType < 2)
                    {
                        #region Persistencia de cargas 
                        // max ID carga 
                        var MaxIdCarga = db.Carga.Where(x => x.CAR_ID.Substring(0, 1) == "A").Max(m => m.CAR_ID);
                        int idcar = 0;
                        if (MaxIdCarga != null)
                        {
                            idcar = Convert.ToInt32(MaxIdCarga.Substring(1));
                        }
                        string sqlCarga = ""; string updCarga = ""; string updPedidos = ""; string sqlPedidos = ""; string sqlItens = ""; string sqlItensPacked = ""; List<string> AsqlCarga = new List<string>(); List<string> AUpdCarga = new List<string>(); List<string> AsqltPedidos = new List<string>(); List<string> AUpdPedidos = new List<string>(); List<string> AsqlItens = new List<string>(); List<string> AsqlItensPacked = new List<string>(); int contaC = 0; int contaPedidos = 0; int contaCUPD = 0; int contaI = 0; int contaIP = 0;
                        foreach (var carga in OptQueueTransport.sheOptQueueTransport.Cargas)
                        {
                            if (carga.Opt == "")
                            {
                                foreach (var pe in carga.PedidosParaExpedicao)
                                {
                                    foreach (var ord in pe.Ordens)
                                    {
                                        contaPedidos++;
                                        if (contaPedidos > 900)
                                        {
                                            AUpdPedidos.Add(updPedidos);
                                            updPedidos = "";
                                            contaPedidos = 0;
                                        }
                                        if (updPedidos != "")
                                        {
                                            updPedidos += ";";
                                        }
                                        updPedidos += " UPDATE T_ORDENS SET ORD_EMBARQUE_ALVO = '" + ord.InicioJanelaEmbarque.AddSeconds(((ord.FimJanelaEmbarque - ord.InicioJanelaEmbarque).TotalSeconds * (OptQueueTransport.sheOptQueueTransport.PercentualJanelaEmbarque / 100))).ToString("yyyyMMdd HH:mm:ss") + "' " +
                                        " ,ORD_DATA_ENTREGA_DE = dateadd(MINUTE, (" + ord.DataEntregaDe.ToString("HH") + " * 60.0) + ((" + ord.DataEntregaDe.ToString("mm") + " / 60.0) * 60), convert(datetime, convert(varchar(10), ORD_DATA_ENTREGA_DE, 112))) " +
                                        " ,ORD_DATA_ENTREGA_ATE = dateadd(MINUTE, (" + ord.DataEntregaAte.ToString("HH") + " * 60.0) + ((" + ord.DataEntregaAte.ToString("mm") + " / 60.0) * 60), convert(datetime, convert(varchar(10), ORD_DATA_ENTREGA_ATE, 112))) " +
                                        " ,ORD_INICIO_JANELA_EMBARQUE= '" + ord.InicioJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                        " ,ORD_FIM_JANELA_EMBARQUE= '" + ord.FimJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                        " WHERE ORD_ID = '" + ord.Id + "'";
                                    }
                                }
                                if (carga.CAR_STATUS <= 1)
                                {
                                    idcar++;
                                    carga.CAR_ID = "A" + idcar.ToString().PadLeft(5, '0');
                                    carga.CAR_STATUS = 1;
                                    if (sqlCarga != "")
                                    {
                                        sqlCarga += ",";
                                    }

                                    sqlCarga += "  ('" + carga.CAR_ID + "'" +
                                    ",'" + carga.EmbarqueAlvo.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",'" + carga.InicioJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",'" + carga.FimJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",'" + carga.CAR_DATA_INICIO_PREVISTO.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",0 " +
                                    ",'" + carga.CAR_DATA_FIM_PREVISTO.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",0 " +
                                    "," + carga.CAR_STATUS.ToString() +
                                    "," + carga.CAR_PESO_TEORICO.ToString().Replace(',', '.') +
                                    "," + carga.CAR_VOLUME_TEORICO.ToString().Replace(',', '.') +
                                    "," + carga.CAR_PESO_REAL.ToString().Replace(',', '.') +
                                    "," + carga.CAR_VOLUME_REAL.ToString().Replace(',', '.') +
                                    //"," + carga.CAR_PESO_EMBALAGEM.ToString().Replace(',', '.') +
                                    // temporario
                                    "," + carga.Rota.Distancia.ToString().Replace(',', '.') +
                                    "," + carga.CAR_PESO_ENTRADA.ToString().Replace(',', '.') +
                                    "," + carga.CAR_PESO_SAIDA.ToString().Replace(',', '.') +
                                    ",'" + carga.CAR_ID_DOCA.ToString() + "'" +
                                    //",'" + carga.VEI_PLACA + "'" +
                                    ",'" + carga.TIP_ID.ToString() + "'" +
                                    ",'" + carga.CAR_PREVISAO_MATERIA_PRIMA.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                    ",0" +
                                    ",0" +
                                    ",'')";
                                }
                                contaC++;
                                if (contaC > 900)
                                {
                                    AsqlCarga.Add(sqlCarga);
                                    AsqltPedidos.Add(sqlPedidos);
                                    sqlPedidos = "";
                                    sqlCarga = "";
                                    contaC = 0;
                                }
                                if (carga.CAR_STATUS <= 1)
                                {
                                    if (sqlPedidos != "")
                                    {
                                        sqlPedidos += ",";
                                    }
                                    sqlPedidos += "('" + carga.CAR_ID + "'" +
                                        ",''" +//ORD_ID_CONJUNTO
                                        ",'" + OptQueueTransport.sheOptQueueTransport.getTipoCarregamento(carga) + "'" +//PRO_ID
                                        ",''" +//PRO_ID_CONJUNTO
                                        ",'PLAYSIS'" +//CLI_ID
                                        ",0.0 " +//ORD_PRECO_UNITARIO
                                        "," + (int)(carga.CAR_DATA_FIM_PREVISTO - carga.CAR_DATA_INICIO_PREVISTO).TotalSeconds +//ORD_QUANTIDADE
                                        ",'" + carga.InicioJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +//ORD_DATA_ENTREGA_DE
                                        ",'" + carga.FimJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +//ORD_DATA_ENTREGA_ATE
                                        ",100 " +//ORD_TIPO
                                        ",0.0 " +//ORD_TOLERANCIA_MAIS
                                        ",0.0 " +//ORD_TOLERANCIA_MENOS
                                        ",''" +//HASH_KEY

                                        ",'" + carga.InicioJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +//ORD_INICIO_JANELA_EMBARQUE
                                        ",'" + carga.FimJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +//ORD_FIM_JANELA_EMBARQUE
                                        ",'" + carga.EmbarqueAlvo.ToString("yyyyMMdd HH:mm:ss") + "' " +//ORD_EMBARQUE_ALVO
                                                                                                        //",'" + carga.InicioGrupoProdutivo.ToString("yyyyMMdd HH:mm:ss") + "' " +//FPR_INICIO_GRUPO_PRODUTIVO
                                                                                                        //",'" + carga.FimGrupoProdutivo.ToString("yyyyMMdd HH:mm:ss") + "' " +//FPR_FIM_GRUPO_PRODUTIVO
                                        ",0.0 " +//ORD_PESO_UNITARIO
                                        ",0.0 " +//ORD_M2_UNITARIO
                                        ",''" +//ORD_MIT
                                        ",''" +//CAR_TIPO_CARREGAMENTO
                                        ",'" + carga.CAR_STATUS.ToString() + "'" +//ORD_STATUS
                                        ",'CIF'" +//ORD_TIPO_FRETE
                                        ",''" +//ORD_ENDERECO_ENTREGA
                                        ",''" +//ORD_BAIRRO_ENTREGA
                                        ",''" +//UF_ID_ENTREGA
                                        ",''" +//ORD_CEP_ENTREGA
                                               // ",''" +//MUN_ID_ENTREGA
                                               // ",''" +//ORD_REGIAO_ENTREGA
                                        ",0.0 " +//ORD_LARGURA
                                        ",0.0 " +//ORD_COMPRIMENTO
                                        ",0.0 " +//ORD_GRAMATURA
                                        ",''" +//GRP_ID
                                        ",'T_CARGA'" +//ORD_ID_INTEGRACAO
                                        ",''" +
                                        ",'" + MUN_ID_ENTREGA + "')";//ORD_OBSERVACAO_OTIMIZADOR

                                    foreach (var i in carga.ItensCarga)
                                    {
                                        contaI++;
                                        if (contaI > 900)
                                        {
                                            AsqlItens.Add(sqlItens);
                                            sqlItens = "";
                                            contaI = 0;
                                        }

                                        if (sqlItens != "")
                                        {
                                            sqlItens += ",";
                                        }
                                        sqlItens += "  ('" + carga.CAR_ID + "'," +
                                           "'" + i.ORD_ID + "'" +
                                           "," + i.ITC_ORDEM_ENTREGA.ToString() +
                                           "," + i.ITC_QTD_PLANEJADA.ToString() +
                                           "," + i.ITC_QTD_REALIZADA.ToString() +
                                           ",'" + i.ITC_ENTREGA_REALIZADA.ToString("yyyyMMdd HH:mm:ss") + "'" +
                                           ",'" + i.ITC_ENTREGA_PLANEJADA.ToString("yyyyMMdd HH:mm:ss") + "','')";
                                    }
                                }
                                int id = 1;
                                // pedencia para compilar 
                                if (carga.ItensPacked != null)
                                {
                                    foreach (var t in carga.ItensPacked)
                                    {
                                        contaIP++;
                                        if (contaIP > 900)
                                        {
                                            AsqlItensPacked.Add(sqlItensPacked);
                                            sqlItensPacked = "";
                                            contaIP = 0;
                                        }

                                        if (sqlItensPacked != "")
                                        {
                                            sqlItensPacked += ",";
                                        }
                                        sqlItensPacked += "(" + id + ",'" + carga.CAR_ID + "','" + t.PRO_ID + "','" + t.ORD_ID + "'," + t.CoordX.ToString().Replace(',', '.') + "," + t.CoordY.ToString().Replace(',', '.') + "," + t.CoordZ.ToString().Replace(',', '.') + "," + t.PackDimX.ToString().Replace(',', '.') + "," + t.PackDimY.ToString().Replace(',', '.') + "," + t.PackDimZ.ToString().Replace(',', '.') + "," + t.PecasPorPalete.ToString().Replace(',', '.') + ")";
                                        id++;
                                    }
                                }
                            }
                            contaCUPD = 0;
                            contaCUPD++;
                            if (contaCUPD > 900)
                            {
                                AUpdCarga.Add(updCarga);
                                updCarga = "";
                                contaCUPD = 0;
                            }
                            if (updCarga != "")
                            {
                                updCarga += ";";
                            }
                            if (carga.CAR_STATUS > 1)
                            {
                                updCarga += " UPDATE T_CARGA SET  " +
                                // " CAR_EMBARQUE_ALVO= '" + carga.EmbarqueAlvo.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                " CAR_INICIO_JANELA_EMBARQUE= '" + carga.InicioJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                " ,CAR_FIM_JANELA_EMBARQUE= '" + carga.FimJanelaEmbarque.ToString("yyyyMMdd HH:mm:ss") + "' " +
                                " ,CAR_VOLUME_TEORICO= " + carga.CAR_VOLUME_TEORICO.ToString().Replace(',', '.') +

                                " FROM T_CARGA" +
                                " WHERE CAR_ID = '" + carga.CAR_ID + "' AND CAR_STATUS < 5  ";
                            }
                        }

                        if (updCarga != "")
                        {
                            AUpdCarga.Add(updCarga);
                        }
                        if (sqlCarga != "")
                        {
                            AsqlCarga.Add(sqlCarga);
                        }
                        if (sqlPedidos != "")
                        {
                            AsqltPedidos.Add(sqlPedidos);
                        }
                        if (sqlItens != "")
                        {
                            AsqlItens.Add(sqlItens);
                        }
                        if (sqlItensPacked != "")
                        {
                            AsqlItensPacked.Add(sqlItensPacked);
                        }
                        if (updPedidos != "")
                        {
                            AUpdPedidos.Add(updPedidos);
                        }


                        sqlPedidos = " INSERT INTO T_ORDENS( " +
                        " ORD_ID, ORD_ID_CONJUNTO, PRO_ID, PRO_ID_CONJUNTO, CLI_ID, ORD_PRECO_UNITARIO, ORD_QUANTIDADE, ORD_DATA_ENTREGA_DE, ORD_DATA_ENTREGA_ATE, ORD_TIPO, ORD_TOLERANCIA_MAIS, ORD_TOLERANCIA_MENOS, HASH_KEY " +
                        " , ORD_INICIO_JANELA_EMBARQUE, ORD_FIM_JANELA_EMBARQUE, ORD_EMBARQUE_ALVO " +
                        " , ORD_PESO_UNITARIO, ORD_M2_UNITARIO, ORD_MIT, CAR_TIPO_CARREGAMENTO, ORD_STATUS, ORD_TIPO_FRETE, ORD_ENDERECO_ENTREGA, ORD_BAIRRO_ENTREGA, UF_ID_ENTREGA, ORD_CEP_ENTREGA " +
                        " ,  ORD_LARGURA, ORD_COMPRIMENTO, ORD_GRAMATURA, GRP_ID, ORD_ID_INTEGRACAO, ORD_OBSERVACAO_OTIMIZADOR,MUN_ID_ENTREGA) VALUES ";


                        sqlCarga = "INSERT INTO T_CARGA  (CAR_ID" +
                            ",CAR_EMBARQUE_ALVO" +
                            ",CAR_INICIO_JANELA_EMBARQUE" +
                            ",CAR_FIM_JANELA_EMBARQUE" +
                            ",CAR_DATA_INICIO_PREVISTO" +
                            ",CAR_DATA_INICIO_REALIZADO" +
                            ",CAR_DATA_FIM_PREVISTO" +
                            ",CAR_DATA_FIM_REALIZADO" +
                            ", CAR_STATUS" +
                            ", CAR_PESO_TEORICO" +
                            ", CAR_VOLUME_TEORICO" +
                            ", CAR_PESO_REAL" +
                            ", CAR_VOLUME_REAL" +
                            ", CAR_PESO_EMBALAGEM" +
                            ", CAR_PESO_ENTRADA" +
                            ", CAR_PESO_SAIDA" +
                            ", CAR_ID_DOCA" +
                            //", VEI_PLACA" +
                            ", TIP_ID" +
                            ",CAR_PREVISAO_MATERIA_PRIMA" +
                            ",CAR_DATA_ENTRADA_VEICULO" +
                            ",CAR_DATA_SAIDA_VEICULO  " +
                            ",CAR_ID_JUNTADA) VALUES ";

                        sqlItens = " INSERT INTO dbo.T_ITENS_CARGA " +
                            " (CAR_ID, ORD_ID, ITC_ORDEM_ENTREGA, ITC_QTD_PLANEJADA, ITC_QTD_REALIZADA, ITC_ENTREGA_REALIZADA, ITC_ENTREGA_PLANEJADA,ORD_HASH_KEY)VALUES ";
                        sqlItensPacked = " INSERT INTO T_ITENS_PACKED (IPA_ID,CAR_ID,PRO_ID,ORD_ID,IPA_COORDC,IPA_COORDL,IPA_COORDA,IPA_DIMC,IPA_DIML,IPA_DIMA,IPA_QTD_POR_PALETE) VALUES";
                        //using (DbContextTransaction tran = db.Database.BeginTransaction())
                        //{
                        if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "75", RunType))
                            return;

                        foreach (var s in AsqlCarga.Where(x => x != ""))
                        {
                            db.Database.ExecuteSqlCommand(sqlCarga + s);
                        }
                        if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "80", RunType))
                            return;

                        foreach (var s in AsqltPedidos.Where(x => x != ""))
                        {
                            db.Database.ExecuteSqlCommand(sqlPedidos + s);
                        }
                        if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "85", RunType))
                            return;

                        foreach (var s in AUpdPedidos.Where(x => x != ""))
                        {
                            db.Database.ExecuteSqlCommand(s);
                        }
                        if (OPTParametrosSingleton.MsgSingleton("%OPTransporte", "90", RunType))
                            return;


                        foreach (var s in AsqlItens.Where(x => x != ""))
                        {
                            db.Database.ExecuteSqlCommand(sqlItens + s);
                        }
                        //foreach (var s in AsqlItensPacked)
                        //{
                        //    db.Database.ExecuteSqlCommand(sqlItensPacked + s);
                        //}
                        db.Database.ExecuteSqlCommand(sql);
                        //tran.Commit();
                        //}


                        foreach (var u in AUpdCarga.Where(x => x != ""))
                        {
                            db.Database.ExecuteSqlCommand(u);
                        }

                        //// atualiza embarque pedido com base na carga 
                        //updCarga = " UPDATE T_ORDENS SET ORD_EMBARQUE_ALVO = CAR_EMBARQUE_ALVO, ORD_INICIO_JANELA_EMBARQUE = CAR_INICIO_JANELA_EMBARQUE, ORD_FIM_JANELA_EMBARQUE = CAR_FIM_JANELA_EMBARQUE " +
                        //" FROM T_ORDENS O INNER JOIN T_ITENS_CARGA(NOLOCK) I ON I.ORD_ID = O.ORD_ID  " +
                        //" INNER JOIN T_CARGA(NOLOCK) C ON I.CAR_ID = C.CAR_ID " +
                        //" WHERE O.ORD_ID IN( SELECT OrderId FROM V_OPS_A_PLANEJAR UNION ALL SELECT Id FROM V_PEDIDOS_A_PLANEJAR_EXPEDICAO) ";
                        //db.Database.ExecuteSqlCommand(updCarga);


                        ////// data hora entrega cliente atualmente buscando a data inicio
                        ////TimeSpan horaIni = new TimeSpan(8, 0, 0);
                        ////TimeSpan horaFim = new TimeSpan(17, 0, 0);
                        ////f.InicioJanelaEmbarque =
                        ////    f.ORD_DATA_ENTREGA_ATE.Date.AddHours(
                        ////    (Convert.ToDouble(f.HRE_HORA_INICIAL.Substring(0, 2)) +
                        ////        (Convert.ToDouble(f.HRE_HORA_INICIAL.Substring(3, 2)) / 60)
                        ////    ) * -1);
                        ////// adiciona translado
                        ////var Translado = (f.CLI_TRANSLADO * -1);
                        ////f.InicioJanelaEmbarque = f.InicioJanelaEmbarque.AddHours(Translado);
                        ////// adiciona tempo de expedição carregamento mais separação // considerar quando o carregamento for direto da produção para caminhao sem passar por estoque.
                        ////f.InicioJanelaEmbarque = f.InicioJanelaEmbarque.AddSeconds(tempoExpedicao * -1);
                        // APOS TER CRIADO TODAS AS CARGAS CHAMA PROCEDURE SP_OPT_ESTUDO_JANELA_EMBARQUE QUE AJUSTAS AS DATAS DE INICIO DE EMBARQUE 
                        // PARA QUE O PROCESSO DE ORDENAÇÃO DE FILAS DE PRODUÇÃO PASSE A OCORRER 

                        // PASSO 0 DEFINE JANEA DE EMBARQUE 
                        //////* calcula datas ORD_INICIO_JANELA_EMBARQUE e ORD_FIM_JANELA_EMBARQUE
                        ////// QUANDO PRIORIZA DATA DE ENTREGA OS PEDIDDOS SERÃO AGRUPADOS POR MESMA DATA DE EMBARQUE
                        ////// QUANDO PRIORIZA OCUPAÇÃO EXISTE UM PARAMETRO QUE DEFINE A QUANTIDADE DE DIAS MAXIMA DE ATRAZO OU ANTECIPAÇÃO ASSIM O SISTEMA AGRUPARA OS PEDIDOS RESPEITANDO ESTE LIMITES PARA POR EXEMPLO NÃO ANTECIPAR ENTREGAS MAIORES QUE 60 DIAS AO ALGO ASSIM.

                        // PASSO 1 PLANEJAR EXPEDIÇÃO 
                        ////// Calculo de rotas e cu_bagem 


                        // PASSO 2 PLANEJAR PRODUÇÃO DE MODO QUE O PLANEJAMENTO SEJA FAVORAVEL A EXPEDIÇÃO (PRODUZIR NA MESMA SEQUENCIA QUE IRA TRANSPORTAR GERANDO UMA INFINIDADE DE VANTAGENS COMO: REDUÇÃO DA MOVIMENTAÇÃO INTERNA, CONGESTIONAMENTOS, DISPERDICIO DE TEMPO, ETC...
                        // TRATAR COLENDARIO SEM EXPEDIENTE E PARADAS DE MANUTENÇÃO PARA A FILA DE PRODUÇÃO  


                        // ATUALIZA PARAMETROS DE INICIO DO PROCESSO DE PRODUCAO 
                        sql = " UPDATE T_PARAMETROS SET PAR_VALOR_S = 'RUN - PRODUCAO - '+CONVERT(varchar, getdate(), 100) WHERE PAR_ID = 'SCHEDULE_STATUS'; ";
                        db.Database.ExecuteSqlCommand(sql);


                        int cont = 1;
                        foreach (var item in sheOptQueueTransport.Menssagens.Where(x => x.MEN_TYPE == "SQL").ToList())
                        {
                            try
                            {
                                db.Database.ExecuteSqlCommand(item.MEN_SEND);
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine("#################################### ERRO ######################################");
                                System.Console.WriteLine("ERRO: " + UtilPlay.getErro(e));
                                System.Console.WriteLine("SQL: " + item.MEN_SEND);
                                System.Console.WriteLine("#################################### ERRO ######################################");
                                break;
                            }
                            cont++;
                        }
                        OPTParametrosSingleton.MsgSingleton("%OPTransporte", "100", RunType);

                        System.Console.WriteLine((DateTime.Now - tempo).TotalSeconds + "  Transporte UPDATES");
                        System.Console.WriteLine((DateTime.Now - SubtotalTotal).TotalSeconds + "  Transporte TOTAL");

                        tempo = DateTime.Now;
                        //Exception eFila = FilaProducao(db, false, diasAfrente, cmdDebug);

                        System.Console.WriteLine((DateTime.Now - tempo).TotalSeconds + "  TOTAL TEMPO FILA ");
                        System.Console.WriteLine((DateTime.Now - Total).TotalSeconds + "  TOTAL TOTAL ");
                        #endregion  fim persistencia transporte 
                    }
                    #endregion

                    if (RunType < 2)
                    {// se for completo 0    ou for 1 apenas produção   2 é expedição customizada
                        #region Fila de Produção 
                        sheOptQueueBox sheOptQueueBox = null;
                        OptQueueBox OptQueueBox = null;

                        try
                        {
                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "0", RunType))
                                return;

                            #region INICIO
                            //Reservas 
                            // 1 - encerra reservas que viraram pedidos 
                            string reservas = " UPDATE T_ORDENS SET ORD_STATUS = 'EV' FROM T_ORDENS" +
                                " INNER JOIN T_ORDENS O ON O.ORD_ID_RESERVA = T_ORDENS.ORD_ID  AND T_ORDENS.PRO_ID = O.PRO_ID AND O.ORD_ID_RESERVA<> O.ORD_ID " +
                                " WHERE T_ORDENS.ORD_ID_RESERVA = T_ORDENS.ORD_ID AND left(T_ORDENS.ORD_STATUS,1) <> 'E'";
                            db.Database.ExecuteSqlCommand(reservas);

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "5", RunType))
                                return;
                            // 2 - Encerar reservas R1 que estão vencidas 
                            reservas = " UPDATE T_ORDENS SET ORD_STATUS = 'E1' FROM T_ORDENS  WHERE ORD_ID_RESERVA = ORD_ID AND ORD_STATUS = 'R1' " +
                            " AND DATEADD(SECOND, (SELECT PAR_VALOR_N FROM T_PARAMETROS (NOLOCK) WHERE PAR_ID = 'RESERVA_TEMPORESERVA_R1'),ORD_EMISSAO) < GETDATE()";
                            db.Database.ExecuteSqlCommand(reservas);

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "10", RunType))
                                return;


                            // 3 - Encerar reservas R2 que estão vencidas 
                            reservas = " UPDATE T_ORDENS SET ORD_STATUS = 'E2' FROM T_ORDENS  WHERE ORD_ID_RESERVA = ORD_ID AND ORD_STATUS = 'R2' " +
                            " AND DATEADD(SECOND, (SELECT PAR_VALOR_N FROM T_PARAMETROS (NOLOCK) WHERE PAR_ID = 'RESERVA_TEMPORESERVA_R2'),ORD_EMISSAO) < GETDATE() ";
                            db.Database.ExecuteSqlCommand(reservas);

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "15", RunType))
                                return;


                            int SeqInclusaoFila = 0;
                            string insertFillaWMS = " INSERT INTO T_FILA_PRODUCAO(FPR_DATA_NECESSIDADE_INICIO_PRODUCAO,FPR_DATA_NECESSIDADE_FIM_PRODUCAO,FPR_PRIORIDADE,ROT_MAQ_ID, ORD_ID, ROT_PRO_ID, ROT_SEQ_TRANFORMACAO, FPR_SEQ_REPETICAO," +
                            " FPR_DATA_INICIO_PREVISTA, FPR_DATA_FIM_PREVISTA, FPR_DATA_FIM_MAXIMA, FPR_QUANTIDADE_PREVISTA, FPR_OBS_PRODUCAO, FPR_STATUS) " +
                            " select  ORD_EMBARQUE_ALVO,ORD_EMBARQUE_ALVO, 0,'PLAYSIS',O.ORD_ID, O.PRO_ID, SEQ_TRANFORMACAO,1 FPR_SEQ_REPETICAO, " +
                            " GETDATE(), GETDATE(), GETDATE(),O.ORD_QUANTIDADE, ''FPR_OBS_PRODUCAO,''FPR_STATUS from T_ORDENS O " +
                            " INNER JOIN T_PRODUTOS P ON P.PRO_ID = O.PRO_ID " +
                            " INNER JOIN T_GRUPO_PRODUTO G ON G.GRP_ID = P.GRP_ID AND GRP_TIPO IN(999,9) " + // pendencia mudar cadastro da cartron alterarndo de 999 para 9 e tirar o 999 do in
                            " INNER JOIN(SELECT PRO_ID, ROT_SEQ_TRANFORMACAO SEQ_TRANFORMACAO FROM T_ROTEIROS " +
                            " GROUP BY PRO_ID, ROT_SEQ_TRANFORMACAO) R ON O.PRO_ID = R.PRO_ID " +
                            " LEFT JOIN T_FILA_PRODUCAO F ON O.ORD_ID = F.ORD_ID AND R.PRO_ID = F.ROT_PRO_ID AND F.ROT_SEQ_TRANFORMACAO = SEQ_TRANFORMACAO " +
                            " WHERE F.ORD_ID IS NULL AND O.CLI_ID IN(SELECT CLI_ID FROM T_HORARIO_RECEBIMENTO) " +
                            " AND ORD_STATUS<> 'E1' AND ORD_TIPO = 100 ";
                            db.Database.ExecuteSqlCommand(insertFillaWMS);


                            // marca OPs como firmes caso tenham sido integralmente produzidas 
                            db.Database.ExecuteSqlCommand(" EXEC SP_PLUG_INTERFACE_BEFORE_OPT ");
                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "20", RunType))
                                return;


                            List<sheOcupanteEspacoEsquerda> listasheOcupanteEspacoEsquerda = new List<sheOcupanteEspacoEsquerda>();
                            // CustoEntreOps
                            sql = " select CUS_OPERACOES,CUS_UNIC_ID,CUS_ID,isnull(PRO_ID,'')PRO_ID,isnull(GRP_ID,'')GRP_ID,isnull(MAQ_ID,'')MAQ_ID,isnull(CUS_DESCRICAO,'')CUS_DESCRICAO,CUS_GRUPO_TEMPO_SETUP,CUS_PESO_CASO_VERDADEIRO,CUS_PESO_CASO_FALSO,isnull(CUS_TIPO_AVALIACAO,'')CUS_TIPO_AVALIACAO,CUS_TEMPO_CASO_VERDADEIRO,CUS_TEMPO_CASO_FALSO FROM T_CUSTO_ENTRE_OPS ";
                            List<CustoEntreOps> CustoEntreOps = null;
                            CustoEntreOps = db.CustoEntreOps.FromSql(sql).ToList();
                            List<sheCustoEntreOps> sheCustoEntreOps = new List<sheCustoEntreOps>();
                            foreach (var de in CustoEntreOps)
                                sheCustoEntreOps.Add((sheCustoEntreOps)UtilPlay.ConverterObjetos(de, new sheCustoEntreOps()));
                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "25", RunType))
                                return;


                            // Estrutura
                            sql = " SELECT * FROM V_ESTRUTURA_DO_SCHEDULE ";
                            List<EstruturaDoSchedule> EstruturaDoSchedule = null;
                            EstruturaDoSchedule = db.EstruturaDoSchedule.FromSql(sql).ToList();
                            List<sheEstruturaDoSchedule> sheEstruturaDoSchedule = new List<sheEstruturaDoSchedule>();
                            foreach (var de in EstruturaDoSchedule)
                                sheEstruturaDoSchedule.Add((sheEstruturaDoSchedule)UtilPlay.ConverterObjetos(de, new sheEstruturaDoSchedule()));
                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "30", RunType))
                                return;

                            List<Maquina> Maquina = db.Maquina.ToList();
                            List<sheMaquina> sheMaquina = new List<sheMaquina>();
                            foreach (var de in Maquina)
                                sheMaquina.Add((sheMaquina)UtilPlay.ConverterObjetos(de, new sheMaquina()));

                            List<T_MAQUINAS_EQUIPES> MaquinaEquipe = db.T_MAQUINAS_EQUIPES.ToList();
                            List<sheT_MAQUINAS_EQUIPES> sheMaquinaEquipe = new List<sheT_MAQUINAS_EQUIPES>();
                            foreach (var de in MaquinaEquipe)
                                sheMaquinaEquipe.Add((sheT_MAQUINAS_EQUIPES)UtilPlay.ConverterObjetos(de, new sheT_MAQUINAS_EQUIPES()));

                            sql = " select * FROM V_OPS_A_PLANEJAR WHERE (LEFT(FPR_STATUS,1) <>'E' or FPR_STATUS is null) AND LEFT(ORD_STATUS, 1) <> 'E' AND ORD_STATUS <> 'SS' ";
                            if (diasAfrente > 0)
                                sql += " AND convert(varchar, EMBARQUE_ALVO  ,112)  <= '" + FiltroOtimizador(diasAfrente).ToString("yyyyMMdd") + "'";
                            sql += " ORDER by OrderId, GRP_TIPO DESC ,ProdutoId DESC,SequenciaTransformacao DESC, SequenciaRepeticao DESC  ";
                            List<FilaDoSchedule> FilaDoSchedule = db.FilaDoSchedule.FromSql(sql).ToList();
                            List<sheFilaDoSchedule> sheFilaDoSchedule = new List<sheFilaDoSchedule>();
                            foreach (var de in FilaDoSchedule)
                                sheFilaDoSchedule.Add((sheFilaDoSchedule)UtilPlay.ConverterObjetos(de, new sheFilaDoSchedule()));
                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "50", RunType))
                                return;

                            sql = " select  *  FROM V_OPS_A_PLANEJAR WHERE LEFT(FPR_STATUS,1) ='E'  AND DataFimPrevista > '" + Agora.AddDays(-7).ToString("yyyyMMdd 00:00:00") + "'";
                            sql += " ORDER BY MaquinaId,DataFimPrevista ";
                            List<FilaDoSchedule> UltimaOPsEncerradas = db.FilaDoSchedule.FromSql(sql).ToList();
                            List<sheFilaDoSchedule> sheUltimasOPsEncerradas = new List<sheFilaDoSchedule>();
                            foreach (var de in UltimaOPsEncerradas)
                                sheUltimasOPsEncerradas.Add((sheFilaDoSchedule)UtilPlay.ConverterObjetos(de, new sheFilaDoSchedule()));

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "55", RunType))
                                return;

                            sql = " SELECT * FROM V_ROTEIROS_POSSIVEIS_DO_PRODUTO ";
                            List<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> roteiros = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.FromSql(sql).ToList();
                            List<sheV_ROTEIROS_POSSIVEIS_DO_PRODUTO> sheRoteiros = new List<sheV_ROTEIROS_POSSIVEIS_DO_PRODUTO>();
                            foreach (var de in roteiros)
                                sheRoteiros.Add((sheV_ROTEIROS_POSSIVEIS_DO_PRODUTO)ConverterObjetos(de, new sheV_ROTEIROS_POSSIVEIS_DO_PRODUTO()));

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "60", RunType))
                                return;

                            sql = " SELECT * FROM T_OPERACOES ";
                            List<Operacoes> Operacoes = Operacoes = db.Operacoes.FromSql(sql).ToList();
                            List<sheOperacoes> sheOperacoes = new List<sheOperacoes>();
                            foreach (var de in roteiros)
                                sheOperacoes.Add((sheOperacoes)UtilPlay.ConverterObjetos(de, new sheOperacoes()));

                            if (OPTParametrosSingleton.MsgSingleton("%Preparando Filas", "65", RunType))
                                return;

                            sheOptQueueBox = new sheOptQueueBox(sheTemposLogistica, sheOrderOpt,
                                IDsCalendarioExpedicao, OptQueueTransport.sheOptQueueTransport.Cargas, sheOperacoes,
                                sheUltimasOPsEncerradas, OptQueueTransport.sheOptQueueTransport.GruposProdutivosExpedicao,
                                sheRoteiros, sheFilaDoSchedule, sheMaquina, sheMaquinaEquipe,
                                sheCalend, sheParametros, sheCustoEntreOps,
                                sheEstruturaDoSchedule, Agora, false, DebugLog);
                            OptQueueBox = new OptQueueBox(sheOptQueueBox);

                            if (OPTParametrosSingleton.Instance.Cancelar)
                            {
                                return;
                            }


                            // implementar checksun
                            CheckSun(sheOptQueueBox);
                            // implementar persistencia

                            int conta = 1;
                            db.Database.SetCommandTimeout(1000000);

                            Console.WriteLine("Gravando dados....... " + conta + "/" + sheOptQueueBox.Menssagens.Where(x => x.MEN_TYPE == "SQL").Count());
                            double TotalAjustado = sheOptQueueBox.Menssagens.Count() * 1.1;
                            foreach (var item in sheOptQueueBox.Menssagens)
                            {
                                string Percentual = ((conta / TotalAjustado * 100)).ToString();
                                if (OPTParametrosSingleton.MsgSingleton("%Gravando Dados", Percentual, RunType))
                                    return;

                                if (item.MEN_TYPE == "SQL")
                                {
                                    if (item.MEN_ID == "CARGA_MAQUINA")
                                        OPTParametrosSingleton.Instance.semaforoCargaMaquina = "USING";
                                    else
                                        OPTParametrosSingleton.Instance.semaforoCargaMaquina = "FREE";
                                    sqlLog = "";
                                    int contaTran = 1;
                                    while (contaTran < 10)
                                    {

                                        using (var tran = db.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                Console.WriteLine("Gravando dados try(" + contaTran + ") ....... " + conta + "/" + sheOptQueueBox.Menssagens.Where(x => x.MEN_TYPE == "SQL").Count());
                                                db.Database.ExecuteSqlCommand(item.MEN_SEND);
                                                tran.Commit();
                                                sqlLog = "";
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                sqlLog = item.MEN_SEND;
                                                Console.WriteLine("Falha ao gravar try(" + contaTran + ") ....... " + conta + "/" + sheOptQueueBox.Menssagens.Where(x => x.MEN_TYPE == "SQL").Count());
                                                tran.Rollback();
                                            }
                                            contaTran++;
                                        }
                                    }
                                    if (sqlLog != "")
                                    {
                                        Console.WriteLine("");
                                        Console.WriteLine("");
                                        Console.WriteLine("ERRO SQL: " + sqlLog);
                                        Console.WriteLine("");
                                        Console.WriteLine("");
                                    }

                                    conta++;
                                }
                                else
                                {
                                    this.AddManssagem(item, 0);
                                }
                            }
                            OPTParametrosSingleton.Instance.semaforoCargaMaquina = "FREE";
                            #endregion
                        }
                        catch (Exception e)
                        {
                            if (sheOptQueueBox != null && sheOptQueueBox.Menssagens != null)
                            {
                                foreach (var item in sheOptQueueBox.Menssagens.Where(x => x.MEN_TYPE != "SQL"))
                                {
                                    System.Console.WriteLine("Menssagens: " + item.MEN_SEND);
                                }
                            }
                            foreach (var item in sheOptQueueTransport.Menssagens.Where(x => x.MEN_TYPE != "SQL"))
                            {
                                System.Console.WriteLine("Menssagens: " + item.MEN_SEND);
                                this.AddManssagem(item, 0);
                            }
                            System.Console.WriteLine("SQL: " + sqlLog);
                            System.Console.WriteLine("#################################### ERRO ######################################");
                            System.Console.WriteLine("");
                            System.Console.WriteLine("");
                            System.Console.WriteLine("");
                            System.Console.WriteLine("ERRO: " + UtilPlay.getErro(e));
                            System.Console.WriteLine("#################################### ERRO ######################################");
                        }

                        #endregion
                        sql = " UPDATE T_CARGA SET CAR_PREVISAO_MATERIA_PRIMA = FPR_PREVISAO_MATERIA_PRIMA, CAR_DATA_INICIO_PREVISTO = FPR_DATA_INICIO_PREVISTA, CAR_DATA_FIM_PREVISTO = FPR_DATA_FIM_PREVISTA " +
                              " FROM T_CARGA " +
                              " INNER JOIN T_FILA_PRODUCAO F ON F.ORD_ID = T_CARGA.CAR_ID ";
                        db.Database.ExecuteSqlCommand(sql);
                        if (OPTParametrosSingleton.MsgSingleton("%Gravando Dados", "93", RunType))
                            return;
                        // ATUALIZA PARAMETROS DE INICIO DO PROCESSO 
                        sql = " UPDATE T_PARAMETROS SET PAR_VALOR_D = GETDATE() WHERE PAR_ID = 'SCHEDULE_DATA_FIM';UPDATE T_PARAMETROS SET PAR_VALOR_S = 'STOP - '+CONVERT(varchar, getdate(), 100) WHERE PAR_ID = 'SCHEDULE_STATUS'; ";
                        db.Database.ExecuteSqlCommand(sql);

                        if (OPTParametrosSingleton.MsgSingleton("%Gravando Dados", "95", RunType))
                            return;
                        //SP_PLUG_INTERFACE_AFTER_OPT
                        db.Database.ExecuteSqlCommand(" EXEC SP_PLUG_PREVISTO_REALIZADO '" + Agora.ToString("yyyyMMdd") + "'  ");
                        if (OPTParametrosSingleton.MsgSingleton("%Gravando Dados", "97", RunType))
                            return;
                        db.Database.ExecuteSqlCommand(" EXEC SP_PLUG_INTERFACE_AFTER_OPT ");
                        if (OPTParametrosSingleton.MsgSingleton("%Gravando Dados", "100", RunType))
                            return;
                    }
                    System.Console.WriteLine((DateTime.Now - tempo).TotalSeconds + "  TOTAL TEMPO FILA ");
                    System.Console.WriteLine((DateTime.Now - Total).TotalSeconds + "  TOTAL TOTAL ");
                }
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                if (RunType < 2)
                {
                    OPTParametrosSingleton.Instance.semaforoOtimizador = "FIM";
                }
            }
        }
    }
}