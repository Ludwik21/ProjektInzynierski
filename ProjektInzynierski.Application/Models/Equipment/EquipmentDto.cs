using ProjektInzynierski.Domain.Common;
using ProjektInzynierski.Domain.Entities.Equipments;

namespace ProjektInzynierski.Application.Models.Equipment
{
    public class EquipmentDto
    {
        public Guid Id { get; set; }
        public EquipmentCategory Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public Currency PricePerDayCurrency { get; set; }
        public bool IsAvailable { get; set; }
    }
}
