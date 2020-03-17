﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TOT.Entities;

namespace TOT.Interfaces.Repositories
{

    public interface ICanDeleteEntity<TEntity> where TEntity : class
    {
        void Delete(int id);
    }

    public interface ICanUpdateEntity<TEntity> where TEntity : class
    {
        void Update(TEntity item, params Expression<Func<TEntity, object>>[] updatedProperties);
    }

    public interface ICanCreateEntity<TEntity> where TEntity : class
    {
        void Create(TEntity item);
    }

    public interface ICanGetEntity<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAll();
        TEntity GetOne(int id);
        TEntity GetOne(Expression<Func<TEntity, bool>> filterExpression);
        ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> filterExpression);
    }

    public interface IManagerResponseRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        void TransferToHistory(int id);

        TEntity GetOneWithUserInfoAndVacationRequest(int id);
        ICollection<TEntity> GetAllWithUserInfoAndVacationRequest();
    }

    public interface ITeamRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IUserInformationRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        TEntity GetOneWithVacationRequests(int id);
        ICollection<TEntity> GetAllWithVacationsRequests();

        TEntity GetOneWithTeamAndLocation(int id);
        ICollection<TEntity> GetAllWithTeamAndLocation();

        TEntity GetOneWithAllProperties(int id);
        ICollection<TEntity> GetAllWithAllProperties();
    }

    public interface ILocationRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IVacationRequestRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        void TransferToHistory(int id);

        TEntity GetOneWithManagerResponcesAndUserInfo(int vacationRequestId);
        ICollection<TEntity> GetAllWithManagerResponcesAndUserInfo();
    }

    public interface IVacationTypeRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

        ICollection<TEntity> GetAllWithUserInformationTeamAndLocation();
        TEntity GetOneWithUserInformationTeamAndLocation(int id);
    }


}
