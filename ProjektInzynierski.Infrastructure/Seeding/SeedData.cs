using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjektInzynierski.Domain.Entities.Users;
using ProjektInzynierski.Infrastructure.Models;
using System;

{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ProjektContext context)
        {
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();

            // Sprawdź, czy użytkownik Admin już istnieje
            if (!context.Users.Any(u => u.UserEmail == "admin@projekt.com"))
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    UserEmail = "admin@projekt.com",
                    UserPhone = "123456789",
                    UserRole = Role.Admin, // Ustaw rolę na "Admin"
                    UserPassword = passwordHasher.HashPassword(null, "admin") // Hasło
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
