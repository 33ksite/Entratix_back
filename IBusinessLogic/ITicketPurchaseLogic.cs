using Domain;
using System.Threading.Tasks;

namespace IBusinessLogic
{
    public interface ITicketPurchaseLogic
    {
        Task<bool> PurchaseTickets(List<TicketPurchase> ticketPurchases);
    }
}
