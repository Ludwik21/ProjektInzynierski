using ProjektInzynierski.Application.Models.Equipment;
using ProjektInzynierski.Domain.Entities.Equipments;

namespace ProjektInzynierski.Application.Services
{
    public interface IEquipmentService
    {
        public Task AddEquipment(Guid id, CreateEquipmentDto equipmentDto);
        public Task DeleteEquipment(Guid id);
        public Task EditEquipment(EquipmentDto equipmentDto);
        public Task<EquipmentDto> GetEquipment(Guid id);
        public Task<List<EquipmentDto>> GetEquipments();
        public Task<List<EquipmentDto>> GetEquipmentsByCategory(EquipmentCategory equipmentCategory);
        public Task ReserveEquipment(Guid id);
    }
}
