using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjektInzynierski.Domain.Entities.Users;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Seeding;
public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider, ProjektContext context)
    {
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();

        // Tworzenie konta Admin, jeśli nie istnieje
        if (!context.Users.Any(u => u.UserEmail == "admin@projekt.com"))
        {
            var adminUser = new User
            {
                UserName = "Admin",
                UserEmail = "admin@projekt.com",
                UserPhone = "123456789",
                UserRole = Role.Admin,
                UserPassword = passwordHasher.HashPassword(null, "admin") // Hasło: Admin123!
            };
            context.Users.Add(adminUser);
        }

        // Tworzenie konta Employee, jeśli nie istnieje
        if (!context.Users.Any(u => u.UserEmail == "employee@projekt.com"))
        {
            var employeeUser = new User
            {
                UserName = "Employee",
                UserEmail = "employee@projekt.com",
                UserPhone = "987654321",
                UserRole = Role.Employee,
                UserPassword = passwordHasher.HashPassword(null, "employee") // Hasło: Employee123!
            };
            context.Users.Add(employeeUser);
        }

        // Tworzenie konta Client, jeśli nie istnieje
        if (!context.Users.Any(u => u.UserEmail == "client@projekt.com"))
        {
            var clientUser = new User
            {
                UserName = "Client",
                UserEmail = "client@projekt.com",
                UserPhone = "555555555",
                UserRole = Role.Client,
                UserPassword = passwordHasher.HashPassword(null, "client") // Hasło: Client123!
            };
            context.Users.Add(clientUser);
        }

        context.SaveChanges();
    }
}
