using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TopJobs.Data;
using TopJobs.Models;
using TopJobs.ViewModels;

namespace TopJobs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
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

        public IActionResult Profile(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            var userEducation = _context.EducationEntries
                                                    .Where(e => e.UserId == userId)
                                                    .Include(e => e.EducationType)
                                                    .OrderByDescending(e => e.DateStarted)
                                                    .Select(e => new EducationViewModel(e))
                                                    .ToList();
            var userExperience = _context.JobExperienceEntries
                                                    .Include(j => j.PositionType)
                                                    .Include(j => j.Company)
                                                    .OrderByDescending(j => j.DateStarted)
                                                    .Where(j => j.UserId == userId)
                                                    .Select(j => new JobExperienceViewModel(j))
                                                    .ToList();

            var technologies = _context.TechnologyPreferences
                                                    .Where(tp => tp.PreferenceId == user.PreferenceId)
                                                    .Include(tp => tp.Technology)
                                                    .Select(tp => tp.Technology.Name)
                                                    .Distinct()
                                                    .ToList();
            ViewBag.User = user;
            ViewBag.Education = userEducation;
            ViewBag.Experience = userExperience;
            ViewBag.CurrentPosition = userExperience[0].Timeframe.Contains("Present") ? userExperience[0].Position : string.Empty;
            ViewBag.Technologies = technologies;
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
