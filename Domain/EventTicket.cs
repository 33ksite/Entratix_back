using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class EventTicket
    {
        [Required]
        [Column("eventid")]
        public int EventId { get; set; }

        [Required]
        [Column("tickettypeid")]
        public int TicketTypeId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        public Event Event { get; set; }
        public TicketType TicketType { get; set; }
    }
}
