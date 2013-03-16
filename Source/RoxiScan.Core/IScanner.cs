using System.IO;
using System.ServiceModel;
using RoxiScan.Model;

namespace RoxiScan
{
    [ServiceContract]
    public interface IScanner
    {
        [OperationContract]
        GetScannerInfoResponse GetScannerInfo();

        [OperationContract]
        Stream Scan(ScanRequest request);
    }
}
