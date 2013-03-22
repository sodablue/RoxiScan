using RoxiScan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RoxiScan.Commands
{
    public class ScanCommand
    {
        public void Execute(ViewModel viewModel)
        {
            viewModel.Status = string.Empty;
            var request = new ScanRequest()
            {
                DeviceId = viewModel.Scanner.Device.DeviceId,
                Settings = new ScannerSettings
                {
                    PageSize = PageSizes.Letter,
                    ColorDepth = ColorDepths.Color,
                    Orientation = Orientations.Portrait,
                    Resolution = Resolutions.R300,
                    UseAutomaticDocumentFeeder = true
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
