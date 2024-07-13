using Domain;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface ITicketPurchaseService
    {
        Task<string> PurchaseTicket(TicketPurchase ticketPurchase);
    }
}
