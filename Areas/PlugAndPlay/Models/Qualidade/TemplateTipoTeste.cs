using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TemplateTipoTeste
    {
        public TemplateTipoTeste() { }

        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TTT_ID requirido.")] public int TTT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TT_ID requirido.")] public int TT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo TEM_ID requirido.")] public int TEM_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }

        public virtual TipoTeste TipoTeste { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
