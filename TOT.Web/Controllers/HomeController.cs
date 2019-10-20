using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TOT.Interfaces.Services;
using TOT.Web.Models;

namespace TOT.Web.Controllers
{
    public class HomeController : Controller
    {
        IUserInformationService _userInfoService;
        public HomeController(IUserInformationService userInfoService)
        {
            _userInfoService = userInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Get()
        {
            var userInfo = _userInfoService.getUserInformation(1);
            return View(userInfo);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
