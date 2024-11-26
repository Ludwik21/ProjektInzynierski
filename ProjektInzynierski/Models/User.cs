﻿namespace ProjektInzynierski.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public string Role { get; set; } // Rola użytkownika (np. "Admin", "Client")

        // Relacja z rezerwacjami
        public List<Reservation> Reservations { get; set; }

        // Relacja do klienta
        public int ClientID { get; set; }   // Klucz obcy do klienta
        public Client Client { get; set; }   // Nawiazanie do obiektu Client
    }
}
