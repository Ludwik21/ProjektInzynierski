using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models.ProjektContext;

var builder = WebApplication.CreateBuilder(args);

// Dodanie kontrolerów z widokami (MVC)
builder.Services.AddControllersWithViews();

// Konfiguracja bazy danych
builder.Services.AddDbContext<ProjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodanie Swaggera do dokumentacji API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware dla œrodowiska Development (Swagger, Swagger UI)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Routing aplikacji
app.UseRouting();
app.UseAuthentication();
// Autoryzacja (mo¿esz dodaæ, jeœli w przysz³oœci bêdziesz mia³ mechanizmy autoryzacji)
app.UseAuthorization();

// Konfiguracja œcie¿ek i routingu
app.UseEndpoints(endpoints =>
{
    // Domyœlny routing dla aplikacji
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Uruchomienie aplikacji
app.Run();
