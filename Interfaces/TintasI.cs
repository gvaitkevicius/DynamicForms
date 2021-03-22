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
    public class TintasI
    {
        public void ImportarTintas(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _produtoImportados = new List<object>();
            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            string _erros = "";
            int cont = 0;
            bool flag = true;
            MasterController mc = new MasterController();
            V_INPUT_T_PRODUTO_TINTAS itAux = new V_INPUT_T_PRODUTO_TINTAS();
            var stopwatch = new Stopwatch();
            try
            {
                List<V_INPUT_T_PRODUTO_TINTAS> _listaInterface = new List<V_INPUT_T_PRODUTO_TINTAS>();
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando tintas...");
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_TINTAS");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoTintalInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_TINTAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_TINTAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_TINTAS", UtilPlay.getErro(ex)));
                    return;
                }
                //Checando se as dependencias de importaçao foram atendidas
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
                        _produtoImportados.Add(itAux.ToProdutoTinta());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToProdutoTinta(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itAux.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itAux.ToProdutoTinta(), "ERRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo   
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
                    if (_erros.Contains("GRUPO_PRODUTO_TINTA"))
                    {
                        GrupoProdutoTintaI gpti = new GrupoProdutoTintaI();
                        gpti.ImportarGrupoProdutoTinta(ref log, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    LogLocal.Clear();
                    _produtoImportados.Clear();
                    Console.WriteLine("Importando tintas apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_TINTAS");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoTintalInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_TINTAS: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        if (flag)
                        {
                            _produtoImportados.Add(itAux.ToProdutoTinta());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToProdutoTinta(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itAux.ToProdutoTinta(), "ERRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de tintas: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_TINTA", UtilPlay.getErro(ex)));
            }
            List<List<object>> ll = new List<List<object>>
            {
                _produtoImportados
            };
            if (_produtoImportados.Count > 0)
            {
                Console.WriteLine($"Atualizando tintas na base dadados...");
                stopwatch.Start();
                LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                stopwatch.Stop();
                Console.WriteLine($"Fim da Atualizacao dos tintas: {stopwatch.Elapsed}");
            }

            //#region ReportLog
            //LogLocal.ForEach(x => x.Properties = null);
            //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarTintas.json");
            //#endregion ReportLog

            log.AddRange(LogLocal);
        }
        public class V_INPUT_T_PRODUTO_TINTAS
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

            public string V_INPUT_T_GRUPO_PRODUTO_TINTA { get; set; }

            public V_INPUT_T_PRODUTO_TINTAS()
            {

            }
            public ProdutoTinta ToProdutoTinta()
            {
                ProdutoTinta o = new ProdutoTinta();
                o = new ProdutoTinta
                {
                    PRO_ID = this.PRO_ID,
                    PRO_DESCRICAO = this.PRO_DESCRICAO,
                    UNI_ID = this.UNI_ID,
                    GRP_ID = this.GRP_ID,
                    PRO_ESCALA_COR = this.PRO_ESCALA_COR,
                    PRO_CUSTO_SUBIDA_ESCALA_COR = this.PRO_CUSTO_SUBIDA_ESCALA_COR,
                    PRO_CUSTO_DECIDA_ESCALA_COR = this.PRO_CUSTO_DECIDA_ESCALA_COR,
                    PRO_ID_INTEGRACAO = this.PRO_ID_INTEGRACAO,
                    PRO_ID_INTEGRACAO_ERP = this.PRO_ID_INTEGRACAO_ERP,

                    PlayAction = this.Action
                };
                return o;
            }
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_GRUPO_PRODUTO_TINTA != "")
                {
                    msg += "GRUPO_PRODUTO_TINTA_" + this.V_INPUT_T_GRUPO_PRODUTO_TINTA + ";";
                }
                if (Action.Contains("ERRO"))
                {
                    msg += " ";
                }
                return msg;
            }
        }

    }
}
