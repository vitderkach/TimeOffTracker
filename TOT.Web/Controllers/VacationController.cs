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
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;

        public VacationController(IVacationService vacationService, IMapper mapper
            , UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _vacationService = vacationService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }
        // GET: Vacation
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Apply() 
        {
            var managers = _userService.GetAllByRole("Manager");

            var selectListManagers = new SelectList(managers, "Id", "UserInformation.LastName");

            ApplyForRequestGetDto apply = new ApplyForRequestGetDto();
            ViewBag.TimeOffTypes = apply.VacationTypes;

            ViewBag.Managers = selectListManagers;
            return View(); 
        }
        [HttpPost]
        public async Task<ActionResult> Apply([FromHeader] ApplyForRequestGetDto applyForRequestGetDto)
        {
            if(ModelState.IsValid)
            {   
                var user = await _userManager.GetUserAsync(HttpContext.User);
                applyForRequestGetDto.UserId = user.Id;

                var vacationRequest = _mapper.Map<ApplyForRequestGetDto, VacationRequestDto>(applyForRequestGetDto);

                _vacationService.ApplyForVacation(vacationRequest);
            }
            return RedirectToAction("List");
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

            return View(vacation);
        }
        [HttpPost]
        public IActionResult Edit(int Id, string Notes)
        {
            _vacationService.UpdateVacation(Id, Notes);

            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            _vacationService.DeleteVacation(id);

            return RedirectToAction("List");
        }
    }
}