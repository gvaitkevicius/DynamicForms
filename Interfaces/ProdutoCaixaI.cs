using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynamicForms.Interfaces
{
    public class ProdutoCaixaI
    {
        public void ImportarProdutoCaixas(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _produtoImportados = new List<object>();

            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            LogPlay logComplemento = new LogPlay();
            MasterController mc = new MasterController();
            string _erros = "";
            bool flag = true;
            int cont = 0;
            V_INPUT_T_PRODUTO_CAIXAS itAux = new V_INPUT_T_PRODUTO_CAIXAS();
            try
            {
                //Importando lista da Interface de Pedidos
                List<V_INPUT_T_PRODUTO_CAIXAS> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando caixas...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_CAIXAS");
                    stopwatch.Start();
                    _listaInterface = db.GetCaixasInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_CAIXAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_CAIXAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_CAIXAS", UtilPlay.getErro(ex)));
                    _listaInterface = new List<V_INPUT_T_PRODUTO_CAIXAS>();
                    flag = false;
                    _erros = "GRUPO_PRODUTO_CAIXA; PRODUTO_TINTAS; GRUPO_CHAPA; GRUPO_CONJUNTO";
                }

                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    if (flag == true)
                    {
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());//Verificando mensagens de erro da view de interface
                    }
                    if (flag)//se não há erros
                    {
                        var msvet = itAux.Action.Split('|');
                        itAux.Action = msvet.ElementAt(0);

                        _produtoImportados.Add(itAux.ToProduto());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToProduto(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itAux.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itAux.ToProduto(), "ERRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo   
                        foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                        {
                            if (!String.IsNullOrEmpty(it.Trim()) && !erros.Contains(it))
                            {
                                erros.Add(it);
                                _erros += " " + it;
                            }
                        }
                    }
                    cont++;
                }
                if (!flag)
                {
                    LogLocal.Clear(); // COnfirmar se log deve ser limpo apos fase de debug do projeto

                    // PENDENCIA TRATAR ERRO NO CADASTRO DO CONJUNTO 
                    //"PRODUTO_CONJUNTO;"

                    //Verificando lista de depenências não atendidadas
                    if (_erros.Contains("GRUPO_PRODUTO_CAIXA"))
                    {
                        _erros = _erros.Replace("GRUPO_PRODUTO_CAIXA", "");
                        GrupoFerramentalI grupoFerramental = new GrupoFerramentalI();
                        GrupoProdutoPaleteI grupoProdutoPalete = new GrupoProdutoPaleteI();

                        GrupoProdutoCaixaI grupoProdutoCaixa = new GrupoProdutoCaixaI();
                        //--importando cliches,paletes e facas
                        grupoFerramental.ImportarGrupoFerramental(ref log, forceInsert, db);
                        grupoProdutoPalete.ImportarGrupoProdutoPalete(ref log, forceInsert, db);

                        //--
                        grupoProdutoCaixa.ImportarGrupoProdutoCaixa(ref log, forceInsert, db);
                    }
                    if (_erros.Contains("PRODUTO_TINTAS"))
                    {
                        _erros = _erros.Replace("PRODUTO_TINTAS", "");
                        TintasI ti = new TintasI();
                        ti.ImportarTintas(ref log, forceInsert, db);
                    }
                    // CHAPA SERA CADASTRADO JUNTO COM A PROPRIA CAIXA ATRAVÉS DOS CAMPOS  GRUPO   LARGURA E ALTURA DA CHAPA A ESTRUTURA TAMBEM SERA CADASTRADA POIS TEMOS QTD CAIXAS POR CHAPA
                    // POR ESTE MOTIVO CHAMAREMOS O GRUPO DE CHAPAS AQUI DENTRO DO CADASTRO DE CAIXAS
                    if (_erros.Contains("GRUPO_CHAPA"))
                    { // PENDENCIA LA NO CADASTRO DO GRUPO CHAPA TESTAR GRUPO PAPEIS HOJE ESTA NO PROPRIO CADASTRO DE CHAPAS   
                        GrupoProdutoChapaI gchi = new GrupoProdutoChapaI();
                        gchi.ImportarGrupoChapa(ref log, forceInsert, db);
                    }

                    LogLocal.Clear();
                    if (_erros.Contains("GRUPO_CONJUNTO"))
                    {
                        GrupoConjuntoI grupoConjuntoI = new GrupoConjuntoI();
                        grupoConjuntoI.ImportarGrupoConjunto(ref log, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    LogLocal.Clear();
                    _produtoImportados.Clear();
                    Console.WriteLine("Importando caixas apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_CAIXAS");
                    stopwatch.Start();
                    _listaInterface = db.GetCaixasInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_CAIXAS: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        if (flag)
                        {
                            var msvet = itAux.Action.Split('|');
                            itAux.Action = msvet.ElementAt(0);
                            _produtoImportados.Add(itAux.ToProduto());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToProduto(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itAux.ToProduto(), "ERRO_CAIXA", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }

                if (_produtoImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando caixas na base dadados...");
                    stopwatch.Start();
                    List<List<object>> ll = new List<List<object>>();
                    List<object> subLista = new List<object>();
                    List<LogPlay> logsUpdateData = new List<LogPlay>();
                    foreach (var caixa in _produtoImportados)
                    {
                        subLista.Add(caixa);
                        ll.Add(subLista);
                        logsUpdateData.AddRange(mc.UpdateData(ll, forceInsert, true, db));
                        subLista.Clear();
                        ll.Clear();
                    }
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos caixas: {stopwatch.Elapsed}");

                    //#region ReportLog
                    //logsUpdateData.ForEach(x => x.Properties = null);
                    //UtilPlay.GravarArquivoJson(logsUpdateData.Where(x => x.Status != "OK" && 
                    //    !x.MsgErro.Contains("Este objeto sofreu rollback")).ToList(), "ImportarProdutoCaixas.json");
                    //#endregion ReportLog

                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, logsUpdateData);
                }

                //PALETES
                try
                {
                    ProdutoPaleteI produtoPalete = new ProdutoPaleteI();
                    produtoPalete.ImportarProdutoPalete(ref log, forceInsert, db);
                }
                catch (Exception ex)
                {
                    LogLocal.Add(new LogPlay(new Produto(), "ERRO_PALETE", UtilPlay.getErro(ex)));
                }

                //"ESTRUTURA_PRODUTO"
                try
                {
                    EstruturaProdutoI epi = new EstruturaProdutoI();
                    epi.ImportarEstruturaProdutos(ref log, forceInsert, db);
                }
                catch (Exception ex)
                {
                    LogLocal.Add(new LogPlay(new Produto(), "ERRO_ESTRUTURA", UtilPlay.getErro(ex)));
                }

                // "ROTEIRO"
                try
                {
                    RoteirosI roti = new RoteirosI();
                    roti.ImportarRoteiros(ref log, forceInsert, db);
                }
                catch (Exception ex)
                {
                    log.Add(new LogPlay(new Produto(), "ERRO_ROTEIRO", UtilPlay.getErro(ex)));
                }

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de caixas: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_CAIXA", UtilPlay.getErro(ex)));
            }
        }
        public class V_INPUT_T_PRODUTO_CAIXAS
        {

            public V_INPUT_T_PRODUTO_CAIXAS()
            { }

            public string PRO_ID { get; set; }
            public string ABN_ID { get; set; }
            public string PRO_DESCRICAO { get; set; }
            public double PRO_ESTOQUE_ATUAL { get; set; }
            public string UNI_ID { get; set; }
            //public double PRO_PECAS_POR_CONJUNTO { get; set; }
            public double PRO_FARDOS_POR_CAMADA { get; set; }
            public double PRO_CAMADAS_POR_PALETE { get; set; }
            public int PRO_TIPO_IDENTIFICACAO { get; set; }
            public string PRO_GRUPO_PALETIZACAO { get; set; }
            public double PRO_PECAS_POR_FARDO { get; set; }
            public string PRO_ID_INTEGRACAO { get; set; }
            public string PRO_ID_INTEGRACAO_ERP { get; set; }
            public string GRP_ID { get; set; }
            public int? TEM_ID { get; set; }
            public double PRO_LARGURA_PECA { get; set; }
            public double PRO_COMPRIMENTO_PECA { get; set; }
            public double PRO_ALTURA_PECA { get; set; }
            public double PRO_LARGURA_INTERNA { get; set; }
            public double PRO_COMPRIMENTO_INTERNA { get; set; }
            public double PRO_ALTURA_INTERNA { get; set; }
            public double PRO_AREA_LIQUIDA { get; set; }
            public double PRO_PESO { get; set; }
            public double PRO_LARGURA_EMBALADA { get; set; }
            public double PRO_COMPRIMENTO_EMBALADA { get; set; }
            public double? PRO_ALTURA_EMBALADA { get; set; }
            public double PRO_QTD_CANTONEIRAS { get; set; }
            public double PRO_QTD_FORROS { get; set; }
            public double PRO_QTD_CHAPEL { get; set; }
            public string PRO_VINCOS_COMPRIMENTO { get; set; }
            public string PRO_VINCOS_LARGURA { get; set; }
            public string PRO_FRENTE { get; set; }
            public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
            public string PRO_ROTACIONA_LARGURA { get; set; }
            public string PRO_ROTACIONA_ALTURA { get; set; }
            public string PRO_ESCALA_COR { get; set; }
            public double PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
            public double PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
            public string TMP_TIPO_CARGA { get; set; }
            public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
            public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
            public double PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
            public string V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS { get; set; }
            public string V_INPUT_T_GRUPO_PRODUTO_CAIXA { get; set; }
            public string V_INPUT_T_PRODUTO_TINTAS { get; set; }
            public string V_INPUT_T_GRUPO_CHAPA { get; set; }
            public string V_INPUT_T_GRUPO_CONJUNTO { get; set; }
            public string V_INPUT_T_ESTRUTURA_PRODUTO { get; set; }
            public string V_INPUT_T_ROTEIRO { get; set; }
            public string V_INPUT_T_PRODUTO_CONJUNTO { get; set; }

            public string GRP_ID_COMPOSICAO { get; set; }
            public string PRO_ID_INTEGRACAO_ERP_CHAPA { get; set; }
            public double PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA { get; set; }
            public double PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA { get; set; }
            public double PRO_AREA_LIQUIDA_CHAPA { get; set; }
            public double PRO_PESO_CHAPA { get; set; }
            public double CAIXAS_POR_CHAPA { get; set; }
            public string PRO_VINCOS_ONDULADEIRA { get; set; }


            public string PRO_ID_CONJUNTO { get; set; }
            public string PRO_DESCRICAO_CONJUNTO { get; set; }
            public int CAIXAS_POR_CONJUNTO { get; set; }

            public string PRO_ID_CLICHE { get; set; }
            public string PRO_ID_FACA { get; set; }

            public int PRO_ARRANJO_LARGURA { get; set; }
            public int PRO_ARRANJO_COMPRIMENTO { get; set; }

            public string Action { get; set; }

            public ProdutoCaixa ToProduto()
            {
                ProdutoCaixa o = new ProdutoCaixa();
                o = new ProdutoCaixa
                {
                    PRO_ID = this.PRO_ID,
                    ABN_ID = this.ABN_ID,
                    PRO_DESCRICAO = this.PRO_DESCRICAO,
                    UNI_ID = this.UNI_ID,
                    PRO_PECAS_POR_FARDO = this.PRO_PECAS_POR_FARDO,
                    PRO_FARDOS_POR_CAMADA = this.PRO_FARDOS_POR_CAMADA,
                    PRO_GRUPO_PALETIZACAO = this.PRO_GRUPO_PALETIZACAO,
                    PRO_LARGURA_PECA = this.PRO_LARGURA_PECA,
                    PRO_COMPRIMENTO_PECA = this.PRO_COMPRIMENTO_PECA,
                    PRO_ALTURA_PECA = this.PRO_ALTURA_PECA,
                    PRO_LARGURA_INTERNA = this.PRO_LARGURA_INTERNA,
                    PRO_COMPRIMENTO_INTERNA = this.PRO_COMPRIMENTO_INTERNA,
                    PRO_ALTURA_INTERNA = this.PRO_ALTURA_INTERNA,
                    PRO_AREA_LIQUIDA = this.PRO_AREA_LIQUIDA,
                    PRO_PESO = this.PRO_PESO,
                    PRO_FRENTE = this.PRO_FRENTE,
                    PRO_ROTACIONA_LARGURA = this.PRO_ROTACIONA_LARGURA,
                    PRO_ROTACIONA_COMPRIMENTO = this.PRO_ROTACIONA_COMPRIMENTO,
                    PRO_ROTACIONA_ALTURA = this.PRO_ROTACIONA_ALTURA,
                    PRO_LARGURA_EMBALADA = this.PRO_LARGURA_EMBALADA,
                    PRO_COMPRIMENTO_EMBALADA = this.PRO_COMPRIMENTO_EMBALADA,
                    PRO_ALTURA_EMBALADA = this.PRO_ALTURA_EMBALADA,
                    PRO_QTD_CANTONEIRAS = this.PRO_QTD_CANTONEIRAS,
                    PRO_QTD_FORROS = this.PRO_QTD_FORROS,
                    PRO_QTD_CHAPEL = this.PRO_QTD_CHAPEL,
                    PRO_TEMPO_CARREGAMENTO_UNITARIO = this.PRO_TEMPO_CARREGAMENTO_UNITARIO,
                    PRO_TEMPO_DESCARREGAMENTO_UNITARIO = this.PRO_TEMPO_DESCARREGAMENTO_UNITARIO,
                    TMP_TIPO_CARGA = this.TMP_TIPO_CARGA,
                    PRO_PERCENTUAL_JANELA_EMBARQUE = this.PRO_PERCENTUAL_JANELA_EMBARQUE,
                    GRP_ID = this.GRP_ID,
                    PRO_ID_INTEGRACAO = this.PRO_ID_INTEGRACAO,
                    PRO_ID_INTEGRACAO_ERP = this.PRO_ID_INTEGRACAO_ERP,


                    GRP_ID_COMPOSICAO = this.GRP_ID_COMPOSICAO,
                    PRO_ID_INTEGRACAO_ERP_CHAPA = this.PRO_ID_INTEGRACAO_ERP_CHAPA,
                    PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA = this.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA,
                    PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA = this.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA,
                    PRO_AREA_LIQUIDA_CHAPA = this.PRO_AREA_LIQUIDA_CHAPA,
                    PRO_PESO_CHAPA = this.PRO_PESO_CHAPA,
                    CAIXAS_POR_CHAPA = this.CAIXAS_POR_CHAPA,
                    PRO_VINCOS_ONDULADEIRA = this.PRO_VINCOS_ONDULADEIRA,

                    PRO_CAMADAS_POR_PALETE = this.PRO_CAMADAS_POR_PALETE,
                    PRO_VINCOS_COMPRIMENTO = this.PRO_VINCOS_COMPRIMENTO,
                    PRO_VINCOS_LARGURA = this.PRO_VINCOS_LARGURA,

                    PRO_ARRANJO_LARGURA = this.PRO_ARRANJO_LARGURA,
                    PRO_ARRANJO_COMPRIMENTO = this.PRO_ARRANJO_COMPRIMENTO,

                    //GRP_ID_CONJUNTO = this.GRP_ID_CONJUNTO,
                    PRO_ID_CONJUNTO = this.PRO_ID_CONJUNTO,
                    PRO_DESCRICAO_CONJUNTO = this.PRO_DESCRICAO_CONJUNTO,
                    CAIXAS_POR_CONJUNTO = this.CAIXAS_POR_CONJUNTO,

                    PRO_ID_CLICHE = this.PRO_ID_CLICHE,
                    PRO_ID_FACA = this.PRO_ID_FACA,

                    PlayAction = this.Action
                };
                return o;
            }
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_GRUPO_PRODUTO_CAIXA != "")
                {
                    msg += "GRUPO_PRODUTO_CAIXA_" + this.V_INPUT_T_GRUPO_PRODUTO_CAIXA + ";";
                }
                if (this.V_INPUT_T_PRODUTO_TINTAS != "")
                {
                    msg += "PRODUTO_TINTAS_" + this.V_INPUT_T_PRODUTO_TINTAS + ";";
                }
                if (this.V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS != "")
                {
                    msg += "PRODUTO_CHAPAS_INTERMEDIARIAS_" + this.V_INPUT_T_PRODUTO_CHAPAS_INTERMEDIARIAS + ";";
                }
                if (this.V_INPUT_T_GRUPO_CHAPA != "")
                {
                    msg += "GRUPO_CHAPA_" + this.V_INPUT_T_GRUPO_CHAPA + ";";
                }
                if (this.V_INPUT_T_GRUPO_CONJUNTO != "")
                {
                    msg += "GRUPO_CONJUNTO_" + this.V_INPUT_T_GRUPO_CONJUNTO + ";";
                }
                if (this.V_INPUT_T_PRODUTO_CONJUNTO != "")
                {
                    msg += "PRODUTO_CONJUNTO_" + this.V_INPUT_T_PRODUTO_CONJUNTO + ";";
                }

                return msg;
            }
        }

    }
}
