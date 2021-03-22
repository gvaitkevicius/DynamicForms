using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "IMPRESSORAS")]
    public class Impressora
    {
        public Impressora()
        {
            MaquinaImpressora = new HashSet<MaquinaImpressora>();
        }

        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "COD IMPRESSORA")] [Required(ErrorMessage = "Campo IMP_ID requirido.")] public int IMP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IP")] [MaxLength(20, ErrorMessage = "Maximode 20 caracteres, campo IMP_IP")] public string IMP_IP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IMPRESSORA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo IMP_NOME")] public string IMP_NOME { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual ICollection<MaquinaImpressora> MaquinaImpressora { get; set; }
    }
}
