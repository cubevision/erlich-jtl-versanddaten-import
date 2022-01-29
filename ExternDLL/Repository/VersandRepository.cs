using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTLVersandImport.Models;

namespace JTLVersandImport.Repository
{
    public class VersandRepository : AdoRepository<Versand>
    {
        public VersandRepository(string connectionString)
            : base(connectionString)
        {}
        public VersandRepository(string server, string datenbank, string benutzer, string passwort) : this($"Data Source = {server}; Initial Catalog = {datenbank}; User Id = {benutzer}; Password = {passwort}; ")
        {}
        public IEnumerable<Versand> GetAll()
        {
            using (var command = new SqlCommand("SELECT * FROM dbo.tVersand"))
            {
                return GetRecords(command);
            }
        }
        public Versand GetById(string id)
        {
            using (var command = new SqlCommand("SELECT * FROM dbo.tVersand WHERE kVersand = @id"))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                return GetRecord(command);
            }
        }
        public IEnumerable<Versand> GetByLieferscheinId(string id)
        {
            using (var command = new SqlCommand("SELECT * FROM dbo.tVersand WHERE kLieferschein = @id"))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                return GetRecords(command);
            }
        }
        public void InsertVersand(Versand versand)
        {
            using (var command = new SqlCommand(@"
                INSERT INTO [dbo].[tVersand] (
                    [cHinweis],
                    [kLieferschein],
                    [kBenutzer],
                    [kLogistik],
                    [cIdentCode],
                    [dErstellt],
                    [fGewicht],
                    [kVersandArt],
                    [cLogistiker],
                    [cFulfillmentCenter],
                    [nVerpackZeitSek],
                    [dVersendet],
                    [nStatus],
                    [cShipmentId],
                    [cReference],
                    [cShipmentOrderId],
                    [cEnclosedReturnIdentCode],
                    [nViaAmazonMWS]
                ) VALUES (
                    @cHinweis,
                    @kLieferschein,
                    @kBenutzer,
                    @kLogistik,
                    @cIdentCode,
                    @dErstellt,
                    @fGewicht,
                    @kVersandArt,
                    @cLogistiker,
                    @cFulfillmentCenter,
                    @nVerpackZeitSek,
                    @dVersendet,
                    @nStatus,
                    @cShipmentId,
                    @cReference,
                    @cShipmentOrderId,
                    @cEnclosedReturnIdentCode,
                    @nViaAmazonMWS
                );"))
            {
                command.Parameters.Add(new SqlParameter("cHinweis", versand.Hinweis));
                command.Parameters.Add(new SqlParameter("kLieferschein", versand.LieferscheinId));
                command.Parameters.Add(new SqlParameter("kBenutzer", versand.BenutzerId));
                command.Parameters.Add(new SqlParameter("kLogistik", versand.Logistik));
                command.Parameters.Add(new SqlParameter("cIdentCode", versand.IdentCode));
                command.Parameters.Add(new SqlParameter("dErstellt", versand.Erstellt));
                command.Parameters.Add(new SqlParameter("fGewicht", versand.Gewicht));
                command.Parameters.Add(new SqlParameter("kVersandArt", versand.VersandartId));
                command.Parameters.Add(new SqlParameter("cLogistiker", versand.Logistiker));
                command.Parameters.Add(new SqlParameter("cFulfillmentCenter", versand.FullfillmentCenter));
                command.Parameters.Add(new SqlParameter("nVerpackZeitSek", versand.VerpackZeitSek));
                command.Parameters.Add(new SqlParameter("dVersendet", DBNull.Value));
                command.Parameters.Add(new SqlParameter("nStatus", versand.Status));
                command.Parameters.Add(new SqlParameter("cShipmentId", versand.ShipmentId));
                command.Parameters.Add(new SqlParameter("cReference", versand.Reference));
                command.Parameters.Add(new SqlParameter("cShipmentOrderId", versand.ShipmentOrderId));
                command.Parameters.Add(new SqlParameter("cEnclosedReturnIdentCode", versand.EnclosedReturnIdentCode));
                command.Parameters.Add(new SqlParameter("nViaAmazonMWS", versand.ViaAmazonMWS));
                GetRecord(command);
            }
        }
        public void UpdateVersand(Versand versand)
        {
            using (var command = new SqlCommand(@"
                UPDATE [dbo].[tVersand]
                SET [cHinweis] = @cHinweis,
                    [kLieferschein] = @kLieferschein,
                    [kBenutzer] = @kBenutzer,
                    [kLogistik] = @kLogistik,
                    [cIdentCode] = @cIdentCode,
                    [dErstellt] = @dErstellt,
                    [fGewicht] = @fGewicht,
                    [kVersandArt] = @kVersandArt,
                    [cLogistiker] = @cLogistiker,
                    [cFulfillmentCenter] = @cFulfillmentCenter,
                    [nVerpackZeitSek] = @nVerpackZeitSek,
                    [dVersendet] = @dVersendet,
                    [nStatus] = @nStatus,
                    [cShipmentId] = @cShipmentId,
                    [cReference] = @cReference,
                    [cShipmentOrderId] = @cShipmentOrderId,
                    [cEnclosedReturnIdentCode] = @cEnclosedReturnIdentCode,
                    [nViaAmazonMWS] = @nViaAmazonMWS
                WHERE [kVersand] = @id;")
            )
            {
                command.Parameters.Add(new SqlParameter("id", versand.ID));
                command.Parameters.Add(new SqlParameter("cHinweis", versand.Hinweis));
                command.Parameters.Add(new SqlParameter("kLieferschein", versand.LieferscheinId));
                command.Parameters.Add(new SqlParameter("kBenutzer", versand.BenutzerId));
                command.Parameters.Add(new SqlParameter("kLogistik", versand.Logistik));
                command.Parameters.Add(new SqlParameter("cIdentCode", versand.IdentCode));
                command.Parameters.Add(new SqlParameter("dErstellt", versand.Erstellt));
                command.Parameters.Add(new SqlParameter("fGewicht", versand.Gewicht));
                command.Parameters.Add(new SqlParameter("kVersandArt", versand.VersandartId));
                command.Parameters.Add(new SqlParameter("cLogistiker", versand.Logistiker));
                command.Parameters.Add(new SqlParameter("cFulfillmentCenter", versand.FullfillmentCenter));
                command.Parameters.Add(new SqlParameter("nVerpackZeitSek", versand.VerpackZeitSek));
                command.Parameters.Add(new SqlParameter("dVersendet", DBNull.Value));
                command.Parameters.Add(new SqlParameter("nStatus", versand.Status));
                command.Parameters.Add(new SqlParameter("cShipmentId", versand.ShipmentId));
                command.Parameters.Add(new SqlParameter("cReference", versand.Reference));
                command.Parameters.Add(new SqlParameter("cShipmentOrderId", versand.ShipmentOrderId));
                command.Parameters.Add(new SqlParameter("cEnclosedReturnIdentCode", versand.EnclosedReturnIdentCode));
                command.Parameters.Add(new SqlParameter("nViaAmazonMWS", versand.ViaAmazonMWS));
                GetRecord(command);
            }
        }
        public override Versand PopulateRecord(SqlDataReader reader)
        {
            var ID = reader.GetInt32(0);
            var LieferscheinId = reader.GetInt32(1);
            var BenutzerId = reader.GetInt32(2);
            var LogistikId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
            var IdentCode = reader.IsDBNull(4) ? "" : reader.GetString(4);
            var Erstellt = reader.IsDBNull(5) ? new DateTime?() : reader.GetDateTime(5);
            var Hinweis = reader.IsDBNull(6) ? "" : reader.GetString(6);
            var Gewicht = reader.GetDecimal(7);
            var VersandartId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
            var Logistiker = reader.IsDBNull(9) ? "" : reader.GetString(9);
            var FulfillmentCenter = reader.IsDBNull(10) ? "" : reader.GetString(10);
            var VerpackZeitSek = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
            var Versendet = reader.IsDBNull(13) ? new DateTime?() : reader.GetDateTime(13);
            var Status = reader.IsDBNull(14) ? 0 : reader.GetByte(14);
            var ShipmentId = reader.IsDBNull(15) ? "" : reader.GetString(15);
            var Reference = reader.IsDBNull(16) ? "" : reader.GetString(16);
            var ShipmentOrderId = reader.IsDBNull(17) ? "" : reader.GetString(17);
            var RowVersion = reader.GetSqlBytes(18);
            var EnclosedReturnIdentCode = reader.IsDBNull(19) ? "" : reader.GetString(19);
            var ViaAmazonMWS = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);

            return new Versand
            {
                ID = ID,
                LieferscheinId = LieferscheinId,
                BenutzerId = BenutzerId,
                Logistik = LogistikId,
                IdentCode = IdentCode,
                Erstellt = Erstellt,
                Hinweis = Hinweis,
                Gewicht = Gewicht,
                VersandartId = VersandartId,
                Logistiker = Logistiker,
                FullfillmentCenter = FulfillmentCenter,
                VerpackZeitSek = VerpackZeitSek,
                Versendet = Versendet,
                Status = Status,
                ShipmentId = ShipmentId,
                Reference = Reference,
                ShipmentOrderId = ShipmentOrderId,
                RowVersion = RowVersion,
                EnclosedReturnIdentCode = EnclosedReturnIdentCode,
                ViaAmazonMWS = ViaAmazonMWS
            };
        }
    }
}
