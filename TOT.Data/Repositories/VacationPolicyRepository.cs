using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories {
    public class VacationPolicyRepository : IRepository<VacationPolicyInfo> {
        private ApplicationDbContext context;
        private bool disposed = false;

        public VacationPolicyRepository(ApplicationDbContext appcontext)
        {
            this.context = appcontext;
        }

        public void Create(VacationPolicyInfo item)
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

        public VacationPolicyInfo Get(int id)
        {
            var response = context.VacationPolicies
                .Include(v => v.TimeOffTypes)
                .Include(v => v.UserInformation)
                .FirstOrDefault();

            return response;
        }

        public IEnumerable<VacationPolicyInfo> GetAll()
        {
            return context.VacationPolicies
                .Include(v => v.TimeOffTypes)
                .Include(v => v.UserInformation)
                    .ThenInclude(v => v.VacationPolicyInfo)
                .ToList();
        }

        public void Update(VacationPolicyInfo item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
