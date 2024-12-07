using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using ProjektInzynierski.Controllers;
using ProjektInzynierski.Models.Configuration;

namespace ProjektInzynierski.Models
{
    public class ProjektContext : DbContext
    {
        public ProjektContext(DbContextOptions<ProjektContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration( new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentCompatibilityConfiguration());


            //// Relacja między CartItem a Equipment
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.Equipment)
            //    .WithMany(e => e.CartItems)
            //    .HasForeignKey(ci => ci.EquipmentId);

            //// Relacja między CartItem a Client
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(ci => ci.Client)
            //    .WithMany(c => c.CartItems)
            //    .HasForeignKey(ci => ci.ClientId);


            // Opcjonalne: dokładność kwot w tabeli Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<CartItem>().HasKey(x => x.Id);
            modelBuilder.Entity<Payment>().HasKey(x => x.Id);


        }
    }
}
