namespace ProjektInzynierski.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } //enum
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; } // enum zw waluta

        // Zmiana typu na nullable bool (bool?)
       public bool? AvailabilityStatus { get; set; }

        public virtual List<ReservationItem> ReservationItems { get; set; }

        //Lista sprzętów, które są kompatybilne z tym sprzętem
        public virtual List<EquipmentCompatibility> CompatibleEquipments { get; set; }

        // Lista sprzętó, które mają ten sprzęt jako kompatybilny
        public virtual List<EquipmentCompatibility> IsCompatibleWith { get; set; }

    }

}
