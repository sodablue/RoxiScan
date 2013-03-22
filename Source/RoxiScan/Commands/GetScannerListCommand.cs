using RoxiScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Commands
{
    class GetScannerListCommand
    {
        public void Execute(ViewModel viewModel)
        {
            IScanner proxy = DiscoveryHelper.CreateDiscoveryProxy();
            try
            {
                var response = proxy.GetScannerInfo();

                viewModel.Scanners.Clear();
                foreach (var item in response)
                {
                    viewModel.Scanners.Add(item);
                }
                if (viewModel.Scanners.Count > 0)
                {
                    viewModel.Scanner = viewModel.Scanners[0];
                }
            }
            catch (FaultException<ScanError> e)
            {
                viewModel.Status = e.Detail.ErrorMessage;
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
    }
}
