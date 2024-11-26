using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using System.Collections.Generic;
using ProjektInzynierski.Models.ProjektContext;
using System.Threading.Tasks;

namespace ProjektInzynierski.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ProjektContext _context;
        public ClientsController(ProjektContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            return client == null ? NotFound() : View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients.FindAsync(id);
            return client == null ? NotFound() : View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _context.Clients
                .Include(c => c.Reservations) // assuming Reservations are related to Clients
                .FirstOrDefaultAsync(m => m.ClientID == id);

            return client == null ? NotFound() : View(client);
        }
        public IActionResult SelectCategory()
        {
            var categories = new List<string> { "Camera", "Light", "Accessory" };
            return View(categories); // To musi wskazywać na Views/Clients/SelectCategory.cshtml
        }

        [HttpGet]
        public IActionResult SelectEquipment(string category)
        {
            // Pobranie sprzętu dla danej kategorii
            var equipmentList = GetEquipmentByCategory(category);
            return View("~/Views/Equipments/SelectEquipment.cshtml", equipmentList);

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
                new Equipment { EquipmentID = 1, Name = "Camera Sony", Category = category },
                new Equipment { EquipmentID = 2, Name = "Camera Canon", Category = category }
            };
        }

        private void SendReservationRequest(int equipmentId)
        {
            // Wysłanie zapytania do admina o zatwierdzenie rezerwacji
        }
    }
}
