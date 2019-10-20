using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class UserInformationRepository : IRepository<UserInformation>
    {
        ApplicationDbContext db;
        public UserInformationRepository()
        {
            db = new ApplicationDbContext();
        }
        public void Create(UserInformation item)
        {
            db.Add(item);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            UserInformation userInfo = db.UserInformations.Find(id);
            if (userInfo != null)
                db.Remove(userInfo);
            db.SaveChanges();
        }

        public UserInformation Get(int id)
        {
            return db.UserInformations
                .Find(id);
        }

        public IEnumerable<UserInformation> GetAll()
        {
            return db.UserInformations
                .Include(u => u.User);
        }

        public void Update(UserInformation item)
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
