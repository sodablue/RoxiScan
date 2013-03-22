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
using RoxiScan.Commands;

namespace RoxiScan
{
    public class ViewModel : INotifyPropertyChanged
    {
        private GetScannerListCommand getScannerListCommand = new GetScannerListCommand();

        public ViewModel()
        {
            getScannerListCommand.Execute(this);
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






    }
}
