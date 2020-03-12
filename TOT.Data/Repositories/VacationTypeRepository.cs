using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class VacationTypeRepository : IRepository<VacationType>
    {
        private ApplicationDbContext context;
        private bool disposed = false;

        public VacationTypeRepository(ApplicationDbContext appcontext)
        {
            this.context = appcontext;
        }

        public void Create(VacationType item)
        {
            context.VacationTypes.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var response = context.VacationTypes
                   .Find(id);
            if (response != null)
            {
                context.VacationTypes.Remove(response);
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

        public VacationType GetOne(int id) => context.VacationTypes.Where(vt => vt.UserInformationId == id).Include(vt => vt.UserInformation).FirstOrDefault();

        public IEnumerable<VacationType> GetAll() => context.VacationTypes.ToList();

        public void Update(VacationType item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
