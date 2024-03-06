using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketPortal09.Models;
using TicketPortal09.Services;

namespace TicketPortal09.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly ILogger<AgentController> _logger;


        public AgentController(IAgentService agentService, ILogger<AgentController> logger)
        {
            _agentService = agentService;
            _logger = logger;
        }

        // GET: Agent/Tickets
        public async Task<IActionResult> AgentIndex()
        {
            try
            {
                var currentAgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var agentTickets = await _agentService.GetAgentTicketsAsync(currentAgentId);

                return View(agentTickets);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Agent/EditTicket/5
        public async Task<IActionResult> EditTicket(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                var ticket = await _agentService.GetTicketByIdAsync(id.Value);
                if (ticket == null || ticket.AgentId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Agent/EditTicket/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTicket(int id, Ticket ticket, string remark)
        {
            try
            {
                var currentAgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var success = await _agentService.UpdateTicketAsync(id, ticket, currentAgentId, remark);
                if (!success)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(AgentIndex));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
