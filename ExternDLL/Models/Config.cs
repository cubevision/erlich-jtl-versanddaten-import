using System.Collections.Generic;

namespace JTLVersandImport.Models
{
    public class Config
    {
        public IList<Versandart> Versand { get; set; }
        public ImapCredentials Imap { get; set; }
        public Provider[] Provider { get; set; }
        public DatabaseConnection DatabaseConnection { get; set; }
    }
}
