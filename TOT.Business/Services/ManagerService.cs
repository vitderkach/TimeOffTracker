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


        private IEnumerable<VacationRequestListForManagersDTO> CreateListForManagersDto(ICollection<ManagerResponse> managerResponses)
        {
            List<VacationRequestListForManagersDTO> vacationRequestsDto = new List<VacationRequestListForManagersDTO>();
            var vacationRequests = managerResponses.Select(mr => mr.VacationRequest).ToList();
            foreach (var vacationRequest in vacationRequests)
            {
                vacationRequestsDto.Add(_mapper.MergeInto<VacationRequestListForManagersDTO>(vacationRequest, _userInfoService.GetUserInfo(vacationRequest.UserInformationId)));
            }
            foreach (var item in vacationRequestsDto)
            {
                item.ManagerApproval = managerResponses.FirstOrDefault(mr => mr.VacationRequestId == item.VacationRequestId).Approval;
            }
            return vacationRequestsDto;
        }

        public IEnumerable<VacationRequestListForManagersDTO> GetDefinedManagerVacationRequests(int userId, bool? approval)
        {
            var managerResponses =
                _uow.ManagerResponseRepository
                .GetAllWithVacationRequestsAndUserInfos(mr => mr.Approval == approval && mr.ManagerId == userId && mr.VacationRequest.StageOfApproving > 1)
                .Where(mr => !(mr.Approval == null && mr.VacationRequest.SelfCancelled == true)).ToList();

            return CreateListForManagersDto(managerResponses);
        }

        public IEnumerable<VacationRequestListForManagersDTO> GetAllManagerVacationRequests(int userId)
        {
            var managerResponses = 
                _uow.ManagerResponseRepository
                .GetAllWithVacationRequestsAndUserInfos(mr => mr.ManagerId == userId && mr.VacationRequest.StageOfApproving > 1)
                .Where(mr => !(mr.Approval == null && mr.VacationRequest.SelfCancelled == true))
                .ToList();
            return CreateListForManagersDto(managerResponses);
        }

        public bool CheckManagerResponsesByUserId(int userId)
            => _uow.ManagerResponseRepository
            .GetAll(mr => mr.ManagerId == userId && mr.Approval == null)
            .Any();
        public ManagerResponseDto GetManagerResponse(int vacationRequestId, int managerId)
        {
            var managerResponse = 
                _uow.ManagerResponseRepository
                .GetOneWithVacationRequestAndUserInfo(mr => mr.VacationRequestId == vacationRequestId && mr.ManagerId == managerId && mr.VacationRequest.StageOfApproving == 2);
            return _mapper.Map<ManagerResponse, ManagerResponseDto>(managerResponse);
        }

        public void GiveManagerResponse(int managerResponseId, string managerNotes, bool approval)
        {
            var managerResponse = _uow.ManagerResponseRepository.GetOne(mr => mr.Id == managerResponseId);
            managerResponse.Notes = managerNotes;
            managerResponse.Approval = approval;
            managerResponse.DateResponse = DateTime.Now;
            _uow.ManagerResponseRepository.Update(managerResponse, mr => mr.Approval, mr => mr.Notes, mr => mr.DateResponse);
            _uow.Save();
            if (approval == true)
            {

                bool leftNotAnsweredResponses = _uow.ManagerResponseRepository.GetAll(mr => mr.VacationRequestId == managerResponse.VacationRequestId && mr.ForStageOfApproving == 2 && mr.Approval == null).Any();
                if (!leftNotAnsweredResponses)
                {
                    VacationRequest vacationRequest = _uow.VacationRequestRepository.GetOne(managerResponse.VacationRequestId);
                    vacationRequest.StageOfApproving = 3;
                    _uow.VacationRequestRepository.Update(vacationRequest, vr => vr.StageOfApproving);
                    ManagerResponse response = new ManagerResponse();
                    response.ManagerId = _uow.ManagerResponseRepository.GetOne(mr => mr.VacationRequestId == vacationRequest.VacationRequestId && mr.ForStageOfApproving == 1).ManagerId;
                    response.VacationRequestId = vacationRequest.VacationRequestId;
                    response.ForStageOfApproving = 3;
                    response.DateResponse = DateTime.MaxValue;
                    _uow.ManagerResponseRepository.Create(response);
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
