using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUserListDto> GetApplicationUserList();
        List<IdentityRole<int>> GetApplicationRoles();
        Task<IdentityResult> RegistrationNewUser(RegistrationUserDto registrationForm);
        EditApplicationUserDto EditUserData(int userId);
        Task<IdentityResult> UserDataManipulation(int userId, int userRoleId);
        bool FireUser(int userId);
    }
}
