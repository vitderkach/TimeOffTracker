using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    internal class UserInformationRepository : IUserInformationRepository<UserInformation>
    {
        private readonly ApplicationDbContext _context;
        public UserInformationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(UserInformation item) => _context.UserInformations.Add(item);

        public UserInformation GetOne(int id)
            => _context.UserInformations
            .Where(ui => ui.IsFired == false && ui.ApplicationUserId == id)
            .FirstOrDefault();

        public ICollection<UserInformation> GetAll()
            => _context.UserInformations
            .Where(ui => ui.IsFired == false)
            .ToList();

        public void Update(UserInformation item, params Expression<Func<UserInformation, object>>[] updatedProperties)
        {
            var entry = _context.Entry<UserInformation>(item);
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

        public UserInformation GetOneWithAllProperties(int id)
            => _context.UserInformations
            .Where(ui => ui.ApplicationUserId == id && ui.IsFired == false)
            .Include(ui => ui.VacationTypes)
            .Include(ui => ui.Team)
            .Include(ui => ui.Location)
            .FirstOrDefault();

        public ICollection<UserInformation> GetAllWithAllProperties()
            => _context.UserInformations
            .Where(ui => ui.IsFired == false)
            .Include(ui => ui.VacationTypes)
            .Include(ui => ui.Team)
            .Include(ui => ui.Location)
            .ToList();

        public UserInformation GetOne(Expression<Func<UserInformation, bool>> filterExpression, params Expression<Func<UserInformation, object>>[] includes)
        {
            IQueryable<UserInformation> query = _context.UserInformations;
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

        public ICollection<UserInformation> GetAll(Expression<Func<UserInformation, bool>> filterExpression, params Expression<Func<UserInformation, object>>[] includes)
        {
            IQueryable<UserInformation> query = _context.UserInformations;
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
