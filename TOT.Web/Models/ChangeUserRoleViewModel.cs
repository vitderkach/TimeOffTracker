using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace TOT.Web.Models
{
    public class ChangeUserRoleViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<IdentityRole<int>> AllRoles { get; set; }
        public IList<string> RoleName { get; set; }

        public ChangeUserRoleViewModel()
        {
            AllRoles = new List<IdentityRole<int>>();
            RoleName = new List<string>();
        }
    }
}
