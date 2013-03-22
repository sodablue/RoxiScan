using PdfSharp.Drawing;
using PdfSharp.Pdf;
using RoxiScan.Models;
using System;
using System.Drawing;
using System.IO;

namespace RoxiScan
{
    public class ScanToPDFService
    {
        private string deviceId;
        private ScannerSettings settings;

        public ScanToPDFService(string deviceId, ScannerSettings settings)
        {
            this.deviceId = deviceId;
            this.settings = settings;
        }

        public Stream CreatePDF()
        {
            using (PdfDocument doc = new PdfDocument())
            {
                ScanImages((image) =>
                {
                    using (XImage ximage = XImage.FromGdiPlusImage(image))
                    {
                        PdfPage page = doc.AddPage();
                        page.Width = XUnit.FromInch(this.settings.PageSize.Width);
                        page.Height = XUnit.FromInch(this.settings.PageSize.Height);
                        using (XGraphics xgraphics = XGraphics.FromPdfPage(page))
                        {
                            xgraphics.DrawImage(ximage, 0, 0);
                        }
                    }
                });

                if (doc.PageCount > 0)
                {
                    var response = new MemoryStream();
                    doc.Save(response, false);
                    return response;
                }
                else
                {
                    throw new ScanException("Nothing was scanned.");
                }
            }
        }

        private void ScanImages(Action<Image> handleImage)
        {
            bool hasMorePages = true;

            while (hasMorePages)
            {
                try
                {
                    using (var device = new ScanDevice(this.deviceId))
                    {
                        device.SetColorDepth(settings.ColorDepth);
                        device.SetPageSize(settings.PageSize, settings.Orientation, settings.Resolution);
                        if (settings.UseAutomaticDocumentFeeder)
                            device.UseAutomaticDocumentFeeder();
                        else
                            device.UseFlatbed();

                        device.Scan(handleImage);

                        hasMorePages = device.HasMorePages();
                    }
                }
                catch (PaperEmptyScanException)
                {
                    hasMorePages = false;
                }
            }
        }
    }
}
