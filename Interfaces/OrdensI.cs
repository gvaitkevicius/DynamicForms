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
    public class OrdensI
    {
        public void ImportarOrdensProducao(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _pedidosImportados = new List<object>();
            List<string> erros = new List<string>();
            string _erros = "";
            List<LogPlay> LogLocal = new List<LogPlay>();

            MasterController mc = new MasterController();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_ORDENS itAux = new V_INPUT_T_ORDENS();
            var stopwatch = new Stopwatch();
            try
            {
                if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "10"))
                    return;

                //Importando lista da Interface de Pedidos
                List<V_INPUT_T_ORDENS> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando pedidos...");
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_ORDENS");
                    stopwatch.Start();
                    _listaInterface = db.GetOrdensInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_ORDENS: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_ORDENS:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_ORDENS", UtilPlay.getErro(ex)));
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
                        var msvet = itAux.Action.Split('|');
                        itAux.Action = msvet.ElementAt(0);

                        _pedidosImportados.Add(itAux.ToOrder());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itAux.ToOrder(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itAux.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itAux.ToOrder(), "ERRO_ORDENS", itAux.CheckImportMsg() + " " + itAux.Action));//Log deu certo   
                        foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                        {
                            if (!String.IsNullOrEmpty(it.Trim()) && !_erros.Contains(it))
                            {
                                erros.Add(it);
                                _erros += " " + it;
                            }
                        }
                    }
                    cont++;
                }
                if (!flag)
                {
                    LogLocal.Clear();

                    if (_erros.Contains("MUNICIPIOS"))
                    {
                        if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "20"))
                            return;
                        
                        try
                        {
                            MunicipiosI mui = new MunicipiosI();
                            mui.ImportarMunicipios(ref log, forceInsert, db);
                        }
                        catch (Exception ex)
                        {
                            LogLocal.Add(new LogPlay(new Municipio(), "ERRO_MUNICIPIO", UtilPlay.getErro(ex)));
                        }
                    }

                    if (_erros.Contains("CLIENTES"))
                    {
                        if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "30"))
                            return;

                        try
                        {
                            _erros = _erros.Replace("CLIENTES", "");
                            ClientesI cli = new ClientesI();
                            cli.InportarClientes(ref log, forceInsert, db);
                        }
                        catch (Exception ex)
                        {
                            LogLocal.Add(new LogPlay(new Cliente(), "ERRO_CLIENTE", UtilPlay.getErro(ex)));
                        }

                    }

                    if (_erros.Contains("PRODUTO_CHAPAS_VENDAS"))
                    {
                        if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "40"))
                            return;
                        
                        try
                        {
                            ProdutoChapaVendaI pcvi = new ProdutoChapaVendaI();
                            _erros = _erros.Replace("PRODUTO_CHAPAS_VENDAS", "");
                            pcvi.ImportarChapasVenda(ref log, forceInsert, db);
                        }
                        catch (Exception ex)
                        {
                            LogLocal.Add(new LogPlay(new Produto(), "ERRO_CHAPA_VENDA", UtilPlay.getErro(ex)));
                        }

                    }
                    if (_erros.Contains("PRODUTO_CAIXA") || _erros.Contains("PRODUTO_CONJUNTO"))
                    {
                        if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "50"))
                            return;
                        
                        try
                        {
                            _erros = _erros.Replace("PRODUTO_CAIXA", "");
                            ProdutoCaixaI pci = new ProdutoCaixaI();
                            pci.ImportarProdutoCaixas(ref log, forceInsert, db);
                        }
                        catch (Exception ex)
                        {
                            LogLocal.Add(new LogPlay(new Produto(), "ERRO_CAIXA", UtilPlay.getErro(ex)));
                        }

                    }

                    if (!ParametrosSingleton.MsgSingleton("%CImportando ordens de produção", "80"))
                        return;

                    //--- Reconsultando Interface
                    _pedidosImportados.Clear();
                    Console.WriteLine("Importando pedidos apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_ORDENS");
                    stopwatch.Start();
                    _listaInterface = db.GetOrdensInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_ORDENS: {stopwatch.Elapsed}");
                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itAux = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itAux.CheckImportMsg());
                        if (flag)
                        {
                            var msvet = itAux.Action.Split('|');
                            itAux.Action = msvet.ElementAt(0);
                            _pedidosImportados.Add(itAux.ToOrder());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itAux.ToOrder(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itAux.ToOrder(), "ERRO_ORDENS", itAux.CheckImportMsg() + ' ' + itAux.Action));//Log deu erro
                        }
                        cont++;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de pedidos: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay(itAux.ToOrder(), "ERRO_ORDENS", UtilPlay.getErro(ex)));
            }

            List<List<object>> ll = new List<List<object>>
            {
                _pedidosImportados
            };
            if (_pedidosImportados.Count > 0)
            {
                Console.WriteLine($"Atualizando pedidos na base dadados...");
                stopwatch.Start();
                List<LogPlay> logsUpdateData = mc.UpdateData(ll, forceInsert, true, db);
                LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, logsUpdateData);
                stopwatch.Stop();
                Console.WriteLine($"Fim da Atualizacao dos pedidos: {stopwatch.Elapsed}");
            }

            log.AddRange(LogLocal);

            if (!ParametrosSingleton.MsgSingleton("%Importando ordens de produção", "90"))
                return;

            //#region ReportLog
            //LogLocal.ForEach(x => x.Properties = null);
            //UtilPlay.GravarArquivoJson(LogLocal.Where(x => x.Status != "OK").ToList(), "ImportarOrdensProducao.json");
            //#endregion ReportLog
        }

        /// <summary>
        /// Cria uma Mensagem informando a porcentagem atual do andamento da interface e adiciona nas mensagens do ParametrosSingleton
        /// </summary>
        /// <param name="porcentagem">Porcentagem atual (valor entre 0 e 100)</param>
        private void AddMsgPorcentagemInterface(int porcentagem)
        {
            if (porcentagem <= 0 || porcentagem > 100)
                return;
            var msgInterface = new Mensagem
            {
                MEN_TYPE = "PERCENTUAL_INTERFACE",
                MEN_SEND = $"{porcentagem}%",
                MEN_EMISSION = DateTime.Now
            };
            ParametrosSingleton.Instance.Menssagens.Add(msgInterface);
        }

        public class V_INPUT_T_ORDENS
        {
            public V_INPUT_T_ORDENS()
            {

            }
            public string ORD_ID { get; set; }
            public DateTime ORD_DATA_ENTREGA_DE { get; set; }
            public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
            public double ORD_QUANTIDADE { get; set; }
            public double? ORD_PRECO_UNITARIO { get; set; }
            public int ORD_TIPO { get; set; } // 1-normal(PRoduz e Entrega)    2-estoque(Somente produz)    3-somente faturamento (Somente entrega)
            public double? ORD_TOLERANCIA_MAIS { get; set; }
            public double? ORD_TOLERANCIA_MENOS { get; set; }
            public string CLI_ID { get; set; }
            public string PRO_ID { get; set; }
            public string PRO_ID_INTEGRACAO { get; set; }
            public string HASH_KEY { get; set; }
            public DateTime ORD_INICIO_JANELA_EMBARQUE { get; set; }
            public DateTime ORD_FIM_JANELA_EMBARQUE { get; set; }
            public DateTime ORD_EMBARQUE_ALVO { get; set; }
            public DateTime ORD_FIM_GRUPO_PRODUTIVO { get; set; }
            public DateTime ORD_INICIO_GRUPO_PRODUTIVO { get; set; }
            public double? ORD_PESO_UNITARIO { get; set; }
            public double? ORD_PESO_UNITARIO_BRUTO { get; set; }
            public string CAR_TIPO_CARREGAMENTO { get; set; }
            public double? ORD_M2_UNITARIO { get; set; }
            public string ORD_MIT { get; set; }  // Mits iguais o sistema carrega na mesma carga
            public string GRP_ID { get; set; }
            public string MUN_ID_ENTREGA { get; set; }
            public string UF_ID_ENTREGA { get; set; }
            public string ORD_ENDERECO_ENTREGA { get; set; }
            public string ORD_BAIRRO_ENTREGA { get; set; }
            public string ORD_CEP_ENTREGA { get; set; }
            public string ORD_REGIAO_ENTREGA { get; set; }
            public string V_INPUT_T_PRODUTO_CAIXAS { get; set; }
            public string V_INPUT_T_MUNICIPIOS { get; set; }
            public string V_INPUT_T_CLIENTES { get; set; }
            public string V_INPUT_T_PRODUTO_CHAPAS_VENDAS { get; set; }

            public string INPUT_TIPO { get; set; }
            public string ORD_STATUS { get; set; }
            public string ORD_TIPO_FRETE { get; set; }
            public string ORD_ID_CONJUNTO { get; set; }
            public string PRO_ID_CONJUNTO { get; set; }
            public string ORD_OP_INTEGRACAO { get; set; }
            public string ORD_ID_INTEGRACAO { get; set; }
            public string ORD_PED_CLI { get; set; }
            public string PRO_ID_INTEGRACAO_ERP { get; set; }
            public string ORD_VINCOS_ONDULADEIRA { get; set; }
            



            public string Action { get; set; }
            public Order ToOrder()
            {
                Order o = new Order
                {
                    ORD_ID = this.ORD_ID,
                    ORD_DATA_ENTREGA_DE = this.ORD_DATA_ENTREGA_DE,
                    ORD_DATA_ENTREGA_ATE = this.ORD_DATA_ENTREGA_ATE,
                    ORD_QUANTIDADE = this.ORD_QUANTIDADE,
                    ORD_PRECO_UNITARIO = this.ORD_PRECO_UNITARIO,
                    ORD_TIPO = this.ORD_TIPO,
                    ORD_TOLERANCIA_MAIS = this.ORD_TOLERANCIA_MAIS,
                    ORD_TOLERANCIA_MENOS = this.ORD_TOLERANCIA_MENOS,
                    CLI_ID = this.CLI_ID,
                    PRO_ID = this.PRO_ID,
                    HASH_KEY = this.HASH_KEY,
                    ORD_INICIO_JANELA_EMBARQUE = this.ORD_INICIO_JANELA_EMBARQUE,
                    ORD_FIM_JANELA_EMBARQUE = this.ORD_FIM_JANELA_EMBARQUE,
                    ORD_EMBARQUE_ALVO = this.ORD_EMBARQUE_ALVO,
                    ORD_INICIO_GRUPO_PRODUTIVO = this.ORD_INICIO_GRUPO_PRODUTIVO,
                    ORD_FIM_GRUPO_PRODUTIVO = this.ORD_FIM_GRUPO_PRODUTIVO,
                    ORD_PESO_UNITARIO = this.ORD_PESO_UNITARIO,
                    ORD_PESO_UNITARIO_BRUTO = this.ORD_PESO_UNITARIO_BRUTO,
                    CAR_TIPO_CARREGAMENTO = this.CAR_TIPO_CARREGAMENTO,
                    ORD_M2_UNITARIO = this.ORD_M2_UNITARIO,
                    ORD_MIT = this.ORD_MIT,
                    GRP_ID = this.GRP_ID,
                    MUN_ID_ENTREGA = this.MUN_ID_ENTREGA,
                    UF_ID_ENTREGA = this.UF_ID_ENTREGA,
                    ORD_ENDERECO_ENTREGA  = this.ORD_ENDERECO_ENTREGA,
                    ORD_BAIRRO_ENTREGA  = this.ORD_BAIRRO_ENTREGA,
                    ORD_CEP_ENTREGA = this.ORD_CEP_ENTREGA,
                    ORD_REGIAO_ENTREGA = this.ORD_REGIAO_ENTREGA,
                    ORD_ID_CONJUNTO = this.ORD_ID_CONJUNTO,
                    PRO_ID_CONJUNTO = this.PRO_ID_CONJUNTO,
                    PlayAction = this.Action,
                    ORD_OP_INTEGRACAO = this.ORD_OP_INTEGRACAO,
                    ORD_ID_INTEGRACAO = this.ORD_ID_INTEGRACAO,
                    ORD_PED_CLI = this.ORD_PED_CLI,
                    ORD_STATUS = this.ORD_STATUS,
                    ORD_TIPO_FRETE = this.ORD_TIPO_FRETE,
                    PRO_ID_INTEGRACAO_ERP = this.PRO_ID_INTEGRACAO_ERP,
                    ORD_VINCOS_ONDULADEIRA = this.ORD_VINCOS_ONDULADEIRA,
                };
                return o;
            }

            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_CLIENTES != "")
                {
                    msg += "CLIENTES_" + this.V_INPUT_T_CLIENTES + ";";
                }
                if (this.V_INPUT_T_MUNICIPIOS != "")
                {
                    msg += "MUNICIPIOS_" + V_INPUT_T_MUNICIPIOS + ";";
                }
                if (this.V_INPUT_T_PRODUTO_CAIXAS != "")
                {
                    msg += "PRODUTO_CAIXAS_" + this.V_INPUT_T_PRODUTO_CAIXAS + ";";
                }
                if (this.V_INPUT_T_PRODUTO_CHAPAS_VENDAS != "")
                {
                    msg += "PRODUTO_CHAPAS_VENDAS_" + this.V_INPUT_T_PRODUTO_CHAPAS_VENDAS + ";";
                }

                return msg;
            }
        }

    }
}
