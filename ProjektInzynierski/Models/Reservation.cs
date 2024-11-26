using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektInzynierski.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public string? UserEmail { get; set; }
        public decimal TotalAmount { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }
        public int EquipmentID { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        // Właściwość CartItems oznaczona jako [NotMapped] dla Entity Framework
        [NotMapped]
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
