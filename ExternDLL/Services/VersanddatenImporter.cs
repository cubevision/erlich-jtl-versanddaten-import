using System;
using System.Collections.Generic;
using System.Linq;
using JTLwawiExtern;
using JTLwawiExtern.VersanddatenImport;
using JTLVersandImport.Models;
using JTLVersandImport.Repository;

namespace JTLVersandImport.Services
{
    public sealed class VersanddatenImporterService
    {
        private readonly CJTLwawiExtern wawiExtern;
        private readonly LieferscheinRepository lieferscheinRepository;
        private readonly VersandRepository versandRepository;
        private readonly VersanddatenImporter versandImporter;

        public VersanddatenImporterService(string server, string datenbank, string benutzer, string passwort)
        {
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
                } else
                {
                    groupedVersanddatenExport.Add(export.BestellNr, new List<VersanddatenExport> { export });
                }
            });

            return groupedVersanddatenExport;
        }

        public void Import(List<VersanddatenExport> versanddatenexport)
        {
            var groupedVersanddatenExport = GetGroupedVersanddatenExport(versanddatenexport);
            foreach(var group in groupedVersanddatenExport)
            {
                var lieferschein = lieferscheinRepository.GetByBestellNr(group.Key);
                if (lieferschein != null)
                {
                    var existingVersandElements = versandRepository.GetByLieferscheinId(lieferschein.ID.ToString());
                    for (int i = 0, j = group.Value.Count; i < j; i++)
                    {
                        var versandExport = group.Value[i];
                        try
                        {
                            var versand = existingVersandElements.ElementAt(i);
                            versand.IdentCode = versandExport.TrackingNummer;
                            versand.VersandartId = versandExport.Spediteur;
                            versandRepository.UpdateVersand(versand);
                        } catch(ArgumentOutOfRangeException)
                        {
                            var versand = versandExport.ToVersand();
                            versand.IdentCode = versandExport.TrackingNummer;
                            versand.VersandartId = versandExport.Spediteur;
                            versand.LieferscheinId = lieferschein.ID;
                            versandRepository.InsertVersand(versand);
                        }
                    }
                    existingVersandElements = versandRepository.GetByLieferscheinId(lieferschein.ID.ToString());
                    
                    foreach(var existingVersandElement in existingVersandElements)
                    {
                        versandImporter.Add(
                            $"{lieferschein.LieferscheinNummer}${existingVersandElement.ID}",
                            existingVersandElement.Versendet ?? DateTime.Now,
                            existingVersandElement.IdentCode,
                            ""
                        );
                    }
                }
            }
            versandImporter.Apply();
        }
    }
}
