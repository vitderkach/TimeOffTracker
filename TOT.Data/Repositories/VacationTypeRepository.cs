using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Update(VacationType item) => _context.Entry<VacationType>(item).State = EntityState.Modified;

        public void Create(VacationType item) => _context.VacationTypes.Add(item);

        public bool СhargeVacationDays(int userId, int count, TimeOffType vacationType, bool isAlreadyCharged)
        {
            if (count < 0)
            {
                throw new ArgumentException("The count of vacation days cannot be less than 0!");
            }
            var vacation = _context.VacationTypes.Where(vt => vt.UserInformationId == userId && vt.TimeOffType == vacationType).FirstOrDefault();
            if (isAlreadyCharged)
            {
                _context.VacationTypes.Attach(vacation).Property(gv => gv.StatutoryDays);
                vacation.StatutoryDays = count;
                return true;
            }
            else
            {
                if (vacation.StatutoryDays != 0)
                {
                    return false;
                }
                else
                {
                    _context.VacationTypes.Attach(vacation).Property(gv => gv.StatutoryDays);
                    vacation.StatutoryDays = count;
                    return true;
                }
            }
        }

        public bool UseVacationDays(int userId, int count, TimeOffType vacationType, bool overflowIsAllowed)
        {
            if (count < 0)
            {
                throw new ArgumentException("The count of used days cannot be less than 0!");
            }

            var vacation = _context.VacationTypes.Where(vt => vt.UserInformationId == userId && vt.TimeOffType == vacationType).FirstOrDefault();

            if (overflowIsAllowed)
            {

                _context.VacationTypes.Attach(vacation).Property(gv => gv.UsedDays);
                vacation.UsedDays += count;
                return true;
            }
            else
            {
                int allowedToUseDays = vacation.StatutoryDays - vacation.UsedDays;
                if (allowedToUseDays < count)
                {
                    return false;
                }
                else
                {
                    _context.VacationTypes.Attach(vacation).Property(gv => gv.UsedDays);
                    vacation.UsedDays += count;
                    return true;
                }
            }
        }

        public void ChangeCountOfGiftdays(int userId, int count)
        {
            var giftVacation = _context.VacationTypes.Where(vt => vt.UserInformationId == userId && vt.TimeOffType == TimeOffType.GiftLeave).FirstOrDefault();
            _context.VacationTypes.Attach(giftVacation).Property(gv => gv.StatutoryDays);
            giftVacation.StatutoryDays += count;

        }

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
    }
}
