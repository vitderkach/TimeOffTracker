using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOT.Business.Services;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Utility.AutoMapper;

namespace TOT.Tests {
    [TestClass]
    public class ManagerTests {
        TOTAutoMapper _mapper = new TOTAutoMapper();
        ApplicationUser user;
        List<ManagerResponse> managerResponses;

        [TestInitialize]
        public void Initialize()
        {
            user = new ApplicationUser() { Id = 1, };

            managerResponses = new List<ManagerResponse>()
            {
                new ManagerResponse() {Id = 1, ManagerId = 1, isRequested = false, Approval = true, VacationRequestId = 10,  VacationRequest = new VacationRequest() { VacationType = TimeOffType.ConfirmedSickLeave }  },
                new ManagerResponse() {Id = 2, ManagerId = 1, isRequested = true , Approval = null, VacationRequestId = 5,  VacationRequest = new VacationRequest() { VacationType = TimeOffType.ConfirmedSickLeave }},
                new ManagerResponse() {Id = 3, ManagerId = 2, isRequested = true , Approval = null, VacationRequestId = 11,  VacationRequest = new VacationRequest() { VacationType = TimeOffType.ConfirmedSickLeave }},
                new ManagerResponse() {Id = 4, ManagerId = 3, isRequested = true , Approval = null, VacationRequestId = 12,  VacationRequest = new VacationRequest() { VacationType = TimeOffType.ConfirmedSickLeave }},
            };

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
