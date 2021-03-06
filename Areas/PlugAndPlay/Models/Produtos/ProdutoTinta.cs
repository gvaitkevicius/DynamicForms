using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "TINTAS")]
    public class ProdutoTinta : ProdutoAbstrato
    {
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 11.0f)] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 12.0f)] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 13.0f)] [Display(Name = "ESCALA DE COR")] public string PRO_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 13.0f)] [Display(Name = "SUB ESCALA DE COR")] public string PRO_SUB_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 14.0f)] [HIDDEN] [Display(Name = "CUSTO_SUBIDA_ESCALA_COR")] public double? PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 15.0f)] [HIDDEN] [Display(Name = "CUSTO_DECIDA_ESCALA_COR")] public double? PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COLOR_HEXA")] [MaxLength(6, ErrorMessage = "Maximode 6 caracteres, campo PRO_COLOR_HEXA")] public string PRO_COLOR_HEXA { get; set; }
        [HIDDENINTERFACE] public string PRO_GRUPO_PALETIZACAO { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }
    }

}
