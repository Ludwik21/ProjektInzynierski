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
        private readonly IEquipmentCompatibilityService _compatibilityService;

        public EquipmentsController(IEquipmentService equipmentService, IEquipmentCompatibilityService compatibilityService)
        {
            _equipmentService = equipmentService;
            _compatibilityService = compatibilityService;
        }

        //INDEX
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

            // Pobierz kompatybilności sprzętu
            var compatibilities = await _compatibilityService.GetCompatibilities(id);
            var compatibilityDtos = compatibilities.Select(c => new EquipmentCompatibilityDto
            {
                Id = c.Id,
                EquipmentId = c.EquipmentId,
                CompatibleEquipmentId = c.CompatibleEquipmentId,
                CompatibleEquipmentName = c.CompatibleEquipment?.Name ?? "Nieznany sprzęt"
            }).ToList();

            // Upewnij się, że Compatibilities jest zainicjalizowane
            equipment.Compatibilities = compatibilityDtos;

            return View(equipment);
        }


        // GET: Equipments/Create
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            var allEquipments = _equipmentService.GetEquipments().Result;
            ViewBag.AllEquipments = allEquipments;
            return View();
        }


        // POST: Equipments/Create
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEquipmentDto equipmentDto)
        {
            if (ModelState.IsValid)
            {
                // Generowanie nowego Id
                var newEquipmentId = Guid.NewGuid();

                // Tworzenie sprzętu
                await _equipmentService.AddEquipment(newEquipmentId, equipmentDto);

                // Dodanie kompatybilnych sprzętów
                foreach (var compatibleId in equipmentDto.CompatibleEquipmentIds)
                {
                    await _compatibilityService.AddCompatibility(newEquipmentId, compatibleId);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(equipmentDto);
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

            var allEquipments = await _equipmentService.GetEquipments();
            var compatibilities = await _compatibilityService.GetCompatibilities(id);

            ViewBag.AllEquipments = allEquipments;
            ViewBag.Compatibilities = compatibilities;

            return View(equipment);
        }


        // POST: Equipments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EquipmentDto equipment, List<Guid> CompatibleEquipmentIds)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _equipmentService.EditEquipment(equipment);

                // Usuń istniejące kompatybilności i dodaj nowe
                await _compatibilityService.ClearCompatibilities(equipment.Id);
                foreach (var compatibleId in CompatibleEquipmentIds)
                {
                    await _compatibilityService.AddCompatibility(equipment.Id, compatibleId);
                }

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
        [HttpPost]
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

        [HttpPost("{equipmentId}/add-compatibility")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> AddCompatibility(Guid equipmentId, Guid compatibleEquipmentId)
        {
            try
            {
                await _compatibilityService.AddCompatibility(equipmentId, compatibleEquipmentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove-compatibility/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> RemoveCompatibility(Guid id)
        {
            try
            {
                await _compatibilityService.RemoveCompatibility(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{equipmentId}/compatibilities")]
        [Authorize(Roles = "Admin, Employee, Client")]
        public async Task<IActionResult> GetCompatibilities(Guid equipmentId)
        {
            try
            {
                var compatibilities = await _compatibilityService.GetCompatibilities(equipmentId);
                return Ok(compatibilities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
