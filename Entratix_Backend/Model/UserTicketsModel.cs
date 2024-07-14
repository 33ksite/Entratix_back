using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Entratix_Backend.Model
{
    public class UserTicketsModel
    {
        public EventDto Event { get; set; }
        public List<TicketDetailDto> Tickets { get; set; }

        public UserTicketsModel(Event ev, List<TicketPurchase> ticketPurchases)
        {
            Event = new EventDto(ev);
            Tickets = ticketPurchases.Select(tp => new TicketDetailDto(tp)).ToList();
        }
    }

    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public string Department { get; set; }

        public EventDto(Event ev)
        {
            Id = ev.Id;
            Name = ev.Name;
            Description = ev.Description;
            Date = ev.Date;
            Location = ev.Location;
            Photo = ev.Photo;
            Department = ev.Department;
        }
    }

    public class TicketDetailDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketTypeId { get; set; }
        public string Entry { get; set; }
        public int QuantityPurchased { get; set; }
        public bool Used { get; set; }
        public decimal Price { get; set; } // Nuevo campo para el costo del ticket

        public TicketDetailDto(TicketPurchase ticketPurchase)
        {
            UserId = ticketPurchase.UserId;
            EventId = ticketPurchase.EventId;
            TicketTypeId = ticketPurchase.TicketTypeId;
            Entry = ticketPurchase.EventTicket?.Entry ?? string.Empty;
            QuantityPurchased = ticketPurchase.QuantityPurchased;
            Used = ticketPurchase.Used;
            Price = ticketPurchase.EventTicket?.Price ?? 0; // Obtener el precio del ticket
        }
    }
}
