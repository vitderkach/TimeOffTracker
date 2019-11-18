using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class VacationService: IVacationService
    {
        private IMapper _mapper;
        private IUnitOfWork _uow; 
        private readonly IVacationEmailSender _vacationEmailSender;
        private readonly IUserService _userService;

        public VacationService(IMapper mapper, IUnitOfWork uow,
            IVacationEmailSender vacationEmailSender,
            IUserService userService)
        {
            _mapper = mapper;
            _uow = uow;
            _userService = userService;
            _vacationEmailSender = vacationEmailSender;
        }
        public void ApplyForVacation(VacationRequestDto vacationRequestDto)
        {
            var vacation = _mapper.Map<VacationRequestDto, VacationRequest>(vacationRequestDto);
            for (int i=0; i < vacationRequestDto.SelectedManager.Count; i++)
            {

                vacation.ManagersResponses.Add(new ManagerResponse()
                {
                    ManagerId = vacationRequestDto.SelectedManager[i],
                    isRequested = i == 0 ? true : false,
                });
            }
            _uow.VacationRequestRepository.Create(vacation);

            var user = _userService.GetCurrentUser().Result;
            var userInfo = _uow.UserInformationRepository
                .GetAll()
                .Where(u => u.User.Id == user.Id)
                .FirstOrDefault();

            EmailModel emailModel = new EmailModel()
            {
                To = vacation.ManagersResponses.ElementAt(0).Manager.Email,
                FullName = $"{userInfo.LastName} {userInfo.FirstName}",
                Body = vacation.Notes
            };
            _vacationEmailSender.ExecuteToManager(emailModel);
        }
        /*public void DeleteVacationByUserId(int id)
        {
            var vacationsRemoved = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserId == id);
            foreach(var removed in vacationsRemoved)
            {
                _uow.VacationRequestRepository.Delete(removed.VacationRequestId);
            }
        }*/

        public List<int> GetAllVacationIdsByUser(int userId)
        {
            List<int> vacationIds = new List<int>();
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserId == userId);

            foreach (VacationRequest request in vacations)
            {
                vacationIds.Add(request.VacationRequestId);
            }

            return vacationIds;
        }

        public void DeleteVacationById(int id)
        {
            _uow.VacationRequestRepository.Delete(id);
        }

        public IEnumerable<VacationRequestListDto> GetAllByCurrentUser()
        {
            var currentUserId = _userService.GetCurrentUser().Result.Id;
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserId == currentUserId);

            var vacationsDto = _mapper.Map<IEnumerable<VacationRequest>, IEnumerable<VacationRequestListDto>>(vacations);
            return vacationsDto;
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
            if (vacation != null)
            {
                vacation.Notes = notes;
                _uow.VacationRequestRepository.Update(vacation);
            }
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
