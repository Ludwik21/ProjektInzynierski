using System;
using System.Collections.Generic;

namespace ProjektInzynierski.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        // Nowe właściwości
        public string Address { get; set; }  // Dodane pole Adress
        public DateTime RegistrationDate { get; set; }  // Dodane pole RegistrationDate

        // Relacja: jeden klient może mieć wiele rezerwacji
        public ICollection<Reservation> Reservations { get; set; }
    }
}
