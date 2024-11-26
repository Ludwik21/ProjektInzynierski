﻿namespace ProjektInzynierski.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }

        // Zmiana typu na nullable bool (bool?)
       public bool? AvailabilityStatus { get; set; }

    }

}
