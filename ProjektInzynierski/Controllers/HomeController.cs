using Microsoft.AspNetCore.Mvc;

namespace ProjektInzynierski.Controllers
{
    public class HomeController : Controller
    {
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
