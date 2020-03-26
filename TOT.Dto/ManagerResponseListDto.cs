using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class ManagerResponseListDto
    {
        public int ManagerId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Response date")]
        [DataType(DataType.Date)]
        public DateTime ResponseDate { get; set; }
        public int ForStageOfApproving { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Status")]
        public bool? Approval { get; set; }
    }
}
