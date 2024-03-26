using IBusinessLogic;
using Domain;
using IDataAccess;

namespace BusinessLogic
{
    public class AuthLogic : IAuthLogic
    {
        private readonly IUserService _userService;

        public AuthLogic(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<User> GetUserAsync(string userEmail)
        {
            return await _userService.Get(userEmail);
        }

        public async Task<string> RegisterAsync(User user)
        { 
           return await _userService.Insert(user);
        }

        public async Task<string> RefreshTokensAsync(User user)
        {
            return await _userService.UpdateToken(user);
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            return await _userService.GetUserByToken(token);
        }

        public async Task<string> RevokeToken(User user)
        {
            return await _userService.UpdateToken(user);
        }


    }
}
