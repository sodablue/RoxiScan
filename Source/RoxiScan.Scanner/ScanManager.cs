using RoxiScan.Models;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using WIA;

namespace RoxiScan
{
    public class ScanManager
    {
        private event Action<Image> handleImage;
        private event Action scanComplete;

        private DeviceInfo scanner;
        private ScannerSettings settings;

        public ScanManager(DeviceInfo scanner, ScannerSettings settings)
        {
            if (scanner == null)
                throw new ScanException("Device must be specified.");

            this.scanner = scanner;
            this.settings = settings;
        }

        public void BeginScan(Action<Image> handleImage, Action handleComplete)
        {
            this.handleImage = handleImage;
            this.scanComplete = handleComplete;


            Scan();
        }

        void Scan()
        {
            bool hasMorePages = true;

            while (hasMorePages)
            {
                try
                {
                    var device = this.scanner.Connect();

                    var item = device.Items[1];

                    SetColorDepth(item);
                    SetPageSize(item);

                    ImageFile imgFile = item.Transfer(WIA_IMAGE_FORMAT.JPEG);

                    // process File
                    var imageBytes = (byte[])imgFile.FileData.get_BinaryData();
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var ret = Image.FromStream(ms);
                        this.handleImage(ret);
                    }

                    Property documentHandlingSelect = device.Properties["Document Handling Select"];
                    Property documentHandlingStatus = device.Properties["Document Handling Status"];

                    hasMorePages = false;
                    if (documentHandlingSelect != null)
                    {
                        int select = documentHandlingSelect.get_Value();
                        if ((select & WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
                        {
                            int status = documentHandlingStatus.get_Value();
                            hasMorePages = ((status & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
                        }
                    }
                }
                catch (COMException ex)
                {
                    // paper empty
                    if ((uint)ex.ErrorCode != WIA_ERROR.WIA_ERROR_PAPER_EMPTY)
                    {
                        throw;
                    }
                    hasMorePages = false;
                }
            }

            this.scanComplete();
        }

        private void SetScannerFeedType(Device device)
        {
            try
            {
                Property documentHandlingSelect = device.Properties["Document Handling Select"];

                if (documentHandlingSelect != null)
                {
                    if (this.settings.UseAutomaticDocumentFeeder)
                    {
                        documentHandlingSelect.set_Value(WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER);
                    }
                    else
                    {
                        documentHandlingSelect.set_Value(WIA_DPS_DOCUMENT_HANDLING_SELECT.FLATBED);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ScanException("Error setting document feed.", ex);
            }
        }

        private void SetColorDepth(Item item)
        {
            try
            {
                item.Properties["Current Intent"].set_Value(this.settings.ColorDepth.Value);
                item.Properties["Bits Per Pixel"].set_Value(this.settings.ColorDepth.BitsPerPixel);
            }
            catch (Exception ex)
            {
                throw new ScanException("Error setting color depth.", ex);
            }
        }

        private void SetPageSize(Item item)
        {
            if (item == null)
                return;

            item.Properties["Horizontal Resolution"].set_Value(this.settings.Resolution.Value);
            item.Properties["Vertical Resolution"].set_Value(this.settings.Resolution.Value);

            double hExtent = item.Properties["Horizontal Extent"].SubTypeMax;
            double vExtent = item.Properties["Vertical Extent"].SubTypeMax;

            if (this.settings.Orientation.Direction == 0)
            {
                item.Properties["Horizontal Extent"].set_Value(this.settings.Resolution.Value * this.settings.PageSize.Width);
                item.Properties["Vertical Extent"].set_Value(this.settings.Resolution.Value * this.settings.PageSize.Height);
            }
            else
            {
                item.Properties["Horizontal Extent"].set_Value(this.settings.Resolution.Value * this.settings.PageSize.Height);
                item.Properties["Vertical Extent"].set_Value(this.settings.Resolution.Value * this.settings.PageSize.Width);
            }
        }

    }
}
