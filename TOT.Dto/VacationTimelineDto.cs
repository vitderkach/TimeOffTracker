using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationTimelineDto
    {
        public IEnumerable<TemporalVacationRequest> TemporalVacationRequests { get; set; }
        public IEnumerable<ManagerResponseForTimelineDto> ManagerResponseForTimelines { get; set; }
    }

    public class TemporalVacationRequest{
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        public bool? Approval { get; set; }
        public int StageOfApproving { get; set; }
        public DateTime ActionTime { get; set; }
        public bool? SelfCancelled { get; set; }
    }
}
