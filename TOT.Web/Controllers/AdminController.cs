using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TOT.Interfaces.Services;
using TOT.Entities;
using TOT.Web.Models;
using TOT.Dto;

namespace TOT.Web.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole<int>> roleManager;
        private IUserInfoService userInfoService;

        public AdminController(RoleManager<IdentityRole<int>> roleMgr,
            UserManager<ApplicationUser> userMgr, IUserInfoService service)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            userInfoService = service;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.Name,
                    Email = model.Email,
                    UserInformationId = model.UserProfile
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        public IActionResult CreateInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateInfo(AddUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserInformationDTO userInfoDTO = new UserInformationDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName                    
                };

                if (userInfoDTO != null)
                {
                    userInfoService.SaveUserInfo(userInfoDTO);
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new NullReferenceException("userInfoDTO = null");
                }
            }
            return View(model);
        }
    }
}