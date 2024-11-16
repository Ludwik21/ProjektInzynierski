namespace ProjektInzynierski.Models
{
    public class CartItem
    {
        public int EquipmentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => PricePerDay * Quantity;
    }
}
