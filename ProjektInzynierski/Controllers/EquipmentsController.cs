using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektInzynierski.Application.Models.Equipment;
using ProjektInzynierski.Application.Services;
using ProjektInzynierski.Domain.Entities.Equipments;


namespace ProjektInzynierski.Controllers
{
    [Authorize]
    public class EquipmentsController : Controller
    {
        private readonly IEquipmentService _equipmentService;


        public EquipmentsController(IEquipmentService equipmentService)
        {

            _equipmentService = equipmentService;
        }


        // GET: Equipments
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                var equipments = await _equipmentService.GetEquipments();
                return View(equipments);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message); // Strona błędu w razie problemów
            }
        }


        // GET: Equipments/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Nieprawidłowy identyfikator.");
            }

            var equipment = await _equipmentService.GetEquipment(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }


        // GET: Equipments/Create
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Equipments/Create

        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEquipmentDto equipment)
        {
            if (ModelState.IsValid)
            {
                await _equipmentService.AddEquipment(equipment);
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }


        // GET: Equipments/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var equipment = await _equipmentService.GetEquipment(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }


        // POST: Equipments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EquipmentDto equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                await _equipmentService.EditEquipment(equipment);
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }


        // GET: Equipments/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Nieprawidłowy identyfikator.");
            }

            var equipment = await _equipmentService.GetEquipment(id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment); // Widok potwierdzenia
        }



        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _equipmentService.DeleteEquipment(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Employee, Client")]
        public async Task<IActionResult> SelectEquipment(EquipmentCategory category)
        {
            var equipmentList = await _equipmentService.GetEquipmentsByCategory(category);
            ViewData["Category"] = category;


            return View(equipmentList);
        }



        [Authorize(Roles = "Admin, Employee, Client")]
        public async Task<IActionResult> Reserve(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Nieprawidłowy identyfikator.");
            }

            var equipment = await _equipmentService.GetEquipment(id);
            if (equipment == null || !equipment.IsAvailable)
            {
                return NotFound("Sprzęt jest niedostępny.");
            }

            await _equipmentService.ReserveEquipment(id);
            return RedirectToAction(nameof(ReservationConfirmed), new { id });
        }

        [Authorize(Roles = "Admin, Employee, Client")]
        public IActionResult ReservationConfirmed(Guid id)
        {
            ViewData["EquipmentId"] = id;
            return View();
        }
    }
}
