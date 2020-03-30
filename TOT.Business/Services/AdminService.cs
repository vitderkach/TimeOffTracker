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
        private readonly IVacationEmailSender _vacationEmailSender;

        private readonly string defaultPassword = "user123";

        private bool disposed = false;

        public AdminService(RoleManager<IdentityRole<int>> roleManager,
            IMapper mapper, IUserService userService,
            UserManager<ApplicationUser> userManager,
            IManagerService managerService,
            IUserInfoService userInfoService, IVacationService vacationService, IUnitOfWork unitOfWork, IVacationEmailSender vacationEmailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _userInfoService = userInfoService;
            _vacationService = vacationService;
            _managerService = managerService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _vacationEmailSender = vacationEmailSender;
        }



        // Список всех пользователей, кроме самого админа
        // Чтобы он сам себя не мог удалить. 
        public ICollection<UserInformationListDto> GetApplicationUserList()
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
                userListDto.Add(userDto);
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

                var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRoleName);

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

        public bool СhargeVacationDays(int userId, int count, TimeOffType vacationType, bool isAlreadyCharged)
        {
            if (count < 0)
            {
                throw new ArgumentException("The count of vacation days cannot be less than 0!");
            }
            var vacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.UserInformationId == userId && vt.TimeOffType == vacationType);
            if (isAlreadyCharged)
            {
                vacation.StatutoryDays = count;
                _unitOfWork.VacationTypeRepository.Update(vacation, v => v.StatutoryDays);
                return true;
            }
            else
            {
                if (vacation.StatutoryDays != 0)
                {
                    return false;
                }
                else
                {
                    vacation.StatutoryDays = count;
                    _unitOfWork.VacationTypeRepository.Update(vacation, v => v.StatutoryDays);
                    return true;
                }
            }
        }

        public void ChangeCountOfGiftdays(int userId, int count)
        {
            var giftVacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.UserInformationId == userId && vt.TimeOffType == TimeOffType.GiftLeave);
            giftVacation.StatutoryDays += count;
            _unitOfWork.VacationTypeRepository.Update(giftVacation, gv => gv.StatutoryDays);
        }

        public bool FireEmployee(int id)
        {
            var user = _unitOfWork.UserInformationRepository.GetOne(id);
            if (user is UserInformation)
            {
                user.IsFired = true;
                _unitOfWork.UserInformationRepository.Update(user, u => u.IsFired);

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

        public ManagerResponseDto GetAdminResponse(int vacationId, int userId)
        {
            ManagerResponse managerResponse = _unitOfWork.ManagerResponseRepository.GetOne(
                mr => mr.VacationRequestId == vacationId
                && mr.ManagerId == userId && mr.ForStageOfApproving == 3, mr => mr.VacationRequest);
            return _mapper.Map<ManagerResponse, ManagerResponseDto>(managerResponse);
        }

        public void AcceptVacationRequest(int vacationRequestId, string adminNotes, bool approve)
        {
            VacationRequest vacationRequest = _unitOfWork.VacationRequestRepository.GetOne(vacationRequestId);
            if (vacationRequest.StageOfApproving == 1)
            {
                ManagerResponse managerResponse = new ManagerResponse();
                managerResponse.Approval = approve;
                managerResponse.Notes = adminNotes;
                managerResponse.ForStageOfApproving = 1;
                managerResponse.Manager = _unitOfWork.UserInformationRepository.GetOne(_userService.GetCurrentUser().Result.Id);
                managerResponse.VacationRequest = vacationRequest;
                _unitOfWork.ManagerResponseRepository.Create(managerResponse);
                if (approve == true)
                {
                    vacationRequest.StageOfApproving = 2;
                    _unitOfWork.VacationRequestRepository.Update(vacationRequest, vr => vr.StageOfApproving);
                    _unitOfWork.Save();
                    foreach (var result in _unitOfWork.ManagerResponseRepository.GetAllWithVacationRequestsAndUserInfos(mr => mr.VacationRequestId == vacationRequestId && mr.ForStageOfApproving == 2).Select(mr => new { mr.ManagerId, mr.VacationRequest.Notes, mr.VacationRequest.UserInformation }))
                    {
                        UserInformation managerInformation = _unitOfWork.UserInformationRepository.GetOne(ui => ui.ApplicationUserId == result.ManagerId, ui => ui.ApplicationUser);
                        ApplicationUser manager = managerInformation.ApplicationUser;
                        EmailModel emailModel = new EmailModel()
                        {
                            To = manager.Email,
                            FullName = $"{managerInformation.FirstName}",
                            Body = $"You have a new vacation request from {result.UserInformation.FirstName} by reason of: \"{result.Notes}\""
                        };
                        _vacationEmailSender.ExecuteToManager(emailModel);
                    }
                }
                else
                {
                    vacationRequest.Approval = false;
                    _unitOfWork.VacationRequestRepository.Update(vacationRequest, vr => vr.Approval);
                    _unitOfWork.Save();
                }
            }
        }

        public void ApproveVacationRequest(int adminResponseId, string adminNotes, bool approve)
        {
            ManagerResponse managerResponse = _unitOfWork.ManagerResponseRepository.GetOne(mr => mr.Id == adminResponseId, mr => mr.VacationRequest);
            if (managerResponse.ForStageOfApproving == 3)
            {
                VacationRequest vacationRequest = managerResponse.VacationRequest;
                managerResponse.Approval = approve;
                managerResponse.Notes = adminNotes;
                _unitOfWork.ManagerResponseRepository.Update(managerResponse, mr => mr.Notes, mr => mr.Approval);
                if (approve == true)
                {
                    vacationRequest.StageOfApproving = 4;
                    vacationRequest.Approval = true;
                    _unitOfWork.VacationRequestRepository.Update(vacationRequest, vr => vr.StageOfApproving, vr => vr.Approval);
                }
                else
                {
                    vacationRequest.Approval = false;
                    _unitOfWork.VacationRequestRepository.Update(vacationRequest, vr => vr.Approval);
                }
                _unitOfWork.Save();
            }
        }

        public ICollection<VacationRequestsListForAdminsDTO> GetDefinedVacationRequestsForApproving(int userId, bool? approve)
        {
            ICollection<VacationRequest> vacationRequests = _unitOfWork.VacationRequestRepository.GetAll(vr => vr.StageOfApproving == 3 && vr.Approval == approve, vr => vr.ManagersResponses);
            vacationRequests = vacationRequests.Where(vr => vr.ManagersResponses.Where(mr => mr.ManagerId == userId).Any()).ToList();
            return ConcatVacationRequestsWithUserInfos(vacationRequests);
        }

        public ICollection<VacationRequestsListForAdminsDTO> GetApprovedVacationRequests(int userId, bool approve)
        {
            ICollection<VacationRequest> vacationRequests = _unitOfWork.VacationRequestRepository.GetAll(vr => vr.StageOfApproving == 4 && vr.Approval == approve, vr => vr.ManagersResponses);
            vacationRequests = vacationRequests.Where(vr => vr.ManagersResponses.Where(mr => mr.ManagerId == userId).Any()).ToList();
            return ConcatVacationRequestsWithUserInfos(vacationRequests);
        }

        public ICollection<VacationRequestsListForAdminsDTO> GetDefinedVacationRequestsForAcceptance(int userId, bool? approve)
        {
            ICollection<VacationRequest> vacationRequests;
            if (approve == null)
            {
                vacationRequests = _unitOfWork.VacationRequestRepository.GetAll(vr => vr.StageOfApproving == 1 && vr.Approval == approve, vr => vr.ManagersResponses);
            }
            else
            {
                vacationRequests = _unitOfWork.VacationRequestRepository.GetAll(vr => vr.StageOfApproving >= 1 && vr.StageOfApproving < 3 && vr.Approval == approve, vr => vr.ManagersResponses);
            }
            return ConcatVacationRequestsWithUserInfos(vacationRequests);
        }

        public ICollection<VacationRequestsListForAdminsDTO> GetAllVacationRequests(int userId)
        {
            ICollection<VacationRequest> vacationRequests;
            vacationRequests = _unitOfWork.VacationRequestRepository.GetAll(vr => vr.StageOfApproving == 1 && vr.Approval == null);
            vacationRequests = vacationRequests
                .Concat(_unitOfWork.VacationRequestRepository
                .GetAll(vr => vr.ManagersResponses.Where(mr => mr.ManagerId == userId && mr.Approval != null && mr.ForStageOfApproving == 1).Any(), vr => vr.ManagersResponses).ToList())
                .ToList();
            return ConcatVacationRequestsWithUserInfos(vacationRequests);
        }

        private ICollection<VacationRequestsListForAdminsDTO> ConcatVacationRequestsWithUserInfos(ICollection<VacationRequest> vacationRequests)
        {
            var userInfos = _userInfoService.GetUsersInfo();
            userInfos = userInfos.Where(ui => vacationRequests.Where(vr => vr.UserInformationId == ui.Id).Any()).ToList();
            var vacationRequestsDtos = _mapper.Map<ICollection<VacationRequest>, ICollection<VacationRequestsListForAdminsDTO>>(vacationRequests);
            for (int i = 0; i < vacationRequestsDtos.Count; i++)
            {
                var item = vacationRequestsDtos.ElementAt(i);
                var userInfo = userInfos.Where(ui => ui.Id == item.UserInformationId).FirstOrDefault();
                item.FullName = userInfo.FullName;
                item.Email = userInfo.Email;
            }
            return vacationRequestsDtos;
        }

        public void EditVacationRequest(ApplicationDto applicationDto)
        {
            VacationRequest vacationRequest = _unitOfWork.VacationRequestRepository.GetOne(vr => vr.VacationRequestId == applicationDto.Id, vr => vr.ManagersResponses);
            if (vacationRequest is VacationRequest && applicationDto.UserId == vacationRequest.UserInformationId)
            {
                vacationRequest = _mapper.Map<ApplicationDto, VacationRequest>(applicationDto, vacationRequest);
                _unitOfWork.VacationRequestRepository.Update(vacationRequest);

            }
            if (vacationRequest.StageOfApproving == 1)
            {
                List<ManagerResponse> managerResponses = _unitOfWork.ManagerResponseRepository.GetAll(mr => mr.VacationRequestId == vacationRequest.VacationRequestId && mr.ForStageOfApproving == 2).ToList();
                var userInfos = _userInfoService.GetUsersInfo();
                foreach (var email in applicationDto.RequiredManagersEmails)
                {
                    var userInfo = userInfos.Where(ui => ui.Email == email).First();
                    if (managerResponses.Where(mr => mr.Manager.ApplicationUserId == userInfo.Id).FirstOrDefault() is ManagerResponse response)
                    {
                        managerResponses.Remove(response);
                    }
                    else
                    {
                        ManagerResponse newResponse = new ManagerResponse();
                        newResponse.ForStageOfApproving = 2;
                        newResponse.VacationRequest = vacationRequest;
                        newResponse.Manager = _unitOfWork.UserInformationRepository.GetOne(userInfo.Id);
                        _unitOfWork.ManagerResponseRepository.Create(newResponse);
                    }
                }

                foreach (var item in managerResponses)
                {
                    _unitOfWork.ManagerResponseRepository.TransferToHistory(item.Id);
                }
            }
            _unitOfWork.Save();
        }

        public bool CalculateDays(int daysCount, TimeOffType timeOffType, int userId, int year)
        {
            var vacation = _unitOfWork.VacationTypeRepository.GetOne(vt => vt.UserInformationId == userId && vt.Year == year && vt.TimeOffType == timeOffType);
            if (timeOffType != TimeOffType.AdministrativeLeave || timeOffType != TimeOffType.ConfirmedSickLeave)
            {
                int allowedDays = vacation.StatutoryDays - vacation.UsedDays;
                if (daysCount > allowedDays)
                {
                    return false;
                }
                else
                {
                    vacation.UsedDays += daysCount;
                    _unitOfWork.VacationTypeRepository.Update(vacation, vt => vt.UsedDays);
                    _unitOfWork.Save();
                    return true;
                }
            }
            else
            {
                vacation.UsedDays += daysCount;
                _unitOfWork.VacationTypeRepository.Update(vacation, vt => vt.UsedDays);
                _unitOfWork.Save();
                return true;
            }

        }
        ~AdminService()
        {
            CleanUp(false);
        }
    }
}
