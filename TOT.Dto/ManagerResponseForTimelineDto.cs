using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace TOT.Dto
{
    public class ManagerResponseForTimelineDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int VacationRequestId { get; set; }
        [Display(Name = "Manager notes")]
        public string Notes { get; set; }
        [Display(Name = "Date response")]
        public DateTime DateResponse { get; set; }
        [Range(1, 4)]
        public bool? Approval { get; set; }
        [JsonIgnore]
        public int ForStageOfApproving { get; set; }
        [JsonIgnore]
        public int ManagerId { get; set; }
        public UserInformationDto Manager { get; set; }
    }
}
