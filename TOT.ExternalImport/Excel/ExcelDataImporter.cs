using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TOT.DataImport.Interfaces;
using TOT.Entities;

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

        private ICell GetCell(ISheet sheet, string address)
        {
            CellReference cellReference = new CellReference(address);
            return sheet.GetRow(cellReference.Row).GetCell(cellReference.Col);
        }

        private ICell GetBottomCell(ISheet sheet, ICell cell)
        {
            return sheet.GetRow(cell.RowIndex + 1).GetCell(cell.ColumnIndex);
        }

        private ICell GetRightCell(ISheet sheet, ICell cell)
        {
            return sheet.GetRow(cell.RowIndex).GetCell(cell.ColumnIndex + 1);
        }

        private void ParseEmployeeVacationDays(ISheet sheet, ICell vacationDaysCell, int month)
        {
            int daysCount = DateTime.DaysInMonth(excelConfiguration.Year, month);
            for (int i = 0; i < daysCount; ++i)
            {
                string value = vacationDaysCell.StringCellValue;
                if (excelConfiguration.AttendanceTableConfiguration.CellValueTimeOffTypePairs.ContainsKey(value))
                {
                    TimeOffType type = excelConfiguration.AttendanceTableConfiguration.CellValueTimeOffTypePairs.GetValueOrDefault(value);
                    int from = i, to = i;

                    for (; i < daysCount; ++i)
                    {
                        value = vacationDaysCell.StringCellValue;

                        if (type != excelConfiguration.AttendanceTableConfiguration.CellValueTimeOffTypePairs.GetValueOrDefault(value))
                        {
                            --i;
                            break;
                        }
                        to = i;
                        vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                    }

                    Console.WriteLine($"Vacation Type={type}" +
                               $" From={from + 1}" +
                               $" To={to + 1}");
                } else
                {
                    vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                }
            }
        }

        private void ParseSheet(ISheet sheet, int month)
        {
            ICell nameCell = GetCell(sheet, excelConfiguration.NameColumnStartCell);
            ICell employmentDateCell = GetCell(sheet, excelConfiguration.EmploymentDateColumnStartCell);
            ICell giftDaysCell = GetCell(sheet, excelConfiguration.GiftDaysColumnStartCell);

            ICell vacationDaysRowStartCell = GetCell(sheet, excelConfiguration.VacationDaysColumnsStartCell);

            for (int i = 0; i < excelConfiguration.EntriesCount; ++i)
            {
                string employmentDate = employmentDateCell.CellType == CellType.String
                    ? employmentDateCell.StringCellValue
                    : employmentDateCell.DateCellValue.ToString();

                Console.WriteLine($"{i} Name={nameCell.StringCellValue}" +
                    $" EmploymentDate={employmentDate}" +
                    $" GiftDays={giftDaysCell.NumericCellValue}");

                ParseEmployeeVacationDays(sheet, vacationDaysRowStartCell, month);

                vacationDaysRowStartCell = GetBottomCell(sheet, vacationDaysRowStartCell);
                nameCell = GetBottomCell(sheet, nameCell);
                employmentDateCell = GetBottomCell(sheet, employmentDateCell);
                giftDaysCell = GetBottomCell(sheet, giftDaysCell);
            }
        }

        public void Start()
        {
            IWorkbook workbook = new XSSFWorkbook(input);

            excelConfiguration.Months
                .ForEach((monthNumber) => {
                    string month = new DateTime(2020, monthNumber, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
                    ISheet sheet = workbook.GetSheet($"{month} {excelConfiguration.Year}");
                    ParseSheet(sheet, monthNumber);
                });
        }
    }
}
