namespace ProjektInzynierski.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int ClientID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

}
