using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Graphics;
using System.IO;
using BlazorWebassemblyPdfExport.Models;
using BlazorWebassemblyPdfExport.Utils;

using Microsoft.JSInterop;
using Syncfusion.Pdf.Interactive;

namespace BlazorWebassemblyPdfExport.Services
{
    public class WeatherForecastExportToPdfService
    {
        IJSRuntime _js;
        public WeatherForecastExportToPdfService(IJSRuntime js)
        {
            _js = js;
        }
        public void ExportToPdf(IEnumerable<WeatherForecast> forecasts)
        {
            int paragraphAfterSpacing = 8;
            int cellMargin = 8;

            PdfDocument pdfDocument = new PdfDocument();

            //Add Page to the PDF document.
            PdfPage page = pdfDocument.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            //Creates document bookmarks.
            PdfBookmark bookmark = pdfDocument.Bookmarks.Add("1. Weather Forecast");

            //Create a new font.
            PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);

            //Create a text element to draw a text in PDF page.
            PdfTextElement title = new PdfTextElement("Weather Forecast", font, PdfBrushes.Black);
            PdfLayoutResult result = title.Draw(page, new PointF(0, 0));


            PdfStandardFont contentFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfTextElement content = new PdfTextElement("This component demonstrates fetching data from a client side and Exporting the data to PDF document using Syncfusion .NET PDF library.", contentFont, PdfBrushes.Black);
            PdfLayoutFormat format = new PdfLayoutFormat();
            format.Layout = PdfLayoutType.Paginate;

            //Draw a text to the PDF document.
            result = content.Draw(page, new RectangleF(0, result.Bounds.Bottom + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.CellPadding.Left = cellMargin;
            pdfGrid.Style.CellPadding.Right = cellMargin;

            //Applying built-in style to the PDF grid
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            //Assign data source.
            pdfGrid.DataSource = forecasts;

            pdfGrid.Style.Font = contentFont;

            //Draw PDF grid into the PDF page.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

            //watermark text.

            PdfGraphicsState state = graphics.Save();

            graphics.SetTransparency(0.5f);

            graphics.RotateTransform(-30);

            PdfStandardFont fontWatermark = new PdfStandardFont(PdfFontFamily.Courier, 40);

            graphics.DrawString("SAMPLE", fontWatermark, PdfPens.Red, PdfBrushes.Red, new PointF(100, 200));

            MemoryStream memoryStream = new MemoryStream();

            // Save the PDF document.
            pdfDocument.Save(memoryStream);

            // Download the PDF document
            _js.SaveAs("WeatherForecast.pdf", memoryStream.ToArray());

        }
    }
}
