using System;
using System.Collections.Generic;
using System.Linq;
using JTLVersandImport.Models;
using JTLVersandImport.Repository;
using JTLwawiExtern;
using JTLwawiExtern.VersanddatenImport;
using log4net;

namespace JTLVersandImport.Services
{
    public sealed class VersanddatenImporterService
    {
        private static ILog logger = LogManager.GetLogger(typeof(VersanddatenImporterService));
        private readonly CJTLwawiExtern wawiExtern;
        private readonly LieferscheinRepository lieferscheinRepository;
        private readonly VersandRepository versandRepository;
        private readonly VersanddatenImporter versandImporter;

        public VersanddatenImporterService(string server, string datenbank, string benutzer, string passwort)
        {
            logger.Debug($"initializing {nameof(VersanddatenImporterService)} with parameters: {server}:{datenbank}:{benutzer}");
            wawiExtern = new CJTLwawiExtern();
            lieferscheinRepository = new LieferscheinRepository(server, datenbank, benutzer, passwort);
            versandRepository = new VersandRepository(server, datenbank, benutzer, passwort);
            versandImporter = wawiExtern.VersanddatenImporter(server, datenbank, benutzer, passwort, 1);
        }

        private Dictionary<string, List<VersanddatenExport>> GetGroupedVersanddatenExport(List<VersanddatenExport> versanddatenexport)
        {
            var groupedVersanddatenExport = new Dictionary<string, List<VersanddatenExport>>();

            versanddatenexport.ForEach(export =>
            {
                List<VersanddatenExport> items;
                if (groupedVersanddatenExport.TryGetValue(export.BestellNr, out items))
                {
                    items.Add(export);
                }
                else
                {
                    groupedVersanddatenExport.Add(export.BestellNr, new List<VersanddatenExport> { export });
                }
            });

            return groupedVersanddatenExport;
        }

        public void Import(List<VersanddatenExport> versanddatenexport)
        {
            logger.Debug("importing versanddatenexport started");
            var groupedVersanddatenExport = GetGroupedVersanddatenExport(versanddatenexport);
            foreach (var group in groupedVersanddatenExport)
            {
                logger.Debug($"importing versanddaten for order id {group.Key}");
                var lieferschein = lieferscheinRepository.GetByBestellNr(group.Key);
                if (lieferschein != null)
                {
                    logger.Debug($"found lieferschein {lieferschein.ID} for order id {group.Key}");
                    var existingVersandElements = versandRepository.GetByLieferscheinId(lieferschein.ID.ToString());
                    logger.Debug($"found {existingVersandElements.Count()} pakete for lieferschein {lieferschein.ID}");
                    for (int i = 0, j = group.Value.Count; i < j; i++)
                    {
                        var versandExport = group.Value[i];
                        try
                        {
                            var versand = existingVersandElements.ElementAt(i);
                            versand.IdentCode = versandExport.TrackingNummer;
                            versand.VersandartId = versandExport.Spediteur;
                            logger.Debug($"updating paket {versand.ID}");
                            versandRepository.UpdateVersand(versand);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            var versand = versandExport.ToVersand();
                            versand.IdentCode = versandExport.TrackingNummer;
                            versand.VersandartId = versandExport.Spediteur;
                            versand.LieferscheinId = lieferschein.ID;
                            logger.Debug($"creating new paket {versand.ID}");
                            versandRepository.InsertVersand(versand);
                        }
                    }
                    existingVersandElements = versandRepository.GetByLieferscheinId(lieferschein.ID.ToString());

                    foreach (var existingVersandElement in existingVersandElements)
                    {
                        logger.Debug("adding versanddatenimport to JTL Wawi external dll versandatenimport");
                        versandImporter.Add(
                            $"{lieferschein.LieferscheinNummer}${existingVersandElement.ID}",
                            existingVersandElement.Versendet ?? DateTime.Now,
                            existingVersandElement.IdentCode,
                            ""
                        );
                    }
                }
            }
            logger.Debug("applying versanddatenimport to JTL Wawi external dll versandatenimport");
            versandImporter.Apply();
        }
    }
}
