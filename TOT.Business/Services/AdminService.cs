using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class AdminService : IAdminService
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole<int>> _roleManager;
        private IMapper _mapper;

        private readonly IUserService _userService;

        private readonly string defaultPassword = "user";

        public AdminService(RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _mapper = mapper;
        }

        // Список всех пользователей, кроме самого админа
        // Чтобы он сам себя не мог удалить. 
        public IEnumerable<ApplicationUserListDto> GetApplicationUserList()
        {
            var currentUserId = _userService.GetCurrentUser().Result.Id;
            IList<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (ApplicationUser user in _userManager.Users.Include(
                userInfo => userInfo.UserInformation))
            {
                if (user.Id == currentUserId)
                    continue;

                userList.Add(user);
            }

            var applicationUserListDto = _mapper.Map<IEnumerable<ApplicationUser>,
                IEnumerable<ApplicationUserListDto>>(userList);

            return applicationUserListDto;
        }

        // список ролей для DropDownList
        public List<IdentityRole<int>> GetApplicationRoles()
        {
            return _roleManager.Roles.ToList();
        }

        // [HttpPost] Create() Добавление нового пользователя
        public async Task<IdentityResult> RegistrationNewUser(RegistrationUserDto registrationForm)
        {
            var userRole = _roleManager.FindByIdAsync(
                registrationForm.RoleId.ToString()).Result;

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationForm.UserName,
                Email = registrationForm.Email,
                UserInformation = new UserInformation()
                {
                    FirstName = registrationForm.Name,
                    LastName = registrationForm.Surname,
                    VacationPolicyInfo = new VacationPolicyInfo()
                    {
                        TimeOffTypes = new List<VacationType>()
                        {
                            new VacationType() { TimeOffType = TimeOffType.SickLeave, WastedDays = 0 },
                            new VacationType() { TimeOffType = TimeOffType.StudyLeave, WastedDays = 0 },
                            new VacationType() { TimeOffType = TimeOffType.Vacation, WastedDays = 0 },
                            new VacationType() { TimeOffType = TimeOffType.UnpaidVacation, WastedDays = 0 }
                        }
                    }
                }
            };

            if (string.IsNullOrEmpty(user.UserName))
            {
                user.UserName = user.Email;
            }

            IdentityResult result = null;
            result = await _userManager.CreateAsync(user, defaultPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userRole.Name);
            }

            return result;
        }
    }
}
