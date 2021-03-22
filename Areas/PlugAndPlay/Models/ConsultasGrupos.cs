using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ConsultasGrupos
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CON_ID")] public int? CON_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRU_ID")] public int? GRU_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs) {  } 
    }
}
