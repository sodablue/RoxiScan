using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.ServiceModel.Channels;

namespace RoxiScan
{
    public static class DiscoveryHelper
    {
        public static EndpointAddress DiscoverAddress<T>(Uri scope = null)
        {
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            FindCriteria criteria = new FindCriteria(typeof(T));
            criteria.MaxResults = 1;
            if (scope != null)
            {
                criteria.Scopes.Add(scope);
            }

            FindResponse discovered = discoveryClient.Find(criteria);
            discoveryClient.Close();

            return discovered.Endpoints[0].Address;
        }

        public static Binding CreateBinding()
        {
            var binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.StreamedResponse;
            binding.MaxReceivedMessageSize = 1000000;

            return binding;
        }

        public static IScanner CreateDiscoveryProxy()
        {
            return ChannelFactory<IScanner>.CreateChannel(CreateBinding(), DiscoverAddress<IScanner>());
        }
    }
}
