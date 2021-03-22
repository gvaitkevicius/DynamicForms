using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoTeste
    {
        public TipoTeste()
        {
            TemplateTipoTeste = new HashSet<TemplateTipoTeste>();
            TesteFisico = new HashSet<TesteFisico>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO")] [Required(ErrorMessage = "Campo TT_ID requirido.")] public int TT_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME")] [Required(ErrorMessage = "Campo TT_NOME requirido.")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TT_NOME")] public string TT_NOME { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo TT_DESC requirido.")] [MaxLength(200, ErrorMessage = "Maximode 200 caracteres, campo TT_DESC")] public string TT_DESC { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOL_MAIS")] public double? TT_TOL_MAIS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TOL_MENOS")] public double? TT_TOL_MENOS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NORMA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TT_NORMA")] public string TT_NORMA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "INICIO DO PROCESSO")] [Required(ErrorMessage = "Campo TT_INICIO_PROCESSO requirido.")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo TT_INICIO_PROCESSO")] public string TT_INICIO_PROCESSO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO AVALIAÇÃO")] [Required(ErrorMessage = "Campo TA_ID requirido.")] public int TA_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UNIDADE DE MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "Nº_AMOSTRAS_P_TESTE")] public int? TT_N_AMOSTRAS_P_TESTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAX_DEF_CRITICO")] public int? TT_MAX_DEF_CRITICO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "MAX_DEF_GRAVE")] public int? TT_MAX_DEF_GRAVE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VALOR ESPECIFICAÇÃO")] public double? TT_ESPECIFICACAO { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual TipoAvaliacao TipoAvaliacao { get; set; }
        public ICollection<TemplateTipoTeste> TemplateTipoTeste { get; set; }
        public ICollection<TesteFisico> TesteFisico { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { return true; }
    }
}
