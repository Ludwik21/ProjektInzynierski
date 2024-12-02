namespace ProjektInzynierski.Models
{
    public class ReservationItem
    {
        public int Id { get; set; }
        public virtual Reservation Reservation { get; set; }
        public int ReservationId { get; set; }  
        public virtual Equipment Equipment { get; set; }
        public int EquipmentId { get; set; }    

    }
}
