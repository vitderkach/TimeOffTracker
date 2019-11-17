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

namespace TOT.Web.Controllers {
    [Authorize(Roles = "Manager, Administrator")]
    public class ManagerController : Controller {
        private readonly IManagerService _managerService;
        private readonly IVacationService _vacationService;
        private readonly IMapper _mapper;

        public ManagerController(IManagerService managerService,
            IVacationService vacationService,
            IMapper mapper)
        {
            _managerService = managerService;
            _vacationService = vacationService;
            _mapper = mapper;
        }

        // вывод всех активных запросов на имя менеджера
        // todo - routing
        public IActionResult Index(int page = 1)
        {
            var requestsByCurrentManager =
                _managerService.GetAllCurrentManagerResponses();

            var resultViewModel = _managerService
                .GetCurrentManagerRequests(requestsByCurrentManager);

            int pageSize = 3;   // количество элементов на странице

            var responses = resultViewModel;
            var count = responses.Count();
            var items = responses.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ManagerResponseListViewModel viewModel = new ManagerResponseListViewModel
            {
                PageViewModel = pageViewModel,
                Responses = items
            };

            return View(viewModel);
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