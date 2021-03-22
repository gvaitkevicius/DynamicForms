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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class T_Departamentos
    {

        public T_Departamentos()
        {
            this.T_Indicadores_Departamentos = new HashSet<T_Indicadores_Departamentos>();
        }

        [READ][TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo DEP_ID requirido.")] public int DEP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo DEP_NOME requirido.")] [MaxLength(80, ErrorMessage = "Maximode 80 caracteres, campo DEP_NOME")] public string DEP_NOME { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual ICollection<T_Indicadores_Departamentos> T_Indicadores_Departamentos { get; set; }
    }
}