using System;
using TOT.Entities;

namespace TOT.Interfaces
{
    public interface ISharedService
    {
        void AddVacationDays(int employeeId, int days, int year, TimeOffType type, bool rewritePreviousStatutoryDays);
        void AddVacation(int employeeId, DateTime from, DateTime to, TimeOffType type, int fromTakenDays, int? toTakenDays);
        void AddVacationRequest(int employeeId, DateTime from, DateTime to, TimeOffType type, int stage, bool? approved, int? takenDays, bool? excelFormat);
    }
}