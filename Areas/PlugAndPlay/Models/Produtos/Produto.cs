
using DynamicForms.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class Produto
    {
        public Produto()
        {
            EstruturasProdutoPai = new HashSet<EstruturaProduto>();
            EstruturasProdutoFilho = new HashSet<EstruturaProduto>();
            V_ROTEIROS_POSSIVEIS_DO_PRODUTO = new HashSet<V_ROTEIROS_POSSIVEIS_DO_PRODUTO>();
            Roteiros = new HashSet<Roteiro>();
            TargetsProduto = new HashSet<TargetProduto>();
            Ordens = new HashSet<Order>();

            Feedbacks = new HashSet<Feedback>();
            FilasProducao = new HashSet<FilaProducao>();
            Etiquetas = new HashSet<Etiqueta>();
            MovimentoEstoqueConsumoMateriaPrima = new HashSet<MovimentoEstoqueConsumoMateriaPrima>();
            MovimentoEstoqueProducao = new HashSet<MovimentoEstoqueProducao>();
            MovimentoEstoqueVendas = new HashSet<MovimentoEstoqueVendas>();
            MovimentoEstoqueReservaDeEstoque = new HashSet<MovimentoEstoqueReservaDeEstoque>();
            MovimentoEstoque = new HashSet<MovimentoEstoque>();
            CustoEntreOps = new HashSet<CustoEntreOps>();
            Calendarios = new HashSet<ItensCalendario>();
            ImpressaoEtiquetasOnd = new HashSet<V_IMPRESSAO_ETIQUETAS_OND>();
        }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESTOQUE ATUAL")] public double? PRO_ESTOQUE_ATUAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD UNIDADE MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAMADAS/PALETE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "IDENTIFICAÇÃO")] public int? PRO_TIPO_IDENTIFICACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID TIPO ABNT")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo ABN_ID")] public string ABN_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRUPO PALETIZAÇÃO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_GRUPO_PALETIZACAO")] public string PRO_GRUPO_PALETIZACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PEÇAS/FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO")] public string PRO_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO_ERP")] public string PRO_ID_INTEGRACAO_ERP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO PRODUTO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "Grupo")] public string GRP_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "Tipo")] public double GRP_TIPO { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_INTERNA")] public double? PRO_LARGURA_INTERNA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_INTERNA")] public double? PRO_COMPRIMENTO_INTERNA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA_INTERNA")] public double? PRO_ALTURA_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "AREA DA CAIXA")] public double? PRO_AREA_LIQUIDA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "AREA DA CHAPA")] public double? PRO_AREA_LIQUIDA_CHAPA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "PESO DA CAIXA")] public double? PRO_PESO { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "PESO DA CHAPA")] public double? PRO_PESO_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA (EMBALADA)")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO (EMBALADA)")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA (EMBALADA)")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FRENTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_FRENTE")] public string PRO_FRENTE { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA COMPRIMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_COMPRIMENTO")] public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA LARGURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_LARGURA")] public string PRO_ROTACIONA_LARGURA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ROTACIONA ALTURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_ALTURA")] public string PRO_ROTACIONA_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ESCALACOR")] public string PRO_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_SUBIDA_ESCALA_COR")] public double? PRO_CUSTO_SUBIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CUSTO_DECIDA_ESCALA_COR")] public double? PRO_CUSTO_DECIDA_ESCALA_COR { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO CARREGAMENTO UNI")] public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO DESCARREGAMENTO UNI")] public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERCENTUAL JANELA EMB")] public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TEMPO PRODUÇÃO CONJ")] public double? PRO_TEMPO_PRODUCAO_CONJUNTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NÚMERO PEÇAS")] public double? PRO_PECAS_DA_PECA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO PRODUTO")] public double? PRO_TYPE { get; set; }
        [Combobox(Description = "ATIVO", Value = "A")]
        [Combobox(Description = "OBSOLETO", Value = "O")]
        [Combobox(Description = "BLOQUEADO", Value = "B")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo PRO_STATUS")] public string PRO_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COLOR_HEXA")] [MaxLength(6, ErrorMessage = "Maximode 6 caracteres, campo PRO_COLOR_HEXA")] public string PRO_COLOR_HEXA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID_C")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_C")] public string PRO_ID_C { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID_CHAPA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_CHAPA")] public string PRO_ID_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_CHAPA")] public double? QTD_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "BASE_PRODUCAO_CHAPA")] public double? BASE_PRODUCAO_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_LARGURA_PECA_CHAPA")] public double? PRO_LARGURA_PECA_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_COMPRIMENTO_PECA_CHAPA")] public double? PRO_COMPRIMENTO_PECA_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_DESCRICAO_CHAPA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO_CHAPA")] public string PRO_DESCRICAO_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA_CHAPA")] public double ALTURA_CHAPA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VINCOS_LARGURA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_LARGURA")] public string PRO_VINCOS_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VINCOS_COMPRIMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_COMPRIMENTO")] public string PRO_VINCOS_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "VINCOS_ONDULADEIRA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo VINCOS_ONDULADEIRA")] public string PRO_VINCOS_ONDULADEIRA { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_CL")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_CL")] public string PRO_ID_CL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_CLICHE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_CLICHE")] public string PRO_ID_CLICHE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_F")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_F")] public string PRO_ID_F { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_FACA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_FACA")] public string PRO_ID_FACA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD_DESENHO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_COD_DESENHO")] public string PRO_COD_DESENHO { get; set; }
        [Combobox(Description = "AUTOMÁTICO", Value = "1")]
        [Combobox(Description = "GRAMPO", Value = "2")]
        [Combobox(Description = "COLA", Value = "3")]
        [Combobox(Description = "S/ FECHAMENTO", Value = "4")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FECHAMENTO")] public string PRO_FECHAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO_LAP")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_TIPO_LAP")] public string PRO_TIPO_LAP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TAMANHO_LAP")] public double? PRO_TAMANHO_LAP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LAP_PROLONGADO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_LAP_PROLONGADO")] public string PRO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TAMANHO_LAP_PROLONGADO")] public double? PRO_TAMANHO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARRANJO_LARGURA")] public int? PRO_ARRANJO_LARGURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ARRANJO_COMPRIMENTO")] public int? PRO_ARRANJO_COMPRIMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_LARG")] public int? PRO_FITILHOS_FARDO_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_COMP")] public int? PRO_FITILHOS_FARDO_COMP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_LARG")] public int? PRO_FITILHOS_PALETE_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_COMP")] public int? PRO_FITILHOS_PALETE_COMP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID_P")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_P")] public string PRO_ID_P { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PRO_ID_PALETE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_PALETE")] public string PRO_ID_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_PALETE")] public double? QTD_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FILME_PALETE")] public int? PRO_FILME_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CANTONEIRA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_CANTONEIRA")] public string PRO_CANTONEIRA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_ESPELHO")] public int? PRO_QTD_ESPELHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_TP")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_TP")] public string PRO_ID_TP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TP_PALETE")] public double? QTD_TP_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID_TP_PALETE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID_TP_PALETE")] public string PRO_ID_TP_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO_PALETE")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO_PALETE")] public string PRO_DESCRICAO_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRICAO_TP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO_TP")] public string PRO_DESCRICAO_TP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_PALETE")] public double? PRO_LARGURA_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_PALETE")] public double? PRO_COMPRIMENTO_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "LARGURA_TP_PALETE")] public double? PRO_LARGURA_TP_PALETE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COMPRIMENTO_TP_PALETE")] public double? PRO_COMPRIMENTO_TP_PALETE { get; set; }

        [TAB(Value = "PRINCIPAL")] [Display(Name = "Composicao")] public string GRP_DESCRICAO_C { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID Composicao")] public string GRP_ID_C { get; set; }



        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual TipoABNT TipoABNT { get; set; }
        public virtual GrupoProduto GrupoProduto { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ICollection<EstruturaProduto> EstruturasProdutoPai { get; set; }
        public virtual ICollection<EstruturaProduto> EstruturasProdutoFilho { get; set; }
        public virtual ICollection<Roteiro> Roteiros { get; set; }
        public virtual ICollection<TargetProduto> TargetsProduto { get; set; }
        public virtual ICollection<Order> Ordens { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<FilaProducao> FilasProducao { get; set; }
        public virtual ICollection<Etiqueta> Etiquetas { get; set; }
        public virtual ICollection<MovimentoEstoqueConsumoMateriaPrima> MovimentoEstoqueConsumoMateriaPrima { get; set; }
        public virtual ICollection<MovimentoEstoqueProducao> MovimentoEstoqueProducao { get; set; }
        public virtual ICollection<MovimentoEstoqueVendas> MovimentoEstoqueVendas { get; set; }
        public virtual ICollection<MovimentoEstoqueReservaDeEstoque> MovimentoEstoqueReservaDeEstoque { get; set; }
        public virtual ICollection<MovimentoEstoque> MovimentoEstoque { get; set; }

        public virtual ICollection<CustoEntreOps> CustoEntreOps { get; set; }
        public virtual ICollection<Observacoes> Observacoes { get; set; }
        public virtual ICollection<ItensCalendario> Calendarios { get; set; }
        public virtual ICollection<V_IMPRESSAO_ETIQUETAS_OND> ImpressaoEtiquetasOnd { get; set; }
        public virtual ICollection<V_ROTEIROS_POSSIVEIS_DO_PRODUTO> V_ROTEIROS_POSSIVEIS_DO_PRODUTO { get; set; }
    }
}