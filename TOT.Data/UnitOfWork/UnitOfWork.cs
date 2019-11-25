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
        readonly IRepository<ManagerResponse> _managerResponseRepository;
        readonly IRepository<VacationPolicyInfo> _vacationPolicyRepository;
        readonly IRepository<VacationType> _vacationTypeRepository;

        public UnitOfWork(ApplicationDbContext context,
            IRepository<UserInformation> userInformationRepository,
            IRepository<VacationRequest> vacationRequestRepository,
            IRepository<ManagerResponse> managerResponseRepository,
            IRepository<VacationPolicyInfo> vacationPolicyRepository,
            IRepository<VacationType> vacationTypeRepository)
        {
            _userInformationRepostitory = userInformationRepository;
            _vacationRequestRepository = vacationRequestRepository;
            _managerResponseRepository = managerResponseRepository;
            _vacationPolicyRepository = vacationPolicyRepository;
            _vacationTypeRepository = vacationTypeRepository;
            db = context;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public IRepository<UserInformation> UserProfiles
        {
            get { return _userInformationRepostitory; }
        }

        public IRepository<VacationRequest> VacationRequestRepository
        {
            get { return _vacationRequestRepository; }
        }

        public IRepository<UserInformation> UserInformationRepository
        {
            get { return _userInformationRepostitory; }
        }

        public IRepository<ManagerResponse> ManagerResponseRepository
        {
            get { return _managerResponseRepository; }
        }

        public IRepository<VacationPolicyInfo> VacationPolicyRepository
        {
            get { return _vacationPolicyRepository; }
        }
        public IRepository<VacationType> VacationTypeRepository
        {
            get { return _vacationTypeRepository; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
