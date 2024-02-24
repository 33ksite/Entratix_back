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

        public List<string> GetAll()
        {
            
          
            return new List<string> { "user1", "user2", "user3" };
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



        public async Task<string> Update(User user)
        {
            try
            {
                var existingUser = await _dbContexto.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Phone = user.Phone;
                    existingUser.PasswordHash = user.PasswordHash;
                    existingUser.PasswordSalt = user.PasswordSalt;
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



    }
}
