using Microsoft.AspNetCore.Identity;
using ProjektInzynierski.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider, ProjektContext context)
    {
        if (!context.Users.Any())
        {
            var passwordHasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                UserName = "admin",
                UserEmail = "admin@admin.com",
                UserPhone = "123456789",
                UserPassword = passwordHasher.HashPassword(null, "Admin@123") // Hashowanie hasła
            };

            var adminClient = new Client
            {

                Address = "Admin Address",
                RegistrationDate = DateTime.UtcNow
            };

            // Powiązanie User z Client
            adminUser.Client = adminClient;

            context.Users.Add(adminUser);
            context.Clients.Add(adminClient);
            context.SaveChanges();
        }
    }
}
