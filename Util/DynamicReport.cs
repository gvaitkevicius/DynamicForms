using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Areas.PlugAndPlay.Models.Estoque;
using DynamicForms.Context;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DynamicForms.Util
{
    public class DynamicReport
    {
        public JSgi _db { get; set; }

        public string RelatorioId { get; set; }
        public List<Relatorios> Estrutura { get; set; }
        public List<EtiquetaGenerica> Conteudo { get; set; }
        //Caminho do Arquivo 
        public string PathReportFile { get; set; }
        public string ReportFileName { get; set; }
        public string BackGround { get; set; }
        public SKDocumentPdfMetadata Metadata { get; set; }
        //Tamanho da página
        public int Width { get; set; }
        public int Height { get; set; }
        //--
        public byte[] MolduraRelatorio { get; set; }
        public DynamicReport()
        {
            this._db = new ContextFactory().CreateDbContext(new string[] { });
            this.Metadata = new SKDocumentPdfMetadata();
            this.Width = (Width == 0) ? 595 : Width;
            this.Height = (Height == 0) ? 842 : Height;

        }
        public void GerarRelatorio(string userID)
        {
            SKDocumentPdfMetadata metadata = new SKDocumentPdfMetadata
            {
                Author = "APS PLAY SISTEMAS INTELIGENTES",
                Creator = "APS PLAY SISTEMAS INTELIGENTES",
                Creation = DateTime.Now,
                Title = "APS PLAY SISTEMAS INTELIGENTES Copyright© 2017-2019  All Rights Reserved."
            };
            PathReportFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Reports\PDF\", ReportFileName);
            using (var stream = new SKFileWStream(PathReportFile))
            {
                using (var document = SKDocument.CreatePdf(stream, metadata))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.IsAntialias = true;
                        //Cor do Texto
                        paint.Color = SKColors.Black;
                        paint.TextAlign = SKTextAlign.Left;
                        //paint.IsStroke = true;
                        paint.StrokeWidth = 2;

                        //Convertendo imagem da moldura para array de bytes
                        PathReportFile = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\", @"" + BackGround);
                        MolduraRelatorio = QRCodeGen.BitmapToBytes(new Bitmap(PathReportFile));
                        Estrutura = _db.Relatorios.Where(e => e.REL_NOME_RELATORIO.Equals(RelatorioId)).ToList();
                        var label = Estrutura.Where(E => E.REL_TIPO_CAMPO.Equals("LABEL")).ToList();
                        var fields = Estrutura.Where(E => E.REL_TIPO_CAMPO.Equals("FIELD") || E.REL_TIPO_CAMPO.Equals("QR_CODE") || E.REL_TIPO_CAMPO.Equals("BAR_CODE")).ToList();
                        // Inicio da impressao do conteudo
                        for (int i = 0; i < Conteudo.Count; i++)
                        {
                            using (var pdfCanvas = document.BeginPage(Width, Height))
                            {
                                //desenhando moldura da etiqueta
                                pdfCanvas.DrawBitmap(SKBitmap.Decode(MolduraRelatorio), 0, 0, paint);
                                SKPoint point = new SKPoint();
                                //Desenhando o nome dos campos
                                foreach (var item in label)
                                {
                                    point.X = Convert.ToSingle(item.REL_POS_X);
                                    point.Y = Convert.ToSingle(item.REL_POS_Y);
                                    paint.TextSize = Convert.ToSingle(item.REL_TAMANHO_FONTE);
                                    pdfCanvas.DrawText(item.REL_NOME_CAMPO + ":", point, paint);
                                }
                                //Preenchendo valor dos campos
                                foreach (var item in fields)
                                {
                                    point.X = Convert.ToSingle(item.REL_POS_X);
                                    point.Y = Convert.ToSingle(item.REL_POS_Y);
                                    PropertyInfo campo = Conteudo.ElementAt(i).GetType().GetProperty(item.REL_NOME_CAMPO);
                                    if (campo != null)
                                    {
                                        string value = (string)campo.GetValue(Conteudo.ElementAt(i));
                                        value = (String.IsNullOrEmpty(value)) ? "" : value;
                                        paint.TextSize = Convert.ToSingle(item.REL_TAMANHO_FONTE);
                                        switch (item.REL_TIPO_CAMPO)
                                        {
                                            case "FIELD":
                                                pdfCanvas.DrawText(value, point, paint);
                                                break;
                                            case "QR_CODE":
                                                //Gerando e decodificando QR code
                                                var bitmap = SKBitmap.Decode(QRCodeGen.BitmapToBytes(QRCodeGen.GerarQRCode(value)));
                                                pdfCanvas.DrawBitmap(bitmap, point.X, point.Y, paint);
                                                break;
                                            case "BAR_CODE":
                                                var bitmapBarCode = SKBitmap.Decode(QRCodeGen.BitmapToBytes(QRCodeGen.GerarBarCode(value)));
                                                pdfCanvas.DrawBitmap(bitmapBarCode, point.X, point.Y, paint);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    };
                    // end the doc
                    document.Close();
                };
            };


        }

    }
}
