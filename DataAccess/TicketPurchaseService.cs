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
                // Verificar si ya existe una compra de ticket con los mismos UserId, EventId y TicketTypeId
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
                    // Cargar entidades relacionadas
                    ticketPurchase.User = await _dbContexto.Users.FindAsync(ticketPurchase.UserId);
                    ticketPurchase.Event = await _dbContexto.Events.FindAsync(ticketPurchase.EventId);
                    ticketPurchase.TicketType = await _dbContexto.TicketTypes.FindAsync(ticketPurchase.TicketTypeId);

                    if (ticketPurchase.User == null || ticketPurchase.Event == null || ticketPurchase.TicketType == null)
                    {
                        return "User, Event, or TicketType not found";
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
