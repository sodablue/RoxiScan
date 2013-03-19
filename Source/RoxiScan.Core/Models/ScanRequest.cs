using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace RoxiScan.Models
{
    [DataContract]
    public class ScanRequest
    {
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public ScannerSettings Settings { get; set; }
    }
}
