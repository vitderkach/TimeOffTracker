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

        public UserInformation Get(int id)
        {
            return context.UserInformations.Find(id);
        }

        public IEnumerable<UserInformation> GetAll()
        {
            return context.UserInformations
                .Include(u => u.VacationPolicies)
                    .ThenInclude(u => u.VacationTypes)

                .Include(u => u.ApplicationUser)
                    .ThenInclude(u => u.UserInformation)
                        .ThenInclude(u => u.VacationPolicies)
                .ToList();
        }

        public void Update(UserInformation item)
        {
            context.Entry(item).State = Microsoft.
                EntityFrameworkCore.EntityState.Modified;
        }
    }
}
