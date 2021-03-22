using DynamicForms.Areas.PlugAndPlay.Enums;
using DynamicForms.Areas.SGI.Model;
using DynamicForms.Areas.SGI.Utils;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P.Pager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace DynamicForms.Areas.SGI.Controllers
{
    [Authorize]
    [Area("sgi")]
    public class HomeController : BaseController
    {
        // https://localhost:44369/sgi/home/index
        public ActionResult Index(int? nPageSize, int? page, int? idGrupo, int? idUnidade, int? idNegocio, int? idDepartamento, string sltAno, string pGrafico, string search)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                T_Usuario usuario = ObterUsuarioLogado();

                //Controle Acesso
                if (!ValidacoesUsuario.ValidarAcessoTela(usuario, typeof(HomeController).FullName))
                    return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

                #region ViewDataLayout
                ViewData["mainmenu_scroll"] = "fixedscroll"; //pagescroll , fixedscroll
                ViewData["body_class"] = "sidebar-collapse";
                ViewData["html_class"] = "";
                ViewData["pagetopbar_class"] = "sidebar_shift";
                ViewData["maincontent_class"] = "sidebar_shift";
                ViewData["maincontent_type"] = "";
                ViewData["pagesidebar_class"] = "collapseit";
                ViewData["pagechatapi_class"] = "";
                ViewData["chatapiwindows_demo"] = "";
                ViewData["chatapiwindows_class"] = "";
                #endregion ViewDataLayout

                #region Variaveis
                Graficos grafico = new Graficos();
                #endregion Variaveis

                #region ViewBags

                List<T_Negocio> dbNegocios = db.T_Negocio.OrderBy(x => x.NEG_DESCRICAO).ToList();
                SelectList slIdNegocio = new SelectList(dbNegocios, "NEG_ID", "NEG_DESCRICAO", idNegocio);
                IEnumerable<SelectListItem> negocios = slIdNegocio as IEnumerable<SelectListItem>;
                ViewBag.idNegocio = negocios;

                List<T_Grupo> dbGrupo = db.T_Grupo.Where(x => x.EXIBELISTA == (int)YesNo.Sim).OrderBy(x => x.NOME).ToList();
                SelectList slIdGrupo = new SelectList(dbGrupo, "GRU_ID", "NOME", idGrupo);
                IEnumerable<SelectListItem> grupos = slIdGrupo as IEnumerable<SelectListItem>;
                ViewBag.idGrupo = grupos;

                List<T_Unidade> dbUnidades = db.T_Unidade.OrderBy(x => x.DEESCRICAO).ToList();
                SelectList slIdUnidade = new SelectList(dbUnidades, "UNI_ID", "UN", idUnidade);
                IEnumerable<SelectListItem> unidades = slIdUnidade as IEnumerable<SelectListItem>;
                ViewBag.idUnidade = unidades;

                List<T_Departamentos> dbDepartamentos = db.T_Departamentos.OrderBy(x => x.DEP_NOME).ToList();
                SelectList slIdDepartamento = new SelectList(dbDepartamentos, "DEP_ID", "DEP_NOME", idDepartamento);
                IEnumerable<SelectListItem> departamentos = slIdDepartamento as IEnumerable<SelectListItem>;
                ViewBag.idDepartamento = slIdDepartamento;

                var dbAnos = db.T_Medicoes.Select(x => new { Ano = x.MED_DATAMEDICAO.Substring(0, 4) }).ToList();
                var dbAnos2 = dbAnos.OrderByDescending(x => x.Ano).GroupBy(x => new { x.Ano }).ToList();
                SelectList slAnos = new SelectList(dbAnos2, "Key.Ano", "Key.Ano", sltAno);
                IEnumerable<SelectListItem> anos = slAnos as IEnumerable<SelectListItem>;
                ViewBag.pAno = anos;

                sltAno = (sltAno == "" || sltAno == null && dbAnos2.Count() > 0 ? dbAnos2.FirstOrDefault().Key.Ano : sltAno);
                ViewBag.anoAtual = sltAno;
                ViewBag.search = search;
                ViewBag.negAtual = idNegocio;
                ViewBag.grpAtual = idGrupo;
                ViewBag.unAtual = idUnidade;
                ViewBag.grafico = pGrafico ?? "G";
                ViewBag.departamentoAtu = idDepartamento;
                #endregion ViewBags

                //Busca favoritos
                grafico.Favoritos = db.T_favoritos.Where(x => x.USE_ID == usuario.USE_ID).ToList();
                //Chama metódo para trazer dados agrupados por indicadores e metas
                IPager<MedicoesInd> indicadores = ExtratorMedidor.GetIndicadores(nPageSize, page, idNegocio, idGrupo, idUnidade, idDepartamento, sltAno, search, grafico.Favoritos);

                foreach (var item in indicadores)
                {
                    item.Indicador.T_Negocio = db.T_Negocio.Where(x => x.NEG_ID == item.Indicador.NEG_ID).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(sltAno))
                {
                    //Chama metódo para poder buscar dados de medições
                    grafico.Medicoes = ExtratorMedidor.GetMedicoes(indicadores.ToList(), sltAno);
                    grafico.AnoAnterior = QueryAnaliser.AnoAnterior(sltAno);
                    string anoAnterior = grafico.AnoAnterior.Count > 0 ? grafico.AnoAnterior.FirstOrDefault().Mes.Substring(0, 4) : "";
                    ViewBag.anoAnterior = anoAnterior;
                }
                else
                {
                    grafico.Medicoes = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                    grafico.AnoAnterior = new List<vw_SGI_PARAMETRO_RELMEDICOES>();
                    ViewBag.anoAnterior = null;
                }

                //Busca lista de indicadores
                grafico.Indicadores = indicadores;
                if (grafico.Medicoes.Count > 0)
                {
                    //Busca informações complementares
                    List<T_Informacoes_Complementares> infComplementares = db.T_Informacoes_Complementares.AsNoTracking()
                                                                                    .Where(x => grafico.Medicoes.Any(j => j.MET_ID == x.MET_ID)).ToList();
                    grafico.Complementares = infComplementares;
                }
                //Busca planos de ações
                List<T_PlanoAcao> planos = db.T_PlanoAcao.AsNoTracking().Where(x => indicadores.Any(i => i.IND_ID == x.T_Metas.IND_ID)).ToList();
                grafico.PlanoAcoes = planos;
                string listaMetas = "";
                if (pGrafico == "M")
                {
                    List<V_PAINEL_LISTA_METAS> dbListaMetas = db.Query<V_PAINEL_LISTA_METAS>().FromSql("select * from V_PAINEL_LISTA_METAS ").ToList();
                    ViewBag.listaMetas = dbListaMetas;
                }

                return View(grafico);
            }
        }

        public ActionResult Detalhado(int idIndicador, int? nPageSize, int? page, string searchString)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var _PageNumber = page ?? 1;
                var _PageSize = nPageSize ?? 10;
                ViewBag.ItensPageSize = new List<int> { 5, 10, 25, 50, 100 };

                ///Busca registros no banco de dados
                string query = "SELECT object_id Id,Replace(Replace(UPPER(NAME),'VW_SGI_" + idIndicador.ToString() + "_',''),'_',' ') Nome FROM ";
                query += "(SELECT object_id,name FROM SYS.views ";
                query += "union ";
                query += "SELECT object_id,name FROM SYS.procedures) as p ";
                query += "WHERE UPPER(NAME) LIKE '%VW_SGI_" + idIndicador.ToString() + "_%' ";
                List<ViewsNome> views = new List<ViewsNome>();
                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    db.Database.OpenConnection();
                    command.CommandTimeout = 99999;
                    try
                    {
                        DbDataReader result = command.ExecuteReader();
                        while (result.Read())
                        {
                            views.Add(new ViewsNome
                            {
                                Id = (int)result.GetValue(result.GetOrdinal("Id")),
                                Nome = (string)result.GetValue(result.GetOrdinal("Nome"))
                            });
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao executar procedure: " + erro.Message);
                    }
                }

                //Filtro
                if (searchString != null && searchString != "")
                    views = views.Where(x => x.Nome.ToUpper().Contains(searchString.ToUpper())).ToList();

                return View(views.OrderBy(x => x.Nome).ToPagerList(_PageNumber, _PageSize));
            }
        }

        public ActionResult DetalhadoView(int id, int idIndicador, string data1, string data2)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var views = new ViewsNome();
                var dataAtual = DateTime.Now;
                ViewBag.data1 = String.IsNullOrEmpty(data1) ? new DateTime(dataAtual.Year, dataAtual.Month, 1).ToString() : data1;
                ViewBag.data2 = String.IsNullOrEmpty(data2) ? new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(1).AddDays(-1).ToString() : data2;
                string query = "SELECT Tipo, object_id Id,upper(NAME) Nome FROM ";
                query += "(SELECT 'view' Tipo,object_id,name FROM SYS.views ";
                query += "union ";
                query += "SELECT 'procedure' Tipo,object_id,name FROM SYS.procedures) as p ";
                query += "WHERE object_id = '" + id + "' ";


                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    db.Database.OpenConnection();
                    command.CommandTimeout = 99999;
                    try
                    {
                        DbDataReader result = command.ExecuteReader();
                        if (result.Read())
                        {
                            views.Id = (int)result.GetValue(result.GetOrdinal("Id"));
                            views.Nome = (string)result.GetValue(result.GetOrdinal("Nome"));
                            views.Tipo = (string)result.GetValue(result.GetOrdinal("Tipo"));
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao executar procedure: " + erro.Message);
                    }
                    finally
                    {
                        db.Database.CloseConnection();
                    }

                    views.Indicador = db.T_Indicadores.Find(idIndicador);

                    if (views.Tipo == "view")
                    {
                        List<ViewsCampos> campos = new List<ViewsCampos>();
                        query = "SELECT name Nome FROM sys.columns where object_id = " + id.ToString();
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        db.Database.OpenConnection();
                        command.CommandTimeout = 99999;
                        try
                        {
                            DbDataReader result = command.ExecuteReader();
                            while (result.Read())
                            {
                                campos.Add(new ViewsCampos { Nome = (string)result.GetValue(result.GetOrdinal("Nome")) });
                            }

                            views.Campos = campos;
                        }
                        catch (Exception erro)
                        {
                            throw new Exception("Erro ao executar query: " + erro.Message);
                        }
                        finally
                        {
                            db.Database.CloseConnection();
                        }
                    }
                    else
                    {
                        views.Campos = QueryAnaliser.GetCamposProcedure(views.Nome);
                    }

                    views.Valores = new string[0, 0];
                    if ((data1 != "" && data1 != null) && (data2 != "" && data2 != null))
                        views.Valores = QueryAnaliser.GetValores(views.Tipo, views.Nome, data1, data2);
                    views.Nome = views.Nome.Replace("VW_SGI_" + idIndicador.ToString() + "_", "").Replace("_", " ");
                    //select list itens
                    return View(views);
                }
            }
        }

        #region MetodosJson
        public JsonResult GetFatosPeriodos(int dimensao, int indicador)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var periodos = db.T_Medicoes
                                        .Where(x => x.IND_ID == indicador && x.DIM_ID == dimensao.ToString() &&
                                                x.PER_ID.Trim().ToUpper() != "MAC" && x.PER_ID.Trim().ToUpper() != "DAC")
                                        .Select(x => new { id = x.PER_ID, descricao = x.PER_DESCRICAO }).Distinct().ToList();
                return Json(new { periodos });
            }
        }
        public JsonResult GetPeriodos(int indicador, int dimensao)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var periodos = db.T_Medicoes.Where(x => x.IND_ID == indicador && x.DIM_ID == dimensao.ToString() &&
                                                    x.PER_ID.Trim().ToUpper() != "MAC" && x.PER_ID.Trim().ToUpper() != "DAC")
                                             .Select(x => new { id = x.PER_ID, descricao = x.PER_DESCRICAO }).Distinct().ToList();
                return Json(new { periodos });
            }
        }
        public JsonResult GetGrafico(int idInd, int tipoGrafico, string ano, int dimensao, string periodo, string strDataIni, string strDataFim, string subDime)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                // testar o tipo do grafico 
                // testar se a medicao possui sub dimencoes caso sim  o grafico deve ser mostrado em pizza onde todos os fotos+ subdimencoes sao agrupadas na pizza continua explicacao em baixo 
                // em caso de grafico de linha   o eixo x sera preenchido com os fatos e as linhas serao as subdimencoes
                // neste caso o filtro sempre sera por data. 

                DateTime dataDe = DateTime.Now;
                DateTime dataAte = DateTime.Now;
                DateTime dataAux = DateTime.Now;

                if (periodo != null)
                {
                    periodo = periodo.Trim();
                }

                if ((string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(strDataIni) && !string.IsNullOrEmpty(strDataFim)) || (periodo == "D"))
                {
                    dataDe = DateTime.ParseExact(strDataIni, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dataAte = DateTime.ParseExact(strDataFim, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dataAux = dataAte;
                }

                string query = "";
                if (periodo == "M")
                {
                    query = @"select * from vw_SGI_PARAMETRO_RELMEDICOES 
                        WHERE IND_ID = '" + idInd.ToString() + "' AND DimId = '" + dimensao + "' AND LEFT(PerId, 1) ='" + periodo + "'";
                    //Filtra ano atual
                    if (ano != "" && ano != null)
                    {
                        query += " AND ((LEFT(MES,4) = '" + (Convert.ToInt32(ano) - 1) + "' and PerId in ('MAC', 'DAC' )) or LEFT(MES,4) = '" + ano + "')";
                    }
                    if (subDime != null && subDime != "null" && subDime.Trim() != "")
                    {
                        query += " AND DimSubId = '" + subDime + "' ";
                    }
                    query += "order by IND_ID,Mes";
                }
                else if (periodo == "D")
                {
                    query = string.Format(@"select * 
                        from vw_SGI_PARAMETRO_RELMEDICOES 
                        WHERE IND_ID = {0} AND MED_DATA_MEDICAO between {1} and {2} 
                        AND LEFT(PerId, 1) = 'D' AND DimId = '{3}' "
                            , idInd.ToString()
                            , dataDe.ToString("yyyyMMdd")
                            , dataAte.ToString("yyyyMMdd")
                            , dimensao);
                    if (subDime != null && subDime != "null" && subDime.Trim() != "")
                    {
                        query += " AND DimSubId = '" + subDime + "' ";
                    }
                    query += " order by MED_DATA_MEDICAO ";
                }
                else if (string.IsNullOrEmpty(periodo) && !string.IsNullOrEmpty(strDataIni) && !string.IsNullOrEmpty(strDataFim))
                {
                    query = string.Format(@"select * 
                        from vw_SGI_PARAMETRO_RELMEDICOES 
                        WHERE IND_ID = {0} AND MED_DATA_MEDICAO between {1} and {2} 
                        AND LEFT(PerId, 1) = 'D' AND DimId = '{3}' "
                           , idInd.ToString()
                           , dataDe.ToString("yyyyMMdd")
                           , dataAte.ToString("yyyyMMdd")
                           , dimensao);
                    if (subDime != "null" && subDime.Trim() != "")
                    {
                        query += " AND DimSubId = '" + subDime + "' ";
                    }
                    query += " order by MED_DATA_MEDICAO ";
                }


                Graficos grafico = new Graficos();
                List<vw_SGI_PARAMETRO_RELMEDICOES> medicao = db.VW_SGI_PARAMETRO_RELMEDICOES.FromSql(query).ToList();
                List<object> fatos = new List<object>();
                List<object> dados = new List<object>();
                List<string> cabecalho = new List<string>();
                List<string> legenda = new List<string>();
                string temDimSubId = "";

                var fatosId = medicao.Select(x => new { x.FatId, x.FatDescricao }).Distinct().ToList();
                var subDimensao = medicao.Select(x => new { x.DimSubId, x.DimSubDescricao }).Distinct().ToList();

                if (medicao.Count > 0)
                {
                    fatosId = medicao.Select(x => new { x.FatId, x.FatDescricao }).Distinct().ToList();
                    subDimensao = medicao.Select(x => new { x.DimSubId, x.DimSubDescricao }).Distinct().ToList();

                    T_Indicadores indicador = db.T_Indicadores.Find(idInd);
                    indicador.DIM_ID = dimensao;

                    indicador.PER_ID = (periodo == null ? "" : periodo);
                    indicador.IND_GRAFICO = tipoGrafico;
                    string tipoCompMeta = indicador.IND_TIPOCOMPARADOR.ToString();
                    db.SaveChanges();//Salva mudanças
                    if (!string.IsNullOrEmpty(periodo))
                    {
                        if (!string.IsNullOrEmpty(subDimensao[0].DimSubId)) // teste se tem sub dimentcao 
                        {
                            int teste = 0;
                            try
                            {
                                temDimSubId = "T";
                                foreach (var s in subDimensao)
                                {
                                    List<object> medicoesGrafico = new List<object>();
                                    vw_SGI_PARAMETRO_RELMEDICOES med = null;


                                    foreach (var f in fatosId)
                                    {
                                        teste++;
                                        var medicaoFato = medicao.Where(x => x.FatId.Trim() == f.FatId.Trim() &&
                                        x.DimSubId.Trim() == s.DimSubId.Trim() && x.PerId.Trim() == periodo.Trim()).GroupBy(g => new
                                        { g.DimSubId, g.FatId, g.FatDescricao }).Select(g => new
                                        {
                                            subDesc = g.Key.DimSubId,
                                            name = g.Key.FatDescricao,
                                            value = g.Sum(m => m.Valor)
                                        }).FirstOrDefault();


                                        if (medicaoFato != null)
                                        {
                                            medicoesGrafico.Add(new
                                            {
                                                ano = "",
                                                mes = "",
                                                un = "",
                                                meta = "",
                                                valor = medicaoFato.value,
                                                tipo = 0,
                                                dimId = 0,
                                                dimDescricao = "",
                                                fatId = f.FatId,
                                                fatDescricao = f.FatDescricao,
                                                perId = "D",
                                                perDescricao = "Dia",
                                                dimSubId = s.DimSubId,
                                                dimSubDescricao = s.DimSubDescricao
                                            });
                                        }
                                        else
                                        {
                                            medicoesGrafico.Add(new
                                            {
                                                ano = "",
                                                mes = "",
                                                un = "",
                                                meta = "",
                                                valor = "",
                                                ac = 0,
                                                tipo = 0,
                                                data = "",
                                                dimId = 0,
                                                dimDescricao = "",
                                                fatId = "",
                                                fatDescricao = "",
                                                perId = "",
                                                perDescricao = "",
                                                medId = "",
                                                dimSubId = ""
                                            });
                                        }
                                    }
                                    fatos.Add(medicoesGrafico);
                                }
                            }
                            catch (Exception e)
                            {

                                throw;
                            }

                        }
                        else
                        {
                            foreach (var f in fatosId)
                            {
                                List<vw_SGI_PARAMETRO_RELMEDICOES> medicaoFato = medicao.Where(x => x.FatId == f.FatId).ToList();
                                List<object> medicoesGrafico = new List<object>();
                                vw_SGI_PARAMETRO_RELMEDICOES med = null;
                                if (periodo == "M")
                                {
                                    for (int i = 0; i < 14; i++)
                                    {
                                        string mes;
                                        if (i + 1 < 10)
                                            mes = "0" + (i + 1);
                                        else
                                            mes = (i + 1).ToString();
                                        if (i == 12)
                                        {
                                            med = medicaoFato.Where(x => x.PerId.Trim() == "MAC" && x.Mes.Substring(0, 4) == ano).FirstOrDefault();
                                        }
                                        else if (i == 13)
                                        {
                                            med = medicaoFato.Where(x => x.PerId.Trim() == "MAC" && x.Mes.Substring(0, 4) == (Convert.ToInt32(ano) - 1).ToString()).FirstOrDefault();
                                        }
                                        else
                                        {
                                            med = medicaoFato.Where(x => x.Mes.Substring(4, 2) == mes && x.PerId.Trim() == "M").FirstOrDefault();

                                        }
                                        if (med != null)
                                        {
                                            medicoesGrafico.Add(new
                                            {
                                                ano = med.Mes.Substring(0, 4),
                                                mes = med.Mes.Substring(4, 2),
                                                un = med.UNID,
                                                meta = med.META,
                                                valor = med.Valor,
                                                tipo = tipoCompMeta,
                                                dimId = med.DimId,
                                                dimDescricao = med.DimDescricao,
                                                fatId = med.FatId,
                                                fatDescricao = med.FatDescricao,
                                                perId = med.PerId,
                                                perDescricao = med.DimDescricao,
                                                dimSubId = med.DimSubId,
                                                dimSubDescricao = med.DimSubDescricao
                                            });
                                        }
                                        else
                                        {

                                            medicoesGrafico.Add(new
                                            {
                                                ano = "",
                                                mes = "",
                                                un = "",
                                                meta = "",
                                                valor = "",
                                                ac = 0,
                                                tipo = 0,
                                                data = "",
                                                dimId = 0,
                                                dimDescricao = "",
                                                fatId = "",
                                                fatDescricao = "",
                                                perId = "",
                                                perDescricao = "",
                                                medId = "",
                                                dimSubId = ""
                                            });
                                        }
                                    }
                                }
                                else if (periodo == "D")
                                {
                                    dataAux = dataDe;
                                    while (dataAte >= dataAux)
                                    {
                                        cabecalho.Add(dataAux.Day.ToString());

                                        med = medicaoFato.Where(x => x.MED_DATA.Date == dataAux.Date && x.PerId.Trim().ToUpper() == "D").FirstOrDefault();
                                        if (med != null)
                                        {
                                            medicoesGrafico.Add(new
                                            {
                                                ano = med.Mes.Substring(4, 2),
                                                mes = med.Mes.Substring(0, 4),
                                                un = med.UNID,
                                                meta = med.META,
                                                valor = med.Valor,
                                                tipo = tipoCompMeta,
                                                data = med.MED_DATA.ToShortTimeString(),
                                                dimId = med.DimId,
                                                dimDescricao = med.DimDescricao,
                                                fatId = med.FatId,
                                                fatDescricao = med.FatDescricao,
                                                perId = med.PerId,
                                                perDescricao = med.DimDescricao,
                                                dimSubId = med.DimSubId,
                                                dimSubDescricao = med.DimSubDescricao
                                            });
                                        }
                                        else
                                        {
                                            medicoesGrafico.Add(new
                                            {
                                                ano = "",
                                                mes = "",
                                                un = "",
                                                meta = "",
                                                valor = "",
                                                ac = 0,
                                                tipo = 0,
                                                data = "",
                                                dimId = 0,
                                                dimDescricao = "",
                                                fatId = "",
                                                fatDescricao = "",
                                                perId = "",
                                                perDescricao = "",
                                                medId = "",
                                                dimSubId = ""
                                            });
                                        }
                                        dataAux = dataAux.AddDays(1);
                                    }
                                }
                                fatos.Add(medicoesGrafico);
                            }
                        }
                    }
                    else
                    {
                        var g1 = medicao.GroupBy(g => new { g.DimSubId, g.FatId, g.FatDescricao }).Select(g => new
                        {
                            subDesc = g.Key.DimSubId,
                            name = g.Key.FatDescricao,
                            value = g.Sum(m => m.Valor)
                        }).ToList();

                        var g2 = medicao.GroupBy(g => new { g.DimSubId }).Select(g => new
                        {
                            subDesc = g.Key.DimSubId,
                            value = g.Sum(m => m.Valor)
                        }).ToList();

                        foreach (var dt in g2)
                        {
                            foreach (var d in g1)
                            {
                                if (d.subDesc == dt.subDesc)
                                {
                                    float percentual = (float)((d.value / dt.value) * 100);
                                    dados.Add(new { d.subDesc, d.name, d.value, percent = Math.Round(percentual, 2) });
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(subDimensao[0].DimSubId)) // teste se tem sub dimentcao 
                        {
                            temDimSubId = "T";
                        }
                    }

                    //monta o cabeçalio que é a escala do grafico
                    if (temDimSubId == "T")
                    {
                        foreach (var f in fatosId)
                        {
                            cabecalho.Add(f.FatDescricao.ToString().Trim());
                        }
                        // legenda 
                        if (tipoGrafico == 3)// pizzaa
                        {
                            var temp = medicao.Select(x => new { x.FatDescricao, x.DimSubId }).Distinct().ToList();
                            foreach (var t in temp)
                            {
                                legenda.Add(t.FatDescricao.ToString().Trim() + " - " + t.DimSubId.ToString().Trim());
                            }
                        }
                        else
                        {
                            var temp = medicao.Select(x => new { x.DimSubId }).Distinct().ToList();
                            foreach (var t in temp)
                            {
                                legenda.Add(t.DimSubId.ToString().Trim());
                            }
                        }
                    }
                    else
                    {
                        var temp = medicao.Select(x => new { x.FatDescricao }).Distinct().ToList();
                        foreach (var t in temp)
                        {
                            legenda.Add(t.FatDescricao.ToString().Trim());
                        }

                        if (periodo == "M")
                        {
                            cabecalho.Add("01");
                            cabecalho.Add("02");
                            cabecalho.Add("03");
                            cabecalho.Add("04");
                            cabecalho.Add("05");
                            cabecalho.Add("06");
                            cabecalho.Add("07");
                            cabecalho.Add("08");
                            cabecalho.Add("09");
                            cabecalho.Add("10");
                            cabecalho.Add("11");
                            cabecalho.Add("12");
                            cabecalho.Add("AC");
                            cabecalho.Add("AN");
                        }
                        else if (periodo == "D")
                        {
                            dataAux = dataDe;
                            while (dataAte >= dataAux)
                            {
                                cabecalho.Add(dataAux.Day.ToString());
                                dataAux = dataAux.AddDays(1);
                            }
                        }
                    }
                }
                // fatos = serie                              cabecalho = eicho X xAxis
                return this.Json(new { fatos, dados, legenda, cabecalho, temDimSubId, subDimensao });
            }
        }
        public JsonResult GetDtMedicao(int idInd, string ano)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string dtMedicao = "";

                if (ano != "" && ano != null)
                {
                    dtMedicao = db.VW_SGI_PARAMETRO_RELMEDICOES.AsNoTracking()
                                        .Where(x => x.IND_ID == idInd && x.Mes.StartsWith(ano)).Max(x => x.MED_DATA).ToString();
                }
                else
                {
                    dtMedicao = db.VW_SGI_PARAMETRO_RELMEDICOES.AsNoTracking().Where(x => x.IND_ID == idInd).Max(x => x.MED_DATA).ToString();
                }

                return Json(new { dtMedicao });

            }
        }
        public JsonResult AtuDados(int idInd, string ano)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
                {
                    int mesAtual = DateTime.Now.Month;
                    string data = $"{ano}{mesAtual}01";

                    command.CommandText = @"EXEC SP_SGI_EXTRATOR @data,@indicador";
                    command.CommandType = CommandType.Text;

                    DbParameter param = command.CreateParameter();
                    param.ParameterName = "@data";
                    param.Value = data;
                    command.Parameters.Add(param);

                    param = command.CreateParameter();
                    param.ParameterName = "@indicador";
                    param.Value = idInd;
                    command.Parameters.Add(param);

                    db.Database.OpenConnection();

                    try
                    {
                        command.CommandTimeout = 99999;
                        DbDataReader reader = command.ExecuteReader();
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("Erro ao executar procedure: " + erro.Message);
                    }
                    finally
                    {
                        db.Database.CloseConnection();
                    }
                    return Json(new { sucess = true });
                }
            }
        }
        public JsonResult AddFavoritos(int idIndicador)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                T_Favoritos favorito = new T_Favoritos();
                string status = "incluir";
                if (db.T_favoritos.Count(x => x.ID_INDICADOR == idIndicador) > 0)
                {
                    favorito = db.T_favoritos.First(x => x.ID_INDICADOR == idIndicador);
                    db.T_favoritos.Remove(favorito);
                    db.SaveChanges();
                    status = "remover";
                }
                else
                {
                    int userId = ObterUsuarioLogado().USE_ID;
                    favorito.USE_ID = db.T_Usuario.First(x => x.USE_ID == userId).USE_ID;
                    favorito.ID_INDICADOR = idIndicador;
                    db.T_favoritos.Add(favorito);
                    db.SaveChanges();
                }
                return Json(new { status });
            }
        }
        #endregion MetodosJson
    }
}