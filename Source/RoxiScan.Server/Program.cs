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
            //Test();
            //StartService();
            StartDiscoveryService();
        }

        private static void StartDiscoveryService()
        {
            Uri baseAddress = Discovery.AvailableTcpBaseAddress;

            Console.WriteLine(baseAddress.ToString());
            using (ServiceHost host = new ServiceHost(typeof(ScannerService), baseAddress))
            {
                host.AddServiceEndpoint(typeof(IScanner), Helper.CreateBinding(), string.Empty);
                ServiceDiscoveryBehavior discoveryBehavior = new ServiceDiscoveryBehavior();
                host.Description.Behaviors.Add(discoveryBehavior);
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
                discoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());

//                host.AddDefaultEndpoints();
                host.Open();

                //Can do blocking calls:
                Console.WriteLine("Press ENTER to shut down service.");
                Console.ReadLine();
            }
//            host.Close();
        }

        private static void StartService()
        {
            Uri baseAddress = new Uri("net.tcp://localhost:8080/roxiscan");

            using (ServiceHost host = new ServiceHost(typeof(ScannerService)))
            {
                var binding = Helper.CreateBinding();

                host.AddServiceEndpoint(typeof(IScanner), binding , baseAddress);
                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }
        }

        private static void Test()
        {
            DeviceManager manager = new DeviceManager();
            foreach (DeviceInfo info in manager.DeviceInfos)
            {
                Console.WriteLine(info.Type.ToString());
                if (info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    string deviceId = info.DeviceID;
                    string deviceName = null;
                    foreach (Property item in info.Properties)
                    {
                        Console.WriteLine("{0}: {1}",item.Name, item.get_Value());
                        if (item.Name == "Description")
                            deviceName = item.get_Value();
                    }
                }
            }
        }
    }
}
