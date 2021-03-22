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
    public class EstruturaProdutoI
    {
        public void ImportarEstruturaProdutos(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _estruturaProdutoImportados = new List<object>();
            List<string> erros = new List<string>();
            MasterController mc = new MasterController();
            List<LogPlay> LogLocal = new List<LogPlay>();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_ESTRUTURA_PRODUTO itAux = null;
            try
            {
                List<V_INPUT_T_ESTRUTURA_PRODUTO> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando estrura dos produtos...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_ESTRUTURA_PRODUTO");
                    stopwatch.Start();
                    _listaInterface = db.GetEstruturasProdutoInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_ESTRUTURA_PRODUTO: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_ESTRUTURA_PRODUTO:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_ESTRUTURA_PRODUTO", UtilPlay.getErro(ex)));
                    return;
                }

                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    if (flag)//se não há erros
                    {
                        _estruturaProdutoImportados.Add(itAux.ToEstrutura());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToEstrutura(), "OK", ""));//Log deu certo
                    }
                    cont++;
                }

                List<List<object>> ll = new List<List<object>>();
                ll.Add(_estruturaProdutoImportados);
                if (_estruturaProdutoImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando estrura dos produtos na base dadados...");
                    stopwatch.Start();
                    List<LogPlay> logsUpdateData = mc.UpdateData(ll, forceInsert, true, db);
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos estrura dos produtos: {stopwatch.Elapsed}");
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, logsUpdateData);
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarEstruturaProdutos.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de estrura dos produtos: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_ESTRUTURA_PRODUTO", UtilPlay.getErro(ex)));
                //log.Add(new LogPlay(itAux.ToEstrutura(), "ERRO_ESTRUTURA_PRODUTO", (ex.InnerException != null) ? ex.Message + "\n" + ex.InnerException.Message : ex.Message));
            }
        }
        public class V_INPUT_T_ESTRUTURA_PRODUTO
        {

            public string PRO_ID_PRODUTO { get; set; }
            public string PRO_ID_COMPONENTE { get; set; }
            public DateTime EST_DATA_VALIDADE { get; set; }
            public double EST_QUANT { get; set; }
            public DateTime EST_DATA_INCLUSAO { get; set; }
            public double EST_BASE_PRODUCAO { get; set; }
            public string EST_TIPO_REQUISICAO { get; set; }
            public string EST_CODIGO_DE_EXCECAO { get; set; }
            public string GRP_ID_PRODUTO { get; set; }
            public string GRP_ID_COMPONENTE { get; set; }
            public string Action { get; set; }

            public V_INPUT_T_ESTRUTURA_PRODUTO()
            {

            }
            public EstruturaProduto ToEstrutura()
            {
                EstruturaProduto o = new EstruturaProduto();
                o = new EstruturaProduto
                {
                    PRO_ID_PRODUTO = this.PRO_ID_PRODUTO.Trim(),
                    PRO_ID_COMPONENTE = this.PRO_ID_COMPONENTE.Trim(),
                    EST_DATA_VALIDADE = this.EST_DATA_VALIDADE,
                    EST_QUANT = this.EST_QUANT,
                    EST_DATA_INCLUSAO = this.EST_DATA_INCLUSAO,
                    EST_BASE_PRODUCAO = this.EST_BASE_PRODUCAO,
                    EST_TIPO_REQUISICAO = this.EST_TIPO_REQUISICAO,
                    EST_CODIGO_DE_EXCECAO = this.EST_CODIGO_DE_EXCECAO,
                    PlayAction = this.Action
                };
                Produto p = new Produto();
                p.PRO_ID = this.PRO_ID_PRODUTO;
                p.GRP_ID = this.GRP_ID_PRODUTO;
                Produto c = new Produto();
                c.PRO_ID = this.PRO_ID_COMPONENTE;
                c.GRP_ID = this.GRP_ID_COMPONENTE;
                o.ProdutoP = p;
                o.ProdutoComponenteP = c;
                return o;
            }
        }
    }
}
