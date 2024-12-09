﻿using Microsoft.EntityFrameworkCore;
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
                IsAvailable = equipment.IsAvailable,
                ReservationItems = new List<ReservationItemDao>(),
                CompatibleEquipments = new List<EquipmentCompatibility>(),
            };

            await _context.Equipment.AddAsync(equipmentDao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquipment(Guid id)
        {
            var equipmentToRemove = _context.Equipment.FirstOrDefault(e => e.Id == id);
            if (equipmentToRemove is null)
            {
                throw new InvalidOperationException("Equipment was not found");
            }
            _context.Remove(equipmentToRemove);
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