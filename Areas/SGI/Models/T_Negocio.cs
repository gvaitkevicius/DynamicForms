//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DynamicForms.Areas.SGI.Model
{
    using DynamicForms.Models;
    using DynamicForms.Util;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class T_Negocio
    {
        public T_Negocio()
        {
            this.T_Indicadores = new HashSet<T_Indicadores>();
        }

        [READ][TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo NEG_ID requirido.")] public int NEG_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo NEG_DESCRICAO requirido.")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo NEG_DESCRICAO")] public string NEG_DESCRICAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual ICollection<T_Indicadores> T_Indicadores { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }
    }
}
