namespace ProjektInzynierski.Infrastructure.Models
{
    public class ReservationItemDao
    {
        public virtual ReservationDao Reservation { get; set; }
        public Guid ReservationId { get; set; }  
        public virtual EquipmentDao Equipment { get; set; }
        public Guid EquipmentId { get; set; }    

    }
}
