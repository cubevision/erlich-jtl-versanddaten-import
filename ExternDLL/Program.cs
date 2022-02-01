using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using JTLVersandImport.Appenders;
using JTLVersandImport.Models;
using JTLVersandImport.Reader;
using JTLVersandImport.Services;
using log4net;

namespace JTLVersandImport
{
    static class Program
    {

        private static ILog logger = LogManager.GetLogger(typeof(Program));
        public class Options
        {
            [Option('c', "config", Default = "", Required = false, HelpText = "Path to config file")]
            public string ConfigPath { get; set; }

            [Option('d', "date", Default = "", Required = false, HelpText = "Date in dd.MM.yyyy to process import")]
            public string Date { get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
            {
                Config config = null;
                DateTime sent;

                try
                {
                    config = ConfigService.GetConfig(parsedArgs.ConfigPath);
                    try
                    {
                        sent = DateTime.ParseExact(parsedArgs.Date, "dd.MM.yyyy", null);
                    } catch (FormatException) {
                        sent = DateTime.Now;
                    }

                    InstrumentSmtpAppender(config);
                    ValidateAssembly validateAssembly = new ValidateAssembly();
                    if (!validateAssembly.IsValid)
                    {
                        throw new Exception("JTLwawiextern.dll not found");
                    }
                    new ConnectionService(config).CheckConnection();
                    RunJob(config, sent);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    Environment.Exit(1);
                }
            });
        }

        static void InstrumentSmtpAppender(Config config)
        {
            ConfigSmtpAppender smtpAppender = new ConfigSmtpAppender(config);
            smtpAppender.Layout = new log4net.Layout.PatternLayout("%date %-5level %logger - %message%newline");
            smtpAppender.Threshold = log4net.Core.Level.Error;
            ((log4net.Repository.Hierarchy.Logger)logger.Logger).AddAppender(smtpAppender);
        }

        static void RunJob(Config config, DateTime sent)
        {
            logger.Debug("######## running new job ########");
            var importerService = new VersanddatenImporterService(config.DatabaseConnection.Server, config.DatabaseConnection.Database, config.DatabaseConnection.User, config.DatabaseConnection.Password);
            var emailService = new EmailService(config);
            var versanddatenExport = new List<VersanddatenExport>();

            foreach (var provider in config.Provider)
            {
                Stream stream = emailService.GetAttachment(provider, sent);
                if (stream != null)
                {
                    Type readerType = Type.GetType(provider.Reader);
                    VersanddatenExportReader reader = (AbstractReader)Activator.CreateInstance(readerType, stream, config);
                    List<VersanddatenExport> versanddatenExports = reader.ToVersanddatenExport();
                    versanddatenExport.AddRange(versanddatenExports);
                }
            }

            importerService.Import(versanddatenExport);
            logger.Debug("######## job finished ########");
        }
    }
}
