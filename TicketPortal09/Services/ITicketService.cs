// IServices/ITicketService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetTicketsForUserAsync(string userId);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<string> GetAgentIdAsync();
        Task<bool> CreateTicketAsync(Ticket ticket, string userId, string agentId);
        Task<bool> UpdateTicketAsync(Ticket ticket);
        Task<bool> DeleteTicketAsync(int id);
    }
}
