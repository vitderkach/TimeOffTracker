using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.DataImport.Excel
{
    public struct ExcelDataImporterConfiguration
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int StartColumnIndex { get; set; }
        public int EndColumnIndex { get; set; }
        public byte[] WeekendsRGB { get; set; }
        public string NameColumnStartCell { get; set; }
        public string EmploymentDateColumnStartCell { get; set; }
        public string GiftDaysColumnStartCell { get; set; }
        public bool OverwriteTeamAndLocation { get; set; }
        public bool OverwriteGiftAndPaidVacations { get; set; }
        public bool UseWeekendColor { get; set; }
        public string VacationDaysColumnsStartCell { get; set; }
        public string TeamColumnStartCell { get; set; }
        public string LocationColumnStartCell { get; set; }
        public List<string> PaidDaysDateColumnCells { get; set; }
    }
}
