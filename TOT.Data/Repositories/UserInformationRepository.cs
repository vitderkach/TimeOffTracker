using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class UserInformationRepository : IUserInformationRepository<UserInformation>
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

        public void Update(UserInformation item) => _context.Entry<UserInformation>(item).State = EntityState.Modified;

        public void Fire(int id)
        {
            if (_context.UserInformations.Find(id) is UserInformation userInformation)
            {            
                var d = _context.UserInformations.Attach(userInformation).Property("IsFired");
                userInformation.IsFired = true;
            }
        }

        public UserInformation GetFiredOne(int id)
            => _context.UserInformations
            .Where(ui => ui.IsFired == false && ui.ApplicationUserId == id)
            .FirstOrDefault();

        public ICollection<UserInformation> GetFiredAll()
            => _context.UserInformations
            .Where(ui => ui.IsFired == false)
            .ToList();

        public UserInformation GetOneWithVacationRequests(int id)
            => _context.UserInformations
            .Where(ui => ui.ApplicationUserId == id && ui.IsFired == false)
            .Include(ui => ui.VacationTypes)
            .FirstOrDefault();

        public ICollection<UserInformation> GetAllWithVacationsRequests()
            => _context.UserInformations
            .Where(ui => ui.IsFired == false)
            .Include(ui => ui.VacationTypes)
            .ToList();

        public UserInformation GetOneWithTeamAndLocation(int id)
            => _context.UserInformations
            .Where(ui => ui.ApplicationUserId == id && ui.IsFired == false)
            .Include(ui => ui.Team)
            .Include(ui => ui.Location)
            .FirstOrDefault();

        public ICollection<UserInformation> GetAllWithTeamAndLocation()
            => _context.UserInformations
            .Where(ui => ui.IsFired == false)
            .Include(ui => ui.Team)
            .Include(ui => ui.Location)
            .ToList();

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
    }
}
