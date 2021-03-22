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
    public class ProdutoChapaVendaI
    {
        public void ImportarChapasVenda(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _gruposImportados = new List<object>();
            List<string> erros = new List<string>();
            string _erros = "";
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_PRODUTO_CHAPAS_VENDAS itemAtual = new V_INPUT_T_PRODUTO_CHAPAS_VENDAS();
            try
            {
                //Importando lista da Interface  
                List<V_INPUT_T_PRODUTO_CHAPAS_VENDAS> _listaInterface = new List<V_INPUT_T_PRODUTO_CHAPAS_VENDAS>();
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando chapa venda...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_CHAPAS_VENDAS");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoChapasVendaInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_CHAPAS_VENDAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_CHAPAS_VENDAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_CHAPAS_VENDAS", UtilPlay.getErro(ex)));
                    return;
                }

                while (cont < _listaInterface.Count)
                {
                    itemAtual = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    if (flag == true)
                    {
                        flag = String.IsNullOrEmpty(itemAtual.CheckImportMsg());//Verificando mensagens de erro da view de interface
                    }
                    if (flag)//se não há erros
                    {
                        _gruposImportados.Add(itemAtual.ToProdutoChapa());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itemAtual.ToProdutoChapa(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itemAtual.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itemAtual.ToProdutoChapa(), "ERRO_CHAPA_VENDA", itemAtual.CheckImportMsg()));//Log deu certo   
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
                    if (_erros.Contains("GRUPO_PRODUTO_CHAPA"))
                    {
                        GrupoProdutoChapaI _grupoChapaI = new GrupoProdutoChapaI();
                        _grupoChapaI.ImportarGrupoChapa(ref log, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    LogLocal.Clear();
                    _gruposImportados.Clear();
                    Console.WriteLine("Importando chapa venda apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_CHAPAS_VENDAS");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoChapasVendaInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_CHAPAS_VENDAS: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itemAtual = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itemAtual.CheckImportMsg());
                        if (flag)
                        {
                            _gruposImportados.Add(itemAtual.ToProdutoChapa());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itemAtual.ToProdutoChapa(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itemAtual.ToProdutoChapa(), "ERRO_CHAPA_VENDA", itemAtual.CheckImportMsg()));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }

                List<List<object>> ll = new List<List<object>>();
                ll.Add(_gruposImportados);
                if (_gruposImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando chapa venda na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos chapa venda: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarChapasVenda.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de chapa venda: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_CHAPA_VENDA", UtilPlay.getErro(ex)));
            }
        }
        public class V_INPUT_T_PRODUTO_CHAPAS_VENDAS
        {
            public V_INPUT_T_PRODUTO_CHAPAS_VENDAS() { }
            public string PRO_ID { get; set; }
            public string PRO_DESCRICAO { get; set; }
            public string UNI_ID { get; set; }
            //public double PRO_PECAS_POR_CONJUNTO { get; set; }
            public double PRO_FARDOS_POR_CAMADA { get; set; }
            public string PRO_GRUPO_PALETIZACAO { get; set; }
            public double PRO_PECAS_POR_FARDO { get; set; }
            public string PRO_ID_INTEGRACAO { get; set; }
            public string PRO_ID_INTEGRACAO_ERP { get; set; }
            public string GRP_ID { get; set; }
            public int? TEM_ID { get; set; }
            public double PRO_LARGURA_PECA { get; set; }
            public double PRO_COMPRIMENTO_PECA { get; set; }
            public double PRO_ALTURA_PECA { get; set; }
            public double PRO_LARGURA_EMBALADA { get; set; }
            public double PRO_COMPRIMENTO_EMBALADA { get; set; }
            public double PRO_ALTURA_EMBALADA { get; set; }
            public double PRO_AREA_LIQUIDA { get; set; }

            public string PRO_FRENTE { get; set; }
            public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
            public string PRO_ROTACIONA_LARGURA { get; set; }
            public string PRO_ROTACIONA_ALTURA { get; set; }
            public string TMP_TIPO_CARGA { get; set; }
            public double PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
            public double PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
            public double PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
            public string V_INPUT_T_GRUPO_PRODUTO_CHAPA { get; set; }
            

            public string Action { get; set; }
            // PODEMOS UNIFICAR CHAPA E CHAPA VENDA PODEM SER UMA UNICA CLASSE E INTERFACE  POIS A CHAPA É CADASTRADA A PARTIR DA CAIXA E A CHAPA VENDA A PARTIR DO PEDIDO POREM AMBOS DEVEM SER CHAPA    CHAPAS PALETIZADAS SÃO TRATADAS COMO UMA CAIXA CUJO ROTEIRO É PALETIZAÇÃO APENAS 
            public ProdutoChapaIntermediaria ToProdutoChapa()
            {
                ProdutoChapaIntermediaria o = new ProdutoChapaIntermediaria();
                o = new ProdutoChapaIntermediaria
                {
                    PRO_ID = this.PRO_ID,
                    PRO_DESCRICAO = this.PRO_DESCRICAO,
                    UNI_ID = this.UNI_ID,
                    PRO_LARGURA_PECA = this.PRO_LARGURA_PECA,
                    PRO_COMPRIMENTO_PECA = this.PRO_COMPRIMENTO_PECA,
                    PRO_ALTURA_PECA = this.PRO_ALTURA_PECA,

                    PRO_LARGURA_EMBALADA = this.PRO_LARGURA_EMBALADA,
                    PRO_COMPRIMENTO_EMBALADA = this.PRO_COMPRIMENTO_EMBALADA,
                    PRO_ALTURA_EMBALADA = this.PRO_ALTURA_EMBALADA,
                    PRO_AREA_LIQUIDA = this.PRO_AREA_LIQUIDA,
                    GRP_ID = this.GRP_ID,
                    PlayAction = this.Action
                };
                return o;
            }

            //public ProdutoChapaVenda ToProduto()
            //{
            //    ProdutoChapaVenda o = new ProdutoChapaVenda();
            //    o = new ProdutoChapaVenda
            //    {
            //        PRO_ID = this.PRO_ID,
            //        PRO_ID_INTEGRACAO = this.PRO_ID_INTEGRACAO,
            //        PRO_DESCRICAO = this.PRO_DESCRICAO,
            //        UNI_ID = this.UNI_ID,
            //        PRO_PECAS_POR_FARDO = this.PRO_PECAS_POR_FARDO,
            //        PRO_FARDOS_POR_CAMADA = this.PRO_FARDOS_POR_CAMADA,
            //        PRO_LARGURA_PECA = this.PRO_LARGURA_PECA,
            //        PRO_COMPRIMENTO_PECA = this.PRO_COMPRIMENTO_PECA,
            //        PRO_ALTURA_PECA = this.PRO_ALTURA_PECA,
            //        PRO_FRENTE = this.PRO_FRENTE,
            //        PRO_ROTACIONA_LARGURA = this.PRO_ROTACIONA_LARGURA,
            //        PRO_ROTACIONA_COMPRIMENTO = this.PRO_ROTACIONA_COMPRIMENTO,
            //        PRO_ROTACIONA_ALTURA = this.PRO_ROTACIONA_ALTURA,
            //        PRO_LARGURA_EMBALADA = this.PRO_LARGURA_EMBALADA,
            //        PRO_COMPRIMENTO_EMBALADA = this.PRO_COMPRIMENTO_EMBALADA,
            //        PRO_ALTURA_EMBALADA = this.PRO_ALTURA_EMBALADA,
            //        PRO_TEMPO_CARREGAMENTO_UNITARIO = this.PRO_TEMPO_CARREGAMENTO_UNITARIO,
            //        PRO_TEMPO_DESCARREGAMENTO_UNITARIO = this.PRO_TEMPO_DESCARREGAMENTO_UNITARIO,
            //        TMP_TIPO_CARGA = this.TMP_TIPO_CARGA,
            //        PRO_PERCENTUAL_JANELA_EMBARQUE = this.PRO_PERCENTUAL_JANELA_EMBARQUE,
            //        GRP_ID = this.GRP_ID,
            //        PlayAction = this.Action
            //    };
            //    return o;
            //}
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_GRUPO_PRODUTO_CHAPA != "")
                {
                    msg += "GRUPO_PRODUTO_CHAPA_" + this.V_INPUT_T_GRUPO_PRODUTO_CHAPA + ";";
                }
                return msg;
            }
        }
    }
}
