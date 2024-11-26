using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektInzynierski.Models
{
    [NotMapped]
    public class CartItem
    {
        public int EquipmentID { get; set; } // Powiązanie ze sprzętem
        public string Name { get; set; } // Nazwa sprzętu
        public decimal PricePerDay { get; set; } // Cena za dzień wynajmu
        public int Quantity { get; set; } = 1; // Ilość dni wynajmu
    }
}
