using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class ViewPedidosFuturosPI
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] public int? ORD_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_DESCRICAO")] [MaxLength(22, ErrorMessage = "Maximode 22 caracteres, campo ORD_TIPO_DESCRICAO")] public string ORD_TIPO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_ESTOQUE")] [Required(ErrorMessage = "Campo SALDO_ESTOQUE requirido.")] public double SALDO_ESTOQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo ORD_QUANTIDADE requirido.")] public double ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo CLI_NOME")] public string CLI_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_DE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_DE requirido.")] public DateTime ORD_DATA_ENTREGA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA_ENTREGA_ATE")] [Required(ErrorMessage = "Campo ORD_DATA_ENTREGA_ATE requirido.")] public DateTime ORD_DATA_ENTREGA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] public double? GRP_TIPO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
