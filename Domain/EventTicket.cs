using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class EventTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("eventid")]
        public int EventId { get; set; }

        [Required]
        [Column("entry")]
        public string Entry { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        public Event Event { get; set; }
    }
}
