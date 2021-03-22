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
    public class ProdutoPaleteI
    {
        public void ImportarProdutoPalete(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _produtoImportados = new List<object>();
            MasterController mc = new MasterController();
            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            int cont = 0;
            V_INPUT_T_PRODUTO_PALETE itAux = new V_INPUT_T_PRODUTO_PALETE();
            try
            {
                List<V_INPUT_T_PRODUTO_PALETE> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando palete...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_PALETE");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoPaleteInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_PALETE: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_PALETE:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_PALETE", UtilPlay.getErro(ex)));
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
                    Console.WriteLine($"Atualizando palete na base dadados...");
                    stopwatch.Start();
                    List<LogPlay> logsUpdateData = mc.UpdateData(ll, forceInsert, true, db);
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, logsUpdateData);
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos palete: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarProdutoPalete.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de palete: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_PALETE", UtilPlay.getErro(ex)));
            }
        }

    }
    public class V_INPUT_T_PRODUTO_PALETE
    {
        public string PRO_ID { get; set; }
        public string PRO_DESCRICAO { get; set; }
        public double PRO_ESTOQUE_ATUAL { get; set; }
        public string UNI_ID { get; set; }
        //public double PRO_PECAS_POR_CONJUNTO { get; set; }
        public string PRO_GRUPO_PALETIZACAO { get; set; }
        public string PRO_ID_INTEGRACAO { get; set; }
        public string PRO_ID_INTEGRACAO_ERP { get; set; }
        public string GRP_ID { get; set; }
        public int? TEM_ID { get; set; }
        public double PRO_LARGURA_PECA { get; set; }
        public double PRO_COMPRIMENTO_PECA { get; set; }
        public double PRO_ALTURA_PECA { get; set; }
        public string Action { get; set; }
        public double PRO_PESO { get; set; }
        public ProdutoPalete ToProduto()
        {
            ProdutoPalete o = new ProdutoPalete();
            o = new ProdutoPalete
            {
                PRO_ID = this.PRO_ID,
                PRO_DESCRICAO = this.PRO_DESCRICAO,
                PRO_ESTOQUE_ATUAL = this.PRO_ESTOQUE_ATUAL,
                UNI_ID = this.UNI_ID,
                PRO_LARGURA_PECA = this.PRO_LARGURA_PECA,
                PRO_COMPRIMENTO_PECA = this.PRO_COMPRIMENTO_PECA,
                PRO_ALTURA_PECA = this.PRO_ALTURA_PECA,
                GRP_ID = this.GRP_ID,
                PRO_PESO = this.PRO_PESO,
                PlayAction = this.Action
            };
            return o;
        }

        public V_INPUT_T_PRODUTO_PALETE()
        {

        }
    }
}
