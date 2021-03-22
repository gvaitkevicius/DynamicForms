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
    public class MaquinaI
    {
        public void ImportarMaquinas(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _maquinasImportadas = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            string _erros = "";
            int cont = 0;
            bool flag = true;

            List<string> erros = new List<string>();
            MasterController mc = new MasterController();
            V_INPUT_T_MAQUINAS itAux = new V_INPUT_T_MAQUINAS();
            Console.WriteLine("\n----------------------");
            Console.WriteLine("Importando maquina...");
            var stopwatch = new Stopwatch();
            try
            {
                List<V_INPUT_T_MAQUINAS> _listaInterface = null;
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_MAQUINAS");
                    stopwatch.Start();
                    _listaInterface = db.GetMaquinasInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_MAQUINAS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_MAQUINAS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_MAQUINAS", UtilPlay.getErro(ex)));
                    return;
                }

                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    string ms = itAux.CheckImportMsg();
                    if (flag == true)
                    {
                        flag = String.IsNullOrEmpty(ms);
                    }
                    if (flag)
                    {
                        _maquinasImportadas.Add(itAux.ToMaquina());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToMaquina(), "OK", ""));//Log deu certo
                        //--
                    }
                    else
                    {
                        var msvet = itAux.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itAux.ToMaquina(), "ERRO", itAux.CheckImportMsg() + " " + itAux.ACTION));//Log deu Errado   
                        foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                        {
                            if (!String.IsNullOrEmpty(it.Trim()) && !erros.Contains(it))
                                erros.Add(it);
                            _erros += " " + it;
                        }
                    }
                    cont++;
                }
                if (!flag)
                {
                    if (_erros.Contains("GRUPO_MAQUINAS"))
                    {
                        GrupoMaquinaI grupoMaquinaInterface = new GrupoMaquinaI();
                        grupoMaquinaInterface.ImportarGrupoMaquina(ref log, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    erros.Clear();
                    _maquinasImportadas.Clear();
                    LogLocal.Clear();
                    Console.WriteLine("Importando maquina apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_MAQUINAS");
                    stopwatch.Start();
                    _listaInterface = db.GetMaquinasInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_MAQUINAS: {stopwatch.Elapsed}");

                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        //Checando se as dependencias de importaçao foram atendidas
                        if (flag)
                        {
                            _maquinasImportadas.Add(itAux.ToMaquina());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToMaquina(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            var msvet = itAux.CheckImportMsg().Split(';');
                            LogLocal.Add(new LogPlay(itAux.ToMaquina(), "ERRO_MAQUINA", itAux.CheckImportMsg() + " " + itAux.ACTION));//Log deu Errado   
                            foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                            {
                                if (!String.IsNullOrEmpty(it.Trim()) && !erros.Contains(it))
                                    erros.Add(it);
                            }
                        }
                        cont++;
                    }
                }

                List<List<object>> ll = new List<List<object>>();
                ll.Add(_maquinasImportadas);
                if (_maquinasImportadas.Count > 0)
                {
                    Console.WriteLine($"Atualizando maquina na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos maquina: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarMaquinas.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de maquina: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                //log erro
                LogLocal.Clear();
                log.Add(new LogPlay("ERRO_MAQUINA", UtilPlay.getErro(ex)));
            }
        }
        public class V_INPUT_T_MAQUINAS
        {
            public string MAQ_ID { get; set; }
            public string MAQ_DESCRICAO { get; set; }
            public int CAL_ID { get; set; }
            public string MAQ_CONTROL_IP { get; set; }
            public string MAQ_STATUS { get; set; }
            public DateTime MAQ_ULTIMA_ATUALIZACAO { get; set; }
            public int MAQ_SIRENE_SEMAFORO { get; set; }
            public string MAQ_COR_SEMAFORO { get; set; }
            public string MAQ_ID_MAQ_PAI { get; set; }
            public int MAQ_TIPO_CONTADOR { get; set; }
            public string MAQ_TIPO_PLANEJAMENTO { get; set; }
            public string MAQ_ID_INTEGRACAO { get; set; }
            public string MAQ_ID_INTEGRACAO_ERP { get; set; }
            public int FPR_ID_OP_PRODUZINDO { get; set; }
            public int MAQ_CONGELA_FILA { get; set; }
            public int MAQ_TEMPO_MIN_PARADA { get; set; }
            public int MAQ_QTD_CORES { get; set; }
            public string GMA_ID { get; set; }
            public string ACTION { get; set; }
            public string V_INPUT_T_GRUPO_MAQUINAS { get; set; }

            public V_INPUT_T_MAQUINAS()
            {

            }
            public Maquina ToMaquina()
            {
                Maquina o = new Maquina();
                o = new Maquina
                {
                    MAQ_ID = this.MAQ_ID,
                    MAQ_DESCRICAO = this.MAQ_DESCRICAO,
                    CAL_ID = this.CAL_ID,
                    MAQ_CONTROL_IP = this.MAQ_CONTROL_IP,
                    GMA_ID = this.GMA_ID,
                    MAQ_STATUS = this.MAQ_STATUS,
                    MAQ_ULTIMA_ATUALIZACAO = this.MAQ_ULTIMA_ATUALIZACAO,
                    MAQ_SIRENE_SEMAFORO = this.MAQ_SIRENE_SEMAFORO,
                    MAQ_COR_SEMAFORO = this.MAQ_COR_SEMAFORO,
                    MAQ_ID_MAQ_PAI = this.MAQ_ID_MAQ_PAI,
                    MAQ_TIPO_CONTADOR = this.MAQ_TIPO_CONTADOR,
                    MAQ_TIPO_PLANEJAMENTO = this.MAQ_TIPO_PLANEJAMENTO,
                    FPR_ID_OP_PRODUZINDO = this.FPR_ID_OP_PRODUZINDO,
                    MAQ_CONGELA_FILA = this.MAQ_CONGELA_FILA,
                    MAQ_TEMPO_MIN_PARADA = this.MAQ_TEMPO_MIN_PARADA,
                    MAQ_QTD_CORES = this.MAQ_QTD_CORES,
                    MAQ_ID_INTEGRACAO = this.MAQ_ID_INTEGRACAO,
                    MAQ_ID_INTEGRACAO_ERP = this.MAQ_ID_INTEGRACAO_ERP,
                    PlayAction = this.ACTION
                };
                return o;
            }
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_GRUPO_MAQUINAS != "")
                {
                    msg += "GRUPO_MAQUINAS_" + this.V_INPUT_T_GRUPO_MAQUINAS + ";";
                }
                return msg;
            }
        }
    }
}
