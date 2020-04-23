﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Web.Models;
using System.Security.Claims;
using Microsoft.Extensions.Localization;
using TOT.Entities;
using System.Text.Json;
using System;

namespace TOT.Web.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;

        public VacationController(IVacationService vacationService, IMapper mapper, IUserService userService)
        {
            _vacationService = vacationService;
            _mapper = mapper;
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int vacationRequestId)
        {
            if (_vacationService.CancelVacation(vacationRequestId))
            {
                return RedirectToAction("List");
            }
            return BadRequest();
        }

        [HttpGet]
        public ActionResult Apply()
        {
            int userId = int.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));
            ApplicationDto apply = new ApplicationDto();
            apply.UserId = userId;
            ViewBag.TimeOffTypeList = _vacationService.GetTimeOffTypeList();
            ViewBag.AvailableManagers = _vacationService.GetManagersForVacationApply(userId);
            return View(apply);
        }
        [HttpPost]
        public ActionResult Apply(ApplicationDto applyForRequestGetDto)
        {
            if (applyForRequestGetDto != null)
            {
                if (applyForRequestGetDto.RequiredManagersEmails.Count() == 0)
                {
                    ModelState.AddModelError("RequiredManagers", "At least 1 manager is required");
                }
            }
            if (ModelState.IsValid)
            {
                _vacationService.ApplyForVacation(applyForRequestGetDto);
                return RedirectToAction("List");
            }
            else
            {
                int userId = int.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));
                ViewBag.TimeOffTypeList = _vacationService.GetTimeOffTypeList();
                ViewBag.AvailableManagers = _vacationService.GetManagersForVacationApply(userId);
                return View(applyForRequestGetDto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> List(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["NameSortParm"] = sortOrder;
            ViewData["CurrentFilter"] = currentFilter;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = ViewData["CurrentFilter"] as string;
            }

            IEnumerable<VacationRequestListDto> vacations = new List<VacationRequestListDto>();
            switch (currentFilter)
            {
                case "All":
                    {
                        vacations = _vacationService.GetAllByCurrentUser();
                        break;
                    }
                case "In Proccess":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == null && v.SelfCancelled != true);
                        break;
                    }
                case "Approved":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == true && v.SelfCancelled != true);
                        break;
                    }
                case "Declined":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == false && v.SelfCancelled != true);
                        break;
                    }
                case "Self-cancelled":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.SelfCancelled == true);
                        break;
                    }
                default:
                    {
                        vacations = _vacationService.GetAllByCurrentUser();
                        break;
                    }
            }
            int pageSize = 3;
            return View(await PaginatedList<VacationRequestListDto>.CreateAsync(vacations, pageNumber ?? 1, pageSize));
        }
        public IActionResult GetVacationDays(int id)
        {
            var vacationDays = _vacationService.GetVacationDays(id);
            return PartialView("_VacationDaysPartial", vacationDays);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vacationDto = _vacationService.GetVacationById(id);
            ViewBag.TimeOffTypeList = _vacationService.GetTimeOffTypeList();
            ViewBag.AvailableManagers = _vacationService.GetManagersForVacationApply(vacationDto.User.Id);
            return View(vacationDto);
        }
        [HttpPost]
        public IActionResult Edit(ApplicationDto applicationDto)
        {
            if (ModelState.IsValid)
            {
                _vacationService.EditVacationRequest(applicationDto);
                return RedirectToAction("List", new { id = applicationDto.Id });
            }
            else
            {
                return RedirectToAction("Edit", new { id = applicationDto.Id });
            }
        }
            public IActionResult Deactivate(int id)
        {
            bool isRemoved = _vacationService.CancelVacation(id);
            if (isRemoved)
            {
                return RedirectToAction("List");
            }
            else
            {
                return BadRequest();
            }

        }

        public IActionResult Report()
        {
            var endDate = DateTime.Today;
            var startDate = endDate.AddMonths(-1);
            var reportInfo = _vacationService.GetReportInfo(startDate, endDate, 0);
            return View(reportInfo);
        }

        [HttpPost]
        public IActionResult VacationReport(ReportDto report)
        {
            var reportInfo = _vacationService.GetReportInfo(report.StartDate, report.EndDate, report.TeamId);
            return View("Report", reportInfo);
        }

        [HttpGet]
        public IActionResult VacationTimeline(int id)
        {
            var vacationTimelineDto = _vacationService.GetVacationTimeline(id);
            return View("VacationTimeline", vacationTimelineDto);
        }

        [HttpPost]
        [Route("[controller]/VacationTimeline/VacationRequestTable")]
        public IActionResult VacationRequestTable([FromBody] TemporalVacationRequestDto temporalVacationRequest) {
            return PartialView("_VacationRequestTablePartial", temporalVacationRequest);
        }

        [HttpPost]
        [Route("[controller]/VacationTimeline/ComparisonTable")]
        public IActionResult ComparisonTable([FromBody] TemporalVacationRequestDto[] temporalVacationRequests)
        {
            return PartialView("_ComparisonTablePartial", new ComparisonTableDto { OldVacationRequest = temporalVacationRequests[0], NewVacationRequest = temporalVacationRequests[1] });
        }

        [HttpPost]
        [Route("[controller]/VacationTimeline/ManagerResponseTable")]
        public IActionResult ManagerResponseTable([FromBody] ManagerResponseForTimelineDto managerResponse)
        {
            return PartialView("_ManagerResponseTablePartial", managerResponse);
        }
    }
}