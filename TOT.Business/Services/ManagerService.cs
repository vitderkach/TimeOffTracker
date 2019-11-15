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

        public ManagerService(IMapper mapper, IUnitOfWork uow, 
            IHttpContextAccessor httpContext,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

        /*
        public IEnumerable<VacationRequestListDto> GetAllNeedToConsiderByCurrentManager()
        {
            var Id = GetCurrentUser().Result.Id;

            var vacationRequests = _uow.VacationRequestRepository.GetAll()
                .Where(v => v.ManagersResponses
                .Any(x => x.ManagerId == Id && x.isRequested == true))
                .OrderBy(v => v.StartDate);

            var vacationsRequestListDto = _mapper.Map<IEnumerable<VacationRequest>,
                IEnumerable<VacationRequestListDto>>(vacationRequests);

            return vacationsRequestListDto;
        }
        */

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

        public IEnumerable<ManagerResponseListDto> GetRequestsToConsiderByCurrentManager(
            IEnumerable<ManagerResponseDto> managerResponsesDto)
        {
            var requestsToManager = _mapper.Map<IEnumerable<ManagerResponseDto>,
                IEnumerable<ManagerResponseListDto>>(managerResponsesDto);

            return requestsToManager;
        }

        public IEnumerable<VacationRequestListDto> GetProcessedRequestsByCurrentManager()
        {
            var Id = GetCurrentUser().Result.Id;

            var vacationRequests = _uow.VacationRequestRepository.GetAll()
                .Where(v => v.ManagersResponses
                .Any(x => x.ManagerId == Id && x.Approval != null))
                .OrderByDescending(v => v.EndDate);

            var vacationsRequestListDto = _mapper.Map<IEnumerable<VacationRequest>,
                IEnumerable<VacationRequestListDto>>(vacationRequests);

            return vacationsRequestListDto;
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

        public void ApproveUserRequest(int managerResponseId,
            string managerNotes, bool approval)
        {
            var managerResponse = _uow.ManagerResponseRepository.Get(managerResponseId);
            managerResponse.Approval = approval;
            managerResponse.Notes = managerNotes;
            managerResponse.isRequested = false;

            //Это для если 1 менеджер, работает. Теперь надо пройти по всем managerResponse которые привязаны к vacation id
            //и проверить, если менеджер остался 1, которому надо ответить - выполнять код ниже.

            //var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
            //vacation.Approval = true;
            //_uow.VacationRequestRepository.Update(vacation);
            _uow.ManagerResponseRepository.Update(managerResponse);
        }
    }
}
