using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOT.Entities;

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
        [StringLength(50)]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int? TeamId { get; set; }

        public List<IdentityRole<int>> AllRoles { get; set; }

        public ICollection<Location> AllLocations { get; set; }

        public ICollection<Team> AllTeams { get; set; }
    }
}
