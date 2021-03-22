
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TemplateDeTestes
    {
        public TemplateDeTestes()
        {
            ProdutosCaixa = new HashSet<ProdutoCaixa>();
            ProdutoChapaIntermediaria = new HashSet<ProdutoChapaIntermediaria>();
            ProdutoTinta = new HashSet<ProdutoTinta>();
            GrupoProdutoOutros = new HashSet<GrupoProdutoOutros>();
            GrupoProdutoConjunto = new HashSet<GrupoProdutoConjunto>();
            GrupoProdutoComposicao = new HashSet<GrupoProdutoComposicao>();
            GrupoProdutoPalete = new HashSet<GrupoProdutoPalete>();
            GrupoProduto = new HashSet<GrupoProduto>();
            Produtos = new HashSet<Produto>();
            ProdutoCliches = new HashSet<ProdutoCliches>();
            ProdutoPalete = new HashSet<ProdutoPalete>();
            ProdutoConjunto = new HashSet<ProdutoConjunto>();
            ProdutoFacas = new HashSet<ProdutoFaca>();
            ProdutoChapaVenda = new HashSet<ProdutoChapaVenda>();
            //--
            Maquina = new HashSet<Maquina>();
            Roteiro = new HashSet<Roteiro>();
            TemplateTipoTeste = new HashSet<TemplateTipoTeste>();
            TemplateTipoInspecaoVisual = new HashSet<TemplateTipoInspecaoVisual>();
        }

        public int TEM_ID { get; set; }
        public string TEM_DESCRICAO { get; set; }
        public string Observacao { get; set; }
        [NotMapped] public string PlayAction { get; set; }
        [NotMapped] public string PlayMsgErroValidacao { get; set; }
        [NotMapped] public int? IndexClone { get; set; }

        public ICollection<Maquina> Maquina { get; set; }
        public ICollection<Roteiro> Roteiro { get; set; }
        public ICollection<TemplateTipoTeste> TemplateTipoTeste { get; set; }
        public ICollection<TemplateTipoInspecaoVisual> TemplateTipoInspecaoVisual { get; set; }

        public virtual ICollection<ProdutoCaixa> ProdutosCaixa { get; set; }
        public virtual ICollection<ProdutoChapaIntermediaria> ProdutoChapaIntermediaria { get; set; }
        public virtual ICollection<ProdutoTinta> ProdutoTinta { get; set; }
        public virtual ICollection<GrupoProdutoOutros> GrupoProdutoOutros { get; set; }
        public virtual ICollection<GrupoProdutoConjunto> GrupoProdutoConjunto { get; set; }
        public virtual ICollection<GrupoProdutoComposicao> GrupoProdutoComposicao { get; set; }
        public virtual ICollection<GrupoProdutoPalete> GrupoProdutoPalete { get; set; }
        public virtual ICollection<GrupoProduto> GrupoProduto { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<ProdutoCliches> ProdutoCliches { get; set; }
        public virtual ICollection<ProdutoPalete> ProdutoPalete { get; set; }
        public virtual ICollection<ProdutoConjunto> ProdutoConjunto { get; set; }
        public virtual ICollection<ProdutoFaca> ProdutoFacas { get; set; }
        public virtual ICollection<ProdutoChapaVenda> ProdutoChapaVenda { get; set; }
        public void Inserir_Tipos_Testes(List<object> objects, ref List<LogPlay> Logs)
        {
            int TemId = -1;
            //Para cada item da lista
            foreach (var item in objects)
            {
                TemplateDeTestes _TemPlateDeTestes = (TemplateDeTestes)item;
                TemId = _TemPlateDeTestes.TEM_ID;
                Logs.Add(new LogPlay(this.ToString(), "PROTOCOLO", "LINK", "/PlugAndPlay/Qualidade/TemplateDeTestes?TemId=", "" + TemId + ""));
            }

        }

        public int GetTemplateOPProduzindo(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string ROT_SEQ_TRANSFORMACAO, string FPR_SEQ_REPETICAO)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                int _TemplateTestes = 0;
                bool flag = true;
                if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID) || String.IsNullOrEmpty(ROT_MAQ_ID) || String.IsNullOrEmpty(FPR_SEQ_REPETICAO) || String.IsNullOrEmpty(ROT_SEQ_TRANSFORMACAO))
                {
                    flag = false;
                }
                if (flag)
                {

                    _TemplateTestes = (from vfp in db.ViewFilaProducao
                                       where
                                            vfp.OrdId.Equals(ORD_ID) &&
                                            vfp.PaProId.Equals(ROT_PRO_ID) &&
                                            vfp.RotMaqId.Equals(ROT_MAQ_ID) &&
                                            vfp.FprSeqRepeticao == Convert.ToInt32(FPR_SEQ_REPETICAO) &&
                                            vfp.RotSeqTransformacao == Convert.ToInt32(ROT_SEQ_TRANSFORMACAO)
                                       select vfp.TEMPLATE_TESTES.Value).FirstOrDefault();
                }
                return _TemplateTestes;
            }
        }
        [HIDDEN]
        public int GetTemplateOP(string ORD_ID, string ROT_PRO_ID, string ROT_MAQ_ID, string ROT_SEQ_TRANSFORMACAO, string FPR_SEQ_REPETICAO)
        {
            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                int _TemplateTestes = 0;
                bool flag = true;
                if (String.IsNullOrEmpty(ORD_ID) || String.IsNullOrEmpty(ROT_PRO_ID) || String.IsNullOrEmpty(ROT_MAQ_ID) || String.IsNullOrEmpty(FPR_SEQ_REPETICAO) || String.IsNullOrEmpty(ROT_SEQ_TRANSFORMACAO))
                {
                    flag = false;
                }
                if (flag)
                {
                    var Db_GrupoProduto = (from fila in db.FilaProducao
                                           join produto in db.Produto on
                   fila.ROT_PRO_ID equals produto.PRO_ID
                                           select new { fila.ORD_ID, fila.ROT_PRO_ID, fila.ROT_MAQ_ID, fila.ROT_SEQ_TRANFORMACAO, fila.FPR_SEQ_REPETICAO, produto.GRP_ID } into gruProd
                                           join grupo in GrupoProduto on
                                           gruProd.GRP_ID equals grupo.GRP_ID
                                           where
                                           gruProd.ORD_ID == ORD_ID && gruProd.ROT_PRO_ID == ROT_PRO_ID &&
                                           gruProd.ROT_MAQ_ID == ROT_MAQ_ID &&
                                           gruProd.ROT_SEQ_TRANFORMACAO == Convert.ToInt32(ROT_SEQ_TRANSFORMACAO) &&
                                           gruProd.FPR_SEQ_REPETICAO == Convert.ToInt32(FPR_SEQ_REPETICAO)
                                           select new { grupo.TEM_ID }
                                                ).FirstOrDefault();


                    if (Db_GrupoProduto != null)
                    {
                        _TemplateTestes = Db_GrupoProduto.TEM_ID.Value;
                    }

                    if (_TemplateTestes == 0)
                    {
                        var Db_Roteiro = db.Roteiro.AsNoTracking().Where(x => x.PRO_ID.Equals(ROT_PRO_ID) && x.MAQ_ID.Equals(ROT_MAQ_ID) && x.ROT_SEQ_TRANFORMACAO == Convert.ToInt32(ROT_SEQ_TRANSFORMACAO)).Select(x => x.TEM_ID).FirstOrDefault();
                        if (Db_Roteiro != null)
                        {
                            _TemplateTestes = Db_Roteiro.Value;
                        }

                    }
                    if (_TemplateTestes == 0)
                    {
                        var Db_Maquina = db.Maquina.AsNoTracking().Where(x => x.MAQ_ID.Equals(ROT_MAQ_ID)).Select(x => x.TEM_ID).FirstOrDefault();
                        _TemplateTestes = Db_Maquina ?? _TemplateTestes;
                    }

                }
                return _TemplateTestes;
            }

        }

    }
}