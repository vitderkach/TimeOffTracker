using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;

namespace TOT.Interfaces.Services
{
    public interface IUserService
    {
        IEnumerable<UserInformationDto> GetAllByRole(string role);
        Task<ApplicationUser> GetCurrentUser();
        void Dispose();
    }
}
