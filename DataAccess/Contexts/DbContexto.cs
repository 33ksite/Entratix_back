using Microsoft.EntityFrameworkCore;
using Domain;

namespace DataAccess.Contexts
{
    public class DbContexto : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<EventTicket> EventTickets { get; set; }
        public DbSet<TicketPurchase> TicketPurchases { get; set; }

        public DbContexto(DbContextOptions<DbContexto> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Role>().Property(r => r.Id).HasColumnName("id");
            modelBuilder.Entity<Role>().HasKey(r => r.Id);

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("id");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasMany(u => u.TicketPurchases).WithOne(tp => tp.User).HasForeignKey(tp => tp.UserId);

            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<Event>().Property(e => e.Id).HasColumnName("id");
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            modelBuilder.Entity<Event>().HasOne(e => e.User).WithMany(u => u.Events).HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Event>().HasMany(e => e.EventTickets).WithOne(et => et.Event).HasForeignKey(et => et.EventId);
            modelBuilder.Entity<Event>().HasMany(e => e.TicketPurchases).WithOne(tp => tp.Event).HasForeignKey(tp => tp.EventId);

            modelBuilder.Entity<TicketType>().ToTable("tickettypes");
            modelBuilder.Entity<TicketType>().Property(tt => tt.Id).HasColumnName("id");
            modelBuilder.Entity<TicketType>().HasKey(tt => tt.Id);
            modelBuilder.Entity<TicketType>().HasMany(tt => tt.EventTickets).WithOne(et => et.TicketType).HasForeignKey(et => et.TicketTypeId);
            modelBuilder.Entity<TicketType>().HasMany(tt => tt.TicketPurchases).WithOne(tp => tp.TicketType).HasForeignKey(tp => tp.TicketTypeId);

            modelBuilder.Entity<EventTicket>().ToTable("eventtickets");
            modelBuilder.Entity<EventTicket>().HasKey(et => new { et.EventId, et.TicketTypeId });
            modelBuilder.Entity<EventTicket>().HasOne(et => et.Event).WithMany(e => e.EventTickets).HasForeignKey(et => et.EventId);
            modelBuilder.Entity<EventTicket>().HasOne(et => et.TicketType).WithMany(tt => tt.EventTickets).HasForeignKey(et => et.TicketTypeId);

            modelBuilder.Entity<TicketPurchase>().ToTable("ticketpurchases");
            modelBuilder.Entity<TicketPurchase>().HasKey(tp => new { tp.UserId, tp.EventId, tp.TicketTypeId });
            modelBuilder.Entity<TicketPurchase>().HasOne(tp => tp.User).WithMany(u => u.TicketPurchases).HasForeignKey(tp => tp.UserId);
            modelBuilder.Entity<TicketPurchase>().HasOne(tp => tp.Event).WithMany(e => e.TicketPurchases).HasForeignKey(tp => tp.EventId);
            modelBuilder.Entity<TicketPurchase>().HasOne(tp => tp.TicketType).WithMany(tt => tt.TicketPurchases).HasForeignKey(tp => tp.TicketTypeId);
        }
    }
}
