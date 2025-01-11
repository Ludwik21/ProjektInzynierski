using ProjektInzynierski.Domain.Common;
using ProjektInzynierski.Domain.Entities.Equipments;

namespace ProjektInzynierski.Infrastructure.Models
{
    public class EquipmentDao
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EquipmentCategory Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public Currency PricePerDayCurrency { get; set; }
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }
        public virtual List<ReservationItemDao> ReservationItems { get; set; }

        // Kolekcje dla relacji
        public virtual List<EquipmentCompatibility> CompatibleEquipments { get; set; } // Sprzęt, który jest kompatybilny
        public virtual List<EquipmentCompatibility> CompatibleAsEquipment { get; set; } // Sprzęt, dla którego ten jest kompatybilny
    }

}
