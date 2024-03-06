using System.ComponentModel.DataAnnotations;

namespace TicketPortal09.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Navigation property
       public ICollection<SubCategory>? Subategories { get; set; }
       /* public ICollection<Ticket> Tickets { get; set; }*/
    }
}
