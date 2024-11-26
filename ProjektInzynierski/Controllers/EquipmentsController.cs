using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Models;
using ProjektInzynierski.Models.ProjektContext;
using System.Threading.Tasks;

namespace ProjektInzynierski.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly ProjektContext _context;

        public EquipmentsController(ProjektContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index()
        {
            var equipments = await _context.Equipment.ToListAsync();
            return View(equipments);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EquipmentID,Category,Name,Brand,Description,AvailabilityStatus,PricePerDay")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EquipmentID,Category,Name,Brand,Description,AvailabilityStatus,PricePerDay")] Equipment equipment)
        {
            if (id != equipment.EquipmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.EquipmentID))
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
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SelectEquipment(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return NotFound("Category not specified.");
            }
            var equipmentList = await _context.Equipment
                .Where(e => e.Category == category)
                .ToListAsync();
            ViewData["Category"] = category;
            return View(equipmentList);
        }

        public async Task<IActionResult> Reserve(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null || !equipment.AvailabilityStatus)
            {
                return NotFound("Sprzęt niedostępny do rezerwacji.");
            }

            // Oznacz jako zarezerwowany
            equipment.AvailabilityStatus = false;
            await _context.SaveChangesAsync();

            // Przekierowanie do strony potwierdzenia rezerwacji
            return RedirectToAction("ReservationConfirmed", new { id });
        }

        public IActionResult ReservationConfirmed(int id)
        {
            ViewData["EquipmentId"] = id;
            return View();
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.EquipmentID == id);
        }
    }
}
