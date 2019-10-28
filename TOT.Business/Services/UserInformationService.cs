using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Business.Services
{
    public class UserInformationService : IUserInformationService
    {
        IUnitOfWork _uow;
        IMapper _mapper;
        public UserInformationService(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }
        public UserInformationDTO getUserInformation(int id)
        {
            var userInfo = _uow.UserInformationRepostitory.Get(id);
            return _mapper.Map<UserInformation, UserInformationDTO>(userInfo);
        }
    }
}
