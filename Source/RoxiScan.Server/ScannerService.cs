using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WIA;
using PdfSharp.Pdf;
using System.IO;
using RoxiScan.Model;

namespace RoxiScan.Server
{
    public class ScannerService : IScanner
    {
        public GetScannerInfoResponse GetScannerInfo()
        {
            GetScannerInfoResponse response = new GetScannerInfoResponse();

            DeviceManager manager = new DeviceManager();
            foreach (DeviceInfo info in manager.DeviceInfos)
            {
                if (info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    Console.WriteLine("Scanner");
                    string deviceId = info.DeviceID;
                    string deviceName = null;
                    foreach (Property item in info.Properties)
                    {
                        if (item.Name == "Description")
                            deviceName = item.get_Value();
                    }

                    Console.WriteLine(deviceId);
                    Console.WriteLine(deviceName);
                    if (deviceName != null)
                    {
                        GetScannerInfoResponse.Scanner scanner = new GetScannerInfoResponse.Scanner();
                        scanner.DeviceId = deviceId;
                        scanner.DeviceName = deviceName;

                        response.Scanners.Add(scanner);
                    }

                }
            }

            return response;
        }

        public System.IO.Stream Scan(ScanRequest request)
        {
            ScanAdapter adapter = new ScanAdapter(request.DeviceId, request.PaperSize, request.ScanSource);
            PdfDocument pdfDoc = adapter.ScanToPDF();

            var response = new MemoryStream();
            pdfDoc.Save(response, false);

            response.Seek(0, SeekOrigin.Begin);
            return response;
        }
    }
}
