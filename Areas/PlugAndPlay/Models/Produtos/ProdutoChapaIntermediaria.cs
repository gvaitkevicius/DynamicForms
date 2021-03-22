using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    [Display(Name = "CHAPAS INTERMEDIÁRIAS")]
    public class ProdutoChapaIntermediaria : ProdutoAbstrato
    {
        public ProdutoChapaIntermediaria()
        {
            V_ROTEIROS_CHAPAS = new HashSet<V_ROTEIROS_CHAPAS>();
        }

        [TAB(Value = "ACABAMENTO")] [Display(Name = "LARGURA_EMBALADA")] public double? PRO_LARGURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "COMPRIMENTO_EMBALADA")] public double? PRO_COMPRIMENTO_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "ALTURA_EMBALADA")] public double? PRO_ALTURA_EMBALADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "FARDOS/CAMADA")] public double? PRO_FARDOS_POR_CAMADA { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "CAMADAS/PALETE")] public double? PRO_CAMADAS_POR_PALETE { get; set; }
        [TAB(Value = "ACABAMENTO")] [Display(Name = "PECAS_POR_FARDO")] public double? PRO_PECAS_POR_FARDO { get; set; }
        [TAB(Value = "QUALIDADE")] [Display(Name = "TEMPLATE DE TESTE")] public int? TEM_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "UN MEDIDA")] [Required(ErrorMessage = "Campo UNI_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo UNI_ID")] public string UNI_ID { get; set; }
        [TAB(Value = "PRINCIPAL")] [Display(Name = "COD GRUPO")] [Required(ErrorMessage = "Campo GRP_ID requirido.")] [MaxLength(30, ErrorMessage = "Maximode 30 caracteres, campo GRP_ID")] public string GRP_ID { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "LARGURA_PECA")] public double? PRO_LARGURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "COMPRIMENTO_PECA")] public double? PRO_COMPRIMENTO_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "AREA LIQUIDA")] public double? PRO_AREA_LIQUIDA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "AREA LIQUIDA")] public double? PRO_PESO { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "ALTURA_PECA")] public double? PRO_ALTURA_PECA { get; set; }
        [TAB(Value = "MEDIDAS")] [Display(Name = "PRO_VINCOS_LARGURA")] public string PRO_VINCOS_LARGURA { get; set; }

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


        [HIDDENINTERFACE] public string PRO_GRUPO_PALETIZACAO { get; set; }
        public virtual TemplateDeTestes TemplateDeTestes { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual GrupoProdutoComposicao GrupoProdutoComposicao { get; set; }
        public virtual ProdutoCaixa GrupoPaletizacao { get; set; }

        [Display(Name = "Roteiro")] public virtual ICollection<V_ROTEIROS_CHAPAS> V_ROTEIROS_CHAPAS { get; set; }

        public bool BeforeChanges(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert)
        {
            List<object> newList = new List<object>();
            using (JSgi db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                foreach (var obj in objects)
                {
                    if (obj.GetType().Name != nameof(ProdutoChapaIntermediaria))
                        continue;

                    ProdutoChapaIntermediaria produtoChapa = (ProdutoChapaIntermediaria)obj;

                    if (!produtoChapa.PlayAction.Equals("insert", StringComparison.OrdinalIgnoreCase) &&
                        !produtoChapa.PlayAction.Equals("update", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (produtoChapa.PRO_CAMADAS_POR_PALETE == null || produtoChapa.PRO_CAMADAS_POR_PALETE <= 0)
                    {
                        produtoChapa.PRO_CAMADAS_POR_PALETE = 1;
                    }
                    if (produtoChapa.PRO_FARDOS_POR_CAMADA == null || produtoChapa.PRO_FARDOS_POR_CAMADA <= 0)
                    {
                        produtoChapa.PRO_FARDOS_POR_CAMADA = 1;
                    }
                    if (produtoChapa.PRO_PECAS_POR_FARDO == null || produtoChapa.PRO_PECAS_POR_FARDO <= 0)
                    {
                        produtoChapa.PRO_PECAS_POR_FARDO = 1;
                    }

                    if (produtoChapa.PRO_LARGURA_EMBALADA == null || produtoChapa.PRO_LARGURA_EMBALADA <= 0)
                    {
                        produtoChapa.PRO_LARGURA_EMBALADA = produtoChapa.PRO_LARGURA_PECA;
                    }
                    if (produtoChapa.PRO_COMPRIMENTO_EMBALADA == null || produtoChapa.PRO_COMPRIMENTO_EMBALADA <= 0)
                    {
                        produtoChapa.PRO_COMPRIMENTO_EMBALADA = produtoChapa.PRO_COMPRIMENTO_PECA;
                    }
                    if (produtoChapa.PRO_ALTURA_EMBALADA == null || produtoChapa.PRO_ALTURA_EMBALADA <= 0)
                    {
                        produtoChapa.PRO_ALTURA_EMBALADA = produtoChapa.PRO_ALTURA_PECA;
                    }
                }
            }
            objects.AddRange(newList);
            return true;
        }

        public bool AfterChangesInTransaction(List<object> objects, ref CloneObjeto cloneObjeto, List<LogPlay> Logs, ref int modo_insert, JSgi db = null)
        {
            if (db == null)
            {
                db = new ContextFactory().CreateDbContext(new string[] { });
            }

            List<object> newObjects = new List<object>();

            IEnumerable<ProdutoChapaIntermediaria> chapas = objects.Where(r => r.GetType().Name == nameof(ProdutoChapaIntermediaria)).Cast<ProdutoChapaIntermediaria>();
            foreach (ProdutoChapaIntermediaria chapa in chapas)
            {
                if (chapa.PlayAction.ToLower() == "insert")
                {
                    #region Criando Roteiro da Chapa
                    Roteiro roteiroChapa = GetRoteiroChapa(chapa);
                    roteiroChapa.PlayAction = "insert";
                    newObjects.Add(roteiroChapa);
                    #endregion Criando Roteiro da Chapa

                    #region Criando Estrutura da Chapa
                    List<EstruturaProduto> estruturasDaChapa = GetEstruturasDaChapa(chapa, db);
                    newObjects.AddRange(estruturasDaChapa);
                    #endregion Criando Estrutura da Chapa
                }
            }

            // Aqui é adicionado o prefixo AFTER- para que o método AfterChangesInTransaction do MasterController possa identificar quais são os novos objetos da lista
            foreach (dynamic item in newObjects)
            {
                item.PlayAction = $"AFTER-{item.PlayAction}";
            }
            objects.InsertRange(0, newObjects);
            return true;
        }

        private List<EstruturaProduto> GetEstruturasDaChapa(ProdutoChapaIntermediaria produtoChapa, JSgi db)
        {
            List<EstruturaProduto> estruturasDaChapa = new List<EstruturaProduto>();
            EstruturaProduto estrutura = null;

            #region Estrutura Chapa Cola
            estrutura = new EstruturaProduto
            {
                EST_DATA_VALIDADE = DateTime.MaxValue,
                PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                PRO_ID_COMPONENTE = "COLA01",
                EST_QUANT = 1, // Pendencia
                EST_DATA_INCLUSAO = DateTime.Now,
                EST_BASE_PRODUCAO = 1000.0,
                PlayAction = "insert"
            };
            estruturasDaChapa.Add(estrutura);
            #endregion Estrutura Chapa Cola

            GrupoProdutoComposicao grupoProdutoComposicao = db.GrupoProdutoComposicao.AsNoTracking()
                                                                .Where(x => x.GRP_ID == produtoChapa.GRP_ID).FirstOrDefault();

            #region Estrutura Chapa Papeis
            if (!string.IsNullOrEmpty(grupoProdutoComposicao.GRP_PAPEL1))
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = grupoProdutoComposicao.GRP_PAPEL1,
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            if (!string.IsNullOrEmpty(grupoProdutoComposicao.GRP_PAPEL2))
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = grupoProdutoComposicao.GRP_PAPEL2,
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            if (!string.IsNullOrEmpty(grupoProdutoComposicao.GRP_PAPEL3))
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = grupoProdutoComposicao.GRP_PAPEL3,
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            if (!string.IsNullOrEmpty(grupoProdutoComposicao.GRP_PAPEL4))
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = grupoProdutoComposicao.GRP_PAPEL4,
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            if (!string.IsNullOrEmpty(grupoProdutoComposicao.GRP_PAPEL5))
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = grupoProdutoComposicao.GRP_PAPEL5,
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            #endregion Estrutura Chapa Papeis

            #region Estrutura Chapa Resina
            if (grupoProdutoComposicao.GRP_RESINA_INTERNA == "S")
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = "RESINA01",
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            if (grupoProdutoComposicao.GRP_RESINA_EXTERNA == "S")
            {
                estrutura = new EstruturaProduto
                {
                    EST_DATA_VALIDADE = DateTime.MaxValue,
                    PRO_ID_PRODUTO = produtoChapa.PRO_ID,
                    PRO_ID_COMPONENTE = "RESINA01",
                    EST_QUANT = 1, // Pendencia
                    EST_DATA_INCLUSAO = DateTime.Now,
                    EST_BASE_PRODUCAO = 1000.0,
                    PlayAction = "insert"
                };
                estruturasDaChapa.Add(estrutura);
            }
            #endregion Estrutura Chapa Resina

            #region removendo estruturas repetidas
            List<IGrouping<string, EstruturaProduto>> papeisRepetidos = estruturasDaChapa
                                                                         .GroupBy(x => x.PRO_ID_COMPONENTE)
                                                                         .Where(g => g.Count() > 1)
                                                                         .ToList();

            foreach (IGrouping<string, EstruturaProduto> item in papeisRepetidos)
            {
                estrutura = estruturasDaChapa.Where(x => x.PRO_ID_COMPONENTE == item.Key).FirstOrDefault();
                estrutura.EST_QUANT = item.Count();

                foreach (EstruturaProduto aux in item)
                {
                    estruturasDaChapa.Remove(aux);
                }
                estruturasDaChapa.Add(estrutura);
            }

            #endregion removendo estruturas repetidas

            return estruturasDaChapa;
        }

        private Roteiro GetRoteiroChapa(ProdutoChapaIntermediaria produtoChapa)
        {
            Roteiro roteiroChapa = new Roteiro();
            roteiroChapa.GMA_ID = "OND";
            roteiroChapa.MAQ_ID = "PLAYSIS";
            roteiroChapa.PRO_ID = produtoChapa.PRO_ID;
            roteiroChapa.ROT_SEQ_TRANFORMACAO = 10;
            roteiroChapa.ROT_PECAS_POR_PULSO = 1;
            roteiroChapa.ROT_PRIORIDADE_INFORMADA = 0;
            roteiroChapa.ROT_PERFORMANCE = 1000;

            return roteiroChapa;
        }

        public double ObterM2Unitario(string PRO_ID)
        {
            ProdutoChapaIntermediaria chapa;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                chapa = db.ProdutoChapaIntermediaria.AsNoTracking().Where(ch => ch.PRO_ID == PRO_ID)
                .Select(ch => new ProdutoChapaIntermediaria
                {
                    PRO_ID = PRO_ID,
                    PRO_LARGURA_PECA = ch.PRO_LARGURA_PECA,
                    PRO_COMPRIMENTO_PECA = ch.PRO_COMPRIMENTO_PECA
                }).FirstOrDefault();
            }

            double m2Unitario = 0;
            if (chapa != null)
                m2Unitario = (chapa.PRO_LARGURA_PECA.Value / 1000) * (chapa.PRO_COMPRIMENTO_PECA.Value / 1000);

            return m2Unitario;
        }
    }
}
