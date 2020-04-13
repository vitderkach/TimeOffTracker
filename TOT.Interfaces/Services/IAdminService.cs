using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOT.Dto;
using TOT.Entities;

namespace TOT.Interfaces.Services
{
    public interface IAdminService
    {
        ICollection<UserInformationListDto> GetApplicationUserList();
        List<IdentityRole<int>> GetApplicationRoles();
        Task<IdentityResult> RegistrationNewUser(RegistrationUserDto registrationForm);
        EditApplicationUserDto EditUserData(int userId);
        Task<IdentityResult> UserDataManipulation(int userId, int userRoleId);
        EditVacationDaysDto GetUsersVacationDays(int id);
        void EditUsersVacationDays(int id, EditVacationDaysDto editUsersVacationDays);
        bool FireEmployee(int userId);
        bool СhargeVacationDays(int userId, int count, TimeOffType vacationType, bool isAlreadyCharged);
        void ChangeCountOfGiftdays(int userId, int count);
        ManagerResponseDto GetAdminResponse(int vacationId, int userId);
        void AcceptVacationRequest(int vacationrequestId, string adminNotes, bool approve);
        void ApproveVacationRequest(int adminResponseId, string adminNotes, bool approve);
        ICollection<VacationRequestsListForAdminsDTO> GetDefinedVacationRequestsForApproving(int userId, bool? approve);
        ICollection<VacationRequestsListForAdminsDTO> GetDefinedVacationRequestsForAcceptance(int userId, bool? approve);
        ICollection<VacationRequestsListForAdminsDTO> GetSelfCancelledVacationRequests(int userId);
        ICollection<VacationRequestsListForAdminsDTO> GetAllVacationRequests(int userId);
        bool CalculateVacationDays(int vacationRequestId, int daysCount);
        void EditVacationRequest(ApplicationDto applicationDto);
        void Dispose();
    }
}
