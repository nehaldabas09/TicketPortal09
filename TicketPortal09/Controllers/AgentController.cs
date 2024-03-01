using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;
using System.Security.Claims;

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly TicketDbContext _context;

        public AgentController(TicketDbContext context)
        {
            _context = context;
        }

        // GET: Agent/Tickets
        public async Task<IActionResult> AgentIndex()
        {
            var currentAgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var agentTickets = await _context.Tickets
                .Where(t => t.AgentId == currentAgentId)
                .ToListAsync();

            var tickets = await _context.Tickets.ToListAsync();

            // Fetch remarks for each ticket
            
           

            return View(agentTickets);
        }

        // GET: Agent/EditTicket/5
        public async Task<IActionResult> EditTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null || ticket.AgentId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            List<Remark> remarks = await _context.Remarks.Where(a=>a.TicketId==id).ToListAsync();

            ViewBag.Remarks = remarks;

            return View(ticket);
        }

        // POST: Agent/EditTicket/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(int id,Ticket ticket,string remark)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentAgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var existingTicket = await _context.Tickets.FindAsync(id);

                    if (existingTicket == null || existingTicket.AgentId != currentAgentId)
                    {
                        return NotFound();
                    }

                    existingTicket.Status = ticket.Status;
                    _context.Update(existingTicket);
                    await _context.SaveChangesAsync();

                    Remark newRemark = new Remark {
                        TicketId = ticket.TicketId,
                        Title = remark,
                        DateTime = DateTime.Now,
                        UserId = ticket.UserId,
                        Status = ticket.Status
                    };

                    await _context.Remarks.AddAsync(newRemark);
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
                return RedirectToAction(nameof(AgentIndex));
            }
            return View(ticket);
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
