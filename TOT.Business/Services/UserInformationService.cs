using System;
using System.Collections.Generic;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using AutoMapper;
using TOT.Entities;

namespace TOT.Business.Services
{
    public class UserInformationService : IUserInfoService
    {
        private IUnitOfWork Database { get; set; }

        public UserInformationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Dispose()
        {
            throw new NotImplementedException("Dispose() method not implemented");
        }

        public UserInformationDTO GetUserInfo(int? id)
        {
            if (id == null)
                throw new NullReferenceException("id = null");

            var userInfo = Database.UserProfiles.Get(id.Value);

            if (userInfo == null)
                throw new NullReferenceException("userInfo not found");

            return new UserInformationDTO
            {
                UserInformationId = userInfo.UserInformationId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName
            };
        }

        public IEnumerable<UserInformationDTO> GetUsersInfo()
        {
            var mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<UserInformation, UserInformationDTO>()).CreateMapper();

            return mapper.Map<IEnumerable<UserInformation>, 
                List<UserInformationDTO>>(Database.UserProfiles.GetAll());
        }

        public void SaveUserInfo(UserInformationDTO userInfoDTO)
        {
            UserInformation userInfo = new UserInformation()
            {
                FirstName = userInfoDTO.FirstName,
                LastName = userInfoDTO.LastName
            };

            Database.UserProfiles.Create(userInfo);
            Database.Save();
        }
    }
}
