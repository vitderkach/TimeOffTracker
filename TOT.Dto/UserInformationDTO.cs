using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class UserInformationDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registered")]
        public DateTime RecruitmentDate { get; set; }

        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;

        //public ICollection<VacationRequestDto> VacationRequests { get; set; }
        //public ICollection<ManagerResponseDto> ManagerResponses { get; set; }

    }
}
