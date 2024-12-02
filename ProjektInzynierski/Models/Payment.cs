namespace ProjektInzynierski.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; } = 0;
        public DateTime PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
