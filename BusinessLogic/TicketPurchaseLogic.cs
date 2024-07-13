using Domain;
using IBusinessLogic;
using IDataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class TicketPurchaseLogic : ITicketPurchaseLogic
    {
        private readonly ITicketPurchaseService _ticketPurchaseService;

        public TicketPurchaseLogic(ITicketPurchaseService ticketPurchaseService)
        {
            _ticketPurchaseService = ticketPurchaseService;
        }

        public async Task<bool> PurchaseTickets(List<TicketPurchase> ticketPurchases)
        {
            foreach (var ticketPurchase in ticketPurchases)
            {
                var result = await _ticketPurchaseService.PurchaseTicket(ticketPurchase);
                if (result != "Ticket purchased successfully")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
