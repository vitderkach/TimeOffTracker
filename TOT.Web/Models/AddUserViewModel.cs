using System.ComponentModel.DataAnnotations;

namespace TOT.Web.Models
{
    public class AddUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserProfile { get; set; }
    }
}
