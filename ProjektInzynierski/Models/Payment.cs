namespace ProjektInzynierski.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int ReservationID { get; set; }
        public decimal Amount { get; set; } = 0;
        public DateTime PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
