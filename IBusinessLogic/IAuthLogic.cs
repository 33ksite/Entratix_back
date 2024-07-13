using Domain;

namespace IBusinessLogic
{
    public interface IAuthLogic
    {
        public Task<string> RegisterAsync(User user);
        public Task<User> GetUserAsync(string userEmail);
        public Task<User> GetUserByTokenAsync(string token);
        public Task<string> RefreshTokensAsync(User user);
        public Task<string> RevokeToken(User user);
    }
}
