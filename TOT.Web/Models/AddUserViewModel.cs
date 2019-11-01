using System.ComponentModel.DataAnnotations;

namespace TOT.Web.Models
{
    public class AddUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int UserInfoId { get; set; }
    }
}
