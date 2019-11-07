using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Entities {
    public class VacationType {
        public int Id { get; set; }
        public TimeOffType TimeOffType { get; set; }
        public int WastedDays { get; set; }

        public int VacationPolicyInfoId { get; set; }
        public VacationPolicyInfo VacationPolicyInfo { get; set; }
    }
}
