using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IUserInfoService
    {
        UserInformationDto GetUserInfo(int? id);
        IEnumerable<UserInformationDto> GetUsersInfo();
        void SaveUserInfo(UserInformationDto userInfoDTO);
        void DeleteUserInfo(int? id);
        void Dispose();
    }
}
