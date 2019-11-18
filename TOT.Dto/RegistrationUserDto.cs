using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class RegistrationUserDto
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Surname*")]
        public string Surname { get; set; }

        [Display(Name = "Login")]
        [StringLength(40)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(70)]
        [Display(Name = "E-mail*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
    }
}
