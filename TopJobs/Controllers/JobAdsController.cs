using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TopJobs.Data;
using TopJobs.Methods;
using TopJobs.Models;
using TopJobs.ViewModels;
using X.PagedList;

namespace TopJobs.Controllers
{
    public class JobAdsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobAdsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobAds
        public async Task<IActionResult> Index(int? page = 1)
        {
            if (page != null && page < 1)
            {
                page = 1;
            }

            var userId = (await GetCurrentUserAsync()).Id;
            var user = _context.Users
                                    .Include(u => u.JobExperienceEntries)
                                    .Include(u => u.EducationEntries)
                                        .ThenInclude(e => e.EducationType)
                                    .Include(u => u.Preference)
                                        .ThenInclude(p => p.TechnologyPreferences)
                                    .Include(u => u.Preference)
                                        .ThenInclude(p => p.PositionType)
                                    .FirstOrDefault(u => u.Id == userId);
            var jobAds = _context.JobAds
                                        .Where(j => !j.Archived)
                                        .Include(j => j.Company)
                                        .Include(j => j.Preference)
                                            .ThenInclude(p => p.PositionType)
                                        .Include(j => j.Preference)
                                            .ThenInclude(p => p.TechnologyPreferences)
                                        .ToList();
            
            //var userPreference = _context.Preferences
            //                                .Include(p => p.PositionType)
            //                                .Include(p => p.TechnologyPreferences)
            //                                .ThenInclude(tp => tp.Technology)
            //                                .FirstOrDefault(x => x.Id == usr.PreferenceId);
            foreach (var jobAd in jobAds)
            {
                //jobAd.MatchingPercentage = MatchPercentage.CalculateMatchPercentage(userPreference, jobAd.Preference);
                jobAd.MatchingPercentage = MatchPercentage.Calculate(user, jobAd, user.Preference.PositionType.Level);
            }
            var pageSize = 10;
            return View(await jobAds.OrderByDescending(j => j.MatchingPercentage).ToPagedListAsync(page ?? 1, pageSize));
        }
        // GET: JobAds/MyJobAds
        public async Task<IActionResult> MyJobAds(string currentFilter, string searchString, bool includeArchived, int? page = 1)
        {
            if (page != null && page < 1)
            {
                page = 1;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.IncludeArchived = includeArchived;
            var usr = await GetCurrentUserAsync();
            var employerCompanies = GetCompaniesByEmloyer(usr);
            var jobAds = _context.JobAds
                                    .Include(j => j.Company)
                                    .Where(j => (!j.Archived | includeArchived) && employerCompanies.Contains(j.Company)) // only employer's job ads
                                    .Include(j => j.JobApplications)
                                    .ThenInclude(a => a.User)
                                    .Include(j => j.Preference)
                                    .Include(j => j.Preference.PositionType)
                                    .Include(j => j.Preference.TechnologyPreferences)
                                    .ThenInclude(tp => tp.Technology)
                                    .OrderByDescending(j => j.Archived)
                                    .ThenBy(j => j.DateSubmitted)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                jobAds = jobAds.Where(j => j.Name.ToUpper().Contains(searchString) || j.Preference.PositionType.Name.ToUpper().Contains(searchString));
            }

            var userPreference = _context.Preferences
                                            .Include(p => p.PositionType)
                                            .Include(p => p.TechnologyPreferences)
                                            .ThenInclude(tp => tp.Technology)
                                            .FirstOrDefault(x => x.Id == usr.PreferenceId);
            foreach (var jobAd in jobAds)
            {
                jobAd.MatchingPercentage = MatchPercentage.CalculateMatchPercentage(userPreference, jobAd.Preference);
            }
            var pageSize = 5;
            return View(await jobAds.ToPagedListAsync(page ?? 1, pageSize));
        }



        public async Task<IActionResult> Charts()
        {
            var jobAds = _context.JobAds
                                    .Include(j => j.Company)
                                    .Include(j => j.Preference)
                                    .Include(j => j.Preference.PositionType)
                                    .Include(j => j.Preference.TechnologyPreferences)
                                    .ThenInclude(tp => tp.Technology);

            var technologyNumbers = new Dictionary<string, int>();

            foreach (var technology in _context.Technologies)
            {
                technologyNumbers.Add(technology.Name, 0);
            }

            foreach (var jobAd in jobAds)
            {
                foreach (var technologyPreference in jobAd.Preference.TechnologyPreferences)
                {
                    technologyNumbers[technologyPreference.Technology.Name] += 1;
                }
            }

            technologyNumbers = technologyNumbers.OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);

            string dataString = "&chd=t:";
            string labelsString = "&chl=";

            foreach (var item in technologyNumbers)
            {
                dataString += item.Value.ToString() + ",";
                labelsString += item.Key + ": " + item.Value.ToString() + "|";
            }

