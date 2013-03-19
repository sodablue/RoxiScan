using PdfSharp.Drawing;
using PdfSharp.Pdf;
using RoxiScan.Models;
using System.Drawing;
using System.IO;

namespace RoxiScan
{
    public class ScanToPDF
    {
        private PdfDocument doc;
        private ScannerSettings settings;

        public ScanToPDF()
        {
            this.doc = new PdfDocument();
        }

        public Stream Scan(string deviceId, ScannerSettings settings)
        {
            this.settings = settings;

            ScanInfoService scanInfoSvc = new ScanInfoService();
            var scanner = scanInfoSvc.GetDeviceInfo(deviceId);

            ScanManager manager = new ScanManager(scanner, this.settings);

            manager.BeginScan(HandleImage, HandleComplete);

            if (this.doc.PageCount > 0)
            {
                var response = new MemoryStream();
                this.doc.Save(response, false);
                return response;
            }
            else
            {
                throw new ScanException("Nothing was scanned.");
            }
        }

        private void HandleImage(Image image)
        {
            using (XImage ximage = XImage.FromGdiPlusImage(image))
            {
                PdfPage page = this.doc.AddPage();
                page.Width = XUnit.FromInch(this.settings.PageSize.Width);
                page.Height = XUnit.FromInch(this.settings.PageSize.Height);
                using (XGraphics xgraphics = XGraphics.FromPdfPage(page))
                {
                    xgraphics.DrawImage(ximage, 0, 0);
                }
            }
        }

        private void HandleComplete()
        {

        }
    }
}
