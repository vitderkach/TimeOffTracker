using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationCalendarDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public ICollection<EmployeeVacationsDto> EmployeeVacationsDtos { get; set; }
    }

    public class EmployeeVacationsDto
    {
        public UserInformationDto UserInformationDto { get; set; }
        public ICollection<VacationDateRangeDto> VacationDateRangeDtos { get; set; }
    }

    public class VacationDateRangeDto
    {
        public TimeOffType VacationType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TakenDays { get; set; }
    }
}
