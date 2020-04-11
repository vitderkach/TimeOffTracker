using System;
using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IUserInfoService
    {
        UserInformationDto GetUserInfo(int? id);
        IEnumerable<UserInformationDto> GetUsersInfo();
        IEnumerable<UserInformationDto> GetHistoryManagerInfos(int vacationRequestId, DateTime systemStart);
        void SaveUserInfo(UserInformationDto userInfoDTO);
        void Dispose();
    }
}
