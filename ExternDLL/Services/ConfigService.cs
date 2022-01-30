using System;
using System.IO;
using JTLVersandImport.Models;
using log4net;

namespace JTLVersandImport.Services
{
    public class ConfigService
    {
        private static ILog logger = LogManager.GetLogger(typeof(ConfigService));
        private static string DEFAULT_CONFIG = ".jtlImporterConfig.json";
        public static Config GetConfig(string ConfigPath)
        {

            var configPath =
                string.IsNullOrEmpty(ConfigPath)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), DEFAULT_CONFIG)
                    : ConfigPath;
            logger.Debug($"reading config file from: {configPath}");
            var jsonString = File.ReadAllText(configPath);
            return JsonUnmarshaller.Unmarshall<Config>(jsonString);
        }
    }
}
