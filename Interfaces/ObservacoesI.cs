using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DynamicForms.Interfaces
{
    public class ObservacoesI
    {

        public void ImportarObservacoes(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _observacoesImportadas = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();

            V_INPUT_T_OBSERVACOES itAux = new V_INPUT_T_OBSERVACOES();
            try
            {
                List<V_INPUT_T_OBSERVACOES> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando observacoes...");

                if (!ParametrosSingleton.MsgSingleton("%Importando observações", "10"))
                    return;

                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_OBSERVACOES");
                    stopwatch.Start();
                    _listaInterface = db.GetObservacoesInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_OBSERVACOES: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_OBSERVACOES:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_OBSERVACOES", UtilPlay.getErro(ex)));
                    return;
                }

                if (!ParametrosSingleton.MsgSingleton("%Importando observações", "50"))
                    return;

                int cont = 0;
                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    _observacoesImportadas.Add(itAux.ToObservacoes());
                    LogLocal.Add(new LogPlay(itAux.ToObservacoes(), "OK", ""));

                    cont++;
                }

                if (!ParametrosSingleton.MsgSingleton("%Importando observações", "80"))
                    return;

                List<List<object>> ll = new List<List<object>> { _observacoesImportadas };
                if (_observacoesImportadas.Count > 0)
                {
                    Console.WriteLine($"Atualizando observacoes na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();

                }

                if (!ParametrosSingleton.MsgSingleton("%Importando observações", "90"))
                    return; Console.WriteLine($"Fim da Atualizacao dos observacoes: {stopwatch.Elapsed}");

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarObservacoes.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Falha na importacao de observacoes: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay(itAux.ToObservacoes(), "ERRO_OBSERVACOES", UtilPlay.getErro(ex)));
            }


        }
    }

    public class V_INPUT_T_OBSERVACOES
    {
        public V_INPUT_T_OBSERVACOES() { }
        public int OBS_ID { get; set; }
        public string OBS_TIPO { get; set; }
        public string OBS_DESCRICAO { get; set; }
        public string CLI_ID { get; set; }
        public string MAQ_ID { get; set; }
        public string PRO_ID { get; set; }
        public int ROT_SEQ_TRANFORMACAO { get; set; }
        public string OBS_INTEGRACAO { get; set; }
        public string ACTION { get; set; }

        public Observacoes ToObservacoes()
        {
            Observacoes observacoes = new Observacoes
            {
                OBS_ID = this.OBS_ID,
                OBS_TIPO = this.OBS_TIPO,
                OBS_DESCRICAO = this.OBS_DESCRICAO,
                CLI_ID = this.CLI_ID,
                MAQ_ID = this.MAQ_ID,
                PRO_ID = this.PRO_ID,
                ROT_SEQ_TRANFORMACAO = this.ROT_SEQ_TRANFORMACAO,
                OBS_INTEGRACAO = this.OBS_INTEGRACAO,
                PlayAction = this.ACTION
            };

            return observacoes;
        }
    }
}
