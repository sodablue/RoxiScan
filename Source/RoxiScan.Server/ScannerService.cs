using RoxiScan.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoxiScan.Server
{
    public class ScannerService : IScanner
    {
        public IEnumerable<Scanner> GetScannerInfo()
        {
            try
            {
                var svc = new RoxiScan.ScanInfoService();
                return svc.GetScanners();
            }
            catch (Exception ex)
            {
                throw ScanError.Fault("Error retrieving list of scanners.", ex);
            }
        }

        public System.IO.Stream Scan(ScanRequest request)
        {
            try
            {
                ScanToPDF svc = new ScanToPDF();
                var pdfDocStream = svc.Scan(request.DeviceId, request.Settings);
                pdfDocStream.Seek(0, SeekOrigin.Begin);
                return pdfDocStream;
            }
            catch (Exception ex)
            {
                throw ScanError.Fault("Error during scan.", ex);
            }
        }
    }
}
