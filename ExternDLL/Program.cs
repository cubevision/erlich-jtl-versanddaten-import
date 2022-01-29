using System;
using System.IO;
using System.Collections.Generic;
using JTLVersandImport.Services;
using JTLVersandImport.Reader;
using JTLVersandImport.Models;
using CommandLine;

namespace JTLVersandImport
{
    static class Program
    {
        public class Options
        {
            [Option('c', "config", Default = "", Required = false, HelpText = "Path to config file")]
            public string ConfigPath { get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
            {
                ValidateAssembly validateAssembly = new ValidateAssembly();
                if (!validateAssembly.IsValid)
                    return;

                var config = ConfigService.GetConfig(parsedArgs.ConfigPath);
                var importerService = new VersanddatenImporterService(config.DatabaseConnection.Server, config.DatabaseConnection.Database, config.DatabaseConnection.User, config.DatabaseConnection.Password);
                var emailService = new EmailService(config);
                DateTime lastExecutionDatetime = JobExecutionManagerService.GetLastExecutionDate().AddDays(1);
                var versanddatenExport = new List<VersanddatenExport>();

                foreach (var provider in config.Provider)
                {
                    Stream stream = emailService.GetAttachment(provider, lastExecutionDatetime);
                    if (stream != null)
                    {
                        Type readerType = Type.GetType(provider.Reader);
                        VersanddatenExportReader reader = (AbstractReader)Activator.CreateInstance(readerType, stream, config);
                        List<VersanddatenExport> versanddatenExports = reader.ToVersanddatenExport();
                        versanddatenExport.AddRange(versanddatenExports);
                    }
                }

                importerService.Import(versanddatenExport);
                JobExecutionManagerService.UpdateExecutionDate();
            });
        }
    }
}
