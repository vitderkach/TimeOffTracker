using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto {

    public class VacationRequestListDto {
        public int VacationRequestId { get; set; }
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "To")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Vacation type")]
        public TimeOffType VacationType { get; set; }
        [Display(Name = "Approval")]
        public bool? Approval { get; set; }

        public UserInformationDto User { get; set; }
        
        public ICollection<ManagerResponseDto> ManagersResponses { get; set; }
        [Display(Name = "Request notes")]
        public string Notes { get; set; }
    }
}
