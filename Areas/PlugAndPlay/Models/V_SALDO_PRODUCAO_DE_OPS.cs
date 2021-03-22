using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_SALDO_PRODUCAO_DE_OPS
    {
        public V_SALDO_PRODUCAO_DE_OPS()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RRADO_NORMAL")] [Required(ErrorMessage = "Campo ENCERRADO_NORMAL requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo ENCERRADO_NORMAL")] public string ENCERRADO_NORMAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DUO_ELIMINADO")] [Required(ErrorMessage = "Campo RESIDUO_ELIMINADO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo RESIDUO_ELIMINADO")] public string RESIDUO_ELIMINADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] [Required(ErrorMessage = "Campo FASE requirido.")] [MaxLength(15, ErrorMessage = "Maximode 15 caracteres, campo FASE")] public string FASE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ROT_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANFORMACAO requirido.")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INICIO_PREVISTA")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_PREVISTA")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PREVISAO_MATERIA_PRIMA")] public DateTime FPR_PREVISAO_MATERIA_PRIMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OP_INTEGRACAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_OP_INTEGRACAO")] public string ORD_OP_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_A_PRODUZIR")] public double? SALDO_A_PRODUZIR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_PRODUCAO")] [Required(ErrorMessage = "Campo DATA_PRODUCAO requirido.")] public DateTime DATA_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECAS_BOAS")] public double? QTD_PECAS_BOAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERDAS")] public double? QTD_PERDAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo MAQ_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_DESCRICAO")] public string MAQ_DESCRICAO { get; set; }
        [NotMapped] public string MAQUINAS { get; set; }
        [NotMapped] public string OBS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
