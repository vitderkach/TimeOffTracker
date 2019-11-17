using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class RegistrationUserDto
    {
        [Required]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname*")]
        public string Surname { get; set; }

        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "E-mail*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
    }
}
