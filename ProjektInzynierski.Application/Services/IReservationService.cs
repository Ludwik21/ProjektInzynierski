using ProjektInzynierski.Application.Models.Reservation;
using ProjektInzynierski.Domain.Entities.Reservations;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Application.Services
{
    public interface IReservationService
    {
        public Task MakeReservation(Guid clientId, int requestingUserId, CreateReservationDto request);
        public Task UpdateReservationStatus(Guid id, ReservationStatus status);

    }
}
