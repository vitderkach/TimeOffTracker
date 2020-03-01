using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TOT.DataImport.Interfaces;

namespace TOT.DataImport.Excel
{
    class ExcelDataImporter : IExcelDataImporter, IDataImporter, IDataImporterWithStreamProvided, IPopulatedDataImporter
    {
        private Stream input;
        private IStorageProvider storageProvider;
        private ExcelDataImporterConfiguration excelConfiguration;

        public IDataImporter SetConfiguration(ExcelDataImporterConfiguration excelConfiguration)
        {
            this.excelConfiguration = excelConfiguration;
            return this;
        }

        public IDataImporterWithStreamProvided ImportFromStream(Stream input)
        {
            this.input = input;
            return this;
        }

        public IPopulatedDataImporter SaveToStorage(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
            return this;
        }

        public void Start()
        {
            var workbook = new XSSFWorkbook(input);

            storageProvider.AddEmployeeIfNotExists().
            Console.WriteLine(workbook.NumberOfSheets);
            Console.WriteLine(workbook.GetSheetAt(0).SheetName);
            var sheet1 = workbook.GetSheetAt(0);
            Console.WriteLine("Cell value: " + string.Join(" ", sheet1.GetRow(6).Cells.Take(54).Select(cell => {
                if (cell.CellType == NPOI.SS.UserModel.CellType.String || cell.CellType == NPOI.SS.UserModel.CellType.Blank) {
                    return cell.StringCellValue;
                }
                return cell.NumericCellValue.ToString();
            })));
            Console.WriteLine("Importing");
        }
    }
}
