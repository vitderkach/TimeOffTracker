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

namespace TOT.Web.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;

        public VacationController(IVacationService vacationService, IMapper mapper,
            IUserService userService)
        {
            _vacationService = vacationService;
            _mapper = mapper;
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Apply()
        {
            /*var managers = _userService.GetAllByRole("Manager");
            var selectListManagers = new SelectList(managers, "Id", "UserInformation.FullName");
            var userId = _userService.GetCurrentUser().Result.Id; */

            ApplyForRequestGetDto apply = new ApplyForRequestGetDto();
            ViewBag.TimeOffTypeList = GetTimeOffTypeList();
            ViewBag.Excitingmanagers = GetExcitingManagersList(0, new List<UserInformation>());
            return View();
        }
        [HttpPost]
        public ActionResult Apply([FromBody]ApplyForRequestGetDto applyForRequestGetDto)
        {
            if (applyForRequestGetDto != null)
            {
                int left = (int)(applyForRequestGetDto.EndDate - applyForRequestGetDto.StartDate).TotalDays;
                if (applyForRequestGetDto.RequiredManagers.Count() == 0)
                {
                    ModelState.AddModelError("RequiredManagers", "At least 1 manager is required");
                }
                if (left > 30)
                {
                    ModelState.AddModelError("StartDate", "Maximum interval between dates is 30 days");
                }
                if (left < 0)
                {
                    ModelState.AddModelError("StartDate", "End date must be later than start date");
                }
            }
            if (ModelState.IsValid)
            {

                var user = _userService.GetCurrentUser().Result;
                applyForRequestGetDto.UserId = user.Id;

                var vacationRequest = _mapper.Map<ApplyForRequestGetDto, VacationRequestDto>(applyForRequestGetDto);

                _vacationService.ApplyForVacation(vacationRequest);
                return RedirectToAction("List");
            }
            else
            {
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



        public List<SelectListItem> GetTimeOffTypeList()
        {
            var list = new List<SelectListItem>();
            foreach (TimeOffType item in Enum.GetValues(typeof(TimeOffType)))
            {
                list.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(TimeOffType), item),
                    Value = item.ToString()
                });
            }
            return list.OrderBy(x => x.Text).ToList();
        }

        public List<SelectListItem> GetExcitingManagersList(int userId, List<UserInformation> excitingManagers)
        {
            var list = new List<SelectListItem>();
            foreach (var manager in excitingManagers)
            {
                if (manager.ApplicationUserId == userId)
                {
                    continue;
                }
                list.Add(new SelectListItem
                {
                    Text = manager.FirstName + " " + manager.LastName,
                    Value = manager.ApplicationUserId.ToString()
                });
            }
            return list.OrderBy(x => x.Text).ToList();
        }
    }
}