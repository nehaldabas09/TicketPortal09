using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public class AgentService : IAgentService
    {
        private readonly TicketDbContext _context;

        public AgentService(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAgentTicketsAsync(string agentId)
        {
            return await _context.Tickets.Where(t => t.AgentId == agentId).ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<bool> UpdateTicketAsync(int id, Ticket ticket, string agentId, string remark)
        {
            if (id != ticket.TicketId)
            {
                return false;
            }

            try
            {
                var existingTicket = await _context.Tickets.FindAsync(id);

                if (existingTicket == null || existingTicket.AgentId != agentId)
                {
                    return false;
                }

                existingTicket.Status = ticket.Status;
                _context.Update(existingTicket);
                await _context.SaveChangesAsync();

                Remark newRemark = new Remark
                {
                    TicketId = ticket.TicketId,
                    Title = remark,
                    DateTime = DateTime.Now,
                    UserId = ticket.UserId,
                    Status = ticket.Status
                };

                await _context.Remarks.AddAsync(newRemark);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
