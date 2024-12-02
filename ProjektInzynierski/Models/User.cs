namespace ProjektInzynierski.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;

        // Relacja do klienta
        public virtual Client? Client { get; set; } // Nawiazanie do obiektu Client
        public int? ClientId { get; set; }

        //Domyślna rola to klient
        public Role UserRole { get; set; } = Role.Client;
    }
}
