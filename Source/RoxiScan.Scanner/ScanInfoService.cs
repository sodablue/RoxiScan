using RoxiScan.Models;
using System.Collections.Generic;
using System.Linq;
using WIA;

namespace RoxiScan
{
    public class ScanInfoService
    {
        public IEnumerable<DeviceInfo> GetDevices()
        {
            return new DeviceManager().DeviceInfos.Cast<DeviceInfo>().ToList();
        }

        public IEnumerable<Scanner> GetScanners()
        {
            var scanners = GetDevices().Where(s => s.Type == WiaDeviceType.ScannerDeviceType);
            foreach (var info in scanners)
            {
                var name = info.Properties["Description"].get_Value();
                Device device = info.Connect();

                var scannerInfo = new ScannerInfo(info.DeviceID, name);

                scannerInfo.HorizontalBedSize = device.Properties["Horizontal Bed Size"].get_Value();
                scannerInfo.VerticalBedSize = device.Properties["Vertical Bed Size"].get_Value();

                scannerInfo.HorizontalOpticalResolution = device.Properties["Horizontal Optical Resolution"].get_Value();
                scannerInfo.VerticalOpticalResolution = device.Properties["Vertical Optical Resolution"].get_Value();

                scannerInfo.HasFeeder = ((device.Properties["Document Handling Capabilities"].get_Value() & WIA_DPS_DOCUMENT_HANDLING_CAPABILITIES.FEED) != 0);
                if (scannerInfo.HasFeeder)
                {
                    scannerInfo.HorizontalSheetFeedSize = device.Properties["Horizontal Sheet Feed Size"].get_Value();
                    scannerInfo.VerticalSheetFeedSize = device.Properties["Vertical Sheet Feed Size"].get_Value();
                }

                yield return new Scanner
                {
                    Name = name,
                    Device = scannerInfo
                };
            }
        }

        public DeviceInfo GetDeviceInfo(string deviceId)
        {
            return GetDevices()
                .Where(s => s.Type == WiaDeviceType.ScannerDeviceType)
                .Where(s => s.DeviceID == deviceId)
                .SingleOrDefault();
        }
    }
}
