using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class ViewEstoqueIntermediario
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo ORD_ID requirido.")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo ORD_ID")] public string ORD_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo GRP_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo GRP_DESCRICAO")] public string GRP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [Required(ErrorMessage = "Campo GRP_TIPO requirido.")] public double GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] public double? ORD_QUANTIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MENOS")] public double? ORD_TOLERANCIA_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOLERANCIA_MAIS")] public double? ORD_TOLERANCIA_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O")] public double? SALDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "O_RETIDO")] public double? SALDO_RETIDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PALETES")] public int? QTD_PALETES { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROMISSADO")] public double? COMPROMISSADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ONIVEL")] public double? DISPONIVEL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "A_PRODUCAO")] public double? SOBRA_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "A_EXPEDICAO")] public double? SOBRA_EXPEDICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LUCAO")] public double? DEVOLUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VENDA")] public double? QTD_VENDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DOS_FUTUROS")] public double? PEDIDOS_FUTUROS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
