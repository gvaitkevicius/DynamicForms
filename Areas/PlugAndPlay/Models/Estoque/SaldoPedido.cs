using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class SaldoPedido
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CLI_ID")] [Required(ErrorMessage = "Campo CLI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo CLI_ID")] public string CLI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_DATA_ENTREGA_DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ORD_DATA_ENTREGA_ATE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_ATE requirido.")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SALDO")] public double? SALDO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
