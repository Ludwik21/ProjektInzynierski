using System.ComponentModel.DataAnnotations;

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
    }

    public class Equipment
    {
        public int EquipmentID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public bool AvailabilityStatus { get; set; }
        public decimal PricePerDay { get; set; }

    }

    public class Reservation
    {
        public int ReservationID { get; set; }
        public int ClientID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

    public class Payment
    {
        public int PaymentID { get; set; }
        public int ReservationID { get; set; }
        public decimal Amount { get; set; } = 0;
        public DateTime PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }
        public string Role { get; set; }
    }
}
