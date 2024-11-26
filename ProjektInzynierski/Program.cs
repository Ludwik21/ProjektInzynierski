using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models.ProjektContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache(); // Wymagane do sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas przechowywania sesji
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Wymagane dla GDPR
});


// Konfiguracja autoryzacji
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.AccessDeniedPath = "/Users/Login";
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseSession(); // Sesja przed autoryzacj¹ i routingiem
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
