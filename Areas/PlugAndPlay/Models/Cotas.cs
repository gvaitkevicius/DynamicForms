using DynamicForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Cotas
    {
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COT ID")] [Required(ErrorMessage = "Campo COT_ID requirido.")] public int COT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE")] public DateTime COT_DATA_DE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA ATE")] public DateTime COT_DATA_ATE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] public decimal COT_VALOR { get; set; }
        [HIDDEN][TAB(Value = "PRINCIPAL")] [Display(Name = "OCUPADO")] public decimal COT_OCUPADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "REPRESENTANTE ID")] [Required(ErrorMessage = "Campo REP_ID requirido.")] public int REP_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public virtual Representantes Representantes { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
