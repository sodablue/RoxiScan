using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using RoxiScan.Models;
using System.Threading.Tasks;

namespace RoxiScan
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            this.GetScannerList();
        }

        private readonly ObservableCollection<Scanner> scanners = new ObservableCollection<Scanner>();
        public ObservableCollection<Scanner> Scanners
        {
            get { return scanners; }
        }

        private Scanner scanner;
        public Scanner Scanner
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
            IScanner proxy = DiscoveryHelper.CreateDiscoveryProxy();
            try
            {
                var response = proxy.GetScannerInfo();

                this.Scanners.Clear();
                foreach (var item in response)
                {
                    this.Scanners.Add(item);
                }
                if (this.Scanners.Count > 0)
                {
                    this.Scanner = this.Scanners[0];
                }
            }
            catch (FaultException<ScanError> e)
            {
                this.Status = e.Detail.ErrorMessage;
            }
            catch (CommunicationException)
            {

            }
            finally
            {
                ICommunicationObject comm = ((ICommunicationObject)proxy);

                if (comm.State == CommunicationState.Faulted)
                {
                    comm.Abort();
                }
                else
                {
                    comm.Close();
                }
            }
        }

        public void Scan()
        {
            this.Status = string.Empty;
            var request = new ScanRequest()
            {
                DeviceId = this.Scanner.Device.DeviceId,
                Settings = new ScannerSettings
                {
                    PageSize = PageSizes.Letter,
                    ColorDepth = ColorDepths.BlackAndWhite,
                    Orientation = Orientations.Portrait,
                    Resolution = Resolutions.R300,
                    UseAutomaticDocumentFeeder = false
                }
            };

            IScanner proxy = DiscoveryHelper.CreateDiscoveryProxy();
            try
            {
                var response = proxy.Scan(request);

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
            catch (FaultException<ScanError> e)
            {
                this.Status = e.Detail.ErrorMessage;
            }
            catch (CommunicationException)
            {

            }
            finally
            {
                ICommunicationObject comm = ((ICommunicationObject)proxy);

                if (comm.State == CommunicationState.Faulted)
                {
                    comm.Abort();
                }
                else
                {
                    comm.Close();
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
