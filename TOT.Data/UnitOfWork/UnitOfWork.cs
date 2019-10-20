using System;
using System.Collections.Generic;
using System.Text;
using TOT.Data.Repositories;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;

namespace TOT.Data.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        ApplicationDbContext db;
        IRepository<UserInformation> _userInformationRepostitory;
        public UnitOfWork(ApplicationDbContext context, IRepository<UserInformation> userInformationRepository)
        {
            db = new ApplicationDbContext();
            _userInformationRepostitory = userInformationRepository;
        }
        public IRepository<UserInformation> UserInformationRepostitory
        {
            get { return _userInformationRepostitory; }
        }
    }
}
