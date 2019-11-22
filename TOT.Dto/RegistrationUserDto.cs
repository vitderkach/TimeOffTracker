using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class RegistrationUserDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "The Name field can contain only letters.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Name field can not exceed 50 characters.")]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "The Surname field can contain only letters.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The Surname field can not exceed 50 characters.")]
        [Display(Name = "Surname*")]
        public string Surname { get; set; }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "The Login field can not exceed 40 characters.")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The E-mail field is required.")]
        [EmailAddress(ErrorMessage = "The E-mail field is not a valid e-mail address.")]
        [StringLength(70, ErrorMessage = "The E-mail field can not exceed 70 characters.")]
        [Display(Name = "E-mail*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
    }
}
