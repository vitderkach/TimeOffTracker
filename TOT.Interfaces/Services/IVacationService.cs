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
        ReportDto GetReportInfo(DateTime startDate, DateTime endDate, int teamId);
        void UpdateVacation(int id, string notes);
        VacationDaysDto GetVacationDays(int userId);
        List<int> GetAllVacationIdsByUser(int userId);
        SelectList GetManagersForVacationApply(int userId);
        SelectList GetTimeOffTypeList();
        bool CancelVacation(int vacationRequestId);
        VacationTimelineDto GetVacationTimeline(int vacationRequestId);
        void Dispose();
        void EditVacationRequest(ApplicationDto applicationDto);
    }
}
