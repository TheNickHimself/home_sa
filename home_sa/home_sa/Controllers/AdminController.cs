using home_sa.Data;
using home_sa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace home_sa.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;

        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRolesViewModel = new List<RollManager>();

            foreach (var user in users)
            {
                var thisViewModel = new RollManager
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user),
                    AvailableRoles = roles
                };
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }

        // GET: Admin/EditRole
        public async Task<IActionResult> EditRole(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new RollManager
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles,
                AvailableRoles = roles,
                SelectedRole = userRoles.FirstOrDefault() // Assuming a user can have only one role
            };

            return View(model);
        }

        // POST: Admin/EditRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RollManager model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeRolesResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to remove user roles.");
                return View(model);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);

            if (!addRoleResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to add user role.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
