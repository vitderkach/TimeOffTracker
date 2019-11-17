﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TOT.Interfaces.Services;
using TOT.Entities;
using TOT.Web.Models;
using TOT.Dto;
using Microsoft.AspNetCore.Authorization;
using TOT.Interfaces;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private IAdminService _adminService;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole<int>> roleManager;
        private IUserInfoService userInfoService;
        private IVacationService _vacationService;
        private IMapper _mapper;

        private readonly string defaultPassword = "user";

        public AdminController(RoleManager<IdentityRole<int>> roleMgr,
            UserManager<ApplicationUser> userMgr, IUserInfoService service,
            IMapper mapper, IAdminService adminService,
            IVacationService vacationService)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            userInfoService = service;
            _mapper = mapper;
            _adminService = adminService;
            _vacationService = vacationService;
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public IActionResult List()
        {
            var userList = _adminService.GetApplicationUserList();

            return View(userList);
        }

        public IActionResult Create()
        {
            ViewData["roles"] = _adminService.GetApplicationRoles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegistrationUserDto registrationForm)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = await _adminService.RegistrationNewUser(registrationForm);

                if (!registrationResult.Succeeded)
                {
                    AddErrorsFromResult(registrationResult);
                    ViewData["roles"] = _adminService.GetApplicationRoles();
                    return View(registrationForm);
                }
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    userInfoService.DeleteUserInfo(user.UserInformationId);
                    _vacationService.DeleteVacationByUserId(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return View("Index", userManager.Users);
        }

        public IActionResult Edit(int id)
        {
            var editUser = _adminService.EditUserData(id);

            if (editUser == null)
            {
                return NotFound();
            }

            return View(editUser);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, int currentRoleId)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.UserDataManipulation(id, currentRoleId);

                if (!result.Succeeded)
                {
                    AddErrorsFromResult(result);
                    return View();
                   // return RedirectToAction($"Edit/{id}");
                }
            }

            return RedirectToAction("List");
        }
    }
}