using System;
using System.IO;
using JTLVersandImport.Models;

namespace JTLVersandImport.Services
{
    public class ConfigService
    {
        private static string DEFAULT_CONFIG = ".jtlImporterConfig.json";
        public static Config GetConfig(string ConfigPath)
        {
            var configPath = 
                string.IsNullOrEmpty(ConfigPath) 
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), DEFAULT_CONFIG) 
                    : ConfigPath;
            var jsonString = File.ReadAllText(configPath);
            return JsonUnmarshaller.Unmarshall<Config>(jsonString);
        }
    }
}
