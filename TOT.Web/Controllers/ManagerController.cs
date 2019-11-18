using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Web.Models;
using PagedList;

namespace TOT.Web.Controllers {
    [Authorize(Roles = "Manager, Administrator")]
    public class ManagerController : Controller {
        private readonly IManagerService _managerService;
        private readonly IVacationService _vacationService;

        public ManagerController(IManagerService managerService,
            IVacationService vacationService,
            IMapper mapper)
        {
            _managerService = managerService;
            _vacationService = vacationService;
        }

        // вывод всех активных запросов на имя менеджера
        // todo - routing
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (searchString != null)
            {
                ViewData["NameSortParm"] = searchString;
            }
            else
            {
                if (currentFilter == null)
                {
                    pageNumber = 1;
                }
            }

            if (currentFilter != null)
                ViewData["CurrentFilter"] = currentFilter;

            IEnumerable<ManagerResponseListDto> responses = new List<ManagerResponseListDto>();
            switch (currentFilter)
            {
                case "In Proccess":
                {
                    var requestsByCurrentManager = _managerService.GetAllCurrentManagerResponses();
                    responses = _managerService
                        .GetCurrentManagerRequests(requestsByCurrentManager).Where(m => m.Approval == null);
                    break;
                }
                case "Approved":
                {
                    responses = _managerService.GetAllMyManagerResponses().Where(m => m.Approval == true);
                    break;
                }
                case "Declined":
                {
                    responses = _managerService.GetAllMyManagerResponses().Where(m => m.Approval == false);
                    break;
                }
                default:
                {
                    var requestsByCurrentManager = _managerService.GetAllCurrentManagerResponses();
                    responses = _managerService
                        .GetCurrentManagerRequests(requestsByCurrentManager).Where(m => m.Approval == null);
                    var name = ViewData["NameSortParm"] as string;
                    if (name != null)
                        responses = responses.Where(m => m.VacationRequest.User.UserInformation.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    break;
                }
            }
            int pageSize = 3;
            return View(await PaginatedList<ManagerResponseListDto>.CreateAsync(responses, pageNumber ?? 1, pageSize));
        }

        // вывод обработанных запросов менеджера
        public IActionResult Processed()
        {
            var processedRequestsByCurrentManager =
                _managerService.GetProcessedRequestsByCurrentManager();

            var resultViewModel = _managerService
                .GetCurrentManagerRequests(processedRequestsByCurrentManager);

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
            if (submit == "Approve")
            {
                response.isApproval = true;
            }
            else if (submit == "Decline")
            {
                response.isApproval = false;
            }
            else
            {
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