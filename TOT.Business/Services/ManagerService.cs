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
    public class ManagerService : IManagerService
    {
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;
        private IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IVacationService _vacationService;
        private readonly IVacationEmailSender _vacationEmailSender;

        public ManagerService(IMapper mapper, IUnitOfWork uow,
            IHttpContextAccessor httpContext,
            IVacationService vacationService,
            IVacationEmailSender vacationEmailSender,
            IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _vacationService = vacationService;
            _vacationEmailSender = vacationEmailSender;
            _mapper = mapper;
            _uow = uow;
            _httpContext = httpContext;
        }

        /*Можно вынести куда-то*/
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            return user;
        }
        private bool CheckManagerResponsesForVacation(int vacationId)
        {
            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.Approval == null);

            if (managerResponses.Any())
                return true;

            return false;
        }
        private ManagerResponse SelectNextManager(int vacationId)
        {
            var nextResponse = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.isRequested == false
                    && r.Approval == null)
                .FirstOrDefault();

            return nextResponse;
        }
        public IEnumerable<ManagerResponseDto> GetAllCurrentManagerResponses()
        {
            var Id = GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.isRequested == true)
                .OrderBy(v => v.VacationRequest.StartDate);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        public IEnumerable<ManagerResponseListDto> GetCurrentManagerRequests(
            IEnumerable<ManagerResponseDto> managerResponsesDto)
        {
            var requestsToManager = _mapper.Map<IEnumerable<ManagerResponseDto>,
                IEnumerable<ManagerResponseListDto>>(managerResponsesDto);

            return requestsToManager;
        }

        public IEnumerable<ManagerResponseDto> GetProcessedRequestsByCurrentManager()
        {
            var Id = GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.Approval != null)
                .OrderByDescending(r => r.DateResponse);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        public ManagerResponseDto GetResponseByVacationId(int vacationRequestId)
        {
            ManagerResponseDto managerResponseDto = default;
            var managerId = GetCurrentUser().Result.Id;

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

        public VacationRequestApprovalDto VacationApproval(
            ManagerResponseDto managerResponse)
        {
            var approval = _mapper.Map<ManagerResponseDto,
                VacationRequestApprovalDto>(managerResponse);

            return approval;
        }
            
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

            EmailModel emailModel = new EmailModel()
            {
                Body = vacation.Notes,
                FullName = $"{vacation.User.UserInformation.LastName} {vacation.User.UserInformation.FirstName}",
                To = vacation.User.Email
            };
            //All managers reviewed vacation
            if (!CheckManagerResponsesForVacation(managerResponse.VacationRequestId))
            {
                var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
                vacation.Approval = approval;

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
                if (approval != false)
                {
                    var nextResponse = SelectNextManager(managerResponse.VacationRequestId);
                    emailModel.To = nextResponse.Manager.Email;
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

            //Это для если 1 менеджер, работает. Теперь надо пройти по всем managerResponse которые привязаны к vacation id
            //и проверить, если менеджер остался 1, которому надо ответить - выполнять код ниже.

            //var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
            //vacation.Approval = true;
            //_uow.VacationRequestRepository.Update(vacation);
        }
    }
}
