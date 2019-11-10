using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOT.Entities;
using TOT.Interfaces.Repositories;
using TOT.Web.Controllers;
using Xunit;

namespace TOT.Tests.Controller {
    /* public class VacationControllerTests {
         [Fact]
          public void VacationRequest_Returns()
          {
              // Arrange
              var mock = new Mock<IRepository<VacationRequest>>();
              mock.Setup(repo => repo.GetAll()).Returns(GetVacationRequests());
              var controller = new VacationController(mock.Object);

              // Act
              var result = controller.Index();

              // Assert
              var viewResult = Assert.IsType<ViewResult>(result);
              var model = Assert.IsAssignableFrom<IEnumerable<VacationRequest>>(viewResult.Model);
              Assert.Equal(GetVacationRequests().Count, model.Count());

          }
          private ICollection<VacationRequest> GetVacationRequests()
          {
              ICollection<VacationRequest> vacations = new List<VacationRequest>();
              vacations.Add(new VacationRequest()
              {
                  //StartDate = DateTime.
              });
          }
      }*/
}
