using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NPOI.HSSF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "ETIQUETAS")]
    public class Etiqueta
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE PALETE")] public double? ETI_QUANTIDADE_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD IMPRESSORA")] public int? IMP_ID { get; set; }//Pendencia mapear chave estrangeira 
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IMPRIMIR DE")] public int? ETI_IMPRIMIR_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IMPRIMIR ATÉ")] public int? ETI_IMPRIMIR_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NÚM CÓPIAS")] public int? ETI_NUMERO_COPIAS { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD BARRAS")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ETI_CODIGO_BARRAS")] public string ETI_CODIGO_BARRAS { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo ETI_STATUS")] public string ETI_STATUS { get; set; }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQUÊNCIA")] public int? ETI_SEQUENCIA { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "COD ETIQUETAS")] [Required(ErrorMessage = "Campo ETI_ID requirido.")] public int ETI_ID { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "EMISSÃO")] public DateTime ETI_EMISSAO { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "DATA FABRICAÇÃO")] public DateTime ETI_DATA_FABRICACAO { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "COD BARRAS ORIGINAL")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ETI_COD_BARRAS_ORIGINAL")] public string ETI_COD_BARRAS_ORIGINAL { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "OP ORIGINAL")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ETI_OP_ORIGINAL")] public string ETI_OP_ORIGINAL { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "USUÁRIO")] public int? USE_ID { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "LOTE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ETI_LOTE")] public string ETI_LOTE { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "SUB LOTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ETI_SUB_LOTE")] public string ETI_SUB_LOTE { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual T_Usuario Usuario { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Order Order { get; set; }
        [HIDDEN]

        public Etiqueta GerarEtiquetaLoteProduto(string produto, string lote, string sublote, int seqRep, List<LogPlay> Logs, int idUsuario, int nCop = 1)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                MasterController mc = new MasterController();
                Etiqueta etiqueta = new Etiqueta();
                List<object> etiquetaGerada = new List<object>();
                #region Validacoes gerais
                if (idUsuario > 0)
                {
                    mc.UsuarioLogado = db.T_Usuario.Where(x => x.USE_ID == idUsuario).FirstOrDefault();
                    if (mc.UsuarioLogado == null)
                    {
                        Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = $"O ID de Usuário [{idUsuario}] não foi cadastrado,informe um id válido" });
                    }
                }
                else
                {
                    Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = "Voce deve infomar um usuário logado." });
                }

                //selecionando uma impressora para com maquina associada para satisfazer a FK na Etiqueta
                var impressoraDaMaquina = db.MaquinaImpressora.AsNoTracking()
                    .Select(x => new { x.IMP_ID, x.MAQ_ID })
                    .FirstOrDefault();
                
                if (impressoraDaMaquina == null)
                {
                    Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = $"Não há nenhuma impressora associada a uma maquina no sistema" });
                }
                #endregion
                #region Montando etiqueta
                if (!Logs.Any(x => x.Status.Equals("ERRO")))
                {
                    //Consultando saldo do lote do produto informado
                    var saldo = db.SaldosEmEstoquePorLote.AsNoTracking()
                        .Where(x => x.PRO_ID.Equals(produto) && x.MOV_LOTE.Equals(lote) && x.MOV_SUB_LOTE.Equals(sublote))
                        .Select(x => new { SALDO = x.SALDO.Value, PEDIDO = x.ORD_ID }).FirstOrDefault();

                    //Verificando se é chapa
                    var produtoDb = db.Produto.AsNoTracking().Where(p => p.PRO_ID == produto)
                                    .Select(p => new Produto
                                    {
                                        PRO_ID = p.PRO_ID,
                                        PRO_ID_CHAPA = p.PRO_ID_CHAPA
                                    })
                                    .FirstOrDefault();
                    
                    if (saldo != null && saldo.SALDO >= 0)
                    {
                        string prefixoCodBarras = "";
                        if (String.IsNullOrEmpty(produtoDb.PRO_ID_CHAPA))
                            prefixoCodBarras = "CH.";

                        var pedidoStr = String.IsNullOrWhiteSpace(saldo.PEDIDO) ? "PEDIDO N/D" : saldo.PEDIDO;
                        
                        etiqueta = db.Etiqueta.AsNoTracking()
                            .Where(e => e.ETI_LOTE == lote && e.ETI_SUB_LOTE == sublote && e.ROT_PRO_ID == produto).FirstOrDefault();

                        if (etiqueta != null)
                        { // ja existe etiqueta para este lote e sublote
                            etiqueta.ETI_ID = 0;
                            etiqueta.ORD_ID = saldo.PEDIDO;
                            etiqueta.ETI_EMISSAO = DateTime.Now;
                            etiqueta.ETI_STATUS = String.Empty;
                            etiqueta.ETI_QUANTIDADE_PALETE = saldo.SALDO;
                            etiqueta.PlayAction = "insert";
                        }
                        else
                        {
                            etiqueta = db.Etiqueta.AsNoTracking()
                            .Where(e => e.ETI_LOTE == lote && e.ROT_PRO_ID == produto).FirstOrDefault();

                            if (etiqueta != null)
                            {// é o mesmo lote e produto, só mudou o sublote
                                etiqueta.ETI_ID = 0;
                                etiqueta.ETI_EMISSAO = DateTime.Now;
                                etiqueta.ETI_STATUS = String.Empty;
                                etiqueta.ETI_QUANTIDADE_PALETE = saldo.SALDO;
                                etiqueta.ETI_CODIGO_BARRAS = $"{prefixoCodBarras}{pedidoStr}.{seqRep}.{sublote}";
                                etiqueta.ETI_SUB_LOTE = sublote;
                                etiqueta.ETI_SEQUENCIA = int.Parse(sublote);
                                ETI_SEQUENCIA = int.Parse(sublote);
                                etiqueta.PlayAction = "insert";
                            }
                            else
                            {
                                etiqueta = new Etiqueta()
                                {
                                    ETI_ID = 0,
                                    ROT_PRO_ID = produto,
                                    ETI_QUANTIDADE_PALETE = saldo.SALDO,
                                    ETI_CODIGO_BARRAS = $"{prefixoCodBarras}{pedidoStr}.{seqRep}.{sublote}",
                                    ETI_LOTE = lote,
                                    ETI_SUB_LOTE = sublote,
                                    ETI_IMPRIMIR_DE = 1,
                                    ETI_IMPRIMIR_ATE = 1,
                                    ETI_EMISSAO = DateTime.Now,
                                    ETI_DATA_FABRICACAO = DateTime.Now,
                                    ETI_NUMERO_COPIAS = nCop,
                                    ETI_STATUS = String.Empty,
                                    MAQ_ID = impressoraDaMaquina.MAQ_ID,
                                    ORD_ID = pedidoStr.Equals("PEDIDO N/D") ? "" : pedidoStr,
                                    FPR_SEQ_REPETICAO = seqRep,
                                    IMP_ID = impressoraDaMaquina.IMP_ID,
                                    ETI_SEQUENCIA = int.Parse(sublote),
                                    USE_ID = mc.UsuarioLogado.USE_ID,
                                    PlayAction = "insert",
                                    PlayMsgErroValidacao = "",
                                };
                            }
                        }
                        etiquetaGerada.Add(etiqueta);
                    }
                    else
                    {
                        Logs.Add(new LogPlay() { Status = "ERRO", MsgErro = $"O SALDO PARA O LOTE[{lote}] - SULOTE[{sublote}] E PRODUTO [{produto}] é {saldo}" });
                    }

                }
                #endregion
                #region Persistindo Etiqueta
                if (!Logs.Any(x => x.Status.Equals("ERRO")))
                {
                    Logs.AddRange(mc.UpdateData(new List<List<object>>() { etiquetaGerada }, 0, true));

                    if (!Logs.Any(x => x.Status.Equals("ERRO")))
                    {
                        EntityEntry entry = db.Entry(etiqueta);
                        etiqueta = (Etiqueta)entry.GetDatabaseValues().ToObject();
                    }
                    #endregion
                    return etiqueta;
                }
                else
                {
                    return null;
                }

            }
        }

        private InterfaceTelaImpressaoEtiquetas EtiquetaToInterfaceTelaImpressaoEtiquetas()
        {
            return new InterfaceTelaImpressaoEtiquetas()
            {
                ETI_EMISSAO = this.ETI_EMISSAO,
                ETI_SEQUENCIA = this.ETI_SEQUENCIA.Value,
                ETI_CODIGO_BARRAS = this.ETI_CODIGO_BARRAS,
                ETI_STATUS = this.ETI_STATUS,
                ETI_DATA_FABRICACAO = this.ETI_DATA_FABRICACAO,
                ETI_COD_BARRAS_ORIGINAL = this.ETI_COD_BARRAS_ORIGINAL,
                ETI_OP_ORIGINAL = this.ETI_OP_ORIGINAL,
                MAQ_ID = this.MAQ_ID,
                IMP_ID = this.IMP_ID,
                USE_ID = this.USE_ID,
                ORD_ID = this.ORD_ID,
                ROT_PRO_ID = this.ROT_PRO_ID,
                FPR_SEQ_REPETICAO = this.FPR_SEQ_REPETICAO,
                ETI_QUANTIDADE_PALETE = this.ETI_QUANTIDADE_PALETE,
                ETI_NUMERO_COPIAS = this.ETI_NUMERO_COPIAS,
                ETI_LOTE = this.ETI_LOTE,
                ETI_SUB_LOTE = this.ETI_SUB_LOTE,
                ETI_IMPRIMIR_DE = this.ETI_IMPRIMIR_DE.Value,
                ETI_IMPRIMIR_ATE = this.ETI_IMPRIMIR_ATE.Value,
                PlayAction = this.PlayAction,
                PlayMsgErroValidacao = this.PlayMsgErroValidacao,
            };
        }
    }
    [Display(Name = "GERAR ETIQUETAS")]
    public class InterfaceTelaImpressaoEtiquetas : InterfaceDeTelas
    {
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Order")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEDIDO")] public string ORD_ID { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Produto")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRODUTO")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE POR PALETE")] public double? ETI_QUANTIDADE_PALETE { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Impressora")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO DA IMPRESSORA")] public int? IMP_ID { get; set; }//Pendencia mapear chave estrangeira 
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GERAR SEQUENCIA DE")] public int ETI_IMPRIMIR_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GERAR SEQUENCIA ATE")] public int ETI_IMPRIMIR_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NUMERO_COPIAS")] public int? ETI_NUMERO_COPIAS { get; set; }
        [Combobox(Description = "SIM", Value = "S")]
        [Combobox(Description = "NÃO", Value = "N")]
        //[TAB(Value = "PRINCIPAL")] [Display(Name = "IMPRIMIR_AGORA")] public string IMPRIMIR_AGORA { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.Maquina")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "LOTE")] public string ETI_LOTE { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "SUB_LOTE")] public string ETI_SUB_LOTE { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "ID")] [AUTOINCREMENT] public int ETI_ID { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "DATA DE EMISSAO")] public DateTime ETI_EMISSAO { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "CODIGO_BARRAS")] public string ETI_CODIGO_BARRAS { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "ETI_SEQUENCIA")] public int ETI_SEQUENCIA { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "STATUS")] public string ETI_STATUS { get; set; }
        [READ] [TAB(Value = "OUTROS")] [Display(Name = "DATA DE FABRICACAO")] public DateTime ETI_DATA_FABRICACAO { get; set; }
        [READ] [TAB(Value = "OUTROS")] public string ETI_COD_BARRAS_ORIGINAL { get; set; }
        [READ] [TAB(Value = "OUTROS")] public string ETI_OP_ORIGINAL { get; set; }
        [READ] [TAB(Value = "OUTROS")] public int? USE_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public T_Usuario UsuarioLogado { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Order Order { get; set; }
        //Construtor
        public InterfaceTelaImpressaoEtiquetas()
        {
            NamespaceOfClassMapped = typeof(Etiqueta).FullName;
        }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            int imprimirDe = 1;
            int imprimirAte = 1;
            List<object> etiquetasParaPersistir = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    if (item.GetType().Name != nameof(InterfaceTelaImpressaoEtiquetas))
                        continue;

                    InterfaceTelaImpressaoEtiquetas etiquetaUI = (InterfaceTelaImpressaoEtiquetas)item;
                    //ORIGEM DA ETIQUETA É DA AÇAO RELACIONADA IMPRMIRIR
                    switch (etiquetaUI.PlayAction)
                    {
                        case "insert":
                            etiquetaUI.PlayAction = "OK";

                            if (etiquetaUI.IMP_ID == 0 || etiquetaUI.IMP_ID == null)
                            {
                                etiquetaUI.PlayMsgErroValidacao = $"Informe a impressora para esta etiqueta.";
                                return false;
                            }

                            //Verificando etiquetas existentes
                            List<Etiqueta> etiquetasExistentes = db.Etiqueta.AsNoTracking()
                                .Where(e => e.ORD_ID == etiquetaUI.ORD_ID && e.ROT_PRO_ID == etiquetaUI.ROT_PRO_ID)
                                .ToList();

                            imprimirDe = etiquetaUI.ETI_IMPRIMIR_DE;
                            imprimirAte = etiquetaUI.ETI_IMPRIMIR_ATE;
                                
                            //validadndo STATUS e quantidades informadas
                            int ultimaSequenciaApontada = etiquetasExistentes
                                    .Where(x => x.ETI_STATUS.Trim().Equals("P"))
                                    .Max(x => x.ETI_SEQUENCIA).GetValueOrDefault(0);

                            if (ultimaSequenciaApontada > 0 && imprimirDe <= ultimaSequenciaApontada)
                            {
                                etiquetaUI.PlayMsgErroValidacao = $"A última sequência apontada foi a [{ultimaSequenciaApontada}], portando o campo GERAR SEQUENCIA DE deve ser maior que [{ultimaSequenciaApontada}].";
                                return false;
                            }
                                
                            var filaProducao = (from fila in db.FilaProducao
                                                join pedido in db.Order on fila.ORD_ID equals pedido.ORD_ID
                                                where fila.ORD_ID == etiquetaUI.ORD_ID &&
                                                        fila.ROT_PRO_ID == etiquetaUI.ROT_PRO_ID &&
                                                        fila.FPR_SEQ_REPETICAO == etiquetaUI.FPR_SEQ_REPETICAO
                                                select new { fila.ORD_ID, pedido.ORD_TIPO, fila.FPR_DATA_FIM_PREVISTA, 
                                                        fila.FPR_SEQ_REPETICAO, fila.ROT_SEQ_TRANFORMACAO, fila.ROT_PRO_ID })
                                                .FirstOrDefault();

                            if (filaProducao == null)
                            {
                                etiquetaUI.PlayMsgErroValidacao = "Ordem de produção não encontrada.";
                                return false;
                            }
                            if (filaProducao.ORD_TIPO == 3)
                            {
                                etiquetaUI.PlayMsgErroValidacao = $"A etiqueta não pode ser emitida, o pedido informado [{filaProducao.ORD_ID }]  é do tipo Somente Faturamento.";
                                return false;
                            }

                            if (imprimirDe < 1)
                            {
                                etiquetaUI.PlayMsgErroValidacao = "Sequência inváLida, o campo GERAR SEQUENCIA DE deve ser maior ou igual a 1.";
                                return false;
                            }
                            
                            if (imprimirDe > imprimirAte)
                            {
                                etiquetaUI.PlayMsgErroValidacao = "Sequência inváLida, o campo GERAR SEQUENCIA DE deve ser menor ou igual ao campo GERAR SEQUENCIA ATÉ";
                                return false;
                            }

                            int proximaSequencia;
                            if (ultimaSequenciaApontada > 0)
                                proximaSequencia = ultimaSequenciaApontada + 1;
                            else
                                proximaSequencia = imprimirDe;


                            // Apagando as etiquetas que já existem e não foram apontadas
                            var etiquetasParaExcluir = etiquetasExistentes
                                .Where(e => e.ETI_SEQUENCIA >= proximaSequencia)
                                .ToList();
                            etiquetasParaExcluir.ForEach(e => e.PlayAction = "delete");

                            etiquetasParaPersistir.AddRange(etiquetasParaExcluir);


                            //Verificando se é chapa
                            var produto = db.Produto.AsNoTracking().Where(p => p.PRO_ID == filaProducao.ROT_PRO_ID)
                                            .Select(p => new Produto 
                                            {
                                                PRO_ID = p.PRO_ID,
                                                PRO_ID_CHAPA = p.PRO_ID_CHAPA
                                            })
                                            .FirstOrDefault();

                            string prefixoCodBarras = "";
                            if (String.IsNullOrEmpty(produto.PRO_ID_CHAPA))
                                prefixoCodBarras = "CH.";

                            //Definindo a quantidade de etiquetas
                            int qtdEtiquetas = (imprimirAte - imprimirDe) + 1;
                            for (int i = imprimirDe; i < imprimirDe + qtdEtiquetas; i++)
                            {
                                Etiqueta etiquetaAux = new Etiqueta
                                {
                                    ETI_SEQUENCIA = proximaSequencia,
                                    ETI_EMISSAO = DateTime.Now,
                                    ETI_CODIGO_BARRAS = $"{prefixoCodBarras}{etiquetaUI.ORD_ID.Trim()}.{filaProducao.FPR_SEQ_REPETICAO}.{proximaSequencia}",
                                    ETI_STATUS = "",
                                    ETI_DATA_FABRICACAO = filaProducao.FPR_DATA_FIM_PREVISTA,
                                    ETI_COD_BARRAS_ORIGINAL = etiquetaUI.ETI_COD_BARRAS_ORIGINAL,
                                    MAQ_ID = etiquetaUI.MAQ_ID,
                                    IMP_ID = etiquetaUI.IMP_ID,
                                    USE_ID = etiquetaUI.UsuarioLogado.USE_ID,
                                    ORD_ID = filaProducao.ORD_ID,
                                    ROT_PRO_ID = filaProducao.ROT_PRO_ID,
                                    FPR_SEQ_REPETICAO = filaProducao.FPR_SEQ_REPETICAO,
                                    ETI_QUANTIDADE_PALETE = etiquetaUI.ETI_QUANTIDADE_PALETE,
                                    ETI_NUMERO_COPIAS = (etiquetaUI.ETI_NUMERO_COPIAS == null) ? 2 : (int)etiquetaUI.ETI_NUMERO_COPIAS,
                                    ETI_LOTE = $"{prefixoCodBarras}{etiquetaUI.ORD_ID.Trim()}",
                                    ETI_SUB_LOTE = $"{proximaSequencia}",
                                    ETI_IMPRIMIR_DE = etiquetaUI.ETI_IMPRIMIR_DE,
                                    ETI_IMPRIMIR_ATE = etiquetaUI.ETI_IMPRIMIR_ATE,
                                    PlayAction = "insert"
                                };
                                etiquetasParaPersistir.Add(etiquetaAux);

                                proximaSequencia++; // Incrementa a última sequência da etiqueta
                            }
                            
                            break;

                        case "delete":
                            Etiqueta etiAux = db.Etiqueta.AsNoTracking()
                                .Where(x => x.ETI_ID == etiquetaUI.ETI_ID).FirstOrDefault();
                                
                            if (etiAux.ETI_STATUS.Equals("P"))
                            {
                                var movReserva = db.MovimentoEstoqueReservaDeEstoque.AsNoTracking().Where(x => x.MOV_LOTE.Equals(etiAux.ETI_CODIGO_BARRAS) && String.IsNullOrEmpty(x.MOV_ESTORNO)).FirstOrDefault();
                                if (movReserva != null)
                                {
                                    if (movReserva.CAR_ID != null)
                                    {
                                        etiquetaUI.PlayMsgErroValidacao = $"Esta etiqueta foi Romaneadana carga [{movReserva.CAR_ID}], utilize a função ROMANEAR CARGA e marque Excluir \n antes de excluir a etiqueta";
                                        return false;
                                    }
                                }
                                var movProducao = db.MovimentoEstoqueProducao.AsNoTracking().Where(x => x.MOV_LOTE.Equals(etiAux.ETI_CODIGO_BARRAS) && String.IsNullOrEmpty(x.MOV_ESTORNO)).FirstOrDefault();
                                if (movProducao != null)
                                {
                                    movProducao.MOV_ESTORNO = "E";
                                    movProducao.PlayAction = "update";
                                    etiquetasParaPersistir.Add(movProducao);
                                }

                                if (movReserva != null)
                                {
                                    movReserva.MOV_ESTORNO = "E";
                                    movReserva.PlayAction = "update";
                                    etiquetasParaPersistir.Add(movReserva);
                                }
                            }
                            etiAux.PlayAction = "delete";
                            etiquetasParaPersistir.Add(etiAux);
                            break;
                            
                        case "update":
                            etiquetaUI.PlayMsgErroValidacao = "Aleterações não são permitidas, você deve excluir e emitir uma nova etiqueta no sistema.";
                            etiquetaUI.PlayAction = "OK";
                            return false;

                    }
                }
                //Adicionando as estiquetas a lista de objects

                objects.AddRange(etiquetasParaPersistir);
            }
            return true;
        }

        public bool AfterChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            foreach (var item in objects)
            {
                if (item.GetType().Name != nameof(InterfaceTelaImpressaoEtiquetas))
                    continue;

                InterfaceTelaImpressaoEtiquetas etiqueta = (InterfaceTelaImpressaoEtiquetas)item;
                
                if (Logs.Any(l => l.Status.Equals("ERRO")))
                    return false;

                //Verificando etiquetas existentes
                var etiquetasAImprimir = db.Etiqueta.AsNoTracking().Where(e => e.ORD_ID == etiqueta.ORD_ID && e.ROT_PRO_ID == etiqueta.ROT_PRO_ID && e.FPR_SEQ_REPETICAO == etiqueta.FPR_SEQ_REPETICAO && e.ETI_SEQUENCIA >= etiqueta.ETI_IMPRIMIR_DE && e.ETI_SEQUENCIA <= etiqueta.ETI_IMPRIMIR_ATE).Select(x => x.ETI_ID).ToList();
                if (etiquetasAImprimir.Count > 0)
                {
                    string listaEt = String.Join(",", etiquetasAImprimir);
                    Logs.Add(ImprimirEt(listaEt));
                }
            }
            return true;
        }
        public bool Imprimir(List<object> objects, ref List<LogPlay> Logs)
        {
            bool check = true;
            foreach (var item in objects)
            {
                if (item.GetType().Name == "InterfaceTelaImpressaoEtiquetas")
                {
                    //Etiqueta da retornada da View
                    InterfaceTelaImpressaoEtiquetas etiqueta = (InterfaceTelaImpressaoEtiquetas)item;
                    try
                    {
                        using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                        {
                            string listaEt = String.Join(",", db.Etiqueta.AsNoTracking()
                               .Where(e => e.ETI_ID == etiqueta.ETI_ID)
                               .Select(x => x.ETI_ID).ToList());
                            if (!String.IsNullOrEmpty(listaEt))
                            {
                                Logs.Add(ImprimirEt(listaEt));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        etiqueta.PlayMsgErroValidacao = ex.Message;
                    }

                }
            }
            return check;
        }
        [HIDDEN]
        public LogPlay ImprimirEt(string etiquetas)
        {
            return (String.IsNullOrEmpty(etiquetas)) ? new LogPlay(this.ToString(), "ERRO", "Você deve passar uma lista de Id´s de etiquetas seprados po ','.") :
            new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/ReportEtiqueta/Etiquetas?listaEtiquetas=", $"{etiquetas}");
        }

        private Etiqueta InterfaceTelaImpressaoEtiquetasToEtiqueta(InterfaceTelaImpressaoEtiquetas InterfaceEtiquetas)
        {
            return new Etiqueta()
            {
                ETI_EMISSAO = InterfaceEtiquetas.ETI_EMISSAO,
                ETI_SEQUENCIA = InterfaceEtiquetas.ETI_SEQUENCIA,
                ETI_CODIGO_BARRAS = InterfaceEtiquetas.ETI_CODIGO_BARRAS,
                ETI_STATUS = InterfaceEtiquetas.ETI_STATUS,
                ETI_DATA_FABRICACAO = InterfaceEtiquetas.ETI_DATA_FABRICACAO,
                ETI_COD_BARRAS_ORIGINAL = InterfaceEtiquetas.ETI_COD_BARRAS_ORIGINAL,
                ETI_OP_ORIGINAL = InterfaceEtiquetas.ETI_OP_ORIGINAL,
                MAQ_ID = InterfaceEtiquetas.MAQ_ID,
                IMP_ID = InterfaceEtiquetas.IMP_ID,
                USE_ID = InterfaceEtiquetas.USE_ID,
                ORD_ID = InterfaceEtiquetas.ORD_ID,
                ROT_PRO_ID = InterfaceEtiquetas.ROT_PRO_ID,
                FPR_SEQ_REPETICAO = InterfaceEtiquetas.FPR_SEQ_REPETICAO,
                ETI_QUANTIDADE_PALETE = InterfaceEtiquetas.ETI_QUANTIDADE_PALETE,
                ETI_NUMERO_COPIAS = InterfaceEtiquetas.ETI_NUMERO_COPIAS,
                ETI_LOTE = InterfaceEtiquetas.ETI_LOTE,
                ETI_SUB_LOTE = InterfaceEtiquetas.ETI_SUB_LOTE,
                ETI_IMPRIMIR_DE = InterfaceEtiquetas.ETI_IMPRIMIR_DE,
                ETI_IMPRIMIR_ATE = InterfaceEtiquetas.ETI_IMPRIMIR_ATE,
                PlayAction = InterfaceEtiquetas.PlayAction,
                PlayMsgErroValidacao = InterfaceEtiquetas.PlayMsgErroValidacao,
            };
        }
        private List<EtiquetaGenerica> GerarEtiquetas(List<Etiqueta> etiquetas, JSgi _db)
        {
            List<EtiquetaGenerica> etiquetasGenericas = new List<EtiquetaGenerica>();

            Etiqueta etiquetaAtual = etiquetas.First();
            var infoPedido = (from fila in _db.FilaProducao
                              join pedido in _db.Order on fila.ORD_ID equals pedido.ORD_ID
                              join produto in _db.Produto on pedido.PRO_ID equals produto.PRO_ID
                              join cliente in _db.Cliente on pedido.CLI_ID equals cliente.CLI_ID
                              join cidade in _db.Municipio on cliente.MUN_ID equals cidade.MUN_ID
                              where fila.ORD_ID == etiquetaAtual.ORD_ID &&
                                                     fila.ROT_PRO_ID == etiquetaAtual.ROT_PRO_ID &&
                                                     fila.FPR_SEQ_REPETICAO == etiquetaAtual.FPR_SEQ_REPETICAO
                              select new
                              {
                                  fila.FPR_ID,
                                  fila.ORD_ID,
                                  cliente.CLI_NOME,
                                  cliente.CLI_ENDERECO_ENTREGA,
                                  cliente.CLI_BAIRRO_ENTREGA,
                                  cliente.Municipio.MUN_NOME,
                                  cliente.CLI_CEP_ENTREGA,
                                  pedido.PRO_ID,
                                  pedido.ORD_QUANTIDADE,
                                  produto.QTD_PALETE,
                                  pedido.ORD_DATA_ENTREGA_ATE,
                                  produto.PRO_DESCRICAO,
                                  produto.PRO_PECAS_POR_FARDO
                              }).AsNoTracking()
                                                              .FirstOrDefault();
            for (int i = 0; i < etiquetas.Count; i++)
            {
                etiquetaAtual = etiquetas[i];
                //Consultando dados para montagem da etiqueta 
                if (infoPedido != null)
                {
                    EtiquetaGenerica etiqueta = new EtiquetaGenerica()
                    {
                        CLI_NOME = infoPedido.CLI_NOME,
                        PRO_ID = infoPedido.PRO_ID,
                        PRO_DESCRICAO = infoPedido.PRO_DESCRICAO,
                        OF = $"{infoPedido.FPR_ID}-{infoPedido.ORD_ID}",
                        CLI_ENDERECO_ENTREGA = infoPedido.CLI_ENDERECO_ENTREGA,
                        ORD_DATA_ENTREGA_ATE = infoPedido.ORD_DATA_ENTREGA_ATE,
                        ETI_QUANTIDADE_PALETE = $"{etiquetaAtual.ETI_QUANTIDADE_PALETE}",
                        MAQ_ID = etiquetaAtual.MAQ_ID,
                        QTD_AMARRADOS = 0,
                        ETI_DATA_FABRICACAO = etiquetaAtual.ETI_DATA_FABRICACAO.ToShortDateString(),
                        ETI_CODIGO_BARRAS = etiquetaAtual.ETI_CODIGO_BARRAS,
                        ETI_LOTE = etiquetaAtual.ETI_LOTE,
                        ETI_SUB_LOTE = etiquetaAtual.ETI_SUB_LOTE
                    };
                    etiquetasGenericas.Add(etiqueta);
                }
            }
            return etiquetasGenericas;
        }
    }
}
