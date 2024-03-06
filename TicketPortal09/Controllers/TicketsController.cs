using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketPortal09.Data;
using TicketPortal09.Services;
using TicketPortal09.Models;

namespace TicketPortal09.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(TicketDbContext context, ITicketService ticketService,ILogger<TicketsController> logger)
        {
            _context = context;
            _ticketService = ticketService;
            _logger = logger;
        }

        // GET: Tickets
        public async Task<IActionResult> TicketIndex()
        {
            try
            {
                _logger.LogInformation("in ticket controller ``````````````````````````````````353545645646746466666611111111111111111111111111`````````````````````````");
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Ticket> tickets = await _ticketService.GetTicketsForUserAsync(currentUserId);
                return View(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
                ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name");
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
        public async Task<IActionResult> Create(Ticket ticket)
        {
            try
            {
                string agentId = await _ticketService.GetAgentIdAsync();
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (await _ticketService.CreateTicketAsync(ticket, currentUserId, agentId))
                {
                    return RedirectToAction(nameof(TicketIndex));
                }

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
                ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);
                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }

        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
                ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            try
            {
                if (id != ticket.TicketId)
                {
                    return NotFound();
                }

                if (await _ticketService.UpdateTicketAsync(ticket))
                {
                    return RedirectToAction(nameof(TicketIndex));
                }

                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
                ViewData["SubCategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "Name", ticket.SubCategoryId);

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                if (await _ticketService.DeleteTicketAsync(id))
                {
                    return RedirectToAction(nameof(TicketIndex));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
