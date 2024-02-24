using Domain;

namespace IDataAccess
{
    public interface IUserService
    {
        List<string> GetAll();

        Task<string> Insert(User user);

        Task<User> Get(string userEmail);

        Task<string> Update(User user);
    }
}
