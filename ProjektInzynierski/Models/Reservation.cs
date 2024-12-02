using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektInzynierski.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public int RequestingUserId { get; set; }
        public decimal TotalAmount { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public List<ReservationItem>  Items { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; }
    }
}
