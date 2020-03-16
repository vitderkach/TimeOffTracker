using System;
using System.Collections.Generic;
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
        void Update(TEntity item);
    }

    public interface ICanCreateEntity<TEntity> where TEntity : class
    {
        void Create(TEntity item);
    }

    public interface ICanGetEntity<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAll();
        TEntity GetOne(int id);
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
        void Fire(int id);
        TEntity GetFiredOne(int id);
        ICollection<TEntity> GetFiredAll();

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
        bool СhargeVacationDays(int userId, int count, TimeOffType vacationType, bool isAlreadyCharged);
        void ChangeCountOfGiftdays(int userId, int count);

        void UseVacationDays(int userId, int count, TimeOffType vacationType);

        ICollection<TEntity> GetAllWithUserInformationTeamAndLocation();

        TEntity GetOneWithUserInformationTeamAndLocation(int id);
    }


}
