using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TOT.Entities;

namespace TOT.Dto
{
    public class UserInformationListDto
    {
        public int Id { get; set; }

        [Display(Name = "Role")]
        public IEnumerable<string> RoleNames { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registered")]
        public DateTime RecruitmentDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Location Location { get; set; }

        public Team Team { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;
    }
}
