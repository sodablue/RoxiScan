using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.ServiceModel.Discovery;
using System.ServiceModel;
using System.Diagnostics;

namespace RoxiScan.Server
{
    public static class Discovery
    {
        public static Uri AvailableTcpBaseAddress
        {
            get
            {
                return new UriBuilder { Scheme = Uri.UriSchemeNetTcp, Port = FindAvailablePort(), Host = System.Environment.MachineName }.Uri;
            }
        }

        static int FindAvailablePort()
        {
            Mutex mutex = new Mutex(false, "ServiceModelEx.DiscoveryHelper.FindAvailablePort");
            try
            {
                mutex.WaitOne();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(endPoint);
                    IPEndPoint local = (IPEndPoint)socket.LocalEndPoint;
                    return local.Port;
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
