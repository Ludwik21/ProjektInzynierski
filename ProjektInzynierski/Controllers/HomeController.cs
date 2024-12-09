using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    // Strona główna
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(); // Widok błędu
    }
}
