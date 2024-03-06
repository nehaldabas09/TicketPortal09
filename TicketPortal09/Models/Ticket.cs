using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace TicketPortal09.Models
{
    public class Ticket
    {


        [Key]
        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public string? Status { get; set; } // Can be "Open", "Assigned", "Closed", etc.

        // Foreign keys
        public string AgentId { get; set; }

        public string UserId { get; set; } // User who submitted the ticket

        public int CategoryId { get; set; } // Foreign key for Category
        public int SubCategoryId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public ApplicationUser? Agent { get; set; }
        public Category? Category { get; set; } // Navigation property for Category
        public SubCategory? SubCategory { get; set; }

    }
}