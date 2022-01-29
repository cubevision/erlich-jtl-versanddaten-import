using System;
using System.IO;

namespace JTLVersandImport.Services
{
    public static class JobExecutionManagerService
    {
        private static string LOG_FILE = ".jtlImporterJobLog.txt";

        public static DateTime GetLastExecutionDate()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LOG_FILE);
            StreamReader streamReader;
            try
            {
                streamReader = File.OpenText(path);
            } catch(FileNotFoundException)
            {
                StreamWriter streamWriter = File.CreateText(path);
                streamWriter.Write(DateTime.Now.ToString("dd.MM.yyyy"));
                streamWriter.Close();
                streamReader = File.OpenText(path);
            }

            var lastExecutionDateTime = DateTime.ParseExact(streamReader.ReadToEnd(), "dd.MM.yyyy", null);
            streamReader.Close();

            return lastExecutionDateTime;
        }

        public static void UpdateExecutionDate()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LOG_FILE);
            File.WriteAllText(path, DateTime.Now.ToString("dd.MM.yyyy"));
        }
    }
}
