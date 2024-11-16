using System.Collections.Generic;

namespace ProjektInzynierski.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Reservation> Reservations { get; set; } // Lista rezerwacji
    }
}
