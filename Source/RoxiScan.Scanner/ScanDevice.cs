using RoxiScan.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace RoxiScan
{
    public class ScanDevice : IDisposable
    {
        private Device device;
        private Item item;

        public ScanDevice(string deviceId)
        {
            var manager = new DeviceManager();
            this.device = null;
            foreach (DeviceInfo info in manager.DeviceInfos)
            {
                if (info.DeviceID == deviceId)
                {
                    this.device = info.Connect();
                    this.item = device.Items[1];
                    break;
                }
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.item != null)
                {
                    Marshal.ReleaseComObject(this.item);
                    this.item = null;
                }

                if (this.device != null)
                {
                    Marshal.ReleaseComObject(this.device);
                    this.device = null;
                }
            }
        }
        #endregion


        public void SetColorDepth(ColorDepth colorDepth)
        {
            try
            {
                item.Properties["Current Intent"].set_Value(colorDepth.Value);
                item.Properties["Bits Per Pixel"].set_Value(colorDepth.BitsPerPixel);
            }
            catch (Exception ex)
            {
                throw new ScanException("Error setting color depth.", ex);
            }
        }

        public void UseAutomaticDocumentFeeder()
        {
            try
            {
                Property documentHandlingSelect = device.Properties["Document Handling Select"];

                if (documentHandlingSelect != null)
                {
                    documentHandlingSelect.set_Value(WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER);
                }
            }
            catch (Exception ex)
            {
                throw new ScanException("Error setting document feed.", ex);
            }
        }

        public void UseFlatbed()
        {
            try
            {
                Property documentHandlingSelect = device.Properties["Document Handling Select"];

                if (documentHandlingSelect != null)
                {
                    documentHandlingSelect.set_Value(WIA_DPS_DOCUMENT_HANDLING_SELECT.FLATBED);
                }
            }
            catch (Exception ex)
            {
                throw new ScanException("Error setting flatbed.", ex);
            }
        }

        public void SetPageSize(PageSize pageSize, Orientation orientation, Resolution resolution)
        {
            if (orientation.Direction == 0)
            {
                item.Properties["Horizontal Extent"].set_Value(resolution.Value * pageSize.Width);
                item.Properties["Vertical Extent"].set_Value(resolution.Value * pageSize.Height);
            }
            else
            {
                item.Properties["Horizontal Extent"].set_Value(resolution.Value * pageSize.Height);
                item.Properties["Vertical Extent"].set_Value(resolution.Value * pageSize.Width);
            }

        }

        public void Scan(Action<Image> handleImage)
        {
            try
            {
                ImageFile imgFile = item.Transfer(WIA_IMAGE_FORMAT.JPEG);

                // process File
                var imageBytes = (byte[])imgFile.FileData.get_BinaryData();
                // If you close the memorystream before you're done with the image it throws an exception
                using (var ms = new MemoryStream(imageBytes))
                {
                    handleImage(Image.FromStream(ms));
                }
            }
            catch (COMException ce)
            {
                if ((uint)ce.ErrorCode == WIA_ERROR.WIA_ERROR_PAPER_EMPTY)
                    throw new PaperEmptyScanException();

                throw new ScanException(ce.Message, ce);
            }
        }

        public bool HasMorePages()
        {
            Property documentHandlingSelect = device.Properties["Document Handling Select"];

            bool hasMorePages = false;
            if (documentHandlingSelect != null)
            {
                int select = documentHandlingSelect.get_Value();
                if ((select & WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
                {
                    int status = device.Properties["Document Handling Status"].get_Value();
                    hasMorePages = ((status & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
                }
            }

            return hasMorePages;
        }
    }
}
