using DynamicForms.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "PAPEL")]
    public class ProdutoPapel : ProdutoAbstrato
    {
        public ProdutoPapel()
        {

        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESTOQUE ATUAL")] public double? PRO_ESTOQUE_ATUAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD UNIDADE MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEÇAS/FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IDENTIFICAÇÃO")] public int? PRO_TIPO_IDENTIFICACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA")] public double? PRO_ALTURA_PECA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FRENTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_FRENTE")] public string PRO_FRENTE { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA LARGURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_LARGURA")] public string PRO_ROTACIONA_LARGURA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA COMPRIMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_COMPRIMENTO")] public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA ALTURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_ALTURA")] public string PRO_ROTACIONA_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA (EMBALADA)")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO (EMBALADA)")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA (EMBALADA)")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO CARREGAMENTO UNI")] public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO DESCARREGAMENTO UNI")] public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL JANELA EMB")] public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO PRODUTO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESCALACOR")] public string PRO_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_SUBIDA_ESCALA_COR")] public double? PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_DECIDA_ESCALA_COR")] public double? PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
        [HIDDENINTERFACE] public string PRO_GRUPO_PALETIZACAO { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  }

        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual GrupoProduto GrupoProduto { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }
    }
}
