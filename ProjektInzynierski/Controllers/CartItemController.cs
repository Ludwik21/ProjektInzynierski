using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using ProjektInzynierski.Extensions;
using System.Linq;
using ProjektInzynierski.Models.ProjektContext;

namespace ProjektInzynierski.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ProjektContext _context;

        public CartItemController(ProjektContext context)
        {
            _context = context;
        }

        // Wyświetlenie koszyka
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // Dodanie do koszyka
        [HttpPost]
        public IActionResult AddToCart(int equipmentId)
        {
            // Sprawdzamy, czy sprzęt istnieje
            var equipment = _context.Equipments.FirstOrDefault(e => e.EquipmentID == equipmentId);
            if (equipment == null)
            {
                // Jeśli sprzęt nie istnieje, zwracamy błąd
                TempData["Error"] = "Wybrany sprzęt nie istnieje.";
                return RedirectToAction("Index");
            }

            // Pobieramy koszyk z sesji
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Sprawdzamy, czy sprzęt jest już w koszyku
            var existingItem = cart.FirstOrDefault(c => c.EquipmentID == equipmentId);

            if (existingItem == null)
            {
                // Jeśli nie ma sprzętu w koszyku, dodajemy nowy element
                cart.Add(new CartItem
                {
                    EquipmentID = equipment.EquipmentID,
                    Name = equipment.Name,
                    PricePerDay = equipment.PricePerDay,
                    Quantity = 1
                });
            }
            else
            {
                // Jeśli sprzęt już jest, zwiększamy ilość
                existingItem.Quantity++;
            }

            // Aktualizujemy sesję
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Informacja dla użytkownika
            TempData["Message"] = "Sprzęt został dodany do koszyka.";
            return RedirectToAction("Index");
        }

        // Usuwanie z koszyka
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToRemove = cart.FirstOrDefault(c => c.EquipmentID == id);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                TempData["Message"] = "Sprzęt został usunięty z koszyka.";
            }
            else
            {
                TempData["Error"] = "Sprzęt nie został znaleziony w koszyku.";
            }

            return RedirectToAction("Index");
        }

        // Finalizacja rezerwacji
        [HttpPost]
        public IActionResult CreateReservation()
        {
            // Pobieramy koszyk z sesji
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (!cart.Any())
            {
                // Jeśli koszyk jest pusty, przekierowujemy z powrotem
                TempData["Error"] = "Koszyk jest pusty. Nie można utworzyć rezerwacji.";
                return RedirectToAction("Index");
            }

            try
            {
                // Tworzymy nową rezerwację
                var reservation = new Reservation
                {
                    ClientID = 1, // ID zalogowanego użytkownika (na razie statycznie)
                    ReservationDate = DateTime.Now,
                    TotalAmount = cart.Sum(c => c.PricePerDay * c.Quantity),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7), // Przykładowy czas wynajmu
                    Status = "Pending",
                    EquipmentID = cart.First().EquipmentID // Przykładowe przypisanie pierwszego elementu
                };

                // Zapisujemy rezerwację w bazie
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                // Czyścimy koszyk
                HttpContext.Session.SetObjectAsJson("Cart", new List<CartItem>());

                TempData["Message"] = "Rezerwacja została pomyślnie utworzona.";
                return RedirectToAction("Index", "Reservation");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Wystąpił błąd podczas tworzenia rezerwacji: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
