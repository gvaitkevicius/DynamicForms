using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "PALETES")]
    public class ProdutoPalete : ProdutoAbstrato
    {
        public ProdutoPalete()
        {

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UNIDADE MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO PRODUTO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPLATE DE TESTES")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_PECA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA_PECA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESTOQUE ATUAL")] public double? PRO_ESTOQUE_ATUAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PESO")] public double? PRO_PESO { get; set; }
        [HIDDENINTERFACE] public string PRO_GRUPO_PALETIZACAO { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual GrupoProdutoPalete GrupoProdutoPalete { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }
    }
}
