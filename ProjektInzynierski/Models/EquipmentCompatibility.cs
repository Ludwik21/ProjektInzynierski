namespace ProjektInzynierski.Models
{
    public class EquipmentCompatibility
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public virtual Equipment Equipment { get; set; }
        public int CompatibleEquipmentId { get; set; }
        public virtual Equipment CompatibleEquipment { get; set; }
    }
}
