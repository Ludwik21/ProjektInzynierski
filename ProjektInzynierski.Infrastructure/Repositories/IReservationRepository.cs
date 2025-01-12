using ProjektInzynierski.Domain.Entities.Reservations;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public interface IReservationRepository
    {
        Task AddReservation(Reservation reservation);
        Task RemoveReservation(Guid id);
        Task UpdateReservation(Reservation reservation);
        Task<Reservation> GetReservationById(Guid id);


    }
}
