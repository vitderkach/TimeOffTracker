using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TOT.DataImport.Excel;
using TOT.Dto;
namespace TOT.DataImport.Interfaces
{
    public interface IExcelMehtods
    {
        List<string> ImportExcelFile(ImportExcelFileDto importExcelFileDto);
        string GetSheetAsHtml(IFormFile excelFile, string sheetName);
        List<string> GetSheetNames(IFormFile excelFile);
        HashSet<string> GetSheetColors(IFormFile excelFile, string sheetName);
    }
}
