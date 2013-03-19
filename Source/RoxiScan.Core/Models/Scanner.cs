using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Models
{
    [DataContract]
    public class Scanner : INotifyPropertyChanged
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ScannerInfo Device { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
