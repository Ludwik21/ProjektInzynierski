namespace ProjektInzynierski.Application.Models.Reservation
{
    public class CreateReservationDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Guid> EquipmentIds { get; set; }
    }
}
