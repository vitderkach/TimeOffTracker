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
using System.Security.Claims;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
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
        public async Task<IActionResult> VacationList(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (searchString != null)
            {
                ViewData["NameSortParm"] = searchString;
            }

            if (currentFilter != null)
                ViewData["CurrentFilter"] = currentFilter;

            int userId = int.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));

            IEnumerable<VacationRequestListForManagersDTO> responses;
            switch (currentFilter)
            {
                case "Not handled yet":
                    {
                        responses = _managerService.GetDefinedManagerVacationRequests(userId, null).Where(vr => vr.Approval == null);
                        break;
                    }
                case "Accepted":
                    {
                        responses = _managerService.GetDefinedManagerVacationRequests(userId, true);
                        break;
                    }
                case "Declined":
                    {
                        responses = _managerService.GetDefinedManagerVacationRequests(userId, false);
                        break;
                    }
                case "Declined by other manager":
                    {
                        responses = _managerService.GetDefinedManagerVacationRequests(userId, null).Where(vr => vr.Approval == false);
                        break;
                    }
                default:
                    {
                        responses = _managerService.GetAllManagerVacationRequests(userId);
                        break;
                    }
            }
            var name = ViewData["NameSortParm"] as string;
            if (name != null)
                responses = responses.Where(vrl => vrl.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            int pageSize = 3;
            return View(await PaginatedList<VacationRequestListForManagersDTO>.CreateAsync(responses, pageNumber ?? 1, pageSize));
        }

        // вывод страницы для Approve/Reject выбранного запроса
        public IActionResult Details(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var vacationRequestDTO = _vacationService.GetVacationById(id);
            VacationDetailsDTO vacationDetailsDTO = null;
            if (vacationRequestDTO.Stage == 2)
            {
                var managerResponse = _managerService.GetManagerResponse(id, userId);
                vacationDetailsDTO = _mapper.MergeInto<VacationDetailsDTO>(vacationRequestDTO, managerResponse);
            }
            else
            {
                vacationDetailsDTO = _mapper.Map<VacationRequestDto, VacationDetailsDTO>(vacationRequestDTO);
            }
            ViewBag.Controller = "Manager";
            return View("_VacationApprovingDetails", vacationDetailsDTO);
        }

        [HttpPost]
        public IActionResult ApproveVacationRequest(int responseId, int vacationRequestId, string notes)
        {
            var vacationRequest = _vacationService.GetVacationById(vacationRequestId);
            if (vacationRequest.Stage == 2)
            {
                _managerService.GiveManagerResponse(responseId, notes, true);
            }

            return RedirectToAction("VacationList");
        }

        [HttpPost]
        public IActionResult DeclineVacationRequest(int responseId, int vacationRequestId, string notes)
        {
            var vacationRequest = _vacationService.GetVacationById(vacationRequestId);
            if (vacationRequest.Stage == 2)
            {
                _managerService.GiveManagerResponse(responseId, notes, false);
            }

            return RedirectToAction("VacationList");
        }
    }
}