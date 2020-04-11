using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Dto
{
    public class ComparisonTableDto
    {
        public TemporalVacationRequestDto OldVacationRequest { get; set; }
        public TemporalVacationRequestDto NewVacationRequest { get; set; }
    }
}
