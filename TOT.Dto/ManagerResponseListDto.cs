using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class ManagerResponseListDto
    {
        public int VacationRequestId { get; set; }
        public string Employee { get; set; }

        [Display(Name = "Time off reason")]
        public string VacationType { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        public bool? Approval { get; set; }
    }
}
