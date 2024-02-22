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

            //Guardar en una variable todas las tablas de la bdd
        foreach (var entityType in entityTypes)
            {
                _dbContexto.Model.FindEntityType(entityType.Name);
            }

            var test = _dbContexto.Users.FirstOrDefault();
            var test2 = _dbContexto.Roles.FirstOrDefault(r => r.Id == 1);

            //Obtener lista de nombres de usuarios
            return _dbContexto.Users.Select(x => x.FirstName).ToList();
        }
    }
}
