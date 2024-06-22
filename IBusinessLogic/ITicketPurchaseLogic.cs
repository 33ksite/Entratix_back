using Domain;
using System.Threading.Tasks;

namespace IBusinessLogic
{
    public interface ITicketPurchaseLogic
    {
        Task<string> PurchaseTicket(TicketPurchase ticketPurchase);
    }
}
