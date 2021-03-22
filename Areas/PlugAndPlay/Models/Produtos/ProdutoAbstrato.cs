using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public abstract class ProdutoAbstrato
    {

        [TAB(Value = "PRINCIPAL", Index = 1.0f)] [Display(Name = "CÓDIGO PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL", Index = 2.0f)] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 3.0f)] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO")] public string PRO_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 4.0f)] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO_ERP")] public string PRO_ID_INTEGRACAO_ERP { get; set; }
        [Combobox(Description = "ATIVO", Value = "A")]
        [Combobox(Description = "OBSOLETO", Value = "O")]
        [Combobox(Description = "BLOQUEADO", Value = "B")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo PRO_STATUS")] public string PRO_STATUS { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

    }
}
