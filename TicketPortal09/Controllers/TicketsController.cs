using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TicketsController(TicketDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Tickets
        public async Task<IActionResult> TicketIndex()
        {
            
            

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                List<Ticket> tickets = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                 .Where(t => t.UserId == currentUserId)
                 .ToListAsync();

                

                /*var ticketDbContext = _context.Tickets.Include(t => t.Agent).Include(t => t.Category).Include(t => t.SubCategory).Include(t => t.User);*/

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Agent)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        /* public IActionResult Create()
         {
             ViewData["AgentId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
             ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
             ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryId");
             ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
             return View();
         }

         // POST: Tickets/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("TicketId,Title,Description,CreatedAt,Status,AgentId,UserId,CategoryId,SubCategoryId")] Ticket ticket)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(ticket);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(TicketIndex));
             }
             ViewData["AgentId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", ticket.AgentId);
             ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", ticket.CategoryId);
             ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryId", ticket.SubCategoryId);
             ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", ticket.UserId);
             return View(ticket);
         }*/
        public IActionResult Create()
        {
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");

            
            ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
           /* if (ModelState.IsValid)*/
            {
                string agentId = await GetAgentIdAsync();

               
                ticket.AgentId = agentId;

                
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ticket.UserId = currentUserId;

               
                _context.Add(ticket);

                
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(TicketIndex));
            }

            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);
            return View(ticket);
        }

        private async Task<string> GetAgentIdAsync()
        {
            
            var agentRole = await _roleManager.FindByNameAsync("Agent");
            var agents = await _userManager.GetUsersInRoleAsync(agentRole.Name);

            return agents.FirstOrDefault()?.Id;
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            // Only include the necessary fields for editing
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
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
                return RedirectToAction(nameof(TicketIndex));
            }

            // Only include the necessary fields for editing
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);

            return View(ticket);
        }


        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Agent)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TicketIndex));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
