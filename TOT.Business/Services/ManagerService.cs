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
    public class ManagerService : IManagerService {
        private IMapper _mapper;
        private IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IVacationEmailSender _vacationEmailSender;

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
        //TODO: Fix the method
        public void ApproveUserRequest(int managerResponseId,
            string managerNotes, bool? approval)
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

                    CalculateVacationDays(vacation);
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
        }

        // TODO: Rewrite the method because the database logic has been changed. As an example the commented code below

        private void CalculateVacationDays(VacationRequest vacationRequestDto)
        {
            throw new NotImplementedException();
        }

        //#region CaluclateVacationDays
        //private void CalculateVacationDays(VacationRequest vacationRequestDto)
        //{
        //    var vacationPolicy = _uow.VacationPolicyRepository.GetAll()
        //        .Where(v => v.UserInformationId == vacationRequestDto.ApplicationUser.UserInformationId)
        //        .FirstOrDefault();

        //    for (int i = 0; i < vacationPolicy.VacationTypes.Count; i++)
        //    {
        //        var timeType = vacationPolicy.VacationTypes.ElementAt(i).TimeOffType;
        //        if (vacationPolicy.VacationTypes.ElementAt(i).TimeOffType ==
        //            vacationRequestDto.VacationType)
        //        {

        //            int wastedDays = (int)(vacationRequestDto.EndDate - vacationRequestDto.StartDate).TotalDays + 1;

        //            switch (timeType)
        //            {
        //                case TimeOffType.ConfirmedSickLeave:
        //                {
        //                    int currentWastedDays = vacationPolicy.VacationTypes.ElementAt(i).UsedDays + wastedDays;
        //                    if (currentWastedDays <= 15)
        //                    {
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays = currentWastedDays;
        //                    }
        //                    else
        //                    {
        //                        var daysToFull = 15 - vacationPolicy.VacationTypes.ElementAt(i).UsedDays;
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays += daysToFull;
        //                        currentWastedDays = wastedDays - daysToFull;

        //                        var paidVacation = vacationPolicy.VacationTypes
        //                            .Where(t => t.TimeOffType == TimeOffType.PaidLeave).FirstOrDefault();

        //                        var predictVacationDays = paidVacation.UsedDays + currentWastedDays;

        //                        if (predictVacationDays <= 15)
        //                        {
        //                            vacationPolicy.VacationTypes.Where(t => t.TimeOffType == TimeOffType.PaidLeave).FirstOrDefault().UsedDays = predictVacationDays;
        //                        }
        //                        else
        //                        {
        //                            daysToFull = 15 - paidVacation.UsedDays;
        //                            paidVacation.UsedDays = 15;
        //                            currentWastedDays = currentWastedDays - daysToFull;

        //                            if (currentWastedDays > 0)
        //                            {
        //                                paidVacation = vacationPolicy.VacationTypes
        //                                    .Where(t => t.TimeOffType == TimeOffType.AdministrativeLeave).FirstOrDefault();

        //                                paidVacation.UsedDays += currentWastedDays;
        //                            }
        //                            vacationPolicy.VacationTypes
        //                                 .Where(t => t.TimeOffType == TimeOffType.AdministrativeLeave).FirstOrDefault()
        //                                 .UsedDays = paidVacation.UsedDays;
        //                        }
        //                    }
        //                    break;
        //                }
        //                case TimeOffType.StudyLeave:
        //                {
        //                    int currentWastedDays = vacationPolicy.VacationTypes.ElementAt(i).UsedDays + wastedDays;
        //                    if (currentWastedDays <= 10)
        //                    {
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays = currentWastedDays;
        //                    }
        //                    else
        //                    {
        //                        var daysToFull = 10 - vacationPolicy.VacationTypes.ElementAt(i).UsedDays;
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays += daysToFull;
        //                        currentWastedDays = wastedDays - daysToFull;

        //                        var paidVacation = vacationPolicy.VacationTypes
        //                            .Where(t => t.TimeOffType == TimeOffType.PaidLeave).FirstOrDefault();

        //                        var predictVacationDays = paidVacation.UsedDays + currentWastedDays;

        //                        if (predictVacationDays <= 15)
        //                        {
        //                            vacationPolicy.VacationTypes.Where(t => t.TimeOffType == TimeOffType.StudyLeave).FirstOrDefault().UsedDays = predictVacationDays;
        //                        }
        //                        else
        //                        {
        //                            daysToFull = 15 - paidVacation.UsedDays;
        //                            paidVacation.UsedDays = 15;
        //                            currentWastedDays = currentWastedDays - daysToFull;

        //                            if (currentWastedDays > 0)
        //                            {
        //                                paidVacation = vacationPolicy.VacationTypes
        //                                    .Where(t => t.TimeOffType == TimeOffType.AdministrativeLeave).FirstOrDefault();

        //                                paidVacation.UsedDays += currentWastedDays;
        //                            }
        //                            vacationPolicy.VacationTypes
        //                                 .Where(t => t.TimeOffType == TimeOffType.AdministrativeLeave).FirstOrDefault()
        //                                 .UsedDays = paidVacation.UsedDays;
        //                        }
        //                    }
        //                    break;
        //                }
        //                case TimeOffType.AdministrativeLeave:
        //                {
        //                    int currentWastedDays = vacationPolicy.VacationTypes.ElementAt(i).UsedDays + wastedDays;
        //                    vacationPolicy.VacationTypes.ElementAt(i).UsedDays = currentWastedDays;
        //                    break;
        //                }
        //                case TimeOffType.PaidLeave:
        //                {
        //                    int currentWastedDays = vacationPolicy.VacationTypes.ElementAt(i).UsedDays + wastedDays;
        //                    if (currentWastedDays <= 15)
        //                    {
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays = currentWastedDays;
        //                    }
        //                    else
        //                    {
        //                        var daysToFull = 15 - vacationPolicy.VacationTypes.ElementAt(i).UsedDays;
        //                        vacationPolicy.VacationTypes.ElementAt(i).UsedDays += daysToFull;
        //                        currentWastedDays = wastedDays - daysToFull;

        //                        vacationPolicy.VacationTypes
        //                                .Where(t => t.TimeOffType == TimeOffType.AdministrativeLeave)
        //                                .FirstOrDefault()
        //                                .UsedDays += currentWastedDays;
        //                    }
        //                    break;
        //                }
        //            }

        //        }
        //    }
        //    _uow.VacationPolicyRepository.Update(vacationPolicy);
        //}
        //#endregion
    }
}
