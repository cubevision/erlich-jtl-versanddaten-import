namespace JTLVersandImport.Models
{
    public class ErrorConfig
    {
        public string To { get; set; }
        public string SmtpHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
    }
}
