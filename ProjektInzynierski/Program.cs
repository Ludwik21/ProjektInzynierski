using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models.ProjektContext;

var builder = WebApplication.CreateBuilder(args);

// Dodanie kontrolerów i widoków
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
    options.Cookie.IsEssential = true; // Wymagane dla zgodnoœci z RODO
});

// Konfiguracja autoryzacji za pomoc¹ plików cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login"; // Œcie¿ka do logowania
        options.AccessDeniedPath = "/Users/Login"; // Œcie¿ka w przypadku braku dostêpu
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
