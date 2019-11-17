using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TOT.Dto
{
    public class EditApplicationUserDto
    {
        public EditApplicationUserDto()
        {
            AllRoles = new List<IdentityRole<int>>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public List<IdentityRole<int>> AllRoles { get; set; }
    }
}
