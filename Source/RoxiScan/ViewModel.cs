using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using RoxiScan.Model;
using System.Threading.Tasks;

namespace RoxiScan
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Binding binding;
        private EndpointAddress address;

        public ViewModel()
        {
            this.address = Helper.DiscoverAddress<IScanner>();
            this.binding = Helper.CreateBinding();
            this.GetScannerList();

            this.PaperSizes.Add("Letter");
            this.PaperSizes.Add("Legal");
            this.PaperSize = "Letter";
            this.ScanSources.Add("Flatbed");
            this.ScanSources.Add("Feeder");
            this.ScanSource = "Flatbed";
        }

        private readonly ObservableCollection<GetScannerInfoResponse.Scanner> scanners = new ObservableCollection<GetScannerInfoResponse.Scanner>();
        public ObservableCollection<GetScannerInfoResponse.Scanner> Scanners
        {
            get { return scanners; }
        }

        private GetScannerInfoResponse.Scanner scanner;
        public GetScannerInfoResponse.Scanner Scanner
        {
            get { return scanner; }
            set
            {
                if (scanner != value)
                {
                    scanner = value;
                    RaisePropertyChanged("Scanner");
                }
            }
        }

        private readonly ObservableCollection<string> papersizes = new ObservableCollection<string>();
        public ObservableCollection<string> PaperSizes
        {
            get { return papersizes; }
        }

        private string papersize;
        public string PaperSize
        {
            get { return papersize; }
            set
            {
                if (papersize != value)
                {
                    papersize = value;
                    RaisePropertyChanged("PaperSize");
                }
            }
        }

        private readonly ObservableCollection<string> scansources = new ObservableCollection<string>();
        public ObservableCollection<string> ScanSources
        {
            get { return scansources; }
        }

        private string scansource;
        public string ScanSource
        {
            get { return scansource; }
            set
            {
                if (scansource != value)
                {
                    scansource = value;
                    RaisePropertyChanged("ScanSource");
                }
            }
        }

        private string filename;
        public string Filename
        {
            get { return filename; }
            set
            {
                if (filename != value)
                {
                    filename = value;
                    RaisePropertyChanged("Filename");
                }
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetScannerList()
        {
            IScanner proxy = ChannelFactory<IScanner>.CreateChannel(binding, address);
            var response = proxy.GetScannerInfo();
            (proxy as ICommunicationObject).Close();

            if (response.Scanners != null)
            {
                this.Scanners.Clear();
                foreach (var item in response.Scanners)
                {
                    this.Scanners.Add(item);
                }
                if (this.Scanners.Count > 0)
                {
                    this.Scanner = this.Scanners[0];
                }
            }
        }

        public void Scan()
        {
            this.Status = string.Empty;
            var request = new ScanRequest()
            {
                DeviceId = this.Scanner.DeviceId,
                PaperSize = this.PaperSize,
                ScanSource = this.ScanSource
            };

            //var client = new ScannerClient();
            var client = new ScannerDiscoveryClient();
            var response = client.Scan(request);

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "Acrobat|*.pdf";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                using (var file = dlg.OpenFile())
                {
                    CopyStream(response, file);
                }
            }
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
