using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class PeriodicidadeTeste
    {
        public PeriodicidadeTeste()
        {

        }
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "ID")] [Required(ErrorMessage = "Campo PER_ID requirido.")] public int PER_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD")] public string PER_QTD { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UNI")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_TIPO")] public string GRP_TIPO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
