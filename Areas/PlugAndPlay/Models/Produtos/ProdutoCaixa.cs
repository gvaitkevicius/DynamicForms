using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CAIXAS E ACESSÓRIOS")]
    public class ProdutoCaixa : ProdutoAbstrato
    {
        public ProdutoCaixa()
        {
            ProdutoCaixas = new HashSet<ProdutoCaixa>();
            ProdutoChapaIntermediaria = new HashSet<ProdutoChapaIntermediaria>();
            ProdutoChapaVenda = new HashSet<ProdutoChapaVenda>();
            ProdutoCliches = new HashSet<ProdutoCliches>();
            ProdutoConjunto = new HashSet<ProdutoConjunto>();
            ProdutoFaca = new HashSet<ProdutoFaca>();
            ProdutoPalete = new HashSet<ProdutoPalete>();
            ProdutoPapel = new HashSet<ProdutoPapel>();
            ProdutoTinta = new HashSet<ProdutoTinta>();
        }
        #region Properties
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "CAMADAS/UE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "PEÇAS/FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "LARGURA (EMBALADA)")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "COMPRIMENTO (EMBALADA)")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "ALTURA (EMBALADA)")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "GRUPO_PALETIZACAO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_GRUPO_PALETIZACAO")] public string PRO_GRUPO_PALETIZACAO { get; set; }
        [Combobox(Description = "Largura", Value = "L")]
        [Combobox(Description = "Comprimento", Value = "C")]
        [TAB(Value = "ACABAMENTO")] [Display(Name = "FRENTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_FRENTE")] public string PRO_FRENTE { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID TIPO ABNT")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo ABN_ID")] public string ABN_ID { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD_DESENHO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_COD_DESENHO")] public string PRO_COD_DESENHO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_LARG")] public int? PRO_FITILHOS_FARDO_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_COMP")] public int? PRO_FITILHOS_FARDO_COMP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_LARG")] public int? PRO_FITILHOS_PALETE_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_COMP")] public int? PRO_FITILHOS_PALETE_COMP { get; set; }
        [Combobox(Description = "AUTOMÁTICO", Value = "1")]
        [Combobox(Description = "GRAMPO", Value = "2")]
        [Combobox(Description = "COLA", Value = "3")]
        [Combobox(Description = "S/ FECHAMENTO", Value = "4")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FECHAMENTO")] public string PRO_FECHAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "LARGURA PEÇA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "COMPRIMENTO PEÇA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "ALTURA PEÇA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "LARGURA INTERNA")] public double? PRO_LARGURA_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "COMPRIMENTO INTERNA")] public double? PRO_COMPRIMENTO_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "ALTURA INTERNA")] public double? PRO_ALTURA_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "AREA DA CAIXA")] public double? PRO_AREA_LIQUIDA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "PESO LIQUIDO CAIXA")] public double? PRO_PESO { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "NÚMERO DE PEÇAS")] public double? PRO_PECAS_DA_PECA { get; set; }
        [Combobox(Description = "INTERNO", Value = "1")]
        [Combobox(Description = "EXTERNO", Value = "2")]
        [Combobox(Description = "S/ LAP", Value = "3")]
        [TAB(Value = "MEDIDAS")] [Display(Name = "TIPO_LAP")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_TIPO_LAP")] public string PRO_TIPO_LAP { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "TAMANHO_LAP")] public double? PRO_TAMANHO_LAP { get; set; }
        [Combobox(Description = "LARGURA", Value = "1")]
        [Combobox(Description = "COMPRIMENTO", Value = "2")]
        [Combobox(Description = "S/ LAP PROLONGADO", Value = "3")]
        [TAB(Value = "MEDIDAS")] [Display(Name = "LAP_PROLONGADO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_LAP_PROLONGADO")] public string PRO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "TAMANHO_LAP_PROLONGADO")] public double? PRO_TAMANHO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "VINCOS LARGURA")] public string PRO_VINCOS_LARGURA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "VINCOS COMPRIMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_COMPRIMENTO")] public string PRO_VINCOS_COMPRIMENTO { get; set; }

        [TAB(Value = "MEDIDAS")] [Display(Name = "VINCOS ONDULADEIRA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_COMPRIMENTO")] public string PRO_VINCOS_ONDULADEIRA { get; set; }

        [TAB(Value = "COMPOSICAO")] [Display(Name = "ARRANJO_LARGURA")] public int? PRO_ARRANJO_LARGURA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "ARRANJO_COMPRIMENTO")] public int? PRO_ARRANJO_COMPRIMENTO { get; set; }
        // Não sabemos o que essa propriedade representa - PENDENCIA [TAB(Value = "PRODUCAO")] [Display(Name = "TEMPO_PRODUCAO_CONJUNTO")] public double? PRO_TEMPO_PRODUCAO_CONJUNTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "VOLTAS FILME PALETE")] public int? PRO_FILME_PALETE { get; set; }
        [Combobox(Description = "SIM", Value = "1")]
        [Combobox(Description = "NÃO", Value = "2")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CANTONEIRA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_CANTONEIRA")] public string PRO_CANTONEIRA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_ESPELHO")] public int? PRO_QTD_ESPELHO { get; set; }

        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_COMPRIMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_COMPRIMENTO")] public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_LARGURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_LARGURA")] public string PRO_ROTACIONA_LARGURA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_ALTURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_ALTURA")] public string PRO_ROTACIONA_ALTURA { get; set; }
        [Combobox(Description = "FARDO", Value = "FARDO")]
        [Combobox(Description = "GRANEL", Value = "GRANEL")]
        [Combobox(Description = "PALETIZADO", Value = "PALETIZADO")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TIPO_CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO_CARREGAMENTO_UNITARIO")] public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO_DESCARREGAMENTO_UNITARIO")] public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "PERCENTUAL_JANELA_EMBARQUE")] public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        [Combobox(Description = "LARGURA", Value = "1")]
        [Combobox(Description = "COMPRIMENTO", Value = "2")]
        [Combobox(Description = "AMBOS", Value = "3")]
        [Combobox(Description = "S/ VINCOS", Value = "4")]
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual TipoABNT TipoABNT { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual GrupoProdutoConjunto GrupoProdutoConjunto { get; set; }
        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }
        #endregion Properties

        public virtual ICollection<ProdutoCaixa> ProdutoCaixas { get; set; }
        public virtual ICollection<ProdutoChapaIntermediaria> ProdutoChapaIntermediaria { get; set; }
        public virtual ICollection<ProdutoChapaVenda> ProdutoChapaVenda { get; set; }
        public virtual ICollection<ProdutoCliches> ProdutoCliches { get; set; }
        public virtual ICollection<ProdutoConjunto> ProdutoConjunto { get; set; }
        public virtual ICollection<ProdutoFaca> ProdutoFaca { get; set; }
        public virtual ICollection<ProdutoPalete> ProdutoPalete { get; set; }
        public virtual ICollection<ProdutoPapel> ProdutoPapel { get; set; }
        public virtual ICollection<ProdutoTinta> ProdutoTinta { get; set; }

        // Campos que são da interface dessa classe
        [NotMapped] public string GRP_ID_COMPOSICAO { get; set; }
        [NotMapped] public string PRO_ID_INTEGRACAO_ERP_CHAPA { get; set; }
        [NotMapped] public double? PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA { get; set; }
        [NotMapped] public double? PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA { get; set; }
        [NotMapped] public double CAIXAS_POR_CHAPA { get; set; }
        [NotMapped] public double? PRO_AREA_LIQUIDA_CHAPA { get; set; }
        [NotMapped] public double? PRO_PESO_CHAPA { get; set; }
        [NotMapped] public string PRO_ID_CONJUNTO { get; set; }
        [NotMapped] public string PRO_DESCRICAO_CONJUNTO { get; set; }
        [NotMapped] public double CAIXAS_POR_CONJUNTO { get; set; }
        [NotMapped] public string PRO_ID_CLICHE { get; set; }
        [NotMapped] public string PRO_ID_FACA { get; set; }
        [NotMapped] public double PRO_QTD_CANTONEIRAS { get; set; }
        [NotMapped] public double PRO_QTD_FORROS { get; set; }
        [NotMapped] public double PRO_QTD_CHAPEL { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> newList = new List<object>();
            List<object> otherObjects = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (object obj in objects)
                {
                    if (obj.ToString() != "DynamicForms.Areas.PlugAndPlay.Models.ProdutoCaixa")
                    {
                        otherObjects.Add(obj);
                        continue;
                    }

                    ProdutoCaixa produtoCaixa = (ProdutoCaixa)obj;

                    #region validacoes da Caixa

                    if (produtoCaixa.CAIXAS_POR_CHAPA <= 0)
                    {
                        produtoCaixa.PlayMsgErroValidacao = "A quantidade de Caixas por Chapa deve ser maior que 0 (zero)";
                    }

                    #endregion validacoes da Caixa

                    #region validacoes da Chapa

                    if (produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA <= 0)
                    {
                        produtoCaixa.PlayMsgErroValidacao = "A largura da Chapa deve ser maior que 0 (zero)";
                    }
                    if (produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA <= 0)
                    {
                        produtoCaixa.PlayMsgErroValidacao = "O comrpimento da Chapa deve ser maior que 0 (zero)";
                    }

                    #endregion validacoes da Chapa

                    EstruturaProduto estruturaCaixaForro = ProdutoCaixaToEstruturaCaixaForro(produtoCaixa);
                    EstruturaProduto estruturaCaixaCantoneira = ProdutoCaixaToEstruturaCaixaCantoneira(produtoCaixa);
                    EstruturaProduto estruturaCaixaChapel = ProdutoCaixaToEstruturaCaixaChapel(produtoCaixa);

                    ProdutoChapaIntermediaria produtoChapa = ProdutoCaixaToProdutoChapaIntermediaria(produtoCaixa, db);
                    EstruturaProduto estruturaCaixaChapa = ProdutoCaixaToEstruturaCaixaChapa(produtoCaixa);

                    ProdutoConjunto produtoConjunto = ProdutoCaixaToProdutoConjunto(produtoCaixa);
                    EstruturaProduto estruturaConjuntoCaixa = (produtoConjunto != null) ?
                        ProdutoCaixaToEstruturaConjuntoCaixa(produtoCaixa) : null;

                    ProdutoCliches produtoCliches = ProdutoCaixaToProdutoCliches(produtoCaixa);
                    EstruturaProduto estruturaCaixaCliche = (produtoCliches != null) ?
                        ProdutoCaixaToEstruturaCaixaCliche(produtoCaixa) : null;

                    ProdutoFaca produtoFaca = ProdutoCaixaToProdutoFaca(produtoCaixa);
                    EstruturaProduto estruturaCaixaFaca = (produtoFaca != null) ?
                        ProdutoCaixaToEstruturaCaixaFaca(produtoCaixa) : null;

                    newList.Add(produtoCaixa);

                    #region Tratamento das Chapas
                    bool existeChapa = false;
                    foreach (var item in newList)
                    {
                        if (item.ToString().ToUpper() == "DYNAMICFORMS.AREAS.PLUGANDPLAY.MODELS.PRODUTOCHAPAINTERMEDIARIA")
                        {
                            ProdutoChapaIntermediaria c = (ProdutoChapaIntermediaria)item;
                            if (c.PRO_ID == produtoChapa.PRO_ID)
                            {
                                existeChapa = true;
                                break;
                            }
                        }
                    }

                    if (!existeChapa && produtoCaixa.PlayAction.ToUpper() != "DELETE")
                    {
                        ProdutoChapaIntermediaria Db_produtoChapa = db.ProdutoChapaIntermediaria
                        .AsNoTracking().Where(x => x.PRO_ID == produtoChapa.PRO_ID).FirstOrDefault();

                        if (Db_produtoChapa == null)
                        {
                            produtoChapa.PlayAction = "insert";
                        }
                        else if (produtoChapa.PRO_VINCOS_LARGURA != Db_produtoChapa.PRO_VINCOS_LARGURA)
                        {
                            produtoChapa.PlayAction = "update";
                        }
                        else
                        {
                            produtoChapa.PlayAction = "IGNORE";
                        }
                        // A chapa será inserida na lista do UpdateData mesmo que não vá ser persistida
                        // pois ela será necessário no método AfterChangesInTransaction para verificar
                        // se a caixa teve sua chapa alterada.
                        newList.Add(produtoChapa);
                    }
                    #endregion Tratamento das Chapas

                    #region Tratamento do Conjunto 
                    bool existeConjunto = false;
                    if (produtoConjunto != null)
                    {
                        foreach (var item in newList)
                        {
                            if (item.ToString().ToUpper() == "DYNAMICFORMS.AREAS.PLUGANDPLAY.MODELS.PRODUTOCONJUNTO")
                            {
                                ProdutoConjunto c = (ProdutoConjunto)item;
                                if (c.PRO_ID == produtoConjunto.PRO_ID)
                                {
                                    existeConjunto = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!existeConjunto && produtoConjunto != null && produtoCaixa.PlayAction.ToUpper() != "DELETE")
                    {
                        ProdutoConjunto Db_produtoConjunto = db.ProdutoConjunto.AsNoTracking()
                            .Where(pc => pc.PRO_ID == produtoConjunto.PRO_ID).FirstOrDefault();

                        if (Db_produtoConjunto == null)
                        {
                            produtoConjunto.PlayAction = "insert";
                            newList.Add(produtoConjunto);
                        }
                    }
                    #endregion Tratamento do Conjunto

                    #region Tratamento do Cliche
                    bool existeCliche = false;
                    if (produtoCliches != null)
                    {
                        foreach (var item in newList)
                        {
                            if (item.ToString() == typeof(ProdutoCliches).FullName)
                            {
                                ProdutoCliches c = (ProdutoCliches)item;
                                if (c.PRO_ID == produtoCliches.PRO_ID)
                                {
                                    existeCliche = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!existeCliche && produtoCliches != null && produtoCaixa.PlayAction.ToUpper() != "DELETE")
                    {
                        ProdutoCliches Db_produtoCliches = db.ProdutoCliches.AsNoTracking()
                                                                .Where(x => x.PRO_ID == produtoCliches.PRO_ID).FirstOrDefault();
                        if (Db_produtoCliches == null)
                        {
                            produtoCliches.PlayAction = "insert";
                            newList.Add(produtoCliches);
                        }
                    }
                    #endregion Tratamento do Cliche

                    #region Tratamento da Faca
                    bool existeFaca = false;
                    if (produtoFaca != null)
                    {
                        foreach (var item in newList)
                        {
                            if (item.ToString() == typeof(ProdutoFaca).FullName)
                            {
                                ProdutoFaca f = (ProdutoFaca)item;
                                if (f.PRO_ID == produtoFaca.PRO_ID)
                                {
                                    existeFaca = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!existeFaca && produtoFaca != null && produtoCaixa.PlayAction.ToUpper() != "DELETE")
                    {
                        ProdutoFaca Db_produtoFaca = db.ProdutoFaca.AsNoTracking()
                                                            .Where(x => x.PRO_ID == produtoFaca.PRO_ID).FirstOrDefault();
                        if (Db_produtoFaca == null)
                        {
                            produtoFaca.PlayAction = "insert";
                            newList.Add(produtoFaca);
                        }
                    }
                    #endregion Tratamento da Faca

                    if (produtoCaixa.PlayAction.ToUpper() == "INSERT")
                    {
                        ProdutoCaixa Db_produtoCaixa = db.ProdutoCaixa
                            .AsNoTracking().Where(x => x.PRO_ID == produtoCaixa.PRO_ID).FirstOrDefault();

                        if (Db_produtoCaixa != null)
                        {// Este ProdutoCaixa já existe
                            produtoCaixa.PlayMsgErroValidacao = "Este ProdutoCaixa já existe";
                        }

                        #region Estrutura da Caixa

                        if (estruturaCaixaForro != null)
                        {
                            estruturaCaixaForro.PlayAction = "insert";
                            newList.Add(estruturaCaixaForro);
                        }

                        if (estruturaCaixaCantoneira != null)
                        {
                            estruturaCaixaCantoneira.PlayAction = "insert";
                            newList.Add(estruturaCaixaCantoneira);
                        }

                        if (estruturaCaixaChapel != null)
                        {
                            estruturaCaixaChapel.PlayAction = "insert";
                            newList.Add(estruturaCaixaChapel);
                        }

                        estruturaCaixaChapa.PlayAction = "insert";
                        newList.Add(estruturaCaixaChapa);

                        if (estruturaConjuntoCaixa != null)
                        {
                            estruturaConjuntoCaixa.PlayAction = "insert";
                            newList.Add(estruturaConjuntoCaixa);
                        }

                        if (estruturaCaixaCliche != null)
                        {
                            estruturaCaixaCliche.PlayAction = "insert";
                            newList.Add(estruturaCaixaCliche);
                        }

                        if (estruturaCaixaFaca != null)
                        {
                            estruturaCaixaFaca.PlayAction = "insert";
                            newList.Add(estruturaCaixaFaca);
                        }
                        #endregion Estrutura da Caixa

                    }
                    else if (produtoCaixa.PlayAction.ToLower() == "update" || produtoCaixa.PlayAction.ToLower() == "ok")
                    {
                        #region Estrutura do Forro
                        if (estruturaCaixaForro != null)
                        {
                            EstruturaProduto Db_estruturaCaixaForro = db.EstruturaProduto
                            .AsNoTracking()
                            .Where(x => x.PRO_ID_PRODUTO == estruturaCaixaForro.PRO_ID_PRODUTO && x.PRO_ID_COMPONENTE == estruturaCaixaForro.PRO_ID_COMPONENTE)
                            .FirstOrDefault();

                            if (Db_estruturaCaixaForro == null )
                            {// não existe a estrutura na base de dados
                                estruturaCaixaForro.PlayAction = "insert";
                                newList.Add(estruturaCaixaForro);
                            }
                            else if (estruturaCaixaForro.EST_QUANT != Db_estruturaCaixaForro.EST_QUANT)
                            {// Existe a estrutura na base de dados, mas a quantidade está difente
                                estruturaCaixaForro.PlayAction = "update";
                                newList.Add(estruturaCaixaForro);
                            }
                        }
                        #endregion Estrutura do Forro
                        #region Estrutura da Cantoneira
                        if (estruturaCaixaCantoneira != null)
                        {
                            EstruturaProduto Db_estruturaCaixaCantoneira = db.EstruturaProduto
                            .AsNoTracking()
                            .Where(x => x.PRO_ID_PRODUTO == estruturaCaixaCantoneira.PRO_ID_PRODUTO && x.PRO_ID_COMPONENTE == estruturaCaixaCantoneira.PRO_ID_COMPONENTE)
                            .FirstOrDefault();

                            if (Db_estruturaCaixaCantoneira == null)
                            {// não existe a estrutura na base de dados
                                estruturaCaixaCantoneira.PlayAction = "insert";
                                newList.Add(estruturaCaixaCantoneira);
                            }
                            else if (estruturaCaixaCantoneira.EST_QUANT != Db_estruturaCaixaCantoneira.EST_QUANT)
                            {// Existe a estrutura na base de dados, mas a quantidade está difente
                                estruturaCaixaCantoneira.PlayAction = "update";
                                newList.Add(estruturaCaixaCantoneira);
                            }
                        }
                        #endregion Estrutura da Cantoneira
                        #region Estrutura do Chapel
                        if (estruturaCaixaChapel != null)
                        {
                            EstruturaProduto Db_estruturaCaixaChapel = db.EstruturaProduto
                            .AsNoTracking()
                            .Where(x => x.PRO_ID_PRODUTO == estruturaCaixaChapel.PRO_ID_PRODUTO && x.PRO_ID_COMPONENTE == estruturaCaixaChapel.PRO_ID_COMPONENTE)
                            .FirstOrDefault();

                            if (Db_estruturaCaixaChapel == null)
                            {// não existe a estrutura na base de dados
                                estruturaCaixaChapel.PlayAction = "insert";
                                newList.Add(estruturaCaixaChapel);
                            }
                            else if (estruturaCaixaChapel.EST_QUANT != Db_estruturaCaixaChapel.EST_QUANT)
                            {// Existe a estrutura na base de dados, mas a quantidade está difente
                                estruturaCaixaChapel.PlayAction = "update";
                                newList.Add(estruturaCaixaChapel);
                            }
                        }
                        #endregion Estrutura do Chapel

                        #region Estrutura da Chapa
                        EstruturaProduto Db_estruturaCaixaChapa = db.EstruturaProduto
                            .AsNoTracking()
                            .Where(x => x.PRO_ID_PRODUTO == produtoCaixa.PRO_ID && x.PRO_ID_COMPONENTE == produtoChapa.PRO_ID)
                            .FirstOrDefault();

                        if (Db_estruturaCaixaChapa == null)
                        {
                            // verificar se já existe uma estrutura de produto para esta caixa
                            // caso exista, esta estrutura deve ser apagada
                            EstruturaProduto Db_estruturaCaixaChapa2 = GetEstruturaCaixaChapa(produtoCaixa, db);

                            if (Db_estruturaCaixaChapa2 != null)
                            {// Já existe uma estrutura para este ProdutoCaixa
                             // A estrutura é inserida antes da caixa 
                             // para não dar erro na hora de excluir

                                int index = newList.IndexOf(produtoCaixa);
                                Db_estruturaCaixaChapa2.PlayAction = "delete";
                                newList.Insert(index, Db_estruturaCaixaChapa2);
                            }

                            estruturaCaixaChapa.PlayAction = "insert";
                            newList.Add(estruturaCaixaChapa);
                        }
                        else if (Db_estruturaCaixaChapa.EST_QUANT != estruturaCaixaChapa.EST_QUANT)
                        {
                            // A quantidade de produto caixa foi alterada
                            estruturaCaixaChapa.PlayAction = "update";
                            newList.Add(estruturaCaixaChapa);
                        }
                        #endregion Estrutura da Chapa

                        #region Estrutura da Conjunto
                        if (produtoConjunto != null)
                        {
                            EstruturaProduto Db_estruturaConjuntoCaixa = db.EstruturaProduto.AsNoTracking()
                              .Where(x => x.PRO_ID_PRODUTO == produtoConjunto.PRO_ID &&
                                  x.PRO_ID_COMPONENTE == produtoCaixa.PRO_ID)
                              .FirstOrDefault();

                            if (Db_estruturaConjuntoCaixa == null)
                            {
                                estruturaConjuntoCaixa.PlayAction = "insert";
                                newList.Add(estruturaConjuntoCaixa);
                            }
                            else if (Db_estruturaConjuntoCaixa.EST_QUANT != estruturaConjuntoCaixa.EST_QUANT)
                            {// A quantidade de produto conjunto foi alterada
                                estruturaConjuntoCaixa.PlayAction = "update";
                                newList.Add(estruturaConjuntoCaixa);
                            }
                        }
                        #endregion  Estrutura da Conjunto

                        #region Estrutura do Cliche
                        if (produtoCliches != null)
                        {
                            EstruturaProduto Db_estruturaCaixaCliche = db.EstruturaProduto.AsNoTracking()
                                                                        .Where(x => x.PRO_ID_PRODUTO == produtoCaixa.PRO_ID && x.PRO_ID_COMPONENTE == produtoCliches.PRO_ID)
                                                                        .FirstOrDefault();
                            if (Db_estruturaCaixaCliche == null)
                            {
                                // verificar se já existe uma estrutura de produto para esta caixa
                                // caso exista, esta estrutura deve ser apagada
                                EstruturaProduto Db_estruturaCaixaCliche2 = GetEstruturaCaixaCliche(produtoCaixa, db);

                                if (Db_estruturaCaixaCliche2 != null)
                                {// Já existe uma estrutura para este ProdutoCaixa
                                 // A estrutura é inserida antes da caixa 
                                 // para não dar erro na hora de excluir

                                    int index = newList.IndexOf(produtoCaixa);
                                    Db_estruturaCaixaCliche2.PlayAction = "delete";
                                    newList.Insert(index, Db_estruturaCaixaCliche2);
                                }
                                estruturaCaixaCliche.PlayAction = "insert";
                                newList.Add(estruturaCaixaCliche);
                            }
                        }
                        #endregion Estrutura do Cliche

                        #region Estrutura da Faca
                        if (produtoFaca != null)
                        {
                            EstruturaProduto Db_estruturaCaixaFaca = db.EstruturaProduto.AsNoTracking()
                                                                            .Where(x => x.PRO_ID_PRODUTO == produtoCaixa.PRO_ID && x.PRO_ID_COMPONENTE == produtoFaca.PRO_ID)
                                                                            .FirstOrDefault();
                            if (Db_estruturaCaixaFaca == null)
                            {
                                // verificar se já existe uma estrutura de produto para esta caixa
                                // caso exista, esta estrutura deve ser apagada
                                EstruturaProduto Db_estruturaCaixaFaca2 = GetEstruturaCaixaFaca(produtoCaixa, db);

                                if (Db_estruturaCaixaFaca2 != null)
                                {// Já existe uma estrutura para este ProdutoCaixa
                                 // A estrutura é inserida antes da caixa 
                                 // para não dar erro na hora de excluir

                                    int index = newList.IndexOf(produtoCaixa);
                                    Db_estruturaCaixaFaca2.PlayAction = "delete";
                                    newList.Insert(index, Db_estruturaCaixaFaca2);
                                }
                                estruturaCaixaFaca.PlayAction = "insert";
                                newList.Add(estruturaCaixaFaca);
                            }
                        }
                        #endregion Estrutura da Faca
                    }
                    else if (produtoCaixa.PlayAction.ToLower() == "delete")
                    {// A estrutura é inserida antes da caixa 
                     // para não dar erro na hora de excluir

                        int index = newList.IndexOf(produtoCaixa);
                        estruturaCaixaChapa.PlayAction = "delete";
                        newList.Insert(index, estruturaCaixaChapa);

                        if (estruturaConjuntoCaixa != null)
                        {
                            estruturaConjuntoCaixa.PlayAction = "delete";
                            newList.Insert(++index, estruturaConjuntoCaixa);
                        }

                        if (estruturaCaixaCliche != null)
                        {
                            estruturaCaixaCliche.PlayAction = "delete";
                            newList.Insert(++index, estruturaCaixaCliche);
                        }

                        if (estruturaCaixaFaca != null)
                        {
                            estruturaCaixaFaca.PlayAction = "delete";
                            newList.Insert(++index, estruturaCaixaFaca);
                        }
                    }

                    if (produtoCaixa.PlayAction.ToLower() == "insert" || produtoCaixa.PlayAction.ToLower() == "update")
                    {
                        #region Grupo Paletizacao
                        if (!String.IsNullOrEmpty(produtoCaixa.PRO_GRUPO_PALETIZACAO))
                        {
                            string produtoMae = db.EstruturaProduto.AsNoTracking()
                                                    .Where(x => x.PRO_ID_COMPONENTE == produtoCaixa.PRO_ID)
                                                    .Select(x => x.PRO_ID_PRODUTO)
                                                    .FirstOrDefault();
                            if (!String.IsNullOrEmpty(produtoMae))
                            {
                                ProdutoCaixa caixaIrma = (from estruturaProduto in db.EstruturaProduto
                                                          join produtoCx in db.ProdutoCaixa
                                                            on estruturaProduto.PRO_ID_COMPONENTE equals produtoCx.PRO_ID
                                                          where estruturaProduto.PRO_ID_PRODUTO == produtoMae &&
                                                            estruturaProduto.PRO_ID_COMPONENTE != produtoCaixa.PRO_ID &&
                                                            estruturaProduto.PRO_ID_COMPONENTE == produtoCaixa.PRO_GRUPO_PALETIZACAO
                                                          select produtoCx
                                                         ).FirstOrDefault();

                                if (caixaIrma != null && String.IsNullOrEmpty(caixaIrma.PRO_GRUPO_PALETIZACAO))
                                {
                                    caixaIrma.PRO_GRUPO_PALETIZACAO = produtoCaixa.PRO_GRUPO_PALETIZACAO;
                                    caixaIrma.PlayAction = "update";
                                    newList.Add(caixaIrma);
                                }
                                else if (caixaIrma == null)
                                {
                                    produtoCaixa.PlayMsgErroValidacao = $"O Produto '{produtoCaixa.PRO_GRUPO_PALETIZACAO}' utilizado para o Grupo de Paletização não existe";
                                }
                                else if (!String.IsNullOrEmpty(caixaIrma.PRO_GRUPO_PALETIZACAO))
                                {
                                    produtoCaixa.PlayMsgErroValidacao = $"O Produto '{produtoCaixa.PRO_GRUPO_PALETIZACAO}' está sendo paletizado com outro Produto: '{caixaIrma.PRO_GRUPO_PALETIZACAO}'";
                                }
                            }
                        }
                        #endregion Grupo Paletizacao
                    }
                }
            }
            objects.RemoveRange(0, objects.Count);
            objects.AddRange(newList);
            objects.AddRange(otherObjects);
            modo_insert = 3;
            return true;
        }

        private EstruturaProduto ProdutoCaixaToEstruturaCaixaForro(ProdutoCaixa produtoCaixa)
        {
            if (produtoCaixa.PRO_QTD_FORROS <= 0)
                return null;

            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = "EMBAFORRO";
            estruturaProduto.EST_QUANT = PRO_QTD_FORROS;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;

            return estruturaProduto;

        }
        private EstruturaProduto ProdutoCaixaToEstruturaCaixaCantoneira(ProdutoCaixa produtoCaixa)
        {
            if (produtoCaixa.PRO_QTD_CANTONEIRAS <= 0)
                return null;

            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = "EMBACANTONEIRA";
            estruturaProduto.EST_QUANT = PRO_QTD_CANTONEIRAS;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;

            return estruturaProduto;
        }
        private EstruturaProduto ProdutoCaixaToEstruturaCaixaChapel(ProdutoCaixa produtoCaixa)
        {
            if (produtoCaixa.PRO_QTD_CHAPEL <= 0)
                return null;

            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = "EMBACHAPEL";
            estruturaProduto.EST_QUANT = PRO_QTD_CHAPEL;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;

            return estruturaProduto;
        }

        private EstruturaProduto ProdutoCaixaToEstruturaCaixaFaca(ProdutoCaixa produtoCaixa)
        {
            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = produtoCaixa.PRO_ID_FACA;
            estruturaProduto.EST_QUANT = 1;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;
            estruturaProduto.EST_TIPO_REQUISICAO = "";
            estruturaProduto.EST_CODIGO_DE_EXCECAO = "";

            return estruturaProduto;
        }

        private EstruturaProduto ProdutoCaixaToEstruturaCaixaCliche(ProdutoCaixa produtoCaixa)
        {
            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = produtoCaixa.PRO_ID_CLICHE;
            estruturaProduto.EST_QUANT = 1;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;
            estruturaProduto.EST_TIPO_REQUISICAO = "";
            estruturaProduto.EST_CODIGO_DE_EXCECAO = "";

            return estruturaProduto;
        }

        private ProdutoFaca ProdutoCaixaToProdutoFaca(ProdutoCaixa produtoCaixa)
        {
            ProdutoFaca produtoFaca = null;
            if (produtoCaixa.PRO_ID_FACA != null && produtoCaixa.PRO_ID_FACA != "")
            {
                produtoFaca = new ProdutoFaca();
                produtoFaca.PRO_ID = produtoCaixa.PRO_ID_FACA;
                produtoFaca.PRO_DESCRICAO = produtoCaixa.PRO_ID_FACA;
                produtoFaca.UNI_ID = "UN";
                produtoFaca.GRP_ID = "FACA";
                produtoFaca.PRO_STATUS = "A";
            }
            return produtoFaca;
        }

        private ProdutoCliches ProdutoCaixaToProdutoCliches(ProdutoCaixa produtoCaixa)
        {
            ProdutoCliches produtoCliches = null;
            if (produtoCaixa.PRO_ID_CLICHE != null && produtoCaixa.PRO_ID_CLICHE != "")
            {
                produtoCliches = new ProdutoCliches();
                produtoCliches.PRO_ID = produtoCaixa.PRO_ID_CLICHE;
                produtoCliches.PRO_DESCRICAO = produtoCaixa.PRO_ID_CLICHE;
                produtoCliches.UNI_ID = "UN";
                produtoCliches.GRP_ID = "CLIC";
                produtoCliches.PRO_STATUS = "A";
            }

            return produtoCliches;
        }

        private EstruturaProduto GetEstruturaCaixaChapa(ProdutoCaixa produtoCaixa, JSgi db)
        {
            EstruturaProduto Db_EstruturaProduto = db.EstruturaProduto.AsNoTracking()
                                .Select(ep => new EstruturaProduto
                                {
                                    EST_DATA_VALIDADE = ep.EST_DATA_VALIDADE,
                                    PRO_ID_PRODUTO = ep.PRO_ID_PRODUTO,
                                    PRO_ID_COMPONENTE = ep.PRO_ID_COMPONENTE,
                                    EST_QUANT = ep.EST_QUANT,
                                    EST_DATA_INCLUSAO = ep.EST_DATA_INCLUSAO,
                                    EST_BASE_PRODUCAO = ep.EST_BASE_PRODUCAO,
                                    EST_TIPO_REQUISICAO = ep.EST_TIPO_REQUISICAO,
                                    EST_CODIGO_DE_EXCECAO = ep.EST_CODIGO_DE_EXCECAO,
                                    Produto = ep.Produto,
                                    ProdutoComponente = ep.ProdutoComponente
                                })
                                .Include(estruturaProd => estruturaProd.ProdutoComponente)
                                    .ThenInclude(prod => prod.GrupoProduto)
                                .Where(estruturaProd => estruturaProd.PRO_ID_PRODUTO == produtoCaixa.PRO_ID &&
                                    estruturaProd.ProdutoComponente.GrupoProduto.GRP_TIPO == 2)
                                .FirstOrDefault();

            return Db_EstruturaProduto;
        }

        private EstruturaProduto GetEstruturaConjuntoCaixa(ProdutoCaixa produtoCaixa, JSgi db)
        {
            EstruturaProduto Db_EstruturaProduto = db.EstruturaProduto.AsNoTracking()
                                .Select(ep => new EstruturaProduto
                                {
                                    EST_DATA_VALIDADE = ep.EST_DATA_VALIDADE,
                                    PRO_ID_PRODUTO = ep.PRO_ID_PRODUTO,
                                    PRO_ID_COMPONENTE = ep.PRO_ID_COMPONENTE,
                                    EST_QUANT = ep.EST_QUANT,
                                    EST_DATA_INCLUSAO = ep.EST_DATA_INCLUSAO,
                                    EST_BASE_PRODUCAO = ep.EST_BASE_PRODUCAO,
                                    EST_TIPO_REQUISICAO = ep.EST_TIPO_REQUISICAO,
                                    EST_CODIGO_DE_EXCECAO = ep.EST_CODIGO_DE_EXCECAO,
                                    Produto = ep.Produto,
                                    ProdutoComponente = ep.ProdutoComponente
                                })
                                .Include(estruturaProd => estruturaProd.Produto)
                                    .ThenInclude(prod => prod.GrupoProduto)
                                .Where(estruturaProd => estruturaProd.PRO_ID_COMPONENTE == produtoCaixa.PRO_ID &&
                                    estruturaProd.Produto.GrupoProduto.GRP_TIPO == 3)
                                .FirstOrDefault();

            return Db_EstruturaProduto;
        }

        private EstruturaProduto GetEstruturaCaixaCliche(ProdutoCaixa produtoCaixa, JSgi db)
        {
            EstruturaProduto Db_EstruturaProduto = db.EstruturaProduto.AsNoTracking()
                                .Select(ep => new EstruturaProduto
                                {
                                    EST_DATA_VALIDADE = ep.EST_DATA_VALIDADE,
                                    PRO_ID_PRODUTO = ep.PRO_ID_PRODUTO,
                                    PRO_ID_COMPONENTE = ep.PRO_ID_COMPONENTE,
                                    EST_QUANT = ep.EST_QUANT,
                                    EST_DATA_INCLUSAO = ep.EST_DATA_INCLUSAO,
                                    EST_BASE_PRODUCAO = ep.EST_BASE_PRODUCAO,
                                    EST_TIPO_REQUISICAO = ep.EST_TIPO_REQUISICAO,
                                    EST_CODIGO_DE_EXCECAO = ep.EST_CODIGO_DE_EXCECAO,
                                    Produto = ep.Produto,
                                    ProdutoComponente = ep.ProdutoComponente
                                })
                                .Include(estruturaProd => estruturaProd.Produto)
                                    .ThenInclude(prod => prod.GrupoProduto)
                                .Where(estruturaProd => estruturaProd.PRO_ID_PRODUTO == produtoCaixa.PRO_ID &&
                                    estruturaProd.ProdutoComponente.GrupoProduto.GRP_TIPO == 8)
                                .FirstOrDefault();

            return Db_EstruturaProduto;
        }

        private EstruturaProduto GetEstruturaCaixaFaca(ProdutoCaixa produtoCaixa, JSgi db)
        {
            EstruturaProduto Db_EstruturaProduto = db.EstruturaProduto.AsNoTracking()
                                .Select(ep => new EstruturaProduto
                                {
                                    EST_DATA_VALIDADE = ep.EST_DATA_VALIDADE,
                                    PRO_ID_PRODUTO = ep.PRO_ID_PRODUTO,
                                    PRO_ID_COMPONENTE = ep.PRO_ID_COMPONENTE,
                                    EST_QUANT = ep.EST_QUANT,
                                    EST_DATA_INCLUSAO = ep.EST_DATA_INCLUSAO,
                                    EST_BASE_PRODUCAO = ep.EST_BASE_PRODUCAO,
                                    EST_TIPO_REQUISICAO = ep.EST_TIPO_REQUISICAO,
                                    EST_CODIGO_DE_EXCECAO = ep.EST_CODIGO_DE_EXCECAO,
                                    Produto = ep.Produto,
                                    ProdutoComponente = ep.ProdutoComponente
                                })
                                .Include(estruturaProd => estruturaProd.Produto)
                                    .ThenInclude(prod => prod.GrupoProduto)
                                .Where(estruturaProd => estruturaProd.PRO_ID_PRODUTO == produtoCaixa.PRO_ID &&
                                    estruturaProd.ProdutoComponente.GrupoProduto.GRP_TIPO == 8.1)
                                .FirstOrDefault();

            return Db_EstruturaProduto;
        }

        public void CalculatedFields(ProdutoCaixa produtoCaixa)
        {
            if (produtoCaixa.CAIXAS_POR_CHAPA > 0)
                produtoCaixa.CAIXAS_POR_CHAPA = 1 / produtoCaixa.CAIXAS_POR_CHAPA;
        }

        private ProdutoChapaIntermediaria ProdutoCaixaToProdutoChapaIntermediaria(ProdutoCaixa produtoCaixa, JSgi db)
        {
            ProdutoChapaIntermediaria chapaIntermediaria = new ProdutoChapaIntermediaria();

            GrupoProdutoComposicao grupoChapa = db.GrupoProdutoComposicao.AsNoTracking()
                                                    .Where(x => x.GRP_ID == produtoCaixa.GRP_ID_COMPOSICAO)
                                                    .Select(gp => new GrupoProdutoComposicao
                                                    {
                                                        GRP_ID = gp.GRP_ID,
                                                        GRP_DESCRICAO = gp.GRP_DESCRICAO,
                                                        GRP_PAP_ALTURA = gp.GRP_PAP_ALTURA
                                                    }).FirstOrDefault();

            chapaIntermediaria.PRO_ID = this.ObterIdProdutoChapaIntermediaria(produtoCaixa.GRP_ID_COMPOSICAO,
                produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA,
                produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA);
            
            
            string largura = PreencherZeros(chapaIntermediaria.PRO_LARGURA_PECA.ToString(),4);
            string comprimento = PreencherZeros(chapaIntermediaria.PRO_COMPRIMENTO_PECA.ToString(),4);
            chapaIntermediaria.PRO_DESCRICAO = $"{grupoChapa.GRP_DESCRICAO}:{chapaIntermediaria.GRP_ID} larg:{largura} comp:{comprimento}";
            
            chapaIntermediaria.GRP_ID = produtoCaixa.GRP_ID_COMPOSICAO;
            chapaIntermediaria.PRO_ID_INTEGRACAO_ERP = produtoCaixa.PRO_ID_INTEGRACAO_ERP_CHAPA;
            //chapaIntermediaria.PRO_AREA_LIQUIDA = produtoCaixa.PRO_AREA_LIQUIDA_CHAPA;
            chapaIntermediaria.PRO_AREA_LIQUIDA = produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA / 1000.0 * produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA / 1000.0;
            chapaIntermediaria.PRO_PESO = produtoCaixa.PRO_PESO_CHAPA;
            chapaIntermediaria.PRO_LARGURA_PECA = produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA;
            chapaIntermediaria.PRO_COMPRIMENTO_PECA = produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA;
            chapaIntermediaria.PRO_ALTURA_PECA = grupoChapa.GRP_PAP_ALTURA;
            chapaIntermediaria.UNI_ID = "UN";

            return chapaIntermediaria;
        }

        private EstruturaProduto ProdutoCaixaToEstruturaCaixaChapa(ProdutoCaixa produtoCaixa)
        {
            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID;
            estruturaProduto.PRO_ID_COMPONENTE = this.ObterIdProdutoChapaIntermediaria(produtoCaixa.GRP_ID_COMPOSICAO,
                produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA,
                produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA);

            // CAIXAS_POR_CHAPA é um campo calculado
            estruturaProduto.EST_QUANT = 1 / produtoCaixa.CAIXAS_POR_CHAPA;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;
            estruturaProduto.EST_TIPO_REQUISICAO = "";
            estruturaProduto.EST_CODIGO_DE_EXCECAO = "";

            return estruturaProduto;
        }

        private EstruturaProduto ProdutoCaixaToEstruturaConjuntoCaixa(ProdutoCaixa produtoCaixa)
        {
            EstruturaProduto estruturaProduto = new EstruturaProduto();

            estruturaProduto.PRO_ID_PRODUTO = produtoCaixa.PRO_ID_CONJUNTO;
            estruturaProduto.PRO_ID_COMPONENTE = produtoCaixa.PRO_ID;
            estruturaProduto.EST_QUANT = produtoCaixa.CAIXAS_POR_CONJUNTO;
            estruturaProduto.EST_BASE_PRODUCAO = 1;
            estruturaProduto.EST_DATA_INCLUSAO = DateTime.Now;
            estruturaProduto.EST_DATA_VALIDADE = DateTime.MaxValue;
            estruturaProduto.EST_TIPO_REQUISICAO = "";
            estruturaProduto.EST_CODIGO_DE_EXCECAO = "";

            return estruturaProduto;
        }

        private ProdutoConjunto ProdutoCaixaToProdutoConjunto(ProdutoCaixa produtoCaixa)
        {
            if (produtoCaixa.PRO_ID_CONJUNTO != null && produtoCaixa.PRO_ID_CONJUNTO != "")
            {
                ProdutoConjunto produtoConjunto = new ProdutoConjunto();

                produtoConjunto.PRO_ID = produtoCaixa.PRO_ID_CONJUNTO;
                produtoConjunto.PRO_DESCRICAO = produtoCaixa.PRO_DESCRICAO_CONJUNTO;
                produtoConjunto.UNI_ID = "CJ";
                produtoConjunto.GRP_ID = "CJ";
                return produtoConjunto;
            }
            return null;
        }

        private string ObterIdProdutoChapaIntermediaria(string grpIdComposicao, double? largura, double? comprimento)
        {
            string auxLarguraMM = largura.ToString();
            string auxComprimentoMM = comprimento.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(grpIdComposicao);
            sb.Append(PreencherZeros(auxLarguraMM,4));
            sb.Append(PreencherZeros(auxComprimentoMM,4));
            return sb.ToString();
        }

        private string PreencherZeros(string str,int len)
        {// Utilizado para preencher zeros a esquerda da largura e comprimento
            StringBuilder sbZeros;

            if (str.Length < len)
            {// Adiciona zeros a esquerda
                sbZeros = new StringBuilder();
                int qtd = len - str.Length;
                for (int i = 0; i < qtd; i++)
                {
                    sbZeros.Append("0");
                }
                sbZeros.Append(str);
                return sbZeros.ToString();
            }
            return str;
        }

        public bool AfterChangesInTransaction(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            if (db == null)
            {
                db = new ContextFactory().CreateDbContext(new string[] { });
            }

            #region verificação se a chapa da caixa foi alterada
            //List<object> newObjects = new List<object>();
            //foreach (var item in objects)
            //{
            //    if (item.GetType().Name == nameof(ProdutoCaixa))
            //    {
            //        ProdutoCaixa caixa = (ProdutoCaixa)item;
            //        var chapa = (ProdutoChapaIntermediaria)objects.FirstOrDefault(x => x.GetType().Name == nameof(ProdutoChapaIntermediaria));
            //        if (chapa == null && (!chapa.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) ||
            //                !chapa.PlayAction.Equals("ignore", StringComparison.OrdinalIgnoreCase)))
            //            continue;

            //        var estruturas = objects.Where(x => x.GetType().Name == nameof(EstruturaProduto)).Cast<EstruturaProduto>();
            //        var estruturaDaNovaChapa = estruturas.FirstOrDefault(ep => ep.PRO_ID_PRODUTO.Equals(caixa.PRO_ID) && ep.PRO_ID_COMPONENTE.Equals(chapa.PRO_ID));

            //        if (estruturaDaNovaChapa == null)
            //            continue;

            //        if (caixa.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase) &&
            //            estruturaDaNovaChapa.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase))
            //        {// A caixa possui uma nova chapa
            //            Order aux = new Order();
            //            List<Order> pedidosEmAberto = aux.GetPedidosEmAberto(caixa.PRO_ID);

            //            List<object> explosaoOPs = new List<object>();
            //            pedidosEmAberto.ForEach(pedido =>
            //            {
            //                explosaoOPs.AddRange(aux.ExplodirOPs(pedido, ref Logs, db));
            //            });
            //            newObjects.AddRange(explosaoOPs);
            //        }
            //    }
            //}
            ////Aqui é adicionado o prefixo AFTER- para que o método AfterChangesInTransaction do MasterController possa identificar quais são os novos objetos da lista
            //foreach (dynamic item in newObjects)
            //{
            //    item.PlayAction = $"AFTER-{item.PlayAction}";
            //}
            //objects.InsertRange(0, newObjects);
            #endregion verificação se a chapa da caixa foi alterada

            modo_insert = 2;
            return true;
        }

        public void ProdutoCaixaToInterfaceTelaProdutoCaixa(List<object> objects)
        {
            List<object> newList = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var obj in objects)
                {
                    ProdutoCaixa produtoCaixa = (ProdutoCaixa)obj;

                    InterfaceTelaProdutoCaixa interfaceCaixa = new InterfaceTelaProdutoCaixa();

                    interfaceCaixa.PRO_ID = produtoCaixa.PRO_ID;
                    interfaceCaixa.ABN_ID = produtoCaixa.ABN_ID;
                    interfaceCaixa.PRO_DESCRICAO = produtoCaixa.PRO_DESCRICAO;
                    interfaceCaixa.PRO_ID_INTEGRACAO = produtoCaixa.PRO_ID_INTEGRACAO;
                    interfaceCaixa.PRO_ID_INTEGRACAO_ERP = produtoCaixa.PRO_ID_INTEGRACAO_ERP;
                    interfaceCaixa.PRO_STATUS = produtoCaixa.PRO_STATUS;
                    interfaceCaixa.PRO_FARDOS_POR_CAMADA = produtoCaixa.PRO_FARDOS_POR_CAMADA;
                    interfaceCaixa.PRO_CAMADAS_POR_PALETE = produtoCaixa.PRO_CAMADAS_POR_PALETE;
                    interfaceCaixa.PRO_GRUPO_PALETIZACAO = produtoCaixa.PRO_GRUPO_PALETIZACAO;
                    interfaceCaixa.PRO_PECAS_POR_FARDO = produtoCaixa.PRO_PECAS_POR_FARDO;
                    interfaceCaixa.PRO_LARGURA_EMBALADA = produtoCaixa.PRO_LARGURA_EMBALADA;
                    interfaceCaixa.PRO_COMPRIMENTO_EMBALADA = produtoCaixa.PRO_COMPRIMENTO_EMBALADA;
                    interfaceCaixa.PRO_ALTURA_EMBALADA = produtoCaixa.PRO_ALTURA_EMBALADA;
                    interfaceCaixa.PRO_FRENTE = produtoCaixa.PRO_FRENTE;
                    interfaceCaixa.TEM_ID = produtoCaixa.TEM_ID;
                    interfaceCaixa.UNI_ID = produtoCaixa.UNI_ID;
                    interfaceCaixa.GRP_ID = produtoCaixa.GRP_ID;
                    interfaceCaixa.PRO_LARGURA_PECA = produtoCaixa.PRO_LARGURA_PECA;
                    interfaceCaixa.PRO_COMPRIMENTO_PECA = produtoCaixa.PRO_COMPRIMENTO_PECA;
                    interfaceCaixa.PRO_ALTURA_PECA = produtoCaixa.PRO_ALTURA_PECA;
                    interfaceCaixa.PRO_LARGURA_INTERNA = produtoCaixa.PRO_LARGURA_INTERNA;
                    interfaceCaixa.PRO_COMPRIMENTO_INTERNA = produtoCaixa.PRO_COMPRIMENTO_INTERNA;
                    interfaceCaixa.PRO_ALTURA_INTERNA = produtoCaixa.PRO_ALTURA_INTERNA;
                    interfaceCaixa.PRO_PECAS_DA_PECA = produtoCaixa.PRO_PECAS_DA_PECA;
                    interfaceCaixa.PRO_ROTACIONA_COMPRIMENTO = produtoCaixa.PRO_ROTACIONA_COMPRIMENTO;
                    interfaceCaixa.PRO_ROTACIONA_LARGURA = produtoCaixa.PRO_ROTACIONA_LARGURA;
                    interfaceCaixa.PRO_ROTACIONA_ALTURA = produtoCaixa.PRO_ROTACIONA_ALTURA;
                    interfaceCaixa.TMP_TIPO_CARGA = produtoCaixa.TMP_TIPO_CARGA;
                    interfaceCaixa.PRO_TEMPO_CARREGAMENTO_UNITARIO = produtoCaixa.PRO_TEMPO_CARREGAMENTO_UNITARIO;
                    interfaceCaixa.PRO_TEMPO_DESCARREGAMENTO_UNITARIO = produtoCaixa.PRO_TEMPO_DESCARREGAMENTO_UNITARIO;
                    interfaceCaixa.PRO_PERCENTUAL_JANELA_EMBARQUE = produtoCaixa.PRO_PERCENTUAL_JANELA_EMBARQUE;
                    interfaceCaixa.PRO_COD_DESENHO = produtoCaixa.PRO_COD_DESENHO;
                    interfaceCaixa.PRO_TIPO_LAP = produtoCaixa.PRO_TIPO_LAP;
                    interfaceCaixa.PRO_TAMANHO_LAP = produtoCaixa.PRO_TAMANHO_LAP;
                    interfaceCaixa.PRO_LAP_PROLONGADO = produtoCaixa.PRO_LAP_PROLONGADO;
                    interfaceCaixa.PRO_TAMANHO_LAP_PROLONGADO = produtoCaixa.PRO_TAMANHO_LAP_PROLONGADO;
                    interfaceCaixa.PRO_ARRANJO_LARGURA = produtoCaixa.PRO_ARRANJO_LARGURA;
                    interfaceCaixa.PRO_ARRANJO_COMPRIMENTO = produtoCaixa.PRO_ARRANJO_COMPRIMENTO;
                    interfaceCaixa.PRO_FITILHOS_FARDO_LARG = produtoCaixa.PRO_FITILHOS_FARDO_LARG;
                    interfaceCaixa.PRO_FITILHOS_FARDO_COMP = produtoCaixa.PRO_FITILHOS_FARDO_COMP;
                    interfaceCaixa.PRO_FILME_PALETE = produtoCaixa.PRO_FILME_PALETE;
                    interfaceCaixa.PRO_CANTONEIRA = produtoCaixa.PRO_CANTONEIRA;
                    interfaceCaixa.PRO_FECHAMENTO = produtoCaixa.PRO_FECHAMENTO;
                    interfaceCaixa.PRO_VINCOS_LARGURA = produtoCaixa.PRO_VINCOS_LARGURA;
                    interfaceCaixa.PRO_VINCOS_COMPRIMENTO = produtoCaixa.PRO_VINCOS_COMPRIMENTO;
                    interfaceCaixa.PRO_QTD_ESPELHO = produtoCaixa.PRO_QTD_ESPELHO;
                    interfaceCaixa.PRO_FITILHOS_PALETE_LARG = produtoCaixa.PRO_FITILHOS_PALETE_LARG;
                    interfaceCaixa.PRO_FITILHOS_PALETE_COMP = produtoCaixa.PRO_FITILHOS_PALETE_COMP;
                    interfaceCaixa.PRO_AREA_LIQUIDA = produtoCaixa.PRO_AREA_LIQUIDA;
                    interfaceCaixa.PRO_PESO = produtoCaixa.PRO_PESO;

                    EstruturaProduto Db_EstruturaCaixaChapa = GetEstruturaCaixaChapa(produtoCaixa, db);
                    if (Db_EstruturaCaixaChapa != null)
                    {
                        //chapa intermediária
                        Produto chapa = Db_EstruturaCaixaChapa.ProdutoComponente;
                        interfaceCaixa.GRP_ID_COMPOSICAO = chapa.GRP_ID;
                        interfaceCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA = chapa.PRO_LARGURA_PECA;
                        interfaceCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA = chapa.PRO_COMPRIMENTO_PECA;
                        interfaceCaixa.PRO_AREA_LIQUIDA_CHAPA = chapa.PRO_LARGURA_PECA/1000.0* chapa.PRO_COMPRIMENTO_PECA/1000.0;
                        //interfaceCaixa.PRO_AREA_LIQUIDA_CHAPA = chapa.PRO_AREA_LIQUIDA;

                        interfaceCaixa.PRO_PESO_CHAPA = chapa.PRO_PESO;


                        // estrutura produto caixa chapa
                        // CAIXAS_POR_CHAPA é um campo calculado
                        interfaceCaixa.CAIXAS_POR_CHAPA = 1 / Db_EstruturaCaixaChapa.EST_QUANT;

                    }

                    EstruturaProduto Db_EstruturaConjuntoCaixa = GetEstruturaConjuntoCaixa(produtoCaixa, db);
                    if (Db_EstruturaConjuntoCaixa != null)
                    {
                        Produto conjunto = Db_EstruturaConjuntoCaixa.Produto;
                        // produto conjunto
                        interfaceCaixa.PRO_ID_CONJUNTO = conjunto.PRO_ID;
                        interfaceCaixa.PRO_DESCRICAO_CONJUNTO = conjunto.PRO_DESCRICAO;
                        // estrutura produto conjunto caixa
                        interfaceCaixa.CAIXAS_POR_CONJUNTO = Db_EstruturaConjuntoCaixa.EST_QUANT;
                    }

                    EstruturaProduto Db_EstruturaCaixaCliche = GetEstruturaCaixaCliche(produtoCaixa, db);
                    if (Db_EstruturaCaixaCliche != null)
                    {
                        Produto cliche = Db_EstruturaCaixaCliche.ProdutoComponente;
                        interfaceCaixa.PRO_ID_CLICHE = cliche.PRO_ID;
                    }

                    EstruturaProduto Db_EstruturaCaixaFaca = GetEstruturaCaixaFaca(produtoCaixa, db);
                    if (Db_EstruturaCaixaFaca != null)
                    {
                        Produto faca = Db_EstruturaCaixaFaca.ProdutoComponente;
                        interfaceCaixa.PRO_ID_FACA = faca.PRO_ID;
                    }

                    newList.Add(interfaceCaixa);
                }
            }
            objects.RemoveRange(0, objects.Count);
            objects.AddRange(newList);
        }

        public double ObterM2Unitario(string PRO_ID)
        {
            Produto caixa;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                caixa = db.Produto.AsNoTracking().Where(cx => cx.PRO_ID == PRO_ID)
                .Select(ch => new Produto
                {
                    PRO_ID = PRO_ID,
                    PRO_LARGURA_PECA_CHAPA = ch.PRO_LARGURA_PECA_CHAPA,
                    PRO_COMPRIMENTO_PECA_CHAPA = ch.PRO_COMPRIMENTO_PECA_CHAPA,
                    QTD_CHAPA = ch.QTD_CHAPA
                }).FirstOrDefault();
            }

            double m2Unitario = 0;
            if (caixa != null)
                m2Unitario = (caixa.PRO_LARGURA_PECA_CHAPA.Value / 1000) * (caixa.PRO_COMPRIMENTO_PECA_CHAPA.Value / 1000) / caixa.QTD_CHAPA.Value;

            return m2Unitario;
        }
    }

    [Display(Name = "CAIXAS E ACESSÓRIOS")]
    [ClassMapped(nameOfClassMapped = nameof(Produto))]
    public class InterfaceTelaProdutoCaixa : InterfaceDeTelas
    {
        public InterfaceTelaProdutoCaixa()
        {
            NamespaceOfClassMapped = typeof(ProdutoCaixa).FullName;
        }

        [TAB(Value = "PRINCIPAL", Index = 1.0f)] [Display(Name = "CÓDIGO PRODUTO")] [Required(ErrorMessage = "Campo PRO_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "ID TIPO ABNT")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo ABN_ID")] public string ABN_ID { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 2.0f)] [Display(Name = "DESCRICAO")] [Required(ErrorMessage = "Campo PRO_DESCRICAO requirido.")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 3.0f)] [Display(Name = "COD INTEGRAÇÃO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO")] public string PRO_ID_INTEGRACAO { get; set; }
        [TAB(Value = "PRINCIPAL", Index = 4.0f)] [Display(Name = "COD INTEGRAÇÃO ERP")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_ID_INTEGRACAO_ERP")] public string PRO_ID_INTEGRACAO_ERP { get; set; }
        [Combobox(Description = "ATIVO", Value = "A")]
        [Combobox(Description = "OBSOLETO", Value = "O")]
        [Combobox(Description = "BLOQUEADO", Value = "B")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "STATUS")] [MaxLength(2, ErrorMessage = "Maximode 2 caracteres, campo PRO_STATUS")] public string PRO_STATUS { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CODIGO DESENHO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_COD_DESENHO")] public string PRO_COD_DESENHO { get; set; }
        [Combobox(Description = "AUTOMÁTICO", Value = "1")]
        [Combobox(Description = "GRAMPO", Value = "2")]
        [Combobox(Description = "COLA", Value = "3")]
        [Combobox(Description = "S/ FECHAMENTO", Value = "4")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FECHAMENTO")] public string PRO_FECHAMENTO { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_LARG")] public int? PRO_FITILHOS_FARDO_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_FARDO_COMP")] public int? PRO_FITILHOS_FARDO_COMP { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_LARG")] public int? PRO_FITILHOS_PALETE_LARG { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "FITILHOS_PALETE_COMP")] public int? PRO_FITILHOS_PALETE_COMP { get; set; }
        [Combobox(Description = "SIM", Value = "1")]
        [Combobox(Description = "NÃO", Value = "2")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "CANTONEIRA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_CANTONEIRA")] public string PRO_CANTONEIRA { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "QTD_ESPELHO")] public int? PRO_QTD_ESPELHO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoCliches")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD CLICHE")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo COD CLICHE")] public string PRO_ID_CLICHE { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoFaca")]
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD FACA")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo COD FACA")] public string PRO_ID_FACA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "PEÇAS/FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "CAMADAS/UE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "LARGURA (EMBALADA)")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "COMPRIMENTO (EMBALADA)")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Range(0.001, Double.PositiveInfinity)] [Display(Name = "ALTURA_EMBALADA")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "GRUPO PALETIZAÇÃO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_GRUPO_PALETIZACAO")] public string PRO_GRUPO_PALETIZACAO { get; set; }
        [Combobox(Description = "Largura", Value = "L")]
        [Combobox(Description = "Comprimento", Value = "C")]
        [TAB(Value = "ACABAMENTO")] [Display(Name = "FRENTE")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_FRENTE")] public string PRO_FRENTE { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "LARGURA PECA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "COMPRIMENTO PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "ALTURA PECA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "LARGURA INTERNA")] public double? PRO_LARGURA_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "COMPRIMENTO INTERNA")] public double? PRO_COMPRIMENTO_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "ALTURA INTERNA")] public double? PRO_ALTURA_INTERNA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "AREA LIQUIDA DA CAIXA")] public double? PRO_AREA_LIQUIDA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "PESO LIQUIDO DA CAIXA")] public double? PRO_PESO { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "PEÇAS POR UNIDADE")] public double? PRO_PECAS_DA_PECA { get; set; }
        [Combobox(Description = "INTERNO", Value = "1")]
        [Combobox(Description = "EXTERNO", Value = "2")]
        [Combobox(Description = "S/ LAP", Value = "3")]
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "TIPO_LAP")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_TIPO_LAP")] public string PRO_TIPO_LAP { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "TAMANHO_LAP")] public double? PRO_TAMANHO_LAP { get; set; }
        [Combobox(Description = "LARGURA", Value = "1")]
        [Combobox(Description = "COMPRIMENTO", Value = "2")]
        [Combobox(Description = "S/ LAP PROLONGADO", Value = "3")]
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "LAP_PROLONGADO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_LAP_PROLONGADO")] public string PRO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "TAMANHO_LAP_PROLONGADO")] public double? PRO_TAMANHO_LAP_PROLONGADO { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "VINCOS LARGURA")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_LARGURA")] public string PRO_VINCOS_LARGURA { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "VINCOS COMPRIMENTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_VINCOS_COMPRIMENTO")] public string PRO_VINCOS_COMPRIMENTO { get; set; }
        [TAB(Value = "MEDIDAS PRODUTO")] [Display(Name = "VINCOS ONDULADEIRA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo VINCOS_ONDULADEIRA")] public string PRO_VINCOS_ONDULADEIRA { get; set; }

        // Não sabemos o que essa propriedade representa - PENDENCIA [TAB(Value = "PRODUCAO")] [Display(Name = "TEMPO_PRODUCAO_CONJUNTO")] public double? PRO_TEMPO_PRODUCAO_CONJUNTO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "VOLTAS FILME PALETE")] public int? PRO_FILME_PALETE { get; set; }

        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_COMPRIMENTO")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_COMPRIMENTO")] public string PRO_ROTACIONA_COMPRIMENTO { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_LARGURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_LARGURA")] public string PRO_ROTACIONA_LARGURA { get; set; }
        [Combobox(Description = "COMPRIMENTO", Value = "C")]
        [Combobox(Description = "LARGURA", Value = "L")]
        [Combobox(Description = "AMBOS", Value = "A")]
        [Combobox(Description = "NENHUM", Value = "N")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "ROTACIONA_ALTURA")] [MaxLength(1, ErrorMessage = "Maximode 1 caracteres, campo PRO_ROTACIONA_ALTURA")] public string PRO_ROTACIONA_ALTURA { get; set; }
        [Combobox(Description = "FARDO", Value = "FARDO")]
        [Combobox(Description = "GRANEL", Value = "GRANEL")]
        [Combobox(Description = "PALETIZADO", Value = "PALETIZADO")]
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TIPO CARGA")] [MaxLength(50, ErrorMessage = "Maximode 50 caracteres, campo TMP_TIPO_CARGA")] public string TMP_TIPO_CARGA { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO CARREGAMENTO/UNI")] public double? PRO_TEMPO_CARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "TEMPO DESCARREGAMENTO/UNI")] public double? PRO_TEMPO_DESCARREGAMENTO_UNITARIO { get; set; }
        [TAB(Value = "EXPEDICAO")] [Display(Name = "PERCENTUAL JANELA EMB")] public double? PRO_PERCENTUAL_JANELA_EMBARQUE { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.GrupoProdutoComposicao")]
        [TAB(Value = "COMPOSICAO")] [Display(Name = "COD COMPOSIÇAO")] [Required(ErrorMessage = "GRP_ID_COMPOSICAO Requirido.")] [MaxLength(30, ErrorMessage = "GRP_ID_COMPOSICAO , Maximo 30 caracteres.")] public string GRP_ID_COMPOSICAO { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "LARGURA PEÇA CHAPA")] [Required(ErrorMessage = "LARGURA_PECA_CHAPA_INTERMEDIARIA Requirido.")] public double? PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "COMPRIMENTO PEÇA CHAPA")] [Required(ErrorMessage = "COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA Requirido.")] public double? PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "AREA LIQUIDA CHAPA")] public double? PRO_AREA_LIQUIDA_CHAPA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "PESO LIQUIDO CHAPA")] public double? PRO_PESO_CHAPA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Required(ErrorMessage = "Campo CAIXAS_POR_CHAPA requirido.")] [Display(Name = "CAIXAS/CHAPA")] public double CAIXAS_POR_CHAPA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "ARRANJO_LARGURA")] public int? PRO_ARRANJO_LARGURA { get; set; }
        [TAB(Value = "COMPOSICAO")] [Display(Name = "ARRANJO_COMPRIMENTO")] public int? PRO_ARRANJO_COMPRIMENTO { get; set; }
        [SEARCH_NOT_FK(namespaceOfForeignClass = "DynamicForms.Areas.PlugAndPlay.Models.ProdutoConjunto")]
        [TAB(Value = "CONJUNTO")] [Display(Name = "CÓDIGO DO CONJUNTO")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo PRO_ID")] public string PRO_ID_CONJUNTO { get; set; }
        [TAB(Value = "CONJUNTO")] [Display(Name = "DESCRIÇÃO DO CONJUNTO")] [MaxLength(100, ErrorMessage = "Maximode 100 caracteres, campo PRO_DESCRICAO")] public string PRO_DESCRICAO_CONJUNTO { get; set; }
        [TAB(Value = "CONJUNTO")] [Display(Name = "PEÇAS/CONJUNTO")] public double CAIXAS_POR_CONJUNTO { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }
        [NotMapped] public string NamespaceOfClassMapped { get; set; }

        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual GrupoProdutoConjunto GrupoProdutoConjunto { get; set; }
        public virtual GrupoProdutoOutros GrupoProdutoOutros { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }

        public virtual ICollection<EstruturaProduto> EstruturaProduto { get; set; }
        public virtual ICollection<Roteiro> Roteiro { get; set; }
        public virtual ICollection<ProdutoCaixa> ProdutoCaixas { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> newList = new List<object>();
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var obj in objects)
                {
                    InterfaceTelaProdutoCaixa interfaceProdCaixa = (InterfaceTelaProdutoCaixa)obj;
                    ProdutoCaixa pc = InterfaceTelaToProdutoCaixa(interfaceProdCaixa);
                    pc.PlayAction = interfaceProdCaixa.PlayAction;
                    newList.Add(pc);
                    interfaceProdCaixa.PlayAction = "ok";
                }
            }
            objects.AddRange(newList);
            return true;
        }

        private ProdutoCaixa InterfaceTelaToProdutoCaixa(InterfaceTelaProdutoCaixa interfaceTelaProdutoCaixa)
        {
            ProdutoCaixa caixa = new ProdutoCaixa();

            caixa.PRO_ID = interfaceTelaProdutoCaixa.PRO_ID;
            caixa.ABN_ID = interfaceTelaProdutoCaixa.ABN_ID;
            caixa.PRO_DESCRICAO = interfaceTelaProdutoCaixa.PRO_DESCRICAO;
            caixa.PRO_ID_INTEGRACAO = interfaceTelaProdutoCaixa.PRO_ID_INTEGRACAO;
            caixa.PRO_ID_INTEGRACAO_ERP = interfaceTelaProdutoCaixa.PRO_ID_INTEGRACAO_ERP;
            caixa.PRO_STATUS = interfaceTelaProdutoCaixa.PRO_STATUS;
            caixa.PRO_FARDOS_POR_CAMADA = interfaceTelaProdutoCaixa.PRO_FARDOS_POR_CAMADA;
            caixa.PRO_CAMADAS_POR_PALETE = interfaceTelaProdutoCaixa.PRO_CAMADAS_POR_PALETE;
            caixa.PRO_GRUPO_PALETIZACAO = interfaceTelaProdutoCaixa.PRO_GRUPO_PALETIZACAO;
            caixa.PRO_PECAS_POR_FARDO = interfaceTelaProdutoCaixa.PRO_PECAS_POR_FARDO;
            caixa.PRO_LARGURA_EMBALADA = interfaceTelaProdutoCaixa.PRO_LARGURA_EMBALADA;
            caixa.PRO_COMPRIMENTO_EMBALADA = interfaceTelaProdutoCaixa.PRO_COMPRIMENTO_EMBALADA;
            caixa.PRO_ALTURA_EMBALADA = interfaceTelaProdutoCaixa.PRO_ALTURA_EMBALADA;
            caixa.PRO_FRENTE = interfaceTelaProdutoCaixa.PRO_FRENTE;
            caixa.TEM_ID = interfaceTelaProdutoCaixa.TEM_ID;
            caixa.UNI_ID = interfaceTelaProdutoCaixa.UNI_ID;
            caixa.GRP_ID = interfaceTelaProdutoCaixa.GRP_ID;
            caixa.PRO_LARGURA_PECA = interfaceTelaProdutoCaixa.PRO_LARGURA_PECA;
            caixa.PRO_COMPRIMENTO_PECA = interfaceTelaProdutoCaixa.PRO_COMPRIMENTO_PECA;
            caixa.PRO_ALTURA_PECA = interfaceTelaProdutoCaixa.PRO_ALTURA_PECA;
            caixa.PRO_LARGURA_INTERNA = interfaceTelaProdutoCaixa.PRO_LARGURA_INTERNA;
            caixa.PRO_COMPRIMENTO_INTERNA = interfaceTelaProdutoCaixa.PRO_COMPRIMENTO_INTERNA;
            caixa.PRO_ALTURA_INTERNA = interfaceTelaProdutoCaixa.PRO_ALTURA_INTERNA;
            caixa.PRO_AREA_LIQUIDA = interfaceTelaProdutoCaixa.PRO_AREA_LIQUIDA;
            caixa.PRO_PESO = interfaceTelaProdutoCaixa.PRO_PESO;
            caixa.PRO_PECAS_DA_PECA = interfaceTelaProdutoCaixa.PRO_PECAS_DA_PECA;
            caixa.PRO_ROTACIONA_COMPRIMENTO = interfaceTelaProdutoCaixa.PRO_ROTACIONA_COMPRIMENTO;
            caixa.PRO_ROTACIONA_LARGURA = interfaceTelaProdutoCaixa.PRO_ROTACIONA_LARGURA;
            caixa.PRO_ROTACIONA_ALTURA = interfaceTelaProdutoCaixa.PRO_ROTACIONA_ALTURA;
            caixa.TMP_TIPO_CARGA = interfaceTelaProdutoCaixa.TMP_TIPO_CARGA;
            caixa.PRO_TEMPO_CARREGAMENTO_UNITARIO = interfaceTelaProdutoCaixa.PRO_TEMPO_CARREGAMENTO_UNITARIO;
            caixa.PRO_TEMPO_DESCARREGAMENTO_UNITARIO = interfaceTelaProdutoCaixa.PRO_TEMPO_DESCARREGAMENTO_UNITARIO;
            caixa.PRO_PERCENTUAL_JANELA_EMBARQUE = interfaceTelaProdutoCaixa.PRO_PERCENTUAL_JANELA_EMBARQUE;
            caixa.PRO_COD_DESENHO = interfaceTelaProdutoCaixa.PRO_COD_DESENHO;
            caixa.PRO_TIPO_LAP = interfaceTelaProdutoCaixa.PRO_TIPO_LAP;
            caixa.PRO_TAMANHO_LAP = interfaceTelaProdutoCaixa.PRO_TAMANHO_LAP;
            caixa.PRO_LAP_PROLONGADO = interfaceTelaProdutoCaixa.PRO_LAP_PROLONGADO;
            caixa.PRO_TAMANHO_LAP_PROLONGADO = interfaceTelaProdutoCaixa.PRO_TAMANHO_LAP_PROLONGADO;
            caixa.PRO_ARRANJO_LARGURA = interfaceTelaProdutoCaixa.PRO_ARRANJO_LARGURA;
            caixa.PRO_ARRANJO_COMPRIMENTO = interfaceTelaProdutoCaixa.PRO_ARRANJO_COMPRIMENTO;
            caixa.PRO_FITILHOS_FARDO_LARG = interfaceTelaProdutoCaixa.PRO_FITILHOS_FARDO_LARG;
            caixa.PRO_FITILHOS_FARDO_COMP = interfaceTelaProdutoCaixa.PRO_FITILHOS_FARDO_COMP;
            caixa.PRO_FILME_PALETE = interfaceTelaProdutoCaixa.PRO_FILME_PALETE;
            caixa.PRO_CANTONEIRA = interfaceTelaProdutoCaixa.PRO_CANTONEIRA;
            caixa.PRO_FECHAMENTO = interfaceTelaProdutoCaixa.PRO_FECHAMENTO;
            caixa.PRO_VINCOS_LARGURA = interfaceTelaProdutoCaixa.PRO_VINCOS_LARGURA;
            caixa.PRO_VINCOS_COMPRIMENTO = interfaceTelaProdutoCaixa.PRO_VINCOS_COMPRIMENTO;
            caixa.PRO_VINCOS_ONDULADEIRA = interfaceTelaProdutoCaixa.PRO_VINCOS_ONDULADEIRA;
            caixa.PRO_QTD_ESPELHO = interfaceTelaProdutoCaixa.PRO_QTD_ESPELHO;
            caixa.PRO_FITILHOS_PALETE_LARG = interfaceTelaProdutoCaixa.PRO_FITILHOS_PALETE_LARG;
            caixa.PRO_FITILHOS_PALETE_COMP = interfaceTelaProdutoCaixa.PRO_FITILHOS_PALETE_COMP;

            //chapa intermediária            
            caixa.GRP_ID_COMPOSICAO = interfaceTelaProdutoCaixa.GRP_ID_COMPOSICAO;
            caixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA = interfaceTelaProdutoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA;
            caixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA = interfaceTelaProdutoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA;
            caixa.PRO_PESO_CHAPA = interfaceTelaProdutoCaixa.PRO_PESO_CHAPA;


            // estrutura produto
            caixa.CAIXAS_POR_CHAPA = interfaceTelaProdutoCaixa.CAIXAS_POR_CHAPA;
            // produto conjunto
            caixa.PRO_ID_CONJUNTO = interfaceTelaProdutoCaixa.PRO_ID_CONJUNTO;
            caixa.PRO_DESCRICAO_CONJUNTO = interfaceTelaProdutoCaixa.PRO_DESCRICAO_CONJUNTO;
            caixa.CAIXAS_POR_CONJUNTO = interfaceTelaProdutoCaixa.CAIXAS_POR_CONJUNTO;

            //cliche
            caixa.PRO_ID_CLICHE = interfaceTelaProdutoCaixa.PRO_ID_CLICHE;
            //faca
            caixa.PRO_ID_FACA = interfaceTelaProdutoCaixa.PRO_ID_FACA;
            return caixa;
        }

    }


}
