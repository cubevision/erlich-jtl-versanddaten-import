using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using JTLVersandImport.Models;

namespace JTLVersandImport.Reader
{
    public sealed class HausfuxReader : AbstractReader
    {

        private readonly string EXISTING_ORDER_LABEL = "SEND";

        public HausfuxReader(Stream stream, Config config) : base(stream, config)
        {
        }

        private bool IsSpedition(string trackingNumber)
        {
            if (trackingNumber.Equals(EXISTING_ORDER_LABEL))
            {
                return true;
            }

            return false;
        }

        private string GetCleanOrderId(string orderId)
        {
            var orderIdStartIndex = orderId.IndexOf("AB");
            return orderId.Substring(orderIdStartIndex > -1 ? orderIdStartIndex : 0);
        }

        public override List<VersanddatenExport> ToVersanddatenExport()
        {
            using (var reader = new StreamReader(stream))
            {
                var versanddatenexport = new List<VersanddatenExport>();
                // skip first three header lines from hausfux export
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line)) continue;

                    var values = line.Split(';');
                    var trackingNumbers = values[0].Split(',');
                    var orderId = values[1];
                    var dispatchDate = values[4];
                    var carrier = values[5];

                    versanddatenexport.AddRange(trackingNumbers.Where(trackingNumber => !string.IsNullOrEmpty(trackingNumber)).Select(trackingNumber =>
                    {
                        return new VersanddatenExport
                        {
                            TrackingNummer = CleanTrackingNumber(trackingNumber),
                            BestellNr = GetCleanOrderId(orderId),
                            Versanddatum = DateTime.ParseExact(dispatchDate, "dd.MM.yy", null),
                            Spediteur = GetMappedCarrier(carrier)
                        };
                    }));
                }

                return versanddatenexport;
            }
        }
    }
}
