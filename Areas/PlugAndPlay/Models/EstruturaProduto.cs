using DynamicForms.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "ESTRUTURA DO PRODUTO")]
    public class EstruturaProduto
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE VALIDADE")] [Required(ErrorMessage = "Campo EST_DATA_VALIDADE requirido.")] public DateTime EST_DATA_VALIDADE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID_PRODUTO requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_PRODUTO")] public string PRO_ID_PRODUTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD COMPONENTE")] [Required(ErrorMessage = "Campo PRO_ID_COMPONENTE requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_COMPONENTE")] public string PRO_ID_COMPONENTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QUANTIDADE")] [Required(ErrorMessage = "Campo EST_QUANT requirido.")] public double EST_QUANT { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DATA DE INCLUSAO")] [Required(ErrorMessage = "Campo EST_DATA_INCLUSAO requirido.")] public DateTime EST_DATA_INCLUSAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BASE DE PRODUCAO")] [Required(ErrorMessage = "Campo EST_BASE_PRODUCAO requirido.")] public double EST_BASE_PRODUCAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DE REQUISIÇÃO")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo EST_TIPO_REQUISICAO")] public string EST_TIPO_REQUISICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD DE EXCECAO")] [MaxLength(3000, ErrorMessage = "Maximode * caracteres, campo EST_CODIGO_DE_EXCECAO")] public string EST_CODIGO_DE_EXCECAO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual Produto Produto { get; set; }
        public virtual Produto ProdutoComponente { get; set; }
        [NotMapped] public Produto ProdutoP { get; set; }
        [NotMapped] public Produto ProdutoComponenteP { get; set; }
    }
}