using System;
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

    public interface TemporalEntity<TEntity> where TEntity : class
    {
        void TransferToHistory(int id);
        ICollection<TEntity> GetHistoryForOne(int id);
        ICollection<TEntity> GetHistoryForAll();
    }

    public interface ICanGetEntity<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAll();
        TEntity GetOne(int id);
        TEntity GetOne(Expression<Func<TEntity, bool>> filterExpression, params Expression<Func<TEntity, object>>[] includesl);
        ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> filterExpression, params Expression<Func<TEntity, object>>[] includes);
    }

    public interface IManagerResponseRepository<BaseEntity, TEntity, HEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>, TemporalEntity<HEntity>
       where BaseEntity:class where TEntity : class, BaseEntity where HEntity : class, BaseEntity
    {
        ICollection<TEntity> GetAllWithVacationRequestsAndUserInfos();
        TEntity GetOneWithVacationRequestAndUserInfo(int id);
        ICollection<TEntity> GetAllWithVacationRequestsAndUserInfos(Expression<Func<ManagerResponse, bool>> filterExpression);
        TEntity GetOneWithVacationRequestAndUserInfo(Expression<Func<ManagerResponse, bool>> filterExpression);
        ICollection<HEntity> GetHistoryForAllForDefinedVacationRequest(int vacationRequestId);
    }

    public interface ITeamRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IUserInformationRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        TEntity GetOneWithAllProperties(int id);
        ICollection<TEntity> GetAllWithAllProperties();
    }

    public interface ILocationRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IAuxiliaryRepository
    {
        ICollection<int> GetHistoryManagerIdentificators(int historyVacationRequestId, DateTime systemStart);
    }

    public interface IVacationRequestRepository<BaseEntity, TEntity, HEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>, TemporalEntity<HEntity>
        where BaseEntity : class where TEntity : class, BaseEntity where HEntity : class, BaseEntity
    {
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
