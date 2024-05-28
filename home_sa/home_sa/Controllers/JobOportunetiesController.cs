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
using Microsoft.Extensions.Logging;
using home_sa.Helpers;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace home_sa.Controllers
{
    [Authorize] 
    public class JobOportunetiesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<JobOportunetiesController> _logger;
        private readonly ApplicationDbContext _context;

        public JobOportunetiesController(ILogger<JobOportunetiesController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
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

        // GET: JobOportuneties/Reply/1
        public async Task<IActionResult> Reply(int id)
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

            var jobReply = new JobReply
            {
                jobId = jobOportunety.jobId
            };

            return View(jobReply);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply([Bind("jobId,UploadedFile")] JobReply jobReply)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdString);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return View(jobReply);
            }

            if (jobReply.UploadedFile != null)
            {
                using (var stream = jobReply.UploadedFile.OpenReadStream())
                {
                    if (!FileValidationHelper.IsValidDocx(stream) && !FileValidationHelper.IsValidPdf(stream))
                    {
                        ModelState.AddModelError(string.Empty, "Invalid file type.");
                        return View(jobReply);
                    }
                }

                var filePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(jobReply.UploadedFile.FileName));
                try
                {
                    if (!Directory.Exists("uploads"))
                    {
                        Directory.CreateDirectory("uploads");
                    }

                    using (var ms = new MemoryStream())
                    {
                        await jobReply.UploadedFile.CopyToAsync(ms);
                        var fileData = ms.ToArray();

                        // Encrypt the file
                        using (var rsa = new RSACryptoServiceProvider(2048))
                        {
                            rsa.ImportRSAPublicKey(Convert.FromBase64String(user.PublicKey), out _);
                            var encryptedFileData = EncryptionHelper.EncryptFile(fileData, rsa, out byte[] encryptedSymmetricKey, out byte[] iv);

                            System.IO.File.WriteAllBytes(filePath, encryptedFileData);
                            jobReply.EncryptedSymmetricKey = Convert.ToBase64String(encryptedSymmetricKey);
                            jobReply.IV = Convert.ToBase64String(iv);
                        }
                    }
                    jobReply.FilePath = filePath;

                    // Sign the file
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var signature = DigitalSignatureHelper.SignData(fileBytes, user.PrivateKey);
                    jobReply.signature = Convert.ToBase64String(signature);
                    var fileBytes2 = System.IO.File.ReadAllBytes(filePath);

                    var didItWork = DigitalSignatureHelper.VerifyData(fileBytes, Convert.FromBase64String(jobReply.signature), user.PublicKey);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "File upload failed: " + ex.Message);
                    return View(jobReply);
                }
            }

            jobReply.userId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(jobReply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = jobReply.jobId });
            }
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    string errorWord = ($"Property: {state.Key}, Error: {error.ErrorMessage}");
                    Console.WriteLine(errorWord);
                }
            }

            return View(jobReply);
        }

        [HttpGet]
        public async Task<IActionResult> Download(int jobId, int replyId)
        {
            var jobReply = await _context.JobReply.FindAsync(replyId);
            if (jobReply == null || jobReply.jobId != jobId)
            {
                return NotFound();
            }

            var filePath = jobReply.FilePath;
            var encryptedFileData = await System.IO.File.ReadAllBytesAsync(filePath);
            var user = await _userManager.FindByIdAsync(jobReply.userId.ToString());

            // Decrypt the file
            byte[] decryptedFileData;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(user.PublicKey), out _);
                decryptedFileData = EncryptionHelper.DecryptFile(encryptedFileData, Convert.FromBase64String(jobReply.EncryptedSymmetricKey), Convert.FromBase64String(jobReply.IV));
            }

            if (!DigitalSignatureHelper.VerifyData(decryptedFileData, Convert.FromBase64String(jobReply.signature), user.PublicKey))
            {
                return BadRequest("File signature verification failed.");
            }

            return File(decryptedFileData, "application/octet-stream", Path.GetFileName(filePath));
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
