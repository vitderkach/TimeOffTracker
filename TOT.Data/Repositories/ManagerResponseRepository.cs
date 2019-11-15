using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class ManagerResponseRepository : IRepository<ManagerResponse>
    {
        private ApplicationDbContext context;
        private bool disposed = false;

        public ManagerResponseRepository(ApplicationDbContext appcontext)
        {
            this.context = appcontext;
        }

        public void Create(ManagerResponse item)
        {
            context.ManagerResponses.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var response = context.ManagerResponses
                   .Find(id);
            if (response != null)
            {
                context.ManagerResponses.Remove(response);
                context.SaveChanges();
            }
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

        public ManagerResponse Get(int id)
        {
            var response = context.ManagerResponses
                .Include(manager => manager.Manager)
                    .ThenInclude(info => info.UserInformation)
                .Include(vr => vr.VacationRequest)
                /*.ThenInclude(user => user.User)
                    .ThenInclude(info => info.UserInformation)*/
                .Where(mr => mr.Id == id)
                .FirstOrDefault();

            return response;
        }

        public IEnumerable<ManagerResponse> GetAll()
        {
            return context.ManagerResponses
                .Include(manager => manager.Manager)
                .Include(vr => vr.VacationRequest)
                    .ThenInclude(user => user.User)
                        .ThenInclude(info => info.UserInformation)
                .ToList();
        }

        public void Update(ManagerResponse item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
