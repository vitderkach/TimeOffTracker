using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOT.Interfaces.Services;
using TOT.Web.Models;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Manager, Administrator")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        private readonly IVacationService _vacationService;
        private readonly IUserInfoService _userInfoService;

        public ManagerController(IManagerService managerService,
            IVacationService vacationService, IUserInfoService userInfoService)
        {
            _managerService = managerService;
            _vacationService = vacationService;
            _userInfoService = userInfoService;
        }

        public IActionResult Index()
        {
            var resultViewModel = new List<RequestsToManagerViewModel>();
            var requestsByCurrentManager =
                _managerService.GetAllNeedToConsiderByCurrentManager();

            foreach (var rq in requestsByCurrentManager)
            {
                resultViewModel.Add(new RequestsToManagerViewModel()
                {
                    VacationRequestId = rq.VacationRequestId,
                    Employee = rq.User.UserInformation.FullName,
                    VacationType = rq.VacationType,
                    StartDate = rq.StartDate,
                    EndDate = rq.EndDate,
                });
            }
            return View(resultViewModel);
        }

        // requestId = null 
        public IActionResult Approval(int requestId)
        {
            var vacationRequest = _vacationService.GetVacationById(requestId);
            var managerResponse = _managerService
                .GetResponseByVacationId(vacationRequest.VacationRequestId);
            var userInfo = _userInfoService.GetUserInfo(vacationRequest.UserId);

            var resultViewModel = new RequestApprovalViewModel()
            {
                VacationRequestId = vacationRequest.VacationRequestId,
                Employee = userInfo.FullName,
                VacationType = vacationRequest.VacationType,
                StartDate = vacationRequest.StartDate,
                EndDate = vacationRequest.EndDate,
                EmployeeNotes = vacationRequest.Notes,
                ManagerResponseId = managerResponse.Id
            };

            return View(resultViewModel);
        }

        [HttpPost]
        public IActionResult Approval(string submit,
            RequestApprovalViewModel response)
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