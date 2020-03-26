using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using TOT.Data.Extensions;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class VacationService : IVacationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IVacationEmailSender _vacationEmailSender;
        private readonly IUserService _userService;
        private readonly IUserInfoService _userInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<Resources.Resources> _localizer;
        private bool disposed = false;

        public VacationService(IMapper mapper, IUnitOfWork uow,
            IVacationEmailSender vacationEmailSender,
            IUserService userService, UserManager<ApplicationUser> userManager, IStringLocalizer<Resources.Resources> localizer,
            IUserInfoService userInfoService)
        {
            _mapper = mapper;
            _uow = uow;
            _userService = userService;
            _vacationEmailSender = vacationEmailSender;
            _userManager = userManager;
            _localizer = localizer;
            _userInfoService = userInfoService;
        }

        public void ApplyForVacation(ApplicationDto applicationDto)
        {
            var vacation = _mapper.Map<ApplicationDto, VacationRequest>(applicationDto);
            vacation.StageOfApproving = 1;
            _uow.VacationRequestRepository.Create(vacation);
            foreach (var managerEmail in applicationDto.RequiredManagersEmails)
            {
                var manager = _userManager.FindByEmailAsync(managerEmail).Result;
                ManagerResponse managerResponse = new ManagerResponse()
                {
                    ManagerId = manager.Id,
                    VacationRequest = vacation,
                    ForStageOfApproving = 2
                };
                _uow.ManagerResponseRepository.Create(managerResponse);
            }
            _uow.Save();
        }

        public VacationDaysDto GetVacationDays(int userId)
        {
            var vacationDays = _uow.UserInformationRepository.GetOne(ui => ui.ApplicationUserId == userId, ui => ui.VacationTypes).VacationTypes.Where(vt => vt.Year == DateTime.Now.Year).ToList();

            VacationDaysDto vacationDaysDto = new VacationDaysDto();
            _mapper.Map<IEnumerable<VacationType>, VacationDaysDto>(vacationDays, vacationDaysDto);
            return vacationDaysDto;
        }

        public List<int> GetAllVacationIdsByUser(int userId)
        {
            List<int> vacationIds = new List<int>();
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserInformationId == userId);

            foreach (VacationRequest request in vacations)
            {
                vacationIds.Add(request.VacationRequestId);
            }

            return vacationIds;
        }

        public IEnumerable<VacationRequestListDto> GetAllByCurrentUser()
        {
            var currentUserId = _userService.GetCurrentUser().Result.Id;
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserInformationId == currentUserId);

            var vacationsDto = _mapper.Map<IEnumerable<VacationRequest>, IEnumerable<VacationRequestListDto>>(vacations);
            return vacationsDto;
        }

        public VacationRequestDto GetVacationById(int id)
        {
            var vacation = _uow.VacationRequestRepository.GetOne(vr => vr.VacationRequestId == id, vr=> vr.ManagersResponses);
            var usersListDto = _userInfoService.GetUsersInfo();
            usersListDto = usersListDto.Where(uid => vacation.ManagersResponses.Where(mr => mr.ManagerId == uid.Id).Any());
            var vacationDto = _mapper.Map<VacationRequest, VacationRequestDto>(vacation);
            vacationDto.User.Email = vacation.UserInformation.ApplicationUser.Email;
            vacationDto.AllManagerResponses = _mapper.Map<IEnumerable<UserInformationDto>, ICollection<ManagerResponseListDto>>(usersListDto, vacationDto.AllManagerResponses);
            return vacationDto;
        }
        public void UpdateVacation(int id, string notes)
        {
            var vacation = _uow.VacationRequestRepository.GetOne(id);
            if (vacation != null)
            {
                vacation.Notes = notes;
                _uow.VacationRequestRepository.Update(vacation);
                _uow.Save();
            }
        }

        public bool DeactivateVacation(int id)
        {
            if (_uow.VacationRequestRepository.GetOne(id) is VacationRequest vacationRequest && vacationRequest.Approval == null)
            {
                _uow.VacationRequestRepository.TransferToHistory(id);
                _uow.Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CalculateVacationDays(VacationRequest vacationRequest, bool overflowIsAllowed)
        {
            int requestedUsedDays = (vacationRequest.EndDate - vacationRequest.StartDate).Days;

            if (requestedUsedDays < 0)
            {
                throw new ArgumentException("The count of used days cannot be less than 0!");
            }

            int userId = vacationRequest.UserInformationId;
            TimeOffType vacationType = vacationRequest.VacationType;
            var vacation = _uow.VacationTypeRepository.GetOne(vt => vt.Id == userId && vt.TimeOffType == vacationType);

            if (overflowIsAllowed)
            {
                vacation.UsedDays += requestedUsedDays;
                _uow.VacationTypeRepository.Update(vacation, v => v.UsedDays);
                _uow.Save();

                return true;
            }
            else
            {
                int allowedToUseDays = vacation.StatutoryDays - vacation.UsedDays;
                if (allowedToUseDays < requestedUsedDays)
                {
                    return false;
                }
                else
                {
                    vacation.UsedDays += requestedUsedDays;
                    _uow.VacationTypeRepository.Update(vacation, v => v.UsedDays);
                    _uow.Save();

                    return true;
                }
            }
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }
                _userService.Dispose();
                _uow.Dispose();
                _userManager.Dispose();
                _userInfoService.Dispose();
                disposed = true;
            }
        }

        public SelectList GetTimeOffTypeList()
        {
            var list = new List<SelectListItem>();
            foreach (TimeOffType item in Enum.GetValues(typeof(TimeOffType)))
            {
                list.Add(new SelectListItem
                {
                    Text = _localizer[item.GetDescription()],
                    Value = item.ToString()
                });
            }
            list = list.OrderBy(x => x.Text).ToList();
            return new SelectList(list, "Value", "Text");
        }

        public SelectList GetManagersForVacationApply(int userId)
        {
            List<UserInformationDto> excitingManagers =
                _userService
                .GetAllByRole("Manager")
                .Where(uid => uid.Id != userId)
                .OrderBy(uid => uid.FullName)
                .ToList();
            return new SelectList(excitingManagers, "Email", "FullName");
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~VacationService()
        {
            CleanUp(false);
        }
    }
}
