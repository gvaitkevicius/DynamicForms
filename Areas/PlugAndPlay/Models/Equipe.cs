using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "EQUIPES")]

    public class Equipe
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo EQU_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximo de 30 caracteres, campo EQU_ID")] public string EQU_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EQU_HIERARQUIA_SEQ_TRANSFORMACAO")] public double? EQU_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual ICollection<T_MAQUINAS_EQUIPES> MaquinasEquipes { get; set; }

        public Equipe()
        {
            MaquinasEquipes = new HashSet<T_MAQUINAS_EQUIPES>();
        }
    }
}
