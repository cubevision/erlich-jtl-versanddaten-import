﻿namespace JTLVersandImport.Models
{
    public class ImapCredentials
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }

    }
}
