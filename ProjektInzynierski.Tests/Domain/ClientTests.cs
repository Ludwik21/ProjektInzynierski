using FluentAssertions;
using ProjektInzynierski.Domain.Entities.Clients;

namespace ProjektInzynierski.Tests.Domain
{
    public class ClientTests
    {
        [Fact]
        public void NewClientSetId()
        {
            // Arrange

            // Act
            var client = new Client("some-name", "some-address");

            // Assert
            client.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void NewClientSetsRegistrationDate()
        {
            // Arrange

            // Act
            var client = new Client("some-name", "some-address");

            // Assert
            client.RegistrationDate.Date.Should().Be(DateTime.UtcNow.Date);
        }
    }
}