            ViewBag.DataString = dataString;
            ViewBag.LabelsString = labelsString;
            ViewBag.ChartString = "https://image-charts.com/chart?cht=p3&chs=600x600&chf=ps0-0,lg,45,1E91D6,0.2,B1D2E7,1|ps0-1,lg,45,0072BB,0.2,009688,1|ps0-4,lg,45,E18335,0.2,ff9688,1|ps0-3,lg,45,E4CC37,0.2,ff9688,1|ps0-2,lg,45,8FC93A,0.2,BCDC8D,1&chan" + dataString + labelsString;

            return View(await jobAds.ToListAsync());
        }

        // GET: JobAds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.IsEmployer = await _userManager.IsInRoleAsync(await GetCurrentUserAsync(), "Employer");

            var jobAd = await _context.JobAds
                .Include(j => j.Company)
                .Include(j => j.Preference)
                .Include(j => j.Preference.PositionType)
                .Include(j => j.Preference.TechnologyPreferences)
                .ThenInclude(tp => tp.Technology)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobAd == null)
            {
                return NotFound();
            }

            return View(jobAd);
        }

        // POST: JobAds/Accept/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(int jobAdId)
        {
            var jobAd = await _context.JobAds.FindAsync(jobAdId);

            jobAd.Archived = true;
            await _context.SaveChangesAsync();

            return RedirectToRoute(new { action = "MyJobAds", controller = "JobAds", jobAdId = jobAdId });
        }

        // GET: JobAds/Create
        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {
            var currentUser = GetCurrentUserAsync().Result;
            var availableCompanies = GetCompaniesByEmloyer(currentUser);

            ViewData["CompanyId"] = new SelectList(availableCompanies, "Id", "Name");
            ViewData["PreferenceId"] = new SelectList(_context.Preferences, "Id", "Id");
            return View();
        }

        // POST: JobAds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CompanyId,DateSubmitted,PreferenceId,RequiredExperience")] JobAd jobAd)
        {
            jobAd.DateSubmitted = DateTime.Now;

            if (ModelState.IsValid)
            {
                var newPreference = new Preference { Id = 0, PositionTypeId = 1 };
                _context.Add(newPreference);
                _context.SaveChanges();

                jobAd.PreferenceId = newPreference.Id;
                _context.Add(jobAd);
                await _context.SaveChangesAsync();

                return RedirectToRoute(new { action = "Edit", controller = "Preferences", id = newPreference.Id }); ;
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobAd.CompanyId);
            ViewData["PreferenceId"] = new SelectList(_context.Preferences, "Id", "Id", jobAd.PreferenceId);
            return View(jobAd);
        }

        // GET: JobAds/Edit/5
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobAd = await _context.JobAds.FindAsync(id);
            if (jobAd == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobAd.CompanyId);
            ViewData["PreferenceId"] = new SelectList(_context.Preferences, "Id", "Id", jobAd.PreferenceId);
            return View(jobAd);
        }

        // POST: JobAds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CompanyId,DateSubmitted,PreferenceId,RequiredExperience")] JobAd jobAd)
        {
            if (id != jobAd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobAd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobAdExists(jobAd.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobAd.CompanyId);
            ViewData["PreferenceId"] = new SelectList(_context.Preferences, "Id", "Id", jobAd.PreferenceId);
            return View(jobAd);
        }

        // GET: JobAds/Delete/5
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobAd = await _context.JobAds
                .Include(j => j.Company)
                .Include(j => j.Preference)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobAd == null)
            {
                return NotFound();
            }

            return View(jobAd);
        }

        // POST: JobAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobAd = await _context.JobAds.FindAsync(id);
            _context.JobAds.Remove(jobAd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobAds/Candidates/5
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> Candidates(int? jobAdId)
        {
            if (jobAdId == null)
            {
                return NotFound();
            }

            var candidates = await _context.JobApplications
                .Where(j => j.JobAdId == jobAdId)
                .Include(j => j.JobAd)
                    .ThenInclude(a => a.Preference)
                        .ThenInclude(p => p.TechnologyPreferences)
                            .ThenInclude(tp => tp.Technology)
                .Include(j => j.JobAd)
                    .ThenInclude(a => a.Preference)
                        .ThenInclude(p => p.PositionType)
                .Include(j => j.User)
                    .ThenInclude(u => u.Preference)
                        .ThenInclude(p => p.TechnologyPreferences)
                            .ThenInclude(tp => tp.Technology)
                .Include(j => j.User)
                    .ThenInclude(u => u.Preference)
                        .ThenInclude(p => p.PositionType)
                .Select(j => new CandidateViewModel(j))
                .ToListAsync();


            if (candidates == null)
            {
                return NotFound();
            }

            return View(candidates);
        }

        private bool JobAdExists(int id)
        {
            return _context.JobAds.Any(e => e.Id == id);
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
