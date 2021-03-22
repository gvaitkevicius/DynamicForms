using DynamicForms.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CLICHÊS")]
    public class ProdutoCliches : ProdutoAbstrato
    {
        public ProdutoCliches()
        {
            Indisponibilidade = new HashSet<V_DISPONIBILIDADE_CLICHE>();
        }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 11.0f)] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 12.0f)] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [HIDDENINTERFACE] public string PRO_GRUPO_PALETIZACAO { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 
        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }
        public virtual ICollection<V_DISPONIBILIDADE_CLICHE> Indisponibilidade { get; set; }

    }
}
