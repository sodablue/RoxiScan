using System;

namespace RoxiScan
{
    public class ScanException : Exception
    {
        public ScanException(string message) : base(message) { }
        public ScanException(string message, Exception ex) : base(message, ex) {  }
    }

    public class PaperEmptyScanException : ScanException
    {
        public PaperEmptyScanException() : base("Paper is Empty") { }
    }

}
