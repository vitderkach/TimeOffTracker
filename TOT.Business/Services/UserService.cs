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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;

        private bool disposed = false;

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
        }

        public IEnumerable<UserInformationDto> GetAllByRole(string role)
        {
            var users = _userManager.GetUsersInRoleAsync(role).Result;
            var usersInformation = new List<UserInformation>();
            foreach (var user in users)
            {
                usersInformation.Add(_uow.UserInformationRepository.GetOne(user.Id));
            }
            List <UserInformationDto> managersDto  = _mapper.MergeInto<List<UserInformationDto>>(usersInformation, users);
            return managersDto;
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }

                _uow.Dispose();
                _userManager.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~UserService()
        {
            CleanUp(false);
        }

    }
}
