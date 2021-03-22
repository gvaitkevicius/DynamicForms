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
    public class ProdutoClichesI
    {
        public void ImportarProdutoCliche(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            MasterController mc = new MasterController();
            List<object> _produtoImportados = new List<object>();
            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            int cont = 0;
            V_INPUT_T_PRODUTO_CLICHES itAux = new V_INPUT_T_PRODUTO_CLICHES();
            try
            {
                List<V_INPUT_T_PRODUTO_CLICHES> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando cliches...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_CLICHES");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoClicheInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_CLICHES: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_CLICHES:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_CLICHES", UtilPlay.getErro(ex)));
                    return;
                }
                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    _produtoImportados.Add(itAux.ToProduto());//converte objeto de interface em Roteiro
                    LogLocal.Add(new LogPlay(itAux.ToProduto(), "OK", ""));//Log deu certo
                    cont++;
                }


                List<List<object>> ll = new List<List<object>>
                {
                    _produtoImportados
                };
                if (_produtoImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando cliches na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos cliches: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarProdutoCliche.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de cliches: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_CLICHE", UtilPlay.getErro(ex)));
            }
        }
    }
}
public class V_INPUT_T_PRODUTO_CLICHES
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

    public ProdutoCliches ToProduto()
    {
        ProdutoCliches o = new ProdutoCliches();
        o = new ProdutoCliches
        {
            PRO_ID = this.PRO_ID,
            PRO_DESCRICAO = this.PRO_DESCRICAO,
            UNI_ID = this.UNI_ID,
            GRP_ID = this.GRP_ID,
            PlayAction = this.Action
        };
        return o;
    }
}

