namespace ProjektInzynierski.Domain.Entities.Reservations
{
    public enum ReservationStatus
    {
        Issued = 1,
        PaymentCompleted = 2,
        Completed = 3,
        Cancelled = 4,
        Rejected = 5,
    }
}
