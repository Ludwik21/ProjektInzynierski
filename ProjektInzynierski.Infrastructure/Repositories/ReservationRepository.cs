using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Domain.Entities.Reservations;
using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ProjektContext _context;

        public ReservationRepository(ProjektContext context)
        {
            _context = context;
        }

        public async Task AddReservation(Reservation reservation)
        {
            var reservationDao = new ReservationDao()
            {
                Id = reservation.Id,
                RequestingUserId = reservation.RequestingUserId,
                ClientId = reservation.ClientId,
                ReservationDate = reservation.ReservationDate,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Status = reservation.Status
            };

            var reservationItems = reservation.EquipmentIds.Select(x => new ReservationItemDao()
            {
                ReservationId = reservation.Id,
                EquipmentId = x
            });

            await _context.Reservations.AddAsync(reservationDao);
            if (reservationItems.Any())
            {
                await _context.ReservationItems.AddRangeAsync(reservationItems);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReservation(Guid id)
        {
            var reservationItems = await _context.ReservationItems.Where(x => x.ReservationId == id).ToListAsync();
            _context.ReservationItems.RemoveRange(reservationItems);

            var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);

            if(reservation is null)
            {
                throw new InvalidOperationException("Reservation was not found");
            }

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync(true);
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            var itemsToRecreate = _context.ReservationItems.Where(x => x.ReservationId == reservation.Id).ToList();
            if(itemsToRecreate.Any())
            {
                _context.ReservationItems.RemoveRange(itemsToRecreate);
            }

            var newReservationItems = reservation.EquipmentIds.Select(x => new ReservationItemDao()
            {
                ReservationId = reservation.Id,
                EquipmentId = x
            });

            if (newReservationItems.Any())
            {
                await _context.ReservationItems.AddRangeAsync(newReservationItems);
            }

            _context.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
