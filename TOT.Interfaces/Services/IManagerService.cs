﻿using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IManagerService
    {
        IEnumerable<VacationRequestListDto> GetAllNeedToConsiderByCurrentManager();
        ManagerResponseDto GetResponseByVacationId(int vacationRequestId);
    }
}
