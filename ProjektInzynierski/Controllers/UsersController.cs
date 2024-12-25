using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ProjektInzynierski.Infrastructure.Models;
using ProjektInzynierski.Application.Models.Users;

namespace ProjektInzynierski.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProjektContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ProjektContext context, IPasswordHasher<User> passwordHasher, ILogger<UsersController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            _logger.LogInformation("Rozpoczęto proces logowania.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Nieprawidłowy model logowania. Pola są puste.");
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == model.Username);
            if (user == null)
            {
                _logger.LogWarning("Nie znaleziono użytkownika z adresem email: {Username}", model.Username);
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            _logger.LogInformation("Użytkownik znaleziony w bazie danych: {Username}", user.UserName);

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.UserPassword, model.Password);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Nieprawidłowe hasło dla użytkownika: {Username}", user.UserName);
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            _logger.LogInformation("Hasło poprawne. Logowanie użytkownika: {Username}", user.UserName);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.UserRole.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            _logger.LogInformation("Użytkownik {Username} został zalogowany pomyślnie.", user.UserName);

            return RedirectToAction("Dashboard", "Users");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Formularz zawiera błędy. Proszę poprawić.";
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.UserEmail == model.UserEmail))
            {
                TempData["ErrorMessage"] = "Użytkownik z tym adresem e-mail już istnieje.";
                return View(model);
            }

            var newUser = new User
            {
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                UserPhone = model.Phone,
                UserPassword = _passwordHasher.HashPassword(null, model.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rejestracja zakończona sukcesem. Możesz się teraz zalogować!";
            return RedirectToAction("Login");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
