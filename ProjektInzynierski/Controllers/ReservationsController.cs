using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using ProjektInzynierski.Models.ProjektContext;
using System.Threading.Tasks;
using System.Linq;

namespace ProjektInzynierski.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ProjektContext _context;

        public ReservationsController(ProjektContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(int? clientId, int? equipmentId, string status)
        {
            // Pobierz listy klientów i sprzętów, które będą użyte w filtrach
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();

            var query = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Equipment)
                .AsQueryable(); // Używamy IQueryable do dynamicznego budowania zapytania

            // Dodanie filtrów, jeśli użytkownik je podał
            if (clientId.HasValue)
            {
                query = query.Where(r => r.ClientID == clientId);
            }

            if (equipmentId.HasValue)
            {
                query = query.Where(r => r.EquipmentID == equipmentId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status == status);
            }

            // Zwróć wynik w widoku
            var reservations = await query.ToListAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationID,ClientID,EquipmentID,StartDate,EndDate,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Sprawdzanie dostępności sprzętu
                bool isAvailable = await CheckEquipmentAvailability(reservation.EquipmentID, reservation.StartDate, reservation.EndDate);
                if (!isAvailable)
                {
                    ModelState.AddModelError("", "Sprzęt jest niedostępny w wybranym okresie.");
                    return View(reservation);
                }

                // Dodanie nowej rezerwacji do bazy
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                // Przekierowanie do strony z listą rezerwacji
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // Sprawdzanie dostępności sprzętu w zadanym okresie
        private async Task<bool> CheckEquipmentAvailability(int equipmentId, DateTime startDate, DateTime endDate)
        {
            return !await _context.Reservations
                .AnyAsync(r => r.EquipmentID == equipmentId &&
                               ((startDate >= r.StartDate && startDate < r.EndDate) ||
                                (endDate > r.StartDate && endDate <= r.EndDate) ||
                                (startDate <= r.StartDate && endDate >= r.EndDate)));
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,ClientID,EquipmentID,StartDate,EndDate,Status")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Sprawdzanie, czy rezerwacja istnieje
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }
    }
}
