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
        private ApplicationDbContext context;
        private UserInformationRepository userInfoRepository;

        private bool disposed = false;

        public UnitOfWork(DbContextOptions<ApplicationDbContext> options)
        {
            this.context = new ApplicationDbContext(options);
        }

        public IRepository<UserInformation> UserProfiles
        {
            get
            {
                if (userInfoRepository == null)
                    userInfoRepository = new UserInformationRepository(context);
                return userInfoRepository;
            }
        }

        public void Save()
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
