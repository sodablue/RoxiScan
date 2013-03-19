using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using WIA;
using System.ServiceModel.Discovery;

namespace RoxiScan.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StartDiscoveryService();
        }

        private static void StartDiscoveryService()
        {
            Uri baseAddress = Discovery.AvailableTcpBaseAddress;

            Console.WriteLine(baseAddress.ToString());
            using (ServiceHost host = new ServiceHost(typeof(ScannerService), baseAddress))
            {
                host.AddServiceEndpoint(typeof(IScanner), DiscoveryHelper.CreateBinding(), string.Empty);
                ServiceDiscoveryBehavior discoveryBehavior = new ServiceDiscoveryBehavior();
                host.Description.Behaviors.Add(discoveryBehavior);
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
                discoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());

                host.Open();

                //Can do blocking calls:
                Console.WriteLine("Press ENTER to shut down service.");
                Console.ReadLine();
            }
        }
    }
}
