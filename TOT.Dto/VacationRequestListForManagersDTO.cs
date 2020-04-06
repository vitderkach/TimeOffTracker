using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestListForManagersDTO
    {
        public int VacationRequestId { get; set; }
        public int UserInformationId { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Vacation type")]
        public TimeOffType VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        public bool? ManagerApproval { get; set; }
        public bool? SelfCancelled { get; set; }
        public bool? Approval { get; set; }
    }
}
