using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TOT.Business.Services;
using TOT.Data.Repositories;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Utility.AutoMapper;

namespace TOT.Tests {

    [TestClass]
    public class UserTests {

        TOTAutoMapper _mapper = new TOTAutoMapper();
        List<UserInformation> listUserInfo;

        [TestInitialize]
        public void Initialize()
        {
            listUserInfo = new List<UserInformation>()
            {
                new UserInformation(){FirstName = "A", ApplicationUserId = 1},
                new UserInformation(){FirstName = "B", ApplicationUserId = 2},
                new UserInformation(){FirstName = "C", ApplicationUserId = 3},
                new UserInformation(){FirstName = "D", ApplicationUserId = 4},
            };
        }

        [TestMethod]
        public void GetUserInfo_ReturnsUserInfoById()
        {
            //arrange
            var expected = listUserInfo[0];
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.UserInformationRepository.GetOne(1))
                .Returns(listUserInfo[0]);

            var userInfoService = new UserInformationService(mockUow.Object, _mapper);

            //act
            var actual = userInfoService.GetUserInfo(1);

            //assert
            mockUow.Verify(m => m.UserInformationRepository.GetOne(1));
            Assert.AreEqual(expected.ApplicationUserId, actual.UserInformationId);
        }

        [TestMethod]
        public void GetUserInfo_ThrowsNotExistsInfoUser()
        {
            //arrange
            var expected = listUserInfo[0];
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.UserInformationRepository.GetOne(5));
                
            var userInfoService = new UserInformationService(mockUow.Object, _mapper);

            //act
            try
            {
                var actual = userInfoService.GetUserInfo(1);
            }
            //assert
            catch(NullReferenceException ex)
            {
                string message = ex.Message;
                if (message == "userInfo not found")
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void GetUserInfo_ThrowsNotIdInParams()
        {
            //arrange
            var expected = listUserInfo[0];
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(m => m.UserInformationRepository.GetOne(5));

            var userInfoService = new UserInformationService(mockUow.Object, _mapper);

            //act
            try
            {
                var actual = userInfoService.GetUserInfo(null);
            }
            //assert
            catch (NullReferenceException ex)
            {
                string message = ex.Message;
                if (message == "id = null")
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
        }
    }
}
