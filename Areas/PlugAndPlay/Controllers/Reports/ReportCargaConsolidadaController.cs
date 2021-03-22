using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Controllers.Reports
{
    [Authorize]
    [Area("plugandplay")]
    public class ReportCargaConsolidadaController : BaseController
    {
        // url => /PlugAndPlay/ReportCargaConsolidada/GerarPDF?CargaId=A00382
        public IActionResult GerarPDF(string CargaId)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportPlanoCargaController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Carga carga = new Carga();
                try
                {
                    carga = db.Carga.AsNoTracking()
                        .Where(x => x.CAR_ID == CargaId && x.CAR_STATUS >= 6 && x.MovimentoEstoqueVendas.Count > 0)
                        .Include(x => x.Transportadora)
                        .Include(x => x.TipoVeiculo)
                        .Include(x => x.Veiculo)
                        .Include(x => x.MovimentoEstoqueVendas)
                        .Include(x => x.ItensCarga)
                            .ThenInclude(it => it.Oredr)
                                .ThenInclude(o => o.Cliente)
                                    .ThenInclude(c => c.HorariosRecebimentos)
                        .Include(x => x.ItensCarga)
                            .ThenInclude(it => it.Oredr)
                                .ThenInclude(o => o.Cliente)
                                    .ThenInclude(o => o.Municipio)
                        .Include(x => x.ItensCarga)
                            .ThenInclude(it => it.Oredr)
                                .ThenInclude(o => o.Produto)
                                    .ThenInclude(pro => pro.Observacoes)
                        .FirstOrDefault();

                    if (carga == null)
                    {
                        ViewData["msgErro"] = "A carga não está consolidada, ou ela não exite.";
                        return new ViewAsPdf(carga, ViewData);
                    }

                    var cargasWeb = db.CargasWeb.AsNoTracking()
                        .Where(x => x.CAR_ID == CargaId)
                        .GroupBy(x => new { x.CLI_ID, x.CLI_NOME, x.PON_ID });

                    List<ExpandoObject> itensCargaWeb = new List<ExpandoObject>();
                    carga.CAR_PESO_REAL = 0;
                    carga.CAR_VOLUME_REAL = 0;
                    foreach (var item in cargasWeb)
                    {
                        dynamic cabecalhoItens = new ExpandoObject();
                        cabecalhoItens.CLI_ID = item.Key.CLI_ID;
                        cabecalhoItens.CLI_NOME = item.Key.CLI_NOME;
                        cabecalhoItens.MUN_NOME = item.First().PON_DESCRICAO;
                        cabecalhoItens.UF = item.First().UF;
                        cabecalhoItens.TRIANGULAR = (item.First().TRIANGULAR == "S") ? " - (TRIANGULAR)" : "";

                        List<CargasWeb> pedidos = new List<CargasWeb>();
                        double somatorioM3Cliente = 0;
                        double somatorioPeso = 0;
                        for (int i = 0; i < item.Count(); i++)
                        {
                            double qtdCarregada = carga.MovimentoEstoqueVendas.Where(m => m.ORD_ID == item.ElementAt(i).ORD_ID && m.MOV_ESTORNO!= "E").Sum(m => m.MOV_QUANTIDADE);
                            somatorioPeso += (double)(item.ElementAt(i).ORD_PESO_UNITARIO * qtdCarregada); /* QUANTIDADE DE MOVIMENTOS */

                            double? m3Pedido = ((item.ElementAt(i).PRO_LARGURA_EMBALADA / 1000.0) * (item.ElementAt(i).PRO_COMPRIMENTO_EMBALADA / 1000.0) * (item.ElementAt(i).PRO_ALTURA_EMBALADA / 1000.0)) *
                                (qtdCarregada / (item.ElementAt(i).PRO_PECAS_POR_FARDO * item.ElementAt(i).PRO_CAMADAS_POR_PALETE * item.ElementAt(i).PRO_FARDOS_POR_CAMADA));
                            somatorioM3Cliente += m3Pedido.Value;
                            
                            
                            CargasWeb dadosPedido = new CargasWeb();
                            dadosPedido.ORD_ID = item.ElementAt(i).ORD_ID;
                            dadosPedido.PRO_DESCRICAO = item.ElementAt(i).PRO_DESCRICAO;
                            dadosPedido.ORD_DATA_ENTREGA_DE = item.ElementAt(i).ORD_DATA_ENTREGA_DE;
                            dadosPedido.ITC_QTD_PLANEJADA = qtdCarregada; /* QUANTIDADE CARREGADA  */
                            dadosPedido.QTD_UE = carga.MovimentoEstoqueVendas.Where(m => m.ORD_ID == item.ElementAt(i).ORD_ID).Count();
                            dadosPedido.SALDO_ESTOQUE_UE = item.ElementAt(i).SALDO_ESTOQUE_UE;
                            dadosPedido.M3_PEDIDO = m3Pedido.Value;

                            dadosPedido.PRO_LARGURA_EMBALADA = (item.ElementAt(i).PRO_LARGURA_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_LARGURA_EMBALADA;
                            dadosPedido.PRO_COMPRIMENTO_EMBALADA = (item.ElementAt(i).PRO_COMPRIMENTO_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_COMPRIMENTO_EMBALADA;
                            dadosPedido.PRO_ALTURA_EMBALADA = (item.ElementAt(i).PRO_ALTURA_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_ALTURA_EMBALADA;
                            
                            dadosPedido.ORD_PESO_UNITARIO = (item.ElementAt(i).ORD_PESO_UNITARIO);
                            dadosPedido.ORD_QUANTIDADE = qtdCarregada;
                            
                            dadosPedido.ITC_ORDEM_ENTREGA = item.ElementAt(i).ITC_ORDEM_ENTREGA;

                            pedidos.Add(dadosPedido);
                        }
                        cabecalhoItens.totalM3 = somatorioM3Cliente;
                        cabecalhoItens.totalPeso = somatorioPeso;
                        cabecalhoItens.pedidos = pedidos.OrderBy(x => x.ITC_ORDEM_ENTREGA);
                        itensCargaWeb.Add(cabecalhoItens);

                        carga.CAR_PESO_REAL += somatorioPeso;
                        carga.CAR_VOLUME_REAL += somatorioM3Cliente;
                    }


                    var temp = itensCargaWeb.ToList();
                    ViewData["itensCargaWeb"] = itensCargaWeb;

                    List<Cliente> clientes = carga.ItensCarga.GroupBy(x => x.Oredr.Cliente)
                                                        .Select(x => new Cliente
                                                        {
                                                            CLI_ID = x.Key.CLI_ID,
                                                            CLI_NOME = x.Key.CLI_NOME,
                                                            HorariosRecebimentos = x.Key.HorariosRecebimentos
                                                        }).ToList();

                    List<object> horariosRecebimento = UtilPlay.ObterHorariosRecebimentoDoCliente(clientes);
                    List<ExpandoObject> listHorarios = new List<ExpandoObject>();
                    foreach (dynamic obj in horariosRecebimento)
                    {
                        dynamic objeto = new ExpandoObject();
                        objeto.CodCliente = obj.CodCliente;
                        objeto.HorariosRecebimento = obj.HorariosRecebimento;

                        listHorarios.Add(objeto);
                    }
                    ViewData["horariosRecebimento"] = listHorarios;
                }
                catch (Exception ex)
                {
                    ViewData["msgErro"] = UtilPlay.getErro(ex);
                }

                return new ViewAsPdf(carga, ViewData);
            }
        }
    }
}
