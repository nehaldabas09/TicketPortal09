using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;
using TicketPortal09.Services;

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IManagerService _managerService;
        private readonly ILogger<ManagerController> _logger;

        public ManagerController(TicketDbContext context, UserManager<IdentityUser> userManager, IManagerService managerService, ILogger<ManagerController> logger)
        {
            _context = context;
            _userManager = userManager;
            _managerService = managerService;
            _logger = logger;
        }

        public async Task<IActionResult> ManagerIndex()
        {
            try
            {

                var tickets = await _managerService.GetAllTicketsAsync();
                return View(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Manager/EditTicket/5
        public async Task<IActionResult> ManagerEdit(int? id)
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

                ViewData["AgentUsername"] = await GetAgentUsername(ticket.AgentId);
                ViewData["AgentId"] = new SelectList(await _managerService.GetAgentsAsync(), "Id", "UserName", ticket.AgentId);

                return View(ticket);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

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
                    var newAgent = await _managerService.FindUserByIdAsync(ticket.AgentId);
                    if (newAgent != null)
                    {
                        ticket.AgentId = newAgent.Id;
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("AgentId", "Agent not found");
                        ViewData["AgentId"] = new SelectList(await _managerService.GetAgentsAsync(), "Id", "UserName", ticket.AgentId);
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

            ViewData["AgentId"] = new SelectList(await _managerService.GetAgentsAsync(), "Id", "UserName", ticket.AgentId);

            return View(ticket);
        }

        private async Task<string> GetAgentUsername(string agentId)
        {
            try
            {
                var agent = await _managerService.FindUserByIdAsync(agentId);
                return agent != null ? agent.UserName : "Not assigned";

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
    }
}
