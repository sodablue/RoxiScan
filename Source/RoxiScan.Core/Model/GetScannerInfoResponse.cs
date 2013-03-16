using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace RoxiScan.Model
{
    public class GetScannerInfoResponse
    {
        public GetScannerInfoResponse()
        {
            this.Scanners = new List<Scanner>();
        }

        public class Scanner
        {
            public string DeviceId { get; set; }
            public string DeviceName { get; set; }
            public bool HasAutoDocumentFeeder { get; set; }
        }

        public List<Scanner> Scanners { get; set; }
    }
}
