using NPOI.HSSF.UserModel;
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
using TOT.DataImport.Exceptions;
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
            for (int i = _excelConfiguration.StartDayInMonth; i <= _excelConfiguration.EndDayInMonth; ++i)
            {
                string value = vacationDaysCell.StringCellValue;
                if (_attendanceTableConfiguration.CellValueTimeOffTypePairs.ContainsKey(value))
                {
                    TimeOffType type = _attendanceTableConfiguration.CellValueTimeOffTypePairs.GetValueOrDefault(value);
                    int from = i, to = i;
                    int takenDays = 1;
                    for (; i <= _excelConfiguration.EndDayInMonth; ++i, ++takenDays)
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
                }
                else
                {
                    vacationDaysCell = GetRightCell(sheet, vacationDaysCell);
                }
            }
        }

        private CellType GetCellType(ISheet sheet, string excelAdrress, IFormulaEvaluator evaluator)
        {
            CellType cellType = CellType.Unknown;
            ICell cell = GetCell(sheet, excelAdrress);
            if (cell.CellType == CellType.Formula)
            {
                cellType = evaluator.EvaluateFormulaCell(cell);
            }
            else
            {
                cellType = cell.CellType;
            }
            return cellType;
        }

        private List<string> ParseSheet(ISheet sheet)
        {
            List<string> errors = new List<string>();
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

                try
                {
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
                }
                catch (EmployeeNotFoundException ex)
                {
                    errors.Add(ex.Message);
                    vacationDaysRowStartCell = GetBottomCell(sheet, vacationDaysRowStartCell);
                    nameCell = GetBottomCell(sheet, nameCell);
                    employmentDateCell = GetBottomCell(sheet, employmentDateCell);
                    if (_excelConfiguration.OverwriteGiftAndPaidVacations)
                    {
                        giftDaysCell = GetBottomCell(sheet, giftDaysCell);
                        for (int j = 0; j < paidDaysCell.Length; j++)
                        {
                            paidDaysCell[j] = GetBottomCell(sheet, paidDaysCell[j]);
                        }
                    }
                    if (_excelConfiguration.OverwriteTeamAndLocation == true)
                    {
                        locationCell = GetBottomCell(sheet, locationCell);
                        teamCell = GetBottomCell(sheet, teamCell);
                    }
                    continue;
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

            return errors;
        }

        public List<string> ExexuteImport()
        {
            IWorkbook workbook = null;
            IFormulaEvaluator evaluator = null;
            if (_excelConfiguration.ExcelExtension == ".xlsx")
            {
                workbook = new XSSFWorkbook(input);
                evaluator = new XSSFFormulaEvaluator(workbook);
            }
            else if (_excelConfiguration.ExcelExtension == ".xls")
            {
                workbook = new HSSFWorkbook(input);
                evaluator = new HSSFFormulaEvaluator(workbook);
            }
            else
            {
                throw new ArgumentException("Incompatible file extension");
            }
            string month = new DateTime(_excelConfiguration.Year, _excelConfiguration.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            ISheet sheet = workbook.GetSheet($"{month} {_excelConfiguration.Year}");
            List<string> errors = new List<string>();
            if (GetCellType(sheet, _excelConfiguration.NameColumnStartCell, evaluator) != CellType.String)
            {
                errors.Add("Column name input is invalid.");
            }
            if (GetCellType(sheet, _excelConfiguration.EmploymentDateColumnStartCell, evaluator) != CellType.Numeric)
            {
                errors.Add("Column date employment input is invalid.");
            }
            if (_excelConfiguration.OverwriteGiftAndPaidVacations == true)
            {
                if (GetCellType(sheet, _excelConfiguration.GiftDaysColumnStartCell, evaluator) != CellType.Numeric)
                {
                    errors.Add("Column gift days input is invalid.");
                }
                for (int i = 0; i < _excelConfiguration.PaidDaysDateColumnCells.Count; i++)
                {
                    if (GetCellType(sheet, _excelConfiguration.PaidDaysDateColumnCells[i], evaluator) != CellType.Numeric)
                    {
                        errors.Add("Columns paid days inputs are invalid.");
                        break;
                    }
                }
            }
            if (_excelConfiguration.OverwriteTeamAndLocation == true)
            {
                if (GetCellType(sheet, _excelConfiguration.TeamColumnStartCell, evaluator) != CellType.String)
                {
                    errors.Add("Column team input is invalid.");
                }
                if (GetCellType(sheet, _excelConfiguration.LocationColumnStartCell, evaluator) != CellType.String)
                {
                    errors.Add("Column location input is invalid.");
                }
            }
            if (errors.Count != 0)
            {
                errors.Insert(0, "InputException");
                return errors;
            }
            errors = ParseSheet(sheet);
            errors.Insert(0, (errors.Count == 0) ? "The import was executed successfully." : "The import was executed, but with some errors.");
            errors.Insert(0, "EmployeeNotFoundException");
            return errors;
        }
    }
}
