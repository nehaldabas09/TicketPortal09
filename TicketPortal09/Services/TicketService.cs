// Services/TicketService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public class TicketService : ITicketService
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TicketService(TicketDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<Ticket>> GetTicketsForUserAsync(string userId)
        {
            return await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Agent)
                .Include(t => t.Category)
                .Include(t => t.SubCategory)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
        }

        public async Task<string> GetAgentIdAsync()
        {
            var agentRole = await _roleManager.FindByNameAsync("Agent");
            var agents = await _userManager.GetUsersInRoleAsync(agentRole.Name);
            return agents.FirstOrDefault()?.Id;
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket, string userId, string agentId)
        {
            ticket.UserId = userId;
            ticket.AgentId = agentId;
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            _context.Update(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
