using ProjektInzynierski.Domain.Entities.Equipments;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public interface IEquipmentRepository
    {
        public Task AddEquipment(Equipment equipment);
        public Task DeleteEquipment(Guid id);
        public Task<Equipment> GetEquipment(Guid id);
        public Task<List<Equipment>> GetEquipments();
        public Task<List<Equipment>> GetEquipments(IEnumerable<Guid> ids);
        public Task<List<Equipment>> GetCompatibleEquipments(Guid equipmentId);
        public Task<List<Equipment>> GetEquipmentsByCategory(EquipmentCategory equipmentCategory);
        public Task UpdateEquipment(Equipment equipment);
    }
}
