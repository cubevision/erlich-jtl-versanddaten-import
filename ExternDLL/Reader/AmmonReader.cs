using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using JTLVersandImport.Models;
using IronXL;

namespace JTLVersandImport.Reader
{
    public sealed class AmmonReader : AbstractReader
    {
        private readonly int versandArt;
        public AmmonReader(Stream stream, Config config) : base(stream, config)
        {
            Versandart versandart = config.Versand.First((versand) => versand.Spediteur.ToUpper().Contains("UPS"));
            versandArt = GetMappedCarrier(versandart.Spediteur);
        }

        private bool ShouldSkipRow(RangeColumn[] rangeColumns)
        {
            if (string.IsNullOrEmpty(rangeColumns[0].Value.ToString()))
            {
                return true;
            }

            return false;
        }

        public override List<VersanddatenExport> ToVersanddatenExport()
        {
            WorkBook workbook = new WorkBook(stream);
            WorkSheet worksheet = workbook.WorkSheets.First();
            Cell firstFilledCell = worksheet.FirstFilledCell;
            Cell lastFilledCell = worksheet.LastFilledCell;
            Range range = worksheet.GetRange($"{firstFilledCell.AddressString}:{lastFilledCell.AddressString}");
            List<VersanddatenExport> versanddatenExport = new List<VersanddatenExport>();

            foreach (var row in range.Rows.ToList().GetRange(1, range.Rows.Length - 1))
            {
                if (ShouldSkipRow(row.Columns)) continue;

                var trackingNumber = row.Columns[0].Value.ToString();
                var orderId = row.Columns[3].Value.ToString();
                var dispatchDate = DateTime.ParseExact(row.Columns[4].Value.ToString(), "dd.MM.yyyy HH:mm:ss", null);

                versanddatenExport.Add(new VersanddatenExport
                {
                    TrackingNummer = trackingNumber,
                    BestellNr = orderId,
                    Versanddatum = dispatchDate,
                    Spediteur = versandArt
                });
            }

            return versanddatenExport;
        }
    }
}
