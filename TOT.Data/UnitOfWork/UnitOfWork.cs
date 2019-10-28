using System;
using System.Collections.Generic;
using System.Text;
﻿using Microsoft.EntityFrameworkCore;
using TOT.Data.Repositories;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;

namespace TOT.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext db;
        readonly IRepository<UserInformation> _userInformationRepostitory;
        readonly IRepository<VacationRequest> _vacationRequestRepository;

        public UnitOfWork(ApplicationDbContext context, 
            IRepository<UserInformation> userInformationRepository, 
            IRepository<VacationRequest> vacationRequestRepository)
        {
            db = context;
            _userInformationRepostitory = userInformationRepository;
            _vacationRequestRepository = vacationRequestRepository;
        }
        public IRepository<UserInformation> UserInformationRepostitory
        {
            get { return _userInformationRepostitory; }
        }

        public IRepository<VacationRequest> VacationRequestRepository
        {
            get { return _vacationRequestRepository; 
        }
    }
}
