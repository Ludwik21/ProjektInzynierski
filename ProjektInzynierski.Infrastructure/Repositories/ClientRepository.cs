using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Domain.Entities.Clients;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ProjektContext _context;

        public ClientRepository(ProjektContext context)
        {
            _context = context;
        }

        public async Task AddClient(Client client)
        {
            var clientDao = new ClientDao()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                RegistrationDate = client.RegistrationDate
            };

            await _context.Clients.AddAsync(clientDao);
            await _context.SaveChangesAsync();
        }

        public async Task<Client> GetClient(Guid id)
        {
            var clientDao = await _context.Clients.SingleAsync(x => x.Id == id);
            return new Client(clientDao.Id, clientDao.Name, clientDao.Address, clientDao.RegistrationDate);
        }
    }
}
