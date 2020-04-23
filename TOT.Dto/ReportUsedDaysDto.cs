using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class ReportUsedDaysDto
    {
        public UserInformationDto UserInformation { get; set; }

        public int UsedPaidLeaveDays { get; set; }

        public int UsedSickLeaveDays { get; set; }

        public int UsedStudyLeaveDays { get; set; }

        public int UsedAdministrativeLeaveDays { get; set; }
    }
}
