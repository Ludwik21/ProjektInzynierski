using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using ProjektInzynierski.Controllers;

namespace ProjektInzynierski.Models
{
    public class ProjektContext : DbContext
    {
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }  // Użytkownicy (klienci, administratorzy)
        public DbSet<CartItem> CartItems { get; set; } // Koszyk przedmiotów (w pamięci)
        public DbSet<Client> Clients { get; set; } // Klienci
       

        public ProjektContext(DbContextOptions<ProjektContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Koszyk CartItem nie jest mapowany na tabelę w bazie danych
            modelBuilder.Entity<CartItem>().HasNoKey();

            // Precyzja dla PricePerDay
            modelBuilder.Entity<CartItem>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Equipment>()
                .Property(e => e.PricePerDay)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // Relacja między Reservation a Client
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)               // Każda rezerwacja ma jednego klienta
                .WithMany(c => c.Reservations)       // Jeden klient może mieć wiele rezerwacji
                .HasForeignKey(r => r.ClientID);     // Klucz obcy w tabeli 'Reservation' wskazuje na ClientID

            // Relacja między Equipment a Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Equipment)            // Każda rezerwacja ma jeden sprzęt
                .WithMany()                          // Sprzęt może mieć wiele rezerwacji
                .HasForeignKey(r => r.EquipmentID);  // Klucz obcy w tabeli 'Reservation' wskazuje na EquipmentID

            // Właściwości Address i RegistrationDate dla Clienta
            modelBuilder.Entity<Client>()
                .Property(c => c.Address)
                .HasMaxLength(255)
                .IsRequired(false); // Jeśli chcesz, by adres był opcjonalny, zmień 'IsRequired' na 'false'

            modelBuilder.Entity<Client>()
                .Property(c => c.RegistrationDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()"); // Domyślna data rejestracji to bieżąca data i czas
                                                  // Dodanie AvailabilityStatus do Equipment
            modelBuilder.Entity<Equipment>()
                .Property(e => e.AvailabilityStatus)
                .HasMaxLength(50)
                .IsRequired(false); // Jeśli jest opcjonalne, ustaw IsRequired(false)

            base.OnModelCreating(modelBuilder);
        }
    }
}
