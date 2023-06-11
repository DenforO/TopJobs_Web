using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TopJobs.Data;
using TopJobs.Methods;
using TopJobs.Models;
using TopJobs.ViewModels;

namespace TopJobs.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PreferencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Preferences
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Preferences.Include(p => p.PositionType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Preferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preference = await _context.Preferences
                .Include(p => p.PositionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preference == null)
            {
                return NotFound();
            }

            return View(preference);
        }

        // GET: Preferences/Create
        public IActionResult Create()
        {
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name");
            return View();
        }

        // POST: Preferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PositionTypeId,WorkingHours,FlexibleSchedule,WorkFromHome")] Preference preference, string positionTypeName, string positionTypeLevel)
        {
            if (ModelState.IsValid)
            {
                preference.PositionType = FindOrCreatePositionType(positionTypeName, positionTypeLevel);

                _context.Add(preference);
                var jobApplication = _context.JobApplications
                                                        .Include(x => x.User)
                                                        .Include(x => x.JobAd)
                                                        .SingleOrDefault(x => x.JobAd.PreferenceId == preference.Id);
                jobApplication.MatchingPercentage = MatchPercentage.CalculateMatchPercentage(jobApplication.User.Preference, jobApplication.JobAd.Preference);
                _context.Update(jobApplication);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", preference.PositionTypeId);
            return View(preference);
        }

        // GET: Preferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preference = _context.Preferences.Include(p => p.PositionType).FirstOrDefault(p => p.Id == id);

            if (preference == null)
            {
                return NotFound();
            }

            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", preference.PositionTypeId);

            ViewData["PositionTypeLevels"] = _context.PositionTypes
                                                                .GroupBy(p => p.Level)
                                                                .Select(group => group.Key);

            ViewData["PositionTypeNames"] = _context.PositionTypes
                                                            .GroupBy(p => p.Name)
                                                            .Select(group => group.Key);

            ViewData["Technologies"] = _context.Technologies
                                                        .Include(t => t.TechnologyPreferences)
                                                            .ThenInclude(tp => tp.Preference)
                                                        .AsEnumerable()
                                                        .Select(t => new PreferredTechnologiesViewModel { Technology = t, Selected = t.TechnologyPreferences.Select(tp => tp.PreferenceId).ToList().Contains(preference.Id) });

            ViewData["TechnologyPreferences"] = _context.TechnologyPreferences
                                                                            .Where(x => x.PreferenceId == id)
                                                                            .Join(_context.Technologies,
                                                                                  tp => tp.TechnologyId,
                                                                                  t => t.Id,
                                                                                  (tp, t) => t.Name)
                                                                            .ToListAsync().Result;
            return View(preference);
        }

        // POST: Preferences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositionTypeId,WorkingHours,FlexibleSchedule,WorkFromHome")] Preference preference, string positionTypeName, string positionTypeLevel, string TechnologiesSelected)
        {
            if (id != preference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var positionType = _context.PositionTypes
                                                        .Where(p => p.Level == positionTypeLevel && p.Name == positionTypeName)
                                                        .FirstOrDefault();
                    if (positionType == null)
                    {
                        positionType =_context.Add(new PositionType { Level = positionTypeLevel, Name = positionTypeName }).Entity;
                    }
                    preference.PositionType = positionType;

                    List<string> technologyNames = TechnologiesSelected.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
                    
                    var oldTechnologyPreferences = _context.TechnologyPreferences.Where(x => x.PreferenceId == id);
                    _context.TechnologyPreferences.RemoveRange(oldTechnologyPreferences);
                    await _context.SaveChangesAsync();

                    foreach (var technology in technologyNames)
                    {
                        int technologyId = _context.Technologies.Where(x => x.Name == technology).First().Id;
                        _context.TechnologyPreferences.Add(new TechnologyPreference { PreferenceId = id, TechnologyId = technologyId });
                        await _context.SaveChangesAsync();
                    }

                    _context.Update(preference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreferenceExists(preference.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", preference.PositionTypeId);
            return View(preference);
        }

        // GET: Preferences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preference = await _context.Preferences
                .Include(p => p.PositionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (preference == null)
            {
                return NotFound();
            }

            return View(preference);
        }

        // POST: Preferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preference = await _context.Preferences.FindAsync(id);
            _context.Preferences.Remove(preference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreferenceExists(int id)
        {
            return _context.Preferences.Any(e => e.Id == id);
        }

        [Produces("application/json")]
        [HttpGet("search")]
        [Route("SearchPositions")]
        public async Task<IActionResult> SearchPositions() // for autocomplete
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var names = _context.PositionTypes.Where(p => p.Name.Contains(term))
                        .GroupBy(p => p.Name)
                        .Select(p => p.First())
                        .Select(p => new { p.Id, p.Name }).ToListAsync();
                return Ok(await names);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
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
    }
}
