using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models.ProjektContext;

var builder = WebApplication.CreateBuilder(args);

// Dodanie kontroler�w i widok�w
builder.Services.AddControllersWithViews();

// Konfiguracja bazy danych
builder.Services.AddDbContext<ProjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracja sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas przechowywania sesji
    options.Cookie.HttpOnly = true; // Zabezpieczenie przed skryptami
    options.Cookie.IsEssential = true; // Wymagane dla zgodno�ci z RODO
});

// Konfiguracja autoryzacji za pomoc� plik�w cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login"; // �cie�ka do logowania
        options.AccessDeniedPath = "/Users/Login"; // �cie�ka w przypadku braku dost�pu
    });

var app = builder.Build();

// Konfiguracja potoku middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
