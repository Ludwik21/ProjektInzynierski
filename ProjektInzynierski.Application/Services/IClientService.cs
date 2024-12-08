using ProjektInzynierski.Application.Models.Clients;

namespace ProjektInzynierski.Application.Services
{
    public interface IClientService
    {
        Task AddClient(CreateClientDto createClientDto);
        Task DeleteClient(Guid id);
        Task<ClientDto> GetClient(Guid id);
        Task<List<ClientDto>> GetClients();
    }
}
