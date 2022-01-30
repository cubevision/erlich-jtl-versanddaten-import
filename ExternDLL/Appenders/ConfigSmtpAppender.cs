using JTLVersandImport.Models;
using log4net.Appender;

namespace JTLVersandImport.Appenders
{
    public class ConfigSmtpAppender : SmtpAppender
    {
        private readonly Config config;
        public ConfigSmtpAppender(Config config) : base()
        {
            this.config = config;
        }

        protected override void SendEmail(string messageBody)
        {
            To = config.ErrorConfig.To;
            From = "infomailer@erlich.de";
            Authentication = SmtpAuthentication.Basic;
            SmtpHost = config.ErrorConfig.SmtpHost;
            Username = config.ErrorConfig.User;
            Password = config.ErrorConfig.Password;
            EnableSsl = config.ErrorConfig.Ssl;
            Subject = "Versandjob failed";
            base.SendEmail(messageBody);
        }
    }
}
