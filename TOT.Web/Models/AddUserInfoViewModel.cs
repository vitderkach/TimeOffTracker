using System.ComponentModel.DataAnnotations;

namespace TOT.Web.Models
{
    public class AddUserInfoViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
