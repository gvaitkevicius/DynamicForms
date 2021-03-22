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
    public class GrupoMaquinaI
    {
        public void ImportarGrupoMaquina(ref List<LogPlay> log, int forceInsert, JSgi db)
        {

            int cont = 0;
            V_INPUT_T_GRUPO_MAQUINAS itAux = null;
            List<object> _grupoMaquinasImportadas = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();
            try
            {
                List<V_INPUT_T_GRUPO_MAQUINAS> _listaInterface = null; ;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando grupo de maquina...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_GRUPO_MAQUINAS");
                    stopwatch.Start();
                    _listaInterface = db.GetGrupoMAquinaInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_GRUPO_MAQUINAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_GRUPO_MAQUINAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_GRUPO_MAQUINAS", UtilPlay.getErro(ex)));
                    return;
                }
                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    _grupoMaquinasImportadas.Add(itAux.ToGrupo());
                    LogLocal.Add(new LogPlay(itAux.ToGrupo(), "OK", ""));//Log deu certo
                    cont++;
                }

                List<List<object>> ll = new List<List<object>>();
                ll.Add(_grupoMaquinasImportadas);
                if (_grupoMaquinasImportadas.Count > 0)
                {
                    Console.WriteLine($"Atualizando grupo de maquina na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos grupo de maquina: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarGrupoMaquina.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de grupo de maquina: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_GRUPO_MAQUINA", UtilPlay.getErro(ex)));
            }
        }
    }
    public class V_INPUT_T_GRUPO_MAQUINAS
    {
        public string GMA_ID { get; set; }
        public string GMA_DESCRICAO { get; set; }
        public string Action { get; set; }

        public V_INPUT_T_GRUPO_MAQUINAS()
        {

        }
        public GrupoMaquina ToGrupo()
        {
            GrupoMaquina o = new GrupoMaquina()
            {
                GMA_ID = this.GMA_ID,
                GMA_DESCRICAO = this.GMA_DESCRICAO,
                PlayAction = this.Action
            };
            return o;
        }
    }

}
