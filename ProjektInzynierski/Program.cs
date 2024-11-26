using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using Microsoft.Extensions.Logging;

namespace ProjektInzynierski
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dodanie logowania do konsoli i debugowania
            builder.Services.AddLogging(config =>
            {
                config.AddConsole(); // Logowanie w konsoli
                config.AddDebug();   // Logowanie w trybie debug
            });

            // Dodanie us³ug MVC
            builder.Services.AddControllersWithViews();

            // Dodanie kontekstu bazy danych
            builder.Services.AddDbContext<ProjektContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dodanie sesji
            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache(); // Wymagane do sesji
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas przechowywania sesji
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Wymagane dla GDPR
            });

            // Konfiguracja autoryzacji z ciasteczkami
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/Login";
                });

            // Dodanie roli administratora w procesie logowania
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // Dodanie inicjalizacji SeedData (dodanie admina do bazy)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ProjektContext>();
                SeedData.Initialize(services, context);
            }

            // Konfiguracja logowania i b³êdów
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // W³¹cz szczegó³y b³êdów w trybie developerskim
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Globalna obs³uga b³êdów w trybie produkcyjnym
                app.UseHsts();  // Wymuszenie HTTPS w produkcji
            }

            // U¿ywanie statycznych plików
            app.UseStaticFiles();

            // Sesja przed autoryzacj¹ i routingiem
            app.UseSession();

            // Autoryzacja i routing
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Definicja domyœlnej trasy
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Dodanie trasy tylko dla administratora (dashboard)
            app.MapControllerRoute(
                name: "adminDashboard",
                pattern: "admin/dashboard",
                defaults: new { controller = "Users", action = "Dashboard" })
                .RequireAuthorization("AdminOnly");  // Ochrona trasy

            // Uruchomienie aplikacji
            app.Run();
        }
    }
}
