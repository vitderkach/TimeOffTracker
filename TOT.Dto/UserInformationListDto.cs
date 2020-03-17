using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TOT.Dto
{
    public class UserInformationListDto
    {
        public int Id { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<string> RoleNames { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registered")]
        public DateTime RegistrationDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;
    }
}
