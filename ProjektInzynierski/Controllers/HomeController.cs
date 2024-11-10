using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProjektInzynierski.Models.ProjektContext;
using System.Security.Claims;

namespace ProjektInzynierski.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjektContext _context;

        public HomeController(ProjektContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new Home
            {
                TotalEquipments = await _context.Equipment.CountAsync(),
                TotalReservations = await _context.Reservations.CountAsync(),
                TotalPayments = await _context.Payments.CountAsync(),
                TotalUsers = await _context.Users.CountAsync()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult SelectRole(string role)
        {
            if (role == "users")
            {
                return RedirectToAction("Login", "Users"); // Przekierowanie do logowania użytkownika
            }
            else if (role == "clients")
            {
                return RedirectToAction("SelectCategory", "Clients"); // Przekierowanie do wyboru kategorii
            }
            return RedirectToAction("Index");
        }

    }
}
