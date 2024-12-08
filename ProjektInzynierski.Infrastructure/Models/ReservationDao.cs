using ProjektInzynierski.Domain.Entities.Reservations;

namespace ProjektInzynierski.Infrastructure.Models
{
    public class ReservationDao
    {
        public Guid Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public Guid RequestingUserId { get; set; }
        public Guid ClientId { get; set; }
        public virtual ClientDao Client { get; set; }
        public List<ReservationItemDao>  Items { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
