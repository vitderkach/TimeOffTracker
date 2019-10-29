using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class UserService: IUserService
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;
        public UserService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _uow = uow;
            _userManager = userManager;
            _mapper = mapper;
        }
        public IEnumerable<ApplicationUserDto> GetAllByRole(string role)
        {
            var managers = _userManager.GetUsersInRoleAsync(role).Result;
            var managersDto = _mapper.Map<IEnumerable<ApplicationUser>,
                                IEnumerable<ApplicationUserDto>>(managers);
            return managersDto;
        }
    }
}
