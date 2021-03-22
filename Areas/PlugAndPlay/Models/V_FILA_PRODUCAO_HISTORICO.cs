using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_FILA_PRODUCAO_HISTORICO
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_ID")] [Required(ErrorMessage = "Campo ROT_MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PRODUZIDA")] public double? FPR_QTD_PRODUZIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PERDA")] public double? QTD_PERDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ROT_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_PRO_ID")] public string ROT_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INICIO_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_INICIO_PREVISTA requirido.")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_PREVISTA requirido.")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_MAXIMA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_MAXIMA requirido.")] public DateTime FPR_DATA_FIM_MAXIMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] [Required(ErrorMessage = "Campo ROT_SEQ_TRANFORMACAO requirido.")] public int ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] [Required(ErrorMessage = "Campo FPR_SEQ_REPETICAO requirido.")] public int FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "OBS_PRODUCAO")] [MaxLength(4000, ErrorMessage = "Maximode * caracteres, campo FPR_OBS_PRODUCAO")] public string FPR_OBS_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE_PREVISTA")] [Required(ErrorMessage = "Campo FPR_QUANTIDADE_PREVISTA requirido.")] public double FPR_QUANTIDADE_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TIP_ID requirido.")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo TIP_ID")] public string TIP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo MOV_QUANTIDADE requirido.")] public double MOV_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MOV_ID requirido.")] public int MOV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_HORA_CRIACAO")] public DateTime MOV_DATA_HORA_CRIACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo USE_NOME")] public string USE_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ID")] [MaxLength(10, ErrorMessage = "Maximode 10 caracteres, campo TURN_ID")] public string TURN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public string EQU_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}
