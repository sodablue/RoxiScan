using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace RoxiScan.Models
{
    [DataContract]
    public class ScanError
    {
        public ScanError(string errorMessage, string additionalDetails)
        {
            this.ErrorMessage = errorMessage;
        }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string AdditionalDetails { get; set; }

        public static FaultException<ScanError> Fault(string message, Exception ex)
        {
            return new FaultException<ScanError>(new ScanError(message, ex.ToString()), new FaultReason("An error occurred during a WCF call, see inner Detail for more info!"));
        }
    }
}
