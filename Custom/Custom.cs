using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Models;
using DynamicForms.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicForms.Custom
{
    public class Custom
    {

    }
    static public class BeforeCustom
    {
        public static string ConverterTipoObservacao(string tipoOriginal)
        {
            string retorno = null;
            switch (tipoOriginal)
            {
                case "Onduladeira":
                    retorno = "PO";
                    break;
                case "Conversão":
                    retorno = "PC";
                    break;
                case "Expedição":
                    retorno = "EP";
                    break;
                case "Acabamento":
                    retorno = "AC";
                    break;
                case "Romaneio":
                    retorno = "EP";
                    break;
                default:
                    retorno = tipoOriginal;
                    break;
            }

            return retorno;
        }

        public static List<object> BeforeCustomChanges(List<object> Objetos, ref CloneObjeto cloneObjeto, string classe, ref List<LogPlay> logs)
        {

            if (classe.ToLower() == "dynamicforms.areas.plugandplay.models.observacoes")
            { // PARAIBUNA
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    List<object> newsObs = new List<object>();
                    foreach (object obs in Objetos)
                    {
                        Observacoes observacoes = (Observacoes)obs;
                        observacoes.OBS_TIPO = ConverterTipoObservacao(observacoes.OBS_TIPO);
                    }
                }
            }

            if (classe.ToLower() == "dynamicforms.areas.plugandplay.models.estruturaproduto")
            {
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {// testa se novas tintas estão no banco 
                    List<ProdutoTinta> dbTintas = db.ProdutoTinta.ToList();
                    List<EstruturaProduto> dbEstruturas = db.EstruturaProduto.ToList();
                    //List<object> NovasTintas = new List<object>();
                    List<object> newlist_tintas = new List<object>();
                    foreach (object tinta in Objetos)
                    {
                        dynamic d_tinta = tinta;

                        if (d_tinta.ProdutoComponenteP != null && d_tinta.ProdutoComponenteP.GRP_ID == "TINT")
                        {// caso seja produto componente 

                            string[] vet = null;
                            if (d_tinta.PRO_ID_COMPONENTE.Contains("/"))
                            {
                                vet = d_tinta.PRO_ID_COMPONENTE.Split("/");
                            }
                            if (vet == null && d_tinta.PRO_ID_COMPONENTE.Contains("|"))
                            {
                                vet = d_tinta.PRO_ID_COMPONENTE.Split("|");
                            }
                            if (vet != null && vet.Length > 0)
                            {
                                int j = 0;
                                for (; j < vet.Length; j++)
                                {

                                    if (dbTintas.Where(x => x.PRO_ID.Trim() == vet[j].Trim()).Count() == 0)
                                    {// testa se esta na base de dados CASO NÃO ESTEJA MARCAR REGISTRO COM MENSSAGEM DE ERRO
                                        d_tinta.PlayMsgErroValidacao = " Tinta " + vet[j].Trim() + " não cadastrada";
                                    }
                                    else
                                    {// consuta se esta na tabela de estrutura caso não incluir 

                                        if (dbEstruturas.Where(x => x.PRO_ID_PRODUTO == d_tinta.PRO_ID_PRODUTO.Trim() && x.PRO_ID_COMPONENTE.Trim() == vet[j].Trim()).Count() == 0)
                                        {
                                            EstruturaProduto e = new EstruturaProduto();
                                            e.EST_QUANT = 1;
                                            e.EST_DATA_INCLUSAO = DateTime.Now;
                                            e.EST_DATA_VALIDADE = DateTime.Now.AddYears(100);
                                            e.EST_BASE_PRODUCAO = 1;
                                            e.PRO_ID_PRODUTO = d_tinta.PRO_ID_PRODUTO;
                                            e.PRO_ID_COMPONENTE = vet[j].Trim();
                                            e.EST_TIPO_REQUISICAO = "";
                                            e.EST_CODIGO_DE_EXCECAO = "";
                                            e.PlayAction = "insert";
                                            newlist_tintas.Add((object)e);

                                            d_tinta.PlayAction = "OK";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dbTintas.Where(x => x.PRO_ID.Trim() == d_tinta.PRO_ID_COMPONENTE.Trim()).Count() == 0)
                                {// testa se esta na base de dados CASO NÃO ESTEJA MARCAR REGISTRO COM MENSSAGEM DE ERRO
                                    d_tinta.PlayMsgErroValidacao = "Tinta " + d_tinta.PRO_ID_COMPONENTE + " não cadastrada esta cadastrada. A mesma fas parte da estrutura do produto " + d_tinta.PRO_ID_PRODUTO;
                                }
                                else
                                {//  consuta se esta na tabela de estrutura caso não incluir  

                                    if (dbEstruturas.Where(x => x.PRO_ID_PRODUTO == d_tinta.PRO_ID_PRODUTO.Trim() && x.PRO_ID_COMPONENTE.Trim() == d_tinta.PRO_ID_COMPONENTE.Trim()).Count() == 0)
                                    {
                                        EstruturaProduto e = new EstruturaProduto();
                                        e.EST_QUANT = d_tinta.EST_QUANT;
                                        e.EST_DATA_INCLUSAO = DateTime.Now;
                                        e.EST_DATA_VALIDADE = DateTime.Now.AddYears(100);
                                        e.EST_BASE_PRODUCAO = 1;
                                        e.PRO_ID_PRODUTO = d_tinta.PRO_ID_PRODUTO;
                                        e.PRO_ID_COMPONENTE = d_tinta.PRO_ID_COMPONENTE;
                                        e.EST_TIPO_REQUISICAO = "";
                                        e.EST_CODIGO_DE_EXCECAO = "";
                                        e.PlayAction = "insert";
                                        newlist_tintas.Add((object)e);

                                        d_tinta.PlayAction = "OK";
                                    }
                                }
                            }
                        }

                    }
                    if (newlist_tintas.Count > 0)
                    {
                        Objetos.AddRange(newlist_tintas);
                    }
                }
            }

            if (classe.ToLower() == "dynamicforms.areas.plugandplay.models.produtocaixa")
            {
                using (var db = new ContextFactory().CreateDbContext(new string[] { }))
                {
                    List<object> newList = new List<object>();
                    foreach (var item in Objetos)
                    {
                        if (item.ToString() != "DynamicForms.Areas.PlugAndPlay.Models.ProdutoCaixa")
                        {
                            continue;
                        }

                        ProdutoCaixa produtoCaixa = (ProdutoCaixa)item;

                        double somatorioLargura = 0;
                        if (!String.IsNullOrEmpty(produtoCaixa.PRO_VINCOS_LARGURA))
                        {
                            string[] vincosLargura = produtoCaixa.PRO_VINCOS_LARGURA.ToLower().Split("x");
                            foreach (var vinco in vincosLargura)
                            {
                                somatorioLargura += double.Parse(vinco.Trim());
                            }
                        }

                        double somatorioComprimento = 0;
                        if (!String.IsNullOrEmpty(produtoCaixa.PRO_VINCOS_COMPRIMENTO))
                        {
                            string[] vincosComprimento = produtoCaixa.PRO_VINCOS_COMPRIMENTO.ToLower().Split("x");
                            foreach (var vinco in vincosComprimento)
                            {
                                somatorioComprimento += double.Parse(vinco.Trim());
                            }
                        }

                        if (produtoCaixa.PRO_LARGURA_EMBALADA == -99)
                        {
                            produtoCaixa.PRO_LARGURA_EMBALADA = somatorioLargura;
                        }

                        if (produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA == -99)
                        {
                            produtoCaixa.PRO_LARGURA_PECA_CHAPA_INTERMEDIARIA = somatorioLargura;
                        }

                        if (produtoCaixa.PRO_COMPRIMENTO_EMBALADA == -99)
                        {
                            produtoCaixa.PRO_COMPRIMENTO_EMBALADA = somatorioComprimento;
                        }

                        if (produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA == -99)
                        {
                            produtoCaixa.PRO_COMPRIMENTO_PECA_CHAPA_INTERMEDIARIA = somatorioComprimento;
                        }
                    }
                }
            }

            // UTILIZADO APENAS PARA FAZER A PRIMEIRA CARGA DA INTERFACE, APÓS ISTO, DEVERÁ SER
            // COMENTADO.
            //if (classe.ToUpper() == "DYNAMICFORMS.AREAS.PLUGANDPLAY.MODELS.PRODUTOTINTA")
            //{
            //    using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            //    {// testa se novas tintas estão no banco 
            //        List<ProdutoTinta> dbTintas = db.ProdutoTinta.ToList();
            //        if (dbTintas.Count() > 0)
            //        {
            //            // APENAS CADASTRA 
            //            foreach (object tinta in Objetos)
            //            {
            //                dynamic d_tinta = tinta;
            //                d_tinta.PlayAction = "OK";
            //            }
            //        }
            //        else
            //        {

            //            //List<object> NovasTintas = new List<object>();
            //            List<object> newlist_tintas = new List<object>();
            //            foreach (object tinta in Objetos)
            //            {
            //                dynamic d_tinta = tinta;
            //                d_tinta.PlayAction = "OK";
            //                string[] vet = null;
            //                if (d_tinta.PRO_ID.Contains("/"))
            //                {
            //                    vet = d_tinta.PRO_ID.Split("/");
            //                }
            //                if (vet == null && d_tinta.PRO_ID.Contains("|"))
            //                {
            //                    vet = d_tinta.PRO_ID.Split("|");
            //                }
            //                if (vet != null && vet.Length > 0)
            //                {
            //                    int j = 0;
            //                    for (; j < vet.Length; j++)
            //                    {
            //                        bool estaNalista = false;
            //                        foreach (var item in newlist_tintas)
            //                        {
            //                            ProdutoTinta t = (ProdutoTinta)item;
            //                            if (t.PRO_ID.Trim() == vet[j].Trim().ToUpper())
            //                            {
            //                                estaNalista = true;
            //                            }
            //                        }
            //                        if (!estaNalista)
            //                        {
            //                            if (dbTintas.Where(x => x.PRO_ID.Trim() == vet[j].Trim()).Count() == 0)
            //                            {// testa se esta na base de dados 

            //                                ProdutoTinta t = new ProdutoTinta();
            //                                t.PRO_ID = vet[j].ToUpper().Trim();
            //                                t.PRO_DESCRICAO = vet[j].ToUpper().Trim();
            //                                t.PRO_ID_INTEGRACAO = d_tinta.PRO_ID_INTEGRACAO;
            //                                t.PRO_ID_INTEGRACAO_ERP = d_tinta.PRO_ID_INTEGRACAO_ERP;
            //                                t.UNI_ID = d_tinta.UNI_ID;
            //                                t.GRP_ID = d_tinta.GRP_ID;
            //                                t.PlayAction = "insert";
            //                                newlist_tintas.Add((object)t);
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    bool estaNalista = false;
            //                    foreach (var item in newlist_tintas)
            //                    {
            //                        ProdutoTinta t = (ProdutoTinta)item;
            //                        if (t.PRO_ID.Trim() == d_tinta.PRO_ID.Trim().ToUpper())
            //                        {
            //                            estaNalista = true;
            //                        }
            //                    }
            //                    if (!estaNalista)
            //                    {
            //                        if (dbTintas.Where(x => x.PRO_ID.Trim() == d_tinta.PRO_ID.Trim()).Count() == 0)
            //                        {// testa se esta na base de dados 
            //                            d_tinta.PRO_ID = d_tinta.PRO_ID.Trim();
            //                            d_tinta.PRO_DESCRICAO = d_tinta.PRO_DESCRICAO.Trim();
            //                            d_tinta.PlayAction = "insert";
            //                            d_tinta.UNI_ID = d_tinta.UNI_ID.Trim();

            //                            newlist_tintas.Add(tinta);
            //                        }
            //                    }
            //                }
            //            }
            //            if (newlist_tintas.Count > 0)
            //            {
            //                return newlist_tintas;
            //            }
            //        }
            //    }
            //}
            return Objetos;
        }

    }

    static public class AfterCustom
    {
        public static List<object> AfterCustomChanges(List<object> Objetos, string classe)
        {

            return Objetos;
        }
    }

    static public class ValidationCustom
    {
        public static List<object> ExecuteCustomValidation(List<object> Objetos, string classe)
        {
            return Objetos;
        }
    }
}
