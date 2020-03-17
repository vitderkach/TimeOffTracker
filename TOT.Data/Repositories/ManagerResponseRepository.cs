using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{

    internal class ManagerResponseRepository : IManagerResponseRepository<ManagerResponse>
    {
        private readonly ApplicationDbContext _context;
        public ManagerResponseRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(ManagerResponse item) => _context.ManagerResponses.Add(item);

        public ManagerResponse GetOne(int id) => _context.ManagerResponses.Find(id);

        public ICollection<ManagerResponse> GetAll() => _context.ManagerResponses.ToList();

        public void Update(ManagerResponse item) => _context.Entry<ManagerResponse>(item).State = EntityState.Modified;

        public void TransferToHistory(int id)
        {
            if (_context.ManagerResponses.Find(id) is ManagerResponse managerResponse)
            {
                _context.Remove(managerResponse);
            }
        }

        public ManagerResponse GetOneWithUserInfoAndVacationRequest(int id)
            => _context.ManagerResponses
            .Where(mr => mr.Id == id)
            .Include(mr => mr.VacationRequest)
            .Include(mr => mr.Manager).FirstOrDefault();

        public ICollection<ManagerResponse> GetAllWithUserInfoAndVacationRequest()
            => _context.ManagerResponses
            .Include(mr => mr.VacationRequest)
            .Include(mr => mr.Manager).ToList();
    }
}
