using System;
using System.Collections.Generic;
using System.IO;
using TOT.DataImport.Excel;
using TOT.DataImport.Interfaces;
using TOT.DataImport.StorageProviders;

namespace TOT.DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var newFile = @"Excel/2020.xlsx";
            using (var fileStream = new FileStream(newFile, FileMode.Open, FileAccess.Read))
            {
                IExcelDataImporter dataImporter = new ExcelDataImporter();
                dataImporter
                    .SetConfiguration(new ExcelDataImporterConfiguration
                    {
                        Year = 2020,
                        Months = new List<int> { 1 },
                        NameColumnStartCell = "G4",
                        EmploymentDateColumnStartCell = "B4",
                        GiftDaysColumnStartCell = "C4",
                        VacationDaysColumnsStartCell = "H4",
                        EntriesCount = 100
                    })
                    .ImportFromStream(fileStream)
                    .SaveToStorage(new DbStorageProvider())
                    .Start();
            }
        }
    }
}
