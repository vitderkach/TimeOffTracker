using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.DataImport.Interfaces
{
    public interface IStorageProvider
    {
        IEmployeeStorageProvider AddEmployeeIfNotExists(
            string name,
            DateTime? employmentDate = null,
            string teamName = null,
            string workPlace = null);
    }
}
