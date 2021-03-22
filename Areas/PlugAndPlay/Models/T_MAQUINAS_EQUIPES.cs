
using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class T_MAQUINAS_EQUIPES
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. MAQUINA")] [Required(ErrorMessage = "Campo MAQ_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. EQUIPE")] [Required(ErrorMessage = "Campo EQU_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo EQU_ID")] public string EQU_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD. CALENDARIO")] public int? CAL_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, int modo_insert) {  } 

        public Maquina Maquina { get; set; }
        public Equipe Equipe { get; set; }
    }
}
