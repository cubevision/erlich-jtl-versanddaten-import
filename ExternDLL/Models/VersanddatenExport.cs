using System;

namespace JTLVersandImport.Models
{
    public class VersanddatenExport
    {
        public string TrackingNummer { get; set; }
        public string BestellNr { get; set; }
        public DateTime Versanddatum { get; set; }
        public int Spediteur { get; set; }

        public Versand ToVersand()
        {
            return new Versand
            {
                Hinweis = "",
                LieferscheinId = 0,
                BenutzerId = 0,
                Logistik = 0,
                IdentCode = TrackingNummer,
                Erstellt = DateTime.Now,
                Gewicht = 0,
                VersandartId = 0,
                Logistiker = "",
                FullfillmentCenter = "",
                Ankunftszeit = null,
                VerpackZeitSek = 0,
                Versendet = null,
                Status = 0,
                ShipmentId = "",
                Reference = "",
                ShipmentOrderId = "",
                EnclosedReturnIdentCode = "",
                ViaAmazonMWS = 0
            };
        }

        public override string ToString()
        {
            return $"{{{nameof(TrackingNummer)}={TrackingNummer}, {nameof(BestellNr)}={BestellNr}, {nameof(Versanddatum)}={Versanddatum}, {nameof(Spediteur)}={Spediteur}}}";
        }
    }
}
