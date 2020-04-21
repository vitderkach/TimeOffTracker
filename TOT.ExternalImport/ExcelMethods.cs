using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TOT.DataImport.Converters;
using TOT.DataImport.Excel;
using TOT.DataImport.Interfaces;
using TOT.DataImport.StorageProviders;
using TOT.Dto;
using TOT.Interfaces;

namespace TOT.DataImport
{
    public class ExcelMethods : IExcelMehtods
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISharedService _sharedService;
        public ExcelMethods(IUnitOfWork unitOfWork, ISharedService sharedService)
        {
            _unitOfWork = unitOfWork;
            _sharedService = sharedService;
        }
        public void ImportExcelFile(ImportExcelFileDto importExcelFileDto)
        {
            ExcelDataImporterConfiguration excelConfiguration = new ExcelDataImporterConfiguration();
            using (var excelStream = importExcelFileDto.ExcelFile.OpenReadStream())
            {
                Workbook outputWorkbook = new Workbook();
                outputWorkbook.LoadFromStream(excelStream);
                Worksheet outputSheet = (Worksheet)outputWorkbook.Worksheets.First(ws => ws.Name == importExcelFileDto.SheetName);
                //We add two for every employee edge index because two first rows in sheet are reserved for service information.
                var firstRowCellList = outputSheet.Rows[importExcelFileDto.StartEmployeeIndex+2].CellList;
                excelConfiguration.StartColumnIndex = importExcelFileDto.StartEmployeeIndex + 2;
                excelConfiguration.EndColumnIndex = importExcelFileDto.EndEmployeeIndex + 2;
                excelConfiguration.EmploymentDateColumnStartCell = firstRowCellList.ElementAt(importExcelFileDto.EmploymentDateIndex).RangeGlobalAddressWithoutSheetName.Replace("$", "");
                excelConfiguration.NameColumnStartCell = firstRowCellList.ElementAt(importExcelFileDto.EmployeeNameIndex).RangeGlobalAddressWithoutSheetName.Replace("$", "");
                excelConfiguration.VacationDaysColumnsStartCell = outputSheet.Rows[0].CellList.Where(cr => cr.Value != null && cr.Value == "1").First().RangeGlobalAddressWithoutSheetName.Split('$')[1] + (importExcelFileDto.StartEmployeeIndex + 2 + 1).ToString();
                if (importExcelFileDto.LocationIndex != 0)
                {
                    excelConfiguration.OverwriteTeamAndLocation = true;
                    excelConfiguration.LocationColumnStartCell = firstRowCellList.ElementAt(importExcelFileDto.LocationIndex).RangeGlobalAddressWithoutSheetName.Replace("$", "");
                    excelConfiguration.TeamColumnStartCell = firstRowCellList.ElementAt(importExcelFileDto.TeamIndex).RangeGlobalAddressWithoutSheetName.Replace("$", "");
                }
                if (importExcelFileDto.GiftDaysIndex != 0)
                {
                    excelConfiguration.OverwriteGiftAndPaidVacations = true;
                    excelConfiguration.PaidDaysDateColumnCells = new List<string>();
                    excelConfiguration.GiftDaysColumnStartCell = firstRowCellList.ElementAt(importExcelFileDto.GiftDaysIndex).RangeGlobalAddressWithoutSheetName.Replace("$", "");
                    for (int i = importExcelFileDto.StartPaidDaysIndex; i <= importExcelFileDto.EndPaidDaysIndex; i++)
                    {
                        excelConfiguration.PaidDaysDateColumnCells.Add(firstRowCellList.ElementAt(i).RangeGlobalAddressWithoutSheetName.Replace("$", ""));
                    }
                }
                if (importExcelFileDto.WeekendColor != null)
                {
                    excelConfiguration.UseWeekendColor = true;
                    Match matchResult = Regex.Match(importExcelFileDto.WeekendColor, "[0-9]+,[0-9]+,[0-9]+");
                    string[] colors = matchResult.Value.Split(',');
                    excelConfiguration.WeekendsRGB = new byte[3];
                    for (int i = 0; i < 3; i++)
                    {
                        excelConfiguration.WeekendsRGB[i] = byte.Parse(colors[i]);
                    }
                }

                string[] dateArray = importExcelFileDto.SheetName.Split(' ');
                MonthIntConverter converter = new MonthIntConverter("ru");
                excelConfiguration.Month = converter.ConvertFromString(dateArray[0]);
                excelConfiguration.Year = int.Parse(dateArray[1]);

            }
            using (var excelStream = importExcelFileDto.ExcelFile.OpenReadStream())
            {
                IExcelDataImporter dataImporter = new ExcelDataImporter();
                dataImporter
                    .SetConfiguration(excelConfiguration, new AttendanceTableConfiguration())
                    .ImportFromStream(excelStream)
                    .SaveToStorage(new DbStorageProvider(_unitOfWork, _sharedService))
                    .Start();
            }
        }

