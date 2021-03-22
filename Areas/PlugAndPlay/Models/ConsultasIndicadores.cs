using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ConsultasIndicadores
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CON_ID")] public int? CON_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INT_ID")] public int? IND_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
