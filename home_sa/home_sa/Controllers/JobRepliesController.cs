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
using System.Security.Cryptography;
using System.Security.Claims;
using home_sa.Helpers;
using Microsoft.AspNetCore.Identity;

namespace home_sa.Controllers
{
    [Authorize]
    public class JobRepliesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JobRepliesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        
        // GET: JobReplies
        public async Task<IActionResult> Index2()
        {
            var applicationDbContext = _context.JobReply.Include(j => j.jobId);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Index()
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var replies = await _context.JobReply
                .Where(r => r.userId == new Guid(userIdString))
                .Include(r => r.JobOpportunity) // Include related JobOpportunity
                .ToListAsync();

            return View(replies);
        }

        [HttpGet]
        public async Task<IActionResult> Download(Guid replyId)
        {
            //var jobReply = await _context.JobReply.FindAsync(JobReply);
            var jobReply = await _context.JobReply.Include(r => r.JobOpportunity).FirstOrDefaultAsync(r => r.replyId == replyId);
            if (jobReply == null)
            {
                return NotFound();
            }
            var filePath = jobReply.FilePath;
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var encryptedFileData = await System.IO.File.ReadAllBytesAsync(filePath);
            var user = await _userManager.FindByIdAsync(jobReply.userId.ToString());

            // Decrypt the file
            byte[] decryptedFileData;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {

                //byte[] publicKeyBytes = Convert.FromBase64String(user.PublicKey);
                rsa.ImportRSAPublicKey(Convert.FromBase64String(user.PublicKey), out _);
                var encryptSymKey = Convert.FromBase64String(jobReply.EncryptedSymmetricKey);

                byte[] symmetricKey = rsa.Decrypt(encryptSymKey, RSAEncryptionPadding.Pkcs1);

                decryptedFileData = EncryptionHelper.DecryptFile(encryptedFileData, symmetricKey, Convert.FromBase64String(jobReply.IV));

            }

            if (!DigitalSignatureHelper.VerifyData(decryptedFileData, Convert.FromBase64String(jobReply.signature), user.PublicKey))
            {
                return BadRequest("File signature verification failed.");
            }

            return File(decryptedFileData, "application/octet-stream", Path.GetFileName(filePath));
        }

        /*
        // GET: JobReplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobReply == null)
            {
                return NotFound();
            }

            var jobReply = await _context.JobReply
                .Include(j => j.jobId)
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
    */
    }
}
