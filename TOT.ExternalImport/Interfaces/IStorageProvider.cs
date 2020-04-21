using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.DataImport.Interfaces
{
    public interface IStorageProvider
    {
        IEmployeeStorageProvider AddEmployee(
            string name,
            DateTime? employmentDate,
            bool? isFired);

        IEmployeeStorageProvider AddEmployeeAndRewriteHimTeamAndWorkplace(
    string name,
    DateTime? employmentDate,
    bool? isFired,
    string teamName,
    string workplace);
    }
}
