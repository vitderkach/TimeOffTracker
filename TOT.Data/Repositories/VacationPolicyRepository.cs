using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories {
    public class VacationPolicyRepository : IRepository<VacationPolicy> {
        private ApplicationDbContext context;
        private bool disposed = false;

        public VacationPolicyRepository(ApplicationDbContext appcontext)
        {
            this.context = appcontext;
        }

        public void Create(VacationPolicy item)
        {
            context.VacationPolicies.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var response = context.VacationPolicies
                   .Find(id);
            if (response != null)
            {
                context.VacationPolicies.Remove(response);
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

        public VacationPolicy Get(int id)
        {
            var response = context.VacationPolicies
                .Include(v => v.VacationTypes)
                .Include(v => v.UserInformation)
                .FirstOrDefault();

            return response;
        }

        public IEnumerable<VacationPolicy> GetAll()
        {
            return context.VacationPolicies
                .Include(v => v.VacationTypes)
                .Include(v => v.UserInformation)
                    .ThenInclude(v => v.ApplicationUser)
                .Include(v => v.UserInformation)
                    .ThenInclude(v => v.VacationPolicies)
                .ToList();
        }

        public void Update(VacationPolicy item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
