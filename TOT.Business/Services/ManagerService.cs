﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IUserService _userService;
        private IMapper _mapper;
        private IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVacationService _vacationService;
        private readonly IVacationEmailSender _vacationEmailSender;

        public ManagerService(IMapper mapper, IUnitOfWork uow,
            IHttpContextAccessor httpContext,
            UserManager<ApplicationUser> userManager,
            IVacationService vacationService,
            IVacationEmailSender vacationEmailSender)
            IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _vacationService = vacationService;
            _vacationEmailSender = vacationEmailSender;
            _mapper = mapper;
            _uow = uow;
        }

        // проверяет, есть ли еще менеджеры, которые должны рассмотреть заявку
        private bool CheckManagerResponsesForVacation(int vacationId)
        {
            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.Approval == null);

            if (managerResponses.Any())
                return true;

            return false;
        }
        // выберает следующий на очереди ManagerResponse для выбранной заявки
        private ManagerResponse SelectNextManager(int vacationId)
        {
            var nextResponse = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.isRequested == false
                    && r.Approval == null)
                .FirstOrDefault();

            return nextResponse;
        }

        // список всех активных запросов на имя менеджера
        public IEnumerable<ManagerResponseDto> GetAllCurrentManagerResponses()
        {
            var Id = _userService.GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.isRequested == true)
                .OrderBy(v => v.VacationRequest.StartDate);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        // мапит данные из списка запросов для вывода во View
        public IEnumerable<ManagerResponseListDto> GetCurrentManagerRequests(
            IEnumerable<ManagerResponseDto> managerResponsesDto)
        {
            var requestsToManager = _mapper.Map<IEnumerable<ManagerResponseDto>,
                IEnumerable<ManagerResponseListDto>>(managerResponsesDto);

            return requestsToManager;
        }

        // список всех уже рассмотренных запросов текущего менеджера
        public IEnumerable<ManagerResponseDto> GetProcessedRequestsByCurrentManager()
        {
            var Id = _userService.GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.Approval != null)
                .OrderByDescending(r => r.DateResponse);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        // получает ManagerResponse, которой был предварительно создан после
        // создания заявки, для текущего менеджера
        public ManagerResponseDto GetResponseByVacationId(int vacationRequestId)
        {
            ManagerResponseDto managerResponseDto = default;
            var managerId = _userService.GetCurrentUser().Result.Id;

            var managerResponse = _uow.ManagerResponseRepository.GetAll()
                .Where(x => x.VacationRequestId == vacationRequestId &&
                x.ManagerId == managerId && x.isRequested == true);

            ManagerResponse response = managerResponse.FirstOrDefault();

            if (response != null)
            {
                managerResponseDto = _mapper.Map<ManagerResponse,
                    ManagerResponseDto>(response);
            }
            return managerResponseDto;
        }

        // мапит данные выбранной менеджером заявки для
        // вывода View с Approve/Reject 
        public VacationRequestApprovalDto VacationApproval(
            ManagerResponseDto managerResponse)
        {
            var approval = _mapper.Map<ManagerResponseDto,
                VacationRequestApprovalDto>(managerResponse);

            return approval;
        }

        // действия, которые происходят после Approve/Reject
        // заявки менеджером (передача следующему на очереди менеджеру
        // или закрытие данной заявки)
        public void ApproveUserRequest(int managerResponseId,
            string managerNotes, bool approval)
        {
            var managerResponse = _uow.ManagerResponseRepository.Get(managerResponseId);
            managerResponse.Approval = approval;
            managerResponse.Notes = managerNotes;
            managerResponse.isRequested = false;
            managerResponse.DateResponse = System.DateTime.UtcNow;

            _uow.ManagerResponseRepository.Update(managerResponse);
            var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);

            //All managers reviewed vacation
            if (!CheckManagerResponsesForVacation(managerResponse.VacationRequestId))
            {
                var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
                vacation.Approval = approval;

                vacation.Approval = approval;

                EmailModel emailModel = new EmailModel()
                {
                    Body = managerNotes,
                    FullName = $"{vacation.User.UserInformation.LastName} {vacation.User.UserInformation.FirstName}",
                    To = managerResponse.Manager.Email
                };
                if(approval == true)
                {
                   _vacationService.WasteVacationDays(_mapper.Map<VacationRequest, VacationRequestDto>(vacation));
                    _vacationEmailSender.ExecuteToEmployeeAccept(emailModel);
                }
                else
                {
                    _vacationEmailSender.ExecuteToEmployeeDecline(emailModel);
                }

                _uow.VacationRequestRepository.Update(vacation);
            }
            //if vacation request has any manager responses when manager hasn't reviewed request (approval == null)
            else
            {
                EmailModel emailModel = new EmailModel()
                {
                    Body = managerNotes,
                    FullName = $"{vacation.User.UserInformation.LastName} {vacation.User.UserInformation.FirstName}",
                    To = managerResponse.Manager.Email
                };

                if (approval != false)
                {
                    var nextResponse = SelectNextManager(managerResponse.VacationRequestId);
                    nextResponse.isRequested = true;
                    _vacationEmailSender.ExecuteToManager(emailModel);
                    _uow.ManagerResponseRepository.Update(nextResponse);
                }
                else
                {
                    vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
                    vacation.Approval = false;
                    _vacationEmailSender.ExecuteToEmployeeDecline(emailModel);
                    _uow.VacationRequestRepository.Update(vacation);
                }
            }
        }
    }
}
