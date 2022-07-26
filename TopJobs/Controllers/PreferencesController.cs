using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TopJobs.Data;
using TopJobs.Models;

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
        public async Task<IActionResult> Create([Bind("Id,PositionTypeId,WorkingHours,FlexibleSchedule,WorkFromHome")] Preference preference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(preference);
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

            var preference = await _context.Preferences.FindAsync(id);
            if (preference == null)
            {
                return NotFound();
            }
            ViewData["PositionTypeId"] = new SelectList(_context.PositionTypes, "Id", "Name", preference.PositionTypeId);
            ViewData["PositionTypeLevels"] = _context.PositionTypes.GroupBy(p => p.Level).Select(group => group.Key);
            ViewData["PositionTypeNames"] = _context.PositionTypes.GroupBy(p => p.Name).Select(group => group.Key);
            return View(preference);
        }

        // POST: Preferences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PositionTypeId,WorkingHours,FlexibleSchedule,WorkFromHome")] Preference preference, string positionTypeName, string positionTypeLevel)
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
                        var newPositionType =_context.Add(new PositionType { Level = positionTypeLevel, Name = positionTypeName });
                        await _context.SaveChangesAsync();
                    }
                    preference.PositionTypeId = _context.PositionTypes
                                                    .Where(p => p.Level == positionTypeLevel && p.Name == positionTypeName)
                                                    .FirstOrDefault().Id;

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
                return RedirectToAction(nameof(Index));
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
    }
}
