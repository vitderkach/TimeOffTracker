using System;
using System.Collections.Generic;
using System.Text;

namespace TOT.Interfaces.Repositories
{

    public interface ICanDeleteEntity<TEntity> where TEntity : class
    {
        void Delete(int id);
    }

    public interface ICanUpdateEntity<TEntity> where TEntity : class
    {
        void Update(TEntity item);
    }

    public interface ICanCreateEntity<TEntity> where TEntity : class
    {
        void Create(TEntity item);
    }

    public interface ICanGetEntity<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetOne(int id);
    }

    public interface IManagerResponseRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        void TransferToHistory(int id);
    }

    public interface ITeamRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IUserInformationRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        void Fire(int id);
        TEntity GetFiredOne(int id);
        IEnumerable<TEntity> GetFiredAll();

        TEntity GetOneWithVacationRequests(int id);
        IEnumerable<TEntity> GetAllWithVacationsRequests();

        TEntity GetOneWithTeamAndLocation(int id);
        IEnumerable<TEntity> GetAllWithTeamAndLocation();

        TEntity GetOneWithAllProperties(int id);
        IEnumerable<TEntity> GetAllWithAllProperties();
    }

    public interface ILocationRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }

    public interface IVacationRequestRepository<TEntity> : ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {
        void TransferToHistory(int id);
    }

    public interface IVacationTypeRepository<TEntity> : ICanDeleteEntity<TEntity>, ICanUpdateEntity<TEntity>, ICanCreateEntity<TEntity>, ICanGetEntity<TEntity>
        where TEntity : class
    {

    }


}
