﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using JTLVersandImport.Models;
using JTLVersandImport.Services;

namespace JTLVersandImport.Reader
{
    public abstract class AbstractReader : VersanddatenExportReader
    {
        protected readonly Stream stream;
        protected readonly Config config;

        public AbstractReader(Stream stream, Config config)
        {
            this.stream = stream;
            this.config = config;
        }

        public abstract List<VersanddatenExport> ToVersanddatenExport();

        protected string CleanTrackingNumber(string trackingNumber)
        {
            return TrackingNumberCorrector.Clean(trackingNumber);
        }

        protected int GetMappedCarrier(string carrier)
        {
            return config.Versand.First((Versandart) => Versandart.Spediteur.Equals(carrier)).VersandartId;
        }
    }
}
