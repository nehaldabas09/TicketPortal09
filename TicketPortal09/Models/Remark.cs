using System.ComponentModel.DataAnnotations;

namespace TicketPortal09.Models
{
    public class Remark
    {
        [Key]
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; }


        public string Title {  get; set; }

        public string Status { get; set; }

        public DateTime DateTime { get; set; }

    }
}
