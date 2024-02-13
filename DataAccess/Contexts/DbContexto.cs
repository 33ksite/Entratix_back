using Microsoft.EntityFrameworkCore;
using Domain;

namespace DataAccess.Contexts
{
    public class DbContexto : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbContexto(DbContextOptions<DbContexto> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("Id"); // Asegúrate de que coincide con el nombre de la columna en la base de datos
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
