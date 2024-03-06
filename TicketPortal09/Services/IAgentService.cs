using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public interface IAgentService
    {
        Task<IEnumerable<Ticket>> GetAgentTicketsAsync(string agentId);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<bool> UpdateTicketAsync(int id, Ticket ticket, string agentId, string remark);
        bool TicketExists(int id);
    }
}
