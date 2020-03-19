using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow; 
        private readonly IVacationEmailSender _vacationEmailSender;
        private readonly IUserService _userService;

        private bool disposed = false;

        public VacationService(IMapper mapper, IUnitOfWork uow,
            IVacationEmailSender vacationEmailSender,
            IUserService userService)
        {
            _mapper = mapper;
            _uow = uow;
            _userService = userService;
            _vacationEmailSender = vacationEmailSender;
        }

        public SelectList GetManagersForVacationApply()
        {
            var currentUser = _userService.GetCurrentUser().Result;

            var managers = _userService.GetAllByRole("Manager");
            managers = managers.Concat(_userService.GetAllByRole("Administrator"));

            if (managers.Where(u => u.Id == currentUser.Id).Any())
            {
                managers = managers.Where(m => m.Id != currentUser.Id);
            }

            managers.OrderBy(n => n.FullName);

            return new SelectList(managers, "Id", "UserInformation.FullName");
        }

        public void ApplyForVacation(VacationRequestDto vacationRequestDto)
        {
            var vacation = _mapper.Map<VacationRequestDto, VacationRequest>(vacationRequestDto);
            for (int i=0; i < vacationRequestDto.SelectedManager.Count; i++)
            {
                ManagerResponse managerResponse =
                    new ManagerResponse()
                    {
                        ManagerId = vacationRequestDto.SelectedManager[i],
                        isRequested = i == 0 ? true : false,
                        VacationRequestId = vacation.VacationRequestId

                    };
                _uow.ManagerResponseRepository.Create(managerResponse);
            }
            _uow.VacationRequestRepository.Create(vacation);
            _uow.Save();
            var user = _userService.GetCurrentUser().Result;
            var userInfo = _uow.UserInformationRepository
                .GetAll()
                .Where(u => u.ApplicationUser.Id == user.Id)
                .FirstOrDefault();

            EmailModel emailModel = new EmailModel()
            {
                To = vacation.ManagersResponses.ElementAt(0).Manager.ApplicationUser.Email,
                FullName = $"{userInfo.LastName} {userInfo.FirstName}",
                Body = vacation.Notes
            };
            _vacationEmailSender.ExecuteToManager(emailModel);
        }

        public VacationDaysDto GetVacationDays(int userId)
        {
            var vacationDays = _uow.UserInformationRepository.GetOneWithVacationRequests(userId).VacationTypes.Where(vt => vt.Year == DateTime.Now.Year).ToList();

            VacationDaysDto vacationDaysDto = new VacationDaysDto(); 
            _mapper.Map<IEnumerable<VacationType>, VacationDaysDto>(vacationDays, vacationDaysDto);
            return vacationDaysDto;
        }

        public List<int> GetAllVacationIdsByUser(int userId)
        {
            List<int> vacationIds = new List<int>();
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserInformationId== userId);

            foreach (VacationRequest request in vacations)
            {
                vacationIds.Add(request.VacationRequestId);
            }

            return vacationIds;
        }

        public IEnumerable<VacationRequestListDto> GetAllByCurrentUser()
        {
            var currentUserId = _userService.GetCurrentUser().Result.Id;
            var vacations = _uow.VacationRequestRepository
                .GetAll()
                .Where(v => v.UserInformationId == currentUserId);

            var vacationsDto = _mapper.Map<IEnumerable<VacationRequest>, IEnumerable<VacationRequestListDto>>(vacations);
            return vacationsDto;
        }

        public VacationRequestDto GetVacationById(int id)
        {
            var vacation = _uow.VacationRequestRepository.GetOne(id);
            var vacationDto = _mapper.Map<VacationRequest, VacationRequestDto>(vacation);

            return vacationDto;
        }
        public void UpdateVacation(int id, string notes)
        {
            var vacation = _uow.VacationRequestRepository.GetOne(id);
            if (vacation != null)
            {
                vacation.Notes = notes;
                _uow.VacationRequestRepository.Update(vacation);
                _uow.Save();
            }
        }

        public bool DeactivateVacation(int id)
        {
            if(_uow.VacationRequestRepository.GetOne(id) is VacationRequest vacationRequest && vacationRequest.Approval == null)
            {
                _uow.VacationRequestRepository.TransferToHistory(id);
                _uow.Save();
                return true;
            }
            else
            {
                return false;
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

        ~VacationService()
        {
            CleanUp(false);
        }
    }
}
