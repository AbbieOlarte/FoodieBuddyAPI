using FoodieBuddy.Domain.Models.Reservations;
using System;

namespace FoodieBuddy.Domain.Reservations
{
    public interface IReservationService
    {
        Reservation Save(Guid id, Reservation reservation);
    }
}