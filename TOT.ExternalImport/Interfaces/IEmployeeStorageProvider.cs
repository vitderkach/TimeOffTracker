using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.DataImport.Interfaces
{
    public interface IEmployeeStorageProvider
    {
        void AddVacationDays(int days, int year, TimeOffType type, bool rewritePreviousStatutoryDays);
        void AddVacation(DateTime from, DateTime to, TimeOffType type, int fromTakenDays, int? toTakenDays);
        void AddVacationRequest(DateTime from, DateTime to, TimeOffType type, int stage, bool? approved, int? takenDays, bool? excelFormat);
    }
}
