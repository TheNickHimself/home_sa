using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using home_sa.Data;
using Microsoft.AspNetCore.Authorization;
using home_sa.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace home_sa.Controllers
{
    [Authorize]
    public class JobOportunetiesController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JobOportunetiesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            //_userManager = userManager;
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
        public async Task<IActionResult> Details(int id)
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("jobTitle,jobDescription")] JobOpportunity jobOpportunity)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                // Handle the case where the userId is not a valid Guid
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return View(jobOpportunity);
            }
            jobOpportunity.employerId = userId;

            

            if (ModelState.IsValid)
            {
                _context.Add(jobOpportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var state in ModelState)
            {
                string error = ($"{state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
            }


            return View(jobOpportunity);
        }

        public IActionResult Reply(int jobId)
        {
            var model = new JobReply { jobId = jobId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply([Bind("jobId,UploadedFile")] JobReply jobReply)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return View(jobReply);
            }

            jobReply.userId = userId;

            if (ModelState.IsValid)
            {
                if (jobReply.UploadedFile != null)
                {
                    var filePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(jobReply.UploadedFile.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await jobReply.UploadedFile.CopyToAsync(stream);
                    }
                    jobReply.FilePath = filePath;
                }

                _context.Add(jobReply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = jobReply.jobId });
            }

            return View(jobReply);
        }

        // GET: JobOportuneties/Edit/5
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, [Bind("jobId,employerId,jobTitle,jobDescription")] JobOpportunity jobOpportunity)
        {
            if (id != jobOpportunity.jobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOpportunity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOportunetyExists(jobOpportunity.jobId))
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
            return View(jobOpportunity);
        }

        // GET: JobOportuneties/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.JobOportuneties == null)
            {
                return NotFound();
            }

            var jobOpportunity = await _context.JobOportuneties
                .FirstOrDefaultAsync(m => m.jobId == id);
            if (jobOpportunity == null)
            {
                return NotFound();
            }

            return View(jobOpportunity);
        }

        // POST: JobOportuneties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobOportuneties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobOportuneties'  is null.");
            }
            var jobOpportunity = await _context.JobOportuneties.FindAsync(id);
            if (jobOpportunity != null)
            {
                _context.JobOportuneties.Remove(jobOpportunity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOportunetyExists(int id)
        {
          return (_context.JobOportuneties?.Any(e => e.jobId == id)).GetValueOrDefault();
        }
    }
}
