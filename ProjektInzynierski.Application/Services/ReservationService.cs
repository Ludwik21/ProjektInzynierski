using ProjektInzynierski.Application.Models.Reservation;
using ProjektInzynierski.Domain.Entities.Reservations;
using ProjektInzynierski.Infrastructure.Repositories;

namespace ProjektInzynierski.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task MakeReservation(Guid clientId, int requestingUserId, CreateReservationDto request)
        {
            var reservation = new Reservation(requestingUserId, clientId, request.StartDate, request.EndDate);
            
            foreach(var equipmentId in request.EquipmentIds)
            {
                reservation.AddEquipment(equipmentId);
            }

            await _repository.AddReservation(reservation);
        }
    }
}
