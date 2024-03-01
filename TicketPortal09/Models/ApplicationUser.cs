using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketPortal09.Models
{
    public class ApplicationUser :IdentityUser
    {
       public ICollection<Ticket>? AgentTickets { get; set; }
       public ICollection<Ticket>? UserTickets { get; set; }
   }
}
