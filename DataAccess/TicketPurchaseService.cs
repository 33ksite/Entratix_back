using DataAccess.Contexts;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TicketPurchaseService : ITicketPurchaseService
    {
        private readonly DbContexto _dbContexto;

        public TicketPurchaseService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public async Task<string> PurchaseTicket(TicketPurchase ticketPurchase)
        {
            try
            {
                // Load related entities
                var user = await _dbContexto.Users.FindAsync(ticketPurchase.UserId);
                var eventEntity = await _dbContexto.Events.FindAsync(ticketPurchase.EventId);
                var eventTicket = await _dbContexto.EventTickets.FindAsync(ticketPurchase.TicketTypeId);

                if (user == null || eventEntity == null || eventTicket == null)
                {
                    return "User, Event, or EventTicket not found";
                }

                if (eventTicket.EventId != eventEntity.Id)
                {
                    return "The EventTicket does not belong to the specified Event";
                }

                ticketPurchase.User = user;
                ticketPurchase.Event = eventEntity;
                ticketPurchase.EventTicket = eventTicket;

                var newTicketPurchase = new TicketPurchase
                {
                    UserId = ticketPurchase.UserId,
                    EventId = ticketPurchase.EventId,
                    TicketTypeId = ticketPurchase.TicketTypeId,
                    QuantityPurchased = 1,
                    TicketCode = ticketPurchase.TicketCode,           
                    Used = ticketPurchase.Used,
                    User = user,
                    Event = eventEntity,
                    EventTicket = eventTicket
                };

                _dbContexto.TicketPurchases.Add(newTicketPurchase);

                await _dbContexto.SaveChangesAsync();
                return "Ticket purchased successfully";
            }
            catch (Exception ex)
            {
                return $"Error purchasing ticket: {ex.Message}";
            }
        }
    }
}
