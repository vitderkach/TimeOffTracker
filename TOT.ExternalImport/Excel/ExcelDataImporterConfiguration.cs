using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.DataImport.Excel
{
    class ExcelDataImporterConfiguration
    {
        public int Year { get; set; }
        public List<int> Months { get; set; }

        public int EntriesCount { get; set; }

        public string NameColumnStartCell { get; set; }
        public string EmploymentDateColumnStartCell { get; set; }
        public string GiftDaysColumnStartCell { get; set; }

        public string VacationDaysColumnsStartCell { get; set; }

        public AttendanceTableConfiguration AttendanceTableConfiguration = new AttendanceTableConfiguration();
    }
}
