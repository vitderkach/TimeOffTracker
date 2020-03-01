using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;

namespace TOT.Dto {
    public class VacationTypeDto {
        public int Id { get; set; }
        public string TimeOffType { get; set; }
        public int WastedDays { get; set; }

        public int VacationPolicyInfoId { get; set; }
        public VacationPolicy VacationPolicyInfo { get; set; }
    }
}
