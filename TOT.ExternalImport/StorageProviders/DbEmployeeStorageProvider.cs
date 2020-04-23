using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;
using TOT.Interfaces;

namespace TOT.DataImport.StorageProviders
{
    class DbEmployeeStorageProvider : IEmployeeStorageProvider
    {
        private readonly ISharedService _sharedService;
        private readonly int _employeeId;
        public DbEmployeeStorageProvider(ISharedService sharedService, int employeeId)
        {
            _sharedService = sharedService;
            _employeeId = employeeId;
        }

        public void AddVacation(DateTime from, DateTime to, TimeOffType type, int fromTakenDays, int? toTakenDays)
        {
            _sharedService.AddVacation(_employeeId, from, to, type, fromTakenDays, toTakenDays);
        }

        public void AddVacationDays(int days, int year, TimeOffType type, bool rewritePreviousStatutoryDays)
        {
            _sharedService.AddVacationDays(_employeeId, days, year, type, rewritePreviousStatutoryDays);
        }

        public void AddVacationRequest(DateTime from, DateTime to, TimeOffType type, int stage, bool? approved, int? takenDays, bool? excelFormat)
        {
            _sharedService.AddVacationRequest(_employeeId, from, to, type, stage, approved, takenDays, excelFormat);
        }
    }
}
