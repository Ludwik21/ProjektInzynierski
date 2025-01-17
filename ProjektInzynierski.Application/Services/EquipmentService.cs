﻿using ProjektInzynierski.Application.Models.Equipment;
using ProjektInzynierski.Domain.Entities.Equipments;
using ProjektInzynierski.Infrastructure.Repositories;
using ProjektInzynierski.Application.Models.Equipment;

namespace ProjektInzynierski.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _repository;

        public EquipmentService(IEquipmentRepository repository)
        {
            _repository = repository;
        }

        public async Task AddEquipment(Guid id, CreateEquipmentDto equipmentDto)
        {
            var equipment = new Equipment(
                id,
                equipmentDto.Category,
                equipmentDto.Name,
                equipmentDto.Brand,
                equipmentDto.Description,
                equipmentDto.PricePerDay,
                equipmentDto.PricePerDayCurrency, // Przekazywanie Currency bezpośrednio
                equipmentDto.Quantity,
                true // Domyślna dostępność
            );

            await _repository.AddEquipment(equipment);
        }


        public async Task DeleteEquipment(Guid id)
        {
            await _repository.DeleteEquipment(id);
        }

        public async Task EditEquipment(EquipmentDto equipmentDto)
        {
            var equipment = new Equipment(equipmentDto.Id,
                equipmentDto.Category,
                equipmentDto.Name,
                equipmentDto.Brand,
                equipmentDto.Description,
                equipmentDto.PricePerDay,
                equipmentDto.PricePerDayCurrency,
                equipmentDto.Quantity,
                equipmentDto.IsAvailable);

            await _repository.UpdateEquipment(equipment);
        }

        public async Task<EquipmentDto> GetEquipment(Guid id)
        {
            var equipment = await _repository.GetEquipment(id);
            return GetViewModelFromDomain(equipment);
        }

        public async Task<List<EquipmentDto>> GetEquipments()
        {
            var equipments = await _repository.GetEquipments();
            return equipments.Select(x => GetViewModelFromDomain(x)).ToList();
        }

        public async Task<List<EquipmentDto>> GetEquipmentsByCategory(EquipmentCategory equipmentCategory)
        {
            var equipments = await _repository.GetEquipmentsByCategory(equipmentCategory);
            return equipments.Select(x => GetViewModelFromDomain(x)).ToList();
        }

        public async Task ReserveEquipment(Guid id)
        {
            var equipment = await _repository.GetEquipment(id);
            equipment.Reserve();
            await _repository.UpdateEquipment(equipment);
        }

        private EquipmentDto GetViewModelFromDomain(Equipment equipment)
        {
            return new EquipmentDto()
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Category = equipment.Category,
                Description = equipment.Description,
                Brand = equipment.Brand,
                IsAvailable = equipment.IsAvailable,
                PricePerDay = equipment.PricePerDay,
                PricePerDayCurrency = equipment.PricePerDayCurrency,
                Quantity = equipment.Quantity
            };
        }
    }
}
