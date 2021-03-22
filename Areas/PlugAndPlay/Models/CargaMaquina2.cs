using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CargaMaquina2
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA")] [Required(ErrorMessage = "Campo MED_DATA requirido.")] public DateTime MED_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo GMA_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_ID")] public string DIM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo MAQ_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_DESCRICAO")] public string MAQ_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SO")] public double? ATRASO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ATA")] public double? NA_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NTADO")] public double? ADIANTADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SO")] public double? OCIOSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "L")] public double? TOTAL { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
