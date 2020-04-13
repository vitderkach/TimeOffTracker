using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TOT.Entities;
using System.Text.Json.Serialization;
namespace TOT.Dto
{
    public class VacationTimelineDto
    {
        public IEnumerable<TemporalVacationRequestDto> TemporalVacationRequests { get; set; }
        public IEnumerable<ManagerResponseForTimelineDto> ManagerResponseForTimelines { get; set; }
    }

    public class TemporalVacationRequestDto{
        [JsonIgnore]
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [JsonIgnore]
        public DateTime SystemStart { get; set; }
        [JsonIgnore]
        public DateTime SystemEnd { get; set; }
        public TimeOffType VacationType { get; set; }
        public string Notes { get; set; }
        public bool? Approval { get; set; }
        [JsonIgnore]
        public int StageOfApproving { get; set; }
        [JsonIgnore]
        public DateTime ActionTime { get; set; }
        public bool? SelfCancelled { get; set; }
        public IEnumerable<UserInformationDto> Managers { get; set; }
    }
}
