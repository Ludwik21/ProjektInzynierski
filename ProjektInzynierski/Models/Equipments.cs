namespace ProjektInzynierski.Models
{
    public class Equipments
    {
        public int EquipmentID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public bool AvailabilityStatus { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
