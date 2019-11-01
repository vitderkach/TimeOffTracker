using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class VacationController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        private readonly IUserService _userService;

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

        // GET: Vacation/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }
        [HttpGet]
        public ActionResult Apply() 
        {
            var managers = _userService.GetAllByRole("Employee");

            var selectListManagers = new SelectList(managers, "Id", "UserInformation.LastName");

            ApplyForRequestGetDto apply = new ApplyForRequestGetDto();
            ViewBag.TimeOffTypes = apply.VacationTypes;

            ViewBag.Managers = selectListManagers;
            return View(); 
        }
        [HttpPost]
        public async Task<ActionResult> Apply(ApplyForRequestGetDto applyForRequestGetDto)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                //vacationRequestDto.UserId = user.Id;

                applyForRequestGetDto.UserId = 1;
                var vacationRequest = _mapper.Map<ApplyForRequestGetDto, VacationRequestDto>(applyForRequestGetDto);

                _vacationService.ApplyForVacation(vacationRequest);
            }
            return Redirect("Apply");
        }

        // POST: Vacation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Vacation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Vacation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Vacation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vacation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}