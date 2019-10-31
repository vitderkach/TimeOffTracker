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
using Microsoft.AspNetCore.Authorization;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole<int>> roleManager;
        private IUserInfoService userInfoService;

        private readonly string defaultPassword = "user";

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
            ViewData["roles"] = roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            var role = roleManager.FindByIdAsync(model.RoleId.ToString()).Result;

            if (ModelState.IsValid)
            {
                UserInformationDTO userInfoDTO = new UserInformationDTO()
                {
                    FirstName = model.Name,
                    LastName = model.Surname
                };

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.Login,
                    Email = model.Email                  
                };

                IdentityResult result = null;

                if (!string.IsNullOrEmpty(userInfoDTO.FirstName) && !string.IsNullOrEmpty(userInfoDTO.LastName))
                {
                    userInfoService.SaveUserInfo(userInfoDTO);

                    user.UserInformationId = userInfoService.GetUsersInfo().Where(fn =>
                        fn.FirstName == userInfoDTO.FirstName && fn.LastName == userInfoDTO.LastName)
                        .FirstOrDefault().UserInformationId;

                    result = await userManager.CreateAsync(user, defaultPassword);
                }

                if (result.Succeeded)
                {                   
                    await userManager.AddToRoleAsync(user, role.Name);
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
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

        public async Task<IActionResult> Edit(int id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();

                return View(new ChangeUserRoleViewModel
                {
                    UserId = user.Id,
                    Name = user.UserName,
                    RoleName = userRoles,
                    AllRoles = allRoles
                });
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, List<string> roles)
        {
            var user = userManager.FindByIdAsync(id.ToString()).Result;

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();

                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);
                await userManager.AddToRolesAsync(user, addedRoles);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return NotFound();
        }


        /*   public IActionResult CreateInfo()
        {
            return View();
        } */
    }
}