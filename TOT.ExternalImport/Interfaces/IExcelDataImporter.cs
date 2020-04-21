using System;
using System.Collections.Generic;
using System.Text;
using TOT.DataImport.Excel;

namespace TOT.DataImport.Interfaces
{
    interface IExcelDataImporter
    {
        IDataImporter SetConfiguration(ExcelDataImporterConfiguration excelConfiguration, AttendanceTableConfiguration attendanceTableConfiguration);
    }
}
