using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class CargaMaquina
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "")] [Required(ErrorMessage = "Campo TIPO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TIPO")] public string TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA")] [Required(ErrorMessage = "Campo MED_DATA requirido.")] public DateTime MED_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GMA_ID")] public string GMA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo DIM_ID")] public string DIM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo MAQ_DESCRICAO")] public string MAQ_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SO")] public double? ATRASO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ATA")] public double? NA_DATA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NTADO")] public double? ADIANTADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "SO")] public double? OCIOSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ONIVEL")] public double? DISPONIVEL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "L")] public double? TOTAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AQUINA_DIA")] public double? M2_MAQUINA_DIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CABADO_DIA")] public double? M2_ACABADO_DIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_MAQUINA_DIA")] public double? PESO_MAQUINA_DIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "_ACABADO_DIA")] public double? PESO_ACABADO_DIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "HIERARQUIA_SEQ_TRANSFORMACAO")] public double? MAQ_HIERARQUIA_SEQ_TRANSFORMACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo MAQ_ID")] public string MAQ_ID { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        //public bool BeforeChanges(List<object> objects, List<LogPlay> Logs, ref int modo_insert) {  } 
    }
}
