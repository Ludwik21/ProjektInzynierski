namespace ProjektInzynierski.Infrastructure.Models
{
    public class EquipmentCompatibility
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public virtual EquipmentDao Equipment { get; set; }
        public Guid CompatibleEquipmentId { get; set; }
        public virtual EquipmentDao CompatibleEquipment { get; set; }
    }
}
