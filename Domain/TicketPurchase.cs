using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TicketPurchase
    {
        [Required]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("eventid")]
        public int EventId { get; set; }

        [Required]
        [Column("tickettypeid")]
        public int TicketTypeId { get; set; }

        [Required]
        [Column("quantity_purchased")]
        public int QuantityPurchased { get; set; }

        [Column("used")]
        public bool Used { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
        public TicketType TicketType { get; set; }
    }
}
