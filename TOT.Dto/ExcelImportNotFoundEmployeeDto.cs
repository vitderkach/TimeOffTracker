using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class ExcelImportNotFoundEmployeeDto
    {
        public string FullName { get; set; }
        public DateTime RecruitmentDate { get; set; }
        public int RowIndex { get; set; }
    }
}
