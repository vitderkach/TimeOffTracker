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

        public VacationService(IMapper mapper, IUnitOfWork uow,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _httpContext = httpContext;
        }
        public void ApplyForVacation(VacationRequestDto vacationRequestDto)
        {
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
    }
}
