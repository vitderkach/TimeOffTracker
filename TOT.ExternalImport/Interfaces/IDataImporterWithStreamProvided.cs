using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.DataImport.Interfaces
{
    interface IDataImporterWithStreamProvided
    {
        IPopulatedDataImporter SaveToStorage(IStorageProvider storageProvider);
    }
}
