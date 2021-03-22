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
    public class ClientesI
    {
        public void InportarClientes(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> __clientesImportados = new List<object>();
            List<string[]> _className = new List<string[]>();
            List<string> erros = new List<string>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            MasterController mc = new MasterController();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_CLIENTES itAux = null;
            try
            {
                List<V_INPUT_T_CLIENTES> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando clientes...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_CLIENTES");
                    stopwatch.Start();
                    _listaInterface = db.GetClientesInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_CLIENTES: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_CLIENTES:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_CLIENTES", UtilPlay.getErro(ex)));
                    return;
                }

                while (cont < _listaInterface.Count)
                {
                    itAux = _listaInterface.ElementAt(cont);
                    flag = String.IsNullOrEmpty(itAux.CheckImportMsg());//Verificando mensagens de erro da view de interface
                    //Checando se as dependencias de importaçao foram atendidas
                    if (flag)//se não há erros
                    {
                        __clientesImportados.Add(itAux.ToCliente());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToCliente(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        LogLocal.Add(new LogPlay(itAux.ToCliente(), "ERRO_CLIENTES", itAux.CheckImportMsg()));
                    }
                    cont++;
                }


                List<List<object>> ll = new List<List<object>>();
                ll.Add(__clientesImportados);
                if (__clientesImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando clientes na base dadados...");
                    stopwatch.Start();
                    List<LogPlay> logsUpdateData = mc.UpdateData(ll, forceInsert, true, db);
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos clientes: {stopwatch.Elapsed}");
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, logsUpdateData);
                }
                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "InportarClientes.json");
                //#endregion ReportLog

                log.AddRange(LogLocal);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de clientes: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_CLIENTE", UtilPlay.getErro(ex)));
                //log.Add(new LogPlay(itAux.ToCliente(), "ERRO_CLIENTE", (ex.InnerException != null) ? ex.Message + "\n" + ex.InnerException.Message : ex.Message));
            }
        }
    }
    public class V_INPUT_T_CLIENTES
    {
        public V_INPUT_T_CLIENTES()
        {

        }

        public string CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string CLI_FONE { get; set; }
        public string CLI_OBS { get; set; }
        public string CLI_ENDERECO_ENTREGA { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CLI_BAIRRO_ENTREGA { get; set; }
        public string CLI_CEP_ENTREGA { get; set; }
        public string CLI_EMAIL { get; set; }
        public string CLI_INTEGRACAO { get; set; }
        public string MUN_ID_ENTREGA { get; set; }
        public double CLI_TRANSLADO { get; set; }
        public string CLI_REGIAO_ENTREGA { get; set; }
        public int CLI_EXIGENTE_NA_IMPRESSAO { get; set; }
        public double CLI_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        public double CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO { get; set; }
        public double CLI_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        public string Action { get; set; }

        public Cliente ToCliente()
        {
            Cliente o = new Cliente
            {
                CLI_ID = this.CLI_ID,
                CLI_NOME = this.CLI_NOME,
                CLI_FONE = this.CLI_FONE,
                CLI_OBS = this.CLI_OBS,
                CLI_ENDERECO_ENTREGA = this.CLI_ENDERECO_ENTREGA,
                CLI_CPF_CNPJ = this.CLI_CPF_CNPJ,
                CLI_BAIRRO_ENTREGA = this.CLI_BAIRRO_ENTREGA,
                CLI_CEP_ENTREGA = this.CLI_CEP_ENTREGA,
                CLI_EMAIL = this.CLI_EMAIL,
                CLI_INTEGRACAO = this.CLI_INTEGRACAO,
                MUN_ID = this.MUN_ID_ENTREGA,
                CLI_TRANSLADO = this.CLI_TRANSLADO,
                CLI_REGIAO_ENTREGA = this.CLI_REGIAO_ENTREGA,
                CLI_EXIGENTE_NA_IMPRESSAO = this.CLI_EXIGENTE_NA_IMPRESSAO,
                CLI_PERCENTUAL_JANELA_EMBARQUE = this.CLI_PERCENTUAL_JANELA_EMBARQUE,
                CLI_TEMPO_DESCARREGAMENTO_UNITARIO = this.CLI_TEMPO_DESCARREGAMENTO_UNITARIO,
                CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO = this.CLI_TEMPO_MEDIO_ESPERA_DE_DESCARREGAMENTO,
                PlayAction = this.Action
            };

            return o;
        }

        public string CheckImportMsg()
        {
            string msg = "";
            if (this.Action.Contains("ERRO"))
            {
                msg += this.Action;
            }
            return msg;
        }
    }


}
