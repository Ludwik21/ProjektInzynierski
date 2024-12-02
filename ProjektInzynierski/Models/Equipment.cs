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

    }

}
