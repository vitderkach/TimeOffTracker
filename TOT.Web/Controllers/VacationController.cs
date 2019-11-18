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
            var managers = _userService.GetAllByRole("Manager");

            var selectListManagers = new SelectList(managers, "Id", "UserInformation.FullName");

            ApplyForRequestGetDto apply = new ApplyForRequestGetDto();
            ViewBag.TimeOffTypes = apply.VacationTypes;

            ViewBag.Managers = selectListManagers;
            return View(); 
        }
        [HttpPost]
        public ActionResult Apply([FromBody]ApplyForRequestGetDto applyForRequestGetDto)
        {
            if(ModelState.IsValid)
            {
                var user = _userService.GetCurrentUser().Result;
                applyForRequestGetDto.UserId = user.Id;

                var vacationRequest = _mapper.Map<ApplyForRequestGetDto, VacationRequestDto>(applyForRequestGetDto);

                _vacationService.ApplyForVacation(vacationRequest);
                return RedirectToAction("List");
            }
            else
            {
                return new BadRequestResult();
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
        public IActionResult Delete(int id)
        {
            bool isRemoved = _vacationService.DeleteVacation(id);

            return RedirectToAction("List");
        }
    }
}