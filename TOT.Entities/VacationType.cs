using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities
{
    public class VacationType
    {
        public int VacationPolicyId { get; set; }
        public VacationPolicy VacationPolicy { get; set; }

        public TimeOffType TimeOffType { get; set; }

        public int StatutoryDays { get; set; }

        public int UsedDays { get; set; }
    }
}
