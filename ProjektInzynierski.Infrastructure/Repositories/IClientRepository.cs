using ProjektInzynierski.Domain.Entities.Clients;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        public Task AddClient(Client client);
        public Task<Client> GetClient(Guid id);
    }
}
