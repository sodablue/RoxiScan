using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace RoxiScan.Model
{
    public class ScanRequest
    {
        public string DeviceId { get; set; }
        public string PaperSize { get; set; }
        public string ScanSource { get; set; }
    }
}
