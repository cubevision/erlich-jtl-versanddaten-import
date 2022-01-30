using System.Collections.Generic;
using System.Data.SqlClient;
using JTLVersandImport.Models;

namespace JTLVersandImport.Repository
{
    public class LieferscheinRepository : AdoRepository<Lieferschein>
    {
        public LieferscheinRepository(string connectionString)
            : base(connectionString)
        { }
        public LieferscheinRepository(string server, string datenbank, string benutzer, string passwort) : this($"Data Source = {server}; Initial Catalog = {datenbank}; User Id = {benutzer}; Password = {passwort}; ")
        { }

        public IEnumerable<Lieferschein> GetAll()
        {
            using (var command = new SqlCommand("SELECT l.kLieferschein, l.kBestellung, l.cLieferscheinNr FROM [dbo].[tLieferschein] l"))
            {
                return GetRecords(command);
            }
        }
        public Lieferschein GetById(string id)
        {
            using (var command = new SqlCommand("SELECT l.kLieferschein, l.kBestellung, l.cLieferscheinNr FROM [dbo].[tLieferschein] WHERE kLieferschein = @id"))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                return GetRecord(command);
            }
        }
        public Lieferschein GetByBestellNr(string id)
        {
            using (var command = new SqlCommand(@"
                SELECT l.kLieferschein, l.kBestellung, l.cLieferscheinNr
                FROM [dbo].[tBestellung] b
                JOIN [dbo].[tLieferschein] l
                ON b.kBestellung = l.kBestellung
                WHERE b.cBestellNr = @id;"))
            {
                command.Parameters.Add(new SqlParameter("id", id));
                return GetRecord(command);
            }
        }
        public override Lieferschein PopulateRecord(SqlDataReader reader)
        {
            var ID = reader.GetInt32(0);
            var BestellId = reader.GetInt32(1);
            var LieferscheinNummer = reader.IsDBNull(2) ? "" : reader.GetString(2);

            return new Lieferschein
            {
                ID = ID,
                BestellId = BestellId,
                LieferscheinNummer = LieferscheinNummer
            };
        }
    }
}
