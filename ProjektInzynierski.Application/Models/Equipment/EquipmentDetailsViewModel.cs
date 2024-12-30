using ProjektInzynierski.Application.Models.Equipment;

namespace ProjektInzynierski.Application.Models.Equipment
{
    public class EquipmentDetailsViewModel
    {
        public EquipmentDto Equipment { get; set; }
        public List<EquipmentDto> CompatibleEquipments { get; set; }
    }
}