using System;
using System.Data.SqlTypes;

namespace JTLVersandImport.Models
{
    public class Versand
    {
        public int ID { get; set; }
        public int LieferscheinId { get; set; }
        public int BenutzerId { get; set; }
        public int Logistik { get; set; }
        public string IdentCode { get; set; }
        public DateTime? Erstellt { get; set; }
        public string Hinweis { get; set; }
        public decimal Gewicht { get; set; }
        public int VersandartId { get; set; }
        public string Logistiker { get; set; }
        public string FullfillmentCenter { get; set; }
        public DateTime? Ankunftszeit { get; set; }
        public int VerpackZeitSek { get; set; }
        public DateTime? Versendet { get; set; }
        public int Status { get; set; }
        public string ShipmentId { get; set; }
        public string Reference { get; set; }
        public string ShipmentOrderId { get; set; }
        public SqlBytes RowVersion { get; set; }
        public string EnclosedReturnIdentCode { get; set; }
        public int ViaAmazonMWS { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID}, {nameof(LieferscheinId)}={LieferscheinId}, {nameof(IdentCode)}={IdentCode}, {nameof(VersandartId)}={VersandartId}, {nameof(Versendet)}={Versendet}}}";
        }
    }
}
