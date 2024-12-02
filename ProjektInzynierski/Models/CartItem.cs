using System;

namespace ProjektInzynierski.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public int Quantity { get; set; }

        // Dodana właściwość TotalPrice
        public decimal TotalPrice => PricePerDay * Quantity;
    }
}
