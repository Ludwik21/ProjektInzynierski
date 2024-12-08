using FluentAssertions;
using Moq;
using ProjektInzynierski.Application.Services;
using ProjektInzynierski.Domain.Entities.Clients;
using ProjektInzynierski.Infrastructure.Repositories;

namespace ProjektInzynierski.Tests.Application
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly ClientService _service;
        public ClientServiceTests() 
        {
            _service = new ClientService(_clientRepository.Object);
        }

        [Fact]
        public async Task GetClientReturnsRepositoryResult()
        {
            // Arrange
            var givenClient = new Client(Guid.NewGuid(), "some-name", "some-address", DateTime.UtcNow);
            _clientRepository.Setup(c => c.GetClient(givenClient.Id)).ReturnsAsync(givenClient);

            // Act
            var getResult = await _service.GetClient(givenClient.Id);

            // Assert
            getResult.Should().BeEquivalentTo(givenClient);
        }
    }
}
