using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
  

    public class ManagerService : IManagerService
    {
        private readonly TicketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ManagerService(TicketDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<IdentityUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<IdentityUser>> GetAgentsAsync()
        {
            return (List<IdentityUser>)await _userManager.GetUsersInRoleAsync("Agent");
        }
    }
}
