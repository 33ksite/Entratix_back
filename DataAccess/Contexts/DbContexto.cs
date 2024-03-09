using Microsoft.EntityFrameworkCore;
using Domain;

namespace DataAccess.Contexts
{
    public class DbContexto : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbContexto(DbContextOptions<DbContexto> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Role>().Property(r => r.Id).HasColumnName("id");
            modelBuilder.Entity<Role>().HasKey(r => r.Id);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(r => r.Id).HasColumnName("id");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<Event>().Property(e => e.Id).HasColumnName("id");
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            modelBuilder.Entity<Event>().HasOne(e => e.User).WithMany(u => u.Events).HasForeignKey(e => e.UserId);
        }
    }
}
