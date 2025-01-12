using Microsoft.AspNetCore.Mvc;

public class ContactController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SendMessage(string name, string email, string subject, string message)
    {
        // Obsługa wysyłania wiadomości (np. zapis do bazy lub wysłanie e-maila)
        TempData["Message"] = "Twoja wiadomość została wysłana! Skontaktujemy się z Tobą wkrótce.";
        return RedirectToAction("Index");
    }
}
