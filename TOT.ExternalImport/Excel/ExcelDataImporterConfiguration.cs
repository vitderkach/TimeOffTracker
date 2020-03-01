using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.DataImport.Excel
{
    class ExcelDataImporterConfiguration
    {
        public int Year { get; set; }
        public List<int> Month { get; set; }

        public int EntriesCount { get; set; }

        public string NameColumnStartCell { get; set; }
        public string EmploymentDateColumnStartCell { get; set; }
        public string GiftDaysColumnStartCell { get; set; }

        public Dictionary<int, string> VacationDaysColumnsStartCells { get; set; }

        public AttendanceTableConfiguration AttendanceTableConfiguration = new AttendanceTableConfiguration();
    }
}
