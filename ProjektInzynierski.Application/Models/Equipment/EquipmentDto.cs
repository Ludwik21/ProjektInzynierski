using ProjektInzynierski.Domain.Common;
using ProjektInzynierski.Domain.Entities.Equipments;
using ProjektInzynierski.Application.Models.Equipment;
using ProjektInzynierski.Domain.Entities.Users;

namespace ProjektInzynierski.Application.Models.Equipment
{
    public class EquipmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public int Quantity { get; set; }
        public Currency PricePerDayCurrency { get; set; } 
        public bool IsAvailable { get; set; }
        public List<EquipmentCompatibilityDto> Compatibilities { get; set; } = new List<EquipmentCompatibilityDto>();
        public EquipmentCategory Category { get; set; }

    }
}
