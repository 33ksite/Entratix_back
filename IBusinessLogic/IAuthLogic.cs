using Domain;

namespace IBusinessLogic
{
    public interface IAuthLogic
    {
        public Task<string> RegisterAsync(User user);

        public Task<User> LoginAsync(string userEmail);

        public Task<string> RefreshTokensAsync(User user);

    }
}
