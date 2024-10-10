using Domain;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserService _userService;

        public UserLogic(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<TicketPurchase>> GetUserTickets(int userId)
        {
            return await _userService.GetUserTickets(userId);
        }
    }
}
