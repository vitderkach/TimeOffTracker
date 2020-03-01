using System;
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
                    .SetConfiguration(new ExcelDataImporterConfiguration())
                    .ImportFromStream(fileStream)
                    .SaveToStorage(new DbStorageProvider())
                    .Start();
            }
        }
    }
}
