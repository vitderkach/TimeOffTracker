using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TOT.Data.Repositories
{
    internal class LocationRepository : ILocationRepository<Location>
    {
        private readonly ApplicationDbContext _context;
        public LocationRepository(ApplicationDbContext context) => _context = context;

        public void Delete(int id)
        {
            if (_context.Locations.Find(id) is Location location)
            {
                _context.Locations.Remove(location);
            }
        }

        public ICollection<Location> GetAll() => _context.Locations.ToList();

        public Location GetOne(int id) => _context.Locations.Where(l => l.Id == id).FirstOrDefault();

        public void Create(Location item)
        {
            _context.Locations.Add(item);
        }

        public void Update(Location item, params Expression<Func<Location, object>>[] updatedProperties)
        {
            var entry = _context.Entry<Location>(item);
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

        public Location GetOne(Expression<Func<Location, bool>> filterExpression, params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> query = _context.Locations;
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

        public ICollection<Location> GetAll(Expression<Func<Location, bool>> filterExpression, params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> query = _context.Locations;
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
