using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TOT.Tests.DataImport.Excel
{
    [TestClass]
    class ExcelDataImporterTests
    {
        string filePath = "DataImport/Excel/2020.xlsx";

        [TestInitialize]
        public void Initialize()
        {
            var newFile = @"Excel/2020.xlsx";
            using (var fileStream = new FileStream(newFile, FileMode.Open, FileAccess.Read))
            {
                IExcelDataImporter dataImporter = new ExcelDataImporter();
                dataImporter
                    .SetConfiguration(new ExcelDataImporterConfiguration())
                    .ImportFromStream(fileStream)
                    .SaveToStorage(new DbStorageProvider())
                    .Start();
            }
            
        }

        [TestMethod]
        public void CheckManagerResponsesForVacation_ReturnsThatIsRequestedForCurrentUser()
        {
            //arrange
            var expectedId = 5;
            var expected = new List<ManagerResponseListDto>()
            {
                new ManagerResponseListDto() { VacationRequestId = 5 }
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetCurrentUser())
                .Returns(Task.FromResult(user));
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.ManagerResponseRepository.GetAll())
                .Returns(managerResponses);

            var service = new ManagerService(_mapper, mockUow.Object, null, null, mockUserService.Object);
            //act
            var actual = service.GetAllCurrentManagerResponses();
            //assert
            mockUow.Verify(m => m.ManagerResponseRepository.GetAll());
            Assert.AreEqual(expected.Count, actual.Count());
            Assert.AreEqual(expectedId, expected.FirstOrDefault().VacationRequestId);
        }

        [TestMethod]
        public void GetResponseActiveByVacationId_ReturnsManagerResponseThatIsActive()
        {
            //arrange
            var expected = new ManagerResponseDto() { Id = 2, isRequested = true, ManagerId = 1 };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetCurrentUser())
                .Returns(Task.FromResult(user));

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.ManagerResponseRepository.GetAll())
                .Returns(managerResponses);

            var service = new ManagerService(_mapper, mockUow.Object, null, null, mockUserService.Object);
            //act
            var actual = service.GetResponseActiveByVacationId(5);
            //assert
            mockUow.Verify(m => m.ManagerResponseRepository.GetAll());
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ManagerId, actual.ManagerId);
            Assert.AreEqual(expected.isRequested, actual.isRequested);
        }

        [TestMethod]
        public void GetProcessedRequestsByCurrentManager_ReturnsManagerResponsesThatNeedToReview()
        {
            //arrange
            var expected = new List<ManagerResponseDto>()
            {
                new ManagerResponseDto()
                {
                    Id = 1,
                    isRequested = true,
                    ManagerId = 1
                }
            };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetCurrentUser())
                .Returns(Task.FromResult(user));

            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.ManagerResponseRepository.GetAll())
                .Returns(managerResponses);

            var service = new ManagerService(_mapper, mockUow.Object, null, null, mockUserService.Object);
            //act

            var actual = service.GetProcessedRequestsByCurrentManager();
            //assert
            mockUow.Verify(m => m.ManagerResponseRepository.GetAll());
            Assert.AreEqual(expected.Count(), actual.Count());
            Assert.AreEqual(expected.FirstOrDefault().Id, actual.FirstOrDefault().Id);
        }
    }
}