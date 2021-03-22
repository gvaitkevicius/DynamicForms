using System.Collections.Generic;
using System.Linq;
using DynamicForms.Context;
using DynamicForms.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Linq.Dynamic.Core;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Util;

namespace DynamicForms.Areas.PlugAndPlay.Controllers.Reports
{
    [Authorize]
    [Area("plugandplay")]
    public class ReportRomaneioController : BaseController
    {
        // url => /PlugAndPlay/ReportRomaneio/GerarPDF
        public IActionResult GerarPDF(string cargaId)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportRomaneioController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Carga carga = db.Carga.AsNoTracking()
                                .Where(x => x.CAR_ID == cargaId && x.CAR_STATUS == 6)
                                .Include(x => x.TipoVeiculo)
                                .Include(x => x.Veiculo)
                                .Include(x => x.Transportadora)
                                .Include(x => x.ItensCarga)
                                    .ThenInclude(it => it.Oredr)
                                        .ThenInclude(o => o.Cliente)
                                            .ThenInclude(c => c.Observacoes)
                                .Include(x => x.V_ITENS_ROMANEADOS)
                                .FirstOrDefault();

                if (carga != null)
                {
                    foreach (var item in carga.ItensCarga)
                    {
                        List<Observacoes> obsFaturamento = item.Oredr.Cliente.Observacoes
                                            .Where(x => x.OBS_TIPO == "F").ToList();
                        item.Oredr.Cliente.Observacoes = obsFaturamento;
                    }
                }

                //var Carga = (from carga in db.Carga
                //             join veiculo in db.Veiculo
                //                on carga.VEI_PLACA equals veiculo.VEI_PLACA
                //             join tipoVeiculo in db.TipoVeiculo
                //                on veiculo.TIP_ID equals tipoVeiculo.TIP_ID
                //             join transportadora in db.Transportadora
                //                on carga.TRA_ID equals transportadora.TRA_ID
                //             join ic in db.ItenCarga
                //                on carga.CAR_ID equals ic.CAR_ID into itensCarga
                //             from ic in itensCarga
                //             join order in db.Order
                //                on ic.ORD_ID equals order.ORD_ID
                //             join cliente in db.Cliente
                //                on order.CLI_ID equals cliente.CLI_ID
                //             join observacoes in db.Observacoes
                //                on new { cliente.CLI_ID, OBS_TIPO = "F" } equals
                //                   new { observacoes.CLI_ID, observacoes.OBS_TIPO }
                //                into observacoesCliente
                //             join ir in db.V_ITENS_ROMANEADOS
                //                on carga.CAR_ID equals ir.CAR_ID into itensRomaneados
                //             where carga.CAR_ID == cargaId && carga.CAR_STATUS == 6
                //             select new Carga
                //             {
                //                 CAR_ID = carga.CAR_ID,
                //                 CAR_STATUS = carga.CAR_STATUS,
                //                 CAR_DATA_INICIO_PREVISTO = carga.CAR_DATA_INICIO_PREVISTO,
                //                 CAR_DATA_FIM_PREVISTO = carga.CAR_DATA_FIM_PREVISTO,
                //                 CAR_PESO_TEORICO = carga.CAR_PESO_TEORICO,
                //                 CAR_PESO_REAL = carga.CAR_PESO_REAL,
                //                 CAR_VOLUME_TEORICO = carga.CAR_VOLUME_TEORICO,
                //                 CAR_VOLUME_REAL = carga.CAR_VOLUME_REAL,
                //                 CAR_PESO_ENTRADA = carga.CAR_PESO_ENTRADA,
                //                 CAR_PESO_SAIDA = carga.CAR_PESO_SAIDA,
                //                 CAR_PESO_EMBALAGEM = carga.CAR_PESO_EMBALAGEM,
                //                 CAR_JUSTIFICATIVA_DE_CARREGAMENTO = carga.CAR_JUSTIFICATIVA_DE_CARREGAMENTO,
                //                 TRA_ID = carga.TRA_ID,
                //                 Transportadora = new Transportadora
                //                 {
                //                     TRA_ID = transportadora.TRA_ID,
                //                     TRA_NOME = transportadora.TRA_NOME,
                //                     TRA_RESPONSAVEL = transportadora.TRA_RESPONSAVEL,
                //                     TRA_EMAIL = transportadora.TRA_EMAIL,
                //                     TRA_FONE = transportadora.TRA_FONE
                //                 },
                //                 VEI_PLACA = carga.VEI_PLACA,
                //                 TipoVeiculo = new TipoVeiculo
                //                 {
                //                     TIP_ID = tipoVeiculo.TIP_ID,
                //                     TIP_DESCRICAO = tipoVeiculo.TIP_DESCRICAO
                //                 },
                //                 Veiculo = new Veiculo
                //                 {
                //                     VEI_PLACA = veiculo.VEI_PLACA,
                //                     VEI_CAPACIDADE_M3 = veiculo.VEI_CAPACIDADE_M3,
                //                     VEI_CAPACIDADE_LARGURA = veiculo.VEI_CAPACIDADE_LARGURA,
                //                     VEI_CAPACIDADE_COMPRIMENTO = veiculo.VEI_CAPACIDADE_COMPRIMENTO,
                //                     VEI_CAPACIDADE_ALTURA = veiculo.VEI_CAPACIDADE_ALTURA
                //                 },
                //                 ItensCarga = itensCarga
                //                                .Select(x => new ItenCarga 
                //                                {
                //                                    CAR_ID = x.CAR_ID,
                //                                    ORD_ID = x.ORD_ID,
                //                                    Oredr = new Order
                //                                    {
                //                                        ORD_ID = order.ORD_ID,
                //                                        Cliente = new Cliente
                //                                        {
                //                                            CLI_ID = cliente.CLI_ID,
                //                                            CLI_NOME = cliente.CLI_NOME,
                //                                            Observacoes = observacoesCliente.ToList()
                //                                        }
                //                                    }
                //                                }).ToList(),
                //                 V_ITENS_ROMANEADOS = itensRomaneados
                //                                        .Select(x => new V_ITENS_ROMANEADOS
                //                                        {
                //                                            CAR_ID = x.CAR_ID,
                //                                            ORD_ID = x.ORD_ID,
                //                                            ITC_ORDEM_ENTREGA = x.ITC_ORDEM_ENTREGA,
                //                                            ITC_ENTREGA_PLANEJADA = x.ITC_ENTREGA_PLANEJADA,
                //                                            ITC_QTD_PLANEJADA = x.ITC_QTD_PLANEJADA,
                //                                            ITC_QTD_REALIZADA = x.ITC_QTD_REALIZADA,
                //                                            QTD_PALETES = x.QTD_PALETES
                //                                        }).ToList()
                //             }
                //            ).FirstOrDefault();

                return new ViewAsPdf(carga);
            }
        }
    }
}