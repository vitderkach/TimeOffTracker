﻿using System;
using System.Collections.Generic;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace TOT.Business.Services
{
    public class UserInformationService : IUserInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        private bool disposed = false;

        public UserInformationService(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _userManager = userManager;
        }

        public UserInformationDto GetUserInfo(int? id)
        {
            if (id == null)
                throw new NullReferenceException("id = null");
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            var userInfo = _unitOfWork.UserInformationRepository.GetOneWithAllProperties(id.Value);
            
            if (userInfo == null)
                throw new NullReferenceException("userInfo not found");

            return _mapper.MergeInto<UserInformationDto>(user, userInfo);
        }

        public IEnumerable<UserInformationDto> GetUsersInfo()
        {
            var users = _userManager.Users.ToList();
            var usersInfo = _unitOfWork.UserInformationRepository.GetAll(null, ui => ui.Team, ui => ui.Location);

            return _mapper.MergeInto<IEnumerable<UserInformationDto>>(users, usersInfo);
        }

        public void SaveUserInfo(UserInformationDto userInfoDTO)
        {
            var userInfo = _mapper.Map<UserInformationDto, UserInformation>(userInfoDTO);
            if(userInfo != null)
            {
                _unitOfWork.UserInformationRepository.Create(userInfo);
                _unitOfWork.Save();
            }
        }
        public IEnumerable<UserInformationDto> GetHistoryManagerInfos(int vacationRequestId, DateTime systemStart)
        {
            var managerIds = _unitOfWork.AuxiliaryRepository.GetHistoryManagerIdentificators(vacationRequestId, systemStart);
            List<UserInformation> managerUIs = new List<UserInformation>();
            List<ApplicationUser> managerAUs = new List<ApplicationUser>();
            foreach (int id in managerIds)
            {
                managerUIs.Add(_unitOfWork.UserInformationRepository.GetOne(id));
                managerAUs.Add(_userManager.Users.First(au => au.Id == id));
            }
            return _mapper.MergeInto<IEnumerable<UserInformationDto>>(managerAUs , managerUIs);
        }

        private void CleanUp(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { }
                _unitOfWork.Dispose();
                disposed = true;
            }
        }
        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~UserInformationService()
        {
            CleanUp(false);
        }
    }
}
