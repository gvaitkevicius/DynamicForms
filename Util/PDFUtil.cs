using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DynamicForms.Util
{
    public class PDFUtil
    {
        public List<Etiqueta> Etiquetas { get; set; }
        public Order Db_Pedido { get; set; }
        public FilaProducao Db_OProducao { get; set; }
        public SaldosEmEstoquePorLote SaldosEmEstoquePorLote { get; set; }
        public int Origem { get; set; }
        public JSgi Context { get; set; }
        /// <summary>
        /// Gera PDF para uma lista de etiquetas fornecida como parâmetro
        /// origem - 0 Etiquetas originadas da producao 1-  Etiquetas originadas de outras movimentacoes
        /// </summary>
        /// <param name="etiquetas"></param>
        /// <param name="origem"></param>
        public PDFUtil(List<Etiqueta> etiquetas, int origem)
        {
            Etiquetas = etiquetas;
            Context = new ContextFactory().CreateDbContext(new string[] { });
            SaldosEmEstoquePorLote = new SaldosEmEstoquePorLote();
            Origem = origem;
        }

        public void GerarEtiquetaPDF(string path)
        {
            SKDocumentPdfMetadata metadata = new SKDocumentPdfMetadata();
            metadata.Author = "APS PLAY SISTEMAS INTELIGENTES";
            metadata.Creator = ((Etiqueta)Etiquetas.ElementAt(0)).USE_ID.ToString();
            metadata.Creation = DateTime.Now;
            metadata.Title = "APS PLAY SISTEMAS INTELIGENTES Copyright© 2017-2019  All Rights Reserved.";
            //Criando um stream de dados e associando o path do arquivo

            using (var stream = new SKFileWStream(path))
            //Criando o arquivo
            using (var document = SKDocument.CreatePdf(stream, metadata))
            using (var paint = new SKPaint())
            {
                int x = 28;
                int y = 594;

                paint.IsAntialias = true;
                //SkiaSharp.SKColor Black = SKColors.Black;
                //Tamanho da fonte
                paint.TextSize = 15.0f;
                //Cor do Texto
                paint.Color = SKColors.Black;

                paint.TextAlign = SKTextAlign.Left;
                //paint.IsStroke = true;
                paint.StrokeWidth = 2;

                //Tamanho da página
                var width = 595;
                var height = 842;
                //Convertendo imagem da moldura para array de bytes

                string _pathFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\", @"ETIQUETA_MODELO.png");
                byte[] _molduraEtiqueta = QRCodeGen.BitmapToBytes(new Bitmap(_pathFile));

                // draw page 1
                for (int i = 0; i < Etiquetas.Count; i++)
                {
                    //Recuperando Objeto do tipo Etiqueta da lista de object 
                    Etiqueta EtiquetaAtual = (Etiqueta)Etiquetas.ElementAt(i);

                    //Consultando dados para montagem da etiqueta (OP(FilaProducao),Pedido(Order),Produto e cliente
                    Db_OProducao = Context.FilaProducao.AsNoTracking().Where(f => f.ORD_ID == EtiquetaAtual.ORD_ID && f.ROT_PRO_ID == EtiquetaAtual.ROT_PRO_ID && f.FPR_SEQ_REPETICAO == EtiquetaAtual.FPR_SEQ_REPETICAO).FirstOrDefault();
                    Db_Pedido = Context.Order.AsNoTracking().Include(o => o.Produto).Include(o => o.Cliente).ThenInclude(c => c.Municipio).Where(o => o.ORD_ID == Db_OProducao.ORD_ID).FirstOrDefault();
                    //Iniciando a construçao da pagina atual
                    using (var pdfCanvas = document.BeginPage(width, height))
                    {
                        //desenhando moldura da etiqueta
                        pdfCanvas.DrawBitmap(SKBitmap.Decode(_molduraEtiqueta), 0, 0, paint);
                        SKPoint point = new SKPoint();

                        //Desenhando o nome dos campos
                        point.X = 7;
                        point.Y = 20;
                        pdfCanvas.DrawText("CLIENTE: ", point, paint);

                        point.X = 7;
                        point.Y = 117;
                        pdfCanvas.DrawText("REFERÊNCIA: ", point, paint);
                        point.X = 400;
                        point.Y = 117;
                        pdfCanvas.DrawText("PRODUTO: ", point, paint);

                        point.X = 7;
                        point.Y = 215;
                        pdfCanvas.DrawText("OF: ", point, paint);
                        point.X = 197;
                        point.Y = 215;
                        pdfCanvas.DrawText("QTD./PALETE: ", point, paint);
                        point.X = 400;
                        point.Y = 215;
                        pdfCanvas.DrawText("DATA DA ENTREGA: ", point, paint);

                        point.X = 7;
                        point.Y = 304;
                        pdfCanvas.DrawText("N. PEDIDO: ", point, paint);
                        point.X = 197;
                        point.Y = 304;
                        pdfCanvas.DrawText("N. AMARRADOS: ", point, paint);
                        point.X = 400;
                        point.Y = 304;
                        pdfCanvas.DrawText("MÁQUINA: ", point, paint);
                        point.X = 604;
                        point.Y = 304;
                        pdfCanvas.DrawText("TURMA: ", point, paint);

                        point.X = 7;
                        point.Y = 402;
                        pdfCanvas.DrawText("QTD. PEDIDOS: ", point, paint);
                        point.X = 197;
                        point.Y = 402;
                        pdfCanvas.DrawText("QTD. AMARRADOS: ", point, paint);
                        point.X = 400;
                        point.Y = 402;
                        pdfCanvas.DrawText("MESA: ", point, paint);
                        point.X = 542;
                        point.Y = 402;
                        pdfCanvas.DrawText("DATA DE FABRICAÇÃO: ", point, paint);

                        point.X = 7;
                        point.Y = 495;
                        pdfCanvas.DrawText("ENDEREÇO DE ENTREGA: ", point, paint);
                        point.X = 604;
                        point.Y = 495;
                        pdfCanvas.DrawText("PRÓXIMA MÁQUINA: ", point, paint);

                        point.X = 7;
                        point.Y = 590;
                        pdfCanvas.DrawText("CÓDIGO DE BARRAS: ", point, paint);

                        point.X = 7;
                        point.Y = 955;
                        pdfCanvas.DrawText("LOTE: ", point, paint);

                        point.X = 400;
                        point.Y = 955;
                        pdfCanvas.DrawText("SUB LOTE: ", point, paint);



                        // draw everything on the pdfCanvas
                        //Nome do cliente
                        point.X = 100;
                        point.Y = 20;
                        pdfCanvas.DrawText(Db_Pedido.Cliente.CLI_NOME, point, paint);
                        //Referência ko
                        point.X = 22;
                        point.Y = 160;
                        //pdfCanvas.DrawText(EtiquetaAtual.., point, paint);
                        //Produto
                        point.X = 435;
                        point.Y = 142;
                        pdfCanvas.DrawText(Db_Pedido.PRO_ID, point, paint);
                        point.Y = 170;
                        pdfCanvas.DrawText(Db_Pedido.Produto.PRO_DESCRICAO, point, paint);

                        //OF ko
                        point.X = 22;
                        point.Y = 260;
                        pdfCanvas.DrawText(Db_OProducao.FPR_ID + "", point, paint);

                        //Fardos por palete
                        point.X = 221;
                        point.Y = 260;
                        pdfCanvas.DrawText((Db_Pedido.Produto.PRO_FARDOS_POR_CAMADA * Db_Pedido.Produto.PRO_PECAS_POR_FARDO) + "", point, paint);

                        //Data entrega
                        point.X = 415;
                        point.Y = 260;
                        pdfCanvas.DrawText(Db_Pedido.ORD_DATA_ENTREGA_ATE + "", point, paint);

                        //Id do Pedido
                        point.X = 12;
                        point.Y = 356;
                        pdfCanvas.DrawText(Db_Pedido.ORD_ID, point, paint);

                        //N. Amarrados ko numero amarrados
                        point.X = 221;
                        point.Y = 356;
                        //pdfCanvas.DrawText(Db_Pedido., point, paint);

                        //Id da Maquina
                        point.X = 415;
                        point.Y = 356;
                        pdfCanvas.DrawText(EtiquetaAtual.MAQ_ID, point, paint);

                        //Turma ko turma
                        point.X = 615;
                        point.Y = 356;
                        //pdfCanvas.DrawText(Db_Pedido.., point, paint);

                        //quantidade Produzida
                        point.X = 12;
                        point.Y = 452;
                        double? quantidadeAux = quantidadeAux = EtiquetaAtual.ETI_QUANTIDADE_PALETE;
                        pdfCanvas.DrawText(quantidadeAux + "", point, paint);

                        //Qtd. Amarrados ko amarrados
                        point.X = 221;
                        point.Y = 452;
                        //pdfCanvas.DrawText(, point, paint);

                        //Data de fabricação
                        point.X = 549;
                        point.Y = 452;
                        pdfCanvas.DrawText(EtiquetaAtual.ETI_DATA_FABRICACAO.ToShortDateString(), point, paint);

                        //Endereço de Entrega ko municipio
                        point.X = 12;
                        point.Y = 550;
                        pdfCanvas.DrawText(Db_Pedido.Cliente.CLI_ENDERECO_ENTREGA + ", " + Db_Pedido.Cliente.CLI_BAIRRO_ENTREGA + ". " + Db_Pedido.Cliente.Municipio.MUN_NOME + ". CEP: " + Db_Pedido.Cliente.CLI_CEP_ENTREGA, point, paint);

                        //Proxima Maquina ko proxima maquina
                        point.X = 615;
                        point.Y = 550;
                        // pdfCanvas.DrawText(Db_Pedido.maq, point, paint);

                        //Lote
                        point.X = 22;
                        point.Y = 990;
                        pdfCanvas.DrawText(EtiquetaAtual.ETI_LOTE, point, paint);

                        //Sub Lote
                        point.X = 410;
                        point.Y = 990;
                        pdfCanvas.DrawText(EtiquetaAtual.ETI_SUB_LOTE, point, paint);


                        //Gerando e decodificando Codigo de barras
                        var bitmap = SKBitmap.Decode(QRCodeGen.BitmapToBytes(QRCodeGen.GerarQRCode(EtiquetaAtual.ETI_CODIGO_BARRAS)));
                        var bitmapBarCode = SKBitmap.Decode(QRCodeGen.BitmapToBytes(QRCodeGen.GerarBarCode(EtiquetaAtual.ETI_CODIGO_BARRAS)));

                        //int x = 80;
                        //int y = 594;
                        pdfCanvas.DrawBitmap(bitmap, x, y, paint);
                        pdfCanvas.DrawBitmap(bitmapBarCode, x + 280, y + 33, paint);
                        document.EndPage();
                    }
                    //Proxima página
                }
                // end the doc
                document.Close();
            }

        }
        public static void OpenPDFInBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }

}
