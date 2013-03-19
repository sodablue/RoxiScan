using System.IO;
using System.ServiceModel;
using RoxiScan.Models;
using System.Collections.Generic;

namespace RoxiScan
{
    [ServiceContract]
    public interface IScanner
    {
        [OperationContract]
        [FaultContract(typeof(ScanError))]
        IEnumerable<Scanner> GetScannerInfo();

        [OperationContract]
        [FaultContract(typeof(ScanError))]
        Stream Scan(ScanRequest request);
    }
}
