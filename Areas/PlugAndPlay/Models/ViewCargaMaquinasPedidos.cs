using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ViewCargaMaquinasPedidos
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_OP")] public int? TEMPO_OP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_INICIO_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_INICIO_PREVISTA requirido.")] public DateTime FPR_DATA_INICIO_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_FIM_PREVISTA")] [Required(ErrorMessage = "Campo FPR_DATA_FIM_PREVISTA requirido.")] public DateTime FPR_DATA_FIM_PREVISTA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAQ_ID")] [Required(ErrorMessage = "Campo ROT_MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ROT_MAQ_ID")] public string ROT_MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo FPR_COR_FILA")] public string FPR_COR_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_COR_FILA")] public string ORD_COR_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID")] [Required(ErrorMessage = "Campo ORD_PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_PRO_ID")] public string ORD_PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_DESCRICAO")] [Required(ErrorMessage = "Campo ORD_PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo ORD_PRO_DESCRICAO")] public string ORD_PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo EQU_ID")] public string EQU_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "M2_TOTAL")] public double? ORD_M2_TOTAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO_TOTAL")] public double? ORD_PESO_TOTAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo CLI_NOME requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
