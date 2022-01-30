using System;
using System.IO;
using JTLVersandImport.Models;
using log4net;
using S22.Imap;

namespace JTLVersandImport.Services
{
    public sealed class EmailService
    {

        private static ILog logger = LogManager.GetLogger(typeof(EmailService));
        private readonly Config config;

        public EmailService(Config config)
        {
            this.config = config;
        }

        public Stream GetAttachment(Provider provider, DateTime sentSince)
        {
            logger.Debug($"initializing imap with arguments {config.Imap.Host}, {config.Imap.Port}, {config.Imap.SSL}");
            using (ImapClient client = new ImapClient(config.Imap.Host, config.Imap.Port, config.Imap.SSL))
            {
                client.Login(config.Imap.User, config.Imap.Password, AuthMethod.Login);
                logger.Debug("login to mail account successful");
                logger.Debug($"searching for messages since {sentSince} from sender {provider.Sender}");
                var uids = client.Search(
                    SearchCondition
                        .SentSince(sentSince)
                        .And(SearchCondition.From(provider.Sender))
                        .And(SearchCondition.Subject(provider.Subject)));
                Stream attachmentStream = null;

                foreach (var uid in uids)
                {
                    logger.Debug($"found message for {provider.Sender}");
                    attachmentStream = client.GetMessage(uid).Attachments[0].ContentStream;
                    break;
                }

                return attachmentStream;
            }
        }
    }
}
