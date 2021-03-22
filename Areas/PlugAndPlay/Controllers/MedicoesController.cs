using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OsmSharp.Tags;

namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class MedicoesController : BaseController
    {
        #region Feedbacks de producao
        // GET: plugandplay/medicoes/index
        public IActionResult Index(string id, string ip, string idEquipe)
        {
            List<string> lista = new List<string>();

            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;

            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(user, typeof(MedicoesController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                db.Order.Where(o => lista.Any(l => l.StartsWith(o.ORD_ID))).ToList();

                ViewBag.HabilitarConsultaPainel = db.Param.Any(x => x.PAR_ID.Equals("CONSULTAR_OP_PAINEL_MAQUINAS") && x.PAR_VALOR_S.Equals("S"));
                if (!string.IsNullOrEmpty(idEquipe))
                {
                    ViewBag.maquinaId = id;
                    ViewBag.maquinaIp = ip;
                    ViewBag.equipeId = idEquipe;
                    ViewBag.Maquina = db.Maquina.Find(id);
                }
                else if (!string.IsNullOrWhiteSpace(id))
                {
                    UtilPlay.SetPrimeiraProducao(id);
                    ViewBag.maquinaId = id;
                    ViewBag.maquinaIp = ip;
                    ViewBag.equipeId = idEquipe;
                    ViewBag.Maquina = db.Maquina.Find(id);
                }
                else
                {
                    ViewBag.Error = "O código da máquina não foi encontrado!";
                    return View("~/Views/Shared/ErrorPlay.cshtml");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult ObterMedicoes(string maquinaId)
        {
            ActionResult retorno = null;
            Maquina maquina = new Maquina();
            object filaProducao = new List<object>();
            object motivosPadada = new List<object>();
            object motivosProduzindo = new List<object>();
            object turnos = new List<object>();
            object turmas = new List<object>();
            List<ViewClpMedicoes> medicoes = new List<ViewClpMedicoes>();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    maquina = db.Maquina.AsNoTracking()
                        .Where(x => x.MAQ_ID == maquinaId).FirstOrDefault();

                    filaProducao = db.ViewFilaProducao.AsNoTracking()
                        .Where(f => f.RotMaqId == maquinaId)
                        .Select(x => new
                        {
                            opId = x.OrdId,
                            segTransformacao = x.RotSeqTransformacao,
                            seqRepeticao = x.FprSeqRepeticao,
                            proId = x.PaProId,
                            pecasPulso = x.RotQuantPecasPulso
                        }).ToList();

                    motivosPadada = db.Ocorrencia.AsNoTracking() // PENDENCIA
                    .Where(m => (m.TIP_ID == 1 || m.TIP_ID == 2) && (m.SPR != 1 && m.OCO_ID != "1.8" && m.OCO_ID != "1.2"))
                    .Select(x => new
                    {
                        Id = x.OCO_ID,
                        Descricao = x.OCO_DESCRICAO,
                        Tipo = x.TIP_ID
                    }).ToList();

                    IEnumerable<string> ocorrenciasProducao = new List<string> { "1.1", "1.9", "2.9" };
                    motivosProduzindo = db.Ocorrencia.AsNoTracking() // PENDENCIA
                    .Where(m => (m.TIP_ID == 5 && m.SPR != 1) || ocorrenciasProducao.Contains(m.OCO_ID))
                    .Select(x => new
                    {
                        Id = x.OCO_ID,
                        Descricao = x.OCO_DESCRICAO,
                        Tipo = x.TIP_ID
                    }).ToList();

                    turnos = db.Turno.AsNoTracking()
                        .Select(t => new Turno { Id = t.Id, Descricao = t.Descricao }).ToList();
                    turmas = db.Turma.AsNoTracking()
                        .Select(t => new Turma { Id = t.Id, Descricao = t.Descricao }).ToList();

                    medicoes = db.ViewClpMedicoes
                          .AsNoTracking()
                          .Where(x => x.MaquinaId == maquinaId && x.FeedBackIdMov == -1)
                          .OrderBy(x => x.Grupo).ToList();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro----->>>" + ex.Message);
                    if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                        Console.WriteLine("Erro IEM----->>>" + ex.InnerException.Message);
                    retorno = StatusCode(500);
                }
            }

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            retorno = Json(new
            {
                maquina,
                medicoes = ConvertToAjaxMedicoes(medicoes),
                fila = filaProducao,
                motivosPadada,
                motivosProduzindo,
                turnos = turnos,
                turmas = turmas
            }, settingsJSON);
            return retorno;
        }

        [HttpPost]
        public ActionResult ObterMedicoesTempoReal(string maquinaId, double ultimoGrupo)
        {
            List<ViewClpMedicoes> medicoes;
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                medicoes = db.ViewClpMedicoes.AsNoTracking()
                    .Where(x => x.MaquinaId == maquinaId && x.Grupo >= ultimoGrupo)
                    .OrderBy(x => x.Grupo).ToList();
            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new
            {
                medicoes = ConvertToAjaxMedicoes(medicoes)
            }, settingsJSON);
        }

        [HttpPost]
        public ActionResult GravarMedicoes(string medicaoJson)
        {
            List<string> msg = new List<string>();
            bool ok = true;
            Feedback medicao = JsonConvert.DeserializeObject<Feedback>(medicaoJson);
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    T_Usuario user = ObterUsuarioLogado();
                    int idUser = user.USE_ID;
                    medicao.UsuarioId = idUser;

                    //ViewClpMedicoes antMedicao = db.ViewClpMedicoes.AsNoTracking()
                    //    .Where(x => x.MaquinaId == medicao.MaquinaId && x.Grupo < medicao.Grupo)
                    //    .OrderByDescending(x => x.Grupo).FirstOrDefault();

                    ViewClpMedicoes antMedicao = db.ViewClpMedicoes.FromSql("SELECT top 1 * FROM V_CLP_MEDICOES WHERE MAQUINA_ID = {0} and" +
                        " GRUPO < {1} order by GRUPO desc ", medicao.MaquinaId, medicao.Grupo)
                     .AsNoTracking().FirstOrDefault();

                    //validar se o turno nao é nulo para em feedbacks com motivos do tipo 1 ou 5
                    int grupo = db.Ocorrencia.AsNoTracking() // PENDENCIA
                        .Where(t => t.OCO_ID == medicao.OcorrenciaId).Select(t => t.TIP_ID)
                        .FirstOrDefault();

                    if ((grupo == 1 || grupo == 5) && (medicao.TurnoId == null || medicao.TurmaId == null))
                    {
                        ok = false;
                        msg.Add("Selecione a turma e o turno.");
                    }
                    if (antMedicao != null)
                    {
                        if (antMedicao.FeedBackId == null)
                        {
                            msg.Add("Não é possível salvar este feedback sem salvar os feedbacks anteriores.");
                            ok = false;
                        }
                    }
                    if (ok)
                    {
                        //if (medicao.Id == 0 && db.Feedback.AsNoTracking().Count(f => f.MaquinaId == medicao.MaquinaId && f.Grupo == medicao.Grupo) == 0 || medicao.Id == 0)
                        if (db.Feedback.AsNoTracking().Where(f => f.MaquinaId == medicao.MaquinaId && f.Grupo == medicao.Grupo).Count() == 0)
                        {
                            db.Entry(medicao).State = EntityState.Added;
                        }
                        else
                            db.Entry(medicao).State = EntityState.Modified;

                        if (db.SaveChanges() < 1)
                            ok = false;
                    }
                }
                catch (Exception e)
                {
                    ok = false;
                    msg.Add("Ocorreu um erro ao salvar o feedback.\n" + e.Message);
                }
            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new
            {
                ok = ok,
                id = medicao.Id,
                msg = msg
            }, settingsJSON);
        }

        [HttpPost]
        public ActionResult CancelarFeedback(int medId, string maquina, double grupo)
        {
            bool ok = true;
            List<string> msgError = new List<string>();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Feedback medGravada = db.Feedback.Find(medId);
                        if (medId == 0 || medGravada == null)
                        {
                            ok = false;
                            msgError.Add("Não é possível excluir um feedback que ainda não foi salvo.");
                        }
                        else if (db.ViewFeedback.AsNoTracking()
                            .Where(x => x.MaquinaId == maquina && x.Grupo > grupo).Count() > 0)
                        {
                            ok = false;
                            msgError.Add("Os feedbacks devem ser excluidos na mesma sequência que foram salvos.");
                        }
                        int resp;
                        if (ok)
                        {
                            resp = db.Database.ExecuteSqlCommand(@"UPDATE T_CLP_MEDICOES SET STATUS = 0 
                                                            WHERE ID IN(SELECT ID FROM T_CLP_MEDICOES (NOLOCK) 
                                                                     WHERE MAQUINA_ID = {0} AND GRUPO = {1})",
                                                        maquina, grupo);
                            //bd.Feedback.Remove(medGravada); 
                            resp = db.Database.ExecuteSqlCommand(@"DELETE FROM T_FEEDBACK WHERE FEE_ID={0} AND FEE_GRUPO={1}", medId, grupo);
                            // Excluindo apontamento manual
                            resp = db.Database.ExecuteSqlCommand(@"DELETE FROM T_CLP_MEDICOES WHERE MAQUINA_ID={0} AND GRUPO={1} AND CLP_ORIGEM='M'",
                                                   maquina, grupo);

                            ///*--------------------- ATUALIZA AS INFORAMACOES PARA O PAINEL DE MONITORAMENTO ---------------------------*/
                            //Maquina maq = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID == maquina).FirstOrDefault();
                            //if (maq != null && maq.MAQ_TIPO_CONTADOR == 3)
                            //{

                            //    /* 30/10/2020
                            //     * Desativei a execução da procedure para os apontamento de pulsos manuais.
                            //     * Essa procedure foi implementada no PlaySchedule, e não terá mais utilidade nos 
                            //     * apontamentos de pulso manuais.
                            //     * 
                            //     * Thiago Luz.
                            //     */

                            //    //Executar Procedure que atualiza o painel de monitoramento caso a máquina seja de apontamento manual
                            //    float proximaMetaPerformance = -1;

                            //    //string sql = "EXEC SP_PLUG_CALCULA_PERFORMANCE_FILA -1, '" + maquina + "','','',0,'','',0," + proximaMetaPerformance.ToString() + ", '','','',''";
                            //    string sql = $"EXEC SP_PLUG_CALCULA_PERFORMANCE_FILA -1, '{maquina}', '', '', 0, '', '', 0, {proximaMetaPerformance}, '', '', '', ''";
                            //    resp = db.Database.ExecuteSqlCommand(sql);
                            //}
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        ok = false;
                        transaction.Rollback();
                        msgError.Add($"Exception try catch {e.Message}");
                    }
                }
            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new { ok = ok, msg = msgError }, settingsJSON);
        }

        [HttpPost]
        public ActionResult DesfazerMedicao(int medId, string maquina, double grupo, double quantidade)
        {
            bool ok = true;
            grupo = Math.Round((double)grupo, 1);
            double grupoInteiro = Math.Floor(grupo);
            double aux = Math.Round(0.1, 1);
            List<string> msgError = new List<string>();

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    //Padrao
                    if (grupoInteiro == grupo)
                    {
                        ok = false;
                        msgError.Add("Não é possível desfazer um período que não foi modificado.");
                    }

                    if (ok && db.ViewFeedback.AsNoTracking()
                        .Count(f => f.MaquinaId == maquina && f.Grupo >= grupoInteiro && f.Grupo <= grupoInteiro + 0.999) > 0)
                    {
                        ok = false;
                        msgError.Add("Remova o feedback antes de desfazer a divisão do periodo.");
                    }
                    //var opAtual = bd.VwFilaProducao.Where(f => f.RotMaqId == maquina).OrderBy(f => f.FprDataInicioPrevista).Take(1).FirstOrDefault();
                    if (ok && db.ViewFeedback.AsNoTracking()
                        .Count(f => Math.Floor(f.Grupo) == grupoInteiro && f.MaquinaId == maquina && f.FeeIdMovEstoque != null) > 0)
                    {
                        ok = false;
                        msgError.Add("Não é possível desfazer este período sem desfazer a OP produzida anteriormente.");
                    }
                    //---
                    if (ok && quantidade > 0 && Math.Floor(db.ClpMedicoes.AsNoTracking().Where(m => m.MaquinaId == maquina &&
                        Math.Floor((double)m.Grupo) == grupoInteiro).Max(mm => mm.Grupo).Value) > grupo)
                    {
                        ok = false;
                        msgError.Add("Estorne o ultimo registro dividido primeiro");
                    }
                    //----
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (ok && quantidade > 0)
                            {
                                //----
                                //Item selecionado 
                                ClpMedicoes itemEst = db.ClpMedicoes.AsNoTracking()
                                    .Where(m => m.MaquinaId == maquina && m.Grupo == grupo).FirstOrDefault();
                                //Atualizando Quantidade e Data
                                if (Math.Round((double)(grupo - Math.Truncate(grupo)), 1) == (aux + aux))
                                {
                                    //Item Anterior
                                    ClpMedicoes itemAnt = db.ClpMedicoes.AsNoTracking()
                                        .Where(m => m.MaquinaId == maquina && m.Grupo == Math.Round((double)(grupo - (aux)), 1)).FirstOrDefault();
                                    db.ClpMedicoes.Attach(itemEst);
                                    db.ClpMedicoes.Attach(itemAnt);
                                    db.Entry(itemEst).State = EntityState.Deleted;
                                    db.Entry(itemAnt).State = EntityState.Deleted;
                                    string sql = $"UPDATE T_CLP_MEDICOES SET STATUS ='{0}' , GRUPO='{grupoInteiro}' WHERE ID IN (SELECT ID FROM T_CLP_MEDICOES (NOLOCK) " +
                                                 $"WHERE MAQUINA_ID ='{maquina}' AND GRUPO ='{grupoInteiro}.{999}')";

                                    db.Database.ExecuteSqlCommand(sql);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //Atualizando linha anterior
                                    ClpMedicoes itemAnt = db.ClpMedicoes.AsNoTracking()
                                        .Where(m => m.MaquinaId == maquina && m.Grupo == Math.Round((double)(grupo - (aux)), 1)).FirstOrDefault();
                                    itemAnt.Quantidade += itemEst.Quantidade;
                                    itemAnt.DataFim = itemEst.DataFim;

                                    db.ClpMedicoes.Attach(itemAnt);
                                    db.Entry(itemAnt).State = EntityState.Modified;
                                    db.SaveChanges();
                                    db.ClpMedicoes.Attach(itemEst);
                                    db.Entry(itemEst).State = EntityState.Deleted;
                                    db.SaveChanges();
                                }
                            }
                            if (ok && quantidade == 0)
                            {
                                string sql = $"DELETE FROM T_CLP_MEDICOES WHERE  MAQUINA_ID ='{ maquina }' AND GRUPO > {grupoInteiro}  AND GRUPO< {(grupoInteiro) + +0.999 };" +
                                             $"UPDATE T_CLP_MEDICOES set GRUPO = {(grupoInteiro)}, STATUS = 0  WHERE MAQUINA_ID ='{ maquina }' AND  GRUPO BETWEEN {grupoInteiro} AND {grupoInteiro + +0.999};";
                                db.Database.ExecuteSqlCommand(sql);

                            }
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            ok = false;
                            transaction.Rollback();
                            msgError.Add("Ocorreu um erro ao desfazer o feedback");
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgError.Add(ex.Message);
                }

            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new
            {
                ok = ok,
                msg = msgError
            }, settingsJSON);
        }

        [HttpPost]
        public IActionResult VerificarSeEhUltimoGrupo(string medicaoJson)
        {
            ClpMedicoes medicao = JsonConvert.DeserializeObject<ClpMedicoes>(medicaoJson);
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                double? ultimoGrupo = db.ClpMedicoes.AsNoTracking()
                    .Where(m => m.MaquinaId == medicao.MaquinaId).Max(mm => mm.Grupo);

                if (medicao.Grupo == ultimoGrupo)
                {
                    return Json(true);
                }
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult DividirPeriodo(string medicoes)
        {
            List<ClpMedicoes> med = JsonConvert.DeserializeObject<List<ClpMedicoes>>(medicoes);
            List<string> msg = new List<string>();
            bool ok = true;
            double? grupo = med[0].Grupo;
            string maquina = med[0].MaquinaId;
            double aux = 0.1;

            for (int i = 0; i < med.Count; i++)
            {
                if (med[i].DataFim <= med[i].DataInicio)
                    ok = false;
            }

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var medicao = db.ViewClpMedicoes.AsNoTracking().Where(m => m.MaquinaId == maquina && m.Grupo == grupo)
                    .Select(m => new { m.Quantidade, m.Grupo }).FirstOrDefault();
                if (medicao.Quantidade > 0)
                {
                    ok = false;
                    msg.Add("Não é permitido dividir um periodo com quantidade maior que 0.");
                }
                if (Math.Floor((double)medicao.Grupo) != grupo)
                {
                    ok = false;
                    msg.Add("Não é permitido dividir um periodo dividido anteriormente.");
                }
                if (db.ViewClpMedicoes.AsNoTracking().Count(m => m.MaquinaId == maquina && m.Grupo > grupo) == 0)
                {
                    ok = false;
                    msg.Add("Aguarde o encerramento do periodo.");
                }
                if (db.ViewFeedback.AsNoTracking().Count(f => f.MaquinaId == maquina && f.Grupo == grupo) > 0)
                {
                    ok = false;
                    msg.Add("Não é permitido dividir um periodo em que o feedback se encontra salvo.");
                }
                if (ok)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Database.ExecuteSqlCommand(@"UPDATE T_CLP_MEDICOES SET GRUPO = @GRUPODEC, STATUS = @STATUS
                                                            WHERE ID IN (SELECT ID FROM T_CLP_MEDICOES (NOLOCK) 
                                                                     WHERE MAQUINA_ID = @MAQUINA_ID AND GRUPO = @GRUPO) "
                                                                , new SqlParameter("@GRUPODEC", (grupo + 0.999))
                                                                , new SqlParameter("@MAQUINA_ID", maquina)
                                                                , new SqlParameter("@GRUPO", grupo)
                                                                , new SqlParameter("@STATUS", 9));
                            foreach (ClpMedicoes m in med)
                            {
                                m.IdLoteClp = -1;
                                m.Grupo = grupo + aux;
                                m.Status = 0;
                                db.ClpMedicoes.Add(m);
                                aux = aux + 0.1;
                            }
                            int cont = db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            if (transaction != null)
                                transaction.Rollback();
                            ok = false;
                        }
                    }
                }
            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new { status = ok, msgError = msg }, settingsJSON);
        }

        [HttpPost]
        public ActionResult DividirPeriodoProducao(string medicaoJson)
        {
            ClpMedicoes medicao = JsonConvert.DeserializeObject<ClpMedicoes>(medicaoJson);
            // Declaraçoes variáveis auxiliares
            List<string> msg = new List<string>();
            double volumeDiscrepante = 0;
            double intervaloTempo = 0;
            double performance = 0;
            bool ok = true;
            double? grupo = medicao.Grupo;
            double AuxGrupo = Math.Round((double)grupo, 1);
            double grupoInt = Math.Floor(AuxGrupo);
            string maquina = medicao.MaquinaId;
            double aux = Math.Round(0.1, 1);
            string msgErro = "";

            //----
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                #region Validações de divisão
                if (medicao.DataFim <= medicao.DataInicio)
                {
                    msgErro = "Não é permitido dividir um periodo com a data fim menor que a data de início.";
                    return Json(new { status = false, msgError = msgErro }, settingsJSON);
                }

                int qtdFeedbacks = db.ViewFeedback.AsNoTracking().Count(f => f.MaquinaId == maquina && f.Grupo == grupo);
                if (qtdFeedbacks > 0)
                {
                    msgErro = "Não é permitido dividir um periodo em que o feedback se encontra salvo.";
                    return Json(new { status = false, msgError = msgErro }, settingsJSON);
                }

                bool periodoJaFoiDividido = db.ClpMedicoes.AsNoTracking()
                    .Any(f => f.MaquinaId == maquina && f.Grupo > grupo && f.Grupo < (grupoInt + 0.999));

                if (periodoJaFoiDividido)
                {
                    msgErro = "Operação inválida, o período já foi dividido.";
                    return Json(new { status = false, msgError = msgErro }, settingsJSON);
                }
                #endregion #region Validações de divisão

                //VERIFICANDO SE A LINHA A SER DIVIDIDA, É A ULTIMA
                var ultimoGrupo = db.ClpMedicoes.AsNoTracking()
                    .Where(m => m.MaquinaId == maquina).Max(mm => mm.Grupo);
                if (grupo == ultimoGrupo)
                {
                    //SE SIM ENVIE MENSAGEM
                    bool envouMsg = UtilPlay.SendMensagem(db, "QUEBRA_GRUPOS_SPLIT_" + maquina, "", "PENDENTE", "SERVICO");
                    if (!envouMsg)
                    {
                        msgErro = "Erro no envio da Mensagem de divisão do último grupo.";
                        return Json(new { status = false, msgError = msgErro }, settingsJSON);
                    }
                    else
                    {
                        string msgAtencao = "Aguarde a divisão automática da última linha!";
                        return Json(new { status = true, msgAtencao }, settingsJSON);
                    }

                }

                bool divididoAnteriormente = false;
                DateTime auxDatIni = medicao.DataInicio;
                DateTime auxDatFim = medicao.DataFim;

                //Auxiliar valores após o corte
                ClpMedicoes clpAbaixo = new ClpMedicoes
                {
                    MaquinaId = maquina,
                    Quantidade = 0,
                    Grupo = grupo,
                    IdLoteClp = -1,
                    Status = 0,
                    TurmaId = medicao.TurmaId,
                    TurnoId = medicao.TurnoId
                };

                // Auxiliar valores até o corte
                ClpMedicoes clpAcima = new ClpMedicoes
                {
                    MaquinaId = maquina,
                    Quantidade = 0,
                    Grupo = grupo,
                    IdLoteClp = -1,
                    Status = 0,
                    TurmaId = medicao.TurmaId,
                    TurnoId = medicao.TurnoId
                };

                //Retirando casas decimais do grupo para obter mediçoes originais
                grupo = Convert.ToDouble(Math.Floor(Convert.ToDecimal(grupo.ToString())));

                //Seleciona mediçoes originais
                var clpMedDados = db.ClpMedicoes.AsNoTracking()
                    .Where(m => m.MaquinaId == maquina && (m.Grupo == grupo || m.Grupo == (grupo + 0.999)) && m.DataInicio >= auxDatIni)
                    .Select(mm => new
                    {
                        mm.MaquinaId,
                        mm.Grupo,
                        mm.DataFim,
                        mm.DataInicio,
                        mm.Quantidade,
                        mm.Status,
                        mm.Id
                    }).OrderBy(mmm => mmm.DataInicio).ToList();

                //Quebrando quantidades
                clpAcima.DataInicio = clpMedDados.First().DataInicio;
                divididoAnteriormente = (clpMedDados.First().Status == 9);
                double controle = 0;
                bool flag = true;
                foreach (var med in clpMedDados)
                {
                    controle += med.Quantidade;
                    if (controle >= medicao.Quantidade)
                    {
                        if (flag)
                        {
                            volumeDiscrepante = controle - medicao.Quantidade;
                            if (volumeDiscrepante == 0)
                            {
                                clpAcima.Quantidade = controle;
                                clpAcima.DataFim = med.DataFim;
                                clpAbaixo.DataInicio = clpAcima.DataFim;
                            }
                            else
                            {
                                intervaloTempo = (med.DataFim.Subtract(med.DataInicio)).TotalSeconds;
                                performance = intervaloTempo / med.Quantidade;
                                TimeSpan periodo = new TimeSpan((long)(volumeDiscrepante * performance));
                                clpAcima.DataFim = med.DataFim.Subtract(periodo);
                                clpAcima.Quantidade = controle - volumeDiscrepante;
                                clpAbaixo.Quantidade = volumeDiscrepante;
                                clpAbaixo.DataInicio = clpAcima.DataFim;
                            }
                            flag = false;
                        }
                        else
                        {
                            clpAbaixo.DataFim = med.DataFim;
                        }
                    }
                }
                //clpAbaixo.Quantidade = medicoes[0].Quantidade -  clpAcima.Quantidade;
                clpAbaixo.Quantidade = medicao.QtdOriginal - clpAcima.Quantidade;
                clpAbaixo.DataInicio = clpAcima.DataFim;
                if (divididoAnteriormente)
                {
                    clpAbaixo.Grupo = Math.Round((double)(clpAcima.Grupo + aux), 1);
                }
                else
                {
                    clpAcima.Grupo = Math.Round((double)(grupo + aux), 1);
                    clpAbaixo.Grupo = Math.Round((double)(clpAcima.Grupo + aux), 1);
                }

                //----
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (!divididoAnteriormente)
                        {
                            int resp = db.Database.ExecuteSqlCommand(@"UPDATE T_CLP_MEDICOES SET GRUPO = @GRUPODEC, STATUS = @STATUS
                                                        WHERE ID IN (SELECT ID FROM T_CLP_MEDICOES (NOLOCK) 
                                                        WHERE MAQUINA_ID = @MAQUINA_ID AND GRUPO = @GRUPO AND 
                                                        DATA_INI BETWEEN @DT_INI AND @DT_FIM)"
                                                        , new SqlParameter("@GRUPODEC", (grupo + 0.999))
                                                        , new SqlParameter("@MAQUINA_ID", maquina)
                                                        , new SqlParameter("@GRUPO", grupo)
                                                        , new SqlParameter("@DT_INI", auxDatIni)
                                                        , new SqlParameter("@DT_FIM", auxDatFim)
                                                        , new SqlParameter("@STATUS", 9));

                            clpAcima.Emissao = DateTime.Now;
                            clpAbaixo.Emissao = DateTime.Now;

                            db.ClpMedicoes.Add(clpAcima);
                            db.ClpMedicoes.Add(clpAbaixo);
                        }
                        else
                        {//Registros previamente divididos
                            //Recuperando registros que sofreram split anterior
                            var clpAcimaAux = db.ClpMedicoes.AsNoTracking()
                                .Where(m => m.MaquinaId == maquina && m.Grupo == clpAcima.Grupo).FirstOrDefault();

                            clpAcimaAux.DataInicio = clpAcima.DataInicio;
                            clpAcimaAux.DataFim = clpAcima.DataFim;
                            clpAcimaAux.Quantidade = clpAcima.Quantidade;

                            db.ClpMedicoes.Attach(clpAcimaAux);
                            db.Entry(clpAcimaAux).State = EntityState.Modified;
                            db.ClpMedicoes.Add(clpAbaixo);
                        }
                        int cont = db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        ok = false;
                        Console.WriteLine("" + e);
                    }
                }
            }

            return Json(new { status = ok, msgError = msg }, settingsJSON);
        }

        #endregion
        #region Feedbacks de quantidade
        [HttpGet]
        public ActionResult FeedbackQuantidade(string order, string maq, int seqTran, int seqRep, string produto, string pecasPulso, string url)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {

                // ESTUDO
                //ATUALIZA quabtidade dos feedbacks pois a quantidade pode nao estar batendo com a clp medicao 
                //string sql = "  UPDATE T_FEEDBACK SET FEE_DIA_TURMA = dbo.DIATURMA(DTF),FEE_DATA_INICIAL = DTI,FEE_DATA_FINAL = DTF, FEE_QTD_PULSOS = QTD," +
                //            // atualiza op pois é posivel que as ops tenham sido trocadas pela opção de troca de OP e os feedbacks tenham sido salvos anteriormente. 
                //            "   ORD_ID = '" + order + "', PRO_ID = '" + produto + "', ROT_SEQ_TRANFORMACAO = " + seqTran.ToString() + ", FPR_SEQ_REPETICAO = " + seqRep.ToString() +
                //            "   FROM T_FEEDBACK (nolock) INNER JOIN(SELECT GRUPO, MAQUINA_ID, MIN(DATA_INI) DTI, MAX(DATA_FIM) DTF, SUM(QTD) QTD  FROM V_CLP_MEDICOES_PENDENTES (NOLOCK) " +
                //            "   GROUP BY GRUPO, MAQUINA_ID ) AS CLP ON GRUPO = FEE_GRUPO AND MAQUINA_ID = MAQ_ID " +
                //            "   WHERE MAQ_ID = '" + maq + "'";
                //db.Database.ExecuteSqlCommand(sql);

                ViewBag.urlAnterior = HttpUtility.UrlDecode(url);
                if (!string.IsNullOrEmpty(order)
                    && !string.IsNullOrEmpty(maq)
                    && !string.IsNullOrEmpty(produto))
                {

                    var feedbacks = db.ViewClpMedicoes.AsNoTracking().Where(f => f.MaquinaId == maq && f.FeedBackId != null && f.FeedBackIdMov == -1)
                    .Select(f => new
                    {
                        quantidade = f.Quantidade * Convert.ToDouble(pecasPulso.Replace('.', ',')),
                        id = f.FeedBackId
                    }).ToList();



                    if (feedbacks.Count > 0)
                    {
                        var qtdProdOpAtual = feedbacks.Sum(f => f.quantidade); //quantidade produzida na parte da op que esta sendo confirmada

                        double qtdProdOpTotal = db.MovimentoEstoque.AsNoTracking().Where(m => m.MAQ_ID == maq // quantidade total baseada em todos os movimentos salvos da op
                            && m.ORD_ID == order && m.FPR_SEQ_REPETICAO == seqRep
                            && m.FPR_SEQ_TRANFORMACAO == seqTran && m.TIP_ID == "000")
                            .Sum(m => (Double?)m.MOV_QUANTIDADE) ?? 0;

                        ViewBag.qtdProdOpTotal = qtdProdOpTotal;

                        ViewBag.sltMotivos = new SelectList(db.Ocorrencia.AsNoTracking().ToList(), "OCO_ID", "OCO_DESCRICAO");
                        ViewBag.quantidade = qtdProdOpAtual;
                        ViewBag.produto = db.Produto.Find(produto);
                        ViewBag.op = order + produto + seqTran + seqRep;

                        ViewBag.order = order;
                        ViewBag.seqTran = seqTran;
                        ViewBag.seqRep = seqRep;
                        ViewBag.pecasPulso = pecasPulso;

                        ViewBag.maquina = db.Maquina.AsNoTracking().Where(m => m.MAQ_ID == maq).Select(m => m.MAQ_DESCRICAO).FirstOrDefault();
                        ViewBag.pedido = order;
                        ViewBag.feedbacks = feedbacks.Select(x => new { FeedbackId = x.id });
                        ViewBag.maquinaId = maq;
                        ViewBag.seqTran = seqTran;
                        ViewBag.seqRep = seqRep;
                        //quantidades
                        var op = db.FilaProducao.AsNoTracking().Where(f => f.ROT_MAQ_ID == maq &&
                            f.ORD_ID == order && f.ROT_PRO_ID == produto &&
                            f.ROT_SEQ_TRANFORMACAO == seqTran && f.FPR_SEQ_REPETICAO == seqRep)
                            .Select(f => new { f.FPR_QUANTIDADE_PREVISTA, f.Order.ORD_TOLERANCIA_MENOS }).FirstOrDefault();

                        var qtdMin = 100 - ((op.ORD_TOLERANCIA_MENOS == null) ? 0 : op.ORD_TOLERANCIA_MENOS);
                        qtdMin = op.FPR_QUANTIDADE_PREVISTA * (qtdMin / 100);
                        qtdMin = Math.Round(qtdMin.Value);
                        ViewBag.qtdMinPrevista = qtdMin;

                        var ocorrencias = db.Ocorrencia.AsNoTracking().Where(o => o.TIP_ID == 6).ToList();
                        var dlOcorrenciaOpParcial = new List<SelectListItem>();
                        foreach (var o in ocorrencias)
                        {
                            dlOcorrenciaOpParcial.Add(new SelectListItem()
                            {
                                Value = o.OCO_ID,
                                Text = o.OCO_DESCRICAO
                            });
                        }
                        ViewBag.ddlOcorrenciaOpParcial = dlOcorrenciaOpParcial;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            return View();
        }
        //public ActionResult ObterFeedbacksAgupOp(string maqId)
        //{
        //    List<Feedback> feedbacks = bd.Feedback.Include(i => i.Produto).Where(f => f.MaquinaId == maqId && !string.IsNullOrEmpty(f.OrderId) && !string.IsNullOrEmpty(f.TurnoId) && f.Grupo != -1)/*.Where(f=>f.MovimentosEstoque.Count() == 0)*/.ToList();
        //    var gupos = feedbacks.GroupBy(g => new { g.MaquinaId, g.OrderId, g.TurnoId, g.SequenciaTransformacao, g.SequenciaRepeticao, g.Produto })
        //                                          .Select(g => new
        //                                          {
        //                                              op = g.Key.OrderId,
        //                                              produtoId = g.Key.Produto.Id,
        //                                              produtoDescricao = g.Key.Produto.Descricao,
        //                                              inicio = g.Min(x => x.DataInicial).ToString(),
        //                                              fim = g.Max(x => x.Datafinal).ToString(),
        //                                              quantidade = g.Sum(x => x.QuantidadePulsos * x.QuantidadePecasPorPulso),
        //                                              seqTranId = g.Key.SequenciaTransformacao,
        //                                              seqRepId = g.Key.SequenciaRepeticao,
        //                                              turno = g.Key.TurnoId,
        //                                              maqId = g.Key.MaquinaId
        //                                          }).ToList();
        //    return Json(gupos);
        //}


        [HttpGet]
        public ActionResult VerificarQtdPerdasProducao(int somatorioPerda, int qtdTotalProduzida)
        {
            if (somatorioPerda > qtdTotalProduzida)
                return Json(new { st = "ERRO", msg = "A quantidade de perda informada deve ser menor ou igual que a produzida." });
            return Json(new { st = "OK", msg = "" });
        }

        private bool VericarSeOpAtingiuQtdMinima(string maqId, string ordId, string proId, int seqTran, int seqRep, JSgi db)
        {
            double qtdProduzida = db.MovimentoEstoque.AsNoTracking().FromSql($"select case when sum(MOV_QUANTIDADE) is null then 0 else sum(MOV_QUANTIDADE) end as MOV_QUANTIDADE from T_MOVIMENTOS_ESTOQUE nolock " +
                $"where TIP_ID = '000' and MAQ_ID = '{maqId.Trim()}' and ORD_ID = '{ordId.Trim()}' and PRO_ID = '{proId.Trim()}' and FPR_SEQ_TRANFORMACAO = {seqTran} and FPR_SEQ_REPETICAO = {seqRep}")
                .Select(m => m.MOV_QUANTIDADE).FirstOrDefault();

            var toleranciaMin = db.Order.AsNoTracking().Where(o => o.ORD_ID == ordId.Trim())
                .Select(o => o.ORD_TOLERANCIA_MENOS).FirstOrDefault();
            toleranciaMin = toleranciaMin.HasValue ? toleranciaMin : 0;


            double qtdPrevista = db.FilaProducao.AsNoTracking().FromSql($"select FPR_QUANTIDADE_PREVISTA from T_FILA_PRODUCAO nolock " +
                $"where ROT_MAQ_ID = '{maqId.Trim()}' and ORD_ID = '{ordId.Trim()}' and ROT_PRO_ID = '{proId.Trim()}' and ROT_SEQ_TRANFORMACAO = {seqTran} and FPR_SEQ_REPETICAO = {seqRep}")
                .Select(f => f.FPR_QUANTIDADE_PREVISTA).FirstOrDefault();

            double qtdMin = Math.Round(qtdPrevista * ((100 - toleranciaMin.Value) / 100));

            return qtdProduzida >= qtdMin;
        }

        [HttpPost]
        public IActionResult EncerrarOP(string maqId, string ordId, string proId, string seqTran, int seqRep, string ocorencia, string justificativa)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var logsEncerramentoOP = EncerrarOrdemProducao(maqId, ordId, proId, int.Parse(seqTran), seqRep, ocorencia, justificativa, db);

                var logsDeErros = logsEncerramentoOP.Where(l => l.Status != "OK").ToList();

                string st = "OK";
                string msg = "";
                if (logsDeErros.Count > 0)
                {
                    st = "ERRO";
                    logsDeErros.ForEach(x => msg += $"{x.MsgErro}; ");
                }

                return Json(new { st, msg });
            }
        }

        private List<LogPlay> EncerrarOrdemProducao(string maqId, string ordId, string proId, int seqTran, int seqRep, string ocorencia, string justificativa, JSgi db)
        {
            List<LogPlay> retorno = new List<LogPlay>();

            var op = db.FilaProducao.AsNoTracking().FromSql(
                "select * from T_FILA_PRODUCAO nolock " +
                "where ROT_MAQ_ID = @ROT_MAQ_ID and ORD_ID = @ORD_ID and ROT_PRO_ID = @ROT_PRO_ID and " +
                "ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO and FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO",
                new SqlParameter("@ROT_MAQ_ID", maqId.Trim()),
                new SqlParameter("@ORD_ID", ordId.Trim()),
                new SqlParameter("@ROT_PRO_ID", proId.Trim()),
                new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
            ).FirstOrDefault();

            if (op == null)
            {
                retorno.Add(new LogPlay("ERRO", "A OP não foi encontrada."));
                return retorno;
            }

            op.FPR_STATUS = "EN";
            op.PlayAction = "update";

            //Verificadno O FPR_ID_ORIGEM para testar se a OP foi dividida
            bool FoiDividida = op.IsDivided(db);
            if (FoiDividida)//Caso tenha sido dividida, testar se é a ULTIMA Sequencia de Transformação
            {
                op.FPR_STATUS = op.IsLastSequence(db) ? op.FPR_STATUS : "ED";//Foi dividida. :--> Caso seja a ultima sequencia EN/ Caso não ED
            }

            MasterController mc = new MasterController();
            mc.UsuarioLogado = ObterUsuarioLogado();

            List<object> objetos = new List<object>();
            objetos.Add(op);

            List<List<object>> objetosParaPersistir = new List<List<object>>();
            objetosParaPersistir.Add(objetos);

            List<LogPlay> logsUpdateData = mc.UpdateData(objetosParaPersistir, 0, true, db);

            var logsDeErros = logsUpdateData.Where(l => l.Status != "OK").ToList();

            if (logsDeErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(ocorencia) && !string.IsNullOrEmpty(justificativa))
                {
                    /* Não atingiu a quantidade mínima de produção, mas a OP será encerrada parcialmente.
                     * O último movimento de produção será atualizado.
                     */

                    // Atualizando a ocorrência e a justificativa do novimento de produção
                    int count = db.Database.ExecuteSqlCommand(
                        "UPDATE T_MOVIMENTOS_ESTOQUE SET MOV_OCO_ID_OP_PARCIAL = @MOV_OCO_ID_OP_PARCIAL, MOV_OBS_OP_PARCIAL = @MOV_OBS_OP_PARCIAL " +
                        "WHERE TIP_ID <= '100' AND ORD_ID = @ORD_ID AND PRO_ID = @PRO_ID AND MAQ_ID = @MAQ_ID AND " +
                        "FPR_SEQ_TRANFORMACAO = @FPR_SEQ_TRANFORMACAO AND FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;",
                        new SqlParameter("@MOV_OCO_ID_OP_PARCIAL", ocorencia),
                        new SqlParameter("@MOV_OBS_OP_PARCIAL", justificativa),
                        new SqlParameter("@ORD_ID", ordId.Trim()),
                        new SqlParameter("@PRO_ID", proId.Trim()),
                        new SqlParameter("@MAQ_ID", maqId.Trim()),
                        new SqlParameter("@FPR_SEQ_TRANFORMACAO", seqTran),
                        new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                    );

                }
                
                string msgConsumirLotesChapas = ConsumirLotesChapas(ordId, proId, maqId, seqTran, seqRep, db);

                if (msgConsumirLotesChapas == "OK")
                {
                    retorno.Add(new LogPlay("OK", ""));
                }
                else
                {
                    retorno.Add(new LogPlay("ERRO", msgConsumirLotesChapas));
                }
            }

            retorno.InsertRange(0, logsUpdateData);

            return retorno;
        }

        /// <summary>
        /// Verifica se está na ordem correta de produção.
        /// </summary>
        /// <param name="maquina"></param>
        /// <param name="produto"></param>
        /// <param name="order"></param>
        /// <param name="seqRep"></param>
        /// <param name="seqTran"></param>
        /// <returns></returns>
        public IActionResult VerificarOrdemNaFila(string maquina, string produto, string order, int seqRep, int seqTran)
        {
            bool estaForaDaOrdem = false;
            List<OcorrenciaPularOrdemFila> ocorrencias = null;
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                bool ativarJustificativa = db.Param.AsNoTracking().Any(p => p.PAR_ID == "OPERADOR_MOTIVO_PULAR_FILA" && p.PAR_VALOR_S == "S");

                if (ativarJustificativa)
                {
                    DateTime inicioPrevisto = db.ViewFilaProducao.Where(f => f.RotMaqId == maquina && f.PaProId == produto && f.OrdId == order &&
                    f.RotSeqTransformacao == seqTran && f.FprSeqRepeticao == seqRep).Select(f => f.FprDataInicioPrevista).FirstOrDefault();

                    estaForaDaOrdem = db.ViewFilaProducao.Any(f => f.RotMaqId == maquina && f.FprDataInicioPrevista < inicioPrevisto);

                    if (estaForaDaOrdem)
                    {
                        ocorrencias = db.OcorrenciaPularOrdemFila.ToList();
                    }
                }
            }

            return Json(new { estaForaDaOrdem, ocorrencias });
        }

        [HttpGet]
        public ActionResult GravarMotivoPularFila(string maquina, string produto, string order, int seqRep, int seqTran, string ocorrenciaId, string obs)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                FilaProducao op = db.FilaProducao.AsNoTracking().Where(f => f.ROT_MAQ_ID == maquina && f.ROT_PRO_ID == produto && f.ORD_ID == order &&
                    f.ROT_SEQ_TRANFORMACAO == seqTran && f.FPR_SEQ_REPETICAO == seqRep).FirstOrDefault();

                if (op == null)
                    return Json(new { st = "ERRO", msg = "Ordem de produçao não encontrada." });

                op.PlayAction = "update";
                op.OCO_ID = ocorrenciaId;
                op.FPR_MOTIVO_PULA_FILA = obs;

                List<List<object>> objetosParaPersistir = new List<List<object>>();
                objetosParaPersistir.Add(new List<object> { op });

                MasterController mc = new MasterController();
                List<LogPlay> logs = mc.UpdateData(objetosParaPersistir, 0, true);

                if (logs.Any(l => l.Status != "OK"))
                {
                    string msg = "";
                    logs.ForEach(x =>
                    {
                        msg += $"{x.MsgErro}\n";
                    });

                    return Json(new { st = "ERRO", msg });
                }

                return Json(new { st = "OK", msg = "" });

            }
        }

        [HttpPost]
        public ActionResult GravarApontamentosProducao(string movimentos, string pecasPulso)
        {
            pecasPulso = JsonConvert.DeserializeObject<string>(pecasPulso);
            List<MovimentoEstoque> listMovimentos = JsonConvert.DeserializeObject<List<MovimentoEstoque>>(movimentos);
            bool ok = true;
            int movId = 0;
            string msg = "";
            double qtdPerdaLancadas = listMovimentos.Where(x => x.TIP_ID == "501").Sum(x => x.MOV_QUANTIDADE);
            double qtdProduzida = listMovimentos.First(x => x.TIP_ID == "000").MOV_QUANTIDADE;
            double qtdPecaBoaProduzida = 0;

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            if (qtdPerdaLancadas > qtdProduzida)
            {
                ok = false;
                msg = "A quantidade de perda lançada é maior que a quantidade produzida.";
            }
            else
            {
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    string proId = listMovimentos[0].PRO_ID;
                    string maquinaId = listMovimentos[0].MAQ_ID;
                    string ordId = listMovimentos[0].ORD_ID;
                    int seqTran = (int)listMovimentos[0].FPR_SEQ_TRANFORMACAO;
                    int seqRep = (int)listMovimentos[0].FPR_SEQ_REPETICAO;

                    string turma = "";
                    string turno = "";

                    double qtdHistProduzida = db.MovimentoEstoque.AsNoTracking().FromSql(
                        "select case when sum(MOV_QUANTIDADE) is null then 0 else sum(MOV_QUANTIDADE) end as MOV_QUANTIDADE from T_MOVIMENTOS_ESTOQUE " +
                        "where TIP_ID = '000' and MAQ_ID = @MAQ_ID and ORD_ID = @ORD_ID and PRO_ID = @PRO_ID and " +
                        "FPR_SEQ_TRANFORMACAO = @FPR_SEQ_TRANFORMACAO and FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO",
                        new SqlParameter("@MAQ_ID", maquinaId.Trim()),
                        new SqlParameter("@ORD_ID", ordId.Trim()),
                        new SqlParameter("@PRO_ID", proId.Trim()),
                        new SqlParameter("@FPR_SEQ_TRANFORMACAO", seqTran),
                        new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                        ).Select(m => m.MOV_QUANTIDADE).FirstOrDefault();

                    FilaProducao op = db.FilaProducao.AsNoTracking().Where(f => f.ROT_MAQ_ID == maquinaId.Trim() && f.ORD_ID == ordId.Trim() &&
                        f.ROT_PRO_ID == proId.Trim() && f.ROT_SEQ_TRANFORMACAO == seqTran && f.FPR_SEQ_REPETICAO == seqRep)
                        .Select(f => new FilaProducao { FPR_ID = f.FPR_ID, FPR_STATUS = f.FPR_STATUS }).FirstOrDefault();

                    qtdPecaBoaProduzida = Math.Abs((qtdProduzida + qtdHistProduzida) - qtdPerdaLancadas);

                    var viewFeedbacks = db.ViewClpMedicoes.AsNoTracking()
                       .Where(f => f.MaquinaId == maquinaId && f.FeedBackId != null && f.FeedBackIdMov == -1).ToList();

                    StringBuilder sbSqlFeedbacks = new StringBuilder();
                    foreach (var feed in viewFeedbacks)
                    {
                        turma = feed.TurmaId;
                        turno = feed.TurnoId;

                        sbSqlFeedbacks.AppendLine($"UPDATE T_FEEDBACK SET FEE_DIA_TURMA = dbo.DIATURMA('{feed.DataFim.ToString("yyyyMMdd HH:mm:ss")}'), FEE_DATA_INICIAL = '{feed.DataIni.ToString("yyyyMMdd HH:mm:ss")}', ");
                        sbSqlFeedbacks.AppendLine($"FEE_DATA_FINAL = '{feed.DataFim.ToString("yyyyMMdd HH:mm:ss")}', FEE_QTD_PULSOS = {feed.Quantidade}, FEE_QTD_PECAS_POR_PULSO = {pecasPulso.Replace(',', '.')}, ");
                        sbSqlFeedbacks.AppendLine($"ORD_ID = '{ordId}', PRO_ID = '{proId}', ROT_SEQ_TRANFORMACAO = {seqTran}, FPR_SEQ_REPETICAO = {seqRep} ");
                        sbSqlFeedbacks.AppendLine($"WHERE FEE_ID = {feed.FeedBackId}; ");
                    }

                    // PONTO CRÍTICO DE TESTE
                    List<int> feedbackIds = viewFeedbacks.Select(f => (int)f.FeedBackId).ToList();
                    int feeMovCount = db.T_FeedbackMovEstoque.AsNoTracking()
                        .Where(x => feedbackIds.Contains(x.FeedbackId)).Count();
                    if (feeMovCount == 0)
                    {
                        DateTime dataCriacao = viewFeedbacks.Max(f => f.DataFim);
                        T_Usuario user = ObterUsuarioLogado();
                        foreach (MovimentoEstoque movimentoEstoque in listMovimentos)
                        {
                            List<T_FeedbackMovEstoque> feedbackMovEstoque = new List<T_FeedbackMovEstoque>();
                            foreach (var id in feedbackIds)
                            {
                                feedbackMovEstoque.Add(new T_FeedbackMovEstoque
                                {
                                    FeedbackId = id,
                                    MovimentoEstoque = movimentoEstoque
                                });
                            }
                            movimentoEstoque.MOV_QUANTIDADE = Math.Floor(movimentoEstoque.MOV_QUANTIDADE);
                            movimentoEstoque.USE_ID = user.USE_ID;
                            movimentoEstoque.MOV_DATA_HORA_CRIACAO = dataCriacao;
                            movimentoEstoque.MOV_DATA_HORA_EMISSAO = dataCriacao;
                            movimentoEstoque.T_FeedbackMovEstoque = feedbackMovEstoque;
                            movimentoEstoque.TURM_ID = turma;
                            movimentoEstoque.TURN_ID = turno;
                            movimentoEstoque.MOV_LOTE = (listMovimentos[0].MOV_LOTE != null) ? listMovimentos[0].MOV_LOTE : "N/A";
                            movimentoEstoque.MOV_SUB_LOTE = (listMovimentos[0].MOV_SUB_LOTE != null) ? listMovimentos[0].MOV_SUB_LOTE : "N/A";

                            db.MovimentoEstoque.Add(movimentoEstoque);
                        }

                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                db.SaveChanges();
                                movId = listMovimentos.Where(x => x.TIP_ID == "000").Select(x => x.MOV_ID).FirstOrDefault();
                                foreach (var item in listMovimentos)
                                {
                                    if (movId != item.MOV_ID)
                                    {
                                        item.MOV_ID_ORIGEM = movId;
                                    }
                                }
                                db.SaveChanges();

                                bool atingiuTolMin = VericarSeOpAtingiuQtdMinima(maquinaId, ordId, proId, seqTran, seqRep, db);
                                int result = 0;
                                string msgConsumoChapas = "OK";

                                // Atualizando a qtd produzida 
                                string scriptAtualizarOP = ObterScriptSQLQtdProduzidaOP();
                                result = db.Database.ExecuteSqlCommand(
                                    scriptAtualizarOP,
                                    new SqlParameter("@ORD_ID", ordId),
                                    new SqlParameter("@ROT_PRO_ID", proId),
                                    new SqlParameter("@ROT_MAQ_ID", maquinaId),
                                    new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                                    new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                                );

                                if (atingiuTolMin)
                                {
                                    // Encerrar a Ordem de Produção
                                    result = db.Database.ExecuteSqlCommand(
                                        "update T_FILA_PRODUCAO set FPR_STATUS = 'EN' " +
                                        "where ROT_MAQ_ID = @ROT_MAQ_ID and ORD_ID = @ORD_ID and " +
                                        "ROT_PRO_ID = @ROT_PRO_ID and ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO and FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;",
                                        new SqlParameter("@ROT_MAQ_ID", maquinaId.Trim()),
                                        new SqlParameter("@ORD_ID", ordId.Trim()),
                                        new SqlParameter("@ROT_PRO_ID", proId.Trim()),
                                        new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                                        new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                                    );

                                    T_LOGS_DATABASE logDb = new T_LOGS_DATABASE();
                                    logDb.LOGS_TABLE = "T_FILA_PRODUCAO";
                                    logDb.LOGS_KEY = $"FPR_ID: {op.FPR_ID} - OPERADOR";
                                    logDb.LOGS_COLUMN = $"{nameof(op.FPR_STATUS)}";
                                    logDb.LOGS_BEFORE = op.FPR_STATUS;
                                    logDb.LOGS_AFTER = "EN";
                                    logDb.LOGS_ACTION = "UPDATE";
                                    logDb.LOGS_DATE = DateTime.Now;
                                    logDb.USE_ID = ObterUsuarioLogado().USE_ID;

                                    db.T_LOGS_DATABASE.Add(logDb);
                                    result = db.SaveChanges();

                                    msgConsumoChapas = ConsumirLotesChapas(ordId, proId, maquinaId, seqTran, seqRep, db);
                                }
                                else
                                {
                                    msg = "AbrirModalEncerrarOP";
                                }

                                result = db.Database.ExecuteSqlCommand(sbSqlFeedbacks.ToString());

                                int count = db.ViewClpMedicoes.AsNoTracking().Count(f => f.MaquinaId == maquinaId && f.FeedBackId != null && f.FeedBackIdMov == -1);
                                if (count == 0 && msgConsumoChapas == "OK")
                                {
                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    ok = false;
                                    msg = "Ocorreu um erro ao salvar os dados.";
                                }
                            }
                            catch (Exception ex)
                            {
                                msg = "Ocorreu um erro ao salvar os dados.";
                                transaction.Rollback();
                                ok = false;
                            }
                        }
                    }
                    else
                    {
                        msg = "Ocorreu um erro ao salvar os dados porque os feedbacks para esta ordem de produção já foram realizados.";
                    }
                }
            }

            return Json(new
            {
                ok,
                msg,
                movId,
                qtdPecaBoaProduzida
            }, settingsJSON);
        }

        private string ObterScriptSQLQtdProduzidaOP()
        {
            return
                "UPDATE T_FILA_PRODUCAO SET FPR_QTD_PRODUZIDA = ISNULL(MOV.QTD, 0) " +
                "FROM T_FILA_PRODUCAO AS F " +
                "LEFT JOIN ( " +
                "   SELECT SUM(MOV_QUANTIDADE) AS QTD, ORD_ID, PRO_ID, FPR_SEQ_TRANFORMACAO, " +
                "   FPR_SEQ_REPETICAO, MAQ_ID " +
                "   FROM T_MOVIMENTOS_ESTOQUE WHERE TIP_ID = '000' AND ISNULL(MOV_ESTORNO, '') <> 'E' " +
                "   GROUP BY ORD_ID, PRO_ID, FPR_SEQ_TRANFORMACAO, FPR_SEQ_REPETICAO, MAQ_ID " +
                ") AS MOV ON F.ORD_ID = MOV.ORD_ID AND F.ROT_PRO_ID = MOV.PRO_ID AND " +
                "   F.ROT_SEQ_TRANFORMACAO = MOV.FPR_SEQ_TRANFORMACAO AND " +
                "   F.FPR_SEQ_REPETICAO = MOV.FPR_SEQ_REPETICAO AND F.ROT_MAQ_ID = MOV.MAQ_ID " +
                "WHERE F.ORD_ID = @ORD_ID AND F.ROT_PRO_ID = @ROT_PRO_ID AND F.ROT_MAQ_ID = @ROT_MAQ_ID AND " +
                "   F.ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO AND F.FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;";
        }


        public string ConsumirLotesChapas(string ordId, string proId, string maqId, int seqTran, int seqRep, JSgi db)
        {
            int qtdLinhas = db.V_OPS_A_PLANEJAR.AsNoTracking().FromSql(
                "select SequenciaTransformacao FROM V_OPS_A_PLANEJAR " +
                "WHERE LEFT(ORD_STATUS, 1) <> 'E' AND ORD_STATUS <> 'SS' and " +
                "OrderId = @OrderId and ProdutoId = @ProdutoId and SequenciaRepeticao = @SequenciaRepeticao and " +
                "SequenciaTransformacao < @SequenciaTransformacao",
                new SqlParameter("@OrderId", ordId),
                new SqlParameter("@ProdutoId", proId),
                new SqlParameter("@SequenciaRepeticao", seqRep),
                new SqlParameter("@SequenciaTransformacao", seqTran)
            ).Count();

            if (qtdLinhas > 0)
            {// Não é a primeira sequencia de transformação da caixa.
                return "OK";
            }

            // O consumo de chapas é feito somente na primeira sequência de transformação

            string chapa = db.Produto.AsNoTracking().FromSql(
                "select PRO_ID_CHAPA from V_PRODUTOS where PRO_ID = @PRO_ID",
                new SqlParameter("@PRO_ID", proId)
            ).Select(p => p.PRO_ID_CHAPA).FirstOrDefault();

            double qtdParaConsumir = db.MovimentoEstoque.AsNoTracking().FromSql(
                "select isnull(sum(M.MOV_QUANTIDADE * P.QTD_CHAPA), 0) as MOV_QUANTIDADE " +
                "from T_MOVIMENTOS_ESTOQUE M " +
                "inner join V_PRODUTOS P ON M.PRO_ID = P.PRO_ID " +
                "where M.ORD_ID = @ORD_ID and M.PRO_ID = @PRO_ID and M.MAQ_ID = @MAQ_ID and M.FPR_SEQ_TRANFORMACAO = @FPR_SEQ_TRANFORMACAO and " +
                "M.FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO and (M.TIP_ID = '000') AND isnull(M.MOV_ESTORNO, '') <> 'E' ",
                new SqlParameter("@ORD_ID", ordId),
                new SqlParameter("@PRO_ID", proId),
                new SqlParameter("@MAQ_ID", maqId),
                new SqlParameter("@FPR_SEQ_TRANFORMACAO", seqTran),
                new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
            ).Select(m => m.MOV_QUANTIDADE).FirstOrDefault();

            List<SaldosEmEstoquePorLote> lotesDeChapas = new List<SaldosEmEstoquePorLote>();
            
            int? tipoPedido = db.Order.AsNoTracking().Where(o => o.ORD_ID == ordId).Select(o => o.ORD_TIPO).FirstOrDefault();
            if (tipoPedido == 1)
            {
                lotesDeChapas = db.SaldosEmEstoquePorLote.AsNoTracking().FromSql(
                    "SELECT * FROM V_SALDO_ESTOQUE_POR_LOTE WHERE ORD_ID = @ORD_ID AND PRO_ID = @PRO_ID",
                    new SqlParameter("@ORD_ID", ordId),
                    new SqlParameter("@PRO_ID", chapa)
                ).ToList();
            }
            else if (tipoPedido == 2)
            {
                lotesDeChapas = db.SaldosEmEstoquePorLote.AsNoTracking().FromSql(
                    "select s.* " +
                    "from V_SALDO_ESTOQUE_POR_LOTE s " +
                    "inner join T_MOVIMENTOS_ESTOQUE m on s.MOV_LOTE = m.MOV_LOTE and s.MOV_SUB_LOTE = m.MOV_SUB_LOTE " +
                    "where m.ORD_ID = @ORD_ID and m.PRO_ID = @PRO_ID and TIP_ID = '001' and isnull(MOV_ESTORNO, '') <> 'E' ",
                    new SqlParameter("@ORD_ID", ordId),
                    new SqlParameter("@PRO_ID", chapa)
                ).ToList();
            }


            #region priorizando lotes com endereço IMP
            var lotesComEnderecoIMP = lotesDeChapas.Where(l => l.MOV_ENDERECO == "IMP").ToList();
            var outrosLotes = lotesDeChapas.Where(l => l.MOV_ENDERECO != "IMP").ToList();

            lotesDeChapas.Clear();
            lotesDeChapas.AddRange(lotesComEnderecoIMP);
            lotesDeChapas.AddRange(outrosLotes);
            #endregion priorizando lotes com endereço IMP

            #region consumo dos lotes de chapas
            var consumoDosLotes = new List<object>();

            int count = 0;
            while (qtdParaConsumir > 0 && count < lotesDeChapas.Count)
            {
                var lote = lotesDeChapas[count];
                    
                MovimentoEstoqueConsumoMateriaPrima movConsumo = new MovimentoEstoqueConsumoMateriaPrima();
                movConsumo.PRO_ID = lote.PRO_ID;
                movConsumo.MOV_LOTE = lote.MOV_LOTE;
                movConsumo.MOV_SUB_LOTE = lote.MOV_SUB_LOTE;
                movConsumo.ORD_ID = ordId;
                movConsumo.FPR_SEQ_TRANFORMACAO = seqTran;
                movConsumo.FPR_SEQ_REPETICAO = seqRep;

                if (qtdParaConsumir >= lote.SALDO)
                {
                    // gerar MovimentoEstoqueConsumoMateriaPrima com a quantidade igual da reserva do lote
                    movConsumo.MOV_QUANTIDADE = lote.SALDO.Value;
                        
                    qtdParaConsumir -= lote.SALDO.Value;
                }
                else
                {
                    movConsumo.MOV_QUANTIDADE = qtdParaConsumir;
                    qtdParaConsumir -= qtdParaConsumir;
                }
                consumoDosLotes.Add(movConsumo);
                count++;
            }
            #endregion consumo dos lotes de chapas

            string msg = "OK";
            if (consumoDosLotes.Count > 0)
            {
                var mc = new MasterController();

                mc.UsuarioLogado = ObterUsuarioLogado();

                List<List<object>> objetosParaPersistir = new List<List<object>>();
                objetosParaPersistir.Add(consumoDosLotes);
                var logsUpdateData = new List<LogPlay>();

                logsUpdateData = mc.UpdateData(objetosParaPersistir, 0, true);

                var logsDeErros = logsUpdateData.Where(l => l.Status == "ERRO").ToList();
                if (logsDeErros.Count > 0)
                {
                    msg = "";
                    logsDeErros.ForEach(l => msg += $"{l.MsgErro};\n");
                }
            }

            return msg;

        }

        //public ActionResult ObterMovimentosEstoque(string orderId, string maqId, int seqRep, int seqTran, string proId)
        //{
        //    object movimentos = new List<object>();
        //    try
        //    {
        //        movimentos = bd.MovimentoEstoque
        //                .Include(m => m.Ocorrencia).Include(m => m.Produto)
        //                .Where(m => m.OrderId == orderId && m.MaquinaId == maqId && m.SequenciaRepeticao == seqRep && m.SequenciaTransformacao == seqTran && m.ProdutoId == proId)
        //                .Select(m => new
        //                {
        //                    id = m.Id,
        //                    quantidade = m.Quantidade,
        //                    observacao = m.Observacao == null ? "" : m.Observacao,
        //                    produtoId = m.Produto.Id,
        //                    produtoDescricao = m.Produto.Descricao,
        //                    ordemProducaoId = m.OrderId,
        //                    tipo = m.Tipo,
        //                    ocorrenciaId = m.OcorrenciaId,
        //                    ocorrenciaDescricao = m.Ocorrencia.Descricao,
        //                    maquinaId = m.MaquinaId
        //                }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpStatusCodeResult(500);
        //    }
        //    return Json(movimentos);
        //}
        [HttpGet]
        public JsonResult HabilitarConsultaPainelOperador() 
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { })) 
            {
                var result = db.Param.Any(x => x.PAR_ID.Equals("CONSULTAR_OP_PAINEL_MAQUINAS") && x.PAR_VALOR_S.Equals("S"));
                return Json(new { result });
            }
        }
        
        [HttpPost]
        public IActionResult PesquisarOrdemProducao(string dados)
        {
            bool flag = true;
            StringBuilder msg = new StringBuilder();
            dynamic _Dados = JsonConvert.DeserializeObject<dynamic>(dados);

            string st = "";

            string idPedido = _Dados.order;
            string idMaquina = _Dados.maqId;
            string idEquipe = _Dados.equipeId;

            idPedido = idPedido.Trim();
            idMaquina = idMaquina.Trim();
            idEquipe = idEquipe.Trim();

            object fila = new List<object>();
            object maquinas = new List<object>();
            object roteirosA = new List<object>();
            object filaHistorico = new List<object>();
            object opsTarPendentes = new List<object>();
            if (String.IsNullOrEmpty(idPedido))
            {
                msg.Append("Erro! Você deve informar um ID do pedido para a pesquisa.");
                flag = false;
            }

            if (flag)
            {
                using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    List<ViewFilaProducao> filaProducao = new List<ViewFilaProducao>();


                    if (String.IsNullOrEmpty(idEquipe))
                    {
                        maquinas = (from maq in db.Maquina
                                    where maq.MAQ_ID == idMaquina
                                    select new
                                    {
                                        maq.MAQ_ID,
                                        maq.MAQ_CONGELA_FILA
                                    }).ToList();

                    }
                    else
                    {
                        maquinas = (from maquinaEquipe in db.T_MAQUINAS_EQUIPES
                                    join maq in db.Maquina
                                        on maquinaEquipe.MAQ_ID equals maq.MAQ_ID
                                    where maquinaEquipe.EQU_ID == idEquipe &&
                                          maq.MAQ_ID == idMaquina
                                    select new
                                    {
                                        maq.MAQ_ID,
                                        maq.MAQ_CONGELA_FILA
                                    }).ToList();

                    
                    }
                    var produto = db.Order.AsNoTracking().Where(x => x.ORD_ID.Equals(idPedido)).Select(x => x.PRO_ID).FirstOrDefault();

                    if (produto == null)
                    {
                        st = $"Erro! Não foi encontrado o produto para o pedido: {idPedido}";
                        return Json(new { st, fila, maquinas });
                    }

                    var rotPossiveis = db.V_ROTEIROS_POSSIVEIS_DO_PRODUTO.AsNoTracking()
                        .Where(x => x.MAQ_ID.Equals(idMaquina) && x.PRO_ID.Equals(produto))
                        .ToList();

                    if (rotPossiveis == null)
                    {
                        st = $"Erro! Não foi encontrado o roteiro para o produto: {produto} e maquina: {idMaquina}";
                        return Json(new { st, fila, maquinas });
                    }

                    var seqMin = rotPossiveis.Min(x => x.ROT_SEQ_TRANFORMACAO);
                    /* PENDENCIA
                     * Quando a FT possui duas sequencias de transformação para a mesma máquina,
                     * ocorre um bug quando é produzido a primeira sequencia de transformação e 
                     * depois o usuário vai buscar o mesmo pedido na tela do operador, ocorre um
                     * erro, justantemente por conta de ter duas sequencias de tranformação para 
                     * a mesma máquina. Isso deverá ser tratado quando for importante.
                     */


                    var pedFla = db.ViewFilaProducao.AsNoTracking()
                        .Where(x => x.OrdId.Equals(idPedido) && x.PaProId.Equals(produto) && x.RotSeqTransformacao == seqMin && !x.FprStatus.StartsWith("E"))
                        .FirstOrDefault();

                    if (pedFla == null)
                    {
                        st = $"Erro! A OP do pedido: {idPedido} e na maquina: {idMaquina} está encerrada.";
                        return Json(new { st, fila, maquinas });
                    }


                    var _Roteiros = (from rot in db.Roteiro
                                     join fProducao in db.ViewFilaProducao
                                        on rot.PRO_ID equals fProducao.PaProId
                                     where fProducao.OrdId == idPedido || fProducao.ORD_OP_INTEGRACAO == pedFla.OrdId
                                     select new { rot.PRO_ID }).Distinct().ToList();

                    if (_Roteiros.Count == 0)
                    {
                        msg.Append("Não há roteiros assosciados para esta OP nesta Maquina.");
                        flag = false;
                    }
                    if (flag)
                    {
                        //Ops em aberto
                        filaProducao.Add(pedFla);
                        //filaProducao = db.ViewFilaProducao.AsNoTracking()
                        //        .Where(f => (f.RotMaqId == idMaquina ) && f.PaProId == _Roteiros.First().PRO_ID).OrderBy(f => f.FprDataInicioPrevista).ToList();
                        
                    }
                    if (flag && filaProducao.Count > 0)
                    {
                        fila = filaProducao.Select(f => new
                        {
                            op = f.OrdId + f.PaProId + f.RotSeqTransformacao + f.FprSeqRepeticao,
                            opErp = f.ORD_OP_INTEGRACAO,
                            proIntegracao = f.PA_PRO_INTEGRACAO,
                            order = f.OrdId,
                            maqId = f.RotMaqId,
                            maqDescricao = f.MaqDescricao,
                            proId = f.PaProId,
                            proDesc = f.PcProDescricao,
                            dataInicio = f.FprDataInicioPrevista.ToString("dd/MM HH:mm"),
                            dataFim = f.FprDataFimPrevista,
                            produzindo = f.Produzindo,
                            seqTran = f.RotSeqTransformacao,
                            seqRep = f.FprSeqRepeticao,
                            obs = f.FprObsProducao,
                            qtd = f.FprQuantidadePrevista,
                            ordTipo = f.OrdTipo,
                            pecasPulso = f.RotQuantPecasPulso,
                            f.ETI_QUANTIDADE_PALETE,
                            f.ETI_IMPRIMIR_DE,
                            f.ETI_IMPRIMIR_ATE,
                            f.ETI_NUMERO_COPIAS,
                            f.FPR_PRIORIDADE,
                            f.ORD_LOTE_PILOTO,
                            f.CorFila,
                            maqAnterior = db.ViewFilaProducao.AsNoTracking()
                                                            .Where(x => x.OrdId == f.OrdId && x.FprSeqRepeticao == f.FprSeqRepeticao &&
                                                                    x.RotSeqTransformacao < f.RotSeqTransformacao)
                                                            .OrderByDescending(x => x.RotSeqTransformacao)
                                                            .Select(x => new
                                                            {
                                                                maqId = x.RotMaqId,
                                                                maqDescricao = x.MaqDescricao,
                                                                fimPrevisto = x.FprDataFimPrevista.ToString("dd/MM HH:mm")
                                                            }).FirstOrDefault()

                        }).ToList();

                    }
                    if (filaProducao.Count == 0)
                    {
                        msg.Append("O OP não foi encontrada,verifique os dados do Pedido e Produto informados.");
                        flag = false;
                    }
                }
            }
            st = (flag) ? "OK" : msg.ToString();
            return Json(new { st, fila, maquinas });
        }
        public ActionResult ObterProdutos(string pesquisa)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var produtos = db.Produto.Where(x => x.PRO_DESCRICAO.Contains(pesquisa) || x.PRO_ID.Contains(pesquisa)).Take(20)
                .Select(x => new
                {
                    id = x.PRO_ID,
                    descricao = x.PRO_DESCRICAO
                }).ToList();

                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };

                return Json(produtos, settingsJSON);
            }
        }
        #endregion
        #region Feedbacks de performace
        public ActionResult FeedbackPerformace(string ordId, string proId, string maqId, int seqRep, string url)
        {
            TargetProduto target;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var movimento = db.MovimentoEstoque.AsNoTracking()
                    .Include(p => p.Produto).Include(m => m.Maquina)
                    .Where(m => m.ORD_ID == ordId && m.PRO_ID == proId && m.MAQ_ID == maqId && m.FPR_SEQ_REPETICAO == seqRep)
                    .OrderByDescending(m => m.MOV_DATA_HORA_CRIACAO).FirstOrDefault();

                var qtdPerdaProduto = db.MovimentoEstoque.AsNoTracking()
                    .Where(m => m.MAQ_ID == movimento.MAQ_ID &&
                        m.ORD_ID == movimento.ORD_ID && m.PRO_ID == movimento.PRO_ID &&
                        m.FPR_SEQ_TRANFORMACAO == movimento.FPR_SEQ_TRANFORMACAO &&
                        m.FPR_SEQ_REPETICAO == movimento.FPR_SEQ_REPETICAO &&
                        m.TIP_ID == "501")
                    .Select(m => m.MOV_QUANTIDADE).FirstOrDefault();

                var ocorrencias = db.Ocorrencia.AsNoTracking()
                         .Where(o => o.TIP_ID == 3 || o.TIP_ID == 4 || o.TIP_ID == 6).ToList();

                var sltOcoAltaPerform = new List<SelectListItem>();
                var sltOcoBaixaPerform = new List<SelectListItem>();
                var sliOcoOpParcial = new List<SelectListItem>();
                foreach (var o in ocorrencias)
                {
                    if (o.TIP_ID == 3)
                    {
                        sltOcoBaixaPerform.Add(new SelectListItem()
                        {
                            Value = o.OCO_ID,
                            Text = o.OCO_DESCRICAO
                        });
                    }
                    else if (o.TIP_ID == 4)
                    {
                        sltOcoAltaPerform.Add(new SelectListItem()
                        {
                            Value = o.OCO_ID,
                            Text = o.OCO_DESCRICAO
                        });
                    }
                    else if (o.TIP_ID == 6)
                    {
                        sliOcoOpParcial.Add(new SelectListItem()
                        {
                            Value = o.OCO_ID,
                            Text = o.OCO_DESCRICAO
                        });
                    }
                }

                CalcularTargets(db, movimento.MOV_ID);
                //db.Database.ExecuteSqlCommand("EXEC [SP_PLUG_TARGETS] " + movimento.MOV_ID + ",'" + movimento.PRO_ID + "','" + movimento.MAQ_ID + "'");
                
                target = db.TargetProduto.Where(t => t.MOV_ID == movimento.MOV_ID).OrderByDescending(t => t.MOV_ID).Take(1).FirstOrDefault();
                var targetAnterior = new TargetProduto().ObterUltimaMeta(target.MAQ_ID, target.PRO_ID, target.MOV_ID);
                
                //selects list item
                ViewBag.sltOcoAltaPerform = sltOcoAltaPerform;
                ViewBag.sltOcoBaixaPerform = sltOcoBaixaPerform;
                ViewBag.sliOcoOpParcial = sliOcoOpParcial;
                //informações
                ViewBag.maquinaId = movimento.MAQ_ID;
                ViewBag.op = movimento.ORD_ID + movimento.PRO_ID + movimento.FPR_SEQ_TRANFORMACAO + movimento.FPR_SEQ_REPETICAO;
                ViewBag.movimento = movimento;
                
                if (targetAnterior != null)
                {
                    ViewBag.target = new
                    {
                        PerformaceMinAmarelo = targetAnterior.TAR_PERFORMANCE_MIN_AMARELO,
                        PerformaceMinVerde = targetAnterior.TAR_PERFORMANCE_MIN_VERDE,
                        PerformaceMaxVerde = targetAnterior.TAR_PERFORMANCE_MAX_VERDE,
                        SetupMinVerde = targetAnterior.TAR_SETUP_MIN_VERDE,
                        SetupMaxVerde = targetAnterior.TAR_SETUP_MAX_VERDE,
                        SetupMaxAmarelo = targetAnterior.TAR_SETUP_MAX_AMARELO,
                        SetupAjusteMinVerde = targetAnterior.TAR_SETUPA_MIN_VERDE,
                        SetupAjusteMaxVerde = targetAnterior.TAR_SETUPA_MAX_VERDE,
                        SetupAjusteMaxAmarelo = targetAnterior.TAR_SETUPA_MAX_AMARELO,
                        RealizadoPerformace = target.TAR_REALIZADO_PERFORMANCE,
                        ProximaMetaPerformace = targetAnterior.TAR_PROXIMA_META_PERFORMANCE,
                        RealizadoTempoSetup = target.TAR_REALIZADO_TEMPO_SETUP,
                        ProximaMetaTempoSetup = targetAnterior.TAR_PROXIMA_META_TEMPO_SETUP,
                        RealizadoTempoSetupAjuste = target.TAR_REALIZADO_TEMPO_SETUP_AJUSTE,
                        ProximaMetaTempoSetupAjuste = targetAnterior.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE
                    };
                }
            }
            ViewBag.urlAnterior = !string.IsNullOrWhiteSpace(url) ? url : Url.Action("Index");
            return View(target);
        }

        [HttpPost]
        public ActionResult GravarFeedbackPerformace(string targetJSON)
        {
            TargetProduto auxTarget = JsonConvert.DeserializeObject<TargetProduto>(targetJSON);

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            TargetProduto target = null;
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                target = db.TargetProduto.AsNoTracking().Where(t => t.TAR_ID == auxTarget.TAR_ID).FirstOrDefault();

                if (target == null)
                    return Json(new { st = false, msg = $"Target {auxTarget.TAR_ID} não encontrado." }, settingsJSON);

                target.TAR_OBS_PERFORMANCE = auxTarget.TAR_OBS_PERFORMANCE;
                target.TAR_OBS_SETUP = auxTarget.TAR_OBS_SETUP;
                target.TAR_OBS_SETUPA = auxTarget.TAR_OBS_SETUPA;
                target.OCO_ID_PERFORMANCE = auxTarget.OCO_ID_PERFORMANCE;
                target.OCO_ID_SETUP = auxTarget.OCO_ID_SETUP;
                target.OCO_ID_SETUPA = auxTarget.OCO_ID_SETUPA;
                target.PlayAction = "update";
            }

            MasterController mc = new MasterController();

            List<List<object>> objetosParaPersistir = new List<List<object>>();
            objetosParaPersistir.Add(new List<object> { target });

            var logsUpdateData = mc.UpdateData(objetosParaPersistir, 0, true);
            var logsComErro = logsUpdateData.Where(l => l.Status != "OK").ToList();

            if (logsComErro.Count > 0)
            {
                string msgErro = "";
                logsComErro.ForEach(l => msgErro += $"{l.MsgErro}\n");
                return Json(new { st = false, msg = msgErro }, settingsJSON);
            }
            return Json(new { st = true, msg = "" }, settingsJSON);
        }
        #endregion
        #region Fila de Producao
        [HttpGet]
        public ActionResult ObterFilaProducao(string maquina, string equipe)
        {
            string erro = "OK";
            object fila = new List<object>();
            object maquinas = new List<object>();

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    int exibirOPs = 35;
                    List<ViewFilaProducao> filaProducao = new List<ViewFilaProducao>();
                    List<string> status = new List<string>() { "AMOSTRA_COLETADA", "AMOSTRA_NAO_COLETADA" };

                    if (equipe != null)
                    {
                        maquinas = db.Maquina.AsNoTracking().FromSql(
                            "SELECT M.MAQ_ID, M.MAQ_CONGELA_FILA " +
                            "FROM T_MAQUINA M " +
                            "INNER JOIN T_MAQUINAS_EQUIPES ME ON M.MAQ_ID = ME.MAQ_ID " +
                            "WHERE ME.EQU_ID = @EQU_ID",
                            new SqlParameter("@EQU_ID", equipe)
                        ).Select(m => new
                        {
                            m.MAQ_ID,
                            m.MAQ_CONGELA_FILA
                        }).ToList();

                        filaProducao = db.ViewFilaProducao.AsNoTracking()
                            .Where(f => f.EQU_ID == equipe)
                            .Select(f => new ViewFilaProducao
                            {
                                OrdId = f.OrdId,
                                PaProId = f.PaProId,
                                RotSeqTransformacao = f.RotSeqTransformacao,
                                FprSeqRepeticao = f.FprSeqRepeticao,
                                ORD_OP_INTEGRACAO = f.ORD_OP_INTEGRACAO,
                                PA_PRO_INTEGRACAO = f.PA_PRO_INTEGRACAO,
                                RotMaqId = f.RotMaqId,
                                MaqDescricao = f.MaqDescricao,
                                PcProDescricao = f.PcProDescricao,
                                FprDataInicioPrevista = f.FprDataInicioPrevista,
                                FprDataFimPrevista = f.FprDataFimPrevista,
                                Produzindo = f.Produzindo,
                                FprObsProducao = f.FprObsProducao,
                                FprQuantidadePrevista = f.FprQuantidadePrevista,
                                RotQuantPecasPulso = f.RotQuantPecasPulso,
                                OrdTipo = f.OrdTipo,
                                ETI_QUANTIDADE_PALETE = f.ETI_QUANTIDADE_PALETE,
                                ETI_IMPRIMIR_DE = f.ETI_IMPRIMIR_DE,
                                ETI_IMPRIMIR_ATE = f.ETI_IMPRIMIR_ATE,
                                ETI_NUMERO_COPIAS = f.ETI_NUMERO_COPIAS,
                                FPR_PRIORIDADE = f.FPR_PRIORIDADE,
                                TEMPLATE_TESTES = f.TEMPLATE_TESTES,
                                ORD_LOTE_PILOTO = f.ORD_LOTE_PILOTO,
                                CorFila = f.CorFila,
                                PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                                OrdToleranciaMenos = f.OrdToleranciaMenos,
                                GRP_TIPO = f.GRP_TIPO,
                                PcProId = f.PcProId

                            })
                            .OrderBy(f => f.FprDataInicioPrevista).Take(exibirOPs).ToList();
                    }
                    else if (maquina != null)
                    {
                        maquinas = db.Maquina.AsNoTracking().FromSql(
                            "SELECT MAQ_ID, MAQ_CONGELA_FILA " +
                            "FROM T_MAQUINA WHERE MAQ_ID = @MAQ_ID",
                            new SqlParameter("@MAQ_ID", maquina)
                        ).Select(m => new
                        {
                            m.MAQ_ID,
                            m.MAQ_CONGELA_FILA
                        }).ToList();

                        filaProducao = db.ViewFilaProducao.AsNoTracking()
                            .Where(f => f.RotMaqId == maquina)
                            .Select(f => new ViewFilaProducao
                            {
                                OrdId = f.OrdId,
                                PaProId = f.PaProId,
                                RotSeqTransformacao = f.RotSeqTransformacao,
                                FprSeqRepeticao = f.FprSeqRepeticao,
                                ORD_OP_INTEGRACAO = f.ORD_OP_INTEGRACAO,
                                PA_PRO_INTEGRACAO = f.PA_PRO_INTEGRACAO,
                                RotMaqId = f.RotMaqId,
                                MaqDescricao = f.MaqDescricao,
                                PcProDescricao = f.PcProDescricao,
                                FprDataInicioPrevista = f.FprDataInicioPrevista,
                                FprDataFimPrevista = f.FprDataFimPrevista,
                                Produzindo = f.Produzindo,
                                FprObsProducao = f.FprObsProducao,
                                FprQuantidadePrevista = f.FprQuantidadePrevista,
                                RotQuantPecasPulso = f.RotQuantPecasPulso,
                                OrdTipo = f.OrdTipo,
                                ETI_QUANTIDADE_PALETE = f.ETI_QUANTIDADE_PALETE,
                                ETI_IMPRIMIR_DE = f.ETI_IMPRIMIR_DE,
                                ETI_IMPRIMIR_ATE = f.ETI_IMPRIMIR_ATE,
                                ETI_NUMERO_COPIAS = f.ETI_NUMERO_COPIAS,
                                TEMPLATE_TESTES = f.TEMPLATE_TESTES,
                                FPR_PRIORIDADE = f.FPR_PRIORIDADE,
                                ORD_LOTE_PILOTO = f.ORD_LOTE_PILOTO,
                                CorFila = f.CorFila,
                                PrevisaoMateriaPrima = f.PrevisaoMateriaPrima,
                                OrdToleranciaMenos = f.OrdToleranciaMenos,
                                GRP_TIPO = f.GRP_TIPO,
                                PcProId = f.PcProId
                            })
                            .OrderBy(f => f.FprDataInicioPrevista).Take(exibirOPs).ToList();
                    }

                    //Ops pendentes(abertas) {Produzindo}
                    fila = filaProducao.Select(f => new
                    {
                        op = f.OrdId + f.PaProId + f.RotSeqTransformacao + f.FprSeqRepeticao,
                        opErp = f.ORD_OP_INTEGRACAO,
                        proIntegracao = f.PA_PRO_INTEGRACAO,
                        order = f.OrdId,
                        maqId = f.RotMaqId,
                        maqDescricao = f.MaqDescricao,
                        proId = f.PaProId,
                        proDesc = f.PcProDescricao,
                        dataInicio = f.FprDataInicioPrevista.ToString("dd/MM HH:mm"),
                        dataFim = f.FprDataFimPrevista,
                        produzindo = f.Produzindo,
                        seqTran = f.RotSeqTransformacao,
                        seqRep = f.FprSeqRepeticao,
                        obs = f.FprObsProducao,
                        qtd = f.FprQuantidadePrevista,
                        pecasPulso = f.RotQuantPecasPulso,
                        ordTipo = f.OrdTipo,
                        f.ETI_QUANTIDADE_PALETE,
                        f.ETI_IMPRIMIR_DE,
                        f.ETI_IMPRIMIR_ATE,
                        f.ETI_NUMERO_COPIAS,
                        f.TEMPLATE_TESTES,
                        f.FPR_PRIORIDADE,
                        f.ORD_LOTE_PILOTO,
                        f.CorFila,
                        PrevisaoMateriaPrima = f.PrevisaoMateriaPrima.ToString("dd/MM HH:mm"),

                        // Verificar se atingiu a quantidade mínima de produção
                        ATG_TOL_MIN =
                            // movimentos de pré apontamento
                            db.MovimentoEstoque.AsNoTracking()
                                .Any(m => m.TIP_ID == "000" && m.ORD_ID == f.OrdId && m.PRO_ID == f.PaProId && m.MAQ_ID == f.RotMaqId &&
                                    m.FPR_SEQ_TRANFORMACAO == f.RotSeqTransformacao && m.FPR_SEQ_REPETICAO == f.FprSeqRepeticao),

                        maqAnterior = new
                        {
                            maqId = "",
                            maqDescricao = "",
                            inicioPrevisto = "",
                            fimPrevisto = "",
                            saldoProduzir = ""
                        },

                        AmostrasAColetar = db.PlanoAmostralTeste.AsNoTracking().
                            Where(pa => (f.FprQuantidadePrevista >= pa.PAT_QTD_CAIXAS_DE &&
                                f.FprQuantidadePrevista <= pa.PAT_QTD_CAIXAS_ATE) && f.GRP_TIPO == pa.GRP_TIPO)
                            .Select(p => p.PAT_N_AMOSTRAGEM).DefaultIfEmpty(-1).First(),

                        TestesPorColeta = (
                            from ttp in db.TemplateTipoTeste
                            join tt in db.TipoTeste on ttp.TT_ID equals tt.TT_ID
                            where ttp.TEM_ID == f.TEMPLATE_TESTES
                            select new { tt.TT_N_AMOSTRAS_P_TESTE }).Sum(x => x.TT_N_AMOSTRAS_P_TESTE
                        ),

                        TestesRealizados = db.TesteFisico.Count(
                            x => x.ORD_ID.Equals(f.OrdId) &&
                            x.ROT_MAQ_ID.Equals(f.RotMaqId) &&
                            x.FPR_SEQ_REPETICAO == Convert.ToInt32(f.FprSeqRepeticao) &&
                            x.ROT_SEQ_TRANSFORMACAO == Convert.ToInt32(f.RotSeqTransformacao) &&
                            x.ROT_PRO_ID.Equals(f.PaProId) &&
                            status.Contains(x.TES_STATUS_LIBERACAO)
                        ),
                    }).ToList();
                }
                catch (Exception ex)
                {
                    erro += ex.Message;
                    if (ex.InnerException != null)
                    {
                        erro += ex.InnerException.Message;
                    }
                }
            }

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new { fila, maquinas, erro }, settingsJSON);
        }

        [HttpGet]
        public ActionResult ObterTargetsPendentes(string maquina, string equipe)
        {
            string erro = "OK";
            object opsTarPendentes = new List<object>();

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                try
                {
                    //Ops fechadas parcialmente, sem o target                    
                    opsTarPendentes = db.V_TARGET_PENDENTES.AsNoTracking()
                    .Where(x => x.MAQ_ID == maquina)
                    .Select(f => new
                    {
                        op = f.MAQ_ID + f.PRO_ID + f.FPR_SEQ_TRANFORMACAO + f.FPR_SEQ_REPETICAO,
                        order = f.ORD_ID,
                        maqId = f.MAQ_ID,
                        proId = f.PRO_ID,
                        seqTran = f.FPR_SEQ_TRANFORMACAO,
                        seqRep = f.FPR_SEQ_REPETICAO,
                        movId = f.MOV_ID,
                        usuarioNome = f.USE_NOME.Length > 13 ? f.USE_NOME.Substring(0, 12) : f.USE_NOME,
                        dataDaProducao = f.MOV_DATA_HORA_CRIACAO.ToString("dd/MM HH:mm"),
                        turno = f.TURN_ID,
                        qtdProduzida = f.MOV_QUANTIDADE,
                        proDesc = f.PRO_DESCRICAO.Length > 20 ? f.PRO_DESCRICAO.Substring(0, 19) : f.PRO_DESCRICAO,
                    }).ToList();
                }
                catch (Exception ex)
                {
                    erro = ex.InnerException.Message;
                }

                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };
                return Json(new { opsTarPendentes, erro }, settingsJSON);
            }
        }

        [HttpGet]
        public ActionResult ObterOpsProduzidas(string maquina, string equipe)
        {
            string erro = "OK";
            List<V_FILA_PRODUCAO_HISTORICO> filaProducaoHistorico = new List<V_FILA_PRODUCAO_HISTORICO>();
            object filaHistorico = new List<object>();

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                
                    if (equipe != null)
                    {
                        filaProducaoHistorico = db.V_FILA_PRODUCAO_HISTORICO.AsNoTracking()
                            .Where(x => x.TIP_ID == "000" && x.EQU_ID == equipe).OrderByDescending(x => x.MOV_ID).Take(5).ToList();
                    }
                    else if (maquina != null)
                    {
                        filaProducaoHistorico = db.V_FILA_PRODUCAO_HISTORICO.AsNoTracking()
                            .Where(x => x.TIP_ID == "000" && x.ROT_MAQ_ID == maquina).OrderByDescending(x => x.MOV_ID).Take(5).ToList();
                    }

                    filaHistorico = filaProducaoHistorico.Select(f => new
                    {
                        order = f.ORD_ID,
                        maqId = f.ROT_MAQ_ID,
                        proId = f.ROT_PRO_ID,
                        proDesc = f.PRO_DESCRICAO.Length > 20 ? f.PRO_DESCRICAO.Substring(0, 19) : f.PRO_DESCRICAO,
                        dataInicio = f.FPR_DATA_INICIO_PREVISTA,
                        dataFim = f.FPR_DATA_FIM_PREVISTA,
                        seqTran = f.ROT_SEQ_TRANFORMACAO,
                        seqRep = f.FPR_SEQ_REPETICAO,
                        obs = f.FPR_OBS_PRODUCAO,
                        qtd = f.FPR_QUANTIDADE_PREVISTA,
                        qtdPerdas = f.QTD_PERDA,
                        qtdPecasBoas = Math.Abs(Convert.ToDecimal(f.MOV_QUANTIDADE - f.QTD_PERDA)),
                        qtdProduzida = f.MOV_QUANTIDADE,
                        movId = f.MOV_ID,
                        dataDaProducao = f.MOV_DATA_HORA_CRIACAO.ToString("dd/MM HH:mm"),
                        usuarioNome = f.USE_NOME.Length > 13 ? f.USE_NOME.Substring(0, 12) : f.USE_NOME,
                        turno = f.TURN_ID
                    }).ToList();

                
            }
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(new { filaHistorico, erro }, settingsJSON);
        }

        [HttpGet]
        public ActionResult ObterDadosOrdemDeProducao(string ordId, string proId, string seqTran)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int seqTransformacao = 0;
                if (!int.TryParse(seqTran, out seqTransformacao))
                {
                    return null;
                }

                //Busca no banco de dados as estruturas do produto (cliches, facas, tintas e etc)
                var grpIds = (from E in db.EstruturaProduto
                              join P in db.Produto on E.PRO_ID_PRODUTO equals P.PRO_ID
                              join P2 in db.Produto on E.PRO_ID_COMPONENTE equals P2.PRO_ID
                              where P.PRO_ID == proId
                              select new { E.PRO_ID_COMPONENTE, P2.GRP_ID, P2.PRO_DESCRICAO }).ToList();

                //Recupera o PRO_ID_COMPONENTE de todos os cliches da lista, e transforma em uma string concatenada
                string cliches = String.Join(", ", grpIds.Where(x => x.GRP_ID == "CLIC").Select(x => x.PRO_ID_COMPONENTE).ToList());
                string facas = String.Join(", ", grpIds.Where(x => x.GRP_ID == "FACA").Select(x => x.PRO_ID_COMPONENTE).ToList());
                string tintas = String.Join(", ", grpIds.Where(x => x.GRP_ID == "TINT").Select(x => x.PRO_ID_COMPONENTE).ToList());
                string paletes = String.Join(", ", grpIds.Where(x => x.GRP_ID == "PALE").Select(x => x.PRO_DESCRICAO).ToList());

                //Recupera dados do endereçamento e quantidade de paletse por endereço
                var enderecos = (from S in db.SaldosEmEstoquePorLote
                                 join O in db.Order on S.ORD_ID equals O.ORD_ID
                                 where S.ORD_ID == ordId && O.PRO_ID != S.PRO_ID
                                 group S by new { S.MOV_ENDERECO } into G
                                 select new { G.Key.MOV_ENDERECO, QUANTIDADE_PALETES = G.Count() }).ToList();

                //Recupera dados da máquina anterior

                List<V_SALDO_PRODUCAO_DE_OPS> V_SALDO_PRODUCAO_DE_OPS = db.V_SALDO_PRODUCAO_DE_OPS.AsNoTracking()
                                                    .Where(v => v.ORD_ID == ordId).ToList();
                var opAnterior = V_SALDO_PRODUCAO_DE_OPS
                                    .Where(v => v.ORD_ID == ordId && v.ROT_PRO_ID.Equals(proId) && v.ROT_SEQ_TRANFORMACAO < seqTransformacao)
                                    .OrderByDescending(v => v.ROT_SEQ_TRANFORMACAO)
                                    .Select(x => new
                                    {
                                        maqId = x.MAQ_ID,
                                        maqDescricao = x.MAQ_DESCRICAO,
                                        inicioPrevisto = x.FPR_DATA_INICIO_PREVISTA.ToString("dd/MM HH:mm"),
                                        fimPrevisto = x.FPR_DATA_FIM_PREVISTA.ToString("dd/MM HH:mm"),
                                        saldoProduzir = Math.Round((double)x.SALDO_A_PRODUZIR,2)
                                    }).FirstOrDefault();

                if (opAnterior== null)
                {// não encontrou op menor vamos procurar OP chapa
                    opAnterior = V_SALDO_PRODUCAO_DE_OPS
                                    .Where(v => v.ORD_ID == ordId && v.ROT_PRO_ID != proId)
                                    .OrderByDescending(v => v.ROT_SEQ_TRANFORMACAO)
                                    .Select(x => new
                                    {
                                        maqId = x.MAQ_ID,
                                        maqDescricao = x.MAQ_DESCRICAO,
                                        inicioPrevisto = x.FPR_DATA_INICIO_PREVISTA.ToString("dd/MM HH:mm"),
                                        fimPrevisto = x.FPR_DATA_FIM_PREVISTA.ToString("dd/MM HH:mm"),
                                        saldoProduzir = Math.Round((double)x.SALDO_A_PRODUZIR, 2)
                                    }).FirstOrDefault();
                }

                return Json(new { opAnterior, cliches, facas, tintas, paletes, enderecos });
            }
        }

        [HttpGet]
        public ActionResult ObterObterOrdemProducao(string maqId, string proId)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var fila = db.ViewFilaProducao.ToList().Select(f => new
                {
                    op = f.RotMaqId + f.OrdId + f.PaProId + f.RotSeqTransformacao + f.FprSeqRepeticao,
                    maquina = f.OrdId,
                    produto = f.PaProId,
                    dataInicio = f.FprDataInicioPrevista,
                    dataFim = f.FprDataFimPrevista,
                    seqTran = f.RotSeqTransformacao,
                    seqRep = f.FprSeqRepeticao,
                    obs = f.FprObsProducao,
                    quantidade = f.OrdQuantidade,
                    pecasPulso = f.RotQuantPecasPulso
                }).ToList();

                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };

                return Json(fila, settingsJSON);
            }

        }
        [HttpGet]
        public ActionResult ObterObterOrdemProducao(string maqId, string proId, string order, int seqRep, int seqTran, int movId)
        {
            object op = null;
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                op = db.V_FILA_PRODUCAO_HISTORICO.AsNoTracking()
                        .Where(x => x.TIP_ID == "000" && x.ROT_MAQ_ID == maqId && x.ORD_ID == order && x.ROT_PRO_ID == proId &&
                            x.ROT_SEQ_TRANFORMACAO == seqTran && x.FPR_SEQ_REPETICAO == seqRep && x.MOV_ID == movId)
                        .ToList().Select(f => new
                        {
                            order = f.ORD_ID,
                            maqId = f.ROT_MAQ_ID,
                            proId = f.ROT_PRO_ID,
                            proDesc = f.PRO_DESCRICAO.Length > 20 ? f.PRO_DESCRICAO.Substring(0, 19) : f.PRO_DESCRICAO,
                            dataInicio = f.FPR_DATA_INICIO_PREVISTA,
                            dataFim = f.FPR_DATA_FIM_PREVISTA,
                            seqTran = f.ROT_SEQ_TRANFORMACAO,
                            seqRep = f.FPR_SEQ_REPETICAO,
                            obs = f.FPR_OBS_PRODUCAO,
                            qtdPrevista = f.FPR_QUANTIDADE_PREVISTA,
                            qtdProduzida = f.FPR_QTD_PRODUZIDA,
                            movId = f.MOV_ID,
                            dataDaProducao = f.MOV_DATA_HORA_CRIACAO.ToString("dd/MM HH:mm"),
                            usuarioNome = f.USE_NOME.Length > 13 ? f.USE_NOME.Substring(0, 12) : f.USE_NOME,
                            turno = f.TURN_ID,
                            dataFimMaxima = f.FPR_DATA_FIM_MAXIMA
                        }).ToList();
            }

            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };

            return Json(new { dados = op }, settingsJSON);
        }
        /// <summary>
        /// Reverte o processo de apontamento de pré-produçao no painel de máquinas permitindo o reprocessamento desta OP pelo sistema
        /// </summary>
        /// <param name="maquina"></param>
        /// <param name="produto"></param>
        /// <param name="order"></param>
        /// <param name="seqRep"></param>
        /// <param name="seqTran"></param>
        /// <param name="movId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DesfazerOpProduzida(string maquina, string produto, string order, int seqRep, int seqTran, int movId)
        {
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            try
            {
                bool ok = true;
                //busca ids dos feedbacks salvos que estao pendentes
                using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    db.Database.OpenConnection();
                    dynamic feedsPendenteIds;
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                            new TransactionOptions
                            {
                                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted // UTILIZA O NOLOCK
                            }))
                    {

                        feedsPendenteIds = (from f in db.Feedback
                                            join fbmv in db.T_FeedbackMovEstoque on f.Id equals fbmv.FeedbackId into feed
                                            from fb in feed.DefaultIfEmpty()
                                            where fb.FeedbackId == 0 && fb.Feedback.MaquinaId == maquina
                                            select new
                                            {
                                                f.Id
                                            }).ToList();


                    }

                    //ids do movimento tipo = "000"
                    List<int> feedIds = db.T_FeedbackMovEstoque.FromSql($"SELECT FEE_ID FROM T_FEEDBACK_MOV_ESTOQUE (nolock) WHERE MOV_ID = {movId}")
                        .AsNoTracking().Distinct().Select(x => x.FeedbackId).ToList();

                    // Verificar se exite registros na tabela T_CLP_MEDICOES
                    int countCLP = db.ViewClpMedicoes.Count(x => feedIds.Contains(x.FeedBackId.Value));
                    if (countCLP == 0)
                    {
                        return Json(new { status = false, msg = "Erro ao desfazer esta OP. A mesma não possui mais registros de CLP Medições." }, settingsJSON);
                    }

                    //mecla os pendentes com os da OP a ser desfeita
                    foreach (var f in feedsPendenteIds)
                    {
                        feedIds.Add(f.Id);
                    }

                    //ids de todos os movimentos 
                    var movIds = db.T_FeedbackMovEstoque.FromSql(String.Format("SELECT MOV_ID FROM T_FEEDBACK_MOV_ESTOQUE (nolock) WHERE FEE_ID IN ({0})", String.Join(",", feedIds)))
                         .AsNoTracking().Select(x => x.MovimentoEstoqueId).Distinct().ToList();

                    //busca a data da op atual
                    var opAtual = db.ViewFilaProducao
                        .AsNoTracking()
                        .Where(f => f.RotMaqId == maquina)
                        .OrderBy(x => x.FprDataInicioPrevista)
                        .Take(1)
                        .Select(x => new
                        {
                            x.FprDataInicioPrevista,
                            x.RotMaqId,
                            x.OrdId,
                            x.PaProId,
                            x.RotSeqTransformacao,
                            x.FprSeqRepeticao
                        }).FirstOrDefault();

                    if (opAtual.RotMaqId != maquina || opAtual.OrdId != order || opAtual.PaProId != produto
                        || opAtual.FprSeqRepeticao != seqRep || opAtual.RotSeqTransformacao != seqTran)
                    {
                        //abre a op na fila de producao
                        var fila = new FilaProducao()
                        {
                            ROT_MAQ_ID = maquina,
                            ORD_ID = order,
                            ROT_PRO_ID = produto,
                            ROT_SEQ_TRANFORMACAO = seqTran,
                            FPR_SEQ_REPETICAO = seqRep,
                            FPR_STATUS = "",
                            FPR_DATA_INICIO_PREVISTA = opAtual.FprDataInicioPrevista.AddMilliseconds(-11)
                        };

                        db.FilaProducao.Attach(fila);//adiciona ao contexto do entity
                        var entry = db.Entry(fila);

                        entry.Property(f => f.FPR_STATUS).IsModified = true;
                        entry.Property(f => f.FPR_DATA_INICIO_PREVISTA).IsModified = true;
                    }

                    int qtdLinhas = db.V_OPS_A_PLANEJAR.AsNoTracking().FromSql(
                        "select SequenciaTransformacao FROM V_OPS_A_PLANEJAR " +
                        "WHERE LEFT(ORD_STATUS, 1) <> 'E' AND ORD_STATUS <> 'SS' and " +
                        "OrderId = @OrderId and ProdutoId = @ProdutoId and SequenciaRepeticao = @SequenciaRepeticao and " +
                        "SequenciaTransformacao < @SequenciaTransformacao",
                        new SqlParameter("@OrderId", order),
                        new SqlParameter("@ProdutoId", produto),
                        new SqlParameter("@SequenciaRepeticao", seqRep),
                        new SqlParameter("@SequenciaTransformacao", seqTran)
                    ).Count();

                    bool podeExtornarConsumoChapa = qtdLinhas == 0;

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            /* regra 1 se o mov possuir menssagem de apontamento com status ok deve estornar integrção
                             * regra 2 se o mov não possuir a menssagem mas possuir o id de integração deve estornar integrção pois a menssagem deve ter cido limpado pela limpessa de menssagens 
                             * regra 3 se o mov possuir menssagem diferente de ok abortar estorno ate que o apontamento seja realizado
                             * regra 4 se o mov não possuir menssagem e não possuir id de integração 
                             */

                            int resultSql = 0;

                            //ATUALIZA OPS PRODUZINDO {Produzindo}
                            resultSql = db.Database.ExecuteSqlCommand(
                                "UPDATE T_FILA_PRODUCAO SET FPR_PRODUZINDO = 0 WHERE ROT_MAQ_ID = @ROT_MAQ_ID AND FPR_PRODUZINDO = 1;",
                                new SqlParameter("@ROT_MAQ_ID", maquina)
                            );

                            //Extorno consumo de chapa
                            if (podeExtornarConsumoChapa)
                                resultSql = db.Database.ExecuteSqlCommand(
                                    "UPDATE T_MOVIMENTOS_ESTOQUE SET MOV_ESTORNO = 'E' WHERE ORD_ID = @ORD_ID AND TIP_ID = 610;",
                                    new SqlParameter("@ORD_ID", order)
                                );

                            resultSql = db.Database.ExecuteSqlCommand(
                                "UPDATE T_FILA_PRODUCAO SET FPR_PRODUZINDO = 1, FPR_STATUS = '', OCO_ID = NULL, FPR_MOTIVO_PULA_FILA = '', " +
                                "FPR_ORDEM_NA_FILA = ISNULL((SELECT MIN(FPR_ORDEM_NA_FILA)-100 FROM T_FILA_PRODUCAO (NOLOCK) WHERE FPR_ORDEM_NA_FILA < 0 AND ROT_MAQ_ID = @ROT_MAQ_ID), -100.0) " +
                                "WHERE ORD_ID = @ORD_ID AND ROT_PRO_ID = @ROT_PRO_ID AND ROT_MAQ_ID = @ROT_MAQ_ID AND " +
                                "ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO AND FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO;",
                                new SqlParameter("@ROT_MAQ_ID", maquina),
                                new SqlParameter("@ORD_ID", order),
                                new SqlParameter("@ROT_PRO_ID", produto),
                                new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                                new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                            );

                            //gera a string separando os valores com "," para incluir na clausa "in" do sql
                            string sqlInMovIds = string.Join(", ", movIds);
                            string sqlInFeeIds = string.Join(", ", feedIds);

                            //deleta target, movimento estoque e feedback
                            resultSql = db.Database.ExecuteSqlCommand(string.Format("DELETE FROM T_TARGET_PRODUTO WHERE MOV_ID IN ({0})", sqlInMovIds));
                            resultSql = db.Database.ExecuteSqlCommand(string.Format("DELETE FROM T_FEEDBACK_MOV_ESTOQUE WHERE FEE_ID IN ({0})", sqlInFeeIds));
                            resultSql = db.Database.ExecuteSqlCommand(string.Format("DELETE FROM T_MOVIMENTOS_ESTOQUE WHERE MOV_ID IN ({0})", sqlInMovIds));
                            resultSql = db.Database.ExecuteSqlCommand(string.Format("DELETE FROM T_FEEDBACK WHERE FEE_ID IN ({0})", sqlInFeeIds));

                            // Atualizando a qtd produzida 
                            string scriptAtualizarOP = ObterScriptSQLQtdProduzidaOP();
                            resultSql = db.Database.ExecuteSqlCommand(
                                scriptAtualizarOP,
                                new SqlParameter("@ORD_ID", order),
                                new SqlParameter("@ROT_PRO_ID", produto),
                                new SqlParameter("@ROT_MAQ_ID", maquina),
                                new SqlParameter("@ROT_SEQ_TRANFORMACAO", seqTran),
                                new SqlParameter("@FPR_SEQ_REPETICAO", seqRep)
                            );

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            return (Json(new { status = false, msg = "Erro ao estornar feedbacks. " + e.Message }, settingsJSON));
                        }
                    }
                }
                return Json(new { status = ok }, settingsJSON);
            }
            catch (Exception e)
            {
                return Json(new { status = false, msg = "Erro ao estornar feedbacks. " + e.Message.ToString() }, settingsJSON);
            }
        }
        /// <summary>
        /// Verifica se foram realizados os feedbacks de produção antes do encerramento de uma produção.
        /// </summary>
        /// <param name="maquina"></param>
        /// <param name="produto"></param>
        /// <param name="order"></param>
        /// <param name="seqRep"></param>
        /// <param name="seqTran"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerificarSeOpTemFeedback(string maquina, string produto, string order, int seqRep, int seqTran)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                int cont = db.ViewClpMedicoes.AsNoTracking().Where(f => f.MaquinaId == maquina && f.FeedBackId != null && f.FeedBackIdMov == -1).Count();
                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };
                return Json(cont > 0 ? true : false, settingsJSON);
            }
        }

        /// <summary>
        /// Verifica se os apontamentos de produção estão seguindo a sequencia de transformação do produto
        /// para impedir que apontamentos sejam realizados fora d sequência planejada pelo roteiro.
        /// </summary>
        /// <param name="maquina"></param>
        /// <param name="produto"></param>
        /// <param name="order"></param>
        /// <param name="seqRep"></param>
        /// <param name="seqTran"></param>
        /// <returns></returns>
        public ActionResult VerificarUltimaSeqProduzida(string maquina, string produto, string order, int seqRep, int seqTran)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                string msg = "OK";

                /* Consulta a sequência de transformação anterior, e caso exista, 
                 * verifica se a mesma possui movimento de apontamento de produção (000).
                 * Se existir uma sequência de transformação anterior e não houver 
                 * movimento apontamento de produção para a mesma, a sequência atual não poderá ser apontada.
                 */
                var OpSequenciaAnterior = (from fp in db.ViewFilaProducao
                                           join mv in db.MovimentoEstoque
                                           on
                                              new
                                              {
                                                  ORD_ID = fp.OrdId,
                                                  PRO_ID = fp.PaProId,
                                                  FPR_SEQ_REPETICAO = fp.FprSeqRepeticao,
                                                  seqTranMenor = fp.RotSeqTransformacao,
                                                  movProducao = true
                                              }
                                            equals
                                              new
                                              {
                                                  mv.ORD_ID,
                                                  mv.PRO_ID,
                                                  FPR_SEQ_REPETICAO = mv.FPR_SEQ_REPETICAO.Value,
                                                  seqTranMenor = mv.FPR_SEQ_TRANFORMACAO.Value,
                                                  movProducao = mv.TIP_ID == "000"
                                              }
                                           into juncao
                                           from leftMovEstoque in juncao.DefaultIfEmpty() // left join
                                           where fp.OrdId == order && fp.PcProId == produto && fp.RotSeqTransformacao < seqTran
                                           orderby fp.RotSeqTransformacao descending
                                           select new
                                           {
                                               fp.RotSeqTransformacao,
                                               movQtd = leftMovEstoque == null ? -1 : leftMovEstoque.MOV_QUANTIDADE
                                           }
                                        ).FirstOrDefault();

                if (OpSequenciaAnterior != null)
                {
                    bool possuiMovApontamento = OpSequenciaAnterior.movQtd > 0;
                    if (!possuiMovApontamento)
                    {
                        msg = $"Operação inválida, você deve primeiro apontar a produção da sequencia de transformação anterior: [{OpSequenciaAnterior.RotSeqTransformacao}]!";
                    }
                }

                return Json(new { msg });
            }
        }

        [HttpGet]
        public ActionResult SetProduzindo(string maquina, string produto, string order, int seqRep, int seqTran)
        {
            UtilPlay.SetProduzindo(maquina, produto, order, seqRep, seqTran);
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            return Json(true, settingsJSON);
        }

        #endregion
        #region Monitoramento em tempo real
        [HttpGet]
        public ActionResult ObterTargetProduto(string maqId)
        {
            object opInfoJson = null, targetJson = null;
            //rever view fila {Produzindo}

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var op = db.ViewFilaProducao.AsNoTracking()
                    .Where(f => f.RotMaqId == maqId && f.Produzindo == 1).OrderBy(f => f.OrdemDaFila).Take(1).FirstOrDefault();
                if (op != null)
                {
                    opInfoJson = new
                    {
                        qtdProgramada = op.FprQuantidadePrevista,
                        op = op.OrdId + op.PaProId + op.RotSeqTransformacao + op.FprSeqRepeticao,
                        opErp = op.ORD_OP_INTEGRACAO,
                        cliente = op.CliNome
                    };
                    
                    var target = new TargetProduto().ObterUltimaMeta(op.RotMaqId, op.PaProId, null);
                    if (target != null)
                    {
                        targetJson = new
                        {
                            tarSetup = target.TAR_PROXIMA_META_TEMPO_SETUP ?? 0,
                            tarSetpuAjuste = target.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE ?? 0,
                            tarPerformace = target.TAR_PROXIMA_META_PERFORMANCE ?? 0,
                            performaceMaxVerde = target.TAR_PERFORMANCE_MAX_VERDE ?? 0,
                            performaceMinVerde = target.TAR_PERFORMANCE_MIN_VERDE ?? 0,
                            setupMaxVerde = target.TAR_SETUP_MAX_VERDE ?? 0,
                            setupMinVerde = target.TAR_SETUP_MIN_VERDE ?? 0,
                            setupAjusteMaxVerde = target.TAR_SETUPA_MAX_VERDE ?? 0,
                            setupAjusteMinVerde = target.TAR_SETUPA_MIN_VERDE ?? 0,
                            setupMaxAmarelo = target.TAR_SETUP_MAX_AMARELO ?? 0,
                            setupAjusteMaxAmarelo = target.TAR_SETUPA_MAX_AMARELO == null ? 0 : target.TAR_SETUPA_MAX_AMARELO,
                            performaceMinAmarelo = target.TAR_PERFORMANCE_MIN_AMARELO ?? 0,
                            aprovado = target.TAR_APROVADO
                        };
                    }
                }
                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };
                return Json(new { status = true, op = opInfoJson, target = targetJson }, settingsJSON);
            }


        }
        public ActionResult ObterStatusMaquina(string maquinaId/*, double qtdProgramada, double metaQtdSegundo*/)
        {
            //rever view fila {Produzindo}
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {

                UsuarioSingleton usuarioSingleton = UsuarioSingleton.Instance;
                T_Usuario usuario = usuarioSingleton.ObterUsuario(ObterUsuarioLogado().USE_ID);

                List<T_PREFERENCIAS> pref = usuario.T_PREFERENCIAS.ToList();
                string fat_tempo = "SEGUNDO";
                string un_tempo = "SEGUNDO";
                string usuario_logado = usuario.USE_NOME + "_" + usuario.USE_ID;
                int i = 0;
                i = 0;
                while (i < pref.Count && !pref[i].PRE_TIPO.Equals("FAT_TEMPO"))
                    i++;

                if (i < pref.Count)
                    fat_tempo = pref[i].PRE_VALOR;

                i = 0;
                while (i < pref.Count && !pref[i].PRE_TIPO.Equals("UN_TEMPO"))
                    i++;

                if (i < pref.Count)
                    un_tempo = pref[i].PRE_VALOR;

                var dados = db.ViewFilaProducao.AsNoTracking()
                    .Where(f => f.RotMaqId == maquinaId && f.Produzindo == 1).OrderBy(f => f.OrdemDaFila).Take(1).Select(f => new
                    {

                        QtdPulsos = f.RotQuantPecasPulso,
                        tempoDecorrido = BaseController.ConversorTempo(Convert.ToDouble(f.FprTempoDecorridoPerformacace) + 
                            Convert.ToDouble(f.FprTempoDecorridoSetup) + 
                            Convert.ToDouble(f.FprTempoDecorridoSetupAjuste) + 
                            Convert.ToDouble(f.TempoDecorridoPequenasParadas), un_tempo),
                        qtdPecasProduzidas = f.FprQuantidadeProduzida,
                        qtdPecasRestante = f.FprQuantidadeRestante,
                        vlcAtualPcSegundoString = BaseController.ConversorUnidades(Convert.ToDouble(f.FprVeloAtuPcSegundo), fat_tempo),
                        segunPerfNecessariaString = BaseController.ConversorUnidades(Convert.ToDouble(f.FprVelocidadeAtingirMeta), fat_tempo),
                        
                        segunPerfDecorrida = f.FprTempoDecorridoPerformacace,
                        segunTempoRestante = f.FprTempoRestantePerformace,
                        tempoGastoPerformace = f.FprTempoDecorridoPerformacace,
                        tempoDecorridoSetup = f.FprTempoDecorridoSetup,
                        tempoDecorridoSetupA = f.FprTempoDecorridoSetupAjuste,
                        tempoGastoSetup = f.FprTempoDecorridoSetup + f.FprTempoDecorridoSetupAjuste,
                        segunPerfNecessaria = f.FprVelocidadeAtingirMeta,
                        percProjVeloc = f.FprPerformaceProjetada ?? 0,
                        tempoDecorridoPequenasParadas = f.TempoDecorridoPequenasParadas,
                        vlcAtualPcSegundo = f.FprVeloAtuPcSegundo,
                    }).FirstOrDefault();
                JsonSerializerSettings settingsJSON = new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                };
                return Json(new { dados = dados }, settingsJSON);
            }
        }
        #endregion
        #region Qualidade
        //--Teste Fisico
        public ActionResult ObterTiposTestesNAmostras(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string FPR_SEQ_REPETICAO, string ROT_SEQ_TRANSFORMACAO)
        {
            //busca o numero de amostras e tipo teste apenas
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                List<TipoTeste> tipo = new List<TipoTeste>();
                double numerominamostras = 1;
                int _TemplateTestes = 0;
                bool flag = true;
                if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID))
                {
                    flag = false;
                }
                if (flag)
                {
                    _TemplateTestes = (from pro in db.Produto
                                       join grp in db.GrupoProduto on pro.GRP_ID equals grp.GRP_ID
                                       where pro.PRO_ID.Equals(ROT_PRO_ID)
                                       select grp.TEM_ID.Value).FirstOrDefault();

                    if (_TemplateTestes == 0 && !String.IsNullOrEmpty(ROT_MAQ_ID) && !String.IsNullOrEmpty(ROT_SEQ_TRANSFORMACAO))
                    {
                        var roteiro = db.Roteiro.AsNoTracking()
                            .Where(x => x.PRO_ID.Equals(ROT_PRO_ID) && x.MAQ_ID.Equals(ROT_MAQ_ID) && x.ROT_SEQ_TRANFORMACAO == Convert.ToInt32(ROT_SEQ_TRANSFORMACAO))
                            .Select(x => x.TEM_ID.Value).FirstOrDefault();
                        _TemplateTestes = roteiro;
                    }
                    if (_TemplateTestes == 0 && !String.IsNullOrEmpty(ROT_MAQ_ID) && !String.IsNullOrEmpty(ROT_SEQ_TRANSFORMACAO))
                    {
                        _TemplateTestes = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID.Equals(ROT_MAQ_ID)).Select(x => x.TEM_ID.Value).FirstOrDefault();
                    }
                    tipo = (from ttt in db.TemplateTipoTeste
                            join tipoTeste in db.TipoTeste on ttt.TT_ID equals tipoTeste.TT_ID
                            select tipoTeste).AsNoTracking().ToList();
                    TesteFisico testefisico = new TesteFisico();
                    if (!String.IsNullOrEmpty(ROT_MAQ_ID) && !String.IsNullOrEmpty(ROT_SEQ_TRANSFORMACAO))
                        numerominamostras = testefisico.NumeroMininoColetas(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO).Value;
                }
                return Json(new { tipo, numerominamostras });
            }
        }

        public void ColetarTestesFisicos(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string FPR_SEQ_REPETICAO, string ROT_SEQ_TRANSFORMACAO, bool Coletado, string Turno, string Turma, int User, string Obs, int TEM_ID)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                TesteFisico testeFisico = new TesteFisico();
                testeFisico.GerarAmostrasTesteFisico(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO, Turno, Turma, User, Obs, Coletado, TEM_ID.ToString());
            }
        }

        /// <summary>
        /// Retorna o nº total de testes que foram realizados para esta op
        /// </summary>
        /// <param name="ORD_ID"></param>
        /// <param name="ROT_PRO_ID"></param>
        /// <param name="ROT_MAQ_ID"></param>
        /// <param name="ROT_SEQ_TRANSFORMACAO"></param>
        /// <param name="FPR_SEQ_REPETICAO"></param>
        /// <returns></returns>
        public int QuantidadeTesteSalvo(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string ROT_SEQ_TRANSFORMACAO, string FPR_SEQ_REPETICAO)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                List<string> status = new List<string>() { "AMOSTRA_COLETADA", "AMOSTRA_NAO_COLETADA" };
                return db.TesteFisico.Count(x => x.ORD_ID.Equals(ORD_ID) &&
                                                   x.ROT_MAQ_ID.Equals(ROT_MAQ_ID) &&
                                                   x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO) &&
                                                   x.ROT_SEQ_TRANSFORMACAO == Convert.ToInt32(ROT_SEQ_TRANSFORMACAO) &&
                                                   x.ROT_PRO_ID.Equals(ROT_PRO_ID) &&
                                                   status.Contains(x.TES_STATUS_LIBERACAO)
                                                   );
            }
        }


        public JsonResult ConsultarTemplateOP(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string FPR_SEQ_REPETICAO, string ROT_SEQ_TRANSFORMACAO)
        {
            StringBuilder msg = new StringBuilder();
            TemplateDeTestes _Tt = new TemplateDeTestes();
            string st;
            try
            {
                int TEM_ID = _Tt.GetTemplateOPProduzindo(ORD_ID, ROT_PRO_ID, ROT_MAQ_ID, FPR_SEQ_REPETICAO, ROT_SEQ_TRANSFORMACAO);
                msg.Append("OK");
                st = msg.ToString();
                return Json(new { st, TEM_ID });

            }
            catch (Exception ex)
            {
                msg.Append(ex.Message);
                if (ex.InnerException != null)
                    msg.Append(ex.InnerException.Message);
                st = msg.ToString();
                return Json(new { st });
            }
        }
        #endregion
        #region Uteis
        private object ConvertToAjaxMedicoes(List<ViewClpMedicoes> medicoes)
        {
            return medicoes != null ? medicoes.Select(m => new
            {
                MaquinaId = m.MaquinaId,
                DataIni = m.DataIni.ToString(),
                DataFim = m.DataFim.ToString(),
                Quantidade = m.Quantidade,
                Grupo = m.Grupo,
                TurmaId = !string.IsNullOrEmpty(m.TurmaId) ? m.TurmaId : "",
                TurnoId = !string.IsNullOrEmpty(m.TurnoId) ? m.TurnoId : "",
                Observacoes = !string.IsNullOrEmpty(m.FeedbackObs) ? m.FeedbackObs : "",
                MedicaoId = m.FeedBackId ?? 0,
                OcorrenciaId = !string.IsNullOrEmpty(m.OcoId) ? m.OcoId : "",
                Clp_Origem = !string.IsNullOrEmpty(m.Clp_Origem) ? m.Clp_Origem : "",
            }).ToList() : null;
        }
        #endregion
        #region CLP Manual
        [HttpPost]
        public JsonResult CreateCLPManual(string Dados)
        {
            dynamic _Dados = JsonConvert.DeserializeObject<dynamic>(Dados);
            string MaqId = _Dados.MaqId;
            string EquId = _Dados.EquId;

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime PrimaData = new DateTime(1900, 1, 1, 7, 0, 0);
                DateTime dtAux = db.ClpMedicoes.AsNoTracking()
                   .Where(c => c.MaquinaId.Equals(MaqId)).Select(c => c.DataFim)
                   .DefaultIfEmpty(PrimaData).Max();
                //--
                ViewBag.Maq = MaqId;
                ViewBag.Equ = EquId;
                ViewBag.DataInicio = dtAux.ToString();
                ViewBag.DataEmissao = DateTime.Now.ToString(); ;

                double? grupoCLP = db.ClpMedicoes.AsNoTracking()
                                        .Where(c => c.MaquinaId == MaqId).Select(c => c.Grupo).DefaultIfEmpty(0).Max();
                double grupoFeedBack = db.Feedback.AsNoTracking()
                                        .Where(f => f.MaquinaId == MaqId).Select(f => f.Grupo).DefaultIfEmpty(0).Max();

                double? Grupo = (grupoCLP >= grupoFeedBack) ? grupoCLP : grupoFeedBack;
                Grupo += 1;
                //double? Grupo = (db.ClpMedicoes.Where(c => c.MaquinaId == MaqId).Select(c => c.Grupo).DefaultIfEmpty(0).Max()) + 1;
                string Origem = "M";

                ViewBag.Grupo = Grupo;
                ViewBag.Origem = 'M';
                //--
                int FlagData = (dtAux == PrimaData) ? 0 : 1;
                string DataInicio = dtAux.ToString();
                string DataEmissao = DateTime.Now.ToString();

                string st = "OK";
                return Json(new { st, DataInicio, FlagData, DataEmissao, Grupo, Origem });
            }
        }

        [HttpPost]
        public JsonResult InsertCLPManual(string Dados)
        {
            string msg = "";
            DateTime PrimaData = new DateTime(1900, 1, 1, 7, 0, 0);
            dynamic _Dados = JsonConvert.DeserializeObject<dynamic>(Dados);
            string MaquinaId = _Dados.MaquinaId;
            double Quantidade = _Dados.Quantidade;
            string _InicioAux = _Dados.DataInicio;
            string _FimAux = _Dados.DataFim;
            string Turno = "";
            string Turma = "";
            string Ocorrencia = "";
            int IdLoteClp = 0;
            double? Grupo = _Dados.Grupo;
            DateTime DataEmissao = DateTime.Now;
            //1- Setup / 3-Produzindo
            int Fase = (Quantidade <= 0) ? 1 : 3;
            int Status = 0;
            DateTime DataInicio = Convert.ToDateTime(_InicioAux);
            DateTime DataFim = Convert.ToDateTime(_FimAux);

            if (DataFim < DataInicio)
            {
                msg = "A data/hora de fim não pode ser menor que a data/hora de início";
                return Json(new { st = msg });
            }
            if (DataFim > DateTime.Now)
            {
                msg = "A data/hora de fim não pode ser maior que a data/hora atual";
                return Json(new { st = msg });
            }

            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                ViewClpMedicoes ultimoCLP = db.ViewClpMedicoes.Where(x => x.MaquinaId == MaquinaId && x.DataFim > DataInicio).OrderByDescending(x => x.DataFim)
                .Select(x => new ViewClpMedicoes
                {
                    DataFim = x.DataFim
                }).FirstOrDefault();

                if (ultimoCLP != null)
                {
                    msg = $"A data de início deste apontamento deve ser maior ou igual a data/hora fim do último apontamento - {ultimoCLP.DataFim:dd/MM/yyyy HH:mm}.";
                    return Json(new { st = msg });
                }

                DateTime dtAux = db.ClpMedicoes.AsNoTracking()
                    .Where(c => c.MaquinaId == MaquinaId).Select(c => c.DataFim)
                    .DefaultIfEmpty(PrimaData).Max();

                ViewBag.FalgDate = (dtAux.Equals(PrimaData) ? 0 : 1);
                ViewBag.Maq = MaquinaId;
                ViewBag.DataInicio = dtAux.ToString();
                ViewBag.DataEmissao = DateTime.Now.ToString();
                ViewBag.Grupo = Grupo;
                ViewBag.Origem = 'M';


                ClpMedicoes _CLPMedicoes = new ClpMedicoes()
                {
                    Id = 0,
                    MaquinaId = MaquinaId,
                    DataInicio = DataInicio,
                    DataFim = DataFim,
                    Quantidade = Quantidade,
                    Grupo = Grupo,
                    Status = Status,
                    TurnoId = Turno,
                    TurmaId = Turma,
                    OcorrenciaId = Ocorrencia,
                    IdLoteClp = IdLoteClp,
                    Fase = Fase,
                    Emissao = DataEmissao,
                    ClpOrigem = "M",
                    PlayAction = "insert"
                };
                List<object> listaItem = new List<object>() { _CLPMedicoes };
                List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                MasterController mc = new MasterController();
                List<LogPlay> Logs = mc.UpdateData(ListOfListObjects, 0, true);
                var logsErros = Logs.Where(l => l.Status == "ERRO").ToList();
                if (logsErros.Count > 0)
                {
                    StringBuilder msgErro = new StringBuilder();
                    logsErros.ForEach(l => { msgErro.Append(l.MsgErro + "\n"); });
                    throw new Exception(msgErro.ToString());
                }

                //int? _TipoContador = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID == MaquinaId).Select(x => x.MAQ_TIPO_CONTADOR).FirstOrDefault();
                //if (_TipoContador == 3)
                //{
                //    /* 30/10/2020
                //     * Desativei a execução da procedure para os apontamento de pulsos manuais.
                //     * Essa procedure foi implementada no PlaySchedule, e não terá mais utilidade nos 
                //     * apontamentos de pulso manuais.
                //     * 
                //     * Thiago Luz.
                //     */

                //    //float proximaMetaPerformance = -1;
                //    //string sql = $"EXEC SP_PLUG_CALCULA_PERFORMANCE_FILA -1, '{MaquinaId}', '', '', 0, '', '', 0, {proximaMetaPerformance}, '', '', {Quantidade.ToString(CultureInfo.InvariantCulture)}, {Fase}";
                //    //int linhasAlteradas = db.Database.ExecuteSqlCommand(sql);
                //}

                return Json(new { st = (msg == "") ? "OK" : msg });
            }
        }
        #endregion CLP Manual

        #region Target
        private void CalcularTargets(JSgi db, int movId)
        {
            int linhasAlteradas = 0;
            string sql = "";

            MovimentoEstoque movEstoque = db.MovimentoEstoque.AsNoTracking().FromSql(
                "SELECT ORD_ID, PRO_ID, MAQ_ID, FPR_SEQ_TRANFORMACAO, FPR_SEQ_REPETICAO " +
                "FROM T_MOVIMENTOS_ESTOQUE WHERE MOV_ID = @MOV_ID",
                new SqlParameter("@MOV_ID", movId)
            ).Select(m => new MovimentoEstoque
            {
                MOV_ID = movId,
                ORD_ID = m.ORD_ID,
                PRO_ID = m.PRO_ID,
                MAQ_ID = m.MAQ_ID,
                FPR_SEQ_TRANFORMACAO = m.FPR_SEQ_TRANFORMACAO,
                FPR_SEQ_REPETICAO = m.FPR_SEQ_REPETICAO
            }).FirstOrDefault();

            int qtdMov = db.MovimentoEstoque.AsNoTracking().FromSql(
                "SELECT M.MOV_ID " +
                "FROM  T_MOVIMENTOS_ESTOQUE M " +
                "INNER JOIN [dbo].[T_FEEDBACK_MOV_ESTOQUE] FM ON M.MOV_ID = FM.MOV_ID " +
                "INNER JOIN [dbo].[T_FEEDBACK] F ON F.FEE_ID = FM.FEE_ID " +
                "INNER JOIN T_CLP_MEDICOES C ON C.GRUPO = F.FEE_GRUPO AND C.MAQUINA_ID = F.MAQ_ID " +
                "WHERE M.MOV_ID = @MOV_ID",
                new SqlParameter("@MOV_ID", movId)
            ).Count();

            /* VERIFICA SE EXISTE O CLP MEDIÇÃO DESTE MOVIMENTO. SE NÃO ENCONTRAR NÃO FAZ NADA */
            if (qtdMov <= 0)
                return;

            var feedbacksId = db.Feedback.AsNoTracking().FromSql(
                "SELECT F.FEE_ID " +
                "FROM T_FEEDBACK F " +
                "LEFT JOIN T_FEEDBACK_MOV_ESTOQUE FM ON F.FEE_ID = FM.FEE_ID " +
                "WHERE FEE_GRUPO = -1 AND (FM.FEE_ID IS NULL OR FM.MOV_ID = @MOV_ID)",
                new SqlParameter("@MOV_ID", movId)
            ).Select(f => f.Id).ToList();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    /* APAGA TODOS OS FEEDBACKS SELECIONADOS
                     * ISSO É NECESSÁRIO PORQUE AS VEZES UM MOVIMENTO DE ESTOQUE É DELETADO PARA QUE SEJA REFEITO
                     */
                    if (feedbacksId.Count > 0)
                    {
                        linhasAlteradas = db.Database.ExecuteSqlCommand($"DELETE FROM T_FEEDBACK_MOV_ESTOQUE WHERE FEE_ID IN ({String.Join(", ", feedbacksId)});");
                        linhasAlteradas = db.Database.ExecuteSqlCommand($"DELETE FROM T_FEEDBACK WHERE FEE_ID IN ({String.Join(", ", feedbacksId)});");
                    }

                    /* APAGA OS TARGETS DO DESTE MOVIMENTO */
                    linhasAlteradas = db.Database.ExecuteSqlCommand($"DELETE FROM T_TARGET_PRODUTO WHERE MOV_ID = {movId};");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            /* Metas de setup */
            var metasSetup = db.FilaProducao.AsNoTracking().FromSql(
                "SELECT TOP 1 FPR_META_SETUP " +
                "FROM [dbo].[T_FILA_PRODUCAO] " +
                "WHERE ORD_ID = @ORD_ID AND ROT_PRO_ID = @ROT_PRO_ID AND ROT_MAQ_ID = @ROT_MAQ_ID AND " +
                "ROT_SEQ_TRANFORMACAO = @ROT_SEQ_TRANFORMACAO AND FPR_SEQ_REPETICAO = @FPR_SEQ_REPETICAO ",
                new SqlParameter("@ORD_ID", movEstoque.ORD_ID),
                new SqlParameter("@ROT_PRO_ID", movEstoque.PRO_ID),
                new SqlParameter("@ROT_MAQ_ID", movEstoque.MAQ_ID),
                new SqlParameter("@ROT_SEQ_TRANFORMACAO", movEstoque.FPR_SEQ_TRANFORMACAO),
                new SqlParameter("@FPR_SEQ_REPETICAO", movEstoque.FPR_SEQ_REPETICAO)
            ).Select(f => new FilaProducao
            {
                FPR_META_SETUP = f.FPR_META_SETUP ?? 0
            }).FirstOrDefault();

            /* Metas de setup ajute */
            var metasSetupAjuste = db.TargetProduto.AsNoTracking().FromSql(
                "SELECT TOP 1 TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE, TAR_SETUPA_MAX_VERDE, TAR_SETUPA_MIN_VERDE " +
                "FROM [dbo].[T_TARGET_PRODUTO] " +
                "WHERE ISNULL(MOV_ID, 0) < @MOV_ID AND PRO_ID = @PRODUTO AND MAQ_ID = @MAQUINA AND " +
                "TAR_REALIZADO_TEMPO_SETUP_AJUSTE > 0  AND LEFT(TAR_APROVADO,1) = 'A' " +
                "ORDER BY MOV_ID DESC",
                new SqlParameter("@MOV_ID", movId),
                new SqlParameter("@PRODUTO", movEstoque.PRO_ID),
                new SqlParameter("@MAQUINA", movEstoque.MAQ_ID)
            ).Select(t => new
            {
                metaMASetupa = t.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE,
                MetaSetupa_MAX_VERDE = t.TAR_SETUPA_MAX_VERDE,
                MetaSetupa_MIN_VERDE = t.TAR_SETUPA_MIN_VERDE
            }).FirstOrDefault();

            /* Metas de performance */
            var metasPerformance = db.TargetProduto.AsNoTracking().FromSql(
                "SELECT TOP 1 TAR_PROXIMA_META_PERFORMANCE, TAR_PERFORMANCE_MAX_VERDE, TAR_PERFORMANCE_MIN_VERDE " +
                "FROM [dbo].[T_TARGET_PRODUTO] " +
                "WHERE ISNULL(MOV_ID, 0) < @MOV_ID AND PRO_ID = @PRODUTO AND MAQ_ID = @MAQUINA AND " +
                "TAR_REALIZADO_PERFORMANCE > 0  AND LEFT(TAR_APROVADO,1) = 'A' " +
                "ORDER BY MOV_ID DESC",
                new SqlParameter("@MOV_ID", movId),
                new SqlParameter("@PRODUTO", movEstoque.PRO_ID),
                new SqlParameter("@MAQUINA", movEstoque.MAQ_ID)
            ).Select(t => new
            {
                metaMAPerformance = t.TAR_PROXIMA_META_PERFORMANCE,
                MetaPerformance_MAX_VERDE = t.TAR_PERFORMANCE_MAX_VERDE,
                MetaPerformance_MIN_VERDE = t.TAR_PERFORMANCE_MIN_VERDE
            }).FirstOrDefault();

            /* Insere os movimentos de estoque que não estão na tabela de target */
            linhasAlteradas = db.Database.ExecuteSqlCommand(
                "INSERT INTO [T_TARGET_PRODUTO] " +
                "(MOV_ID, ORD_ID, PRO_ID, MAQ_ID, UNI_ID, TURM_ID, TURN_ID, TAR_DIA_TURMA, TAR_DIA_TURMA_D, TAR_META_PERFORMANCE,TAR_REALIZADO_PERFORMANCE, " +
                "TAR_PROXIMA_META_PERFORMANCE, TAR_META_TEMPO_SETUP, TAR_REALIZADO_TEMPO_SETUP, TAR_PROXIMA_META_TEMPO_SETUP, " +
                "TAR_META_TEMPO_SETUP_AJUSTE, TAR_REALIZADO_TEMPO_SETUP_AJUSTE, TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE, OCO_ID_PERFORMANCE, " +
                "TAR_OBS_PERFORMANCE, OCO_ID_SETUP, TAR_OBS_SETUP, OCO_ID_SETUPA, TAR_OBS_SETUPA, USE_ID, ROT_SEQ_TRANFORMACAO, FPR_SEQ_REPETICAO, TAR_APROVADO) " +

                "SELECT DISTINCT " +
                "M.MOV_ID, M.ORD_ID, M.PRO_ID, M.MAQ_ID, NULL, NULL, NULL, dbo.DIATURMA(MOV_DATA_HORA_EMISSAO), dbo.DIATURMA_D(MOV_DATA_HORA_EMISSAO), 0, 0, " +
                "0, 0, 0, 0, 0, 0, 0, NULL, '', NULL, '', NULL, '', NULL, F.ROT_SEQ_TRANFORMACAO, F.FPR_SEQ_REPETICAO, 'PG' " +

                "FROM [dbo].[T_MOVIMENTOS_ESTOQUE] M " +
                "INNER JOIN [dbo].[T_FEEDBACK_MOV_ESTOQUE] FM ON M.MOV_ID = FM.MOV_ID " +
                "INNER JOIN [dbo].[T_FEEDBACK] F ON F.FEE_ID = FM.FEE_ID " +
                "LEFT JOIN [dbo].[T_TARGET_PRODUTO] T ON M.MOV_ID = T.MOV_ID " +
                "WHERE T.MOV_ID IS NULL AND M.MOV_ID = @MOV_ID;",
                new SqlParameter("@MOV_ID", movId)
            );

            /* CALCULA AS FASES DAS MEDICÕES  */
            List<SetFaseFinal_ClpMedicoes> medicoes = SetFasesFinal(db, movId, movEstoque.PRO_ID, movEstoque.MAQ_ID, movEstoque.FPR_SEQ_TRANFORMACAO.Value);

            double tarQtdSetupAjuste = medicoes.Where(m => m.Fase == 1 || m.Fase == 2).Sum(m => m.Quantidade * m.QuantidadePecasPorPulso) ?? 0; // QTD PRODUZIDA NA FASE 1 E 2.
            double tarQtd = medicoes.Where(m => m.Fase == 3).Sum(m => m.Quantidade * m.QuantidadePecasPorPulso) ?? 0; // QTD PRODUZIDA NA FASE 3.
            double? feeQtdPecasPorPulso = medicoes.Select(m => m.QuantidadePecasPorPulso).FirstOrDefault();
            DateTime tarDataInicial = medicoes.Min(m => m.DataInicio); // PRIMEIRO FEEDBACK
            DateTime tarDataFinal = medicoes.Max(m => m.DataFim); // ÚLTIMO FEEDBACK

            /* Seleciona as perdas de produção */
            double tarQtdPerdas = medicoes.Where(m => int.Parse(m.TIP_ID) > 500 && int.Parse(m.TIP_ID) < 600).Sum(m => m.Quantidade);

            /* Seleciona o último turno e usuário */
            var dadosUsuario = medicoes.OrderByDescending(m => m.FeeDataInicial).Select(m => new { m.UsuarioId, m.TurnoId, m.TurmaId }).FirstOrDefault();

            /* Selecionando performance de produção. Considerar apenas os registros com quantidade maior que zero, ignorando pequenas paradas. */
            double RealizadoPerformance =
                medicoes.Where(m => m.Quantidade > 0 && (m.DataFim - m.DataInicio).TotalSeconds > 0 && m.OcorrenciaId == "5.1" && m.Fase == 3)
                    .Sum(m => (m.Quantidade * m.QuantidadePecasPorPulso) / (m.DataFim - m.DataInicio).TotalSeconds) ?? -1;

            /* Seleciona o tempo de setup */
            double RealizadoSetup =
                medicoes.Where(m => m.OcorrenciaId == "1.1")
                    .Sum(m => (m.DataFim - m.DataInicio).TotalSeconds);

            /* Seleciona o tempo de setup ajuste */
            double RealizadoSetupa =
                medicoes.Where(m => m.Fase == 2 || m.OcorrenciaId == "1.2")
                    .Sum(m => (m.DataFim - m.DataInicio).TotalSeconds);

            /* Atualiza as metas de quantidade e setup */
            linhasAlteradas = db.Database.ExecuteSqlCommand(
                "UPDATE [T_TARGET_PRODUTO] SET " +
                "TAR_QTD_PERDAS = @TAR_QTD_PERDAS, " +
                "TAR_DATA_INICIAL = @TAR_DATA_INICIAL, " +
                "TAR_DATA_FINAL = @TAR_DATA_FINAL, " +
                "FEE_QTD_PECAS_POR_PULSO = @FEE_QTD_PECAS_POR_PULSO, " +
                "TAR_REALIZADO_PERFORMANCE = @RealizadoPerformance, " +
                "TAR_REALIZADO_TEMPO_SETUP = @RealizadoSetup, " +
                "TAR_REALIZADO_TEMPO_SETUP_AJUSTE = @RealizadoSetupa, " +
                "TAR_QTD_SETUP_AJUSTE = @TAR_QTD_SETUP_AJUSTE, " +
                "TAR_QTD = @TAR_QTD, " +
                "TURM_ID = @TURM_ID, " +
                "TURN_ID = @TURN_ID, " +
                "USE_ID  = @USE_ID " +
                "WHERE MOV_ID = @MOV_ID;",
                new SqlParameter("@TAR_QTD_PERDAS", tarQtdPerdas),
                new SqlParameter("@TAR_DATA_INICIAL", tarDataInicial),
                new SqlParameter("@TAR_DATA_FINAL", tarDataFinal),
                new SqlParameter("@FEE_QTD_PECAS_POR_PULSO", feeQtdPecasPorPulso),
                new SqlParameter("@RealizadoPerformance", RealizadoPerformance),
                new SqlParameter("@RealizadoSetup", RealizadoSetup),
                new SqlParameter("@RealizadoSetupa", RealizadoSetupa),
                new SqlParameter("@TAR_QTD_SETUP_AJUSTE", tarQtdSetupAjuste),
                new SqlParameter("@TAR_QTD", tarQtd),
                new SqlParameter("@TURM_ID", dadosUsuario.TurmaId),
                new SqlParameter("@TURN_ID", dadosUsuario.TurnoId),
                new SqlParameter("@USE_ID", dadosUsuario.UsuarioId),
                new SqlParameter("@MOV_ID", movId)
            );

            /* Targets da produção */
            var targetsProducao = db.TargetProduto.AsNoTracking().FromSql(
                "SELECT TAR_REALIZADO_PERFORMANCE, TAR_REALIZADO_TEMPO_SETUP, TAR_REALIZADO_TEMPO_SETUP_AJUSTE " +
                "FROM [dbo].[T_TARGET_PRODUTO] AS T " +
                "INNER JOIN [dbo].[T_MOVIMENTOS_ESTOQUE] M ON T.PRO_ID = M.PRO_ID AND T.MAQ_ID = M.MAQ_ID " +
                "WHERE M.MOV_ID = @MOV_ID AND LEFT(TAR_APROVADO, 1) = 'A' AND TAR_APROVADO <> 'AP' AND " +
                "(TAR_REALIZADO_PERFORMANCE > 0 OR TAR_REALIZADO_TEMPO_SETUP > 0 OR TAR_REALIZADO_TEMPO_SETUP_AJUSTE > 0)",
                new SqlParameter("@MOV_ID", movId)
            ).Select(t => new TargetProduto 
            {
                /* Selecionando a performance de produção */
                TAR_REALIZADO_PERFORMANCE = t.TAR_REALIZADO_PERFORMANCE ?? 0,
                
                /* Selecionando o tempo de setup */
                TAR_REALIZADO_TEMPO_SETUP = t.TAR_REALIZADO_TEMPO_SETUP ?? 0,
                
                /* Selecionando o tempo de setup ajuste */
                TAR_REALIZADO_TEMPO_SETUP_AJUSTE = t.TAR_REALIZADO_TEMPO_SETUP_AJUSTE ?? 0
            }).ToList();

            string TAR_TIPO_FEEDBACK_PERFORMANCE = "N";
            string TAR_TIPO_FEEDBACK_SETUP = "N";
            string TAR_TIPO_FEEDBACK_SETUP_AJUSTE = "N";

            /* Selecionando a performance de produção */
            List<double> dadosRealizadoPerformance = targetsProducao.Where(t => t.TAR_REALIZADO_PERFORMANCE > 0).Select(t => t.TAR_REALIZADO_PERFORMANCE.Value).ToList();

            double metaPMPerformance = 0;
            if (dadosRealizadoPerformance.Count == 0)
            {
                metaPMPerformance = RealizadoPerformance;
            }
            else if (dadosRealizadoPerformance.Count < 8)
            {
                metaPMPerformance = dadosRealizadoPerformance.OrderByDescending(x => x).FirstOrDefault();
            }
            else
            {
                int count = dadosRealizadoPerformance.Count / 4;
                /* seleciona o primeiro quartil do conjunto de dados */
                List<double> primeiroQuartilPerformance = dadosRealizadoPerformance.OrderByDescending(x => x).Take(count).ToList();
                metaPMPerformance = primeiroQuartilPerformance.Sum() / primeiroQuartilPerformance.Count;
            }

            double RealizadoPerformance_MIN_VERDE = metaPMPerformance * 0.95;
            double RealizadoPerformance_MAX_VERDE = metaPMPerformance * 1.05;

            /* Comparando a meta de performance com a atingida */
            if (RealizadoPerformance > 0)
            {
                if (RealizadoPerformance < metasPerformance.MetaPerformance_MIN_VERDE)
                    TAR_TIPO_FEEDBACK_PERFORMANCE = "B"; // Bad
                else if (RealizadoPerformance > metasPerformance.MetaPerformance_MAX_VERDE)
                    TAR_TIPO_FEEDBACK_PERFORMANCE = "G"; // Good
            }

            /* Selecionando tempo setup */
            double metaPMSetup = metasSetup.FPR_META_SETUP.Value;
            double RealizadoSetup_MIN_VERDE = metaPMSetup * 0.95;
            double RealizadoSetup_MAX_VERDE = metaPMSetup * 1.05;

            /* Comparando a meta de setup com a atingida */
            if (RealizadoSetup > 0 && metasSetup != null)
            {
                if (RealizadoSetup < metasSetup.FPR_META_SETUP)
                    TAR_TIPO_FEEDBACK_SETUP = "B"; // Bad
                else if (RealizadoSetup > metasSetup.FPR_META_SETUP)
                    TAR_TIPO_FEEDBACK_SETUP = "G"; // Good
            }

            /* Selecionando tempo setup ajuste */
            List<double> dadosTempoSetupAjuste = targetsProducao.Where(t => t.TAR_REALIZADO_TEMPO_SETUP_AJUSTE > 0).Select(t => t.TAR_REALIZADO_TEMPO_SETUP_AJUSTE.Value).ToList();
            double metaPMSetupa = 0;
            if (dadosTempoSetupAjuste.Count == 0)
                metaPMSetupa = RealizadoSetupa;
            else if (dadosTempoSetupAjuste.Count < 8)
                metaPMSetupa = dadosTempoSetupAjuste.OrderBy(x => x).FirstOrDefault();
            else
            {
                int count = dadosTempoSetupAjuste.Count / 4;
                /* seleciona o primeiro quartil do conjunto de dados */
                List<double> primeiroQuartilTempoSetupAjuste = dadosTempoSetupAjuste.OrderBy(x => x).Take(count).ToList();
                metaPMSetupa = primeiroQuartilTempoSetupAjuste.Sum() / primeiroQuartilTempoSetupAjuste.Count;
            }

            double RealizadoSetupa_MIN_VERDE = metaPMSetupa * 0.95;
            double RealizadoSetupa_MAX_VERDE = metaPMSetupa * 1.05;

            /* Comparando a meta de setup com a atingida */
            if (RealizadoSetupa > 0 && metasSetupAjuste != null)
            {
                if (RealizadoSetupa < metasSetupAjuste.MetaSetupa_MIN_VERDE)
                    TAR_TIPO_FEEDBACK_SETUP_AJUSTE = "B"; // Bad
                else if (RealizadoSetupa > metasSetupAjuste.MetaSetupa_MAX_VERDE)
                    TAR_TIPO_FEEDBACK_SETUP_AJUSTE = "G"; // Good
            }

            int tws = db.Maquina.AsNoTracking().FromSql(
                "SELECT MAQ_TEMPO_MIN_PARADA FROM T_MAQUINA WHERE MAQ_ID = @MAQ_ID", 
                new SqlParameter("@MAQ_ID", movEstoque.MAQ_ID)
            ).Select(m => m.MAQ_TEMPO_MIN_PARADA ?? 60).FirstOrDefault();
            
            double TAR_PERFORMANCE_MIN_AMARELO = RealizadoPerformance_MIN_VERDE * 0.85;
            double TAR_SETUP_MAX_AMARELO = RealizadoSetup_MAX_VERDE * 1.10;
            double TAR_SETUPA_MAX_AMARELO = RealizadoSetupa_MAX_VERDE * 1.10;
            double TAR_META_TEMPO_SETUP = (metasSetup != null && metasSetup.FPR_META_SETUP != null) ? metasSetup.FPR_META_SETUP.Value : RealizadoSetup;
            double TAR_META_TEMPO_SETUP_AJUSTE = (metasSetupAjuste != null && metasSetupAjuste.metaMASetupa != null) ? metasSetupAjuste.metaMASetupa.Value : RealizadoSetupa;
            double TAR_META_PERFORMANCE = (metasPerformance != null && metasPerformance.metaMAPerformance != null) ? metasPerformance.metaMAPerformance.Value : RealizadoPerformance;

            double TAR_PERCENTUAL_REALIZADO_PERFORMANCE = 0;
            if (RealizadoPerformance != 0 && TAR_META_PERFORMANCE != 0)
                TAR_PERCENTUAL_REALIZADO_PERFORMANCE = (RealizadoPerformance / TAR_META_PERFORMANCE) * 100.0;
            
            double TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE = 5000;
            string TAR_APROVADO = (RealizadoSetup < 0 || RealizadoPerformance < 0) ? "RS" : "PG";

            /* Atualiza próxima meta baseada nos 25% das maiores metas realizadas */
            linhasAlteradas = db.Database.ExecuteSqlCommand(
                "UPDATE [T_TARGET_PRODUTO] SET " +
                "FEE_QTD_PECAS_POR_PULSO = @FEE_QTD_PECAS_POR_PULSO, " +
                "TAR_PERFORMANCE_MAX_VERDE = @RealizadoPerformance_MAX_VERDE, " +
                "TAR_PERFORMANCE_MIN_VERDE = @RealizadoPerformance_MIN_VERDE, " +
                "TAR_SETUP_MAX_VERDE = @RealizadoSetup_MAX_VERDE, " +
                "TAR_SETUP_MIN_VERDE = @RealizadoSetup_MIN_VERDE, " +
                "TAR_SETUPA_MAX_VERDE = @RealizadoSetupa_MAX_VERDE, " +
                "TAR_SETUPA_MIN_VERDE = @RealizadoSetupa_MIN_VERDE, " +
                "TAR_PERFORMANCE_MIN_AMARELO = @TAR_PERFORMANCE_MIN_AMARELO, " +
                "TAR_SETUP_MAX_AMARELO = @TAR_SETUP_MAX_AMARELO, " +
                "TAR_SETUPA_MAX_AMARELO = @TAR_SETUPA_MAX_AMARELO, " +
                "TAR_PROXIMA_META_TEMPO_SETUP = @metaPMSetup, " +
                "TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE = @metaPMSetupa, " +
                "TAR_PROXIMA_META_PERFORMANCE = @metaPMPerformance, " +
                "TAR_TIPO_FEEDBACK_PERFORMANCE = @TAR_TIPO_FEEDBACK_PERFORMANCE, " +
                "TAR_TIPO_FEEDBACK_SETUP = @TAR_TIPO_FEEDBACK_SETUP, " +
                "TAR_TIPO_FEEDBACK_SETUP_AJUSTE = @TAR_TIPO_FEEDBACK_SETUP_AJUSTE, " +
                "TAR_META_TEMPO_SETUP = @TAR_META_TEMPO_SETUP, " +
                "TAR_META_TEMPO_SETUP_AJUSTE = @TAR_META_TEMPO_SETUP_AJUSTE, " +
                "TAR_META_PERFORMANCE = @TAR_META_PERFORMANCE, " +
                "TAR_PERCENTUAL_REALIZADO_PERFORMANCE = @TAR_PERCENTUAL_REALIZADO_PERFORMANCE, " +
                "TAR_PARAMETRO_TIME_WORK_STOP_MACHINE = @TWS, " +
                "TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE = @TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE, " +
                "TAR_APROVADO = @TAR_APROVADO " +
                "WHERE MOV_ID = @MOV_ID;",
                new SqlParameter("@FEE_QTD_PECAS_POR_PULSO", feeQtdPecasPorPulso),
                new SqlParameter("@RealizadoPerformance_MAX_VERDE", RealizadoPerformance_MAX_VERDE),
                new SqlParameter("@RealizadoPerformance_MIN_VERDE", RealizadoPerformance_MIN_VERDE),
                new SqlParameter("@RealizadoSetup_MAX_VERDE", RealizadoSetup_MAX_VERDE),
                new SqlParameter("@RealizadoSetup_MIN_VERDE", RealizadoSetup_MIN_VERDE),
                new SqlParameter("@RealizadoSetupa_MAX_VERDE", RealizadoSetupa_MAX_VERDE),
                new SqlParameter("@RealizadoSetupa_MIN_VERDE", RealizadoSetupa_MIN_VERDE),
                new SqlParameter("@TAR_PERFORMANCE_MIN_AMARELO", TAR_PERFORMANCE_MIN_AMARELO),
                new SqlParameter("@TAR_SETUP_MAX_AMARELO", TAR_SETUP_MAX_AMARELO),
                new SqlParameter("@TAR_SETUPA_MAX_AMARELO", TAR_SETUPA_MAX_AMARELO),
                new SqlParameter("@metaPMSetup", metaPMSetup),
                new SqlParameter("@metaPMSetupa", metaPMSetupa),
                new SqlParameter("@metaPMPerformance", metaPMPerformance),
                new SqlParameter("@TAR_TIPO_FEEDBACK_PERFORMANCE", TAR_TIPO_FEEDBACK_PERFORMANCE),
                new SqlParameter("@TAR_TIPO_FEEDBACK_SETUP", TAR_TIPO_FEEDBACK_SETUP),
                new SqlParameter("@TAR_TIPO_FEEDBACK_SETUP_AJUSTE", TAR_TIPO_FEEDBACK_SETUP_AJUSTE),
                new SqlParameter("@TAR_META_TEMPO_SETUP", TAR_META_TEMPO_SETUP),
                new SqlParameter("@TAR_META_TEMPO_SETUP_AJUSTE", TAR_META_TEMPO_SETUP_AJUSTE),
                new SqlParameter("@TAR_META_PERFORMANCE", TAR_META_PERFORMANCE),
                new SqlParameter("@TAR_PERCENTUAL_REALIZADO_PERFORMANCE", TAR_PERCENTUAL_REALIZADO_PERFORMANCE),
                new SqlParameter("@TWS", tws),
                new SqlParameter("@TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE", TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE),
                new SqlParameter("@TAR_APROVADO", TAR_APROVADO),
                new SqlParameter("@MOV_ID", movId)
            );

            /* Insere registro fake de feedback e setup ajuste */
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    linhasAlteradas = db.Database.ExecuteSqlCommand(
                        "INSERT INTO [dbo].[T_FEEDBACK] ([FEE_DATA_INICIAL], [FEE_DATA_FINAL], [MAQ_ID], [OCO_ID], [TURN_ID], " +
                        "[TURM_ID], [USE_ID], [ORD_ID], [PRO_ID], [FEE_OBSERVACOES], [FEE_GRUPO], [FEE_DIA_TURMA], " +
                        "[ROT_SEQ_TRANFORMACAO], [FPR_SEQ_REPETICAO], [FEE_QTD_PULSOS], [FEE_QTD_PECAS_POR_PULSO]) " +
                        
                        "SELECT MIN(DATA_INI), MAX(DATA_FIM), F.MAQ_ID, '1.2' OCO_ID, F.TURN_ID, " +
                        "F.TURM_ID, F.USE_ID, F.ORD_ID, F.PRO_ID, F.FEE_OBSERVACOES, -1 FEE_GRUPO, [dbo].[DIATURMA](F.FEE_DATA_INICIAL), " +
                        "F.ROT_SEQ_TRANFORMACAO, F.FPR_SEQ_REPETICAO, SUM(QTD), F.FEE_QTD_PECAS_POR_PULSO " +
                        "FROM T_FEEDBACK F " +
                        "INNER JOIN [dbo].[T_FEEDBACK_MOV_ESTOQUE] FM ON FM.FEE_ID = F.FEE_ID " +
                        "INNER JOIN T_CLP_MEDICOES C ON C.GRUPO = F.FEE_GRUPO AND C.MAQUINA_ID = F.MAQ_ID " +
                        "WHERE FM.MOV_ID = @MOV_ID AND FASE = 2 AND LEFT(F.OCO_ID,1) = '5' " +
                        "GROUP BY F.MAQ_ID, F.TURN_ID, F.TURM_ID, F.USE_ID, F.ORD_ID, F.PRO_ID, F.FEE_OBSERVACOES, " +
                        "[dbo].[DIATURMA](F.FEE_DATA_INICIAL), F.ROT_SEQ_TRANFORMACAO, F.FPR_SEQ_REPETICAO, F.FEE_QTD_PECAS_POR_PULSO;" +
                        
                        "if @@IDENTITY > 0 " +
                        "BEGIN " +
                        "  INSERT INTO [dbo].[T_FEEDBACK_MOV_ESTOQUE] (FEE_ID,MOV_ID) VALUES (@@IDENTITY, @MOV_ID); " +
                        "END ",
                        new SqlParameter("@MOV_ID", movId)
                    );

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private List<SetFaseFinal_ClpMedicoes> SetFasesFinal(JSgi db, int movId, string proId, string maqId, int seqTran)
        {
            /* Seleciona todos as Medições do Movimento */
            var medicoes = (from m in db.MovimentoEstoque
                            join fbme in db.T_FeedbackMovEstoque
                                on m.MOV_ID equals fbme.MovimentoEstoqueId
                            join f in db.Feedback
                                on fbme.FeedbackId equals f.Id
                            join c in db.ClpMedicoes
                                on
                                    new { grupo = f.Grupo, maquina = f.MaquinaId }
                                equals
                                    new { grupo = c.Grupo.Value, maquina = c.MaquinaId }
                            where m.MOV_ID == movId
                            select new SetFaseFinal_ClpMedicoes
                            {
                                // clp
                                DataFim = c.DataFim,
                                DataInicio = c.DataInicio,
                                Quantidade = c.Quantidade,
                                Fase = f.OcorrenciaId == "1.1" ? 1 : 3,
                                // feedback
                                QuantidadePecasPorPulso = f.QuantidadePecasPorPulso,
                                UsuarioId = f.UsuarioId,
                                TurmaId = f.TurmaId,
                                TurnoId = f.TurnoId,
                                FeeDataInicial = f.DataInicial,
                                OcorrenciaId = f.OcorrenciaId,
                                // mov estoque
                                MOV_QUANTIDADE = m.MOV_QUANTIDADE,
                                TIP_ID = m.TIP_ID
                            }).OrderBy(x => x.DataInicio).ToList();

            var target = db.TargetProduto.AsNoTracking().FromSql(
                "SELECT TOP 1 ISNULL(TAR_PARAMETRO_TIME_WORK_STOP_MACHINE, 60) AS TAR_PARAMETRO_TIME_WORK_STOP_MACHINE, " +
                "ISNULL(TAR_PROXIMA_META_PERFORMANCE, -1) AS TAR_PROXIMA_META_PERFORMANCE " +
                "FROM T_TARGET_PRODUTO T (NOLOCK) " +
                "WHERE MOV_ID < @MOV_ID AND T.PRO_ID = @PRO_ID AND T.MAQ_ID = @MAQ_ID AND ROT_SEQ_TRANFORMACAO = @FPR_SEQ_TRANFORMACAO " +
                "ORDER BY TAR_ID DESC",
                new SqlParameter("@MOV_ID", movId),
                new SqlParameter("@PRO_ID", proId),
                new SqlParameter("@MAQ_ID", maqId),
                new SqlParameter("@FPR_SEQ_TRANFORMACAO", seqTran)
            ).Select(t => new 
            {
                TEMPO_MINIMO_PARADA = t.TAR_PARAMETRO_TIME_WORK_STOP_MACHINE,
                t.TAR_PROXIMA_META_PERFORMANCE
            }).FirstOrDefault();

            int TEMPO_MINIMO_PARADA;
            double TAR_PROXIMA_META_PERFORMANCE;
            if (target == null)
            {
                TEMPO_MINIMO_PARADA = 60;
                TAR_PROXIMA_META_PERFORMANCE = -1;
            }
            else
            {
                TEMPO_MINIMO_PARADA = target.TEMPO_MINIMO_PARADA.Value;
                TAR_PROXIMA_META_PERFORMANCE = target.TAR_PROXIMA_META_PERFORMANCE.Value;
            }


            int fase = 0;
            double tempoFaseTres = 0;
            List<int> indicesQueTeraoFaseTres = new List<int>();
            for (int i = 0; i < medicoes.Count; i++)
            {
                var clp = medicoes[i];

                /* Cálculo para obter a performance. */
                double tempoSegundos = clp.DataInicio == clp.DataFim ? 1 : (clp.DataFim - clp.DataInicio).TotalSeconds;
                double performance = (clp.Quantidade * (clp.QuantidadePecasPorPulso ?? 1)) / tempoSegundos;

                /* Cálculo para verificar se atingiu a velocidade de 80% de produção. */
                bool atgVelocidadeProducao = performance > (TAR_PROXIMA_META_PERFORMANCE * 0.8);

                if (clp.Quantidade <= 0 && fase <= 1)
                {/* Sem quantidade. Setup. */
                    fase = 1;
                    tempoFaseTres = 0;
                    indicesQueTeraoFaseTres.Clear();
                }
                else if (!atgVelocidadeProducao && fase <= 2)
                {/* Não atingiu 80% de performance. Setup Ajuste. */
                    fase = 2;
                    tempoFaseTres = 0;
                    indicesQueTeraoFaseTres.Clear();
                }
                else if (atgVelocidadeProducao)
                {/* Atingiu a velocidade de 80% da produção. */

                    if (tempoFaseTres >= TEMPO_MINIMO_PARADA)
                    {/* Muda para a fase três quando atingir o tempo mínimo de parada da máquina. */
                        fase = 3;
                    }
                    else
                    {/* Incrementa o tempo de produção na velocidade 80% de produção. */
                        tempoFaseTres += tempoSegundos;
                        fase = 2;
                        indicesQueTeraoFaseTres.Add(i);
                    }
                }
                clp.Fase = fase;
            }
            /* Alterando para Fase Três, as medições que atingiram a performance de 80% mas que ainda era menor que o tempo mínimo de parada. */
            indicesQueTeraoFaseTres.ForEach(i => medicoes[i].Fase = 3);

            return medicoes;
        }
        #endregion Target
    }
}