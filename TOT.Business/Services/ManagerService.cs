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
        private readonly IUserInfoService _userInfoService;
        private bool disposed = false;

        public ManagerService(IMapper mapper, IUnitOfWork uow,
            IVacationService vacationService,
            IVacationEmailSender vacationEmailSender,
            IUserService userService,
            IUserInfoService userInfoService)
        {
            _userService = userService;
            _mapper = mapper;
            _uow = uow;
            _userInfoService = userInfoService;
        }


        public IEnumerable<VacationRequestListForManagersDTO> GetDefinedManagerVacationRequests(int userId, bool? approval)
        {
            IEnumerable<VacationRequestListForManagersDTO> vacationRequestsDTO = new List<VacationRequestListForManagersDTO>();
            var vacationRequests =
                _uow.ManagerResponseRepository
                .GetAllWithVacationRequestsAndUserInfos(mr => mr.Approval == approval && mr.ManagerId == userId && mr.VacationRequest.StageOfApproving > 1)
                .Select(mr => mr.VacationRequest);
            var userInfos = _userInfoService.GetUsersInfo();
            userInfos = userInfos.Where(ui => vacationRequests.Where(vr => vr.UserInformationId == ui.Id).Any()).ToList();
            return _mapper.MergeInto<ICollection<VacationRequestListForManagersDTO>>(vacationRequests, userInfos);
        }

        public IEnumerable<VacationRequestListForManagersDTO> GetAllManagerVacationRequests(int userId)
        {
            IEnumerable<VacationRequestListForManagersDTO> vacationRequestsDTO = new List<VacationRequestListForManagersDTO>();
            var vacationRequests = _uow.ManagerResponseRepository.GetAllWithVacationRequestsAndUserInfos(mr => mr.ManagerId == userId && mr.VacationRequest.StageOfApproving > 1).Select(mr => mr.VacationRequest);
            var userInfos = _userInfoService.GetUsersInfo();
            userInfos = userInfos.Where(ui => vacationRequests.Where(vr => vr.UserInformationId == ui.Id).Any()).ToList();
            return _mapper.MergeInto<ICollection<VacationRequestListForManagersDTO>>(vacationRequests, userInfos);
        }

        public bool CheckManagerResponsesByUserId(int userId)
            => _uow.ManagerResponseRepository
            .GetAll(mr => mr.ManagerId == userId && mr.Approval == null)
            .Any();
        public ManagerResponseDto GetManagerResponse(int vacationRequestId, int managerId)
        {
            var managerResponse = _uow.ManagerResponseRepository.GetOneWithVacationRequestAndUserInfo(mr => mr.VacationRequestId == vacationRequestId && mr.ManagerId == managerId && mr.VacationRequest.StageOfApproving > 1);
            return _mapper.Map<ManagerResponse, ManagerResponseDto>(managerResponse);
        }

        public void GiveManagerResponse(int managerResponseId, string managerNotes, bool approval)
        {
            var managerResponse = _uow.ManagerResponseRepository.GetOne(mr => mr.Id == managerResponseId);
            managerResponse.Notes = managerNotes;
            managerResponse.Approval = approval;
            _uow.ManagerResponseRepository.Update(managerResponse, mr => mr.Approval, mr => mr.Notes);
            if (approval == true)
            {

                bool leftNotAnsweredResponses = _uow.ManagerResponseRepository.GetAll(mr => mr.VacationRequestId == managerResponse.VacationRequestId && mr.ForStageOfApproving == 2 && mr.Approval == null).Any();
                if (!leftNotAnsweredResponses)
                {
                    VacationRequest vacationRequest = _uow.VacationRequestRepository.GetOne(managerResponse.VacationRequestId);
                    vacationRequest.StageOfApproving = 3;
                    _uow.VacationRequestRepository.Update(vacationRequest, vr => vr.StageOfApproving);
                }
            }
            else
            {
                VacationRequest vacationRequest = _uow.VacationRequestRepository.GetOne(managerResponse.VacationRequestId);
                vacationRequest.Approval = false;
                _uow.VacationRequestRepository.Update(vacationRequest, vr => vr.Approval);
            }
            _uow.Save();
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }

                _userService.Dispose();
                _uow.Dispose();
                _userInfoService.Dispose();
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
