using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoInspecaoVisual
    {
        public TipoInspecaoVisual()
        {
            InspecaoVisual = new HashSet<InspecaoVisual>();
            TemplateTipoInspecaoVisual = new HashSet<TemplateTipoInspecaoVisual>();
        }
        [READ] [TAB(Value = "PRINCIPAL")] [Display(Name = "CÓDIGO")] [Required(ErrorMessage = "Campo TIV_ID requirido.")] public int TIV_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [MaxLength(60, ErrorMessage = "Maximode 60 caracteres, campo TIV_NOME")] public string TIV_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [MaxLength(120, ErrorMessage = "Maximode 120 caracteres, campo TIV_DESCRICAO")] public string TIV_DESCRICAO { get; set; }
        [
            Combobox(Value = "S", Description = "S-SIM"),
            Combobox(Value = "N", Description = "N-NÃO"),
        ]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "É UM FECHAMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TIV_FECHAMENTO")] public string TIV_FECHAMENTO { get; set; }
        [
            Combobox(Value = "S", Description = "S-SIM"),
            Combobox(Value = "N", Description = "N-NÃO"),
        ]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "AMOSTRAS ALEATORIAS")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TIV_AMOSTRA_ALEATORIA")] public string TIV_AMOSTRA_ALEATORIA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "Nº DE AMOSTRAS")] public int? TIV_N_AMOSTRAS { get; set; }
        [
            Combobox(Value = "S", Description = "S-SIM"),
            Combobox(Value = "N", Description = "N-NÃO"),
        ]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "É UMA MEDIDA")] public string TIV_MEDIDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESPECIFICACAO")] public double? TIV_ESPECIFICACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOL_MAIS")] public double? TIV_TOL_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOL_MENOS")] public double? TIV_TOL_MENOS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public ICollection<InspecaoVisual> InspecaoVisual { get; set; }
        public ICollection<TemplateTipoInspecaoVisual> TemplateTipoInspecaoVisual { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
