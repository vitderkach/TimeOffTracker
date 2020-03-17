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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserInfoService _userInfoService;
        private readonly IVacationService _vacationService;
        private readonly IManagerService _managerService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string defaultPassword = "user123";

        private bool disposed = false;

        public AdminService(RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager,
            IManagerService managerService,
            IUserInfoService userInfoService, IVacationService vacationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _userInfoService = userInfoService;
            _vacationService = vacationService;
            _managerService = managerService;
            _mapper = mapper;
        }

        // Список всех пользователей, кроме самого админа
        // Чтобы он сам себя не мог удалить. 
        public IEnumerable<UserInformationListDto> GetApplicationUserList()
        {
            var currentUserId = _userService.GetCurrentUser().Result.Id;
            var userList = new List<UserInformationDto>();
            foreach (UserInformationDto user in _userInfoService.GetUsersInfo())
            {
                if (user.Id == currentUserId)
                    continue;

                userList.Add(user);
            }


            List<UserInformationListDto> userListDto = new List<UserInformationListDto>();
            foreach (var user in userList)
            {
                var userDto = _mapper.Map<UserInformationDto, UserInformationListDto>(user);
                var appUser = _userManager.FindByIdAsync(user.Id.ToString()).Result;
                userDto.RoleNames = _userManager.GetRolesAsync(appUser).Result;
            }

            return userListDto;
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

            List<VacationType> vacationTypes = new List<VacationType>();

            foreach (TimeOffType type in Enum.GetValues(typeof(TimeOffType)))
            {
                vacationTypes.Add(new VacationType
                {
                    StatutoryDays = 0,
                    TimeOffType = type,
                    UsedDays = 0,
                    Year = DateTime.Now.Year
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationForm.UserName,
                Email = registrationForm.Email,
                UserInformation = new UserInformation()
                {
                    FirstName = registrationForm.Name,
                    LastName = registrationForm.Surname,
                    VacationTypes = vacationTypes
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
                result = IdentityResult.Failed(result.Errors.ToArray());
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
                if (newRoleName == "Employee" && _managerService.CheckManagerResponsesByUserId(userId))
                {
                    var error = new IdentityError();
                    error.Code = "";
                    error.Description = "You can not change the role to \"Employee\" for current user. " +
                        "User has unprocessed requests.";

                    result = IdentityResult.Failed(error);
                    return result;
                }

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


        public bool FireUser(int userId)
        {
            if (_userInfoService.GetUserInfo(userId) is UserInformationDto userInformation)
            {
                _userInfoService.FireUserInfo(userInformation.Id);

                var vacantionIds = _vacationService.GetAllVacationIdsByUser(userId);

                foreach (int vacationId in vacantionIds)
                {
                    _vacationService.DeactivateVacation(vacationId);
                }

                return true;
            }

            return false;
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }

                _userManager.Dispose();
                _userService.Dispose();
                _vacationService.Dispose();
                _userInfoService.Dispose();
                _unitOfWork.Dispose();
                _roleManager.Dispose();
                _managerService.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~AdminService()
        {
            CleanUp(false);
        }
    }
}
