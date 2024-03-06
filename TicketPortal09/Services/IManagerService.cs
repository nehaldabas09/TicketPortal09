using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public interface IManagerService
    {
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<IdentityUser> FindUserByIdAsync(string userId);
        Task<List<IdentityUser>> GetAgentsAsync();
    }

}
