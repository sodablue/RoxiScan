using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Models
{
    [DataContract]
    public class ScannerSettings
    {
        [DataMember]
        public PageSize PageSize { get; set; }
        [DataMember]
        public ColorDepth ColorDepth { get; set; }
        [DataMember]
        public Resolution Resolution { get; set; }
        [DataMember]
        public Orientation Orientation { get; set; }
        [DataMember]
        public bool UseAutomaticDocumentFeeder { get; set; }
    }
}
