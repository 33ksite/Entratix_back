using DataAccess.Contexts;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserService : IUserService
    {
        private readonly DbContexto _dbContexto;

        public UserService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public async Task<string> Insert(User user)
        {
            try
            {

                await _dbContexto.Users.AddAsync(user);
                await _dbContexto.SaveChangesAsync();

                return "Usuario insertado correctamente";
            }
            catch (Exception ex)
            {
                return ($"Error al insertar el usuario: {ex.Message}");
            }
        }

        public async Task<User> Get(string userEmail)
        {
            try
            {

                User user = await _dbContexto.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                return user;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener el usuario: {ex}");
                return null;
            }
        }



        public async Task<string> UpdateToken(User user)
        {
            try
            {
                var existingUser = await _dbContexto.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    
                    existingUser.Token = user.Token;
                    existingUser.TokenCreated = user.TokenCreated;
                    existingUser.TokenExpires = user.TokenExpires;


                    await _dbContexto.SaveChangesAsync();
                    return "Usuario actualizado correctamente";
                }
                else
                {
                    return "Usuario no encontrado";
                }
            }
            catch (Exception ex)
            {
                return $"Error al actualizar el usuario: {ex.Message}";
            }
        }

        public async Task<User> GetUserByToken(string token)
        {
            try
            {
                return await _dbContexto.Users.FirstOrDefaultAsync(x => x.Token == token);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred while finding user by token: {ex.Message}");
                return null;
            }
        }

        public async Task<List<TicketPurchase>> GetUserTickets(int userId)
        {
            return await _dbContexto.TicketPurchases
                .Include(tp => tp.Event)
                .Include(tp => tp.EventTicket)
                .Where(tp => tp.UserId == userId)
                .ToListAsync();
        }
    }
}
