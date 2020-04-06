﻿using Microsoft.AspNetCore.Identity;
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
            var vacationDto = _mapper.Map<VacationRequest, VacationRequestDto>(vacation);
            vacationDto.User.Email = vacation.UserInformation.ApplicationUser.Email;
            foreach (var response in vacationDto.AllManagerResponses)
            {
                var userDto = usersListDto.First(ud => ud.Id == response.ManagerId);
                response.FullName = userDto.FullName;
                response.Email = userDto.Email;
            }
            foreach (var item in vacationDto.AllManagerResponses)
            {
                vacationDto.ApplicationDto.RequiredManagersEmails.Add(item.Email);
            }
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

        public bool CancelVacation(int vacationRequestId)
        {
            var vacationRequest = _uow.VacationRequestRepository.GetOne(vacationRequestId);
            if (vacationRequest is VacationRequest && vacationRequest.Approval != true)
            {
                vacationRequest.SelfCancelled = true;
                _uow.VacationRequestRepository.Update(vacationRequest, vr => vr.SelfCancelled);
                _uow.Save();
                return true;
            }
            return false;
        }

        public void EditVacationRequest(ApplicationDto applicationDto)
        {
            VacationRequest vacationRequest = _uow.VacationRequestRepository.GetOne(vr => vr.VacationRequestId == applicationDto.Id, vr => vr.ManagersResponses);
            if (vacationRequest is VacationRequest && applicationDto.UserId == vacationRequest.UserInformationId)
            {
                vacationRequest = _mapper.Map<ApplicationDto, VacationRequest>(applicationDto, vacationRequest);
                _uow.VacationRequestRepository.Update(vacationRequest);

            }
            if (vacationRequest.StageOfApproving == 1)
            {
                List<ManagerResponse> managerResponses = _uow.ManagerResponseRepository.GetAll(mr => mr.VacationRequestId == vacationRequest.VacationRequestId && mr.ForStageOfApproving == 2).ToList();
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
                        newResponse.Manager = _uow.UserInformationRepository.GetOne(userInfo.Id);
                        _uow.ManagerResponseRepository.Create(newResponse);
                    }
                }

                foreach (var item in managerResponses)
                {
                    _uow.ManagerResponseRepository.TransferToHistory(item.Id);
                }
            }
            _uow.Save();
        }

        public VacationTimelineDto GetVacationTimeline(int vacationRequestId)
        {
            var vacationRequestHistories = _uow.VacationRequestRepository.GetHistoryForOne(vacationRequestId).OrderBy(vrh => vrh.SystemEnd).ToList();
            var managerResponses = _uow.ManagerResponseRepository.GetAll(mr => mr.VacationRequestId == vacationRequestId);
            VacationTimelineDto vacationTimelineDto = new VacationTimelineDto();
            vacationTimelineDto.TemporalVacationRequests = _mapper.Map<IEnumerable<VacationRequestHistory>, IEnumerable<TemporalVacationRequest>>(vacationRequestHistories);
            vacationTimelineDto.ManagerResponseForTimelines = _mapper.Map<IEnumerable<ManagerResponse>, IEnumerable<ManagerResponseForTimelineDto>>(managerResponses);
            foreach (var item in vacationTimelineDto.ManagerResponseForTimelines)
            {
                item.Manager = _userInfoService.GetUserInfo(item.ManagerId);
            }
            return vacationTimelineDto;
        }

        ~VacationService()
        {
            CleanUp(false);
        }
    }
}
