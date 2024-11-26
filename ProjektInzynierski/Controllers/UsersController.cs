using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ProjektInzynierski.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProjektContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ProjektContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Users/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(); // Zwraca widok logowania
        }

        // POST: Users/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Szukamy użytkownika po nazwie użytkownika
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.Username);

                if (user != null)
                {
                    // Sprawdzamy poprawność hasła
                    if (VerifyPassword(user.UserPassword, model.Password))
                    {
                        // Tworzymy tożsamość użytkownika
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role, user.Role)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Logowanie użytkownika
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        _logger.LogInformation("Użytkownik zalogował się pomyślnie: {UserName}", user.UserName);

                        // Przekierowanie na dashboard, jeśli dane są poprawne
                        return RedirectToAction("Dashboard", "Users");
                    }
                    else
                    {
                        _logger.LogWarning("Nieprawidłowe hasło dla użytkownika: {UserName}", model.Username);
                        ModelState.AddModelError("", "Nieprawidłowe hasło.");
                    }
                }
                else
                {
                    _logger.LogWarning("Nie znaleziono użytkownika: {UserName}", model.Username);
                    ModelState.AddModelError("", "Nie znaleziono użytkownika.");
                }
            }

            return View(model);
        }

        // Funkcja weryfikująca hasło
        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            string inputHashedPassword = HashPassword(inputPassword);  // Haszujemy wprowadzone hasło
            return hashedPassword == inputHashedPassword;  // Porównujemy hashe
        }

        // Funkcja haszująca hasło użytkownika
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // GET: Users/Dashboard
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View(); // Zwraca widok dashboardu dla administratora
        }

        // POST: Users/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
    }
}
