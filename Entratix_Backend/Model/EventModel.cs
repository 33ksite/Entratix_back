using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Entratix_Backend.Model
{
    public class EventModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public decimal Cost { get; set; }
        public string Photo { get; set; }
        public string Department { get; set; }
        public List<EventTicketModel> EventTickets { get; set; }

        // Constructor that takes an Event object
        public EventModel(Event ev)
        {
            Id = ev.Id;
            UserId = ev.UserId;
            Name = ev.Name;
            Description = ev.Description;
            Date = ev.Date;
            Location = ev.Location;
            Photo = ev.Photo;
            Department = ev.Department;
            EventTickets = ev.EventTickets.Select(et => new EventTicketModel(et)).ToList();
        }

        // Static method to create an Event object from EventModel
        public static Event CreateEventModel(EventModel eventModel)
        {
            return new Event
            {
                Id = eventModel.Id,
                UserId = eventModel.UserId,
                Name = eventModel.Name,
                Description = eventModel.Description,
                Date = eventModel.Date,
                Location = eventModel.Location,
                Photo = eventModel.Photo,
                Department = eventModel.Department,
                EventTickets = eventModel.EventTickets.Select(et => new EventTicket
                {
                    Id = et.Id,
                    EventId = eventModel.Id, // Assuming EventId should be set here
                    Entry = et.Entry,
                    Quantity = et.Quantity,
                    Price = et.Price
                }).ToList()
            };
        }
    }

    public class EventTicketModel
    {
        public int Id { get; set; }
        public string Entry { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public EventTicketModel(EventTicket et)
        {
            Id = et.Id;
            Entry = et.Entry;
            Quantity = et.Quantity;
            Price = et.Price;
        }
    }
}
