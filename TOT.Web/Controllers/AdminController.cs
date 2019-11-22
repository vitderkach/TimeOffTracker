using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TOT.Interfaces.Services;
using TOT.Dto;
using Microsoft.AspNetCore.Authorization;
using TOT.Web.Models;
using System.Linq;
using System;
using TOT.Utility.Validation;
using TOT.Utility.Validation.Alerts;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private IAdminService _adminService;

        public AdminController( IAdminService adminService)
        {
            _adminService = adminService;
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> List(string searchString,
            int? pageNumber)
        {
            if (searchString != null)
            {
                ViewData["NameSortParm"] = searchString;
            }

            var userList = _adminService.GetApplicationUserList();
            if (searchString != null)
                userList = userList.Where(m => m.FullName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);

            int pageSize = 3;
            return View(await PaginatedList<ApplicationUserListDto>.CreateAsync(userList, pageNumber ?? 1, pageSize));
        }

        [ImportModelState]
        public IActionResult Create()
        {
            ViewData["roles"] = _adminService.GetApplicationRoles();
            return View();
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> Create(RegistrationUserDto registrationForm)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = await _adminService.RegistrationNewUser(registrationForm);

                if (!registrationResult.Succeeded)
                {
                    AddErrorsFromResult(registrationResult);
                    return RedirectToAction("Create");
                }
            }
            else
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("Create").WithSuccess(
                $"User {registrationForm.Name} {registrationForm.Surname} successfully created");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminService.DeleteUser(id);

            if (!result.Succeeded)
            {
                AddErrorsFromResult(result);
            }

            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
            var editUser = _adminService.EditUserData(id);

            if (editUser == null)
            {
                return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
                    // return RedirectToAction($"Edit/{id}");
                }
            }

            return RedirectToAction("List");
        }
    }
}