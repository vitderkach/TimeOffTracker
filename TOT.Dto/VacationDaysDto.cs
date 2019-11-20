using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto {
    public class VacationDaysDto {
        public ICollection<VacationTypeDto> TimeOffTypes { get; set; }
        public IEnumerable<int> MaxWastedDays { get; set; } = new List<int>() { 20, 15, 10, 20 };
    }
}
