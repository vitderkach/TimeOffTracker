using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IVacationService
    {
        void ApplyForVacation(ApplicationDto applicationDto);
        IEnumerable<VacationRequestListDto> GetAllByCurrentUser();
        VacationRequestDto GetVacationById(int id);
        void UpdateVacation(int id, string notes);
        bool DeactivateVacation(int id);
        VacationDaysDto GetVacationDays(int userId);
        List<int> GetAllVacationIdsByUser(int userId);
        SelectList GetManagersForVacationApply(int userId);
        SelectList GetTimeOffTypeList();

        void Dispose();
    }
}
