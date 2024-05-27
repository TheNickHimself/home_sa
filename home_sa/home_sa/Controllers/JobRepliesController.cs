using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using home_sa.Data;
using home_sa.Models;
using Microsoft.AspNetCore.Authorization;
/*
namespace home_sa.Controllers
{
    [Authorize]
    public class JobRepliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobRepliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobReplies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobReply.Include(j => j.JobOpportunity);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobReplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobReply == null)
            {
                return NotFound();
            }

            var jobReply = await _context.JobReply
                .Include(j => j.JobOpportunity)
                .FirstOrDefaultAsync(m => m.replyId == id);
            if (jobReply == null)
            {
                return NotFound();
            }

            return View(jobReply);
        }

        // GET: JobReplies/Create
        public IActionResult Create()
        {
            ViewData["jobId"] = new SelectList(_context.JobOportuneties, "jobId", "jobDescription");
            return View();
        }

        // POST: JobReplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("replyId,jobId,userId,FilePath")] JobReply jobReply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobReply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["jobId"] = new SelectList(_context.JobOportuneties, "jobId", "jobDescription", jobReply.jobId);
            return View(jobReply);
        }

        // GET: JobReplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobReply == null)
            {
                return NotFound();
            }

            var jobReply = await _context.JobReply.FindAsync(id);
            if (jobReply == null)
            {
                return NotFound();
            }
            ViewData["jobId"] = new SelectList(_context.JobOportuneties, "jobId", "jobDescription", jobReply.jobId);
            return View(jobReply);
        }

        // POST: JobReplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("replyId,jobId,userId,FilePath")] JobReply jobReply)
        {
            if (id != jobReply.replyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobReply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobReplyExists(jobReply.replyId))
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
            ViewData["jobId"] = new SelectList(_context.JobOportuneties, "jobId", "jobDescription", jobReply.jobId);
            return View(jobReply);
        }

        // GET: JobReplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobReply == null)
            {
                return NotFound();
            }

            var jobReply = await _context.JobReply
                .Include(j => j.JobOpportunity)
                .FirstOrDefaultAsync(m => m.replyId == id);
            if (jobReply == null)
            {
                return NotFound();
            }

            return View(jobReply);
        }

        // POST: JobReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobReply == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobReply'  is null.");
            }
            var jobReply = await _context.JobReply.FindAsync(id);
            if (jobReply != null)
            {
                _context.JobReply.Remove(jobReply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobReplyExists(int id)
        {
          return (_context.JobReply?.Any(e => e.replyId == id)).GetValueOrDefault();
        }
    }
}
*/