using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektInzynierski.Application.Services;
using ProjektInzynierski.Infrastructure.Repositories;
using ProjektInzynierski.Infrastructure.Models;
using ProjektInzynierski.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja logowania
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Tworzenie loggera
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
    config.AddDebug();
}).CreateLogger<Program>();

logger.LogInformation("Rozpoczynanie konfiguracji aplikacji...");

//// Konfiguracja uwierzytelniania z plikami cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Users/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();


logger.LogInformation("Dodano konfiguracj� uwierzytelniania z plikami cookie.");

// Konfiguracja MVC i kontekstu bazy danych
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjektContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ProjektInzynierski.Infrastructure")); // Wskazanie projektu z migracjami
    logger.LogInformation("Po��czono z baz� danych za pomoc� �a�cucha po��czenia: {ConnectionString}",
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Dodanie IPasswordHasher do kontenera DI
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IEquipmentCompatibilityService, EquipmentCompatibilityService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHttpContextAccessor();

// Konfiguracja sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

logger.LogInformation("Konfiguracja sesji zako�czona.");

// Konfiguracja CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7049")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

logger.LogInformation("Konfiguracja CORS zosta�a dodana.");

// Konfiguracja autoryzacji
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

logger.LogInformation("Konfiguracja autoryzacji zosta�a zako�czona.");

var app = builder.Build();

///Inicjalizacja danych SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProjektContext>();

    try
    {
        logger.LogInformation("Rozpoczynanie inicjalizacji danych SeedData...");
        SeedData.Initialize(services, context); // Wywo�anie metody SeedData
        logger.LogInformation("Dane SeedData zosta�y pomy�lnie zainicjalizowane.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Wyst�pi� b��d podczas inicjalizacji danych SeedData.");
    }
}


// Middleware dla b��d�w i wymuszenia HTTPS
if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
        logger.LogInformation("W��czono obs�ug� HSTS w �rodowisku produkcyjnym.");
    }
    else
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
        logger.LogInformation("W��czono stron� b��d�w dla deweloper�w.");
    }

    // Middleware
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("AllowSpecificOrigin");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSession();


    logger.LogInformation("Middleware zosta�y poprawnie skonfigurowane.");

    // Dodaj inicjalizacj� danych SeedData
    using (var serviceScope = app.Services.CreateScope())
    {
        var scopedServices = serviceScope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<ProjektContext>();

        try
        {
            SeedData.Initialize(scopedServices, dbContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing SeedData: {ex.Message}");
        }
    }

    

    // Mapowanie tras
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    logger.LogInformation("Dodano mapowanie trasy domy�lnej.");

    app.MapControllerRoute(
        name: "adminDashboard",
        pattern: "admin/dashboard",
        defaults: new { controller = "Users", action = "Dashboard" })
        .RequireAuthorization("AdminOnly");

    logger.LogInformation("Dodano mapowanie trasy dla panelu administratora.");

    // Uruchamianie aplikacji
    logger.LogInformation("Uruchamianie aplikacji...");
    app.Run();
