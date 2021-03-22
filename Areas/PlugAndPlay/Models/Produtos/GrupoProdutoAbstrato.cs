using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public abstract class GrupoProdutoAbstrato
    {
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "ID Grupo Requirido.")] [MaxLength(30, ErrorMessage = "ID GRUPO , Maximo 30 caracteres.")] public string GRP_ID { get; set; }
        [SEARCH] [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Descrição Requirida.")] [MaxLength(100, ErrorMessage = "Descricao, Maximo 100 caracteres.")] public string GRP_DESCRICAO { get; set; }
        [Combobox(Value = "S", Description = "SIM")]
        [Combobox(Value = "N", Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ATIVO")] [MaxLength(1, ErrorMessage = "Ativo Maximo 1 caracter.")] public string GRP_ATIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DT CRIAÇÃO")] [READ] public DateTime GRP_DT_CRIACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "ID INTEGRACAO Max 100 caracteres.")] public string GRP_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "ID INTEGRACAO ERP Max 100 caracteres.")] public string GRP_ID_INTEGRACAO_ERP { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        //public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert) { }

    }

    [Display(Name = "GRUPOS DE WMS")]
    public class GrupoProdutoWMSExpedicao : GrupoProdutoAbstrato
    {
        public GrupoProdutoWMSExpedicao()
        {
            ProdutoWMSExpedicao = new HashSet<ProdutoWMSExpedicao>();
        }

        [Combobox(Value = "9", Description = "9-SERVIÇOS")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }

        public virtual ICollection<ProdutoWMSExpedicao> ProdutoWMSExpedicao { get; set; }
    }

    [Display(Name = "GRUPOS DE PRODUTOS")]
    public class GrupoProdutoOutros : GrupoProdutoAbstrato
    {
        public GrupoProdutoOutros()
        {
            ProdutoTinta = new HashSet<ProdutoTinta>();
            ProdutoCliches = new HashSet<ProdutoCliches>();
            ProdutoCaixa = new HashSet<ProdutoCaixa>();
            CustoEntreOps = new HashSet<CustoEntreOps>();
            ProdutoFacas = new HashSet<ProdutoFaca>();
            ProdutoChapaVenda = new HashSet<ProdutoChapaVenda>();
        }

        [Combobox(Value = "1", Description = "1-BOBINAS")]
        [Combobox(Value = "2", Description = "2-CHAPA PAPELÃO")]
        [Combobox(Value = "4", Description = "4-CAIXAS")]
        [Combobox(Value = "7", Description = "7-TINTAS")]
        [Combobox(Value = "8", Description = "8-CLICHES")]
        [Combobox(Value = "8.1", Description = "8.1-FACAS")]
        [Combobox(Value = "9", Description = "9-SERVIÇOS")]
        [Combobox(Value = "10.1", Description = "10.1-MATERIAIS DE ACABAMENTO")]
        [Combobox(Value = "11", Description = "11-APARAS")]
        [Combobox(Value = "12", Description = "12-EMBA")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ICollection<ProdutoTinta> ProdutoTinta { get; set; }
        public virtual ICollection<ProdutoCliches> ProdutoCliches { get; set; }
        public virtual ICollection<ProdutoCaixa> ProdutoCaixa { get; set; }
        public virtual ICollection<CustoEntreOps> CustoEntreOps { get; set; }
        public virtual ICollection<ProdutoFaca> ProdutoFacas { get; set; }
        public virtual ICollection<ProdutoChapaVenda> ProdutoChapaVenda { get; set; }

        [NotMapped]
        public string V_INPUT_T_PRODUTO_TINTAS { get; set; }
    }
    
    [Display(Name = "GRUPOS DE PALETES")]
    public class GrupoProdutoPalete : GrupoProdutoAbstrato
    {
        public GrupoProdutoPalete()
        {
            ProdutoPalete = new HashSet<ProdutoPalete>();
        }

        [Combobox(Value = "6", Description = "6-PALETES")]
        [Combobox(Value = "6.1", Description = "6.1-TAMPO MADEIRA")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {

            return true;
        }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ICollection<ProdutoPalete> ProdutoPalete { get; set; }
    }
    
    [Display(Name = "GRUPOS DE CONJUNTO")]
    public class GrupoProdutoConjunto : GrupoProdutoAbstrato
    {
        public GrupoProdutoConjunto()
        {
            ProdutoCaixa = new HashSet<ProdutoCaixa>();
            ProdutoConjunto = new HashSet<ProdutoConjunto>();
        }

        [Combobox(Value = "3", Description = "3-CONJUNTO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }

        public virtual ICollection<ProdutoCaixa> ProdutoCaixa { get; set; }
        public virtual ICollection<ProdutoConjunto> ProdutoConjunto { get; set; }

        [NotMapped]
        public string V_INPUT_T_PRODUTO_TINTAS { get; set; }
    }

    [Display(Name = "GRUPOS DE COMPOSIÇÃO")]
    public class GrupoProdutoComposicao : GrupoProdutoAbstrato
    {
        public GrupoProdutoComposicao()
        {
            ProdutoChapaIntermediaria = new HashSet<ProdutoChapaIntermediaria>();
        }

        [TAB(Value = "QUALIDADE")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }
        [Combobox(Description = "2-CHAPA PAPELÃO", Value = "2")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }
        [Combobox(Value = "B", Description = "ONDA B")]
        [Combobox(Value = "C", Description = "ONDA C")]
        [Combobox(Value = "BC", Description = "ONDA BC")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO ONDA")] [MaxLength(10, ErrorMessage = "ONDA, Maximo 10 caracteres.")] public string GRP_PAP_ONDA { get; set; }
        [Combobox(Value = "I", Description = "INTERNA")]
        [Combobox(Value = "E", Description = "EXTERNA")]
        [Combobox(Value = "D", Description = "DUPLA")]
        [Combobox(Value = "N", Description = "NÃO POSSUI")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESINA")] public string GRP_RESINA { get; set; }
        [Combobox(Value = "S", Description = "SIM")]
        [Combobox(Value = "N", Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ENDURECEDOR MIOLO")] public string GRP_ENDURECEDOR_MIOLO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRAM/M² COMP")] public double GRP_PAP_GRAMATURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA/MM COMP")] [Range(1, double.PositiveInfinity, ErrorMessage = "O valor do campo [ALTURA/MM COMP / GRP_PAP_ALTURA] precisar ser maior que zero.")] public double GRP_PAP_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME COMERCIAL")] [MaxLength(100, ErrorMessage = "Nome Comercial, Maximo 100 caracteres.")] public string GRP_PAP_NOME_COMERCIAL { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL1")] [MaxLength(30, ErrorMessage = "Papel1 Max 30 caracteres.")] public string GRP_PAPEL1 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL2")] [MaxLength(30, ErrorMessage = "Papel2 Max 30 caracteres.")] public string GRP_PAPEL2 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL3")] [MaxLength(30, ErrorMessage = "Papel3 Max 30 caracteres.")] public string GRP_PAPEL3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL4")] [MaxLength(30, ErrorMessage = "Papel4 Max 30 caracteres.")] public string GRP_PAPEL4 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL5")] [MaxLength(30, ErrorMessage = "Papel5 Max 30 caracteres.")] public string GRP_PAPEL5 { get; set; }
        [Combobox(Value = "S", Description = "SIM")]
        [Combobox(Value = "N", Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESINA INTERNA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo GRP_RESINA_INTERNA")] public string GRP_RESINA_INTERNA { get; set; }
        [Combobox(Value = "S", Description = "SIM")]
        [Combobox(Value = "N", Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "RESINA EXTERNA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo GRP_RESINA_EXTERNA")] public string GRP_RESINA_EXTERNA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PERFORMANCE_METRO_LINEAR_POR_SEGUNDO")] public double? GRP_PERFORMANCE_METRO_LINEAR_POR_SEGUNDO { get; set; }
        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {

            foreach (var obj in objects)
            {

                if (obj.GetType().FullName != "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoComposicao")
                    continue;

                GrupoProdutoComposicao grupoProdutoComposicao = (GrupoProdutoComposicao)obj;
                if (grupoProdutoComposicao.PlayAction != "delete")
                {
                    // Valor padrão do Tipo
                    grupoProdutoComposicao.GRP_TIPO = 2;
                }
            }

            return true;
        }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual ICollection<ProdutoChapaIntermediaria> ProdutoChapaIntermediaria { get; set; }

        [NotMapped]
        public string V_INPUT_T_PRODUTO_TINTAS { get; set; }
    }

    public class GrupoProduto
    {
        public GrupoProduto()
        {
            Produtos = new HashSet<Produto>();
            ProdutoPapel = new HashSet<ProdutoPapel>();
            FechamentoTeste = new HashSet<FechamentoTeste>();
        }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "ID GRUPO , Maximo 30 caracteres.")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DESCRIÇÃO")] [Required(ErrorMessage = "Campo GRP_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Descricao, Maximo 100 caracteres.")] public string GRP_DESCRICAO { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "COD TEMPLATE TESTES")] public int? TEM_ID { get; set; }
        [Combobox(Value = "1", Description = "1-BOBINAS")]
        [Combobox(Value = "2", Description = "2-CHAPA PAPELÃO")]
        [Combobox(Value = "3", Description = "3-CONJUNTO")]
        [Combobox(Value = "4", Description = "4-CAIXAS")]
        [Combobox(Value = "6", Description = "6-PALETES")]
        [Combobox(Value = "7", Description = "7-TINTAS")]
        [Combobox(Value = "8", Description = "8-CLICHES")]
        [Combobox(Value = "8.1", Description = "8.1-FACAS")]
        [Combobox(Value = "9", Description = "9-SERVIÇOS")]
        [Combobox(Value = "10.1", Description = "10.1-MATERIAIS DE ACABAMENTO")]
        [Combobox(Value = "11", Description = "11-APARAS")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO DO GRUPO")] public double GRP_TIPO { get; set; }
        [Combobox(Value = "B", Description = "ONDA B")]
        [Combobox(Value = "C", Description = "ONDA C")]
        [Combobox(Value = "BC", Description = "ONDA BC")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TIPO ONDA")] [MaxLength(10, ErrorMessage = "ONDA, Maximo 10 caracteres.")] public string GRP_PAP_ONDA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "GRAM/M² COMP")] public double GRP_PAP_GRAMATURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ALTURA/MM COMP")] public double GRP_PAP_ALTURA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "NOME COMERCIAL")] [MaxLength(100, ErrorMessage = "Nome Comercial, Maximo 100 caracteres.")] public string GRP_PAP_NOME_COMERCIAL { get; set; }
        [Combobox(Value = "S", Description = "SIM")]
        [Combobox(Value = "N", Description = "NÃO")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ATIVO")] [MaxLength(1, ErrorMessage = "Ativo Maximo 1 caracter.")] public string GRP_ATIVO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "DT CRIAÇÃO")] [READ] public DateTime GRP_DT_CRIACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL1")] [MaxLength(30, ErrorMessage = "Papel1 Max 30 caracteres.")] public string GRP_PAPEL1 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL2")] [MaxLength(30, ErrorMessage = "Papel2 Max 30 caracteres.")] public string GRP_PAPEL2 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL3")] [MaxLength(30, ErrorMessage = "Papel3 Max 30 caracteres.")] public string GRP_PAPEL3 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL4")] [MaxLength(30, ErrorMessage = "Papel4 Max 30 caracteres.")] public string GRP_PAPEL4 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "PAPEL5")] [MaxLength(30, ErrorMessage = "Papel5 Max 30 caracteres.")] public string GRP_PAPEL5 { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "ID INTEGRACAO Max 100 caracteres.")] public string GRP_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "ID INTEGRACAO ERP Max 100 caracteres.")] public string GRP_ID_INTEGRACAO_ERP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "TYPE")] public int? GRP_TYPE { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            return true;
        }

        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<ProdutoPapel> ProdutoPapel { get; set; }
        public virtual ICollection<FechamentoTeste> FechamentoTeste { get; set; }

        [NotMapped]
        public string V_INPUT_T_PRODUTO_TINTAS { get; set; }
    }
}