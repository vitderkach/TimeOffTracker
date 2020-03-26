using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class VacationRequestDto
    {
        public ApplicationDto ApplicationDto { get; set; }

        public bool? Approval { get; set; }

        [Display(Name = "Status")]
        public int Stage { get; set; }
        [Display(Name = "Vacation type")]
        public TimeOffType VacationType { get; set; }
        public ICollection<ManagerResponseListDto> AllManagerResponses { get; set; }

        public UserInformationDto User { get; set; }
    }
}
