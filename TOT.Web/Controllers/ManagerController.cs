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
        //private UserManager<ApplicationUser> _userManager;
        private readonly IManagerService _managerService;
        private readonly IVacationService _vacationService;
        private readonly IUserInfoService _userInfoService;

        //UserManager<ApplicationUser> userManager,
        public ManagerController(IManagerService managerService,
            IVacationService vacationService, IUserInfoService userInfoService)
        {
            //_userManager = userManager;
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
        public IActionResult Approval(RequestApprovalViewModel vm,
             string notes, bool approval)
        {
            if (vm.ManagerResponseId != 0)
            {
                _managerService.ApproveUserRequest(vm.ManagerResponseId,
                    notes, approval);
            }
            return RedirectToAction("Index");
        }
    }
}