using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class UserInformationRepository : IRepository<UserInformation>
    {
        private ApplicationDbContext context;
        private bool disposed = false;

        public UserInformationRepository(ApplicationDbContext appcontext)
        {
            this.context = appcontext;
        }

        public void Create(UserInformation item)
        {
            context.UserInformations.Add(item);
        }

        public void Delete(int id)
        {
            UserInformation info = context.UserInformations.Find(id);
            if (info != null)
                context.UserInformations.Remove(info);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public UserInformation GetOne(int id)
            => context.UserInformations
            .Where(ui => ui.ApplicationUserId == id)
                .Include(ui => ui.ApplicationUser)
                .Include(ui => ui.Team)
                .Include(ui => ui.Location)
                .Include(ui => ui.VacationTypes).FirstOrDefault();

        // TODO: Rewrite the method because the database logic has been changed. As an example the commented code below

        public IEnumerable<UserInformation> GetAll()
            => context.UserInformations
                .Include(ui => ui.ApplicationUser)
                .Include(ui => ui.Team)
                .Include(ui => ui.Location)
                .Include(ui => ui.VacationTypes).ToList();

        public void Update(UserInformation item)
        {
            context.Entry(item).State = Microsoft.
                EntityFrameworkCore.EntityState.Modified;
        }
    }
}
