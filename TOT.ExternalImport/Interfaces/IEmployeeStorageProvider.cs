using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.DataImport.Interfaces
{
    interface IEmployeeStorageProvider
    {   
        void AddGiftDays(int year, int giftDays);
        public void AddVacationDays(int year, int vacationDays);

        void AddVacation(DateTime from, DateTime to, TimeOffType type);
    }
}
