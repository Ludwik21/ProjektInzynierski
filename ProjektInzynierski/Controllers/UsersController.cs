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

namespace ProjektInzynierski.Controllers
{
    [Authorize] // Wymagana autoryzacja do wszystkich akcji w tym kontrolerze
    public class UsersController : Controller
    {
        private readonly ProjektContext _context;
        private readonly string _defaultUsername = "admin";
        private readonly string _defaultPassword = "admin";

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
                    // Logowanie użytkownika
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role) // Przykładowa rola
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Przekierowanie do dashboardu
                    return RedirectToAction("Dashboard", "Users");
                }

                // Nieprawidłowe dane logowania
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserName,UserEmail,UserPhone,UserPassword,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                // Haszowanie hasła przed zapisaniem w bazie danych
                user.UserPassword = HashPassword(user.UserPassword);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,UserEmail,UserPhone,UserPassword,Role")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(user.UserPassword))
                    {
                        user.UserPassword = HashPassword(user.UserPassword);
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
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
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(inputPassword);
                var hash = sha256.ComputeHash(bytes);
                var inputHashedPassword = Convert.ToBase64String(hash);
                return hashedPassword == inputHashedPassword;
            }
        }
    }
}
