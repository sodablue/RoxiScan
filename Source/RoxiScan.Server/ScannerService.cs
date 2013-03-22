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
            catch (ScanException se)
            {
                throw ScanError.Fault(se.Message, se);
            }
            catch (Exception e)
            {
                throw ScanError.Fault("Error retrieving list of scanners.", e);
            }
        }

        public System.IO.Stream Scan(ScanRequest request)
        {
            try
            {
                ScanToPDFService svc = new ScanToPDFService(request.DeviceId, request.Settings);
                var pdfDocStream = svc.CreatePDF();
                pdfDocStream.Seek(0, SeekOrigin.Begin);
                return pdfDocStream;
            }
            catch (ScanException se)
            {
                throw ScanError.Fault(se.Message, se);
            }
            catch (Exception e)
            {
                throw ScanError.Fault("Error during scan.", e);
            }
        }
    }
}
