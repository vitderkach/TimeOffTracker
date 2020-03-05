using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories {
    public class VacationTypeRepository : IRepository<VacationType> {
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

        // TODO: Rewrite the mehod because the database logic has been changed. As an example the commented code below

         public VacationType Get(int id)
        {
            throw new NotImplementedException();
        }

        //public VacationType Get(int id)
        //{
        //    var response = context.VacationTypes
        //        .Include(v => v.VacationPolicy)
        //        .FirstOrDefault();

        //    return response;
        //}

        // TODO: Rewrite the mehod because the database logic has been changed. As an example the commented code below

        public IEnumerable<VacationType> GetAll()
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<VacationType> GetAll()
        //{
        //    return context.VacationTypes
        //        .Include(v => v.VacationPolicy)
        //        .Include(v => v.VacationPolicy)
        //            .ThenInclude(v => v.VacationTypes)
        //        .ToList();
        //}

        public void Update(VacationType item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
