using System;
using System.Collections.Generic;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;

namespace TOT.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<VacationRequest> VacationRequestRepository { get; }
        IRepository<UserInformation> UserInformationRepository { get; }
        IRepository<ManagerResponse> ManagerResponseRepository { get; }
        IRepository<VacationPolicy> VacationPolicyRepository { get; }
        IRepository<VacationType> VacationTypeRepository { get; }
        void Save();
    }
}
