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
        private IUnitOfWork Database;

        public UserInformationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Dispose()
        {
            throw new NotImplementedException("Dispose() method not implemented");
        }

        public UserInformationDto GetUserInfo(int? id)
        {
            if (id == null)
                throw new NullReferenceException("id = null");

            var userInfo = Database.UserInformationRepository.Get(id.Value);

            if (userInfo == null)
                throw new NullReferenceException("userInfo not found");

            return new UserInformationDto
            {
                UserInformationId = userInfo.UserInformationId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName
            };
        }

        public IEnumerable<UserInformationDto> GetUsersInfo()
        {
            var mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<UserInformation, UserInformationDto>()).CreateMapper();

            return mapper.Map<IEnumerable<UserInformation>,
                List<UserInformationDto>>(Database.UserInformationRepository.GetAll());
        }

        public void SaveUserInfo(UserInformationDto userInfoDTO)
        {
            UserInformation userInfo = new UserInformation()
            {
                FirstName = userInfoDTO.FirstName,
                LastName = userInfoDTO.LastName
            };

            Database.UserInformationRepository.Create(userInfo);
            Database.Save();
        }

        public void DeleteUserInfo(int? id)
        {
            if (id == null)
                throw new NullReferenceException("id = null");

            var userInfo = Database.UserInformationRepository.Get(id.Value);

            if (userInfo != null)
            {
                Database.UserInformationRepository.Delete(userInfo.UserInformationId);
                Database.Save();
            }
        }
    }
}
