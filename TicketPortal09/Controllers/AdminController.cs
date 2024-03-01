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

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(TicketDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Admin/Tickets
        public async Task<IActionResult> Tickets()
        {
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           
            var tickets = await _context.Tickets
                .Where(t => t.UserId == currentUserId)
                .ToListAsync();

            return View(tickets);
        }

        // GET: Admin/EditTicket/5
        public async Task<IActionResult> EditTicket(int id)
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

        // POST: Admin/EditTicket/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(int id,Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing ticket from the database
                    var existingTicket = await _context.Tickets.FindAsync(id);

                    if (existingTicket == null)
                    {
                        return NotFound();
                    }

                    var subcategory = await _context.Subcategories.FindAsync(ticket.SubCategoryId);
                    var category = await _context.Categories.FindAsync(ticket.CategoryId);


                    // Update the fields that need to be modified
                    existingTicket.Title = ticket.Title;
                    existingTicket.Description = ticket.Description;
                    existingTicket.CategoryId = ticket.CategoryId;
                    existingTicket.SubCategoryId = ticket.SubCategoryId;
                    existingTicket.Category = category;
                    existingTicket.SubCategory = subcategory;

                    // Save the changes to the database
                    _context.Update(existingTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Tickets));
            }

            // If the model state is not valid, return to the edit view with the ticket data
            return View(ticket);
        }


        // GET: Admin/CreateTicket
        public IActionResult CreateTicket()
        {



            var categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            // Set the SelectList in ViewData
            ViewData["CategoryId"] = new SelectList(categories, "Value", "Text");

            // Retrieve subcategories from the database and convert them into a SelectList
            var subcategories = _context.Subcategories.Select(s => new SelectListItem
            {
                Value = s.SubcategoryId.ToString(),
                Text = s.Name
            }).ToList();

            // Set the SelectList in ViewData
            ViewData["SubcategoryId"] = new SelectList(subcategories, "Value", "Text");

            return View();

        }


        // POST: Admin/CreateTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket([Bind("Title,Description,CategoryId,SubCategoryId")] Ticket ticket)
        {
            
            
                var agentRole = await _userManager.FindByNameAsync("Agent");
                var agents = await _userManager.GetUsersInRoleAsync(agentRole.Id);


                if (ModelState.IsValid)
                {
                    // Retrieve the ID of the logged-in user
                    string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Set the UserId property of the ticket
                    ticket.UserId = currentUserId;

                    // Retrieve the ID of an agent from your database (you can implement your logic to select an agent)
                    string agentId = await GetAgentIdAsync();

                    // Set the AgentId property of the ticket
                    ticket.AgentId = agentId;

                    // Add the ticket to the database
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Tickets));
                }
                // If the ModelState is not valid, return the view with the submitted ticket model
                return View(ticket);
            }
            

        private async Task<string> GetAgentIdAsync()
        {
            // Find an agent with the "Agent" role
            var agentRole = await _userManager.FindByNameAsync("Agent");
            var agents = await _userManager.GetUsersInRoleAsync(agentRole.Id);

            // Return the ID of the first agent found
            return agents.FirstOrDefault()?.Id;
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }

        public IActionResult CreateUser()
        {
            return View();
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
                        ModelState.AddModelError("Email", "Email Already Exsists . Choose Another one");
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

