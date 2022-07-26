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
    public class JobAdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobAds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobAds.Include(j => j.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobAds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobAd = await _context.JobAds
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobAd == null)
            {
                return NotFound();
            }

            return View(jobAd);
        }

        // GET: JobAds/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: JobAds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CompanyId,DateSubmitted,PreferenceId,RequiredExperience")] JobAd jobAd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobAd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobAd.CompanyId);
            return View(jobAd);
        }

        // GET: JobAds/Edit/5
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
            return View(jobAd);
        }

        // GET: JobAds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobAd = await _context.JobAds
                .Include(j => j.Company)
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

        private bool JobAdExists(int id)
        {
            return _context.JobAds.Any(e => e.Id == id);
        }
    }
}
