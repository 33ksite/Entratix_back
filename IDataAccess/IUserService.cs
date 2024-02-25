using Domain;

namespace IDataAccess
{
    public interface IUserService
    {
        List<string> GetAll();

        Task<string> Insert(User user);

        Task<User> Get(string userEmail);

        Task<User> GetUserByToken(string token);

        Task<string> UpdateToken(User user);
    }
}
