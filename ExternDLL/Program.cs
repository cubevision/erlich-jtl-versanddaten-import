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
        }

        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
            {
                var config = ConfigService.GetConfig(parsedArgs.ConfigPath);

                InstrumentSmtpAppender(config);

                try
                {
                    ValidateAssembly validateAssembly = new ValidateAssembly();
                    if (!validateAssembly.IsValid)
                        return;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    Environment.Exit(1);
                }

                try
                {
                    RunJob(config);
                }
                catch (Exception e)
                {
                    logger.Error(e);
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

        static void RunJob(Config config)
        {
            logger.Debug("######## running new job ########");
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
        }
    }
}
