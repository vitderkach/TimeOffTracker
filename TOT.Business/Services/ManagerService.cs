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
        private readonly IUserService _userService;
        private IMapper _mapper;
        private IUnitOfWork _uow;

        public ManagerService(IMapper mapper, IUnitOfWork uow,
            IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
            _uow = uow;
        }

        // проверяет, есть ли еще менеджеры, которые должны рассмотреть заявку
        private bool CheckManagerResponsesForVacation(int vacationId)
        {
            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.Approval == null);

            if (managerResponses.Any())
                return true;

            return false;
        }
        // выберает следующий на очереди ManagerResponse для выбранной заявки
        private ManagerResponse SelectNextManager(int vacationId)
        {
            var nextResponse = _uow.ManagerResponseRepository.GetAll()
                .Where(r => r.VacationRequestId == vacationId && r.isRequested == false
                    && r.Approval == null)
                .FirstOrDefault();

            return nextResponse;
        }

        // список всех активных запросов на имя менеджера
        public IEnumerable<ManagerResponseDto> GetAllCurrentManagerResponses()
        {
            var Id = _userService.GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.isRequested == true)
                .OrderBy(v => v.VacationRequest.StartDate);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        // мапит данные из списка запросов для вывода во View
        public IEnumerable<ManagerResponseListDto> GetCurrentManagerRequests(
            IEnumerable<ManagerResponseDto> managerResponsesDto)
        {
            var requestsToManager = _mapper.Map<IEnumerable<ManagerResponseDto>,
                IEnumerable<ManagerResponseListDto>>(managerResponsesDto);

            return requestsToManager;
        }

        // список всех уже рассмотренных запросов текущего менеджера
        public IEnumerable<ManagerResponseDto> GetProcessedRequestsByCurrentManager()
        {
            var Id = _userService.GetCurrentUser().Result.Id;

            var managerResponses = _uow.ManagerResponseRepository.GetAll()
                .Where(vr => vr.ManagerId == Id && vr.Approval != null)
                .OrderByDescending(r => r.DateResponse);

            var managerResponsesDto = _mapper.Map<IEnumerable<ManagerResponse>,
                IEnumerable<ManagerResponseDto>>(managerResponses);

            return managerResponsesDto;
        }

        // получает ManagerResponse, которой был предварительно создан после
        // создания заявки, для текущего менеджера
        public ManagerResponseDto GetResponseByVacationId(int vacationRequestId)
        {
            ManagerResponseDto managerResponseDto = default;
            var managerId = _userService.GetCurrentUser().Result.Id;

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

        // мапит данные выбранной менеджером заявки для
        // вывода View с Approve/Reject 
        public VacationRequestApprovalDto VacationApproval(
            ManagerResponseDto managerResponse)
        {
            var approval = _mapper.Map<ManagerResponseDto,
                VacationRequestApprovalDto>(managerResponse);

            return approval;
        }

        // действия, которые происходят после Approve/Reject
        // заявки менеджером (передача следующему на очереди менеджеру
        // или закрытие данной заявки)
        public void ApproveUserRequest(int managerResponseId,
            string managerNotes, bool approval)
        {
            var managerResponse = _uow.ManagerResponseRepository.Get(managerResponseId);
            managerResponse.Approval = approval;
            managerResponse.Notes = managerNotes;
            managerResponse.isRequested = false;
            managerResponse.DateResponse = System.DateTime.UtcNow;

            _uow.ManagerResponseRepository.Update(managerResponse);

            if (!CheckManagerResponsesForVacation(managerResponse.VacationRequestId))
            {
                var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
                if (approval != false)
                {
                    vacation.Approval = true;
                }
                else
                {
                    vacation.Approval = false;
                }
                _uow.VacationRequestRepository.Update(vacation);
            }
            else
            {
                if (approval != false)
                {
                    var nextResponse = SelectNextManager(managerResponse.VacationRequestId);
                    nextResponse.isRequested = true;
                    _uow.ManagerResponseRepository.Update(nextResponse);
                }
                else
                {
                    var vacation = _uow.VacationRequestRepository.Get(managerResponse.VacationRequestId);
                    vacation.Approval = false;
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
