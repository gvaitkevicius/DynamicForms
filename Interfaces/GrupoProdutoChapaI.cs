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
    public class GrupoProdutoChapaI
    {
        public void ImportarGrupoChapa(ref List<LogPlay> log, int forceInsert, JSgi db)
        {
            List<object> _gruposImportados = new List<object>();
            List<LogPlay> LogLocal = new List<LogPlay>();
            string _erros = "";
            List<string> erros = new List<string>();
            MasterController mc = new MasterController();
            bool flag = true;
            int cont = 0;
            V_INPUT_T_GRUPO_CHAPA itemAtual = new V_INPUT_T_GRUPO_CHAPA();
            //Importando lista da Interface  
            try
            {
                List<V_INPUT_T_GRUPO_CHAPA> _listaInterface = null;
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Importando grupo de chapa...");
                var stopwatch = new Stopwatch();
                try
                {
                    Console.WriteLine("Executando a query V_INPUT_T_GRUPO_CHAPA");
                    stopwatch.Start();
                    _listaInterface = db.GetGrupoProdutoChapaInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_GRUPO_CHAPA: {stopwatch.Elapsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Falha na execucao da query V_INPUT_T_GRUPO_CHAPA:");
                    Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                    log.Add(new LogPlay(new Order(), "ERRO SELECT * FROM V_INPUT_T_GRUPO_CHAPA", UtilPlay.getErro(ex)));
                    return;
                }
                while (cont < _listaInterface.Count)
                {
                    itemAtual = _listaInterface.ElementAt(cont);
                    //Checando se as dependencias de importaçao foram atendidas
                    if (flag == true)
                    {
                        flag = String.IsNullOrEmpty(itemAtual.CheckImportMsg());//Verificando mensagens de erro da view de interface
                    }
                    if (flag)//se não há erros
                    {
                        _gruposImportados.Add(itemAtual.ToGrupoProduto());//converte objeto de interface em Roteiro
                        LogLocal.Add(new LogPlay(itemAtual.ToGrupoProduto(), "OK", ""));//Log deu certo
                    }
                    else
                    {
                        var msvet = itemAtual.CheckImportMsg().Split(';');
                        LogLocal.Add(new LogPlay(itemAtual.ToGrupoProduto(), "ERRO_GRUPO_CHAPA", itemAtual.CheckImportMsg()));//Log deu certo   
                        foreach (var it in msvet)//Adicionando depêndencias detectadas a lista de dependencias
                        {
                            if (!String.IsNullOrEmpty(it.Trim()) && !erros.Contains(it))
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
                    if (_erros.Contains("PRODUTO_PAPEL"))
                    {
                        ProdutoPapelI ppi = new ProdutoPapelI();
                        ppi.ImportarProdutoPapel(ref LogLocal, forceInsert, db);
                    }

                    //--- Reconsultando Interface
                    _gruposImportados.Clear();
                    LogLocal.Clear();
                    Console.WriteLine("Importando grupo de chapa apos tentar corrigir erros...");
                    Console.WriteLine("Executando a query V_INPUT_T_GRUPO_CHAPA");
                    stopwatch.Start();
                    _listaInterface = db.GetGrupoProdutoChapaInterface().Result.ToList();
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da query V_INPUT_T_GRUPO_CHAPA: {stopwatch.Elapsed}");

                    cont = 0;
                    while (cont < _listaInterface.Count)
                    {
                        itemAtual = _listaInterface.ElementAt(cont);
                        //Checando se as dependencias de importaçao foram atendidas
                        flag = String.IsNullOrEmpty(itemAtual.CheckImportMsg());
                        if (flag)
                        {
                            _gruposImportados.Add(itemAtual.ToGrupoProduto());//converte objeto de interface em Roteiro
                            LogLocal.Add(new LogPlay(itemAtual.ToGrupoProduto(), "OK", ""));//Log deu certo
                        }
                        else
                        {
                            LogLocal.Add(new LogPlay(itemAtual.ToGrupoProduto(), "ERRO_GRUPO_CHAPA", itemAtual.CheckImportMsg()));//Log deu certo log.Add(new LogPlay(itAux, "ERRO", it));//Log deu certo
                        }
                        cont++;
                    }
                }

                List<List<object>> ll = new List<List<object>>();
                ll.Add(_gruposImportados);
                if (_gruposImportados.Count > 0)
                {
                    Console.WriteLine($"Atualizando grupo de chapa na base dadados...");
                    stopwatch.Start();
                    LogLocal = LogLocal.ElementAt(0).ConcatenateLogs(LogLocal, mc.UpdateData(ll, forceInsert, true, db));
                    stopwatch.Stop();
                    Console.WriteLine($"Fim da Atualizacao dos grupo de chapa: {stopwatch.Elapsed}");
                }

                //#region ReportLog
                //LogLocal.ForEach(x => x.Properties = null);
                //var logsParaGravar = LogLocal.Where(x => x.Status != "OK")
                //    .ToList();
                //UtilPlay.GravarArquivoJson(logsParaGravar, "ImportarGrupoChapa.json");
                //#endregion ReportLog


                log.AddRange(LogLocal);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha na importacao de grupo de chapa: ");
                Console.WriteLine($"{UtilPlay.getErro(ex)}\n");
                log.Add(new LogPlay("ERRO_GRUPO_CHAPA", UtilPlay.getErro(ex)));
            }
        }

        public class V_INPUT_T_GRUPO_CHAPA
        {
            public string GRP_ID { get; set; }
            public string GRP_DESCRICAO { get; set; }
            public int? TEM_ID { get; set; }
            public double GRP_TIPO { get; set; }
            public string GRP_PAP_ONDA { get; set; }
            public string GRP_RESINA { get; set; }
            public string GRP_ENDURECEDOR_MIOLO { get; set; }
            public double GRP_PAP_GRAMATURA { get; set; }
            public double GRP_PAP_ALTURA { get; set; }
            public double GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO { get; set; }
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
            public string V_INPUT_T_PRODUTO_PAPEL { get; set; }
            public V_INPUT_T_GRUPO_CHAPA()
            {

            }
            public GrupoProdutoComposicao ToGrupoProduto()
            {
                GrupoProdutoComposicao o = new GrupoProdutoComposicao();
                o = new GrupoProdutoComposicao
                {
                    TEM_ID = this.TEM_ID,
                    GRP_TIPO = this.GRP_TIPO,
                    GRP_PAP_ONDA = this.GRP_PAP_ONDA,
                    GRP_RESINA = this.GRP_RESINA,
                    GRP_ENDURECEDOR_MIOLO = this.GRP_ENDURECEDOR_MIOLO,
                    GRP_PAP_GRAMATURA = this.GRP_PAP_GRAMATURA,
                    GRP_PAP_ALTURA = this.GRP_PAP_ALTURA,
                    GRP_PAP_NOME_COMERCIAL = this.GRP_PAP_NOME_COMERCIAL,
                    GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO = this.GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO,
                    GRP_PAPEL1 = this.GRP_PAPEL1,
                    GRP_PAPEL2 = this.GRP_PAPEL2,
                    GRP_PAPEL3 = this.GRP_PAPEL3,
                    GRP_PAPEL4 = this.GRP_PAPEL4,
                    GRP_PAPEL5 = this.GRP_PAPEL5,
                    GRP_ID = this.GRP_ID,
                    GRP_DESCRICAO = this.GRP_DESCRICAO,
                    GRP_ATIVO = this.GRP_ATIVO,
                    GRP_DT_CRIACAO = this.GRP_DT_CRIACAO,
                    GRP_ID_INTEGRACAO = this.GRP_ID_INTEGRACAO,
                    GRP_ID_INTEGRACAO_ERP = this.GRP_ID_INTEGRACAO_ERP,
                    PlayAction = this.Action
                };
                return o;
            }
            public string CheckImportMsg()
            {
                string msg = "";
                if (this.V_INPUT_T_PRODUTO_PAPEL != "")
                {
                    msg += "PRODUTO_PAPEL_" + this.V_INPUT_T_PRODUTO_PAPEL + "; ";
                }
                return msg;
            }
        }
    }
}
