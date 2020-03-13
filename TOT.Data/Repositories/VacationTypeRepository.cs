using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class VacationTypeRepository : IVacationTypeRepository<VacationType>
    {
        private readonly ApplicationDbContext _context;
        public VacationTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            if (_context.VacationTypes.Find(id) is VacationType vacationType)
            {
                _context.VacationTypes.Remove(vacationType);
            }
        }

        public VacationType GetOne(int id) => _context.VacationTypes.Find(id);

        public IEnumerable<VacationType> GetAll() => _context.VacationTypes.ToList();
       
        public void Update(VacationType item) =>_context.Entry<VacationType>(item).State = EntityState.Modified;

        public void Create(VacationType item) => _context.VacationTypes.Add(item);
    }
}
