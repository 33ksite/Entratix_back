using DataAccess.Contexts;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                // Verify if there is an existing purchase with the same UserId, EventId, and Entry
                var existingPurchase = await _dbContexto.TicketPurchases
                    .FirstOrDefaultAsync(tp => tp.UserId == ticketPurchase.UserId &&
                                               tp.EventId == ticketPurchase.EventId &&
                                               tp.Entry == ticketPurchase.Entry);

                if (existingPurchase != null)
                {
                    existingPurchase.QuantityPurchased += ticketPurchase.QuantityPurchased;
                    _dbContexto.TicketPurchases.Update(existingPurchase);
                }
                else
                {
                    // Load related entities
                    ticketPurchase.User = await _dbContexto.Users.FindAsync(ticketPurchase.UserId);
                    ticketPurchase.Event = await _dbContexto.Events.FindAsync(ticketPurchase.EventId);

                    if (ticketPurchase.User == null || ticketPurchase.Event == null)
                    {
                        return "User or Event not found";
                    }

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
