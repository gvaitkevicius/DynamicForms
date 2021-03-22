using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models.Estoque
{
    public class ViewEstoqueMP
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_COMPONENTE")] [Required(ErrorMessage = "Campo PRO_ID_COMPONENTE requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_COMPONENTE")] public string PRO_ID_COMPONENTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_DO_PERIODO")] public double? CONSUMO_DO_PERIODO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_HOJE")] public double? CONSUMO_HOJE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_AMANHA")] public double? CONSUMO_AMANHA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_CINCO")] public double? CONSUMO_CINCO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_DEZ")] public double? CONSUMO_DEZ { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_QUINZE")] public double? CONSUMO_QUINZE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UMO_PREVISTO")] public double? CONSUMO_PREVISTO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
