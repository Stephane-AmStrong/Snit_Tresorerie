using Contracts;
//using DinkToPdf;
//using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DinkToPdfRepository /*: IDinkToPdfRepository*/
    {
        //private readonly IConverter _converter;
        /*
        public DinkToPdfRepository(IConverter converter)
        {
            _converter = converter;
        }

        public async Task<byte[]> GeneratePdfAsync(string url, IDinkToPdfRepository.Operation operation = IDinkToPdfRepository.Operation.GenerateFile)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A2,
                //Margins = new MarginSettings { Top = 010, Bottom = 010, Left = 010, Right = 010 },
                Margins = new MarginSettings { Top = 00, Bottom = 00, Left = 00, Right = 00 },
                DocumentTitle = "PDF_"+DateTime.Now,
            };

            var objectSettings = new ObjectSettings
            {
                Page = url,
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            return await Task.Run(() => _converter.Convert(pdf));
        }
        */
    }
}
