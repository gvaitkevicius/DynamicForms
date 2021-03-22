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
    public class ProdutoPapelI
    {
        public void ImportarProdutoPapel(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _produtoImportados = new List<object>();
            List<string> erros = new List<string>();
            string _erros = "";
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_PRODUTO_PAPEL itAux = new V_INPUT_T_PRODUTO_PAPEL();
            try
            {
                List<V_INPUT_T_PRODUTO_PAPEL> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando papel...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_PAPEL");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoPapelInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_PAPEL: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_PAPEL:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_PAPEL", UtilPlay.getErro(ex)));
                    return;
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
                    LogLocal.Clear();
                    if (_erros.Contains("GRUPO_PRODUTO_PAPEL"))
                    {
                        GrupoProdutoPapelI gppi = new GrupoProdutoPapelI();
                        gppi.ImportarGrupoPapel(ref log, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    LogLocal.Clear();
                    _produtoImportados.Clear();
                    Console.WriteLine("Importando papel apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_PAPEL");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoPapelInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_PAPEL: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        if (flag)
                        {
                            _produtoImportados.Add(itAux.ToProduto());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToProduto(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itAux.ToProduto(), "ERRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }

                List<List<object>> ll = new List<List<object>>
                {
                    _produtoImportados
                };
                if (_produtoImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando papel na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos papel: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK")
                //    .ToList(), "ImportarProdutoPapel.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de papel: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_PAPEL", UtilPlay.getErro(ex)));
            }
        }
        public class V_INPUT_T_PRODUTO_PAPEL
        {
            public string PRO_ID { get; set; }
            public string PRO_DESCRICAO { get; set; }
            public double PRO_ESTOQUE_ATUAL { get; set; }
            public string UNI_ID { get; set; }
            //public double PRO_PECAS_POR_CONJUNTO { get; set; }
            public double PRO_FARDOS_POR_CAMADA { get; set; }
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
            public double PRO_LARGURA_EMBALADA { get; set; }
            public double PRO_COMPRIMENTO_EMBALADA { get; set; }
            public double PRO_ALTURA_EMBALADA { get; set; }
            public string PRO_FRENTE { get; set; }
            public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
            public string PRO_ROTACIONA_LARGURA { get; set; }
            public string PRO_ROTACIONA_ALTURA { get; set; }
            public string PRO_ESCALA_COR { get; set; }
            public double PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
            public double PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
            public string TMP_TIPO_CARGA { get; set; }
            public double PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
            public double PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
            public double PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
            public string Action { get; set; }
            public string V_INPUT_T_GRUPO_PRODUTO_PAPEL { get; set; }

            public V_INPUT_T_PRODUTO_PAPEL()
            {

            }
            public ProdutoPapel ToProduto()
            {
                ProdutoPapel o = new ProdutoPapel();
                o = new ProdutoPapel
                {
                    PRO_ID = this.PRO_ID,
                    PRO_DESCRICAO = this.PRO_DESCRICAO,
                    PRO_ESTOQUE_ATUAL = this.PRO_ESTOQUE_ATUAL,
                    UNI_ID = this.UNI_ID,
                    PRO_PECAS_POR_FARDO = this.PRO_PECAS_POR_FARDO,
                    PRO_FARDOS_POR_CAMADA = this.PRO_FARDOS_POR_CAMADA,
                    PRO_TIPO_IDENTIFICACAO = this.PRO_TIPO_IDENTIFICACAO,
                    PRO_LARGURA_PECA = this.PRO_LARGURA_PECA,
                    PRO_COMPRIMENTO_PECA = this.PRO_COMPRIMENTO_PECA,
                    PRO_ALTURA_PECA = this.PRO_ALTURA_PECA,
                    PRO_FRENTE = this.PRO_FRENTE,
                    PRO_ROTACIONA_LARGURA = this.PRO_ROTACIONA_LARGURA,
                    PRO_ROTACIONA_COMPRIMENTO = this.PRO_ROTACIONA_COMPRIMENTO,
                    PRO_ROTACIONA_ALTURA = this.PRO_ROTACIONA_ALTURA,
                    PRO_LARGURA_EMBALADA = this.PRO_LARGURA_EMBALADA,
                    PRO_COMPRIMENTO_EMBALADA = this.PRO_COMPRIMENTO_EMBALADA,
                    PRO_ALTURA_EMBALADA = this.PRO_ALTURA_EMBALADA,
                    PRO_TEMPO_CARREGAMENTO_UNITARIO = this.PRO_TEMPO_CARREGAMENTO_UNITARIO,
                    PRO_TEMPO_DESCARREGAMENTO_UNITARIO = this.PRO_TEMPO_DESCARREGAMENTO_UNITARIO,
                    TMP_TIPO_CARGA = this.TMP_TIPO_CARGA,
                    PRO_PERCENTUAL_JANELA_EMBARQUE = this.PRO_PERCENTUAL_JANELA_EMBARQUE,
                    GRP_ID = this.GRP_ID,
                    PRO_ESCALA_COR = this.PRO_ESCALA_COR,
                    PRO_CUSTO_SUBIDA_ESCALA_COR = this.PRO_CUSTO_SUBIDA_ESCALA_COR,
                    PRO_CUSTO_DECIDA_ESCALA_COR = this.PRO_CUSTO_DECIDA_ESCALA_COR,
                    PlayAction = this.Action
                };
                return o;
            }
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_GRUPO_PRODUTO_PAPEL != "")
                {
                    msg += "GRUPO_PRODUTO_PAPEL_" + this.V_INPUT_T_GRUPO_PRODUTO_PAPEL + ";";
                }
                return msg;
            }
        }
    }
}