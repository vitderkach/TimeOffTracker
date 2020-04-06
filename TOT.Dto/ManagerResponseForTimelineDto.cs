using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Dto
{
    public class ManagerResponseForTimelineDto
    {
        public int Id { get; set; }
        public int VacationRequestId { get; set; }
        [Display(Name = "Manager notes")]
        public string Notes { get; set; }
        [Display(Name = "Date response")]
        public DateTime DateResponse { get; set; }
        [Range(1, 4)]
        public bool? Approval { get; set; }
        public int ForStageOfApproving { get; set; }
        public int ManagerId { get; set; }
        public UserInformationDto Manager { get; set; }

    }
}
