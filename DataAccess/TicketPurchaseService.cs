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

                // Validate that the EventTicket belongs to the Event
                if (eventTicket.EventId != eventEntity.Id)
                {
                    return "The EventTicket does not belong to the specified Event";
                }

                // Verify if there is an existing purchase with the same UserId, EventId, and TicketTypeId
                var existingPurchase = await _dbContexto.TicketPurchases
                    .FirstOrDefaultAsync(tp => tp.UserId == ticketPurchase.UserId &&
                                               tp.EventId == ticketPurchase.EventId &&
                                               tp.TicketTypeId == ticketPurchase.TicketTypeId);

                if (existingPurchase != null)
                {
                    existingPurchase.QuantityPurchased += ticketPurchase.QuantityPurchased;
                    _dbContexto.TicketPurchases.Update(existingPurchase);
                }
                else
                {
                    ticketPurchase.User = user;
                    ticketPurchase.Event = eventEntity;
                    ticketPurchase.EventTicket = eventTicket;

                    _dbContexto.TicketPurchases.Add(ticketPurchase);
                }

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
