using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public VacationService(IMapper mapper, IUnitOfWork uow,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
        }
        public void ApplyForVacation(VacationRequestDto vacationRequestDto)
        {
            var vacation = _mapper.Map<VacationRequestDto, VacationRequest>(vacationRequestDto);
            _uow.VacationRequestRepository.Create(vacation);

        }
    }
}
