using System;

namespace JTLVersandImport.Services
{
    public static class TrackingNumberCorrector
    {
        public static String Clean(String trackingNumber) {
            if (trackingNumber.StartsWith("zu"))
            {
                return "";
            } else if (trackingNumber.Equals("SEND"))
            {
                return "";
            }
            return trackingNumber;
        }
    }
}
