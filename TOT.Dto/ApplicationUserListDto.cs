using System;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class ApplicationUserListDto
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "Login")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Registered")]
        public DateTime RegistrationDate { get; set; }

        public int UserInformationId { get; set; }
        public UserInformationDto UserInformation { get; set; }
    }
}
