using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOT.Dto;
using TOT.Interfaces.Services;
using TOT.Web.Models;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Manager, Administrator")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        private readonly IVacationService _vacationService;

        public ManagerController(IManagerService managerService,
            IVacationService vacationService)
        {
            _managerService = managerService;
            _vacationService = vacationService;
        }

        // вывод всех активных запросов на имя менеджера
        // todo - routing
        public IActionResult Index()
        {
            var requestsByCurrentManager =
                _managerService.GetAllCurrentManagerResponses();

            var resultViewModel = _managerService
                .GetRequestsToConsiderByCurrentManager(requestsByCurrentManager);

            return View(resultViewModel);
        }

        public IActionResult Processed()
        {
            var resultViewModel = new List<RequestsToManagerViewModel>();
            var processedRequestsByCurrentManager =
                _managerService.GetProcessedRequestsByCurrentManager();

            foreach (var rq in processedRequestsByCurrentManager)
            {
                resultViewModel.Add(new RequestsToManagerViewModel()
                {
                    VacationRequestId = rq.VacationRequestId,
                    Employee = rq.User.UserInformation.FullName,
                    VacationType = rq.VacationType,
                    StartDate = rq.StartDate,
                    EndDate = rq.EndDate
                });
            }

            return View(resultViewModel);
        }

        // вывод страницы для Approve/Reject выбранного запроса
        public IActionResult Approval(int id)
        {
            var vacationRequest = _vacationService.GetVacationById(id);

            var managerResponse = _managerService
                .GetResponseByVacationId(vacationRequest.VacationRequestId);
            var resultViewModel = _managerService.VacationApproval(managerResponse);

            return View(resultViewModel);
        }

        [HttpPost]
        public IActionResult Approval(string submit,
            VacationRequestApprovalDto response)
        {
            if (submit == "Approve") {
                response.isApproval = true;
            }
            else if (submit == "Reject") {
                response.isApproval = false;
            }
            else {
                return NotFound();
            }

            if (response.ManagerResponseId != 0)
            {
                _managerService.ApproveUserRequest(response.ManagerResponseId,
                    response.ManagerNotes, response.isApproval);
            }

            return RedirectToAction("Index");
        }
    }
}