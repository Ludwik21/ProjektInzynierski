namespace ProjektInzynierski.Infrastructure.Models
{
    public class ClientDao
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }  // Pole Address
        public DateTime RegistrationDate { get; set; }  // Pole RegistrationDate

        // Relacja: jeden klient -> jeden użytkownik
        public virtual ICollection<User> Users { get; set; }  // Dodane powiązanie z User

        // Relacja: jeden klient -> wiele rezerwacji
        public virtual ICollection<ReservationDao> Reservations { get; set; }
    }
}
