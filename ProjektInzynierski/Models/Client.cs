using System;
using System.Collections.Generic;

namespace ProjektInzynierski.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Address { get; set; }  // Pole Address
        public DateTime RegistrationDate { get; set; }  // Pole RegistrationDate

        // Relacja: jeden klient -> jeden użytkownik
        public virtual ICollection<User> Users { get; set; }  // Dodane powiązanie z User

        // Relacja: jeden klient -> wiele rezerwacji
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
