using System;
using System.Collections.Generic;
using System.Text;
using TOT.Data.Repositories;
using TOT.Interfaces.Services;
using TOT.DataImport.Interfaces;
using TOT.DataImport.StorageProviders;

namespace TOT.Business.Services
{
    class ExcelDataImporterService : IExcelDataImporterService
    {
        private IStorageProvider StorageProvider { get; }

        public ExcelDataImporterService(
            UserInformationRepository userRepository,
            VacationRequestRepository vacationRequestRepository,
            VacationTypeRepository vacationTypeRepository)
        {
            StorageProvider = new DbStorageProvider(
                userRepository,
                vacationRequestRepository,
                vacationTypeRepository);
        }
    }
}
