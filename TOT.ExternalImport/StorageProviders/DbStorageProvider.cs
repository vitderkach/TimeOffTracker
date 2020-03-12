using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;

namespace TOT.DataImport.StorageProviders
{
    class DbStorageProvider : IStorageProvider
    {
        public IEmployeeStorageProvider AddEmployeeIfNotExists(string name, DateTime? employmentDate = null, string teamName = null, string workPlace = null)
        {
            throw new NotImplementedException();
        }

        public void AddGiftDays(int employeeId, int year, int giftDays)
        {
            throw new NotImplementedException();
        }

        public void AddVacation(int employeeId, DateTime from, DateTime to, TimeOffType type)
        {
            throw new NotImplementedException();
        }

        public void AddVacationDays(int employeeId, int year, int vacationDays)
        {
            throw new NotImplementedException();
        }
    }
}
