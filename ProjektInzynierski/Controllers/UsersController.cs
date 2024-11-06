using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using ProjektInzynierski.Models.ProjektContext;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace ProjektInzynierski.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ProjektContext _context;

        public UsersController(ProjektContext context)
        {
            _context = context;
        }

        // GET: Users/Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Zwraca widok Login.cshtml
        }

        // POST: Users/Login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Znajdź użytkownika na podstawie nazwy użytkownika
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == model.Username);

                if (user != null && VerifyPassword(user.UserPassword, model.Password))
                {
                    // Jeśli dane logowania są poprawne, logowanie użytkownika
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Przekierowanie do dashboardu
                    return RedirectToAction("Dashboard", "Users");
                }

                // Jeśli dane logowania są niepoprawne, wyświetl błąd
                ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło.");
            }

            // Zwracamy widok logowania, jeśli model jest niepoprawny
            return View(model);
        }

        // GET: Users/Dashboard
        public IActionResult Dashboard()
        {
            return View(); // Zwraca widok Dashboard.cshtml
        }

        // GET: Users/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
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

        // Funkcja weryfikująca hasło
        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            string inputHashedPassword = HashPassword(inputPassword); // Hashujemy hasło z formularza
            return hashedPassword == inputHashedPassword; // Porównujemy hashe
        }
    }
}
