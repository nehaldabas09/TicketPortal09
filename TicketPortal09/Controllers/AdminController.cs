using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;
using TicketPortal09.Services; // Add this namespace

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(TicketDbContext context, UserManager<IdentityUser> userManager, IAdminService adminService, ILogger<AdminController> logger)
        {
            _context = context;
            _userManager = userManager;
            _adminService = adminService;
            _logger = logger;
        }

        // GET: Admin/Tickets
        public async Task<IActionResult> Tickets()
        {
            try
            {

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tickets = await _context.Tickets
                    .Where(t => t.UserId == currentUserId)
                    .ToListAsync();

                return View(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Admin/EditTicket/5
        public async Task<IActionResult> EditTicket(int id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket == null)
                {
                    return NotFound();
                }

                var catorgies = await _context.Categories.ToListAsync();
                ViewBag.Categories = catorgies;

                var subCategories = await _context.Subcategories.ToListAsync();
                ViewBag.SubCategories = subCategories;
                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Admin/EditTicket/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(int id, Ticket ticket)
        {
          
         
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTicket = await _context.Tickets.FindAsync(id);

                    if (existingTicket == null)
                    {
                        return NotFound();
                    }

                    var subcategory = await _context.Subcategories.FindAsync(ticket.SubCategoryId);
                    var category = await _context.Categories.FindAsync(ticket.CategoryId);

                    existingTicket.Title = ticket.Title;
                    existingTicket.Description = ticket.Description;
                    existingTicket.CategoryId = ticket.CategoryId;
                    existingTicket.SubCategoryId = ticket.SubCategoryId;
                    existingTicket.Category = category;
                    existingTicket.SubCategory = subcategory;

                    _context.Update(existingTicket);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    throw;
                }
                return RedirectToAction(nameof(Tickets));
            }

            return View(ticket);
        }

        // GET: Admin/CreateTicket
        public IActionResult CreateTicket()
        {
            try
            {

            var categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            ViewData["CategoryId"] = new SelectList(categories, "Value", "Text");

            var subcategories = _context.Subcategories.Select(s => new SelectListItem
            {
                Value = s.SubcategoryId.ToString(),
                Text = s.Name
            }).ToList();

            ViewData["SubcategoryId"] = new SelectList(subcategories, "Value", "Text");

            return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Admin/CreateTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket([Bind("Title,Description,CategoryId,SubCategoryId")] Ticket ticket)
        {
            try
            {

            var agentId = await _adminService.GetAgentIdAsync();

            if (ModelState.IsValid)
            {
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ticket.UserId = currentUserId;
                ticket.AgentId = agentId;

                _context.Add(ticket);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Tickets));
            }

            return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        private bool TicketExists(int id)
        {
            try
            {

            return _context.Tickets.Any(e => e.TicketId == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public IActionResult CreateUser()
        {
            try
            {

            return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegisterUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkEmailExsist = await _userManager.FindByEmailAsync(user.Email);
                    if (checkEmailExsist != null)
                    {
                        ModelState.AddModelError("Email", "Email Already Exists. Choose Another one");
                        return View(user);
                    }

                    IdentityUser agentUser = new() { Email = user.Email, UserName = user.Email };
                    var result = await _userManager.CreateAsync(agentUser, user.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(agentUser, user.Role);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("Agent");
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while processing your request.";
                return RedirectToAction("Agent");
            }
        }
    }
}
