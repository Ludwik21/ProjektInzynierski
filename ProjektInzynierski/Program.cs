using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using Microsoft.Extensions.Logging;

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

// Konfiguracja uwierzytelniania z plikami cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.AccessDeniedPath = "/Users/Login";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.HttpOnly = true;
    });

logger.LogInformation("Dodano konfiguracjê uwierzytelniania z plikami cookie.");

// Konfiguracja MVC i kontekstu bazy danych
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProjektContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    logger.LogInformation("Po³¹czono z baz¹ danych za pomoc¹ ³añcucha po³¹czenia: {ConnectionString}",
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dodanie IPasswordHasher do kontenera DI
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

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

logger.LogInformation("Konfiguracja sesji zakoñczona.");

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

logger.LogInformation("Konfiguracja CORS zosta³a dodana.");

// Konfiguracja autoryzacji
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

logger.LogInformation("Konfiguracja autoryzacji zosta³a zakoñczona.");

var app = builder.Build();

// Inicjalizacja danych SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProjektContext>();

    try
    {
        logger.LogInformation("Rozpoczynanie inicjalizacji danych SeedData...");
        SeedData.Initialize(services, context);
        logger.LogInformation("Dane SeedData zosta³y pomyœlnie zainicjalizowane.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Wyst¹pi³ b³¹d podczas inicjalizacji danych SeedData.");
    }
}

// Middleware dla b³êdów i wymuszenia HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    logger.LogInformation("W³¹czono obs³ugê HSTS w œrodowisku produkcyjnym.");
}
else
{
    app.UseDeveloperExceptionPage();
    logger.LogInformation("W³¹czono stronê b³êdów dla deweloperów.");
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

logger.LogInformation("Middleware zosta³y poprawnie skonfigurowane.");

// Mapowanie tras
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

logger.LogInformation("Dodano mapowanie trasy domyœlnej.");

app.MapControllerRoute(
    name: "adminDashboard",
    pattern: "admin/dashboard",
    defaults: new { controller = "Users", action = "Dashboard" })
    .RequireAuthorization("AdminOnly");

logger.LogInformation("Dodano mapowanie trasy dla panelu administratora.");

// Uruchamianie aplikacji
logger.LogInformation("Uruchamianie aplikacji...");
app.Run();
