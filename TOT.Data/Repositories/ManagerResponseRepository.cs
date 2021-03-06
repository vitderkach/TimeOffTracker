﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Data.Repositories
{

    internal class ManagerResponseRepository : IManagerResponseRepository<BaseManagerResponse, ManagerResponse, ManagerResponseHistory>
    {
        private readonly ApplicationDbContext _context;
        public ManagerResponseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(ManagerResponse item) => _context.ManagerResponses.Add(item);

        public ManagerResponse GetOne(int id) => _context.ManagerResponses.Find(id);

        public ICollection<ManagerResponse> GetAll() => _context.ManagerResponses.ToList();

        public void Update(ManagerResponse item, params Expression<Func<ManagerResponse, object>>[] updatedProperties)
        {
            var entry = _context.Entry<ManagerResponse>(item);
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

        public ManagerResponse GetOne(Expression<Func<ManagerResponse, bool>> filterExpression, params Expression<Func<ManagerResponse, object>>[] includes)
        {
            IQueryable<ManagerResponse> query = _context.ManagerResponses;
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

        public ICollection<ManagerResponse> GetAll(Expression<Func<ManagerResponse, bool>> filterExpression, params Expression<Func<ManagerResponse, object>>[] includes)
        {
            IQueryable<ManagerResponse> query = _context.ManagerResponses;
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

        public ManagerResponse GetOneWithVacationRequestAndUserInfo(Expression<Func<ManagerResponse, bool>> filterExpression)
        {
            IQueryable<ManagerResponse> query = _context.ManagerResponses;
            query = query.Include(mr => mr.VacationRequest).ThenInclude(vr => vr.UserInformation);
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.FirstOrDefault();
        }

        public ICollection<ManagerResponse> GetAllWithVacationRequestsAndUserInfos(Expression<Func<ManagerResponse, bool>> filterExpression)
        {
            IQueryable<ManagerResponse> query = _context.ManagerResponses;
            query = query.Include(mr => mr.VacationRequest).ThenInclude(vr => vr.UserInformation);
            if (filterExpression != null)
            {
                query = query.Where(filterExpression);
            }

            return query.ToList();
        }

        public ManagerResponse GetOneWithVacationRequestAndUserInfo(int id)
            => _context.ManagerResponses.Where(mr => mr.Id == id).Include(mr => mr.VacationRequest).ThenInclude(vr => vr.UserInformation).FirstOrDefault();

        public ICollection<ManagerResponse> GetAllWithVacationRequestsAndUserInfos()
            => _context.ManagerResponses.Include(mr => mr.VacationRequest).ThenInclude(vr => vr.UserInformation).ToList();

        public ICollection<ManagerResponseHistory> GetHistoryForOne(int id)
           =>  _context.ManagerResponseHistories.FromSql(
               $@"SELECT Id, Notes, DateResponse, VacationRequestId, ManagerId, Approval, SystemStart, SystemEnd, ForStageOfApproving
                FROM dbo.ManagerResponses
                FOR SYSTEM_TIME ALL WHERE Id = {id};")
                .ToList();

        public ICollection<ManagerResponseHistory> GetHistoryForAll()
            => _context.ManagerResponseHistories.FromSql($@"SELECT Id, Notes, DateResponse, VacationRequestId, ManagerId, Approval, SystemStart, SystemEnd, ForStageOfApproving
            FROM dbo.ManagerResponses
            FOR SYSTEM_TIME ALL;")
            .ToList();

        public ICollection<ManagerResponseHistory> GetHistoryForAllForDefinedVacationRequest(int vacationRequestId)
            => _context.ManagerResponseHistories.FromSql(
                $@"SELECT Id, Notes, DateResponse, VacationRequestId, ManagerId, Approval, SystemStart, SystemEnd, ForStageOfApproving
                FROM dbo.ManagerResponses
                FOR SYSTEM_TIME ALL WHERE VacationRequestId = {vacationRequestId};")
                .ToList();
    }
}
