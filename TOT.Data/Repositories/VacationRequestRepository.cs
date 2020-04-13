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
    internal class VacationRequestRepository : IVacationRequestRepository<BaseVacationRequest, VacationRequest, VacationRequestHistory>
    {
        private readonly ApplicationDbContext _context;
        public VacationRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(VacationRequest item) =>_context.VacationRequests.Add(item);


        public VacationRequest GetOne(int id) => _context.VacationRequests.Find(id);

        public ICollection<VacationRequest> GetAll() => _context.VacationRequests.ToList();

        public void Update(VacationRequest item, params Expression<Func<VacationRequest, object>>[] updatedProperties)
        {
            var entry = _context.Entry<VacationRequest>(_context.VacationRequests.Find(item.VacationRequestId));
            entry.CurrentValues.SetValues(item);
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
                    var original = entry.Property(property.Name).OriginalValue;
                    var current = entry.Property(property.Name).CurrentValue;
                    if (original != null && !original.Equals(current))
                        entry.Property(property.Name).IsModified = true;
                }
            }
        }

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

        public VacationRequest GetOne(Expression<Func<VacationRequest, bool>> filterExpression, params Expression<Func<VacationRequest, object>>[] includes)
        {
            IQueryable<VacationRequest> query = _context.VacationRequests;
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

        public ICollection<VacationRequest> GetAll(Expression<Func<VacationRequest, bool>> filterExpression, params Expression<Func<VacationRequest, object>>[] includes)
        {
            IQueryable<VacationRequest> query = _context.VacationRequests;
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

        public ICollection<VacationRequestHistory> GetHistoryForOne(int id)
           => _context.VacationRequestHistories.FromSql(
               $@"SELECT VacationRequestId, StartDate, EndDate, VacationType, Notes, Approval, 
                SystemStart, SystemEnd, UserInformationId, StageOfApproving, SelfCancelled, TakenDays
                FROM dbo.VacationRequests
                FOR SYSTEM_TIME ALL
                WHERE VacationRequestId = {id};")
                .ToList();

        public ICollection<VacationRequestHistory> GetHistoryForAll()
           => _context.VacationRequestHistories.FromSql(
               $@"SELECT VacationRequestId, StartDate, EndDate, VacationType, Notes, Approval, 
                SystemStart, SystemEnd, UserInformationId, StageOfApproving, SelfCancelled, TakenDays
                FROM dbo.VacationRequests
                FOR SYSTEM_TIME ALL;").ToList();
    }
}
