﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using home_sa.Data;
using Microsoft.AspNetCore.Authorization;

namespace home_sa.Controllers
{
    [Authorize]
    public class JobOportunetiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobOportunetiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobOportuneties
        public async Task<IActionResult> Index()
        {
              return _context.JobOportuneties != null ? 
                          View(await _context.JobOportuneties.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.JobOportuneties'  is null.");
        }

        // GET: JobOportuneties/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.JobOportuneties == null)
            {
                return NotFound();
            }

            var jobOportunety = await _context.JobOportuneties
                .FirstOrDefaultAsync(m => m.jobId == id);
            if (jobOportunety == null)
            {
                return NotFound();
            }

            return View(jobOportunety);
        }

        // GET: JobOportuneties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobOportuneties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("jobId,employerId,jobTitle,jobDescription")] JobOportunety jobOportunety)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobOportunety);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobOportunety);
        }

        // GET: JobOportuneties/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.JobOportuneties == null)
            {
                return NotFound();
            }

            var jobOportunety = await _context.JobOportuneties.FindAsync(id);
            if (jobOportunety == null)
            {
                return NotFound();
            }
            return View(jobOportunety);
        }

        // POST: JobOportuneties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("jobId,employerId,jobTitle,jobDescription")] JobOportunety jobOportunety)
        {
            if (id != jobOportunety.jobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOportunety);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOportunetyExists(jobOportunety.jobId))
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
            return View(jobOportunety);
        }

        // GET: JobOportuneties/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.JobOportuneties == null)
            {
                return NotFound();
            }

            var jobOportunety = await _context.JobOportuneties
                .FirstOrDefaultAsync(m => m.jobId == id);
            if (jobOportunety == null)
            {
                return NotFound();
            }

            return View(jobOportunety);
        }

        // POST: JobOportuneties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.JobOportuneties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobOportuneties'  is null.");
            }
            var jobOportunety = await _context.JobOportuneties.FindAsync(id);
            if (jobOportunety != null)
            {
                _context.JobOportuneties.Remove(jobOportunety);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOportunetyExists(string id)
        {
          return (_context.JobOportuneties?.Any(e => e.jobId == id)).GetValueOrDefault();
        }
    }
}