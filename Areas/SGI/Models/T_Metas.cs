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

    public class T_Metas
    {
        public T_Metas()
        {
            this.T_Medicoes = new HashSet<T_Medicoes>();
            this.T_Informacoes_Complementares = new HashSet<T_Informacoes_Complementares>();
            this.T_PlanoAcao = new HashSet<T_PlanoAcao>();
        }

        [READ][TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MET_ID requirido.")] public int MET_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DTINICIO")] [Required(ErrorMessage = "Campo MET_DTINICIO requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo MET_DTINICIO")] public string MET_DTINICIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DTFIM")] [Required(ErrorMessage = "Campo MET_DTFIM requirido.")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo MET_DTFIM")] public string MET_DTFIM { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALVO")] [Required(ErrorMessage = "Campo MET_ALVO requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo MET_ALVO")] public string MET_ALVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPOALVO")] [Required(ErrorMessage = "Campo MET_TIPOALVO requirido.")] public int MET_TIPOALVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IND ID")] [Required(ErrorMessage = "Campo IND_ID requirido.")] public int IND_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RANGE01")] public double? MET_RANGE01 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RANGE02")] public double? MET_RANGE02 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RANGE03")] public double? MET_RANGE03 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DIM ID")] public int? DIM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FAT ID")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo FAT_ID")] public string FAT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUBDIMENSAO_ID")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo DIM_SUBDIMENSAO_ID")] public string DIM_SUBDIMENSAO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PER ID")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo PER_ID")] public string PER_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMPRESA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo DOM_EMPRESA")] public string DOM_EMPRESA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FILIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo DOM_FILIAL")] public string DOM_FILIAL { get; set; }

        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual T_Indicadores T_Indicadores { get; set; }
        public virtual ICollection<T_Medicoes> T_Medicoes { get; set; }
        public virtual ICollection<T_Informacoes_Complementares> T_Informacoes_Complementares { get; set; }
        public virtual ICollection<T_PlanoAcao> T_PlanoAcao { get; set; }
    }
}
