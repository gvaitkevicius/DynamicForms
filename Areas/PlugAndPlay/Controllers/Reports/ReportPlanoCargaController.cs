using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Controllers.Reports
{
    [Authorize]
    [Area("plugandplay")]
    public class ReportPlanoCargaController : BaseController
    {
        // url => /PlugAndPlay/ReportPlanoCarga/GerarPDF?CargaId=
        public IActionResult GerarPDF(string CargaId)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportPlanoCargaController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                Models.Carga carga = new Carga();
                try
                {
                    UsuarioSingleton usuarioSingleton = UsuarioSingleton.Instance;
                    T_Usuario usuario = usuarioSingleton.ObterUsuario(ObterUsuarioLogado().USE_ID);
                    List<T_PREFERENCIAS> pref = usuario.T_PREFERENCIAS.ToList();
                    string un_peso = "QUILOGRAMA";
                    int w = 0;
                    while (w < pref.Count && !pref[w].PRE_TIPO.Equals("UN_PESO"))
                        w++;

                    if (w < pref.Count)
                        un_peso = pref[w].PRE_VALOR;


                    carga = db.Carga.AsNoTracking()
                        .Where(x => x.CAR_ID == CargaId)
                        .Include(x => x.Transportadora)
                        .Include(x => x.TipoVeiculo)
                        .Include(x => x.Veiculo)
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
                        ViewData["msgErro"] = "A carga não exite.";
                        return new ViewAsPdf(carga, ViewData);
                    }

                    string sql = "SELECT " +
                        "VS.ORD_ID, " +
                        "VS.MOV_ENDERECO, " +
                        "COUNT(*)QTD_LOTES " +
                        "" +
                        "FROM T_ITENS_CARGA(NOLOCK) IT " +
                        "INNER JOIN T_ORDENS(NOLOCK) O ON IT.ORD_ID = O.ORD_ID " +
                        "LEFT JOIN(select PRO_ID, sum(DISPONIVEL) DISPONIVEL from V_ESTOQUE_PA WHERE DISPONIVEL > 0 GROUP BY PRO_ID ) as PA on PA.PRO_ID = O.PRO_ID " +
                        "INNER JOIN V_SALDO_ESTOQUE_POR_LOTE VS ON " +
                        "(VS.ORD_ID = IT.ORD_ID AND VS.PRO_ID = O.PRO_ID)  OR " +
                        "(VS.PRO_ID = O.PRO_ID AND  O.ORD_TIPO = 3 AND PA.DISPONIVEL > 0) " +
                        "" +
                        "WHERE IT.CAR_ID = '" + CargaId + "' " +
                        "GROUP BY VS.ORD_ID,VS.MOV_ENDERECO " +
                        "ORDER BY VS.ORD_ID, VS.MOV_ENDERECO";

                    List<object[]> lista_estoque = new List<object[]>();
                    object[] temp_obj;

                    using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                    {
                        try
                        {
                            command.CommandText = sql;
                            command.CommandType = CommandType.Text;

                            db.Database.OpenConnection();

                            using (DbDataReader result = command.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    temp_obj = new object[3];
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var valor = result.GetValue(i);
                                        temp_obj[i] = valor;
                                    }
                                    lista_estoque.Add(temp_obj);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    var cargasWeb = db.CargasWeb.AsNoTracking()
                        .Where(x => x.CAR_ID == CargaId)
                        .GroupBy(x => new { x.CLI_ID, x.CLI_NOME, x.PON_ID });

                    List<ExpandoObject> itensCargaWeb = new List<ExpandoObject>();
                    carga.CAR_PESO_TEORICO = 0;
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
                            CargasWeb dadosPedido = new CargasWeb();
                            dadosPedido.ORD_ID = item.ElementAt(i).ORD_ID;
                            dadosPedido.ITC_ORDEM_ENTREGA = item.ElementAt(i).ITC_ORDEM_ENTREGA;
                            dadosPedido.PRO_DESCRICAO = item.ElementAt(i).PRO_DESCRICAO;
                            dadosPedido.ORD_DATA_ENTREGA_DE = item.ElementAt(i).ORD_DATA_ENTREGA_DE;
                            dadosPedido.ITC_QTD_PLANEJADA = item.ElementAt(i).ITC_QTD_PLANEJADA;
                            dadosPedido.QTD_UE = item.ElementAt(i).QTD_UE;
                            dadosPedido.SALDO_ESTOQUE_UE = item.ElementAt(i).SALDO_ESTOQUE_UE;
                            dadosPedido.M3_PLANEJADO = (item.ElementAt(i).M3_PLANEJADO == null) ? 0 : item.ElementAt(i).M3_PLANEJADO;
                            somatorioM3Cliente += (item.ElementAt(i).M3_PLANEJADO == null) ? 0 : item.ElementAt(i).M3_PLANEJADO.Value;
                            somatorioPeso += (double)(item.ElementAt(i).ORD_PESO_UNITARIO * item.ElementAt(i).ORD_QUANTIDADE);
                            dadosPedido.PRO_LARGURA_EMBALADA = (item.ElementAt(i).PRO_LARGURA_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_LARGURA_EMBALADA;
                            dadosPedido.PRO_COMPRIMENTO_EMBALADA = (item.ElementAt(i).PRO_COMPRIMENTO_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_COMPRIMENTO_EMBALADA;
                            dadosPedido.PRO_ALTURA_EMBALADA = (item.ElementAt(i).PRO_ALTURA_EMBALADA == null) ? 0 : item.ElementAt(i).PRO_ALTURA_EMBALADA;
                            dadosPedido.ORD_PESO_UNITARIO = BaseController.ConversorPeso(Convert.ToDouble(item.ElementAt(i).ORD_PESO_UNITARIO), un_peso);
                            dadosPedido.ORD_QUANTIDADE = (item.ElementAt(i).ORD_QUANTIDADE);

                            pedidos.Add(dadosPedido);
                        }
                        cabecalhoItens.totalM3 = Math.Round(somatorioM3Cliente, 2);
                        cabecalhoItens.totalPeso = Math.Round(BaseController.ConversorPeso(somatorioPeso, un_peso), 2);
                        cabecalhoItens.pedidos = pedidos.OrderBy(x => x.ITC_ORDEM_ENTREGA);
                        itensCargaWeb.Add(cabecalhoItens);

                        carga.CAR_PESO_TEORICO += Math.Round(BaseController.ConversorPeso(somatorioPeso, un_peso), 2);
                    }
                    carga.CAR_PESO_EMBALAGEM = Math.Round(Convert.ToDouble(carga.CAR_PESO_EMBALAGEM), 2);

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

                    List<dynamic> lista = new List<dynamic>();
                    foreach (var item in lista_estoque)
                    {
                        dynamic objeto = new ExpandoObject();
                        objeto.ord_id = item[0];
                        objeto.endereco = "" + item[1];
                        objeto.qtd_lotes = "" + item[2];

                        lista.Add(objeto);
                    }
                    ViewData["viewdata"] = lista;
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