using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Dto
{
    public class ImportExcelFileDto
    {
        [Display(Name = "Excel file")]
        [Required]
        public IFormFile ExcelFile { get; set; }

        [Display(Name = "Sheet name")]
        [Required]
        public string SheetName { get; set; }

        [Display(Name = "Start employee index")]
        [Required]
        [Range(0, int.MaxValue)]
        public int StartEmployeeIndex { get; set; }

        [Display(Name = "End employee index")]
        [Required]
        [Range(0, int.MaxValue)]
        public int EndEmployeeIndex { get; set; }

        [Display(Name = "Employee name index")]
        [Required]
        [Range(0, int.MaxValue)]
        public int EmployeeNameIndex { get; set; }

        [Display(Name = "Employment date index")]
        [Required]
        [Range(0, int.MaxValue)]
        public int EmploymentDateIndex { get; set; }

        [Display(Name = "Location index")]
        [Range(0, int.MaxValue)]
        public int LocationIndex { get; set; }

        [Display(Name = "Team index")]
        [Range(0, int.MaxValue)]
        public int TeamIndex { get; set; }

        [Display(Name = "Start paid days index")]
        [Range(0, int.MaxValue)]
        public int StartPaidDaysIndex { get; set; }

        [Display(Name = "End paid days index")]
        [Range(0, int.MaxValue)]
        public int EndPaidDaysIndex { get; set; }

        [Display(Name = "Gift days index")]
        [Range(0, int.MaxValue)]
        public int GiftDaysIndex { get; set; }

        [Display(Name = "Weekend color")]
        public string WeekendColor { get; set; }
    }
}
