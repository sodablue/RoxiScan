using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Diagnostics;
using RoxiScan.Model;
using System.IO;
using System.Threading.Tasks;

namespace RoxiScan
{
    public class ScannerDiscoveryClient : IScanner
    {
        private Binding binding;
        private EndpointAddress address;

        public ScannerDiscoveryClient()
        {
            this.address = Helper.DiscoverAddress<IScanner>();
            this.binding = Helper.CreateBinding();
        }

        public GetScannerInfoResponse GetScannerInfo()
        {
            IScanner proxy = ChannelFactory<IScanner>.CreateChannel(binding, address);
            var result = proxy.GetScannerInfo();
            (proxy as ICommunicationObject).Close();

            return result;
        }

        public Stream Scan(ScanRequest request)
        {
            IScanner proxy = ChannelFactory<IScanner>.CreateChannel(binding, address);
            var result = proxy.Scan(request);
            (proxy as ICommunicationObject).Close();

            return result;
        }

    }
}
