using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;
using System.Linq;
using System.Linq.Expressions;

namespace TOT.Data.Repositories
{
    internal class TeamRepository : ITeamRepository<Team>
    {

        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            if (_context.Teams.Find(id) is Team team)
            {
                _context.Teams.Remove(team);
            }
        }

        public ICollection<Team> GetAll() => _context.Teams.ToList();

        public Team GetOne(int id) => _context.Teams.Find(id);

        public void Create(Team item)
        {
            _context.Teams.Add(item);
        }

        public void Update(Team item, params Expression<Func<Team, object>>[] updatedProperties)
        {
            var entry = _context.Entry<Team>(item);
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

        public Team GetOne(Expression<Func<Team, bool>> filterExpression, params Expression<Func<Team, object>>[] includes)
        {
            IQueryable<Team> query = _context.Teams;
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

        public ICollection<Team> GetAll(Expression<Func<Team, bool>> filterExpression, params Expression<Func<Team, object>>[] includes)
        {
            IQueryable<Team> query = _context.Teams;
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
