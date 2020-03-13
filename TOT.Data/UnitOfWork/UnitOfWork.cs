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
        readonly ApplicationDbContext _context;
        private bool disposed = false;

        public IUserInformationRepository<UserInformation> UserInformationRepository { get; }

        public IVacationRequestRepository<VacationRequest> VacationRequestRepository { get; }

        public IManagerResponseRepository<ManagerResponse> ManagerResponseRepository { get; }

        public IVacationTypeRepository<VacationType> VacationTypeRepository { get; }

        public ITeamRepository<Team> TeamRepository { get; }

        public ILocationRepository<Location> LocationRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            UserInformationRepository = new UserInformationRepository(context);
            VacationRequestRepository = new VacationRequestRepository(context);
            ManagerResponseRepository = new ManagerResponseRepository(context);
            VacationTypeRepository = new VacationTypeRepository(context);
            LocationRepository = new LocationRepository(context);
            TeamRepository = new TeamRepository(context);
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void CleanUp(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing) { }

                _context.Dispose();

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            CleanUp(false);
        }
    }
}
