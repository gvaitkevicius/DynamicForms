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
    public class ProdutoFacaI
    {
        public void ImportarProdutoFaca(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            MasterController mc = new MasterController();
            List<object> _produtoImportados = new List<object>();
            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            int cont = 0;
            V_INPUT_T_PRODUTO_FACAS itAux = new V_INPUT_T_PRODUTO_FACAS();
            try
            {
                List<V_INPUT_T_PRODUTO_FACAS> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando faca...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_PRODUTO_FACAS");
                    stopwatch.Start();
                    _listaInterface = db.GetProdutoFacasInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_PRODUTO_FACAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_PRODUTO_FACAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_PRODUTO_FACAS", UtilPlay.getErro(ex)));
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
                    Console.WriteLine($"Atualizando faca na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos faca: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarProdutoFaca.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de faca: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_FACA", UtilPlay.getErro(ex)));
            }

        }
    }
    public class V_INPUT_T_PRODUTO_FACAS
    {
        public string PRO_ID { get; set; }
        public string PRO_DESCRICAO { get; set; }
        public string UNI_ID { get; set; }
        public string PRO_ID_INTEGRACAO { get; set; }
        public string PRO_ID_INTEGRACAO_ERP { get; set; }
        public string GRP_ID { get; set; }
        public int? TEM_ID { get; set; }
        public string Action { get; set; }
        public ProdutoFaca ToProduto()
        {
            ProdutoFaca o = new ProdutoFaca();
            o = new ProdutoFaca
            {
                PRO_ID = this.PRO_ID,
                PRO_DESCRICAO = this.PRO_DESCRICAO,
                UNI_ID = this.UNI_ID,
                GRP_ID = this.GRP_ID,
                PlayAction = this.Action
            };
            return o;
        }
        public V_INPUT_T_PRODUTO_FACAS()
        {

        }
    }
}
