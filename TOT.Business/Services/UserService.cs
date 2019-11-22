using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IHttpContextAccessor _httpContext;
        private UserManager<ApplicationUser> _userManager;
        public UserService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            _uow = uow;
            _userManager = userManager;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            return user;
            
            /*var userWithInfo = _userManager.Users.Where(u => u.Id == user.Id)
                .Include(userInfo => userInfo.UserInformation)
                .FirstOrDefaultAsync().Result;*/
        }
        public IEnumerable<ApplicationUserDto> GetAllByRole(string role)
        {
            var managers = _userManager.GetUsersInRoleAsync(role).Result;
            var userInformation = _uow.UserInformationRepository.GetAll();
            if (managers != null)
            {
                for (int i = 0; i < managers.Count; i++)
                {
                    var userInfo = userInformation
                        .Where(u => u.UserInformationId == managers[i].UserInformationId)
                        .FirstOrDefault();
                    if (userInfo != null)
                        managers[i].UserInformation = userInfo;
                }
            }
            var managersDto = _mapper.Map<IEnumerable<ApplicationUser>,
                                IEnumerable<ApplicationUserDto>>(managers);
            return managersDto;
        }
    }
}
