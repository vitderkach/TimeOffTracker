using System.Collections.Generic;
using TOT.Dto;

namespace TOT.Interfaces.Services
{
    public interface IUserInfoService
    {
        UserInformationDTO GetUserInfo(int? id);
        IEnumerable<UserInformationDTO> GetUsersInfo();
        void SaveUserInfo(UserInformationDTO userInfoDTO);
        void Dispose();
    }
}
