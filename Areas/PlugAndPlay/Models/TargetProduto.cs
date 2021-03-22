
using DynamicForms.Context;
using DynamicForms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "TARGETS DOS PRODUTOS")]
    public class TargetProduto
    {
        // PRINCIPAL
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD ALVO")] [Required(ErrorMessage = "Campo TAR_ID requirido.")] public int TAR_ID { get; set; }
        [Combobox(Description = "APROV PADRAO", Value = "AP")]
        [Combobox(Description = "AP GESTOR", Value = "AG")]
        [Combobox(Description = "AP AUTOMATIC", Value = "AA")]
        [Combobox(Description = "PEND GESTOR", Value = "PG")]
        [Combobox(Description = "REPROV SISTEMA", Value = "RS")]
        [Combobox(Description = "REPROVADO GESTOR", Value = "RG")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo TAR_APROVADO")] public string TAR_APROVADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MOVIMENTO ESTOQUE")] public int? MOV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PEDIDO")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MAQUINA")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ. TRANFORMAÇÃO")] public int? ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ. REPETIÇÃO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TURMA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURM_ID")] public string TURM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TURNO")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD USUÁRIO")] public int? USE_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "META PERFORMANCE")] [Required(ErrorMessage = "Campo TAR_META_PERFORMANCE requirido.")] public double TAR_META_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE REALIZADA")] public double? TAR_REALIZADO_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL PERF REALIZADA")] public double? TAR_PERCENTUAL_REALIZADO_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROX. META PERFORMANCE")] public double? TAR_PROXIMA_META_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "META SETUP")] [Required(ErrorMessage = "Campo TAR_META_TEMPO_SETUP requirido.")] public double TAR_META_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUP REALIZADO")] public double? TAR_REALIZADO_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROX. META SETUP")] public double? TAR_PROXIMA_META_TEMPO_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "META SETUP AJUSTE")] [Required(ErrorMessage = "Campo TAR_META_TEMPO_SETUP_AJUSTE requirido.")] public double TAR_META_TEMPO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SETUP AJUSTE REALIZADO")] public double? TAR_REALIZADO_TEMPO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PROX. META SETUP AJUSTE")] public double? TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PERFORMANCE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_PERFORMANCE")] public string OCO_ID_PERFORMANCE { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS. PERFORMANCE")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_PERFORMANCE")] public string TAR_OBS_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD SETUP")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_SETUP")] public string OCO_ID_SETUP { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS. SETUP")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_SETUP")] public string TAR_OBS_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD SETUP AJUSTE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo OCO_ID_SETUPA")] public string OCO_ID_SETUPA { get; set; }
        [TextArea] [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS. SETUP AJUSTE")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_SETUPA")] public string TAR_OBS_SETUPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FEEDBACK PERFORMANCE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_PERFORMANCE")] public string TAR_TIPO_FEEDBACK_PERFORMANCE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FEEDBACK SETUP")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_SETUP")] public string TAR_TIPO_FEEDBACK_SETUP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FEEDBACK SETUP AJUSTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TAR_TIPO_FEEDBACK_SETUP_AJUSTE")] public string TAR_TIPO_FEEDBACK_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD. SETUP AJUSTE")] public double? TAR_QTD_SETUP_AJUSTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] public double? TAR_QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANT PEÇAS/PULSO")] public double? FEE_QTD_PECAS_POR_PULSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANT PERDAS")] public double? TAR_QTD_PERDAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA INICIAL")] public DateTime TAR_DATA_INICIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA FINAL")] public DateTime TAR_DATA_FINAL { get; set; }
        // OUTROS
        [TAB(Value = "OUTROS")] [Display(Name = "DIA_TURMA")] [Required(ErrorMessage = "Campo TAR_DIA_TURMA requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo TAR_DIA_TURMA")] public string TAR_DIA_TURMA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD UNIDADE MEDIDA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PARAMETRO_TIME_WORK_STOP_MACHINE")] public int? TAR_PARAMETRO_TIME_WORK_STOP_MACHINE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PARAMETRO_TEMPO_QUEBRA_DE_LOTE")] public int? TAR_PARAMETRO_TEMPO_QUEBRA_DE_LOTE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PERFORMANCE_MAX_VERDE")] public double? TAR_PERFORMANCE_MAX_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PERFORMANCE_MIN_VERDE")] public double? TAR_PERFORMANCE_MIN_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUP_MAX_VERDE")] public double? TAR_SETUP_MAX_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUP_MIN_VERDE")] public double? TAR_SETUP_MIN_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUPA_MAX_VERDE")] public double? TAR_SETUPA_MAX_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUPA_MIN_VERDE")] public double? TAR_SETUPA_MIN_VERDE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "PERFORMANCE_MIN_AMARELO")] public double? TAR_PERFORMANCE_MIN_AMARELO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUP_MAX_AMARELO")] public double? TAR_SETUP_MAX_AMARELO { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "SETUPA_MAX_AMARELO")] public double? TAR_SETUPA_MAX_AMARELO { get; set; }
        [TextArea] [TAB(Value = "OUTROS")] [Display(Name = "OBS. OP. PARCIAL")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TAR_OBS_OP_PARCIAL")] public string TAR_OBS_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COD OP PARCIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo TAR_OCO_ID_OP_PARCIAL")] public string TAR_OCO_ID_OP_PARCIAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COR_PERFORMANCE")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TAR_COR_PERFORMANCE")] public string TAR_COR_PERFORMANCE { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COR_SETUP_GERAL")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TAR_COR_SETUP_GERAL")] public string TAR_COR_SETUP_GERAL { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COR_SETUP")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TAR_COR_SETUP")] public string TAR_COR_SETUP { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "COR_SETUPA")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TAR_COR_SETUPA")] public string TAR_COR_SETUPA { get; set; }
        [TAB(Value = "OUTROS")] [Display(Name = "DIA_TURMA_D")] public DateTime TAR_DIA_TURMA_D { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual MovimentoEstoque MovimentoEstoque { get; set; }
        public virtual Order Order { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Maquina Maquina { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual Turma Turma { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual T_Usuario Usuario { get; set; }
        public virtual Ocorrencia OcorrenciaPerformace { get; set; }
        public virtual Ocorrencia OcorrenciaSetup { get; set; }
        public virtual Ocorrencia OcorrenciaSetupAjuste { get; set; }

        public TargetProduto ObterUltimaMeta(string maquinaId, string produtoId, int? movId = null)
        {
            TargetProduto metaPerformace, metaSetup, metaSetupAjuste;
            TargetProduto target = null;
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                if (movId == null)
                {
                    metaPerformace = db.TargetProduto.AsNoTracking()
                        .Where(t => t.MAQ_ID == maquinaId && t.PRO_ID == produtoId &&
                            t.TAR_APROVADO.Substring(0, 1).Equals("A") && t.TAR_REALIZADO_PERFORMANCE != -1)
                        .OrderByDescending(t => t.MOV_ID).Take(1).FirstOrDefault();

                    metaSetup = metaPerformace;

                    metaSetupAjuste = metaPerformace;
                }
                else
                {
                    metaPerformace = db.TargetProduto.AsNoTracking()
                        .Where(t => (t.MOV_ID < movId || t.MOV_ID == null) && t.MAQ_ID == maquinaId &&
                            t.PRO_ID == produtoId && t.TAR_APROVADO.Substring(0, 1).Equals("A") &&
                            t.TAR_REALIZADO_PERFORMANCE != -1)
                        .OrderByDescending(t => t.MOV_ID).Take(1).FirstOrDefault();

                    metaSetup = metaPerformace;

                    metaSetupAjuste = metaPerformace;
                }
                if (metaPerformace != null && metaSetup != null && metaSetupAjuste != null)
                {
                    target = new TargetProduto()
                    {
                        TAR_PERFORMANCE_MIN_AMARELO = metaPerformace.TAR_PERFORMANCE_MIN_AMARELO,
                        TAR_PERFORMANCE_MIN_VERDE = metaPerformace.TAR_PERFORMANCE_MIN_VERDE,
                        TAR_PERFORMANCE_MAX_VERDE = metaPerformace.TAR_PERFORMANCE_MAX_VERDE,
                        TAR_REALIZADO_PERFORMANCE = metaPerformace.TAR_REALIZADO_PERFORMANCE,
                        TAR_PROXIMA_META_PERFORMANCE = metaPerformace.TAR_PROXIMA_META_PERFORMANCE,
                        TAR_SETUP_MIN_VERDE = metaSetup.TAR_SETUP_MIN_VERDE,
                        TAR_SETUP_MAX_VERDE = metaSetup.TAR_SETUP_MAX_VERDE,
                        TAR_SETUP_MAX_AMARELO = metaSetup.TAR_SETUP_MAX_AMARELO,
                        TAR_REALIZADO_TEMPO_SETUP = metaSetup.TAR_REALIZADO_TEMPO_SETUP,
                        TAR_PROXIMA_META_TEMPO_SETUP = metaSetup.TAR_PROXIMA_META_TEMPO_SETUP,
                        TAR_SETUPA_MIN_VERDE = metaSetupAjuste.TAR_SETUPA_MIN_VERDE,
                        TAR_SETUPA_MAX_VERDE = metaSetupAjuste.TAR_SETUPA_MAX_VERDE,
                        TAR_SETUPA_MAX_AMARELO = metaSetupAjuste.TAR_SETUPA_MAX_AMARELO,
                        TAR_REALIZADO_TEMPO_SETUP_AJUSTE = metaSetupAjuste.TAR_REALIZADO_TEMPO_SETUP_AJUSTE,
                        TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE = metaSetupAjuste.TAR_PROXIMA_META_TEMPO_SETUP_AJUSTE,
                        TAR_APROVADO = metaSetupAjuste.TAR_APROVADO
                    };
                }
                return target;
            }
        }

    }
}