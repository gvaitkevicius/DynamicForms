using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Relatorios
    {
        [TAB(Value = "PRINCIPAL")] [READ] [Display(Name = "CODIGO")] public int REL_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME_RELATORIO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo REL_NOME_RELATORIO")] public string REL_NOME_RELATORIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME_CAMPO")] [Required(ErrorMessage = "Campo REL_NOME_CAMPO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo REL_NOME_CAMPO")] public string REL_NOME_CAMPO { get; set; }
        [Combobox(Value = "LABEL", Description = "LABEL")]
        [Combobox(Value = "FIELD", Description = "FIELD")]
        [Combobox(Value = "QR_CODE", Description = "QR CODE")]
        [Combobox(Value = "BAR_CODE", Description = "BAR CODE")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_CAMPO")] [Required(ErrorMessage = "Campo REL_TIPO_CAMPO requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo REL_TIPO_CAMPO")] public string REL_TIPO_CAMPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "POS_X")] public int? REL_POS_X { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "POS_Y")] public int? REL_POS_Y { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TAMANHO_FONTE")] public int? REL_TAMANHO_FONTE { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
