using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOT.Business.Services;
using TOT.Dto;
using TOT.Interfaces;
using TOT.Interfaces.Services;

namespace TOT.Web.Controllers
{
    public class VacationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVacationService _vacationService;
        public VacationController(IVacationService vacationService, IMapper mapper)
        {
            _vacationService = vacationService;
            _mapper = mapper;
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
            return View();
        }
        [HttpPost]
        public ActionResult Apply(VacationRequestDto vacationRequestDto)
        {
            if(ModelState.IsValid)
            {
                _vacationService.ApplyForVacation(vacationRequestDto);
            }
            return View();
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