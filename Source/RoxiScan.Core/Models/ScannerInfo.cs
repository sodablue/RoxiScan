using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Models
{
    [DataContract]
    public class ScannerInfo
    {
        public ScannerInfo(string deviceId, string deviceName)
        {
            this.DeviceId = deviceId;
            this.DeviceName = deviceName;
        }

        [DataMember]
        public string DeviceId { get; private set; }
        [DataMember]
        public string DeviceName { get; private set; }

        [DataMember]
        public int HorizontalBedSize { get; set; }
        [DataMember]
        public int VerticalBedSize { get; set; }
        [DataMember]
        public int HorizontalOpticalResolution { get; set; }
        [DataMember]
        public int VerticalOpticalResolution { get; set; }

        [DataMember]
        public bool HasFeeder { get; set; }
        [DataMember]
        public int HorizontalSheetFeedSize { get; set; }
        [DataMember]
        public int VerticalSheetFeedSize { get; set; }
    }
}
