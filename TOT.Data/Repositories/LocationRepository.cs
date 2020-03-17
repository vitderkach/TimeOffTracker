using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public void Update(Location item) => _context.Entry<Location>(item).State = EntityState.Modified;
    }
}
