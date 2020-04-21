using NPOI.SS.Formula;
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
        private IStorageProvider _storageProvider;
        private ExcelDataImporterConfiguration _excelConfiguration;
        private AttendanceTableConfiguration _attendanceTableConfiguration;
        public IDataImporter SetConfiguration(ExcelDataImporterConfiguration excelConfiguration, AttendanceTableConfiguration attendanceTableConfiguration)
        {
            _excelConfiguration = excelConfiguration;
            _attendanceTableConfiguration = attendanceTableConfiguration;
            return this;
        }

        public IDataImporterWithStreamProvided ImportFromStream(Stream input)
        {
            this.input = input;
            return this;
        }

        public IPopulatedDataImporter SaveToStorage(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
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

        private void ParseEmployeeVacationDays(ISheet sheet, ICell vacationDaysCell, int month, IEmployeeStorageProvider employeeStorageProvider)
        {
            int daysCount = DateTime.DaysInMonth(_excelConfiguration.Year, month);
            for (int i = 0; i < daysCount; ++i)
            {
                string value = vacationDaysCell.StringCellValue;
                if (_attendanceTableConfiguration.CellValueTimeOffTypePairs.ContainsKey(value))
                {
                    TimeOffType type = _attendanceTableConfiguration.CellValueTimeOffTypePairs.GetValueOrDefault(value);
                    int from = i, to = i;
                    int takenDays = 1;
                    for (; i < daysCount; ++i, ++takenDays)
                    {
                        if (_excelConfiguration.UseWeekendColor == true)
                        {
                            if (vacationDaysCell.CellStyle.FillBackgroundColorColor != null
                                && vacationDaysCell.CellStyle.FillBackgroundColorColor.RGB[0] == _excelConfiguration.WeekendsRGB[0]
                                && vacationDaysCell.CellStyle.FillBackgroundColorColor.RGB[1] == _excelConfiguration.WeekendsRGB[1]
                                && vacationDaysCell.CellStyle.FillBackgroundColorColor.RGB[2] == _excelConfiguration.WeekendsRGB[2])
                            {
                                vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                                takenDays--;
                                continue;
                            }
                        }

                        value = vacationDaysCell.StringCellValue;

                        if (type != _attendanceTableConfiguration.CellValueTimeOffTypePairs.GetValueOrDefault(value))
                        {
                            i--;
                            to = i;
                            takenDays--;
                            break;
                        }
                        vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                    }
                    employeeStorageProvider.AddVacation(new DateTime(_excelConfiguration.Year, month, from + 1), new DateTime(_excelConfiguration.Year, month, to + 1), type, takenDays, null);
                    employeeStorageProvider.AddVacationRequest(new DateTime(_excelConfiguration.Year, month, from + 1), new DateTime(_excelConfiguration.Year, month, to + 1), type, 4, true, takenDays, true);

                    Console.WriteLine($"Vacation Type={type}" +
                               $" From={from + 1}" +
                               $" To={to + 1}" +
                               $" Taken Days={takenDays}");
                }
                else
                {
                    vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                }
            }
        }

        private void ParseSheet(ISheet sheet)
        {
            ICell nameCell = GetCell(sheet, _excelConfiguration.NameColumnStartCell);
            ICell employmentDateCell = GetCell(sheet, _excelConfiguration.EmploymentDateColumnStartCell);
            ICell vacationDaysRowStartCell = GetCell(sheet, _excelConfiguration.VacationDaysColumnsStartCell);
            ICell giftDaysCell = null;
            ICell[] paidDaysCell = null;
            ICell locationCell = null;
            ICell teamCell = null;
            if (_excelConfiguration.OverwriteGiftAndPaidVacations)
            {
                giftDaysCell = GetCell(sheet, _excelConfiguration.GiftDaysColumnStartCell);
                paidDaysCell = new ICell[_excelConfiguration.PaidDaysDateColumnCells.Count];
                for (int i = 0; i < _excelConfiguration.PaidDaysDateColumnCells.Count; i++)
                {
                    paidDaysCell[i] = GetCell(sheet, _excelConfiguration.PaidDaysDateColumnCells.ElementAt(i));
                }
            }
            if (_excelConfiguration.OverwriteTeamAndLocation == true)
            {
                locationCell = GetCell(sheet, _excelConfiguration.LocationColumnStartCell);
                teamCell = GetCell(sheet, _excelConfiguration.TeamColumnStartCell);
            }

            for (int i = _excelConfiguration.StartColumnIndex; i <= _excelConfiguration.EndColumnIndex; i++)
            {
                string employmentDateString = employmentDateCell.CellType == CellType.String
                    ? employmentDateCell.StringCellValue
                    : employmentDateCell.DateCellValue.ToString();
                DateTime? employmentDate = null;
                bool? isFired = null;
                Console.WriteLine($"{i} Name={nameCell.StringCellValue}" +
                    $" EmploymentDate={employmentDateString}");
                if (employmentDateCell.CellType == CellType.String)
                {
                    if (employmentDateCell.StringCellValue == "исп.")
                    {
                        isFired = true;
                    }
                }
                else
                {
                    employmentDate = DateTime.Parse(employmentDateString);
                }


                IEmployeeStorageProvider employeeStorageProvider;
                if (_excelConfiguration.OverwriteTeamAndLocation == true)
                {
                    employeeStorageProvider = _storageProvider
                        .AddEmployeeAndRewriteHimTeamAndWorkplace(nameCell.StringCellValue, employmentDate, isFired, teamCell.StringCellValue, locationCell.StringCellValue);
                    locationCell = GetBottomCell(sheet, locationCell);
                    teamCell = GetBottomCell(sheet, teamCell);
                }
                else
                {
                    employeeStorageProvider = _storageProvider.AddEmployee(nameCell.StringCellValue, employmentDate, isFired);
                }

                if (_excelConfiguration.OverwriteGiftAndPaidVacations)
                {
                    int paidDays = 0;
                    employeeStorageProvider.AddVacationDays((int)giftDaysCell.NumericCellValue, _excelConfiguration.Year, TimeOffType.GiftLeave, true);
                    giftDaysCell = GetBottomCell(sheet, giftDaysCell);
                    for (int j = 0; j < paidDaysCell.Length; j++)
                    {
                        paidDays += (int)paidDaysCell[j].NumericCellValue;
                        paidDaysCell[j] = GetBottomCell(sheet, paidDaysCell[j]);
                    }
                    employeeStorageProvider.AddVacationDays(paidDays, _excelConfiguration.Year, TimeOffType.PaidLeave, true);
                }

                employeeStorageProvider.AddVacationDays(0, _excelConfiguration.Year, TimeOffType.ConfirmedSickLeave, false);
                employeeStorageProvider.AddVacationDays(0, _excelConfiguration.Year, TimeOffType.UnofficialSickLeave, false);
                employeeStorageProvider.AddVacationDays(0, _excelConfiguration.Year, TimeOffType.StudyLeave, false);
                employeeStorageProvider.AddVacationDays(0, _excelConfiguration.Year, TimeOffType.AdministrativeLeave, false);

                ParseEmployeeVacationDays(sheet, vacationDaysRowStartCell, _excelConfiguration.Month, employeeStorageProvider);

                vacationDaysRowStartCell = GetBottomCell(sheet, vacationDaysRowStartCell);
                nameCell = GetBottomCell(sheet, nameCell);
                employmentDateCell = GetBottomCell(sheet, employmentDateCell);
            }
        }

        public void Start()
        {
            IWorkbook workbook = new XSSFWorkbook(input);
            string month = new DateTime(_excelConfiguration.Year, _excelConfiguration.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            ISheet sheet = workbook.GetSheet($"{month} {_excelConfiguration.Year}");
            ParseSheet(sheet);
        }
    }
}
