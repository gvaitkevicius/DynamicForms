using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace DynamicForms.Areas.PlugAndPlay.Controllers.Reports
{
    [Authorize]
    [Area("plugandplay")]
    public class ReportOrdemFabricacaoController : BaseController
    {
        public IActionResult GerarPDF(string OrdId, string SeqTran, string SeqRep, string ProLastro)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportOrdemFabricacaoController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int seqTransformacao = Convert.ToInt32(SeqTran);
                int seqRepeticao = Convert.ToInt32(SeqRep);


                #region query1
                var query1 = (from order in db.Order
                              join municipio in db.Municipio
                                  on order.MUN_ID_ENTREGA equals municipio.MUN_ID
                              join filaProducao in db.FilaProducao
                                  on order.ORD_ID equals filaProducao.ORD_ID
                              join clientes in db.Cliente
                                  on order.CLI_ID equals clientes.CLI_ID
                              join produto in db.Produto
                                  on order.PRO_ID equals produto.PRO_ID
                              join ep in db.EstruturaProduto
                                  on
                                      new { prodId = order.PRO_ID, prodIdConjunto = order.PRO_ID_CONJUNTO }
                                  equals
                                      new { prodId = ep.PRO_ID_COMPONENTE, prodIdConjunto = ep.PRO_ID_PRODUTO }
                                  into estruturaProduto_ConjuntoCaixa
                              from estruturaConjuntoCaixa in estruturaProduto_ConjuntoCaixa.DefaultIfEmpty() // LEFT JOIN
                              where order.ORD_ID == OrdId && filaProducao.ROT_SEQ_TRANFORMACAO == seqTransformacao &&
                                  filaProducao.FPR_SEQ_REPETICAO == seqRepeticao
                              select new
                              {
                                  maquinaIdAtual = filaProducao.ROT_MAQ_ID,

                                  dadosPedido = new
                                  {
                                      clienteId = clientes.CLI_ID,
                                      clienteNome = clientes.CLI_NOME,
                                      qtdPedido = order.ORD_QUANTIDADE,
                                      qtdProduzir = filaProducao.FPR_QUANTIDADE_PREVISTA,
                                      pecasPorConjunto = (estruturaConjuntoCaixa != null) ? estruturaConjuntoCaixa.EST_QUANT : 0,
                                      qtdConjunto = (estruturaConjuntoCaixa != null) ? estruturaConjuntoCaixa.EST_QUANT * filaProducao.FPR_QUANTIDADE_PREVISTA : 0,
                                      ft = produto.PRO_ID,
                                      proIntegracao = produto.PRO_ID_INTEGRACAO,
                                      pedido = order.ORD_ID,
                                      referencia = produto.PRO_DESCRICAO,
                                      dataEntregaDe = order.ORD_DATA_ENTREGA_DE,
                                      dataEntregaAte = order.ORD_DATA_ENTREGA_ATE,
                                      toleranciaMenos = order.ORD_TOLERANCIA_MENOS,
                                      toleranciaMais = order.ORD_TOLERANCIA_MAIS,
                                      localEntrega = order.ORD_ENDERECO_ENTREGA,
                                      municipio = municipio.MUN_NOME,
                                      uf = municipio.UF_COD
                                  },

                                  dadosOnduladeira = new
                                  {
                                      codChapa = produto.PRO_ID_CHAPA,
                                      qtdChapa = produto.QTD_CHAPA,
                                      descricaoChapa = produto.PRO_DESCRICAO_CHAPA,
                                      areaUnitaria = (produto.PRO_LARGURA_PECA_CHAPA / 1000) * (produto.PRO_COMPRIMENTO_PECA_CHAPA / 1000),
                                      larguraChapa = produto.PRO_LARGURA_PECA_CHAPA,
                                      comprimentoChapa = produto.PRO_COMPRIMENTO_PECA_CHAPA,
                                      areaTotal = (filaProducao.FPR_QUANTIDADE_PREVISTA * produto.QTD_CHAPA) * ((produto.PRO_LARGURA_PECA_CHAPA / 1000) * (produto.PRO_COMPRIMENTO_PECA_CHAPA / 1000)),
                                      vincosLargura = produto.PRO_VINCOS_LARGURA,
                                      vincosComprimento = produto.PRO_VINCOS_COMPRIMENTO,
                                  },

                                  dadosConversao = new
                                  {
                                      larguraCaixaInterna = produto.PRO_LARGURA_INTERNA,
                                      comprimentoCaixaInterna = produto.PRO_COMPRIMENTO_INTERNA,
                                      alturaCaixaInterna = produto.PRO_ALTURA_INTERNA,
                                      codCliche = produto.PRO_ID_CLICHE,
                                      codFaca = produto.PRO_ID_FACA,
                                      codDesenho = produto.PRO_COD_DESENHO,
                                      fechamento = produto.PRO_FECHAMENTO,
                                      tipoLap = produto.PRO_TIPO_LAP,
                                      tamanhoLap = produto.PRO_TAMANHO_LAP,
                                      lapProlongado = produto.PRO_LAP_PROLONGADO,
                                      tamanhoLapProlongado = produto.PRO_TAMANHO_LAP_PROLONGADO,
                                      qtdAmarrados = order.ORD_QUANTIDADE / produto.PRO_PECAS_POR_FARDO,
                                      qtdPorAmarrado = produto.PRO_PECAS_POR_FARDO,
                                      arranjoLargura = produto.PRO_ARRANJO_LARGURA,
                                      arranjoComprimento = produto.PRO_ARRANJO_COMPRIMENTO,
                                      fitilhosFardoLargura = produto.PRO_FITILHOS_FARDO_LARG,
                                      fitilhosFardoComprimento = produto.PRO_FITILHOS_FARDO_COMP,
                                      areaUnitaria = (produto.PRO_LARGURA_PECA_CHAPA / 1000) * (produto.PRO_COMPRIMENTO_PECA_CHAPA / 1000) /
                                        (1 / produto.QTD_CHAPA)
                                  },

                                  dadosPaletizacao = new
                                  {
                                      qtdVoltasFilme = produto.PRO_FILME_PALETE,
                                      cantoneiras = produto.PRO_CANTONEIRA,
                                      qtdCantoneirasLarg = produto.PRO_FITILHOS_PALETE_LARG * 2,
                                      qtdCantoneirasComp = produto.PRO_FITILHOS_PALETE_COMP * 2,
                                      qtdEspelho = produto.PRO_QTD_ESPELHO,
                                      codPalete = produto.PRO_ID_PALETE,
                                      descricaoPalete = produto.PRO_DESCRICAO_PALETE,
                                      pecasPorPalete = produto.PRO_PECAS_POR_FARDO * produto.PRO_FARDOS_POR_CAMADA * produto.PRO_CAMADAS_POR_PALETE,
                                      codTampo = produto.PRO_ID_TP_PALETE,
                                      descricaoTampo = produto.PRO_DESCRICAO_TP,
                                      larguraPalete = produto.PRO_LARGURA_PALETE,
                                      comprimentoPalete = produto.PRO_COMPRIMENTO_PALETE,
                                      larguraTampo = produto.PRO_LARGURA_TP_PALETE,
                                      comprimentoTampo = produto.PRO_COMPRIMENTO_TP_PALETE,
                                      qtdFitilhosLarg = produto.PRO_FITILHOS_PALETE_LARG,
                                      qtdFitilhosComp = produto.PRO_FITILHOS_PALETE_COMP
                                  }

                              }).FirstOrDefault();
                #endregion query1

                #region query2
                var query2 = (from ordens in db.Order
                              join filaProducao in db.FilaProducao
                                  on ordens.ORD_ID equals filaProducao.ORD_ID
                              join estruturaProduto in db.EstruturaProduto
                                  on ordens.PRO_ID equals estruturaProduto.PRO_ID_PRODUTO
                              join tintas in db.Produto
                                  on estruturaProduto.PRO_ID_COMPONENTE equals tintas.PRO_ID
                              join grupoProduto in db.GrupoProduto
                                  on tintas.GRP_ID equals grupoProduto.GRP_ID
                              where ordens.ORD_ID == OrdId && filaProducao.ROT_SEQ_TRANFORMACAO == seqTransformacao &&
                                  filaProducao.FPR_SEQ_REPETICAO == seqRepeticao && grupoProduto.GRP_TIPO == 7
                              select new
                              {
                                  cor = tintas.PRO_ID
                              }).ToList();
                #endregion query2

                #region query3
                var query3 = (from filaProducao in db.ViewFilaProducao
                              where filaProducao.OrdId == OrdId
                              orderby filaProducao.RotSeqTransformacao
                              select new
                              {
                                  maquinaId = filaProducao.RotMaqId,
                                  maquina = filaProducao.MaqDescricao,
                                  seqTransformacao = filaProducao.RotSeqTransformacao,
                                  seqRepeticao = filaProducao.FprSeqRepeticao,
                                  inicioPrevisto = filaProducao.FprDataInicioPrevista,
                                  fimPrevisto = filaProducao.FprDataFimPrevista,
                                  qtdPulsos = filaProducao.FprQuantidadePrevista / filaProducao.RotQuantPecasPulso
                              }).ToList();
                #endregion query3

                #region query4
                var query4 = (
                    from estoque in db.SaldosEmEstoquePorLote
                    where estoque.ORD_ID == OrdId && estoque.PRO_ID == query1.dadosOnduladeira.codChapa
                    group estoque by new { estoque.ORD_ID, estoque.MOV_ENDERECO }
                    into qr
                    orderby qr.Key.ORD_ID
                    select new { qr.Key.ORD_ID, qr.Key.MOV_ENDERECO, qtd = qr.Sum(x => x.SALDO) }
                    ).ToList();
                #endregion query4

                #region query5
                var query5 = (from obs in db.Observacoes
                              where (obs.CLI_ID == query1.dadosPedido.clienteId && obs.PRO_ID == null) ||
                                    (obs.CLI_ID == query1.dadosPedido.clienteId && obs.PRO_ID == query1.dadosPedido.ft) ||
                                    (obs.PRO_ID == query1.dadosPedido.ft)
                              select new Observacoes
                              {
                                  OBS_TIPO = obs.OBS_TIPO,
                                  OBS_DESCRICAO = obs.OBS_DESCRICAO
                              }).ToList();
                #endregion query5

                #region dadosPedido
                dynamic dadosPedido = new ExpandoObject();
                dadosPedido.clienteId = query1.dadosPedido.clienteId;
                dadosPedido.clienteNome = query1.dadosPedido.clienteNome;
                dadosPedido.qtdProduzir = query1.dadosPedido.qtdProduzir;
                dadosPedido.pecasPorConjunto = query1.dadosPedido.pecasPorConjunto;
                dadosPedido.qtdConjunto = query1.dadosPedido.qtdConjunto;
                dadosPedido.ft = $"{query1.dadosPedido.ft} - {query1.dadosPedido.proIntegracao}";
                dadosPedido.pedido = query1.dadosPedido.pedido;
                dadosPedido.referencia = query1.dadosPedido.referencia;
                dadosPedido.dataEntregaDe = query1.dadosPedido.dataEntregaDe;
                dadosPedido.dataEntregaAte = query1.dadosPedido.dataEntregaAte;
                dadosPedido.toleranciaMenos = query1.dadosPedido.toleranciaMenos;
                dadosPedido.toleranciaMais = query1.dadosPedido.toleranciaMais;
                dadosPedido.localEntrega = query1.dadosPedido.localEntrega;
                dadosPedido.municipio = query1.dadosPedido.municipio;
                dadosPedido.uf = query1.dadosPedido.uf;
                dadosPedido.observacoes = query5.Where(x => x.OBS_TIPO == "PG").ToList();
                ViewData["dadosPedido"] = dadosPedido;
                #endregion dadosPedido

                #region dadosOnduladeira
                dynamic dadosOnduladeira = new ExpandoObject();
                dadosOnduladeira.codChapa = query1.dadosOnduladeira.codChapa;
                dadosOnduladeira.qtdChapa = query1.dadosOnduladeira.qtdChapa;
                dadosOnduladeira.descricaoChapa = query1.dadosOnduladeira.descricaoChapa;
                dadosOnduladeira.areaUnitaria = query1.dadosOnduladeira.areaUnitaria;
                dadosOnduladeira.larguraChapa = query1.dadosOnduladeira.larguraChapa;
                dadosOnduladeira.comprimentoChapa = query1.dadosOnduladeira.comprimentoChapa;
                dadosOnduladeira.areaTotal = query1.dadosOnduladeira.areaTotal;
                dadosOnduladeira.vincosOnduladeira = query1.dadosOnduladeira.vincosLargura;

                StringBuilder sbEndereco = new StringBuilder();
                foreach (var item in query4)
                {
                    sbEndereco.Append($"{item.MOV_ENDERECO} - {item.qtd}   ");
                }
                dadosOnduladeira.endereco = sbEndereco.ToString();

                dadosOnduladeira.observacoes = query5.Where(x => x.OBS_TIPO == "PO").ToList();

                ViewData["dadosOnduladeira"] = dadosOnduladeira;
                #endregion dadosOnduladeira

                #region roteiro
                List<ExpandoObject> roteiro = new List<ExpandoObject>();
                foreach (var item in query3)
                {

                    dynamic eo = new ExpandoObject();
                    eo.maquinaAtual = (item.maquinaId == query1.maquinaIdAtual) ? true : false;
                    eo.maquina = item.maquina;
                    eo.seqTransformacao = item.seqTransformacao;
                    eo.seqRepeticao = item.seqRepeticao;
                    eo.inicioPrevisto = item.inicioPrevisto;
                    eo.fimPrevisto = item.fimPrevisto;
                    eo.qtdPulsos = item.qtdPulsos;

                    roteiro.Add(eo);
                }
                ViewData["roteiro"] = roteiro;
                #endregion roteiro

                #region dadosConversao
                dynamic dadosConversao = new ExpandoObject();
                dadosConversao.larguraCaixaInterna = query1.dadosConversao.larguraCaixaInterna;
                dadosConversao.comprimentoCaixaInterna = query1.dadosConversao.comprimentoCaixaInterna;
                dadosConversao.alturaCaixaInterna = query1.dadosConversao.alturaCaixaInterna;
                dadosConversao.codCliche = query1.dadosConversao.codCliche;
                dadosConversao.codFaca = query1.dadosConversao.codFaca;
                dadosConversao.codDesenho = query1.dadosConversao.codDesenho;

                switch (query1.dadosConversao.fechamento)
                {
                    case "1":
                        dadosConversao.fechamento = "AUTOMÁTICO";
                        break;
                    case "2":
                        dadosConversao.fechamento = "GRAMPO";
                        break;
                    case "3":
                        dadosConversao.fechamento = "COLA";
                        break;
                    case "4":
                        dadosConversao.fechamento = "S/ FECHAMENTO";
                        break;
                    default:
                        dadosConversao.fechamento = "";
                        break;
                }

                switch (query1.dadosConversao.tipoLap)
                {
                    case "1":
                        dadosConversao.tipoLap = "INTERNO";
                        break;
                    case "2":
                        dadosConversao.tipoLap = "EXTERNO";
                        break;
                    default:
                        dadosConversao.tipoLap = "";
                        break;
                }

                dadosConversao.tamanhoLap = query1.dadosConversao.tamanhoLap;

                switch (query1.dadosConversao.lapProlongado)
                {
                    case "1":
                        dadosConversao.lapProlongado = "LARGURA";
                        break;
                    case "2":
                        dadosConversao.lapProlongado = "COMPRIMENTO";
                        break;
                    default:
                        dadosConversao.lapProlongado = "";
                        break;
                }

                dadosConversao.tamanhoLapProlongado = query1.dadosConversao.tamanhoLapProlongado;
                dadosConversao.qtdAmarrados = query1.dadosConversao.qtdAmarrados;
                dadosConversao.qtdPorAmarrado = query1.dadosConversao.qtdPorAmarrado;

                dadosConversao.arranjoLargura = (query1.dadosConversao.arranjoLargura != null) ?
                    query1.dadosConversao.arranjoLargura : 0;

                dadosConversao.arranjoComprimento = (query1.dadosConversao.arranjoComprimento != null) ?
                    query1.dadosConversao.arranjoComprimento : 0;

                dadosConversao.fitilhosFardoLargura = (query1.dadosConversao.fitilhosFardoLargura != null) ?
                    query1.dadosConversao.fitilhosFardoLargura : 0;

                dadosConversao.fitilhosFardoComprimento = (query1.dadosConversao.fitilhosFardoComprimento != null) ?
                    query1.dadosConversao.fitilhosFardoComprimento : 0;

                #region medidas de conversao
                double somatorioMedidasLarg = 0;
                StringBuilder sbMedidas = new StringBuilder();

                if (query1.dadosConversao.lapProlongado == "1" && query1.dadosConversao.tamanhoLap != null)
                {// Possui lap na largura
                    sbMedidas.Append($"{query1.dadosConversao.tamanhoLap} + ");
                    somatorioMedidasLarg += Convert.ToDouble(query1.dadosConversao.tamanhoLap);
                }

                string[] listMedidasLarg = null;
                string medidasLarg = query1.dadosOnduladeira.vincosLargura;
                if (medidasLarg != null && medidasLarg != "")
                {
                    listMedidasLarg = medidasLarg.Split("X");
                    for (int i = 0; i < listMedidasLarg.Length; i++)
                    {
                        if (i < (listMedidasLarg.Length - 1))
                        {
                            sbMedidas.Append($"{listMedidasLarg[i].Trim()} + ");
                        }
                        else
                        {
                            sbMedidas.Append($"{listMedidasLarg[i].Trim()} = ");
                        }
                        somatorioMedidasLarg += Convert.ToDouble(listMedidasLarg[i].Trim());
                    }
                    sbMedidas.Append($"{somatorioMedidasLarg}");
                }

                //sbMedidas.Append("  x  ");

                double somatorioMedidasComp = 0;
                if (query1.dadosConversao.lapProlongado == "2" && query1.dadosConversao.tamanhoLap != null)
                {// Possui lap no comprimento
                    sbMedidas.Append($"{query1.dadosConversao.tamanhoLap} + ");
                    somatorioMedidasComp += Convert.ToDouble(query1.dadosConversao.tamanhoLap);
                }

                string[] listMedidasComp = null;
                string medidasComp = query1.dadosOnduladeira.vincosComprimento;
                if (medidasComp != null && medidasComp != "")
                {
                    listMedidasComp = medidasComp.Split("X");
                    for (int i = 0; i < listMedidasComp.Length; i++)
                    {
                        if (i < (listMedidasComp.Length - 1))
                        {
                            sbMedidas.Append($"{listMedidasComp[i].Trim()} + ");
                        }
                        else
                        {
                            sbMedidas.Append($"{listMedidasComp[i].Trim()} = ");
                        }
                        somatorioMedidasComp += Convert.ToDouble(listMedidasComp[i].Trim());
                    }
                    sbMedidas.Append($"{somatorioMedidasComp}");
                }

                dadosConversao.medidas = sbMedidas.ToString();
                #endregion medidas de conversao

                dadosConversao.areaUnitaria = query1.dadosConversao.areaUnitaria;

                StringBuilder sbCores = new StringBuilder();
                List<string> cores = query2.Select(x => x.cor).ToList();
                for (int i = 0; i < cores.Count; i++)
                {
                    if (i == cores.Count - 1)
                        sbCores.Append(cores[i]); // é a última cor
                    else
                        sbCores.Append($"{cores[i]} - ");
                }
                dadosConversao.cores = sbCores.ToString();

                dadosConversao.observacoes = query5.Where(x => x.OBS_TIPO == "PC").ToList();

                ViewData["dadosConversao"] = dadosConversao;
                #endregion dadosConversao

                #region dadosPaletizacao
                dynamic dadosPaletizacao = new ExpandoObject();

                #region medidas (emba)
                StringBuilder sbMedidasEmba = new StringBuilder();
                if (query1.dadosConversao.lapProlongado == "1" && query1.dadosConversao.tamanhoLap != null)
                {// Possui Lap prolongado na largura
                    sbMedidasEmba.Append($"{query1.dadosConversao.tamanhoLap} / ");
                }

                if (listMedidasLarg != null)
                {
                    double medidaAcumulada = 0;
                    for (int i = 0; i < listMedidasLarg.Length; i++)
                    {
                        double medida = Convert.ToDouble(listMedidasLarg[i].Trim());
                        if (i < (listMedidasLarg.Length - 1))
                        {
                            sbMedidasEmba.Append($"{medida + medidaAcumulada} / ");
                        }
                        else
                        {
                            sbMedidasEmba.Append($"{medida + medidaAcumulada} = ");
                        }

                        medidaAcumulada += medida;
                    }
                    sbMedidasEmba.Append($"{somatorioMedidasLarg}");
                }

                sbMedidasEmba.Append("  x  ");

                if (query1.dadosConversao.lapProlongado == "2" && query1.dadosConversao.tamanhoLap != null)
                {// Possui Lap prolongado no comprimento
                    sbMedidasEmba.Append($"{query1.dadosConversao.tamanhoLap} / ");
                }

                if (listMedidasComp != null)
                {
                    double medidaAcumulada = 0;
                    for (int i = 0; i < listMedidasComp.Length; i++)
                    {
                        double medida = Convert.ToDouble(listMedidasComp[i].Trim());
                        if (i < (listMedidasComp.Length - 1))
                        {
                            sbMedidasEmba.Append($"{medida + medidaAcumulada} / ");
                        }
                        else
                        {
                            sbMedidasEmba.Append($"{medida + medidaAcumulada} = ");
                        }

                        medidaAcumulada += medida;
                    }
                    sbMedidasEmba.Append($"{somatorioMedidasComp}");
                }

                dadosPaletizacao.medidasEmba = sbMedidasEmba.ToString();
                #endregion medidas (emba)

                if (query1.dadosPaletizacao.codPalete == null)
                {
                    dadosPaletizacao.paletizado = false;
                }
                else
                {
                    dadosPaletizacao.paletizado = true;

                    StringBuilder sbPaletizadoCom = new StringBuilder();
                    if (query1.dadosPaletizacao.qtdVoltasFilme > 0)
                    {
                        sbPaletizadoCom.Append($"FILME ({query1.dadosPaletizacao.qtdVoltasFilme} VOLTAS) + ");

                    }
                    if (query1.dadosPaletizacao.cantoneiras == "S")
                    {
                        int? qtdCantoneiras = query1.dadosPaletizacao.qtdCantoneirasLarg + query1.dadosPaletizacao.qtdCantoneirasComp;
                        sbPaletizadoCom.Append($"CANTONEIRAS (L x C) ({query1.dadosPaletizacao.qtdCantoneirasLarg} + " +
                            $"{query1.dadosPaletizacao.qtdCantoneirasComp} = {qtdCantoneiras}) + ");

                    }
                    if (query1.dadosPaletizacao.qtdEspelho > 0)
                    {
                        sbPaletizadoCom.Append($"ESPELHOS ({query1.dadosPaletizacao.qtdEspelho})");
                    }

                    string strPaletizadoCom = sbPaletizadoCom.ToString();
                    if (strPaletizadoCom.EndsWith("+ "))
                    {
                        int index = strPaletizadoCom.LastIndexOf("+ ");
                        strPaletizadoCom = strPaletizadoCom.Remove(index);
                    }
                    dadosPaletizacao.paletizadoCom = strPaletizadoCom;

                    dadosPaletizacao.codPalete = query1.dadosPaletizacao.codPalete;
                    dadosPaletizacao.descricaoPalete = query1.dadosPaletizacao.descricaoPalete;
                    dadosPaletizacao.qtdPalete = Math.Ceiling(query1.dadosPedido.qtdPedido / query1.dadosPaletizacao.pecasPorPalete.Value);
                    dadosPaletizacao.pecasPorPalete = query1.dadosPaletizacao.pecasPorPalete;
                    dadosPaletizacao.larguraPalete = query1.dadosPaletizacao.larguraPalete;
                    dadosPaletizacao.comprimentoPalete = query1.dadosPaletizacao.comprimentoPalete;
                    dadosPaletizacao.qtdFitilhosLarg = query1.dadosPaletizacao.qtdFitilhosLarg;
                    dadosPaletizacao.qtdFitilhosComp = query1.dadosPaletizacao.qtdFitilhosComp;

                    if (query1.dadosPaletizacao.codTampo == null)
                    {
                        dadosPaletizacao.tampo = false;
                    }
                    else
                    {
                        dadosPaletizacao.tampo = true;
                        dadosPaletizacao.codTampo = query1.dadosPaletizacao.codTampo;
                        dadosPaletizacao.descricaoTampo = query1.dadosPaletizacao.descricaoTampo;
                        dadosPaletizacao.qtdTampo = Math.Ceiling(query1.dadosPedido.qtdPedido / query1.dadosPaletizacao.pecasPorPalete.Value);
                        dadosPaletizacao.larguraTampo = query1.dadosPaletizacao.larguraTampo;
                        dadosPaletizacao.comprimentoTampo = query1.dadosPaletizacao.comprimentoTampo;
                    }

                    dadosPaletizacao.observacoes = query5.Where(x => x.OBS_TIPO == "PA").ToList();
                }

                ViewData["dadosPaletizacao"] = dadosPaletizacao;
                #endregion dadosPaletizacao

                #region dadosExpedicao

                dynamic dadosExpedicao = new ExpandoObject();

                dadosExpedicao.observacoes = query5.Where(x => x.OBS_TIPO == "EP").ToList();
                ViewData["dadosExpedicao"] = dadosExpedicao;
                #endregion dadosExpedicao


                dynamic imgLastro = new ExpandoObject();
                if (ProLastro != null)
                {
                    if (ProLastro.ToUpper() == "NULL")
                        ProLastro = "default";
                }
                else
                    ProLastro = "default";


                imgLastro.nome = ProLastro;
                imgLastro.path = ProLastro + ".jpg";
                ViewData["imgLastro"] = imgLastro;
                return new ViewAsPdf(ViewData);
            }

        }
    }
}