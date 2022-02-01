using JTLVersandImport.Models;
using JTLwawiExtern;

namespace JTLVersandImport.Services
{
    public sealed class ConnectionService
    {

        private readonly Config config;
        private readonly CJTLwawiExtern wawi;
        public ConnectionService(Config config)
        {
            this.config = config;
            wawi = new CJTLwawiExtern();
        }

        public bool CheckConnection()
        {
            string errorMessage = "";
            int error = wawi.JTL_TesteDatenbankverbindung(config.DatabaseConnection.Server, config.DatabaseConnection.Database, config.DatabaseConnection.User, config.DatabaseConnection.Password, out errorMessage);
            if (error == 0)
            {
                throw new System.Exception(errorMessage);
            }
            return true;
        }
    }
}
