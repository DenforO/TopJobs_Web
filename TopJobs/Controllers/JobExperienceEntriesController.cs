using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopJobs.Data;
using TopJobs.Models;

namespace TopJobs.Controllers
{
    public class JobExperienceEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobExperienceEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobExperienceEntries
        public async Task<IActionResult> Index(string userId)
        {
            ViewBag.User = _userManager.FindByIdAsync(userId).Result;
            var applicationDbContext = _context.JobExperienceEntries.Include(j => j.Company).Include(j => j.PositionType).Include(j => j.User);
            return View(await applicationDbContext.Where(x => x.UserId == userId).OrderByDescending(x => x.DateStarted).ToListAsync());
        }

        // GET: Unapproved
        public async Task<IActionResult> Unapproved()
        {
            var user = GetCurrentUserAsync();
            var userCompanies = GetCompaniesByEmloyer(await user);
            var unapprovedJobEntries = _context.JobExperienceEntries
                                                        .Include(j => j.Company)
                                                        .Include(j => j.PositionType)
                                                        .Include(j => j.User)
                                                        .Where(j => userCompanies.Contains(j.Company) && !j.Verified)
                                                        .ToListAsync();
            return View(await unapprovedJobEntries);
        }
        // POST: JobAds/Accept/5
        [HttpPost, ActionName("Verify")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(int jobExperienceId)
        {
            var jobExperienceEntry = await _context.JobExperienceEntries.FindAsync(jobExperienceId);
            jobExperienceEntry.Verified = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Unapproved));
        }

        [HttpPost]
        public JsonResult CompaniesPrefix(string prefix)
        {
            //Note : you can bind same list from database  
            List<Company> allCompanies = _context.Companies.ToList();

            var results = allCompanies.Where(x => x.Name.StartsWith(prefix)).Select(x => new { Name = x.Name, Id = x.Id }).ToList();

            return Json(results, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        // GET: JobExperienceEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobExperienceEntry = await _context.JobExperienceEntries
                .Include(j => j.Company)
                .Include(j => j.PositionType)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobExperienceEntry == null)
            {
                return NotFound();
            }

            return View(jobExperienceEntry);
        }

        // GET: JobExperienceEntries/Create
        public IActionResult Create()
        {
            string userId = GetCurrentUserAsync().Result.Id;
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name");
            ViewBag.User = _userManager.FindByIdAsync(userId).Result;
            return View();
        }

        // POST: JobExperienceEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CompanyId,DateStarted,DateFinished,PositionTypeId,Verified")] JobExperienceEntry jobExperienceEntry, string positionTypeName, string positionTypeLevel)
        {
            if (ModelState.IsValid)
            {
                jobExperienceEntry.PositionType = FindOrCreatePositionType(positionTypeName, positionTypeLevel);

                _context.Add(jobExperienceEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = jobExperienceEntry.UserId });
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobExperienceEntry.CompanyId);
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", jobExperienceEntry.PositionTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobExperienceEntry.UserId);
            return View(jobExperienceEntry);
        }

        private PositionType FindOrCreatePositionType(string positionTypeName, string positionTypeLevel)
        {
            foreach (var positionType in _context.PositionTypes)
            {
                if (positionType.Name == positionTypeName && positionType.Level == positionTypeLevel)
                {
                    return positionType;
                }
            }
            return _context.PositionTypes.Add(new PositionType { Level = positionTypeLevel, Name = positionTypeName }).Entity;

        }

        // GET: JobExperienceEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobExperienceEntry = await _context.JobExperienceEntries.FindAsync(id);
            if (jobExperienceEntry == null)
            {
                return NotFound();
            }
            ViewBag.PositionTypeName = _context.PositionTypes.Find(jobExperienceEntry.PositionTypeId).Name;
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobExperienceEntry.CompanyId);
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", jobExperienceEntry.PositionTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobExperienceEntry.UserId);
            return View(jobExperienceEntry);
        }

        // POST: JobExperienceEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CompanyId,DateStarted,DateFinished,PositionTypeId,Verified")] JobExperienceEntry jobExperienceEntry, string positionTypeName, string positionTypeLevel)
        {
            if (id != jobExperienceEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    jobExperienceEntry.PositionType = FindOrCreatePositionType(positionTypeName, positionTypeLevel);
                    jobExperienceEntry.Verified = false;
                    _context.Update(jobExperienceEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExperienceEntryExists(jobExperienceEntry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { userId = jobExperienceEntry.UserId });
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobExperienceEntry.CompanyId);
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", jobExperienceEntry.PositionTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobExperienceEntry.UserId);
            return View(jobExperienceEntry);
        }

        // GET: JobExperienceEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobExperienceEntry = await _context.JobExperienceEntries
                .Include(j => j.Company)
                .Include(j => j.PositionType)
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobExperienceEntry == null)
            {
                return NotFound();
            }

            return View(jobExperienceEntry);
        }

        // POST: JobExperienceEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobExperienceEntry = await _context.JobExperienceEntries.FindAsync(id);
            _context.JobExperienceEntries.Remove(jobExperienceEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { userId = jobExperienceEntry.UserId });
        }

        private bool JobExperienceEntryExists(int id)
        {
            return _context.JobExperienceEntries.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private IQueryable<Company> GetCompaniesByEmloyer(ApplicationUser employer)
        {
            // if user works for more than one company, all will be listed
            return _context.JobExperienceEntries
                                        .Include(j => j.Company)
                                        .Where(j => (j.UserId == employer.Id) && j.DateFinished == null && j.Verified)
                                        .Select(j => j.Company);
        }
    }
}
