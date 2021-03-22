using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class MaquinaImpressora
    {
        public MaquinaImpressora()
        {

        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD MÁQUINA")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD IMPRESSORA")] [Required(ErrorMessage = "Campo IMP_ID requirido.")] public int IMP_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual Maquina Maquina { get; set; }
        public virtual Impressora Impressora { get; set; }
    }
}
