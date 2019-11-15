using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class VacationService: IVacationService
    {
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;
        private IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVacationEmailSender _vacationEmailSender;
        private readonly IUserService _userService;

        public VacationService(IMapper mapper, IUnitOfWork uow,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContext,
            IVacationEmailSender vacationEmailSender,
            IUserService userService)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _httpContext = httpContext;
            _userService = userService;
            _vacationEmailSender = vacationEmailSender;
        }
        public void ApplyForVacation(VacationRequestDto vacationRequestDto)
        {
            //Approve(vacationRequestDto);
            var vacation = _mapper.Map<VacationRequestDto, VacationRequest>(vacationRequestDto);
            //todo send to e-mail that vacation is registred
            for (int i=0; i < vacationRequestDto.SelectedManager.Count; i++)
            {

                vacation.ManagersResponses.Add(new ManagerResponse()
                {
                    ManagerId = vacationRequestDto.SelectedManager[i],
                    isRequested = i == 0 ? true : false,
                });
            }
            _uow.VacationRequestRepository.Create(vacation);
            _uow.Save();

            var user = _userService.GetCurrentUser().Result;
            var userInfo = _uow.UserInformationRepository.GetAll().Where(u => u.User.Id == user.Id).FirstOrDefault();

            EmailModel emailModel = new EmailModel()
            {
                To = vacation.ManagersResponses.ElementAt(0).Manager.Email,
                FullName = $"{userInfo.LastName} {userInfo.FirstName}",
                Body = vacation.Notes
            };
            _vacationEmailSender.ExecuteToManager(emailModel);
        }
        public void DeleteVacationByUserId(int id)
        {
            //need to fix when another will be ready
            var vacationsRemoved = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserId == id);
            foreach(var removed in vacationsRemoved)
            {
                _uow.VacationRequestRepository.Delete(removed.VacationRequestId);
            }
        }

        public IEnumerable<VacationRequestListDto> GetAllByCurrentUser()
        {
            var currentUser = getCurrentUser();
            var currentUserId = currentUser.Result.Id;
            var vacations = _uow.VacationRequestRepository.GetAll()
                .Where(v => v.UserId == currentUserId);

            var vacationsDto = _mapper.Map<IEnumerable<VacationRequest>, IEnumerable<VacationRequestListDto>>(vacations);
            
            return vacationsDto;
        }
        private async Task<ApplicationUser> getCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            return user;
        }

        public VacationRequestDto GetVacationById(int id)
        {
            var vacation = _uow.VacationRequestRepository.Get(id);
            var vacationDto = _mapper.Map<VacationRequest, VacationRequestDto>(vacation);

            return vacationDto;
        }
        public void UpdateVacation(int id, string notes)
        {
            var vacation = _uow.VacationRequestRepository.Get(id);
            vacation.Notes = notes;
            _uow.VacationRequestRepository.Update(vacation);
        }

        public bool DeleteVacation(int id)
        {
            var vacation = _uow.VacationRequestRepository.Get(id);
            if(vacation.Approval == null)
            {
                _uow.VacationRequestRepository.Delete(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        //need to refactor
        public void Approve(VacationRequestDto vacationRequestDto)
        {
            var userInfo = _uow.UserInformationRepository
                .GetAll()
                .Where(u => u.User.Id == vacationRequestDto.UserId)
                .FirstOrDefault();

            for (int i = 0; i < userInfo.VacationPolicyInfo.TimeOffTypes.Count; i++)
            {
                var timeType = userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).TimeOffType;
                if (userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).TimeOffType ==
                    vacationRequestDto.VacationType)
                {

                    int wastedDays = (int)(vacationRequestDto.EndDate - vacationRequestDto.StartDate).TotalDays + 1;

                    switch (timeType)
                    {
                        case TimeOffType.SickLeave:
                        {
                            int currentWastedDays = userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays + wastedDays;
                            if (currentWastedDays <= 20)
                            {
                                userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays = currentWastedDays;
                            }
                            else
                            {
                                currentWastedDays -= 20;
                                var daysToFull = 20 - userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays;
                                userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays += daysToFull;
                                //currentWastedDays -= daysToFull;

                                var paidVacation = userInfo.VacationPolicyInfo.TimeOffTypes
                                    .Where(t => t.TimeOffType == TimeOffType.Vacation).FirstOrDefault();

                                var predictVacationDays = paidVacation.WastedDays + currentWastedDays;

                                if (predictVacationDays <= 15)
                                {
                                    paidVacation.WastedDays = predictVacationDays;
                                }
                                else
                                {
                                    daysToFull = 15 - paidVacation.WastedDays;
                                    paidVacation.WastedDays = 15;
                                    currentWastedDays -= daysToFull;
                                    _uow.VacationTypeRepository.Update(paidVacation);

                                    if (currentWastedDays > 0)
                                    {
                                        paidVacation = userInfo.VacationPolicyInfo.TimeOffTypes
                                            .Where(t => t.TimeOffType == TimeOffType.UnpaidVacation).FirstOrDefault();

                                        paidVacation.WastedDays += currentWastedDays;
                                        _uow.VacationTypeRepository.Update(paidVacation);
                                    }
                                }
                            }
                            break;
                        }
                        case TimeOffType.StudyLeave:
                        {
                            int currentWastedDays = userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays + wastedDays;
                            if (currentWastedDays <= 10)
                            {
                                userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays = currentWastedDays;
                            }
                            else
                            {
                                currentWastedDays -= 10;
                                var daysToFull = 10 - userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays;
                                userInfo.VacationPolicyInfo.TimeOffTypes.ElementAt(i).WastedDays += daysToFull;
                                currentWastedDays -= daysToFull;

                                var paidVacation = userInfo.VacationPolicyInfo.TimeOffTypes
                                    .Where(t => t.TimeOffType == TimeOffType.Vacation).FirstOrDefault();

                                var predictVacationDays = paidVacation.WastedDays + currentWastedDays;

                                if (predictVacationDays <= 15)
                                {
                                    paidVacation.WastedDays = predictVacationDays;
                                }
                                else
                                {
                                    daysToFull = 15 - paidVacation.WastedDays;
                                    paidVacation.WastedDays = 15;
                                    currentWastedDays -= daysToFull;
                                    _uow.VacationTypeRepository.Update(paidVacation);

                                    if (currentWastedDays > 0)
                                    {
                                        paidVacation = userInfo.VacationPolicyInfo.TimeOffTypes
                                            .Where(t => t.TimeOffType == TimeOffType.UnpaidVacation).FirstOrDefault();

                                        paidVacation.WastedDays += currentWastedDays;
                                        _uow.VacationTypeRepository.Update(paidVacation);
                                    }
                                }
                            }
                            break;
                        }
                        case TimeOffType.UnpaidVacation:
                        {

                            break;
                        }
                        case TimeOffType.Vacation:
                        {

                            break;
                        }
                    }

                }
            }
            var vacation = _uow.VacationRequestRepository.Get(vacationRequestDto.VacationRequestId);
            vacation.Approval = true;
            _uow.VacationRequestRepository.Update(vacation);
            _uow.UserInformationRepository.Update(userInfo);
        }

    }
}
