using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class V_PAINEL_GESTOR_RANKING
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "M")] [Required(ErrorMessage = "Campo ORDEM requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo ORDEM")] public string ORDEM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] [Required(ErrorMessage = "Campo DT requirido.")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo DT")] public string DT { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] public double? MED_VALOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo USE_NOME requirido.")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo USE_NOME")] public string USE_NOME { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}
