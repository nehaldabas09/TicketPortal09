using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace TicketPortal09.Models
{
    public class Ticket
    /*    {
            public int TicketId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Status { get; set; } // Can be "Open", "Assigned", "Closed", etc.

            // Foreign keys
            public string UserId { get; set; } // User who submitted the ticket
            public string AgentId { get; set; } // Agent assigned to the ticket
            // Navigation properties
            public IdentityUser User { get; set; }
            public IdentityUser Agent { get; set; }
        }*/
    {
        [Key]
        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // Can be "Open", "Assigned", "Closed", etc.

        // Foreign keys
        public string UserId { get; set; } // User who submitted the ticket
        public string AgentId { get; set; } // Agent assigned to the ticket
        public int CategoryId { get; set; } // Foreign key for Category
        public int SubCategoryId { get; set; }

        // Navigation properties
        public IdentityUser User { get; set; }
        public IdentityUser Agent { get; set; }
        public Category Category { get; set; } // Navigation property for Category
        public SubCategory Subcategory { get; set; }
    }
}