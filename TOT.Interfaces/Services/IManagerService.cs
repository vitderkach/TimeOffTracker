using System.Collections.Generic;
using TOT.Dto;
using TOT.Entities;

namespace TOT.Interfaces.Services
{
    public interface IManagerService
    {
        IEnumerable<VacationRequestListForManagersDTO> GetAllManagerVacationRequests(int userId);
        IEnumerable<VacationRequestListForManagersDTO> GetDefinedManagerVacationRequests(int userId, bool? approval);
        ManagerResponseDto GetManagerResponse(int vacationRequestId, int userId);

        void GiveManagerResponse(int managerResponseId, string managerNotes, bool approve);
        bool CheckManagerResponsesByUserId(int userId);
        void Dispose();
    }
}
