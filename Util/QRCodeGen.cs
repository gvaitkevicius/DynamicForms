using NetBarcode;
using QRCoder;
using System.Drawing;
using System.IO;

namespace DynamicForms.Util
{
    public class QRCodeGen
    {/// <summary>
     /// Retorna um BMP com o QRcode a partir de uma string 
     /// </summary>
     /// <param name="contudo"></param>
     /// <returns></returns>
        public static Bitmap GerarQRCode(string contudo)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(contudo, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(8, Color.Black, Color.White, null, 5, 6, true);
            //                                    (pixelsPerModule,darkColor,lightColor, icon , iconSizePercent ,  iconBorderWidth, drawQuietZones);
           
            return qrCodeImage;
        }
        /// <summary>
        /// Gera o bitmap de um código de barras dado o conteudo de uma string
        /// </summary>
        /// <param name="conteudo">Conteudo a ser representado pelo codigo de barras</param>
        /// <returns></returns>
        public static Bitmap GerarBarCode(string conteudo)
        {
            //var barcode = new Barcode(conteudo, NetBarcode.Type.Code128, true);
            var barcode = new Barcode(conteudo, 1000, 70);
            var value = barcode.GetByteArray();
            Bitmap bmp;
            using (var ms = new MemoryStream(value))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        /// <summary>
        /// COnverte um Bitmap em um array da bytes
        /// </summary>
        /// <param name="img">Bitmap a ser convertido</param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


    }
}

