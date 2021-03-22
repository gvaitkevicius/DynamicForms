using DynamicForms.Areas.PlugAndPlay.Models;
using DynamicForms.Context;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace DynamicForms.Util
{
    public class PrintUtil
    {
        private static Font fonte;
        private static Font FonteCorpo;
        public static bool StatusPrint { get; set; }
        public static Etiqueta Dados { get; set; }
        public static Produto Produto { get; set; }
        public static Order Pedido { get; set; }
        public static FilaProducao OProducao { get; set; }
        public static Cliente Cliente { get; set; }
        public static List<string> ListarImpressoras()
        {
            List<string> lista = new List<string>();
            // Lista os  nomes das impressoras 
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                PrinterSettings printer = new PrinterSettings
                {
                    PrinterName = printerName
                };
                // Verifica se a impressora é válida e adiciona a lista
                if (printer.IsValid)
                {
                    lista.Add(printerName);
                }

            }
            return lista;
        }
        public static PrintDocument CriarEtiqueta(Etiqueta etiqueta, string printer)
        {
            Dados = etiqueta;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                OProducao = db.FilaProducao.Where(f => f.ORD_ID == Dados.ORD_ID && f.ROT_PRO_ID == Dados.ROT_PRO_ID && f.FPR_SEQ_REPETICAO == Dados.FPR_SEQ_REPETICAO).FirstOrDefault();
                Pedido = db.Order.Where(o => o.ORD_ID == OProducao.ORD_ID).FirstOrDefault();
                Produto = db.Produto.Where(p => p.PRO_ID == Pedido.PRO_ID).FirstOrDefault();
                Cliente = db.Cliente.Where(c => c.CLI_ID == Pedido.CLI_ID).FirstOrDefault();
            }
            PrintDocument documento = new PrintDocument();
            documento.BeginPrint += documento_BeginPrint;
            PrinterSettings ps = new PrinterSettings
            {
                Collate = false,
                PrinterName = printer
            };
            documento.EndPrint += documento_EndPrint;
            documento.PrintPage += documento_PrintPage;
            return documento;
        }
        static void documento_BeginPrint(object sender, PrintEventArgs e)
        {
            fonte = new Font("Arial", 14, FontStyle.Bold);
            FonteCorpo = new Font("Arial", 12, FontStyle.Regular);
        }

        static void documento_EndPrint(object sender, PrintEventArgs e)
        {
            StatusPrint = (e.Cancel) ? false : true;
            fonte.Dispose();
        }
        private static void documento_PrintPage(object sender, PrintPageEventArgs e)
        {

            string titulo = "PLAY SISTEMAS INTELIGENTES";
            SizeF tamanho = e.Graphics.MeasureString(titulo, fonte);
            float posicao = (e.PageBounds.Width / 2) - (tamanho.Width / 2);

            //Crinado BMP da Moldura da etiqueta
            Image frameEtiqueta = new Bitmap(@"C:\JoaoDynamic\DynamicForm\DynamicForms\Util\ETIQUETA_MODELO.png");
            //Gerando o QRCode
            Image newImage = QRCodeGen.GerarQRCode(Dados.ETI_CODIGO_BARRAS);
            //Criando coordenadas para o QRCode
            int x = 15;
            int y = 576;
            //Desenhando a Moldura da etiqueta
            e.Graphics.DrawImageUnscaled(frameEtiqueta, 0, 0);
            //-----------------------------------------------------------------------------------------------
            //Dados da etiqueta
            e.Graphics.DrawString(titulo, fonte, new SolidBrush(Color.Black), posicao, 0.0f);
            //-----------------------------------------------------------------------------------------------
            e.Graphics.DrawString(Cliente.CLI_NOME, FonteCorpo, new SolidBrush(Color.Black), 15, 114);
            //-----------------------------------------------------------------------------------------------
            //e.Graphics.DrawString(Dados.Of+"", FonteCorpo, new SolidBrush(Color.Black), 15, 265);
            e.Graphics.DrawString((Pedido.Produto.PRO_FARDOS_POR_CAMADA * Pedido.Produto.PRO_PECAS_POR_FARDO) + "", FonteCorpo, new SolidBrush(Color.Black), 207, 265);
            //e.Graphics.DrawString(Dados.DataEntrega.ToString(), FonteCorpo, new SolidBrush(Color.Black), 400, 265);
            //-----------------------------------------------------------------------------------------------
            e.Graphics.DrawString(Pedido.ORD_ID, FonteCorpo, new SolidBrush(Color.Black), 15, 342);
            // e.Graphics.DrawString(Dados.NumAmarrado + "", FonteCorpo, new SolidBrush(Color.Black), 207, 342);
            e.Graphics.DrawString(OProducao.ROT_MAQ_ID, FonteCorpo, new SolidBrush(Color.Black), 400, 342);
            //e.Graphics.DrawString(Dados.Turma, FonteCorpo, new SolidBrush(Color.Black), 595, 342);
            //-----------------------------------------------------------------------------------------------
            e.Graphics.DrawString(OProducao.FPR_QTD_PRODUZIDA + "", FonteCorpo, new SolidBrush(Color.Black), 15, 421);

            //e.Graphics.DrawString(Dados.QtdAmarrado + "", FonteCorpo, new SolidBrush(Color.Black), 207, 421);
            //e.Graphics.DrawString(Dados.Mesa, FonteCorpo, new SolidBrush(Color.Black), 400, 421);
            e.Graphics.DrawString(Dados.ETI_DATA_FABRICACAO.ToShortDateString(), FonteCorpo, new SolidBrush(Color.Black), 585, 421);
            //-----------------------------------------------------------------------------------------------
            e.Graphics.DrawString(Cliente.CLI_ENDERECO_ENTREGA, FonteCorpo, new SolidBrush(Color.Black), 15, 505);
            //e.Graphics.DrawString(Dados.ProxMaquina, FonteCorpo, new SolidBrush(Color.Black), 595, 505);
            //-----------------------------------------------------------------------------------------------
            e.Graphics.DrawString(Cliente.CLI_NOME, FonteCorpo, new SolidBrush(Color.Black), 15, 740);

            e.Graphics.DrawString(Pedido.ORD_ID, FonteCorpo, new SolidBrush(Color.Black), 595, 740);
            //e.Graphics.DrawString(Dados.Of+"", FonteCorpo, new SolidBrush(Color.Black), 700, 750);
            //-----------------------------------------------------------------------------------------------
            //e.Graphics.DrawString(Dados.NumAmarrado + "", FonteCorpo, new SolidBrush(Color.Black), 15, 815);
            //e.Graphics.DrawString(Dados.QtdAmarrado + "", FonteCorpo, new SolidBrush(Color.Black), 207, 815);
            //e.Graphics.DrawString(Dados.QtdUltAmarrado + "", FonteCorpo, new SolidBrush(Color.Black), 421, 815);
            e.Graphics.DrawString((Pedido.Produto.PRO_FARDOS_POR_CAMADA * Pedido.Produto.PRO_PECAS_POR_FARDO) + "", FonteCorpo, new SolidBrush(Color.Black), 595, 815);
            // QRCode.
            e.Graphics.DrawImageUnscaled(newImage, x, y);
        }
        public PrintUtil()
        {
            Dados = new Etiqueta();
        }
        public PrintUtil(Etiqueta etiqueta)
        {
            Dados = etiqueta;
            using (var db = new ContextFactory().CreateDbContext(new string[] { }))
            {
                OProducao = db.FilaProducao.Where(f => f.ORD_ID == Dados.ORD_ID && f.ROT_PRO_ID == Dados.ROT_PRO_ID && f.FPR_SEQ_REPETICAO == Dados.FPR_SEQ_REPETICAO).FirstOrDefault();
                Pedido = db.Order.Where(o => o.ORD_ID == OProducao.ORD_ID).FirstOrDefault();
                Produto = db.Produto.Where(p => p.PRO_ID == Pedido.PRO_ID).FirstOrDefault();
                Cliente = db.Cliente.Where(c => c.CLI_ID == Pedido.CLI_ID).FirstOrDefault();
            }
        }

        /*
        public void printReport(string printer)
        {
            PrintDocument _pd = new PrintDocument();
            _pd.DefaultPageSettings.Landscape = true;
            _pd.PrinterSettings.PrintFileName = "C:\\TeamServicesRepositorios\\SGI\\Plug_Play_Report\\EtiquetaReport.rdl";
            _pd.PrinterSettings.PrinterName = printer;
            _pd.Print();
        }
        */

    }
}
