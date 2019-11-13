using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IVacationService
    {
        void ApplyForVacation(VacationRequestDto vacationRequestDto);
        IEnumerable<VacationRequestListDto> GetAllByCurrentUser();
        VacationRequestDto GetVacationById(int id);
        void UpdateVacation(int id, string notes);
        bool DeleteVacation(int id);
        void DeleteVacationByUserId(int id);
    }
}
