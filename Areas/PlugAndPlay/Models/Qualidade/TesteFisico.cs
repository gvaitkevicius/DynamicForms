using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TesteFisico
    {

        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO")] [Required(ErrorMessage = "Campo TES_ID requirido.")] public int TES_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "USUARIO")] public int? USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME_TECNICO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TES_NOME_TECNICO")] public string TES_NOME_TECNICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_NUMERICO")] public double? TES_VALOR_NUMERICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_DATA")] public DateTime TES_VALOR_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR_TEXTO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TES_VALOR_TEXTO")] public string TES_VALOR_TEXTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMISSAO")] public DateTime TES_EMISSAO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS_LIBERACAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TES_STATUS_LIBERACAO")] public string TES_STATUS_LIBERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANSFORMACAO")] public int? ROT_SEQ_TRANSFORMACAO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Maquina")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO TESTE")] public int? TT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURNO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo TES_OBS")] public string TES_OBS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TURMA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESULT_LOTE")] public int? RL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO AVALIACAO")] public int? TA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ULTIMA_ALTERACAO")] public DateTime DATA_ULTIMA_ALTERACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual TipoAvaliacao TipoAvaliacao { get; set; }
        public virtual ResultLote ResultLote { get; set; }
        public virtual TipoTeste TipoTeste { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual Turma Turma { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }

        public double? NumeroMininoColetas(string idOrd, string idPro, string idMaq, string seqRep, string seqTrans)
        {
            List<double> papelEChapa = new List<double>() { 1, 2, 5 };
            List<double> caixas = new List<double>() { 3, 4 };
            bool check = true;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (String.IsNullOrEmpty(idOrd) || String.IsNullOrEmpty(idPro) || String.IsNullOrEmpty(idPro) || String.IsNullOrEmpty(idMaq) || String.IsNullOrEmpty(seqRep) || String.IsNullOrEmpty(seqTrans))
                {
                    check = false;
                }

                if (check)
                {
                    var pedidoDb = (from ped in db.Order
                                    join pro in db.Produto on ped.PRO_ID equals pro.PRO_ID
                                    join grp in db.GrupoProduto on pro.GRP_ID equals grp.GRP_ID
                                    where ped.ORD_ID.Equals(idOrd)
                                    select new { grp.GRP_TIPO, ped.ORD_QUANTIDADE })
                                     .AsNoTracking()
                                     .FirstOrDefault();

                    if (papelEChapa.Contains(pedidoDb.GRP_TIPO)) //CHAPAS OU BOBINAS
                    {
                        var dbPlano = db.PlanoAmostralTeste.AsNoTracking().Where(pa => pa.PAT_PERCENT_ESPECIF != null).FirstOrDefault();
                        if (dbPlano.PAT_PERCENT_ESPECIF != null)
                            return dbPlano.PAT_PERCENT_ESPECIF;
                    }
                    if (caixas.Contains(pedidoDb.GRP_TIPO)) //caixa
                    {
                        var dbPlano = db.PlanoAmostralTeste.AsNoTracking().Where(pa => pedidoDb.ORD_QUANTIDADE >= pa.PAT_QTD_CAIXAS_DE && pedidoDb.ORD_QUANTIDADE <= pa.PAT_QTD_CAIXAS_ATE).FirstOrDefault();
                        if (dbPlano.PAT_N_AMOSTRAGEM != null)
                            return dbPlano.PAT_N_AMOSTRAGEM;
                    }
                }
            }
            return -1;
        }

        public int PreencherFechamento(string idOrd, string idPro, string idMaq, string seqTrans, string seqRep)
        {
            int _Fech = -1;
            if (!String.IsNullOrEmpty(idOrd))
            {
                if (!String.IsNullOrEmpty(idPro))
                {
                    if (!String.IsNullOrEmpty(idMaq))
                    {
                        using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                        {
                            var teste = db.ViewFilaProducao.AsNoTracking().Where(x => x.OrdId.Equals(idOrd)
                                                                           && x.PaProId.Equals(idPro)
                                                                           && x.RotMaqId.Equals(idMaq)
                                                                           && x.FprSeqRepeticao == Convert.ToInt32(seqRep)// SEQ_TRANSFORMACAO
                                                                           && x.RotSeqTransformacao == Convert.ToInt32(seqTrans)).FirstOrDefault();

                            string GRP_ID = db.Produto.AsNoTracking().Include(p => p.GrupoProduto).Where(p => p.PRO_ID.Equals(teste.PaProId)).Select(x => x.GRP_ID).FirstOrDefault();
                            _Fech = db.FechamentoTeste.AsNoTracking().Where(p => p.GRP_ID.Equals(GRP_ID)).Select(x => x.FEC_ID).FirstOrDefault();
                        }
                    }
                }
            }
            return _Fech;
        }

        public bool AvaliarSequenciaTeste(string[] testeid, string[] testevalor)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                double resultado = 0, auxItem = 0; string status = "";
                int idAux = 0;
                List<int> TestesIds = new List<int>();
                List<double> ValoresTestes = new List<double>();
                //Filtrando Array de Strings dos parâmetros e convertendo valores
                //-- Ids dos Testes
                foreach (var item in testeid)
                {
                    int.TryParse(item, out idAux);
                    if (idAux != 0)
                        TestesIds.Add(idAux);
                }
                //Valores  recuperados dos testes
                foreach (var item in testevalor)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        double.TryParse(item, out auxItem);
                        ValoresTestes.Add(auxItem);
                    }
                }
                //--
                var Db_TesteFisico = db.TesteFisico
                    .Include(t => t.TipoAvaliacao)
                    .Include(t => t.TipoTeste)
                    .Include(t => t.ResultLote)
                    .AsNoTracking()
                    .Where(x => TestesIds.Contains(x.TES_ID))
                    .FirstOrDefault();
                //Buscando valor do tipo de avaliação para o conjunto de Amostras (mesmo tipo de teste)
                StringBuilder sb = new StringBuilder();
                sb.Append(Db_TesteFisico.TipoAvaliacao != null ? Db_TesteFisico.TipoAvaliacao.TA_DESC : "MEDIA");
                ;
                String casesw = sb.ToString();
                //Calculando resultado conforme o tipo de avaliação do conjunto
                switch (casesw)
                {
                    case "MAIOR":
                        resultado = ValoresTestes.Max();
                        break;
                    case "MENOR":
                        resultado = ValoresTestes.Min();
                        break;
                    case "MEDIA":
                        resultado = ValoresTestes.Average();
                        break;
                    case "VAL_FIXO":

                        break;
                }
                //Atribuindo Status conforme os limites estabelecidos no tipo do teste para o conjunto de amostras
                status = resultado >= (Db_TesteFisico.TipoTeste.TT_ESPECIFICACAO - Db_TesteFisico.TipoTeste.TT_TOL_MENOS) && resultado <= (Db_TesteFisico.TipoTeste.TT_ESPECIFICACAO + Db_TesteFisico.TipoTeste.TT_TOL_MAIS) ? "APROVADO" : "REPROVADO";
                if (status.Equals("APROVADO"))
                {
                    MasterController mc = new MasterController();

                    List<Object> listaItens = new List<object>();
                    var Db_Testes = db.TesteFisico
                   .AsNoTracking()
                   .Where(x => TestesIds.Contains(x.TES_ID))
                   .ToList();
                    ResultLote _ReuslLote = Db_TesteFisico.ResultLote;
                    foreach (var item in Db_Testes)
                    {
                        item.TES_STATUS_LIBERACAO = "APROVADO_SISTEMA";
                        item.PlayAction = "update";
                    }
                    _ReuslLote.RL_VALOR_ENCONTRADO = resultado;
                    _ReuslLote.RL_STATUS = "APROVADO_SISTEMA";
                    _ReuslLote.PlayAction = "update";
                    listaItens.AddRange(Db_Testes);
                    listaItens.Add(_ReuslLote);
                    List<LogPlay> Logs = mc.UpdateData(new List<List<object>>() { listaItens }, 0, true);//Persintindo Objetos
                }
                return status.Equals("APROVADO");
            }
        }

        public void GerarAmostrasTesteFisico(string idOrd, string idPro, string idMaq, string seqRep, string seqTrans, String Turno, String Turma, int User, string Obs, bool coletado, string Tem_id)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                List<int> TestesIds = new List<int>();
                DateTime _dataAux = DateTime.Now;
                string status_amostra = (coletado) ? "AMOSTRA_COLETADA" : "AMOSTRA_NAO_COLETADA";
                int cont = 0;
                var Db_Data = db.TesteFisico.Where(x => x.ORD_ID.Equals(idOrd) &&
                                                   x.FPR_SEQ_REPETICAO == Convert.ToInt32(seqRep) &&
                                                   x.ROT_SEQ_TRANSFORMACAO == Convert.ToInt32(seqTrans)
                                                   && x.ROT_PRO_ID.Equals(idPro)
                                                   && x.ROT_MAQ_ID.Equals(idMaq)).Select(x => x.TES_EMISSAO).FirstOrDefault(); ;
                _dataAux = (Db_Data != null) ? Db_Data : _dataAux;
                MasterController mc = new MasterController();
                List<object> listaItem = new List<object>();
                TesteFisico testeFisico = new TesteFisico();
                int TEM_ID = Convert.ToInt32(Tem_id);
                TestesIds = db.TemplateTipoTeste.Include(t => t.TipoTeste).AsNoTracking().Where(x => x.TEM_ID.Equals(TEM_ID)).Select(x => x.TT_ID).ToList();
                var tipo = db.TipoTeste.AsNoTracking().Where(x => TestesIds.Contains(x.TT_ID)).Select(x => new { x.TT_ID, x.TT_NOME, x.TA_ID, x.TT_N_AMOSTRAS_P_TESTE }).ToList();//Obtendo todos os TiposTeste 
                if (tipo.Count > 0)
                {
                    foreach (var item in tipo)//Iterando sobre os Tipos
                    {
                        for (int i = 0; i < item.TT_N_AMOSTRAS_P_TESTE; i++)
                        {
                            testeFisico.TES_NOME_TECNICO = item.TT_NOME;
                            testeFisico.PlayAction = "insert";
                            testeFisico.TT_ID = Convert.ToInt32(item.TT_ID);
                            testeFisico.TA_ID = item.TA_ID;
                            testeFisico.ORD_ID = idOrd;
                            testeFisico.ROT_PRO_ID = idPro;
                            testeFisico.ROT_MAQ_ID = idMaq;
                            testeFisico.FPR_SEQ_REPETICAO = Convert.ToInt32(seqRep);
                            testeFisico.ROT_SEQ_TRANSFORMACAO = seqTrans != "" ? Convert.ToInt32(seqTrans) : 0;
                            testeFisico.TES_STATUS_LIBERACAO = status_amostra;
                            testeFisico.TURN_ID = Turno;
                            testeFisico.TURM_ID = Turma;
                            testeFisico.USE_ID = User;
                            testeFisico.TES_OBS = Obs ?? "";
                            testeFisico.TES_EMISSAO = _dataAux;
                            testeFisico.DATA_ULTIMA_ALTERACAO = _dataAux;
                            listaItem.Add(testeFisico);
                            testeFisico = new TesteFisico();
                        }
                        cont++;
                    }
                    List<LogPlay> logs = mc.UpdateData(new List<List<object>>() { listaItem }, 0, true);//Persintindo Objetos
                    if (!String.IsNullOrEmpty(idMaq) && !String.IsNullOrEmpty(seqRep))
                        CriarLoteTeste(idOrd, idPro, Convert.ToInt32(seqRep));
                }
            }
        }

        public void CriarLoteTeste(string idOrd, string idPro, int seqRep)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                DateTime _dataAux = DateTime.Now;
                MasterController mc = new MasterController();
                List<object> lista = new List<object>();
                var teste = db.TesteFisico.AsNoTracking().Include(x => x.TipoTeste).Where(x => x.ORD_ID.Equals(idOrd)
                                                                           && x.ROT_PRO_ID.Equals(idPro)
                                                                           && x.FPR_SEQ_REPETICAO.Equals(seqRep)).ToList();

                ResultLote resultLote = new ResultLote
                {
                    RL_ID = 0,
                    PlayAction = "insert",
                    RL_STATUS = "AGUARDANDO"
                };

                lista.Add(resultLote);
                List<List<object>> ListOfListObjects = new List<List<object>>() { lista };
                List<LogPlay> logPlay = mc.UpdateData(ListOfListObjects, 0, true);
                EntityEntry entry = db.Entry(resultLote);
                resultLote = (ResultLote)entry.GetDatabaseValues().ToObject();
                lista = new List<object>();
                foreach (var item in teste)
                {
                    item.PlayAction = "update";
                    item.DATA_ULTIMA_ALTERACAO = _dataAux;
                    item.RL_ID = resultLote.RL_ID;
                    lista.Add(item);
                }
                logPlay = mc.UpdateData(new List<List<object>>() { lista }, 0, true);
            }
        }
        /// <summary>
        /// Verifica se uma OP possui Amostras de Testes programadas e não coletadas 
        /// </summary>
        /// <param name="ORD_ID">Id do Pedido</param>
        /// <param name="ROT_PRO_ID">Id do Produto</param>
        /// <param name="FPR_SEQ_REPETICAO">Sequencia de Repetição do Pedido</param>
        /// <returns>(-1) erro passagem de parâmetros, nº total de amostras não coletadas</returns>
        public int TestesNaoColetados(string ORD_ID, string ROT_PRO_ID, string FPR_SEQ_REPETICAO)
        {
            if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID) || String.IsNullOrEmpty(FPR_SEQ_REPETICAO))
            {
                return -1;
            }
            else
            {
                using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
                {
                    int result;
                    try
                    {
                        result = db.TesteFisico.Count(x => x.ORD_ID.Equals(ORD_ID) &&
                                                             x.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO)
                                                             && x.ROT_PRO_ID.Equals(ROT_PRO_ID)
                                                             && x.TES_STATUS_LIBERACAO.Equals("AMOSTRA_NAO_COLETADA"));
                    }
                    catch { result = -1; }
                    return result;
                }
            }
        }

        /// <summary>
        /// Retorna a somatoria do nº de amostras de testes a serem coletados dado o código de um Template de testes
        /// </summary>
        /// <param name="_TemplateTestes"></param>
        /// <returns></returns>
        public int QuantidadeTipoTeste(int _TemplateTestes)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                //quantidade = db.TemplateTipoTeste.Include(t => t.TipoTeste).AsNoTracking().Where(x => x.TEM_ID.Equals(_TemplateTestes)).Sum(t => t.TipoTeste.TT_N_AMOSTRAS_P_TESTE).Value;
                return (from ttt in db.TemplateTipoTeste
                        join tt in db.TipoTeste on ttt.TT_ID equals tt.TT_ID
                        where ttt.TEM_ID == _TemplateTestes
                        select tt.TT_N_AMOSTRAS_P_TESTE.Value).Sum();
            }
        }
    }
}
