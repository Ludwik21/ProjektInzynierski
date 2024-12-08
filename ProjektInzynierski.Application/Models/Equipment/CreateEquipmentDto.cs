using ProjektInzynierski.Domain.Common;
using ProjektInzynierski.Domain.Entities.Equipments;

namespace ProjektInzynierski.Application.Models.Equipment
{
    public class CreateEquipmentDto
    {
        public EquipmentCategory Category { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public Currency PricePerDayCurrency { get; set; }
    }
}
