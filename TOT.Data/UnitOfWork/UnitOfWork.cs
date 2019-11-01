using System;
using Microsoft.EntityFrameworkCore;
using TOT.Data.Repositories;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;

namespace TOT.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        readonly ApplicationDbContext db;
        readonly IRepository<UserInformation> _userInformationRepostitory;
        readonly IRepository<VacationRequest> _vacationRequestRepository;

        public UnitOfWork(ApplicationDbContext context,
            IRepository<UserInformation> userInformationRepository,
            IRepository<VacationRequest> vacationRequestRepository)
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            get { return _vacationRequestRepository; }
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
