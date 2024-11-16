using Microsoft.EntityFrameworkCore;

namespace ProjektInzynierski.Models.ProjektContext
{
    public class ProjektContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }

        public ProjektContext(DbContextOptions<ProjektContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasNoKey();

            modelBuilder.Entity<Equipment>()
                .Property(e => e.PricePerDay)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
