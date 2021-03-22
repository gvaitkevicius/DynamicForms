using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CorridasOnduladeira
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo COR_ID requirido.")] public int COR_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? BOL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQUENCIA")] public int? COR_SEQUENCIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_REPETICAO")] public int? FPR_SEQ_REPETICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FACAO")] public int? COR_FACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FORMATO_BOBINA")] public int? COR_FORMATO_BOBINA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INICIO_PREVISTO")] public DateTime COR_INICIO_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FIM_PREVISTO")] public DateTime COR_FIM_PREVISTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PACAS")] public int? PRO_QTD_PACAS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PECAS_LARGURA")] public int? COR_PECAS_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SEQ_TRANFORMACAO")] public int? ROT_SEQ_TRANFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO")] public double? ORD_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo ORD_COR_FILA")] public string ORD_COR_FILA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COR_FILA")] [Required(ErrorMessage = "Campo FPR_COR_FILA requirido.")] [MaxLength(7, ErrorMessage = "Maximode 7 caracteres, campo FPR_COR_FILA")] public string FPR_COR_FILA { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public double MinutoIni { get; set; }
        [NotMapped] public double MinutoFim { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
