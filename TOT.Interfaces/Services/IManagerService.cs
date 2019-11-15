﻿using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IManagerService
    {
       // IEnumerable<VacationRequestListDto> GetAllNeedToConsiderByCurrentManager();
        IEnumerable<VacationRequestListDto> GetProcessedRequestsByCurrentManager();
        ManagerResponseDto GetResponseByVacationId(int vacationRequestId);
        void ApproveUserRequest(int managerResponseId, string managerNotes, bool approval);

        IEnumerable<ManagerResponseDto> GetAllCurrentManagerResponses();
        IEnumerable<ManagerResponseListDto> GetRequestsToConsiderByCurrentManager(
            IEnumerable<ManagerResponseDto> managerResponsesDto);
        VacationRequestApprovalDto VacationApproval(ManagerResponseDto managerResponse);
    }
}
