using System;
using System.Collections.Generic;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Entities;

namespace TOT.Business.Services
{
    public class UserInformationService : IUserInfoService
    {
        private IUnitOfWork Database;
        private IMapper _mapper;
        public UserInformationService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
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

            return _mapper.Map<UserInformation, UserInformationDto>(userInfo);

            /*return new UserInformationDto
            {
                UserInformationId = userInfo.UserInformationId,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName
            };*/
        }

        public IEnumerable<UserInformationDto> GetUsersInfo()
        {
            return _mapper.Map<IEnumerable<UserInformation>,
                List<UserInformationDto>>(Database.UserInformationRepository.GetAll());
        }

        public void SaveUserInfo(UserInformationDto userInfoDTO)
        {
            /*UserInformation userInfo = new UserInformation()
            {
                FirstName = userInfoDTO.FirstName,
                LastName = userInfoDTO.LastName
            };*/
            var userInfo = _mapper.Map<UserInformationDto, UserInformation>(userInfoDTO);

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
