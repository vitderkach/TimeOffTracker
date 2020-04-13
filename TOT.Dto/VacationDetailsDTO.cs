using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class VacationDetailsDTO
    {
        public VacationRequestDto VacationRequestDto { get; set; }
        public ManagerResponseDto ManagerResponseDto { get; set; }

        public bool? AccountWeekdays { get; set; }
        public int? HighDays { get; set; }
    }
}
