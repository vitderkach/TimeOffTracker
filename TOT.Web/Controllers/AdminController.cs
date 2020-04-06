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
using System.Security.Claims;
using TOT.Interfaces;
using System.Collections.Generic;

namespace TOT.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IVacationService _vacationService;
        private readonly IMapper _mapper;
        public AdminController(IAdminService adminService, IMapper mapper, IVacationService vacationService)
        {
            _adminService = adminService;
            _mapper = mapper;
            _vacationService = vacationService;
        }

        public async Task<IActionResult> VacationList(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            if (searchString != null)
            {
                ViewData["NameSortParm"] = searchString;
            }
            else
            {
                if (currentFilter == null)
                {
                    pageNumber = 1;
                }
            }

            if (currentFilter != null)
                ViewData["CurrentFilter"] = currentFilter;

            int userId = int.Parse((User.FindFirstValue(ClaimTypes.NameIdentifier)));

            ICollection<VacationRequestsListForAdminsDTO> responses;
            switch (currentFilter)
            {
                case "Just registered":
                    {
                        responses = _adminService.GetDefinedVacationRequestsForAcceptance(userId, null);
                        break;
                    }
                case "Need a final approval":
                    {
                        responses = _adminService.GetDefinedVacationRequestsForApproving(userId, null);
                        break;
                    }
                case "Accepted":
                    {
                        responses = _adminService.GetDefinedVacationRequestsForAcceptance(userId, true);
                        var k = _adminService.GetDefinedVacationRequestsForAcceptance(userId, false);
                        responses = responses.Concat(k).ToList();
                        break;
                    }
                case "Approved":
                    {
                        responses = _adminService.GetApprovedVacationRequests(userId, true);
                        responses = responses.Concat(_adminService.GetApprovedVacationRequests(userId, false)).ToList();

                        break;
                    }
                case "Self-cancelled":
                    {
                        responses = _adminService.GetSelfCancelledVacationRequests(userId);
                        break;
                    }
                default:
                    {
                        responses = _adminService.GetAllVacationRequests(userId);
                        var name = ViewData["NameSortParm"] as string;
                        if (name != null)
                            responses = responses.Where(vrl => vrl.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        break;
                    }
            }
            int pageSize = 3;
            return View(await PaginatedList<VacationRequestsListForAdminsDTO>.CreateAsync(responses, pageNumber ?? 1, pageSize));
        }



        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> UserList(string searchString,
            int? pageNumber)
        {
            if (searchString != null)
            {
                ViewData["NameSortParm"] = searchString;
            }

            var userList = _adminService.GetApplicationUserList();
            if (searchString != null)
                userList = userList.Where(m => m.FullName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            int pageSize = 3;
            return View(await PaginatedList<UserInformationListDto>.CreateAsync(userList, pageNumber ?? 1, pageSize));
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
        public IActionResult Fire(int id)
        {
            bool isSuccessful = _adminService.FireEmployee(id);

            if (!isSuccessful)
            {
                ModelState.AddModelError("", "Oops, the user hasn't been found.");
            }

            return RedirectToAction("List");
        }

        [ImportModelState]
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
        [ExportModelState]
        public async Task<IActionResult> Edit(int id, int currentRoleId)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminService.UserDataManipulation(id, currentRoleId);

                if (!result.Succeeded)
                {
                    AddErrorsFromResult(result);
                    return RedirectToAction($"Edit/{id}");
                }
            }

            return RedirectToAction("UserList");
        }

        public IActionResult EditVacation(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var vacationDto = _vacationService.GetVacationById(id);
            if (vacationDto.Stage == 1 || vacationDto.Stage == 3)
            {
                ViewBag.TimeOffTypeList = _vacationService.GetTimeOffTypeList();
                ViewBag.AvailableManagers = _vacationService.GetManagersForVacationApply(vacationDto.User.Id);
                return View(vacationDto);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EditVacation(ApplicationDto applicationDto)
        {
            if (ModelState.IsValid)
            {
                _adminService.EditVacationRequest(applicationDto);
                return RedirectToAction("Details", new { id = applicationDto.Id });
            }
            else
            {
                return RedirectToAction("EditVacation", new { id = applicationDto.Id });
            }
        }

        // вывод страницы для Approve/Reject выбранного запроса
        public IActionResult Details(int id)
        {

            var vacationRequestDTO = _vacationService.GetVacationById(id);
            if (vacationRequestDTO.SelfCancelled == true && vacationRequestDTO.Stage == 1)
            {
                return BadRequest();
            }
            VacationDetailsDTO vacationDetailsDTO = null;
            if (vacationRequestDTO.Stage == 1)
            {
                var managerResponse = new ManagerResponseDto();
                vacationDetailsDTO = _mapper.MergeInto<VacationDetailsDTO>(vacationRequestDTO, managerResponse);
            }
            if (vacationRequestDTO.Stage == 2 || vacationRequestDTO.Stage == 4)
            {
                vacationDetailsDTO = _mapper.Map<VacationRequestDto, VacationDetailsDTO>(vacationRequestDTO);
            }
            else if (vacationRequestDTO.Stage == 3)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var managerResponse = _adminService.GetAdminResponse(id, userId);
                vacationDetailsDTO = _mapper.MergeInto<VacationDetailsDTO>(vacationRequestDTO, managerResponse);
            }
            ViewBag.Controller = "Admin";
            return View("_VacationApprovingDetails", vacationDetailsDTO);
        }

        [HttpPost]
        public IActionResult ApproveVacationRequest(int responseId, int vacationRequestId, string notes, int? takenDays)
        {
            var vacationRequest = _vacationService.GetVacationById(vacationRequestId);
            if (vacationRequest.Stage == 1)
            {
                _adminService.AcceptVacationRequest(vacationRequestId, notes, true);
            }
            if (vacationRequest.Stage == 3)
            {
                _adminService.ApproveVacationRequest(responseId, notes, true);
                if (takenDays != null)
                {
                    _adminService.CalculateVacationDays(vacationRequestId, takenDays.Value);
                }
                else
                {
                    return BadRequest();
                }
                
            }

            return RedirectToAction("VacationList");
        }

        [HttpPost]
        public IActionResult DeclineVacationRequest(int responseId, int vacationRequestId, string notes)
        {
            var vacationRequest = _vacationService.GetVacationById(vacationRequestId);
            if (vacationRequest.Stage == 1)
            {
                _adminService.AcceptVacationRequest(vacationRequestId, notes, false);
            }
            if (vacationRequest.Stage == 3)
            {
                _adminService.ApproveVacationRequest(responseId, notes, false);
            }

            return RedirectToAction("VacationList");
        }
    }
}