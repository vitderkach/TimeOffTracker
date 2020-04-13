using System;
using System.Collections.Generic;
using System.Text;
using TOT.Data.Repositories;
using TOT.DataImport.Interfaces;
using TOT.Entities;

namespace TOT.DataImport.StorageProviders
{
    public class DbStorageProvider : IStorageProvider
    {
        private UserInformationRepository UserRepository { get; }
        private VacationRequestRepository VacationRequestRepository { get; }
        private VacationTypeRepository VacationTypeRepository { get; }

        public DbStorageProvider()
        {

        }

        public DbStorageProvider(
            UserInformationRepository userRepository,
            VacationRequestRepository vacationRequestRepository,
            VacationTypeRepository vacationTypeRepository)
        {
            UserRepository = userRepository;
            VacationRequestRepository = vacationRequestRepository;
            VacationTypeRepository = vacationTypeRepository;
        }

        public IEmployeeStorageProvider AddEmployeeIfNotExists(string name, DateTime? employmentDate = null, string teamName = null, string workPlace = null)
        {
            throw new NotImplementedException();
        }

        public void AddGiftDays(int employeeId, int year, int giftDays)
        {
            throw new NotImplementedException();
        }

        public void AddVacation(int employeeId, DateTime from, DateTime to, TimeOffType type)
        {
            throw new NotImplementedException();
        }

        public void AddVacationDays(int employeeId, int year, int vacationDays)
        {
            throw new NotImplementedException();
        }
    }
}
