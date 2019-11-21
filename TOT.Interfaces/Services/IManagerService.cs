using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IManagerService
    {
        IEnumerable<ManagerResponseDto> GetProcessedRequestsByCurrentManager();
        ManagerResponseDto GetResponseByVacationId(int vacationRequestId);
        void ApproveUserRequest(int managerResponseId, string managerNotes, bool approval);

        IEnumerable<ManagerResponseDto> GetAllCurrentManagerResponses();
        ManagerResponseDto GetResponseActiveByVacationId(int vacationRequestId);
        IEnumerable<ManagerResponseListDto> GetAllMyManagerResponses();
        IEnumerable<ManagerResponseListDto> GetCurrentManagerRequests(
            IEnumerable<ManagerResponseDto> managerResponsesDto);
        VacationRequestApprovalDto VacationApproval(ManagerResponseDto managerResponse);
    }
}
