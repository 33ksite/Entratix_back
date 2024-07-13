using Domain;

namespace IDataAccess
{
    public interface IUserService
    {

        Task<string> Insert(User user);

        Task<User> Get(string userEmail);

        Task<User> GetUserByToken(string token);

        Task<string> UpdateToken(User user);

        Task<List<TicketPurchase>> GetUserTickets(int userId);
    }
}
