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
    }
}
