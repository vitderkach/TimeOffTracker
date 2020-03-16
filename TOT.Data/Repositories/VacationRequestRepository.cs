using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{
    public class VacationRequestRepository : IVacationRequestRepository<VacationRequest>
    {
        private readonly ApplicationDbContext _context;
        public VacationRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(VacationRequest item) =>_context.VacationRequests.Add(item);


        public VacationRequest GetOne(int id) => _context.VacationRequests.Find(id);

        public ICollection<VacationRequest> GetAll() => _context.VacationRequests.ToList();

        public void Update(VacationRequest item) => _context.Entry<VacationRequest>(item).State = EntityState.Modified;

        public void TransferToHistory(int id)
        {
            if (_context.VacationRequests.Find(id) is VacationRequest vacationRequest)
            {
                _context.Remove(vacationRequest);
            }
        }

        public VacationRequest GetOneWithManagerResponcesAndUserInfo(int id)
            => _context.VacationRequests
            .Where(vr => vr.VacationRequestId == id)
            .Include(vr => vr.ManagersResponses)
            .ThenInclude(mr => mr.Manager).FirstOrDefault();

        public  ICollection<VacationRequest> GetAllWithManagerResponcesAndUserInfo()
            => _context.VacationRequests
            .Include(vr => vr.ManagersResponses)
            .ThenInclude(mr => mr.Manager).ToList();
    }
}
