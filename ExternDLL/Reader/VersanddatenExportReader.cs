using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTLVersandImport.Models;

namespace JTLVersandImport.Reader
{
    interface VersanddatenExportReader
    {
        List<VersanddatenExport> ToVersanddatenExport();
    }
}
