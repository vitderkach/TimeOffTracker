using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;
using System.Linq;

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

        public void Update(Team item) => _context.Entry<Team>(item).State = EntityState.Modified;
    }
}
