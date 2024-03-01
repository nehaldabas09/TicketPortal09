using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManagerController(TicketDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ManagerIndex()
        {
            // Retrieve all tickets
            var tickets = await _context.Tickets.ToListAsync();
            return View(tickets);
        }

        // GET: Manager/EditTicket/5
        // GET: Manager/EditTicket/5
        public async Task<IActionResult> ManagerEdit(int? id)
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

            // Retrieve the agent's username
            var agent = await _userManager.FindByIdAsync(ticket.AgentId);
            if (agent != null)
            {
                ViewData["AgentUsername"] = agent.UserName;
            }
            else
            {
                // Handle the case where the agent is not found
                ViewData["AgentUsername"] = "Not assigned";
            }

            // Populate the dropdown list with agent usernames
            ViewData["AgentId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Agent"), "Id", "UserName", ticket.AgentId);

            return View(ticket);
        }


        // POST: Manager/EditTicket/5
        // POST: Manager/EditTicket/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagerEdit(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the new agent by username
                    var newAgent = await _userManager.FindByIdAsync(ticket.AgentId);
                    if (newAgent != null)
                    {
                        // Update the ticket's agent
                        ticket.AgentId = newAgent.Id;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Handle the case where the user with the provided ID is not found
                        ModelState.AddModelError("AgentId", "Agent not found");
                        ViewData["AgentId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Agent"), "Id", "UserName", ticket.AgentId);
                        return View(ticket);
                    }
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
                return RedirectToAction(nameof(ManagerIndex));
            }

            // Populate the dropdown list with agent usernames again if the model state is invalid
            ViewData["AgentId"] = new SelectList(await _userManager.GetUsersInRoleAsync("Agent"), "Id", "UserName", ticket.AgentId);

            return View(ticket);
        }




        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}


/*private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
*/