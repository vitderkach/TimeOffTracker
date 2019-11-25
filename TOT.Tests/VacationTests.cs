using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TOT.Business.Services;
using TOT.Data;
using TOT.Data.UnitOfWork;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Repositories;
using TOT.Interfaces.Services;
using TOT.Utility.AutoMapper;
using TOT.Web.Controllers;

namespace TOT.Tests {
    [TestClass]
    public class VacationTests {

        TOTAutoMapper _mapper = new TOTAutoMapper();

        List<VacationRequestDto> listVacationDto;
        List<VacationRequest> listVacation;
        VacationRequestDto vacationRequest;
        List<int> vacationsId;

        [TestInitialize]
        public void Initialize()
        {
            listVacationDto = new List<VacationRequestDto>() {
                new VacationRequestDto() { VacationRequestId = 1, Notes = "test1",
                                            VacationType = TimeOffType.SickLeave },
                new VacationRequestDto() {VacationRequestId = 2, Notes = "test2", 
                                            VacationType = TimeOffType.StudyLeave },
                new VacationRequestDto() {VacationRequestId = 3, Notes = "test3",
                                            VacationType = TimeOffType.UnpaidVacation }
            };
            listVacation = new List<VacationRequest>() {
                new VacationRequest() { VacationRequestId = 1, Notes = "test1", 
                                        VacationType = TimeOffType.SickLeave },
                new VacationRequest() {VacationRequestId = 2, Notes = "test2",
                                        VacationType = TimeOffType.StudyLeave  },
                new VacationRequest() {VacationRequestId = 3, Notes = "test3", 
                                        VacationType = TimeOffType.UnpaidVacation }
          };
            vacationsId = new List<int>() { 1, 2, 3 };
            vacationRequest = listVacationDto[0];
        }

        [TestMethod]
        public void VacationGetById()
        {
            var expectedDto = new VacationRequestDto() { 
                VacationRequestId = 1,
                VacationType = TimeOffType.SickLeave
            };

            var expected = new VacationRequest()
            {
                VacationRequestId = 1,
                VacationType = TimeOffType.SickLeave
            };

            var mockVacation = new Mock<IVacationService>();
            mockVacation.Setup(v => v.GetVacationById(1))
                .Returns(expectedDto);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.VacationRequestRepository.Get(1))
                .Returns(expected);

            VacationService vacationService = new VacationService(
                _mapper,
                uowMock.Object,
                null,
                null
                );

            var actual = vacationService.GetVacationById(1);
            Assert.AreEqual(expectedDto.VacationRequestId, actual.VacationRequestId);
            Assert.AreEqual(expectedDto.VacationType, actual.VacationType);
        }
    }
}
