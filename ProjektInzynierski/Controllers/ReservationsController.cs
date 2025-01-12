using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Application.Services;
using ProjektInzynierski.Domain.Entities.Reservations;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ProjektContext _context;

        public ReservationsController(IReservationService reservationService, ProjektContext context)
        {
            _reservationService = reservationService;
            _context = context; // Wstrzyknięcie DbContext
        }

        // GET: Reservations
        public async Task<IActionResult> Index(Guid? clientId, Guid? equipmentId, string status)
        {
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();

            var query = _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Items)
                .ThenInclude(i => i.Equipment)
                .AsQueryable(); 

            // Dodanie filtrów, jeśli użytkownik je podał
            if (clientId.HasValue)
            {
                query = query.Where(r => r.ClientId == clientId);
            }

            if (equipmentId.HasValue)
            {
                query = query.Where(r => r.Items.Any(x => x.EquipmentId == equipmentId.Value));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status.ToString() == status);
            }

            var reservations = await query.ToListAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return BadRequest("Identyfikator rezerwacji jest wymagany.");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Items)
                .ThenInclude(i => i.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Nie znaleziono rezerwacji.");
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationDao reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.Id = Guid.NewGuid();
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();
            return View(reservation);
        }

        // Sprawdzanie dostępności sprzętu w zadanym okresie
        private async Task<bool> CheckEquipmentAvailability(int equipmentId, DateTime startDate, DateTime endDate)
        {
            return true;
            //return !await _context.Reservations
            //    .AnyAsync(r => r.EquipmentID == equipmentId &&
            //                   ((startDate >= r.StartDate && startDate < r.EndDate) ||
            //                    (endDate > r.StartDate && endDate <= r.EndDate) ||
            //                    (startDate <= r.StartDate && endDate >= r.EndDate)));
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return BadRequest("Identyfikator rezerwacji jest wymagany.");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Nie znaleziono rezerwacji.");
            }

            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ReservationDao reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest("Identyfikatory nie pasują.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound("Nie znaleziono rezerwacji.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Equipments = await _context.Equipment.ToListAsync();
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound("Nie znaleziono rezerwacji.");
            }

            reservation.Status = ReservationStatus.Completed; 
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rezerwacja została potwierdzona.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound("Nie znaleziono rezerwacji.");
            }

            reservation.Status = ReservationStatus.Rejected;
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            TempData["ErrorMessage"] = "Rezerwacja została odrzucona.";
            return RedirectToAction(nameof(Index));
        }


        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return BadRequest("Identyfikator rezerwacji jest wymagany.");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Nie znaleziono rezerwacji.");
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(Guid id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
