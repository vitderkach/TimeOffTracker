using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class VacationRequestRepository : IRepository<VacationRequest>
    {
        ApplicationDbContext db;
        public VacationRequestRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(VacationRequest item)
        {
            db.VacationRequests.Add(item);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var vacation = db.VacationRequests
                .Find(id);
            if (vacation != null)
            {
                db.VacationRequests.Remove(vacation);
                db.SaveChanges();
            }
        }

        public VacationRequest Get(int id)
        {
            var vacation = db.VacationRequests
                .Include(v => v.ManagersResponses)
                .Include(v => v.User)
                .Where(v => v.VacationRequestId == id)
                .FirstOrDefault();
            return vacation;
        }

        public IEnumerable<VacationRequest> GetAll()
        {
            return db.VacationRequests
                .Include(v => v.ManagersResponses)
                .Include(v => v.User);
        }

        public void Update(VacationRequest item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
        // ~UserInformationRepository()
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
