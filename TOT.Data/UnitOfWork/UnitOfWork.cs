using Microsoft.EntityFrameworkCore;
using TOT.Data.Repositories;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;

namespace TOT.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        private UserInformationRepository userInfoRepository;

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
    }
}
