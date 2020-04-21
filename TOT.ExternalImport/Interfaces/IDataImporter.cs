using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TOT.DataImport.Interfaces
{
    interface IDataImporter
    {
        IDataImporterWithStreamProvided ImportFromStream(Stream input);
    }
}
