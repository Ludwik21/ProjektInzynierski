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
    }
}
