using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class VacationRequestApprovalDto
    {
        public int VacationRequestId { get; set; }
        public int ManagerResponseId { get; set; }
        //[HiddenInput]
        [Display(Name = "Status")]
        public bool? isApproval { get; set; }
        public string Employee { get; set; }


        [Display(Name = "Vacation Type")]
        public string VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "To use more days than it has been charged is allowed")]
        public bool OverflowIsAllowed { get; set; }

        [Display(Name = "Request notes")]
        public string EmployeeNotes { get; set; }

        [Display(Name = "Notes to current request")]
        public string ManagerNotes { get; set; }
        public int EmployeeId { get; set; }
    }
}
