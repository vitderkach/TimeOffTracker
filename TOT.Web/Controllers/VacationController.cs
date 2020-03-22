using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TOT.Business.Services;
using TOT.Dto;
using TOT.Entities;
using TOT.Interfaces;
using TOT.Interfaces.Services;
using TOT.Web.Models;
using PagedList;
using System.Security.Claims;
using Microsoft.Extensions.Localization;

namespace TOT.Web.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<Resources.Resources> _localizer;

        public VacationController(IVacationService vacationService, IMapper mapper,
            IUserService userService, IStringLocalizer<Resources.Resources> localizer)
        {
            _vacationService = vacationService;
            _mapper = mapper;
            _userService = userService;
            _localizer = localizer;
        }
        public ActionResult Index()
        {
            return View();
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
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == null);
                        break;
                    }
                case "Approved":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == true);
                        break;
                    }
                case "Declined":
                    {
                        vacations = _vacationService.GetAllByCurrentUser().Where(v => v.Approval == false);
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
            var vacation = _vacationService.GetVacationById(id);
            if (vacation != null)
            {
                return View(vacation);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(int Id, string Notes)
        {
            _vacationService.UpdateVacation(Id, Notes);

            return RedirectToAction("List");
        }
        public IActionResult Deactivate(int id)
        {
            bool isRemoved = _vacationService.DeactivateVacation(id);

            return RedirectToAction("List");
        }
    }
}