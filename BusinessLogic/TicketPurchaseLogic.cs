using Domain;
using IBusinessLogic;
using IDataAccess;
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

        public async Task<string> PurchaseTicket(TicketPurchase ticketPurchase)
        {
            return await _ticketPurchaseService.PurchaseTicket(ticketPurchase);
        }
    }
}
