using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Models;
using System.Collections.Generic;

namespace ProjektInzynierski.Controllers
{
    public class ClientsController : Controller
    {
        public IActionResult SelectCategory()
        {
            var categories = new List<string> { "Kamery", "Oświetlenie", "Mikrofony" };
            return View(categories); // To musi wskazywać na Views/Clients/SelectCategory.cshtml
        }

        [HttpGet]
        public IActionResult SelectEquipment(string category)
        {
            // Pobranie sprzętu dla danej kategorii
            var equipmentList = GetEquipmentByCategory(category);
            return View(equipmentList); // To musi wskazywać na Views/Clients/SelectEquipment.cshtml
        }

        [HttpPost]
        public IActionResult Reserve(int equipmentId)
        {
            // Logika dodania rezerwacji dla sprzętu
            SendReservationRequest(equipmentId);
            return RedirectToAction("ReservationPending");
        }

        public IActionResult ReservationPending()
        {
            return View(); // Widok oczekiwania na zatwierdzenie rezerwacji
        }

        private List<Equipment> GetEquipmentByCategory(string category)
        {
            return new List<Equipment>
            {
                new Equipment { EquipmentID = 1, Name = "Kamera Sony", Category = category },
                new Equipment { EquipmentID = 2, Name = "Kamera Canon", Category = category }
            };
        }

        private void SendReservationRequest(int equipmentId)
        {
            // Wysłanie zapytania do admina o zatwierdzenie rezerwacji
        }
    }
}
