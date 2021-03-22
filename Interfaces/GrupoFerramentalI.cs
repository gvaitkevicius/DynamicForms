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
    public class GrupoFerramentalI
    {
        public void ImportarGrupoFerramental(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _grupoProdutoImportados = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();
            int cont = 0;
            V_INPUT_T_GRUPO_FERRAMENTAL itAux = new V_INPUT_T_GRUPO_FERRAMENTAL();
            try
            {
                List<V_INPUT_T_GRUPO_FERRAMENTAL> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando grupo ferramental...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_GRUPO_FERRAMENTAL");
                    stopwatch.Start();
                    _listaInterface = db.GetGrupoFerramentalInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_GRUPO_FERRAMENTAL: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_GRUPO_FERRAMENTAL:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_GRUPO_FERRAMENTAL", UtilPlay.getErro(ex)));
                    return;
                }

                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    itAux.Action = "Insert";
                    _grupoProdutoImportados.Add(itAux.ToGrupoProduto());
                    LogLocal.Add(new LogPlay(itAux.ToGrupoProduto(), "OK", ""));//Log deu certo
                    cont++;
                }


                List<List<object>> ll = new List<List<object>>();
                ll.Add(_grupoProdutoImportados);
                if (_grupoProdutoImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando grupo ferramental na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos grupo ferramental: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarGrupoFerramental.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de grupo ferramental: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_GRUPO_FERRAMENTAL", UtilPlay.getErro(ex)));
            }

        }
    }
    public class V_INPUT_T_GRUPO_FERRAMENTAL
    {
        public string GRP_ID { get; set; }
        public string GRP_DESCRICAO { get; set; }
        public int? TEM_ID { get; set; }
        public double GRP_TIPO { get; set; }
        public string GRP_PAP_ONDA { get; set; }
        public double GRP_PAP_GRAMATURA { get; set; }
        public double GRP_PAP_ALTURA { get; set; }
        public string GRP_PAP_NOME_COMERCIAL { get; set; }
        public string GRP_ATIVO { get; set; }
        public DateTime GRP_DT_CRIACAO { get; set; }
        public string GRP_PAPEL1 { get; set; }
        public string GRP_PAPEL2 { get; set; }
        public string GRP_PAPEL3 { get; set; }
        public string GRP_PAPEL4 { get; set; }
        public string GRP_PAPEL5 { get; set; }
        public string GRP_ID_INTEGRACAO { get; set; }
        public string GRP_ID_INTEGRACAO_ERP { get; set; }
        public string Action { get; set; }


        public GrupoProdutoOutros ToGrupoProduto()
        {
            GrupoProdutoOutros o = new GrupoProdutoOutros();
            o = new GrupoProdutoOutros
            {
                GRP_ID = this.GRP_ID,
                GRP_DESCRICAO = this.GRP_DESCRICAO,
                GRP_TIPO = this.GRP_TIPO,
                GRP_ATIVO = this.GRP_ATIVO,
                GRP_DT_CRIACAO = this.GRP_DT_CRIACAO,
                GRP_ID_INTEGRACAO = this.GRP_ID_INTEGRACAO,
                GRP_ID_INTEGRACAO_ERP = GRP_ID_INTEGRACAO_ERP,
                PlayAction = this.Action
            };
            return o;
        }
        public V_INPUT_T_GRUPO_FERRAMENTAL()
        {

        }
    }
}
