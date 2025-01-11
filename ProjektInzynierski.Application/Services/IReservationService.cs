using ProjektInzynierski.Application.Models.Reservation;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Application.Services
{
    public interface IReservationService
    {
        public Task MakeReservation(Guid clientId, int requestingUserId, CreateReservationDto request);
    }
}
