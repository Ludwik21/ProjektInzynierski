﻿using ProjektInzynierski.Application.Models.Reservation;

namespace ProjektInzynierski.Application.Services
{
    public interface IReservationService
    {
        public Task MakeReservation(Guid clientId, Guid requestingUserId, CreateReservationDto request);
    }
}
