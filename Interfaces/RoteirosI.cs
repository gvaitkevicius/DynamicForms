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
    public class RoteirosI
    {
        public void ImportarRoteiros(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            MasterController mc = new MasterController();
            List<object> roteirosImportados = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            List<string> erros = new List<string>();
            string _erros = "";
            bool flag = true;
            int cont = 0;
            V_INPUT_T_ROTEIROS itAux = new V_INPUT_T_ROTEIROS();
            try
            {
                //Importando lista da Interface de Pedidos
                List<V_INPUT_T_ROTEIROS> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando roteiro...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_ROTEIROS");
                    stopwatch.Start();
                    _listaInterface = db.GetRoteirosInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_ROTEIROS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_ROTEIROS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_ROTEIROS", UtilPlay.getErro(ex)));
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
                        roteirosImportados.Add(itAux.ToRoteiro());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToRoteiro(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itAux.CheckImportMsg().Split(';');

                        LogLocal.Add(new LogPlay(itAux.ToRoteiro(), "ERRO_ROTEIRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo   
                        if (msvet.Length > 0)
                        {
                            foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                            {
                                if (!String.IsNullOrEmpty(it.Trim()) && !erros.Contains(it))
                                {
                                    erros.Add(it);
                                    _erros += " " + it;
                                }
                            }
                        }
                        else
                        {
                            _erros += itAux.CheckImportMsg();
                        }

                    }
                    cont++;
                }
                if (!flag)
                {
                    LogLocal.Clear();
                    if (_erros.Contains("MAQUINAS"))
                    {
                        MaquinaI mqi = new MaquinaI();
                        mqi.ImportarMaquinas(ref log, forceInsert, db);
                    }
                    //--- Reconsultando Interface
                    roteirosImportados.Clear();
                    Console.WriteLine("Importando roteiro apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_ROTEIROS");
                    stopwatch.Start();
                    _listaInterface = db.GetRoteirosInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_ROTEIROS: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        if (flag)
                        {
                            roteirosImportados.Add(itAux.ToRoteiro());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToRoteiro(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itAux.ToRoteiro(), "ERRO_ROTEIRO", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }

                List<List<object>> ll = new List<List<object>>
                {
                    roteirosImportados
                };
                if (roteirosImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando roteiro na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos roteiro: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal
                //    .Where(x => x.Status != "OK")
                //    .ToList(), "ImportarRoteiros.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de roteiro: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_ROTEIRO", UtilPlay.getErro(ex)));
            }
        }
        public class V_INPUT_T_ROTEIROS
        {
            public V_INPUT_T_ROTEIROS()
            {

            }
            public string PRO_ID { get; set; }
            public string MAQ_ID { get; set; }
            public string GMA_ID { get; set; }
            public double ROT_PECAS_POR_PULSO { get; set; }
            public double ROT_PERFORMANCE { get; set; }
            public double ROT_TEMPO_SETUP { get; set; }
            public int ROT_SEQ_TRANFORMACAO { get; set; }
            public double ROT_PRIORIDADE_INFORMADA { get; set; }
            public string ROT_ACAO { get; set; }
            public double ROT_TEMPO_SETUP_AJUSTE { get; set; }
            public string ROT_STATUS { get; set; }

            public string V_INPUT_T_MAQUINAS { get; set; }
            public string Action { get; set; }
            public string CheckImportMsg()

            {
                string msg = "";
                if (this.V_INPUT_T_MAQUINAS != "")
                {
                    msg += "MAQUINAS_" + this.V_INPUT_T_MAQUINAS + ";";
                }
                return msg;
            }
            public Roteiro ToRoteiro()
            {
                Roteiro o = new Roteiro();
                o = new Roteiro
                {
                    GMA_ID = this.GMA_ID,
                    PRO_ID = this.PRO_ID,
                    MAQ_ID = this.MAQ_ID,
                    //GrupoMaquinaId = this.GMA_ID,
                    ROT_SEQ_TRANFORMACAO = this.ROT_SEQ_TRANFORMACAO,
                    ROT_PRIORIDADE_INFORMADA = this.ROT_PRIORIDADE_INFORMADA,
                    ROT_PECAS_POR_PULSO = this.ROT_PECAS_POR_PULSO,
                    ROT_ACAO = this.ROT_ACAO,
                    ROT_PERFORMANCE = this.ROT_PERFORMANCE,
                    ROT_TEMPO_SETUP = this.ROT_TEMPO_SETUP,
                    ROT_TEMPO_SETUP_AJUSTE = this.ROT_TEMPO_SETUP_AJUSTE,
                    ROT_STATUS = this.ROT_STATUS,
                    PlayAction = this.Action
                };
                return o;
            }

        }
    }
}