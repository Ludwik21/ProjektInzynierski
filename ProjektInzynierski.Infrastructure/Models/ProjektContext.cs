using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Infrastructure.Models.Configuration;

namespace ProjektInzynierski.Infrastructure.Models
{
    public class ProjektContext : DbContext
    {
        public ProjektContext(DbContextOptions<ProjektContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ClientDao> Clients { get; set; }
        public DbSet<EquipmentDao> Equipment { get; set; }
        public DbSet<EquipmentCompatibility> EquipmentCompatibility { get; set; }
        public DbSet<ReservationDao> Reservations { get; set; }
        public DbSet<ReservationItemDao> ReservationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration( new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationItemConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentCompatibilityConfiguration());
            modelBuilder.Entity<User>().Property(u => u.UserEmail).IsRequired();
        }
    }
}