        public HashSet<string> GetSheetColors(IFormFile excelFile, string sheetName)
        {
            NPOI.SS.UserModel.IWorkbook workbook = null;
            HashSet<string> colorSet = new HashSet<string>();
            using (var excelStream = excelFile.OpenReadStream())
            {
                if (Path.GetExtension(excelFile.FileName) == ".xlsx")
                {
                    workbook = new XSSFWorkbook(excelStream);
                }
                else if (Path.GetExtension(excelFile.FileName) == ".xls")
                {
                    workbook = new HSSFWorkbook(excelStream);
                }
                else
                {
                    throw new ArgumentException("Incompatible file extension");
                }
                ISheet sheet = workbook.GetSheet(sheetName);

                var targetRow = sheet.GetRow(0);
                foreach (ICell cell in targetRow.Cells)
                {
                    if (cell != null && cell.CellStyle?.FillForegroundColorColor != null)
                    {
                        byte[] rgb = cell.CellStyle.FillForegroundColorColor.RGB;
                        colorSet.Add($"rgb({rgb[0]},{rgb[1]},{rgb[2]})");
                    }
                }
                return colorSet;
            }
        }

        public string GetSheetAsHtml(IFormFile excelFile, string sheetName)
        {
            int lastRowCell = 2;
            int lastColumnCell = 1;
            MemoryStream tempExcelStream = new MemoryStream();
            using (var excelStream = excelFile.OpenReadStream())
            {
                NPOI.SS.UserModel.IWorkbook workbook = null;
                if (Path.GetExtension(excelFile.FileName) == ".xlsx")
                {
                    workbook = new XSSFWorkbook(excelStream);
                }
                else if (Path.GetExtension(excelFile.FileName) == ".xls")
                {
                    workbook = new HSSFWorkbook(excelStream);
                }
                else
                {
                    throw new ArgumentException("Incompatible file extension");
                }

                ISheet sheet = workbook.GetSheet(sheetName);

                var firstRow = sheet.GetRow(0);

                for (int i = firstRow.LastCellNum - 1; i >= 0; i--)
                {
                    if (firstRow.Cells[i] != null && firstRow.Cells[i].CellType != CellType.Blank)
                    {
                        lastColumnCell = i + 1;
                        break;
                    }
                }
                int nameCellIndex = 1;
                while (true)
                {
                    if (sheet.GetRow(0).Cells[nameCellIndex].StringCellValue != "ЧИСЛО")
                    {
                        nameCellIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 3; i < sheet.LastRowNum; i++)
                {
                    ICell checkCell = sheet.GetRow(i).GetCell(nameCellIndex);
                    if (checkCell == null || checkCell.CellType == CellType.Blank)
                    {
                        break;
                    }
                    lastRowCell += 1;
                }
                for (int i = sheet.LastRowNum - 1; i > lastRowCell; i--)
                {
                    sheet.RemoveRow(sheet.GetRow(i));
                }
                sheet.ShiftRows(0, sheet.LastRowNum, 1);
                sheet.CreateRow(0);
                for (int i = 1; i <= lastColumnCell; i++)
                {
                    var newCell = sheet.GetRow(0).CreateCell(i);
                    newCell.SetCellValue(i);
                    newCell.CellStyle.Alignment = HorizontalAlignment.Center;
                }

                workbook.Write(tempExcelStream);
            }

            using (tempExcelStream)
            {
                Workbook outputWorkbook = new Workbook();
                outputWorkbook.LoadFromStream(tempExcelStream);
                Worksheet outputSheet = (Worksheet)outputWorkbook.Worksheets.First(ws => ws.Name == sheetName);
                for (int i = outputSheet.Columns.Length - 1; i > lastColumnCell; i--)
                {
                    outputSheet.DeleteColumn(i);
                }
                MemoryStream outputStream = new MemoryStream();
                outputSheet.SaveToHtml(outputStream);
                StreamReader reader = new StreamReader(outputStream);
                outputStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public List<string> GetSheetNames(IFormFile excelFile)
        {
            using (var excelStream = excelFile.OpenReadStream())
            {
                NPOI.SS.UserModel.IWorkbook workbook = null;
                if (Path.GetExtension(excelFile.FileName) == ".xlsx")
                {
                    workbook = new XSSFWorkbook(excelStream);
                }
                else if (Path.GetExtension(excelFile.FileName) == ".xls")
                {
                    workbook = new HSSFWorkbook(excelStream);
                }
                else
                {
                    throw new ArgumentException("Incompatible file extension");
                }
                List<string> sheetNames = new List<string>();
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    string SheetName = workbook.GetSheetName(i);

                    if (!string.IsNullOrEmpty(SheetName))
                    {
                        sheetNames.Add(SheetName);
                    }
                }
                return sheetNames;
            }
        }
    }
}
