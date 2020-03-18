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
    public class ManagerService : IManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IVacationEmailSender _vacationEmailSender;

        private bool disposed = false;

        public ManagerService(IMapper mapper, IUnitOfWork uow,
            IVacationService vacationService,
            IVacationEmailSender vacationEmailSender,
            IUserService userService)
        {
            _userService = userService;
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

        public bool CheckManagerResponsesByUserId(int userId)
        {
            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(u => u.ManagerId == userId && u.Approval == null);

            if (managerResponses.Any())
                return true;

            return false;
        }

        public IEnumerable<ManagerResponseListDto> GetAllMyManagerResponses()
        {
            var Id = _userService.GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id)
                .OrderBy(v => v.VacationRequest.StartDate);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            var managerResponsesListDto = _mapper.Map<IEnumerable<ManagerResponseDto>,
                IEnumerable<ManagerResponseListDto>>(managerResponsesDto);
            return managerResponsesListDto;
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
        public ManagerResponseDto GetResponseActiveByVacationId(int vacationRequestId)
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
        public ManagerResponseDto GetResponseByVacationId(int vacationRequestId)
        {
            ManagerResponseDto managerResponseDto = default;
            var managerId = _userService.GetCurrentUser().Result.Id;

            var managerResponse = _uow.ManagerResponseRepository.GetAll()
                .Where(x => x.VacationRequestId == vacationRequestId &&
                x.ManagerId == managerId);

            ManagerResponse response = managerResponse.FirstOrDefault();

            if (response != null)
            {
                managerResponseDto = _mapper.Map<ManagerResponse, ManagerResponseDto>(response);
            }
            return managerResponseDto;
        }
        // мапит данные выбранной менеджером заявки для
        // вывода View с Approve/Reject 
        public VacationRequestApprovalDto VacationApproval(
            ManagerResponseDto managerResponse)
        {
            var approval = _mapper.Map<ManagerResponseDto, VacationRequestApprovalDto>(managerResponse);

            return approval;
        }

        public void ApproveUserRequest(int managerResponseId,
            string managerNotes, bool? approval, bool overflowIsAllowed = false)
        {
            var managerResponse = _uow.ManagerResponseRepository.GetOne(managerResponseId);
            managerResponse.Approval = approval;
            managerResponse.Notes = managerNotes;
            managerResponse.isRequested = false;
            managerResponse.DateResponse = System.DateTime.UtcNow;

            _uow.ManagerResponseRepository.Update(managerResponse);
            var vacation = _uow.VacationRequestRepository.GetOne(managerResponse.VacationRequestId);

            EmailModel emailModel = new EmailModel()
            {
                Body = vacation.Notes,
                FullName = $"{vacation.UserInformation.LastName} {vacation.UserInformation.FirstName}",
                To = vacation.UserInformation.ApplicationUser.Email
            };
            //All managers reviewed vacation
            if (!CheckManagerResponsesForVacation(managerResponse.VacationRequestId))
            {

                vacation.Approval = approval;

                if (approval == true)
                {
                    _vacationEmailSender.ExecuteToEmployeeAccept(emailModel);

                    CalculateVacationDays(vacation, overflowIsAllowed);
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
                if (approval != false)
                {
                    var nextResponse = SelectNextManager(managerResponse.VacationRequestId);
                    emailModel.To = nextResponse.Manager.ApplicationUser.Email;
                    nextResponse.isRequested = true;
                    _vacationEmailSender.ExecuteToManager(emailModel);
                    _uow.ManagerResponseRepository.Update(nextResponse);
                }
                else
                {
                    vacation = _uow.VacationRequestRepository.GetOne(managerResponse.VacationRequestId);
                    vacation.Approval = false;
                    _vacationEmailSender.ExecuteToEmployeeDecline(emailModel);
                    _uow.VacationRequestRepository.Update(vacation);
                }
            }

            _uow.Save();
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

                disposed = true;
            }
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~ManagerService()
        {
            CleanUp(false);
        }
    }
}
