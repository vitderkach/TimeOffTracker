using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    internal class VacationTypeRepository : IVacationTypeRepository<VacationType>
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

        public ICollection<VacationType> GetAll() => _context.VacationTypes.ToList();

        public void Update(VacationType item, params Expression<Func<VacationType, object>>[] updatedProperties)
        {
            var entry = _context.Entry<VacationType>(item);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    entry.Property(property).IsModified = true;
                }
            }
            else
            {
                foreach (var property in entry.OriginalValues.Properties)
                {
                    var original = entry.OriginalValues.GetValue<object>(property);
                    var current = entry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current))
                        entry.Property(property.Name).IsModified = true;
                }
            }
        }

        public void Create(VacationType item) => _context.VacationTypes.Add(item);

        public ICollection<VacationType> GetAllWithUserInformationTeamAndLocation()
            => _context.VacationTypes
            .Include(vt => vt.UserInformation)
            .ThenInclude(ui => ui.Team)
            .Include(vt => vt.UserInformation)
            .ThenInclude(vt => vt.Location)
            .ToList();

        public VacationType GetOneWithUserInformationTeamAndLocation(int id)
            => _context.VacationTypes
            .Where(vt => vt.Id == id)
            .Include(vt => vt.UserInformation)
            .ThenInclude(ui => ui.Team)
            .Include(vt => vt.UserInformation)
            .ThenInclude(vt => vt.Location)
            .FirstOrDefault();

        public VacationType GetOne(Expression<Func<VacationType, bool>> filterExpression, params Expression<Func<VacationType, object>>[] includes)
        {
            IQueryable<VacationType> query = _context.VacationTypes;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.FirstOrDefault();
        }

        public ICollection<VacationType> GetAll(Expression<Func<VacationType, bool>> filterExpression, params Expression<Func<VacationType, object>>[] includes)
        {
            IQueryable<VacationType> query = _context.VacationTypes;
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.ToList();
        }
    }
}
