using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TopJobs.Models;

namespace TopJobs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ApplicationUser usr = await GetCurrentUserAsync();

            if (usr == null)
            {
                return View();
            }
            else if (_userManager.IsInRoleAsync(usr, "Admin").Result)
            {
                return RedirectToRoute(new { action = "AdminMain", controller = "Home" });
            }
            else if (_userManager.IsInRoleAsync(usr, "Applicant").Result)
            {
                return RedirectToRoute(new { action = "ApplicantMain", controller = "Home" });
            }
            else if (_userManager.IsInRoleAsync(usr, "Employer").Result)
            {
                return RedirectToRoute(new { action = "EmployerMain", controller = "Home" });
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminMain()
        {
            return View("AdminMain");
        }
        [Authorize(Roles = "Employer")]
        public IActionResult EmployerMain()
        {
            return View("EmployerMain");
        }
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> ApplicantMainAsync()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            ViewBag.Name = "A";
            return View("ApplicantMain", usr);
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

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
