using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TopJobs.Data;
using TopJobs.Methods;
using TopJobs.Models;

namespace TopJobs.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobApplications.Include(j => j.JobAd).Include(j => j.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.JobAd)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.JobAdId == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Create
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Create(int jobAdId)
        {
            ViewData["JobAdId"] = jobAdId;
            var jobAd = _context.JobAds.Find(jobAdId);
            var user = await GetCurrentUserAsync();
            bool alreadyApplied = (await _context.JobApplications.FirstOrDefaultAsync(j => j.UserId == user.Id && j.JobAdId == jobAdId)) != null;
            ViewBag.AlreadyApplied = alreadyApplied;
            ViewData["JobAdName"] = jobAd.Name;
            ViewData["CompanyName"] = _context.Companies.Find(jobAd.CompanyId).Name;
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobAdId,UserId,DateApplied")] JobApplication jobApplication, int jobAdId, string userId)
        {
            jobApplication.DateApplied = DateTime.Now;
            jobApplication.JobAdId = jobAdId;
            var currentUser = GetCurrentUserAsync().Result;
            jobApplication.UserId = currentUser.Id;
            jobApplication.MatchingPercentage = 0;

            if (ModelState.IsValid)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction("ApplicationSuccessful");
            }
            ViewData["JobAdId"] = new SelectList(_context.JobAds, "Id", "Description", jobApplication.JobAdId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobApplication.UserId);
            return View(jobApplication);
        }

        public IActionResult ApplicationSuccessful()
        {
            return View();
        }
        // GET: JobApplications/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            ViewData["JobAdId"] = new SelectList(_context.JobAds, "Id", "Description", jobApplication.JobAdId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobApplication.UserId);
            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobAdId,UserId,DateApplied")] JobApplication jobApplication)
        {
            if (id != jobApplication.JobAdId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.JobAdId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobAdId"] = new SelectList(_context.JobAds, "Id", "Description", jobApplication.JobAdId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobApplication.UserId);
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> Delete(int jobAdId, string userId)
        {
            if (jobAdId == null || userId == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.JobAd)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.JobAdId == jobAdId && m.UserId == userId);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int jobAdId, string userId)
        {
            
            var jobApplication = await _context.JobApplications
                .Include(j => j.JobAd)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.JobAdId == jobAdId && m.UserId == userId);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToRoute(new { action = "Candidates", controller = "JobAds", jobAdId = jobAdId });
        }

        // POST: JobAds/Accept/5
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int jobAdId, string userId)
        {
            var jobApplication = await _context.JobApplications
                                                        .Include(j => j.User)
                                                        .Include(j => j.JobAd)
                                                            .ThenInclude(a => a.Company)
                                                        .FirstOrDefaultAsync(j => j.JobAdId == jobAdId && j.UserId == userId);
            jobApplication.Accepted = true;
            await _context.SaveChangesAsync();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TopJobs", "test_app_23@outlook.com"));
            message.To.Add(new MailboxAddress(jobApplication.User.FullName, jobApplication.User.Email));
            message.Subject = "Job application accepted";
            message.Body = new TextPart("plain")
            {
                Text = $"Hello, {jobApplication.User.FullName},\nYour application for the \"{jobApplication.JobAd.Name}\" position at {jobApplication.JobAd.Company.Name} has been reviewed and accepted.\nExpect a call from us for the upcomming interview.\n\nBest regards,\nTopJobs"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("test_app_23@outlook.com", "@pPT35t;3202");
                client.Send(message);
                client.Disconnect(true);
            }

            return RedirectToRoute(new { action = "Candidates", controller = "JobAds", jobAdId = jobAdId });
        }
        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.JobAdId == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
