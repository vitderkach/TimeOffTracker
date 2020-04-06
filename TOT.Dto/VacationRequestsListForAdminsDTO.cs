using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestsListForAdminsDTO
    {
        public int VacationRequestId { get; set; }
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Vacation type")]
        public TimeOffType VacationType { get; set; }
        [Display(Name = "Stage")]
        public int StageOfApproving { get; set; }
        public int UserInformationId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public bool? SelfCancelled { get; set; }
        public string Email { get; set; }
    }
}
