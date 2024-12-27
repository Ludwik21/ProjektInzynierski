using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ProjektInzynierski.Infrastructure.Models;
using ProjektInzynierski.Application.Models.Users;
using ProjektInzynierski.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;

namespace ProjektInzynierski.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            _logger.LogInformation("Rozpoczęto proces logowania.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model logowania jest nieprawidłowy.");
                TempData["ErrorMessage"] = "Wypełnij wszystkie pola poprawnie.";
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == model.Username);
            if (user == null)
            {
                _logger.LogWarning("Nie znaleziono użytkownika z adresem email: {UserEmail}", model.Username);
                TempData["ErrorMessage"] = "Nie znaleziono użytkownika o podanym adresie email.";
                return View(model);
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.UserPassword, model.Password);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Nieprawidłowe hasło dla użytkownika: {UserEmail}", model.Username);
                TempData["ErrorMessage"] = "Nieprawidłowe hasło.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Zalogowano pomyślnie!";
            _logger.LogInformation("Hasło poprawne dla użytkownika: {UserEmail}.", model.Username);


            _logger.LogInformation("Hasło poprawne dla użytkownika: {UserEmail}.", model.Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            _logger.LogInformation("Użytkownik {UserEmail} został zalogowany.", model.Username);

            if (user.UserRole == Role.Admin)
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (user.UserRole == Role.Client)
            {
                return RedirectToAction("ClientDashboard");
            }
            else if (user.UserRole == Role.Employee)
            {
                return RedirectToAction("EmployeeDashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Formularz zawiera błędy. Proszę poprawić.";
                return View(model);
            }

            // Check if the user already exists
            if (await _context.Users.AnyAsync(u => u.UserEmail == model.UserEmail))
            {
                TempData["ErrorMessage"] = "Użytkownik z tym adresem e-mail już istnieje.";
                return View(model);
            }

            // Create a new user
            var newUser = new User
            {
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                UserPhone = model.Phone,
                UserRole = Role.Client, // Default role
                UserPassword = _passwordHasher.HashPassword(null, model.Password) // Hashing the password
            };

            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rejestracja zakończona sukcesem. Możesz się teraz zalogować!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas rejestracji użytkownika.");
                TempData["ErrorMessage"] = "Wystąpił błąd podczas rejestracji. Spróbuj ponownie później.";
                return View(model);
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Użytkownik został wylogowany.");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        public IActionResult ClientDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDashboard()
        {
            return View();
        }
    }
}
