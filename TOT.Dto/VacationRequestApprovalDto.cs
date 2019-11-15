using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class VacationRequestApprovalDto
    {
        public int VacationRequestId { get; set; }
        public int ManagerResponseId { get; set; }
        //[HiddenInput]
        public bool isApproval { get; set; }
        public string Employee { get; set; }


        [Display(Name = "Time off reason")]
        public string VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Request notes")]
        public string EmployeeNotes { get; set; }

        [Display(Name = "Notes to current request")]
        public string ManagerNotes { get; set; }
    }
}
