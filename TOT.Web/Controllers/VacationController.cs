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

namespace TOT.Web.Controllers
{
    [Authorize]
    public class VacationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;

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
        public IActionResult List()
        {
            var vacations = _vacationService.GetAllByCurrentUser();

            return View(vacations);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vacation = _vacationService.GetVacationById(id);
            if (vacation != null)
            {
                return View(vacation);
            }
            return View();
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