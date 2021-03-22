using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using DynamicForms.Controllers;
using DynamicForms.Models;
using DynamicForms.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;
using Microsoft.EntityFrameworkCore;
using OsmSharp.IO.PBF;
using Rotativa.AspNetCore;
using SkiaSharp;
using OptCuston;

namespace DynamicForms.Areas.PlugAndPlay.Controllers.Reports
{
    [Authorize]
    [Area("plugandplay")]
    public class ReportEtiquetaController : BaseController
    {
        //Mantém todos registros da tabela de EstruturaEtiqueta
        private List<EstruturaEtiqueta> controle_estrutura_etiqueta = new List<EstruturaEtiqueta>();

        public IActionResult EtiquetaIndividualLoteProduto(string etiqueta)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportEtiquetaController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                List<string> listaEti = new List<string>();
                var ids = etiqueta.Split(",").Select(Int32.Parse).ToList();
                #region Dados da etiqueta
                var dadosEtiqueta = Consultaetiqueta(db,$"'{String.Join("','", ids.Select(x => x).ToList())}'");
                List<EstruturaEtiqueta> estruturas_etiqueta = db.EstruturaEtiqueta.AsNoTracking().ToList();

                foreach (var etidados in dadosEtiqueta)
                {
                    for (int j = 0; j < etidados.ETI_NUMERO_COPIAS; j++)
                    {
                        dynamic dados_etiqueta = new ExpandoObject();
                        dados_etiqueta.ORD_ID = etidados.ORD_ID;
                        dados_etiqueta.ORD_QUANTIDADE = etidados.ORD_QUANTIDADE;
                        dados_etiqueta.CLI_NOME = etidados.CLI_NOME;
                        dados_etiqueta.CLI_ID = etidados.CLI_ID;
                        dados_etiqueta.PRO_ID = etidados.PRO_ID;
                        dados_etiqueta.PRO_DESCRICAO = etidados.PRO_DESCRICAO;
                        dados_etiqueta.OF = $"{etidados.ORD_ID}.{etidados.FPR_SEQ_REPETICAO}";
                        dados_etiqueta.CLI_ENDERECO_ENTREGA = $"{etidados.CLI_ENDERECO_ENTREGA} ,{etidados.CLI_BAIRRO_ENTREGA} . { etidados.MUN_NOME }. CEP: {etidados.CLI_CEP_ENTREGA}";
                        dados_etiqueta.ORD_DATA_ENTREGA_ATE = etidados.ORD_DATA_ENTREGA_ATE.ToShortDateString();
                        dados_etiqueta.ETI_QUANTIDADE_PALETE = $"{etidados.ETI_QUANTIDADE_PALETE}";
                        dados_etiqueta.MAQ_ID = etidados.MAQ_ID;
                        dados_etiqueta.QTD_AMARRADOS = etidados.QTD_AMARRADOS;
                        dados_etiqueta.ETI_DATA_FABRICACAO = etidados.ETI_DATA_FABRICACAO.ToShortDateString();
                        dados_etiqueta.ETI_CODIGO_BARRAS = QRCodeGen.BitmapToBytes(QRCodeGen.GerarBarCode(etidados.ETI_CODIGO_BARRAS));
                        dados_etiqueta.ETI_QR_CODE = QRCodeGen.BitmapToBytes(QRCodeGen.GerarQRCode(etidados.ETI_CODIGO_BARRAS));
                        dados_etiqueta.ETI_LOTE = etidados.ETI_LOTE;
                        dados_etiqueta.ETI_SUB_LOTE = etidados.ETI_SUB_LOTE;

                        if (etidados.GRP_TIPO != 2) //só mostra os 4 primeiros dígitos do PRO_ID se não for caixa
                            dados_etiqueta.TIPO_PRO_ID = etidados.PRO_ID.Substring(0, 4);

                        //instancia uma nova string segundo a estrutura base para então dar replace nos campos pedidos
                        List<string> html_etiquetas = MontarEtiqueta(estruturas_etiqueta, etidados.CLI_ID, dados_etiqueta);

                        listaEti.AddRange(html_etiquetas);
                    }
                }
                #endregion
                ViewData["ListaEtiquetas"] = listaEti;
                return new ViewAsPdf(ViewData);
            }
        }
        public IActionResult Etiquetas(string listaEtiquetas)
        {
            //Controle Acesso
            if (!ValidacoesUsuario.ValidarAcessoTela(ObterUsuarioLogado(), typeof(ReportEtiquetaController).FullName))
                return RedirectToAction("SemAcesso", "Acesso", new { area = "" });

            using (JSgi db = new ContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                #region Dados da etiqueta
                var ids = listaEtiquetas.Split(",");
                var etiquetas = db.Etiqueta.AsNoTracking().Where(x => ids.Contains(x.ETI_ID.ToString())).ToList();
                Etiqueta etiquetaAtual = etiquetas.First();
                var infoPedido = (from fila in db.FilaProducao
                                  join pedido in db.Order on fila.ORD_ID equals pedido.ORD_ID
                                  join produto in db.Produto on pedido.PRO_ID equals produto.PRO_ID
                                  //join grupoProduto in db.GrupoProduto on produto.GRP_ID equals grupoProduto.GRP_ID
                                  join cliente in db.Cliente on pedido.CLI_ID equals cliente.CLI_ID
                                  join cidade in db.Municipio on cliente.MUN_ID equals cidade.MUN_ID
                                  where fila.ORD_ID == etiquetaAtual.ORD_ID &&
                                                         fila.ROT_PRO_ID == etiquetaAtual.ROT_PRO_ID &&
                                                         fila.FPR_SEQ_REPETICAO == etiquetaAtual.FPR_SEQ_REPETICAO
                                  select new
                                  {
                                      fila.FPR_ID,
                                      fila.ORD_ID,
                                      cliente.CLI_NOME,
                                      cliente.CLI_ID,
                                      cliente.CLI_ENDERECO_ENTREGA,
                                      cliente.CLI_BAIRRO_ENTREGA,
                                      cliente.Municipio.MUN_NOME,
                                      cliente.CLI_CEP_ENTREGA,
                                      pedido.PRO_ID,
                                      pedido.ORD_QUANTIDADE,
                                      produto.QTD_PALETE,
                                      pedido.ORD_DATA_ENTREGA_ATE,  
                                      produto.PRO_DESCRICAO,
                                      produto.PRO_PECAS_POR_FARDO,
                                      produto.GRP_TIPO,
                                      produto.GRP_DESCRICAO,
                                      produto.GRP_ID,
                                      produto.GRP_DESCRICAO_C,
                                      produto.GRP_ID_C
                                  }).AsNoTracking()
                                    .FirstOrDefault();

                List<string> listaEti = new List<string>();
                List<EstruturaEtiqueta> estruturas_etiqueta = db.EstruturaEtiqueta.AsNoTracking().ToList();
                CustonOpt CustonOpt = new CustonOpt();

                for (int j = 0; j < etiquetaAtual.ETI_NUMERO_COPIAS; j++)
                {
                    for (int i = 0; i < etiquetas.Count; i++)
                    {
                        etiquetaAtual = etiquetas[i];

                        //Consultando dados para montagem da etiqueta 
                        if (infoPedido != null)
                        {                         
                            dynamic dados_pedido = new ExpandoObject();                           
                            dados_pedido.Cliente = infoPedido.CLI_NOME;
                            dados_pedido.Referencia = infoPedido.PRO_ID +" - "+infoPedido.PRO_DESCRICAO;
                            dados_pedido.Of = $"{infoPedido.ORD_ID}.{etiquetaAtual.FPR_SEQ_REPETICAO}";
                            dados_pedido.QtdPalete = etiquetaAtual.ETI_QUANTIDADE_PALETE;
                            dados_pedido.Composicao = infoPedido.GRP_ID_C+" - "+infoPedido.GRP_DESCRICAO_C; 
                            dados_pedido.DataEntrada = "-";
                            dados_pedido.Mesa = etiquetaAtual.ETI_SUB_LOTE;
                            dados_pedido.DataFabricacao = etiquetaAtual.ETI_DATA_FABRICACAO.ToShortDateString(); ;
                            dados_pedido.QtdPedido = infoPedido.ORD_QUANTIDADE;
                            dados_pedido.QtdPorAmarrados = infoPedido.PRO_PECAS_POR_FARDO;
                            
                            if (etiquetaAtual.ETI_QUANTIDADE_PALETE > 0)
                            {
                                dados_pedido.PecasPorFardo = infoPedido.PRO_PECAS_POR_FARDO;
                                dados_pedido.QtdUltimoAmarrado = (etiquetaAtual.ETI_QUANTIDADE_PALETE % infoPedido.PRO_PECAS_POR_FARDO) > 0 ? (etiquetaAtual.ETI_QUANTIDADE_PALETE % infoPedido.PRO_PECAS_POR_FARDO) : infoPedido.PRO_PECAS_POR_FARDO;//Math.Round(((double)etiquetaAtual.ETI_QUANTIDADE_PALETE % (double)infoPedido.PRO_PECAS_POR_FARDO) * (double) infoPedido.PRO_PECAS_POR_FARDO, 0);
                            }
                            else
                            {
                                dados_pedido.QtdUltimoAmarrado = 0;
                                dados_pedido.QtdAmarrados = 0;
                            }
                            dados_pedido.Maquina = etiquetaAtual.MAQ_ID;
                            dados_pedido.Turma = CustonOpt.Custon_getTuma(etiquetaAtual.ETI_EMISSAO.DayOfWeek, new TimeSpan(etiquetaAtual.ETI_EMISSAO.Hour,0,0));
                            dados_pedido.Entrega = $"{infoPedido.CLI_ENDERECO_ENTREGA} ,{infoPedido.CLI_BAIRRO_ENTREGA} . { infoPedido.MUN_NOME }. CEP: {infoPedido.CLI_CEP_ENTREGA}";
                            dados_pedido.StrCodigoBarras =  etiquetaAtual.ETI_CODIGO_BARRAS;
                            dados_pedido.CodigoBarras = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(QRCodeGen.BitmapToBytes(QRCodeGen.GerarBarCode(etiquetaAtual.ETI_CODIGO_BARRAS))));
                            dados_pedido.QRCode = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(QRCodeGen.BitmapToBytes(QRCodeGen.GerarQRCode(etiquetaAtual.ETI_CODIGO_BARRAS))));

                            dados_pedido.GrupoProduto = infoPedido.GRP_ID;
                          
                            //if (infoPedido.GRP_TIPO != 2) //só mostra os primeiros digitos do ORD_ID se não for uma caixa
                            dados_pedido.Pedido = infoPedido.ORD_ID;

                            //instancia uma nova string segundo a estrutura base para então dar replace nos campos pedidos
                            List<string> html_etiquetas = MontarEtiqueta(estruturas_etiqueta, infoPedido.CLI_ID, dados_pedido);

                            listaEti.AddRange(html_etiquetas);
                           
                        }
                    }
                }

                #endregion

                ViewData["ListaEtiquetas"] = listaEti;

                return new ViewAsPdf(ViewData);
            }
        }

        public List<Dadosetiqueta> Consultaetiqueta(JSgi db, string listaEitquetas)
        {
            db.Database.OpenConnection();
            using (DbCommand command = db.Database.GetDbConnection().CreateCommand())
            {
                #region Consulta SQL
                StringBuilder sql = new StringBuilder();
                sql.Append($"SELECT ISNULL(O.ORD_ID, '') AS ORD_ID, " +
                           $"ISNULL(O.ORD_QUANTIDADE, '') AS ORD_QUANTIDADE, " +
                           $"ISNULL(C.CLI_NOME, '') AS CLI_NOME, " +
                           "C.CLI_ID AS CLI_ID, " +
                           $"P.PRO_ID, " +
                           $"P.PRO_DESCRICAO, " +
                           $"ISNULL(C.CLI_ENDERECO_ENTREGA, '') AS CLI_ENDERECO_ENTREGA ," +
                           $"ISNULL(C.CLI_BAIRRO_ENTREGA, '') AS CLI_BAIRRO_ENTREGA," +
                           $"ISNULL(C.CLI_CEP_ENTREGA, '') AS CLI_CEP_ENTREGA, " +
                           $"ISNULL(MU.MUN_NOME, '') AS MUN_NOME, " +
                           $"O.ORD_DATA_ENTREGA_ATE AS ORD_DATA_ENTREGA_ATE, " +
                           $"E.ETI_QUANTIDADE_PALETE, " +
                           $"E.MAQ_ID, " +
                           $"E.ETI_DATA_FABRICACAO, " +
                           $"E.ETI_CODIGO_BARRAS, " +
                           $"E.ETI_LOTE, E.ETI_SUB_LOTE, " +
                           $"G.GRP_TIPO, " +
                           $"E.ETI_NUMERO_COPIAS, " +
                           $"CASE WHEN O.ORD_ID IS NULL " +
                           $"   THEN 0 " +
                           $"   ELSE O.ORD_QUANTIDADE / P.PRO_PECAS_POR_FARDO " +
                           $"END AS QTD_AMARRADOS," +
                           $"M.FPR_SEQ_REPETICAO," +
                           $"O.ORD_DATA_ENTREGA_ATE " +
                           $"FROM T_ETIQUETAS AS E " +
                           $"LEFT JOIN T_MOVIMENTOS_ESTOQUE AS M ON M.MOV_LOTE = E.ETI_LOTE AND M.MOV_SUB_LOTE = E.ETI_SUB_LOTE AND " +
                           $"   M.PRO_ID = E.ROT_PRO_ID AND M.TIP_ID = '998' " +
                           $"LEFT JOIN T_ORDENS AS O ON O.ORD_ID = E.ORD_ID " +
                           $"INNER JOIN T_PRODUTOS AS PO ON PO.PRO_ID = O.PRO_ID " +
                           $"INNER JOIN T_GRUPO_PRODUTO AS G ON G.GRP_ID = PO.GRP_ID " +
                           $"INNER JOIN T_PRODUTOS AS P ON P.PRO_ID = E.ROT_PRO_ID " +
                           $"LEFT JOIN T_CLIENTES AS C ON C.CLI_ID = O.CLI_ID " +
                           $"LEFT JOIN T_MUNICIPIOS AS MU ON MU.MUN_ID = C.MUN_ID_ENTREGA " +
                           $"WHERE ETI_ID IN({listaEitquetas})");

                #endregion
                try
                {
                    var etiquetas = new List<Dadosetiqueta>();

                    command.CommandText = sql.ToString();
                    command.CommandType = CommandType.Text;

                    using (DbDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var ordId = result["ORD_ID"].ToString();
                            var quantidade = Convert.ToDouble(result["ORD_QUANTIDADE"]);
                            var cliNome = result["CLI_NOME"].ToString();
                            var cliId = result["CLI_ID"].ToString();
                            var proId = result["PRO_ID"].ToString();
                            var proDescricao = result["PRO_DESCRICAO"].ToString();
                            var enderecoEntrega = result["CLI_ENDERECO_ENTREGA"].ToString();
                            var bairroEntrega = result["CLI_BAIRRO_ENTREGA"].ToString();
                            var cepEntrega = result["CLI_CEP_ENTREGA"].ToString();
                            var munNome = result["MUN_NOME"].ToString();
                            var qtdPalete = Convert.ToDouble(result["ETI_QUANTIDADE_PALETE"]);
                            var maqId = result["MAQ_ID"].ToString();
                            var dataFabricacao = Convert.ToDateTime(result["ETI_DATA_FABRICACAO"]);
                            var codBarras = result["ETI_CODIGO_BARRAS"].ToString();
                            var lote = result["ETI_LOTE"].ToString();
                            var sublote = result["ETI_SUB_LOTE"].ToString();
                            var numCopias = Convert.ToInt32(result["ETI_NUMERO_COPIAS"]);
                            var qtdAmarrados = Convert.ToDouble(result["QTD_AMARRADOS"]);
                            var seqRepeticao = Convert.ToInt32(result["FPR_SEQ_REPETICAO"]);
                            var dataEntregaAte = Convert.ToDateTime(result["ORD_DATA_ENTREGA_ATE"]);
                            var grp_tipo = Convert.ToInt32(result["GRP_TIPO"]);

                            etiquetas.Add(
                                new Dadosetiqueta(
                                    ordId, quantidade, cliNome, cliId,
                                    proId, proDescricao, enderecoEntrega,
                                    bairroEntrega, cepEntrega, munNome,
                                    qtdPalete, maqId, dataFabricacao,
                                    codBarras, lote, sublote,
                                    numCopias, qtdAmarrados, seqRepeticao,
                                    dataEntregaAte, grp_tipo
                                )
                            );
                        }
                        db.Database.CloseConnection();
                        return etiquetas;
                    }
                    
                }
                catch (Exception ex)
                {
                    return new List<Dadosetiqueta>() { new Dadosetiqueta($"Erro ao executar a consulta: {sql} - {ex.Message}","ERRO" ) };
                }

            }
        }

        /// <summary>
        /// Monta o html da etiqueta pesquisando a sua estrutura base (de acordo com o cliente especificado) e em seguida utilizando .Replace para atribuir os valores
        /// O campo que deve assumir um valor na etiqueta deve estar no formato: @NOME_CAMPO
        /// </summary>
        /// <param name="cli_nome">Nome do cliente que fez o pedido</param>
        /// <param name="propriedades">Um objeto contendo todas as colunas que a etiqueta pode solicitar e também os valores de acordo com a etiqueta atual</param>
        /// <param name="db">Conexão com o banco de dados</param>
        /// <returns>Retorna uma lista ded html da etiqueta com os novos valores dos campos marcados como @NOME_CAMPO</returns>
        private List<string> MontarEtiqueta(List<EstruturaEtiqueta> estruturas_etiquetas, string cli_id, dynamic propriedades)
        {
            IDictionary<string, object> propriedades_para_substituir = (IDictionary<string, object>)propriedades;
            List<string> etiquetas_com_dados = new List<string>();

            //pesquisa no banco de dados a base da etiqueta caso seja um registro diferente do último pesquisado
            if(!controle_estrutura_etiqueta.Any(x => x.CLI_ID == cli_id))
            {
                controle_estrutura_etiqueta = estruturas_etiquetas.Where(x => x.CLI_ID == cli_id).ToList();
            }

            //se o cliente não tiver nenhuma etiqueta específica, seleciona a etiqueta padrão
            if(controle_estrutura_etiqueta.Count == 0)
            {
                EstruturaEtiqueta estrutura_padrao = estruturas_etiquetas.Where(x => x.EST_DESCRICAO == "ESTRUTURA DE ETIQUETA PADRÃO").FirstOrDefault();
                if(estrutura_padrao != null) controle_estrutura_etiqueta.Add(estrutura_padrao);
            }

            //da um foreach nos campos do objeto, e procura o campo na string para substituir pelo valor
            foreach(EstruturaEtiqueta etiqueta in controle_estrutura_etiqueta)
            {
                string etiqueta_com_dados = etiqueta.HTML_ESTRUTURA;
                foreach (dynamic column in propriedades_para_substituir.Keys)
                {
                    var valor = propriedades_para_substituir[column];
                    if(column == "ETI_CODIGO_BARRAS" || column == "ETI_QR_CODE") //se for um código, é preciso converter
                    {
                        valor = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(valor));
                    }

                    etiqueta_com_dados = etiqueta_com_dados.Replace("@" + column, valor.ToString());
                }

                etiquetas_com_dados.Add(etiqueta_com_dados);
            }

            return etiquetas_com_dados;
        }
    
    }
    public class Dadosetiqueta
    {
        public Dadosetiqueta(string playAction, string playMsgErroValidacao)
        {
            PlayAction = playAction;
            PlayMsgErroValidacao = playMsgErroValidacao;
        }

        public Dadosetiqueta(string oRD_ID,double oRD_QUANTIDADE, string cLI_NOME, string cLI_ID, string pRO_ID, string pRO_DESCRICAO, 
            string cLI_ENDERECO_ENTREGA, string cLI_BAIRRO_ENTREGA, string cLI_CEP_ENTREGA, string mUN_NOME, double eTI_QUANTIDADE_PALETE, 
            string mAQ_ID, DateTime eTI_DATA_FABRICACAO, string eTI_CODIGO_BARRAS, string eTI_LOTE, string eTI_SUB_LOTE, 
            int eTI_NUMERO_COPIAS, double qTD_AMARRADOS, int fPR_SEQ_REPETICAO, DateTime oRD_DATA_ENTREGA_ATE, int grp_tipo)
        {
            ORD_ID = oRD_ID;
            ORD_QUANTIDADE = oRD_QUANTIDADE;
            CLI_NOME = cLI_NOME;
            CLI_ID = cLI_ID;
            PRO_ID = pRO_ID;
            PRO_DESCRICAO = pRO_DESCRICAO;
            CLI_ENDERECO_ENTREGA = cLI_ENDERECO_ENTREGA;
            CLI_BAIRRO_ENTREGA = cLI_BAIRRO_ENTREGA;
            CLI_CEP_ENTREGA = cLI_CEP_ENTREGA;
            MUN_NOME = mUN_NOME;
            ETI_QUANTIDADE_PALETE = eTI_QUANTIDADE_PALETE;
            MAQ_ID = mAQ_ID;
            ETI_DATA_FABRICACAO = eTI_DATA_FABRICACAO;
            ETI_CODIGO_BARRAS = eTI_CODIGO_BARRAS;
            ETI_LOTE = eTI_LOTE;
            ETI_SUB_LOTE = eTI_SUB_LOTE;
            ETI_NUMERO_COPIAS = eTI_NUMERO_COPIAS;
            QTD_AMARRADOS = qTD_AMARRADOS;
            FPR_SEQ_REPETICAO = fPR_SEQ_REPETICAO;
            ORD_DATA_ENTREGA_ATE = oRD_DATA_ENTREGA_ATE;
            GRP_TIPO = grp_tipo;
        }

        // Pedido
        public string ORD_ID { get; private set; }
        public double ORD_QUANTIDADE { get; private set; }
        public DateTime ORD_DATA_ENTREGA_ATE { get; private set; }

        // Produdo
        public string PRO_ID { get; private set; }
        public string PRO_DESCRICAO { get; private set; }
        public double QTD_AMARRADOS { get; private set; }
        public int GRP_TIPO { get; private set; }

        // Movimento Estoque
        public int FPR_SEQ_REPETICAO { get; private set; }

        // Etiqueta
        public DateTime ETI_DATA_FABRICACAO { get; private set; }
        public double ETI_QUANTIDADE_PALETE { get; private set; }
        public string MAQ_ID { get; private set; }
        public string ETI_CODIGO_BARRAS { get; private set; }
        public string ETI_LOTE { get; private set; }
        public string ETI_SUB_LOTE { get; private set; }
        public int ETI_NUMERO_COPIAS { get; private set; }

        // Cliente
        public string CLI_ID { get; private set; }
        public string CLI_NOME { get; private set; }
        public string CLI_ENDERECO_ENTREGA { get; private set; }
        public string CLI_BAIRRO_ENTREGA { get; private set; }
        public string CLI_CEP_ENTREGA { get; private set; }

        // Municipio
        public string MUN_NOME { get; private set; }

        // Play Sistemas
        public string PlayAction { get; set; }
        public string PlayMsgErroValidacao { get; set; }
        public int? IndexClone { get; set; }
    }
}
