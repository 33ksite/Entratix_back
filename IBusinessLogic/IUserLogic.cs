using Domain;

namespace IBusinessLogic
{
    public interface IUserLogic
    {

        Task<List<TicketPurchase>> GetUserTickets(int userId);

    }
}