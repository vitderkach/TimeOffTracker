using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestDto
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
        public string VacationTypeString { get; set; }
        [Display(Name = "Request notes")]
        public string Notes { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Managers")]
        public IList<int> SelectedManager { get; set; }
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }
        [Display(Name = "Status")]
        public bool? Approval { get; set; }
        public int UserId { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
