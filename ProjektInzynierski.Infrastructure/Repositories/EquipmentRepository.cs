using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Domain.Entities.Equipments;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ProjektContext _context;

        public EquipmentRepository(ProjektContext context)
        {
            _context = context;
        }
        public async Task AddCompatibility(EquipmentCompatibility compatibility)
        {
            await _context.EquipmentCompatibility.AddAsync(compatibility);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCompatibility(Guid compatibilityId)
        {
            var compatibility = await _context.EquipmentCompatibility.FindAsync(compatibilityId);
            if (compatibility != null)
            {
                _context.EquipmentCompatibility.Remove(compatibility);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EquipmentCompatibility>> GetCompatibilities(Guid equipmentId)
        {
            return await _context.EquipmentCompatibility
                .Include(c => c.CompatibleEquipment) // Ładowanie powiązanego sprzętu
                .Where(c => c.EquipmentId == equipmentId)
                .ToListAsync();
        }




        public async Task AddEquipment(Equipment equipment)
        {
            var equipmentDao = new EquipmentDao()
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Category = equipment.Category,
                Brand = equipment.Brand,
                Description = equipment.Description,
                PricePerDay = equipment.PricePerDay,
                PricePerDayCurrency = equipment.PricePerDayCurrency,
                Quantity = equipment.Quantity,
                IsAvailable = equipment.IsAvailable,
                ReservationItems = new List<ReservationItemDao>(),
                CompatibleEquipments = new List<EquipmentCompatibility>(),
            };

            await _context.Equipment.AddAsync(equipmentDao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquipment(Guid id)
        {
            // Usuń powiązania z EquipmentCompatibility
            var compatibilities = await _context.EquipmentCompatibility
                .Where(c => c.CompatibleEquipmentId == id || c.EquipmentId == id)
                .ToListAsync();

            _context.EquipmentCompatibility.RemoveRange(compatibilities);

            // Usuń sprzęt
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }

            await _context.SaveChangesAsync();
        }


        public async Task<List<Equipment>> GetCompatibleEquipments(Guid equipmentId)
        {
            var firstCompatibilityCheckResults = await _context.EquipmentCompatibility
                .Where(x => x.EquipmentId == equipmentId)
                .Select(x => x.CompatibleEquipmentId)
                .ToListAsync();

            var secondCompatibilityCheckResults = await _context.EquipmentCompatibility
                .Where(x => x.CompatibleEquipmentId == equipmentId)
                .Select(x => x.EquipmentId)
                .ToListAsync();

            return await GetEquipments(firstCompatibilityCheckResults.Concat(secondCompatibilityCheckResults));
        }

        public async Task<Equipment> GetEquipment(Guid id)
        {
            var equipmentDao = await _context.Equipment.FirstOrDefaultAsync(e => e.Id == id);
            
            if(equipmentDao is null)
            {
                throw new InvalidOperationException("Equipment was not found");
            }

            return new Equipment(equipmentDao.Id,
                equipmentDao.Category,
                equipmentDao.Name, 
                equipmentDao.Brand,
                equipmentDao.Description,
                equipmentDao.PricePerDay, 
                equipmentDao.PricePerDayCurrency,
                equipmentDao.Quantity,
                equipmentDao.IsAvailable);
        }

        public async Task<List<Equipment>> GetEquipments()
        {
            return await _context.Equipment
                .Select(e => new Equipment(e.Id,
                    e.Category,
                    e.Name,
                    e.Brand,
                    e.Description,
                    e.PricePerDay,
                    e.PricePerDayCurrency,
                    e.Quantity,
                    e.IsAvailable))
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetEquipments(IEnumerable<Guid> ids)
        {
            return await _context.Equipment
                .Where(eq => ids.Contains(eq.Id))
                .Select(e => new Equipment(e.Id,
                    e.Category,
                    e.Name,
                    e.Brand,
                    e.Description,
                    e.PricePerDay,
                    e.PricePerDayCurrency,
                    e.Quantity,
                    e.IsAvailable))
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetEquipmentsByCategory(EquipmentCategory equipmentCategory)
        {
            return await _context.Equipment
                .Where(x => x.Category == equipmentCategory)
                .Select(e => new Equipment(e.Id,
                    e.Category,
                    e.Name,
                    e.Brand,
                    e.Description,
                    e.PricePerDay,
                    e.PricePerDayCurrency,
                    e.Quantity,
                    e.IsAvailable))
                .ToListAsync();
        }

        public async Task UpdateEquipment(Equipment equipment)
        {
            _context.Update(equipment);
            await _context.SaveChangesAsync();
        }
    }
}
