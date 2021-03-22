using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DynamicForms.Areas.PlugAndPlay.Controllers
{
    [Authorize]
    [Area("plugandplay")]
    public class QualidadeController : BaseController
    {
        public IActionResult Index()
        {
            //Comentário
            T_Usuario user = ObterUsuarioLogado();
            ViewBag.UserName = user.USE_NOME;
            return View();
        }



        public IActionResult VerificarHaTestesFeitos(String ord, String prod) // verifica se ja foi feito testes
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var testes = db.TesteFisico.Count(x => x.ORD_ID.Equals(ord) && x.ROT_PRO_ID.Equals(prod));
                return Json(testes > 0);
            }
        }
        #region Template de Testes
        /// <summary>
        /// Retorna a View de cadastro de testes fisicos e visuais
        /// </summary>
        public IActionResult TemplateDeTestes(string TemId)
        {
            int.TryParse(TemId, out int temId);
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var _TemplateDeTestes = db.TemplateDeTestes.Where(t => t.TEM_ID == temId).Select(t => new { t.TEM_ID, t.TEM_DESCRICAO }).FirstOrDefault();
                if (_TemplateDeTestes != null)
                    ViewBag.Template = _TemplateDeTestes.TEM_ID;
                return View();
            }
        }

        /// <summary>
        /// Deleta do template todos os testes fisicos ou testes visuais
        /// </summary>
        public bool DeletarTestesDoTemplate(int TemId, int qtdTesteFisico, int qtdInspecaoVisual)
        {
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (qtdTesteFisico > 0)
                {

                    var TestesIds = db.TemplateTipoTeste.AsNoTracking().Where(x => x.TEM_ID.Equals(TemId)).ToList();

                    foreach (var item in TestesIds)
                    {
                        item.PlayAction = "delete";
                        listaItem.Add(item);
                    }
                }
                if (qtdInspecaoVisual > 0)
                {
                    var TestesIds = db.TemplateTipoInspecaoVisual.AsNoTracking().Where(x => x.TEM_ID.Equals(TemId)).ToList();
                    foreach (var item in TestesIds)
                    {
                        item.PlayAction = "delete";
                        listaItem.Add(item);
                    }
                }
            }

            if (listaItem.Any())
            {
                List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                List<LogPlay> Logs = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos
            }
            return true;
        }

        /// <summary>
        /// salva os testes fisicos e visuais da tela de cadastro(TemplateDeTestes)
        /// ListaTipos e ListaTiposInspecao são os IDs do tipo teste fisico e tipo inspeção visual
        /// </summary>
        public bool AddTiposDeTestesNoTemplate(string TemId, string[] ListaTipos, string[] ListaTiposInspecao)
        {
            int.TryParse(TemId, out int temId);
            int idAux;
            List<int> ListaTiposint = new List<int>();
            List<int> ListaTiposintI = new List<int>();

            int TEM_ID = Convert.ToInt32(TemId);

            foreach (var item in ListaTipos)
            {
                int.TryParse(item, out idAux);
                if (idAux != 0)
                    ListaTiposint.Add(idAux);
            }
            foreach (var item in ListaTiposInspecao)
            {
                int.TryParse(item, out idAux);
                if (idAux != 0)
                    ListaTiposintI.Add(idAux);
            }

            DeletarTestesDoTemplate(TEM_ID, ListaTiposint.Count, ListaTiposintI.Count);

            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();

            foreach (var t in ListaTiposint)
            {
                TemplateTipoTeste templateTipo = new TemplateTipoTeste() { TTT_ID = 0, TEM_ID = TEM_ID, TT_ID = t, PlayAction = "insert", PlayMsgErroValidacao = "" };
                listaItem.Add(templateTipo);
            }
            foreach (var t in ListaTiposintI)
            {
                TemplateTipoInspecaoVisual templateTipoI = new TemplateTipoInspecaoVisual() { TTI_ID = 0, TEM_ID = TEM_ID, TIV_ID = t, PlayAction = "insert", PlayMsgErroValidacao = "" };
                listaItem.Add(templateTipoI);
            }
            List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
            List<LogPlay> Logs = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos
            return true;
        }
        #endregion
        public JsonResult ObterTestesFisicosAgrupados(String Coluna, String Campo, String Pesquisa, String Quantidade) // Retorna as OPs no gerenciamento de teste com todas as informações
        {


            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                List<string> statusDeLiberacao = new List<string>() { "AMOSTRA_COLETADA", "AMOSTRA_NAO_COLETADA", "REPROVADO_USUARIO", "REPROVADO_SISTEMA" };
                int qtd = Convert.ToInt32(Quantidade);
                var amostrasNaoLiberadas = db.TesteFisico.AsNoTracking().Where(x => statusDeLiberacao.Contains(x.TES_STATUS_LIBERACAO)).ToList();
                var listaAmostras = amostrasNaoLiberadas.GroupBy(x => new { x.ORD_ID, x.FPR_SEQ_REPETICAO, x.ROT_PRO_ID, x.TES_EMISSAO })
                                           .Select(x => new
                                           {
                                               x.Key.ORD_ID,
                                               x.Key.FPR_SEQ_REPETICAO,
                                               x.Key.ROT_PRO_ID,
                                               x.Key.TES_EMISSAO,
                                               Total = x.Count(),
                                               TotalInsp = db.InspecaoVisual.Count(i => i.ORD_ID.Equals(x.Key.ORD_ID)
                                                   && i.FPR_SEQ_REPETICAO.Equals(x.Key.FPR_SEQ_REPETICAO)
                                                   && i.ROT_PRO_ID.Equals(x.Key.ROT_PRO_ID)
                                                   && statusDeLiberacao.Contains(i.IPV_STATUS_LIBERACAO))
                                           })
                                           .OrderBy(x => x.TES_EMISSAO).ToList();

                if (Pesquisa == null)
                {
                    qtd = (qtd > listaAmostras.Count) ? listaAmostras.Count : qtd;
                    var _lista = listaAmostras.GetRange(0, qtd);
                    return Json(new { _lista });
                }
                else
                {
                    if (Coluna == "ORD_ID")
                    {
                        if (Campo == "parecido com")
                        {
                            listaAmostras = listaAmostras.Where(x => x.ORD_ID.Contains(Pesquisa)).ToList();
                        }
                        else
                        {
                            if (Campo == "igual a")
                            {
                                listaAmostras = listaAmostras.Where(x => x.ORD_ID.Equals(Pesquisa)).ToList();
                            }
                        }
                    }
                    else if (Coluna == "FPR_SEQ_REPETICAO")
                    {
                        if (Campo == "parecido com")
                        {
                            listaAmostras = listaAmostras.Where(x => x.FPR_SEQ_REPETICAO.ToString().Contains(Pesquisa)).ToList();
                        }
                        else
                        {
                            if (Campo == "igual a")
                            {
                                listaAmostras = listaAmostras.Where(x => x.FPR_SEQ_REPETICAO.ToString().Equals(Pesquisa)).ToList();
                            }
                        }
                    }
                    else if (Coluna == "ROT_PRO_ID")
                    {
                        if (Campo == "parecido com")
                        {
                            listaAmostras = listaAmostras.Where(x => x.ROT_PRO_ID.Contains(Pesquisa)).ToList();
                        }
                        else
                        {
                            if (Campo == "igual a")
                            {
                                listaAmostras = listaAmostras.Where(x => x.ROT_PRO_ID.Equals(Pesquisa)).ToList();
                            }
                        }
                    }
                    else if (Coluna == "TES_EMISSAO")
                    {
                        DateTime dataAuxConsulta = Convert.ToDateTime(Pesquisa);
                        if (Campo == "igual a")
                        {
                            listaAmostras = listaAmostras.Where(x => x.TES_EMISSAO.Date.CompareTo(dataAuxConsulta) == 0).ToList();
                        }
                        else
                        if (Campo == "Maior")
                        {
                            listaAmostras = listaAmostras.Where(x => x.TES_EMISSAO.Date > dataAuxConsulta).ToList();
                        }
                        else
                        {
                            if (Campo == "Menor")
                            {
                                listaAmostras = listaAmostras.Where(x => x.TES_EMISSAO.Date < dataAuxConsulta).ToList();
                            }
                            else
                            {
                                listaAmostras = listaAmostras.Where(x => x.TES_EMISSAO.ToString().Contains(Pesquisa)).ToList();
                            }
                        }
                    }

                    return Json(new { listaAmostras });
                }
            }
        }
        #region Gerenciar Testes
        public JsonResult ObterTestesFisicosPorOP(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string FPR_SEQ_REPETICAO, string DATA) // obtem os testes fisicos e suas amostras da Op selecionada
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                List<string> status = new List<string>() { "APROVADO_USUARIO", "APROVADO_SISTEMA" };
                DateTime DataAux = Convert.ToDateTime(DATA.Replace("T", " "));
                var Db_lista = db.TesteFisico.Where(x => x.ORD_ID.Equals(ORD_ID) &&
                                                    x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO)
                                                    && x.ROT_PRO_ID.Equals(ROT_PRO_ID)
                                                    && x.TES_EMISSAO.CompareTo(DataAux) == 0
                                                    && !status.Contains(x.TES_STATUS_LIBERACAO)
                                                    ).ToList();
                var _lista = Db_lista.GroupBy(x => x.TES_NOME_TECNICO).Select(group => group.First()).ToList();
                var _listaAmostras = Db_lista.GroupBy(x => x.TES_NOME_TECNICO).ToList();

                return Json(new { _lista, _listaAmostras });
            }
        }

        public ActionResult GravarTesteFisico(string tes_id, string valTes_id)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                string[] id_tes = tes_id.Split(";");
                string[] val_tes = valTes_id.Split(";");
                int cont = 0;
                int totalTestes = 0;
                DateTime _dataAux;
                List<int> TestesIds = new List<int>();
                List<double> ValoresTestes = new List<double>();
                //Filtrando Array de Strings dos parâmetros e convertendo valores
                //-- Ids dos Testes
                foreach (var item in id_tes)
                {
                    int.TryParse(item, out var idAux);
                    if (idAux != 0)
                        TestesIds.Add(idAux);
                }

                //Valores  recuperados dos testes
                foreach (var item in val_tes)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        double.TryParse(item, out var auxItem);
                        ValoresTestes.Add(auxItem);
                    }
                }
                //--
                //Recuperando Testes Fisicos do banco para futura  atualização
                var Db_TesteFisico = db.TesteFisico
                    .AsNoTracking()
                    .Where(x => TestesIds.Contains(x.TES_ID))
                    .ToList();
                totalTestes = Db_TesteFisico.Count;
                //Iterando nos objetos e Atualizando valores
                MasterController mc = new MasterController();
                List<object> lista = new List<object>();
                if (Db_TesteFisico != null && totalTestes > 0)
                {
                    _dataAux = DateTime.Now;
                    while (cont < totalTestes)
                    {
                        TesteFisico teste = Db_TesteFisico.Where(t => t.TES_ID == TestesIds[cont]).FirstOrDefault();
                        if (teste != null)
                        {
                            teste.PlayAction = "update";
                            teste.TES_VALOR_NUMERICO = ValoresTestes[cont];
                            teste.DATA_ULTIMA_ALTERACAO = _dataAux;
                            teste.TES_STATUS_LIBERACAO = "AMOSTRA_COLETADA";
                            lista.Add(teste);
                        }
                        cont++;
                    }
                    List<List<object>> ListOfListObjects = new List<List<object>>() { lista };
                    List<LogPlay> logPlay = mc.UpdateData(ListOfListObjects, 0, true);
                }
                return Json(true);
            }

        }

        public ActionResult ObterTestes(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var tipo = db.TipoTeste.ToList();
                return Json(new { tipo });
            }

        }

        public ActionResult GravarNovoTesteFisico(string idOrd, string idPro, string seqRep, string QtdeColetada)
        {
            string[] quantidadeAmostras = QtdeColetada.Split(";");
            TesteFisico testeFisico = new TesteFisico();
            testeFisico.GerarAmostrasTesteFisico(idOrd, idPro, null, seqRep, "", "A", "A", 27, null, true, ""); // corrigir campos que falta
            return Json(new { testeFisico });
        }

        public ActionResult GerarStatus(string tes_id, string tes_value)
        {
            string[] testeid = tes_id.Split(";");
            string[] testevalor = tes_value.Split(";");
            TesteFisico testeFisico = new TesteFisico();
            return Json((testeFisico.AvaliarSequenciaTeste(testeid, testevalor)));
        }

        public ActionResult ObterInspecoes(string Ord_Id, string Prod_Id, string Maq_Id, string Seq_Rep, string Seq_Trans, string Tem_Id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                TemplateDeTestes _Tt = new TemplateDeTestes();
                var inspecoes_ids = db.TemplateTipoInspecaoVisual.AsNoTracking().Where(x => x.TEM_ID == Convert.ToInt32(Tem_Id)).Select(x => x.TIV_ID).ToList(); // Retorna os Ids do testes visuais de acordo com o template
                var inspecoes = db.TipoInspecaoVisual.Where(x => inspecoes_ids.Contains(x.TIV_ID)).ToList(); // Retorna os testes visuais de acordo com os IDs retornados da inspecoes_ids

                return Json(inspecoes);
            }
        }

        public ActionResult BuscarInspecoes(string termopesquisado)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (termopesquisado == null)
                    termopesquisado = "";
                var inspecoes = db.TipoInspecaoVisual.AsNoTracking().Where(x => x.TIV_NOME.Equals(termopesquisado) || x.TIV_NOME.Contains(termopesquisado)).ToList();

                return Json(inspecoes);
            }

        }

        public ActionResult GravarResultadoInspecaoVisual(String[] medidaInsp, String[] valInput, String[] idsInspecao, String[] Resultado, String[] Obs, String Turno, String Turma, String UserL, String Ord_Id, String Pro_Id, String Maq_id, String Seq_Trans, String Seq_Rep)
        {// Grava as inspeções visuais com os valores do select(OK/NA) e grava inspeções do tipo medida
            DateTime thisDay = DateTime.Now;
            int idAux;
            List<int> ListaIds = new List<int>();
            List<int> ListaIdsinput = new List<int>();
            List<double> vInput = new List<double>();
            foreach (var item in idsInspecao) // Converte os Ids de String para Int e salva em List
            {
                int.TryParse(item, out idAux);
                if (idAux != 0)
                    ListaIds.Add(idAux);
            }
            if (medidaInsp != null && valInput != null)
            {
                foreach (var item in medidaInsp) // Converte os Ids do tipo Medida de String para Int e salva em List
                {
                    int.TryParse(item, out idAux);
                    if (idAux != 0)
                        ListaIdsinput.Add(idAux);
                }

                foreach (var item in valInput) //Converte os valores dos Inputs da inspeção tipo Medida e salva em List
                {
                    double.TryParse(item, out var auxInput);
                    if (auxInput != 0)
                        vInput.Add(auxInput);
                }
            }
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();
            int i = 0;
            for (; i < ListaIds.Count; i++) // Criação dos objetos e add na List
            {
                InspecaoVisual visual = new InspecaoVisual()
                {
                    IPV_ID = 0,
                    TURM_ID = Turma,
                    TURN_ID = Turno,
                    TIV_ID = ListaIds[i],
                    IPV_DATA_COLETA = thisDay,
                    IPV_OBS = (Obs.Length > 0) ? Obs[i] : "",
                    IPV_ID_OPERADOR = Convert.ToInt32(UserL),
                    IPV_VALOR = (Resultado.Length > 0) ? Resultado[i] : "",
                    ORD_ID = Ord_Id,
                    ROT_PRO_ID = Pro_Id,
                    ROT_MAQ_ID = Maq_id,
                    ROT_SEQ_TRANSFORMACAO = Convert.ToInt32(Seq_Trans),
                    FPR_SEQ_REPETICAO = Convert.ToInt32(Seq_Rep),
                    IPV_STATUS_LIBERACAO = "AMOSTRA_COLETADA",
                    PlayAction = "insert",
                    PlayMsgErroValidacao = ""
                };
                listaItem.Add(visual);
            }
            if (ListaIdsinput.Count > 0)
            {
                for (int j = 0; j < ListaIdsinput.Count; j++)  // Criação dos objetos tipo Medida e add na List
                {
                    InspecaoVisual visual = new InspecaoVisual()
                    {
                        IPV_ID = 0,
                        TURM_ID = Turma,
                        TURN_ID = Turno,
                        TIV_ID = ListaIdsinput[j],
                        IPV_DATA_COLETA = thisDay,
                        IPV_OBS = (Obs.Length > 0) ? Obs[i] : "",
                        IPV_VALOR_MEDIDA = vInput.Count > 0 ? vInput[j] : 0,
                        IPV_ID_OPERADOR = Convert.ToInt32(UserL),
                        ORD_ID = Ord_Id,
                        ROT_PRO_ID = Pro_Id,
                        ROT_MAQ_ID = Maq_id,
                        ROT_SEQ_TRANSFORMACAO = Convert.ToInt32(Seq_Trans),
                        FPR_SEQ_REPETICAO = Convert.ToInt32(Seq_Rep),
                        IPV_STATUS_LIBERACAO = "AMOSTRA_COLETADA",
                        PlayAction = "insert",
                        PlayMsgErroValidacao = ""
                    };
                    i++;
                    listaItem.Add(visual);
                }
            }
            List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
            _ = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos

            return Json(true);
        }

        public JsonResult ObterTestesVisuaisPorOP(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string FPR_SEQ_REPETICAO)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var Db_listaInspecoes = db.InspecaoVisual
                                    .AsNoTracking()
                                    .Include(x => x.TipoInspecaoVisual)
                                    .Where(x => x.ORD_ID.Equals(ORD_ID) &&
                                           x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO)
                                           && x.ROT_PRO_ID.Equals(ROT_PRO_ID)
                                           && !x.IPV_STATUS_LIBERACAO.Equals("APROVADO_USUARIO")
                                           && !x.IPV_STATUS_LIBERACAO.Equals("APROVADO_SISTEMA"))
                                    .Select(x => new { x.ORD_ID, x.ROT_PRO_ID, x.FPR_SEQ_REPETICAO, x.TipoInspecaoVisual })
                                    .ToList();
                int TOTAL = Db_listaInspecoes.Count;
                var _listaInspecoes = Db_listaInspecoes.GroupBy(x => x.TipoInspecaoVisual.TIV_NOME).ToList();

                return Json(new { _listaInspecoes, TOTAL });
            }
        }

        public JsonResult ObterTestesVisuaisPorOPTabela(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO) // Retorna as inspeções visuais de acordo com a OP na tabela de gerenciamento de teste
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var Db_listaInspecoes = db.InspecaoVisual.Include(x => x.TipoInspecaoVisual).Where(x => x.ORD_ID.Equals(ORD_ID) &&
                                                      x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO)
                                                      && x.ROT_PRO_ID.Equals(ROT_PRO_ID)
                                                      && (x.IPV_STATUS_LIBERACAO.Equals("AMOSTRA_COLETADA")
                                                      || x.IPV_STATUS_LIBERACAO.Equals("AMOSTRA_NAO_COLETADA")
                                                      || x.IPV_STATUS_LIBERACAO.Equals("REPROVADO_USUARIO")
                                                      || x.IPV_STATUS_LIBERACAO.Equals("REPROVADO_SISTEMA"))).ToList();

                var _listaInspecoes = Db_listaInspecoes.GroupBy(x => x.TipoInspecaoVisual.TIV_NOME).ToList();

                return Json(new { _listaInspecoes });
            }
        }

        public JsonResult GerarStatusVisuais(String[] Id, String[] Valor) // gera os status das inspeções visuais 
        {
            int cont = 0;
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                InspecaoVisual inspecaoVisual = new InspecaoVisual();
                int status = inspecaoVisual.AvaliarSequenciaInspecao(Id, Valor); // Se todas as Inspeções estiverem com OK, return 1
                List<int> TestesIds = new List<int>();
                foreach (var item in Id)
                {
                    int.TryParse(item, out var idAux);
                    if (idAux != 0)
                        TestesIds.Add(idAux);
                }

                var Db = db.InspecaoVisual
                   .AsNoTracking()
                   .Where(x => TestesIds.Contains(x.IPV_ID))
                   .ToList();

                if (Db != null)
                {
                    foreach (var itemId in Db) // Aprova pelo sistema se todas as inspecões daquele tipo de inspeção estiverem OK 
                    {
                        itemId.IPV_VALOR = Valor[cont];
                        itemId.PlayAction = "update";
                        itemId.PlayMsgErroValidacao = "";
                        itemId.IPV_STATUS_LIBERACAO = status == 1 ? "APROVADO_SISTEMA" : "REPROVADO_SISTEMA";

                        cont++;
                        listaItem.Add(itemId);
                    }

                    List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                    _ = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos               

                    return Json(status == 1);
                }
                return Json(false);
            }
        }

        public JsonResult SalvarValorMedida(String[] Id, String[] Valor) // Salva os status das inspeções do tipo medida
        {
            int cont = 0;
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                InspecaoVisual inspecaoVisual = new InspecaoVisual();
                int status = inspecaoVisual.AvaliarSequenciaInspecao(Id, Valor);
                List<int> TestesIds = new List<int>();
                foreach (var item in Id)
                {
                    int.TryParse(item, out var idAux);
                    if (idAux != 0)
                        TestesIds.Add(idAux);
                }

                var Db = db.InspecaoVisual
                   .AsNoTracking()
                   .Where(x => TestesIds.Contains(x.IPV_ID))
                   .ToList();

                if (Db != null)
                {
                    foreach (var itemId in Db) // Aprova ou Reprova as inspeções visuais do tipo Medida
                    {
                        itemId.IPV_VALOR_MEDIDA = Convert.ToDouble(Valor[cont]);
                        itemId.PlayAction = "update";
                        itemId.PlayMsgErroValidacao = "";
                        itemId.IPV_STATUS_LIBERACAO = status == 1 ? "APROVADO_SISTEMA" : "REPROVADO_SISTEMA";

                        cont++;
                        listaItem.Add(itemId);
                    }

                    List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                    _ = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos               

                    if (status == 1)
                        return Json(true);
                    else
                        return Json(false);
                }
                return Json(false);
            }
        }
        /// <summary>
        /// adiciona uma nova inspeção visual a partir da planilha de testes editaveis
        /// </summary>
        /// <param name="idInspecao"></param>
        /// <param name="Ord_Id"></param>
        /// <param name="Pro_Id"></param>
        /// <param name="Maq_id"></param>
        /// <param name="Seq_Trans"></param>
        /// <param name="Seq_Rep"></param>
        /// <param name="turno"></param>
        /// <param name="turma"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public ActionResult AdicionarNovaInspecaoVisual(String idInspecao, String Ord_Id, String Pro_Id, String Maq_id, String Seq_Trans, String Seq_Rep, String turno, String turma, String usuario)
        {
            DateTime thisDay = DateTime.Now;
            int idAux = 0;
            int idUser;
            int.TryParse(idInspecao, out idAux);
            int.TryParse(usuario, out idUser);
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();
            InspecaoVisual visual = new InspecaoVisual()
            {
                IPV_ID = 0,
                TURM_ID = turma,
                TURN_ID = turno,
                TIV_ID = idAux,
                IPV_DATA_COLETA = thisDay,
                IPV_OBS = "",
                IPV_ID_OPERADOR = idUser,
                IPV_VALOR = "",
                ORD_ID = Ord_Id,
                ROT_PRO_ID = Pro_Id,
                ROT_MAQ_ID = "",
                ROT_SEQ_TRANSFORMACAO = 0,
                FPR_SEQ_REPETICAO = Convert.ToInt32(Seq_Rep),
                IPV_STATUS_LIBERACAO = "AMOSTRA_COLETADA",
                PlayAction = "insert",
                PlayMsgErroValidacao = ""
            };
            listaItem.Add(visual);

            List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
            _ = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos

            return Json(true);
        }

        /// <summary>
        /// Permite ao usuário Aprovar ou reprovar Todos Os testes Fisiscos de Uma OP
        /// 
        /// </summary>
        /// <param name="ORD_ID">Id do Pedido</param>
        /// <param name="ROT_PRO_ID">ID do Produto</param>
        /// <param name="FPR_SEQ_REPETICAO">Sequencia de repetição do Pedido</param>
        /// <param name="Data">Data de Emissão dos Testes Fisicos</param>
        /// <param name="status">A Aprovar/R Reprovar OP</param>
        /// <returns>Mensagem de erro ou OK </returns>
        public JsonResult AprovarReprovarLote(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO, string Data, string status)
        {
            bool flag = true;
            StringBuilder msg = new StringBuilder();
            string Status = (status.Equals("A")) ? "APROVADO_USUARIO" : "REPROVADO_USUARIO";
            DateTime DataAux;
            if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID) || String.IsNullOrEmpty(FPR_SEQ_REPETICAO) || String.IsNullOrEmpty(Data))
            {
                flag = false;
                msg.Append(" Pedido, Produto,Data de emissao e Sequencia de transformação devem ser informados corretamente, verifique os dados.");
            }
            if (flag)
            {
                MasterController mc = new MasterController();
                List<object> listaItem = new List<object>();
                DateTime DataAtual = DateTime.Now;
                using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
                {
                    DataAux = Convert.ToDateTime(Data);
                    var Db_TestesFisicos = db.TesteFisico.Include(x => x.ResultLote).AsNoTracking().Where(x => x.ORD_ID.Equals(ORD_ID) &&
                                                          x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO) &&
                                                          x.ROT_PRO_ID.Equals(ROT_PRO_ID) &&
                                                          x.TES_EMISSAO.CompareTo(DataAux) == 0)
                                                        .ToList();
                    //Atualizando Testes Fisicos
                    foreach (var item in Db_TestesFisicos)
                    {
                        item.TES_STATUS_LIBERACAO = Status;
                        item.DATA_ULTIMA_ALTERACAO = DataAtual;
                        item.PlayAction = "update";
                    }
                    //Atualizando ResultLote
                    ResultLote _ResultLote = Db_TestesFisicos.First().ResultLote;
                    _ResultLote.RL_DATA_LIBERACAO = DataAtual;
                    _ResultLote.RL_STATUS = Status;
                    _ResultLote.RL_NOME_LIBERACAO = ObterUsuarioLogado().USE_NOME;
                    _ResultLote.PlayAction = "update";
                    //--
                    if (status.Equals("R"))
                    {
                        var listReservas = (from saldo in db.SaldosEmEstoquePorLote join res in db.MovimentoEstoqueReservaDeEstoque
                                            on saldo.MOV_LOTE equals res.MOV_LOTE where
                                            saldo.ORD_ID.Equals(ORD_ID) &&
                                           saldo.PRO_ID.Equals(ROT_PRO_ID)&&
                                           res.FPR_SEQ_REPETICAO== Convert.ToInt32(FPR_SEQ_REPETICAO)
                                            select res).AsNoTracking().ToList();
                        
                        if (listReservas != null)
                        {
                            foreach (var item in listReservas)
                            {
                                item.MOV_RETIDO = "S";
                                item.PlayAction = "_BLOQUEIO_QUALIDADE";
                                listaItem.Add(item);
                            }
                        }
                    }


                    //
                    if (status.Equals("A"))
                    {
                        //--
                        LaudoTesteFisico _LaudoTesteFisico = new LaudoTesteFisico();
                        _LaudoTesteFisico = _LaudoTesteFisico.GerarLaudoTesteFisico(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO, ObterUsuarioLogado().USE_ID, DataAux, "U");
                        listaItem.Add(_LaudoTesteFisico);
                        //--
                    }
                    //
                    listaItem.AddRange(Db_TestesFisicos);
                    listaItem.Add(_ResultLote);

                    List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                    List<LogPlay> Logs = mc.UpdateData(ListOfListObjects, 0, true);//Persintindo Objetos
                    if (Logs.First().GetLogsErro(Logs).Any())
                    {
                        msg.AppendLine("Os seguintes erros foram encontrados no processo de atualização dos Testes Fisicos:");
                        foreach (var item in Logs.First().GetLogsErro(Logs))
                        {
                            msg.Append(item.NomeAtributo);
                            msg.Append("-");
                            msg.Append(item.MsgErro);
                            msg.AppendLine();
                        }
                    }
                    else { msg.Append("OK"); }
                }
            }
            return Json(msg.ToString());
        }
        /// <summary>
        /// Realiza o Registro do Laudo pré aprovado. 
        /// O método pode ser requisitado tanto pelo sistema quanto pelo usuário.
        /// O registro do Laudo contem a OP e a data de emissão dos testes físicos que o identificam, 
        /// a existência do laudo condiciona o Romaneio do Lote produzido em uma carga
        /// </summary>
        /// <param name="ORD_ID">ID do Pedido</param>
        /// <param name="ROT_PRO_ID">Id do Produto</param>
        /// <param name="FPR_SEQ_REPETICAO">Sequencia de Repetição do Pedido</param>
        /// <param name="Data">Data e hora de emissão dos testes fisicos</param>
        /// <param name="ORIGEM">Origem da chamada do Metodo 'S' (Sistema) 'U' (Usuário)</param>
        /// <returns>OK ou Msg de erro</returns>
        public JsonResult EmitirLaudo(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO, string Data, String ORIGEM)
        {
            bool flag = true;
            StringBuilder msg = new StringBuilder();
            if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID) || String.IsNullOrEmpty(FPR_SEQ_REPETICAO) || String.IsNullOrEmpty(Data))
            {
                flag = false;
                msg.Append(" Pedido, Produto,Data de emissao e Sequencia de transformação devem ser informados corretamente, verifique os dados.");
            }
            if (String.IsNullOrEmpty(ORIGEM))
            {
                flag = false;
                msg.Append(" A origem da chamada ao método deve ser informada, 'S' ,'U' ");
            }
            if (flag)
            {
                using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
                {
                    MasterController mc = new MasterController();
                    List<object> listaItem = new List<object>();
                    var DataAux = Convert.ToDateTime(Data);
                    //--
                    LaudoTesteFisico _LaudoTesteFisico = new LaudoTesteFisico();
                    _LaudoTesteFisico = _LaudoTesteFisico.GerarLaudoTesteFisico(ORD_ID, ROT_PRO_ID, FPR_SEQ_REPETICAO, ObterUsuarioLogado().USE_ID, DataAux, ORIGEM);
                    listaItem.Add(_LaudoTesteFisico);
                    //--
                    //Persintindo Objetos
                    List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                    List<LogPlay> Logs = mc.UpdateData(ListOfListObjects, 0, true);
                    if (Logs.First().GetLogsErro(Logs).Any())
                    {
                        msg.AppendLine("Os seguintes erros foram encontrados na emissão do Laudo:");
                        foreach (var item in Logs.First().GetLogsErro(Logs))
                        {
                            msg.Append(item.NomeAtributo);
                            msg.Append("-");
                            msg.Append(item.MsgErro);
                            msg.AppendLine();
                        }
                    }
                    else { msg.Append("OK"); }
                }
            }
            return Json(msg.ToString());
        }
        #endregion

        #region ComboBox
        public ActionResult RetornarTurmas()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var dados = db.Turma.AsNoTracking().Select(x => new { x.Id, x.Descricao }).ToList();

                return Json(new { dados });
            }

        }
        public ActionResult RetornarTurnos()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var dados = db.Turno.AsNoTracking().Select(x => new { x.Id, x.Descricao }).ToList();
                return Json(new { dados });
            }
        }
        public ActionResult RetornarUsuarios()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var dados = db.T_Usuario.AsNoTracking().Select(x => new { Id = x.USE_ID, Descricao = x.USE_NOME }).ToList();


                return Json(new { dados });
            }

        }
        public ActionResult RetornarMaquinas()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var dados = db.Maquina.AsNoTracking().Select(x => new { Id = x.MAQ_ID, Descricao = x.MAQ_DESCRICAO }).ToList();
                return Json(new { dados });
            }
        }
        public ActionResult RetornarProdutos()
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var dados = db.Produto.AsNoTracking().Select(x => new { Id = x.PRO_ID, Descricao = x.PRO_DESCRICAO }).ToList();
                return Json(new { dados });
            }
        }
        public ActionResult RetornarTiposTestes(string TemId)
        {
            List<int>
                TestesIds = new List<int>();
            int TEM_ID = Convert.ToInt32(TemId);
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                // Testes que já estão no template
                var TestesNoTemplate = (from ttt in db.TemplateTipoTeste
                                        join tt in db.TipoTeste on ttt.TT_ID equals tt.TT_ID
                                        where ttt.TEM_ID == TEM_ID
                                        select new { Id = tt.TT_ID, Descricao = tt.TT_NOME }).ToList();
                TestesIds = TestesNoTemplate.Select(x => x.Id).ToList();
                var dados = db.TipoTeste.Where(x => !TestesIds.Contains(x.TT_ID)).Select(x => new { Id = x.TT_ID, Descricao = x.TT_NOME }).ToList();
                return Json(new { dados, TestesNoTemplate });
            }
        }
        public ActionResult RetornarTiposInspecao(string TemId)
        {
            int TEM_ID = Convert.ToInt32(TemId);

            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var TestesIds = db.TemplateTipoInspecaoVisual.AsNoTracking().Where(x => x.TEM_ID.Equals(TEM_ID)).Select(x => x.TIV_ID).ToList(); // Retorna os Ids do testes visuais de acordo com o template
                var TestesNoTemplate = db.TipoInspecaoVisual.Where(x => TestesIds.Contains(x.TIV_ID)).Select(x => new { Id = x.TIV_ID, Descricao = x.TIV_NOME }).ToList();

                var dados = db.TipoInspecaoVisual.AsNoTracking().Where(x => !TestesIds.Contains(x.TIV_ID)).Select(x => new { Id = x.TIV_ID, Descricao = x.TIV_NOME }).ToList();
                return Json(new { dados, TestesNoTemplate });
            }
        }
        #endregion

        public ActionResult AmostrasDoTipoTeste(String ord_Id, String pro_Id, String seq_Rep, String Nome, String Data) // Retorna as Amostras do tipo Teste, para exibir no gerenciamento de teste
        {                                                                                                               // Chamada quando o User quer excluir uma amostra da OP
            var DataAux = Convert.ToDateTime(Data);
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                var result = db.TesteFisico.Where(x => x.ORD_ID.Equals(ord_Id) &&
                                                    x.FPR_SEQ_REPETICAO == Convert.ToInt32(seq_Rep)
                                                    && x.ROT_PRO_ID.Equals(pro_Id)
                                                    && x.TES_EMISSAO == DataAux
                                                    && x.TES_NOME_TECNICO.Equals(Nome)).ToList();
                return Json(result);
            }
        }

        public ActionResult ExcluirTeste(string[] IdTestes, String ord_Id, String pro_Id, String seq_Rep, int tipo) // exclui uma Amostra ou todo o testes daquele tipo de teste
        {
            MasterController mc = new MasterController();
            List<object> listaItem = new List<object>();
            List<int> TestesIds = new List<int>();
            foreach (var item in IdTestes)
            {
                int.TryParse(item, out var idAux);
                if (idAux != 0)
                    TestesIds.Add(idAux);
            }
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                if (TestesIds.Count > 0)
                {
                    if (tipo == 1) // apenas um testes
                    {
                        foreach (var item in TestesIds)
                        {
                            TesteFisico tf = new TesteFisico() { TES_ID = item, PlayAction = "delete", PlayMsgErroValidacao = "" };
                            listaItem.Add(tf);
                        }
                    }
                    else // todos os testes
                    {
                        var nome = db.TesteFisico.AsNoTracking().Where(x => x.TES_ID == TestesIds[0]).Select(x => x.TES_NOME_TECNICO).FirstOrDefault();
                        var aux = db.TesteFisico.AsNoTracking().Where(x => x.TES_NOME_TECNICO.Equals(nome) && x.ORD_ID.Equals(ord_Id) && x.ROT_PRO_ID.Equals(pro_Id) && x.FPR_SEQ_REPETICAO == Convert.ToInt32(seq_Rep)).ToList();

                        foreach (var item in aux)
                        {
                            TesteFisico tf = new TesteFisico() { TES_ID = item.TES_ID, PlayAction = "delete", PlayMsgErroValidacao = "" };
                            listaItem.Add(tf);
                        }
                    }
                    List<List<object>> ListOfListObjects = new List<List<object>>() { listaItem };
                    _ = mc.UpdateData(ListOfListObjects, 0, true);//Excluindo objeto o lista deles...
                    return Json(true);
                }
                else
                    return Json(false);
            }
        }

        public ActionResult VerificaTesteNaoColetado(string Ord_Id, string Prod_Id, string Maq_Id, string Seq_Rep, string Seq_Trans) // Verifica quando apontar a produção se há testes nao Coletados
        {
            TesteFisico _TesteFisico = new TesteFisico();
            bool flag = _TesteFisico.TestesNaoColetados(Ord_Id, Prod_Id, Seq_Rep) > 0;
            return Json(flag);
        }

        public void GerarAmostrasTesteFisico(string idOrd, string idPro, string idMaq, string seqRep, string seqTrans, String Turno, String Turma, int UserL, string Obs, bool coletado, string Tem_id)
        {
            TesteFisico testeFisico = new TesteFisico();
            testeFisico.GerarAmostrasTesteFisico(idOrd, idPro, idMaq, seqRep, seqTrans, Turno, Turma, UserL, Obs, coletado, Tem_id);
        }

    }
}
