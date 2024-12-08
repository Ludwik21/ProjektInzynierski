namespace ProjektInzynierski.Domain.Entities.Clients
{
    public class Client
    {
        public Client(string name,
            string address)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            RegistrationDate = DateTime.UtcNow;
        }

        public Client(Guid id,
            string name,
            string address,
            DateTime registrationDate)
        {
            Id = id;
            Name = name;
            Address = address;
            RegistrationDate = registrationDate;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Address { get; }
        public DateTime RegistrationDate { get; }
    }
}
