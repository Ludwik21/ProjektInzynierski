namespace ProjektInzynierski.Application.Models.Clients
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
