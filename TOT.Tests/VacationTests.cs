using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOT.Business.Services;
using TOT.Data;
using TOT.Data.Repositories;
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
                                            VacationType = TimeOffType.ConfirmedSickLeave },
                new VacastionRequestDto() {VacationRequestId = 2, Notes = "test2",
                                            VacationType = TimeOffType.StudyLeave },
                new VacationRequestDto() {VacationRequestId = 3, Notes = "test3",
                                            VacationType = TimeOffType.AdministrativeLeave }
            };
            listVacation = new List<VacationRequest>() {
                new VacationRequest() { VacationRequestId = 1, Notes = "test1",
                                        VacationType = TimeOffType.ConfirmedSickLeave,
                UserInformationId = 1 },
                new VacationRequest() {VacationRequestId = 2, Notes = "test2",
                                        VacationType = TimeOffType.StudyLeave,
                UserInformationId = 2 },
                new VacationRequest() {VacationRequestId = 3, Notes = "test3",
                                        VacationType = TimeOffType.AdministrativeLeave,
                UserInformationId = 3 }
          };
            vacationsId = new List<int>() { 1, 2, 3 };
            vacationRequest = listVacationDto[0];
        }

        [TestMethod]
        public void VacationGetByIdReturnsCorrect()
        {
            //arrange
            var expectedDto = new VacationRequestDto() { 
                VacationRequestId = 1,
                VacationType = TimeOffType.ConfirmedSickLeave
            };

            var expected = new VacationRequest()
            {
                VacationRequestId = 1,
                VacationType = TimeOffType.ConfirmedSickLeave
            };

            var mockVacation = new Mock<IVacationService>();
            mockVacation.Setup(v => v.GetVacationById(1))
                .Returns(expectedDto);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.VacationRequestRepository.GetOne(1))
                .Returns(expected);

            VacationService vacationService = new VacationService(
                _mapper,
                uowMock.Object,
                null,
                null
                );

            //act
            var actual = vacationService.GetVacationById(1);

            //assert
            uowMock.Verify(u => u.VacationRequestRepository.GetOne(It.IsAny<int>()));
            Assert.AreEqual(expectedDto.VacationRequestId, actual.VacationRequestId);
            Assert.AreEqual(expectedDto.VacationType, actual.VacationType);
        }

        [TestMethod]
        public void GetAllVacationsByUserIdReturnsCorrect()
        {
            //arrange
            int expected = 1;
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.VacationRequestRepository.GetAll())
                .Returns(listVacation);

            var service = new VacationService(_mapper, uowMock.Object, null, null);

            //act
            var actual = service.GetAllVacationIdsByUser(1).Count;
            
            //assert
            uowMock.Verify(u => u.VacationRequestRepository.GetAll());
            Assert.AreEqual(expected, actual);
        }

        // TODO: Rewrite the method: instead delete transfer to history
        //[TestMethod]
        // public void DeleteVacationIsCorrect()
        // {
        //     //arrange
        //     var uowMock = new Mock<IUnitOfWork>();
        //     var user = new ApplicationUser();
        //     listVacation[0].ApplicationUser = new ApplicationUser();

        //     uowMock.Setup(u => u.VacationRequestRepository.Delete(It.IsAny<int>()))
        //         .Verifiable();

        //     var service = new VacationService(_mapper, uowMock.Object, null, null);

        //     //act
        //     service.DeleteVacationById(1);

        //     //assert
        //     uowMock.Verify(u => u.VacationRequestRepository.Delete(1));
        // }

        //// TODO: Fix the method: add UserInformation entity example

        //[TestMethod]
        //public void UpdateVacation_UpdateVacationNotes()
        //{
        //    //arrange
        //    var uowMock = new Mock<IUnitOfWork>();

        //    listVacation[0].ApplicationUser = new ApplicationUser();

        //    uowMock.Setup(u => u.VacationRequestRepository.Update(It.IsAny<VacationRequest>()))
        //        .Verifiable();

        //    uowMock.Setup(u => u.VacationRequestRepository.GetOne(1))
        //        .Returns(new VacationRequest()
        //        {
        //            VacationRequestId = 1,
        //            Notes = "test1",
        //            VacationType = TimeOffType.ConfirmedSickLeave,
        //            ApplicationUserId = 1
        //        });

        //    var service = new VacationService(_mapper, uowMock.Object, null, null);

        //    //act
        //    service.UpdateVacation(1, "changed");

        //    //assert
        //    uowMock.Verify(u => u.VacationRequestRepository.Update(It.IsAny<VacationRequest>()));
        //}
    }
}
