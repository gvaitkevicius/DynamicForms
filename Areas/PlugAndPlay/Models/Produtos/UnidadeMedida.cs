using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "UNIDADES DE MEDIDA")]
    public class UnidadeMedida
    {
        public UnidadeMedida()
        {
            Produtos = new HashSet<Produto>();
            ProdutoPapel = new HashSet<ProdutoPapel>();
            TargetsProduto = new HashSet<TargetProduto>();
            ProdutoTinta = new HashSet<ProdutoTinta>();
            ProdutoCaixa = new HashSet<ProdutoCaixa>();
            ProdutoPalete = new HashSet<ProdutoPalete>();
            ProdutoChapaIntermediaria = new HashSet<ProdutoChapaIntermediaria>();
            ProdutoCliches = new HashSet<ProdutoCliches>();
            ProdutoConjunto = new HashSet<ProdutoConjunto>();
            ProdutoFacas = new HashSet<ProdutoFaca>();
            ProdutoChapaVenda = new HashSet<ProdutoChapaVenda>();
            ProdutoWMSExpedicao = new HashSet<ProdutoWMSExpedicao>();
            TipoTeste = new HashSet<TipoTeste>();

        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD UNI MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo UNI_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo UNI_DESCRICAO")] public string UNI_DESCRICAO { get; set; }
        [Combobox(Description = "Hora", Value = "H")]
        [Combobox(Description = "Minuto", Value = "M")]
        [Combobox(Description = "Segundo", Value = "S")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESCALA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo UNI_ESCALA_TEMPO")] public string UNI_ESCALA_TEMPO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<ProdutoPapel> ProdutoPapel { get; set; }
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        public virtual ICollection<ProdutoTinta> ProdutoTinta { get; set; }
        public virtual ICollection<ProdutoCaixa> ProdutoCaixa { get; set; }
        public virtual ICollection<ProdutoCliches> ProdutoCliches { get; set; }
        public virtual ICollection<ProdutoPalete> ProdutoPalete { get; set; }
        public virtual ICollection<ProdutoChapaIntermediaria> ProdutoChapaIntermediaria { get; set; }
        public virtual ICollection<ProdutoConjunto> ProdutoConjunto { get; set; }
        public virtual ICollection<ProdutoFaca> ProdutoFacas { get; set; }
        public virtual ICollection<ProdutoChapaVenda> ProdutoChapaVenda { get; set; }
        public virtual ICollection<ProdutoWMSExpedicao> ProdutoWMSExpedicao { get; set; }
        public virtual ICollection<TipoTeste> TipoTeste { get; set; }
    }
}