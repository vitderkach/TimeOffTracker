using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUserListDto> GetApplicationUserList();
        List<IdentityRole<int>> GetApplicationRoles();
    }
}
