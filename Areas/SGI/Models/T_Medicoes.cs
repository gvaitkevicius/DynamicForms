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
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class T_Medicoes
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [Required(ErrorMessage = "Campo MED_ID requirido.")] public int MED_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? IND_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? MET_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] public int? UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA")] [Required(ErrorMessage = "Campo MED_DATA requirido.")] public DateTime MED_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo MED_VALOR")] public string MED_VALOR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AC_ANO")] [MaxLength(70, ErrorMessage = "Maximode 70 caracteres, campo MED_AC_ANO")] public string MED_AC_ANO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATAMEDICAO")] [MaxLength(8, ErrorMessage = "Maximode 8 caracteres, campo MED_DATAMEDICAO")] public string MED_DATAMEDICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PONDERACAO")] public decimal MED_PONDERACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_ID")] public string DIM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_DESCRICAO")] public string DIM_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUBDIMENSAO_ID")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_SUBDIMENSAO_ID")] public string DIM_SUBDIMENSAO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SUB_DESCRICAO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_SUB_DESCRICAO")] public string DIM_SUB_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(3, ErrorMessage = "Maximode 3 caracteres, campo PER_ID")] public string PER_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PER_DESCRICAO")] public string PER_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo FAT_ID")] public string FAT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo FAT_DESCRICAO")] public string FAT_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SQL")] [MaxLength(8000, ErrorMessage = "Maximode * caracteres, campo MED_SQL")] public string MED_SQL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "EMPRESA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo DOM_EMPRESA")] public string DOM_EMPRESA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FILIAL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo DOM_FILIAL")] public string DOM_FILIAL { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}