using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ProjektInzynierski.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjektContext _context;

        public HomeController(ProjektContext context)
        {
            _context = context;
        }

        // Strona główna - domyślnie formularz logowania
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Users");
        }

        public IActionResult Error()
        {
            return View(); // Widok błędu
        }
    }
}
