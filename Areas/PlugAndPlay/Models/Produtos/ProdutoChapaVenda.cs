using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class ProdutoChapaVenda : ProdutoAbstrato
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CAMADAS/PALETE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "GRUPO_PALETIZACAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_GRUPO_PALETIZACAO")] public string PRO_GRUPO_PALETIZACAO { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "PECAS_POR_FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "LARGURA_EMBALADA")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "COMPRIMENTO_EMBALADA")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "ALTURA_EMBALADA")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [Combobox(Description = "Largura", Value = "L")]
        [Combobox(Description = "Comprimento", Value = "C")]
        [TAB(Value = "ACABAMENTO")] [Display(Name = "FRENTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_FRENTE")] public string PRO_FRENTE { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "LARGURA_PECA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "COMPRIMENTO_PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "ALTURA_PECA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "PECAS_DA_PECA")] public double? PRO_PECAS_DA_PECA { get; set; }



        // Não sabemos o que essa propriedade representa - PENDENCIA [TAB(Value = "PRODUCAO")] [Display(Name = "TEMPO_PRODUCAO_CONJUNTO")] public double? PRO_TEMPO_PRODUCAO_CONJUNTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_COMPRIMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_COMPRIMENTO")] public string PRO_ROTACIONA_COMPRIMENTO { get; set; } = "N";
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_LARGURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_LARGURA")] public string PRO_ROTACIONA_LARGURA { get; set; } = "N";
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_ALTURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_ALTURA")] public string PRO_ROTACIONA_ALTURA { get; set; } = "A";
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TIPO_CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO_CARREGAMENTO_UNITARIO")] public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO_DESCARREGAMENTO_UNITARIO")] public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "PERCENTUAL_JANELA_EMBARQUE")] public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) {  } 

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }



        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var item in objects)
                {
                    ProdutoChapaVenda _Produto = (ProdutoChapaVenda)item;
                    //Validaçoes --
                    if (_Produto.PRO_CAMADAS_POR_PALETE == null || _Produto.PRO_CAMADAS_POR_PALETE <= 0)
                    {
                        _Produto.PRO_CAMADAS_POR_PALETE = 1;
                    }
                    if (_Produto.PRO_FARDOS_POR_CAMADA == null || _Produto.PRO_FARDOS_POR_CAMADA <= 0)
                    {
                        _Produto.PRO_FARDOS_POR_CAMADA = 1;
                    }
                    if (_Produto.PRO_PECAS_POR_FARDO == null || _Produto.PRO_PECAS_POR_FARDO <= 0)
                    {
                        _Produto.PRO_PECAS_POR_FARDO = 1;
                    }
                }
            }
            return true;
        }

    }
}
