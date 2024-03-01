using System.ComponentModel.DataAnnotations;

namespace TicketPortal09.Models
{
    public class SubCategory
    {
        [Key]
        public int SubcategoryId { get; set; }
        public string Name { get; set; }

        // Foreign key
        //public int CategoryId { get; set; }
        // Navigation property
        public Category Category { get; set; }
        
        public int CategoryId { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
       
    } 
}
