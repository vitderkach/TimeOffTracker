using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;

namespace TOT.DataImport.StorageProviders
{
    class InMemoryEmployeeStorageProvider : IEmployeeStorageProvider
    {
        public Employee SourceEmployee { get; set; }

        public void AddGiftDays(int year, int giftDays)
        {
            SourceEmployee.GiftDays[year] = giftDays;
        }

        public void AddVacationDays(int year, int vacationDays)
        {
            SourceEmployee.VacationDays[year] = vacationDays;
        }

        public void AddVacation(DateTime from, DateTime to, TimeOffType type)
        {
            SourceEmployee.Vacations.Add(new Vacation
                {
                    From = from,
                    To = to,
                    Type = type
                });
        }

        public class Employee
        {
            public string Name { get; set; }
            public DateTime? EmploymentDate { get; set; } = null;
            public string TeamName { get; set; } = null;
            public string WorkPlace { get; set; } = null;

            public Dictionary<int, int> GiftDays { get; set; } = new Dictionary<int, int>();
            public Dictionary<int, int> VacationDays { get; set; } = new Dictionary<int, int>();
            public List<Vacation> Vacations { get; set; } = new List<Vacation>();
        }

        public class Vacation
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public TimeOffType Type { get; set; }
        }
    }
}
