using Azure3._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Azure3._1.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Fact()]
        public IActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter with Blink rendering engine
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings();

            settings.WebKitPath = Path.Combine("QtBinariesWindows");

            htmlConverter.ConverterSettings = settings;

            PdfDocument document = htmlConverter.Convert("https://www.google.com");

            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            stream.Position = 0;

            FileStream fs = new FileStream("output.pdf", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            stream.CopyTo(fs);

            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Sample.pdf");
        }
        [Fact()]
        public static void SyncfusionLibTestDotNetSix()
        {
            using (var pdfFile = System.IO.File.OpenWrite("outputtest.pdf"))
            {


                var htmlConverter = new HtmlToPdfConverter();
                var _document = (@"<!DOCTYPE html>
                    <html>
                    <head>
                        <title>basic_model_template</title>
                    </head>
                    <body>
                    {{Titolo}}
                    Dal: {{DataInizio|d}} Al: {{DataFine|d}}
                    <p>
                        {{RagioneSociale}} P.IVA: {{PartitaIva}}
                        <table>
                            {{#MovimentiGruppo}}
                            <tr>
                                <td>{{NumeroDocumentoCompleto}}</td>
                            </tr>
                            {{/MovimentiGruppo}}
                        </table>
                    </p>
                    </body>
                    </html>");

                var settings = new WebKitConverterSettings
                {
                    WebKitPath = Path.Combine(AppContext.BaseDirectory, "QtBinariesWindows"),
                    Margin = new PdfMargins { All = 40 },
                    EnableRepeatTableHeader = true,
                    SplitTextLines = false,
                    Orientation = string.Equals(null, "landscape", StringComparison.OrdinalIgnoreCase) ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait
                };

                htmlConverter.ConverterSettings = settings;

                using var pdf = htmlConverter.Convert(_document, null);
                pdf.Save(pdfFile);
                pdf.Close(true);
            }
        }
    }
}
