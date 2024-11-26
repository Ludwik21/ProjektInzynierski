using ProjektInzynierski.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjektInzynierski.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ProjektContext context)
        {
            // Sprawdzamy, czy w bazie danych już istnieją dane użytkowników
            if (context.Users.Any())
            {
                return;   // Baza już zawiera użytkowników, więc nie dodajemy nowych
            }

            // Tworzymy nowego klienta (jeśli nie istnieje)
            var adminClient = new Client
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                Address = "Admin Address",  // Zmieniamy na odpowiednią wartość
                RegistrationDate = DateTime.Now
            };
            context.Clients.Add(adminClient);
            context.SaveChanges();  // Zapisujemy klienta w bazie danych

            // Tworzymy nowego administratora
            var adminUser = new User
            {
                UserName = "admin",
                UserEmail = "admin@admin.com",
                UserPhone = "123456789",  // Numer telefonu dla admina
                UserPassword = "hashedPasswordHere",  // Pamiętaj, żeby zahaszować hasło
                Role = "Admin",
                ClientID = adminClient.ClientID  // Powiązanie z klientem
            };

            // Dodajemy użytkownika do bazy danych
            context.Users.Add(adminUser);
            context.SaveChanges();  // Zapisujemy użytkownika w bazie danych
        }



        // Funkcja haszująca hasło użytkownika
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
