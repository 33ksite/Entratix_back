using DataAccess.Contexts;
using IDataAccess;

namespace DataAccess
{
    public class UserService : IUserService
    {
        private readonly DbContexto _dbContexto;

        public UserService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public List<string> GetUsers()
        {
            var entityNames = new List<string>();

            // Obtener todas las entidades en el contexto
            var entityTypes = _dbContexto.Model.GetEntityTypes();

            // Iterar sobre las entidades y obtener sus nombres
            foreach (var entityType in entityTypes)
            {
                entityNames.Add(entityType.Name);
            }


            //Obtener lista de nombres de usuarios
            return _dbContexto.Users.Select(u => u.FirstName).ToList();
        }
    }
}
