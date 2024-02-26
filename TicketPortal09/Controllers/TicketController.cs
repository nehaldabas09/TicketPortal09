/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TicketPortal09.Data;
using TicketPortal09.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TicketPortal09.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController(TicketDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.Category = _dbContext.Categories.ToList();
            ViewBag.Subcategory = _dbContext.Subcategories.ToList();
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,CategoryId,SubcategoryId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Check if the user is in the "Agent" role
                    if (await _userManager.IsInRoleAsync(user, "Agent"))
                    {
                        ticket.AgentId = user.Id;
                    }
                }

                // Add the ticket to the database
                _dbContext.Add(ticket);
                await _dbContext.SaveChangesAsync();

                // Redirect to the ticket details action after saving the ticket
                return RedirectToAction(nameof(Details), new { id = ticket.TicketId });
            }

            // Repopulate ViewBag data if ModelState is not valid
            ViewBag.Category = _dbContext.Categories.ToList();
            ViewBag.Subcategory = _dbContext.Subcategories.ToList();
            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create([Bind("Title, Description, CategoryId, SubcategoryId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    ticket.UserId = user.Id; // Assign the ticket to the current user
                    ticket.Status = "Open"; // Set the initial status of the ticket
                    _dbContext.Tickets.Add(ticket); // Add the ticket to the database context
                    await _dbContext.SaveChangesAsync(); // Save changes to the database

                    return RedirectToAction("Details", "Ticket", new { id = ticket.TicketId });
                }
            }

            // If ModelState is not valid, repopulate ViewBag data and return the view
            ViewBag.Category = _dbContext.Categories.ToList();
            ViewBag.Subcategory = _dbContext.Subcategories.ToList();
            return View(ticket);

        }
        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // Other actions can be added here
    }
}*/







/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TicketPortal09.Data;
using TicketPortal09.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TicketPortal09.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController(TicketDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        public IActionResult Create()
        {
            ViewBag.Category = _dbContext.Categories.ToList();
            ViewBag.Subcategory = _dbContext.Subcategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,CategoryId,SubcategoryId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {

                    if (await _userManager.IsInRoleAsync(user, "Agent"))
                    {
                        ticket.AgentId = user.Id;
                    }
                }
                _dbContext.Tickets.Add(ticket);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Category = _dbContext.Categories.ToList();
            ViewBag.Subcategory = _dbContext.Subcategories.ToList();
            return View(ticket);
        }


    }
}*/





using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketDbContext _context;

        public TicketController(TicketDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var tickets = await _context.Tickets.Where(t => t.UserId == userId).ToListAsync();
            return View(tickets);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditTicket(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditTicket(int id, [Bind("TicketId,Title,Description,Status")] Ticket ticket)
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
                return RedirectToAction(nameof(Create));
            }
            return View(ticket);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerTickets()
        {
            var tickets = await _context.Tickets.Include(t => t.User).Include(t => t.Agent).ToListAsync();
            return View(tickets);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ReassignTicket(int? id)
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

            // Logic to reassign the ticket to a new agent

            return RedirectToAction(nameof(ManagerTickets));
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> AgentTickets()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var tickets = await _context.Tickets.Where(t => t.AgentId == userId).ToListAsync();
            return View(tickets);
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> UpdateTicketStatus(int? id, string status)
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

            // Update the ticket status
            ticket.Status = status;

            // Logic to add remarks

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AgentTickets));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
