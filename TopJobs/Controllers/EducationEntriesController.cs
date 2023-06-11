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
    public class EducationEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EducationEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EducationEntries
        public async Task<IActionResult> Index(string userId)
        {
            var applicationDbContext = _context.EducationEntries
                                                    .Include(e => e.EducationType)
                                                    .Include(e => e.User);
            ViewBag.User = _userManager.FindByIdAsync(userId).Result;
            return View(await applicationDbContext.Where(x => x.UserId == userId).OrderBy(x => x.DateStarted).ToListAsync());
        }

        // GET: EducationEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educationEntry = await _context.EducationEntries
                .Include(e => e.EducationType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educationEntry == null)
            {
                return NotFound();
            }

            return View(educationEntry);
        }

        // GET: EducationEntries/Create
        public IActionResult Create(string userId)
        {
            ViewData["EducationTypeId"] = new SelectList(_context.EducationTypes, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.User = _userManager.FindByIdAsync(userId).Result;
            return View();
        }

        // POST: EducationEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,School,Description,UserId,EducationTypeId,DateStarted,DateFinished")] EducationEntry educationEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(educationEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { userId = educationEntry.UserId});
            }
            ViewData["EducationTypeId"] = new SelectList(_context.EducationTypes, "Id", "Name", educationEntry.EducationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", educationEntry.UserId);
            return View(educationEntry);
        }

        // GET: EducationEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educationEntry = await _context.EducationEntries.FindAsync(id);
            if (educationEntry == null)
            {
                return NotFound();
            }
            ViewData["EducationTypeId"] = new SelectList(_context.EducationTypes, "Id", "Name", educationEntry.EducationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", educationEntry.UserId);
            return View(educationEntry);
        }

        // POST: EducationEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,School,Description,UserId,EducationTypeId,DateStarted,DateFinished")] EducationEntry educationEntry)
        {
            if (id != educationEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(educationEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationEntryExists(educationEntry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { userId = educationEntry.UserId });
            }
            ViewData["EducationTypeId"] = new SelectList(_context.EducationTypes, "Id", "Name", educationEntry.EducationTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", educationEntry.UserId);
            return View(educationEntry);
        }

        // GET: EducationEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educationEntry = await _context.EducationEntries
                .Include(e => e.EducationType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educationEntry == null)
            {
                return NotFound();
            }

            return View(educationEntry);
        }

        // POST: EducationEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var educationEntry = await _context.EducationEntries.FindAsync(id);
            _context.EducationEntries.Remove(educationEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { userId = educationEntry.UserId });
        }

        private bool EducationEntryExists(int id)
        {
            return _context.EducationEntries.Any(e => e.Id == id);
        }
    }
}
