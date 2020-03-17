using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Interfaces
{
    public interface IUnitOfWork
    {
        IVacationRequestRepository<VacationRequest> VacationRequestRepository { get; }

        IUserInformationRepository<UserInformation> UserInformationRepository { get; }

        IManagerResponseRepository<ManagerResponse> ManagerResponseRepository { get; }

        IVacationTypeRepository<VacationType> VacationTypeRepository { get; }

        ILocationRepository<Location> LocationRepository { get; }

        ITeamRepository<Team> TeamRepository { get; }

        void Dispose();

        void Save();
    }
}
