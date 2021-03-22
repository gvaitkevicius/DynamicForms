using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TemplateTipoInspecaoVisual
    {
        public TemplateTipoInspecaoVisual()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO")] [Required(ErrorMessage = "Campo TTI_ID requirido.")] public int TTI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO INSPEÇAO VISUAL")] public int? TIV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPLATE DE TESTES")] public int? TEM_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }

        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual TipoInspecaoVisual TipoInspecaoVisual { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
