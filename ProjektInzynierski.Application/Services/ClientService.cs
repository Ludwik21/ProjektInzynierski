using ProjektInzynierski.Application.Models.Clients;
using ProjektInzynierski.Domain.Entities.Clients;
using ProjektInzynierski.Infrastructure.Repositories;

namespace ProjektInzynierski.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }


        public async Task AddClient(CreateClientDto createClientDto)
        {
            var client = new Client(createClientDto.Name, createClientDto.Address);
            await _repository.AddClient(client);
        }

        public Task DeleteClient(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClientDto> GetClient(Guid id)
        {
            var client = await _repository.GetClient(id);
            return new ClientDto()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                RegistrationDate = client.RegistrationDate
            };
        }

        public Task<List<ClientDto>> GetClients()
        {
            throw new NotImplementedException();
        }
    }
}
