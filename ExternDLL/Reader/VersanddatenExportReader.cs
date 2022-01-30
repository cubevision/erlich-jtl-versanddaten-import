using System.Collections.Generic;
using JTLVersandImport.Models;

namespace JTLVersandImport.Reader
{
    interface VersanddatenExportReader
    {
        List<VersanddatenExport> ToVersanddatenExport();
    }
}
