using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IUserInfoService _userInfoService;
        private readonly IVacationService _vacationService;

        private readonly string defaultPassword = "user";

        public AdminService(RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager,
            IUserInfoService userInfoService, IVacationService vacationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _userInfoService = userInfoService;
            _vacationService = vacationService;
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

            ApplicationUser applicationUser;
            foreach (ApplicationUserListDto user in applicationUserListDto)
            {
                applicationUser = _mapper.Map<ApplicationUserListDto, ApplicationUser>(user);
                user.RoleName = _userManager.GetRolesAsync(applicationUser).Result.FirstOrDefault();
            }

            return applicationUserListDto;
        }

        // список ролей для DropDownList
        public List<IdentityRole<int>> GetApplicationRoles()    
        {
            return _roleManager.Roles.ToList();
        }

        private int GetRoleIdByUserId(int userId)
        {
            ApplicationUser user = _userManager.FindByIdAsync(
                userId.ToString()).Result;
            string currentRoleName = _userManager.GetRolesAsync(user)
                .Result.FirstOrDefault();

            var currentRole = _roleManager.FindByNameAsync(currentRoleName).Result;
            var currentRoleId = _roleManager.GetRoleIdAsync(currentRole).Result;

            return Convert.ToInt32(currentRoleId);
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
            try
            {
                result = await _userManager.CreateAsync(user, defaultPassword);
            }
            catch
            {
                result = IdentityResult.Failed();
                return result;
            }

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userRole.Name);
            }

            return result;
        }

        // [HttpGet] Edit/{id} Вывод данных пользователя
        public EditApplicationUserDto EditUserData(int userId)
        {
            ApplicationUser user = _userManager.FindByIdAsync(userId.ToString()).Result;
            EditApplicationUserDto editUserDto;

            if (user != null)
            {
                var userInfo = _userInfoService.GetUserInfo(user.UserInformationId);

                editUserDto = new EditApplicationUserDto()
                {
                    Id = user.Id,
                    FullName = userInfo.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    RoleId = GetRoleIdByUserId(userId),
                    AllRoles = GetApplicationRoles()
                };
            }
            else
            {
                editUserDto = null;
            }

            return editUserDto;
        }

        // [HttpPost] Edit/{id} Изменение данных пользователя (роли)
        public async Task<IdentityResult> UserDataManipulation(int userId, int newRoleId)
        {
            IdentityResult result = null;
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            var currentRoleName = _userManager.GetRolesAsync(user)
                .Result.FirstOrDefault();
            var newRoleName = _roleManager.FindByIdAsync(newRoleId.ToString()).Result.Name;

            if (!currentRoleName.Equals(newRoleName))
            {
                var removeResult =  await _userManager.RemoveFromRoleAsync(user, currentRoleName);

                if (removeResult.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, newRoleName);
                }
            }
            else
            {
                result = IdentityResult.Success;
            }
            return result;
        }

        /*В общем, чтобы удалить пользователя без конфликта в БД нужно поставить NULL
          в поле UserId таблицы VacationRequest. Из-за этого использовать метод
          DeleteVacationByUserId(userId) не получается. Можно было, конечно, удалить 
          сначала заявки, а потом пользователя. Но, если что-то пойдет не так и 
          result будет не Succeeded, то все заявки пользователя пропадут.       
          Поэтому сделал вот так */
        public async Task<IdentityResult> DeleteUser(int userId)
        {
            IdentityResult result = null;
            ApplicationUser user = _userManager.FindByIdAsync(userId.ToString()).Result;
            var userRequests = _vacationService.GetAllVacationIdsByUser(user.Id);

            if (user != null)
            {
                result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _userInfoService.DeleteUserInfo(user.UserInformationId);
                    if (userRequests.Any())
                    {
                        foreach (int vacationId in userRequests)
                        {
                            _vacationService.DeleteVacationById(vacationId);
                        }
                    }
                    //_vacationService.DeleteVacationByUserId(userId);
                }
            }
            else
            {
                result = IdentityResult.Failed();
            }
            return result;
        }
    }
}
