using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Event
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("photo")]
        public string Photo { get; set; }

        [Column("department")]
        public string Department { get; set; }

        public User User { get; set; }
        public ICollection<EventTicket> EventTickets { get; set; }
        public ICollection<TicketPurchase> TicketPurchases { get; set; }
    }
}
