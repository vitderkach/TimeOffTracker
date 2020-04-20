using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class ReportDto
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<ReportUsedDaysDto> ReportUsedDays { get; set; }

        public int TeamId { get; set; }

        public ICollection<Team> AllTeams { get; set; }
    }
}
