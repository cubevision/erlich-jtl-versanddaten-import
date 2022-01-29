using System;
using System.IO;
using S22.Imap;
using JTLVersandImport.Models;

namespace JTLVersandImport.Services
{
    public sealed class EmailService
    {

        private readonly Config config;
        
        public EmailService(Config config)
        {
            this.config = config;
        }

        public Stream GetAttachment(Provider provider, DateTime sentSince)
        {
            using (ImapClient client = new ImapClient(config.Imap.Host, config.Imap.Port, config.Imap.SSL))
            {
                client.Login(config.Imap.User, config.Imap.Password, AuthMethod.Login);
                var uids = client.Search(
                    SearchCondition
                        .SentSince(sentSince)
                        .And(SearchCondition.From(provider.Sender))
                        .And(SearchCondition.Subject(provider.Subject)));
                Stream attachmentStream = null;

                foreach (var uid in uids)
                {
                    attachmentStream = client.GetMessage(uid).Attachments[0].ContentStream;
                    break;
                }

                return attachmentStream;
            }
        }
    }
}
