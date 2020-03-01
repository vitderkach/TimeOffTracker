using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class VacationType
    {
        public int VacationPolicyInfoId { get; set; }
        public VacationPolicy VacationPolicyInfo { get; set; }

        public TimeOffType TimeOffType { get; set; }

        public int StatutoryDays { get; set; }

        public int UsedDays { get; set; }
    }
}
