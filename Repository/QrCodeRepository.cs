using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class QrCodeRepository : IQrCodeRepository
    {
        public async Task<byte[]> GenerateAsync(string textToEncoded, int pixels = 60)
        {
            var qrCodeInfo = await Task.Run(() => new QRCodeGenerator().CreateQrCode(textToEncoded, QRCodeGenerator.ECCLevel.Q));
            var qrCode = new QRCode(qrCodeInfo);
            Bitmap QrBitmap = await Task.Run(() => qrCode.GetGraphic(pixels));

            //byte[] bitmapArray = QrBitmap.BitmapToByteArrayCopy();
            return QrBitmap.BitmapToByteArray();
        }
    }
}
