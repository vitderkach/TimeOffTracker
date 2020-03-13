using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IUserInfoService
    {
        UserInformationDto GetUserInfo(int? id);
        IEnumerable<UserInformationDto> GetUsersInfo();
        void SaveUserInfo(UserInformationDto userInfoDTO);
        void FireUserInfo(int? id);
        void Dispose();
    }
}
