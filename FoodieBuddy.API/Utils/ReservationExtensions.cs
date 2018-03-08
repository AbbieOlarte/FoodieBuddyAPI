using FoodieBuddy.Domain.Models.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodieBuddy.API.Utils
{
    public static class ReservationExtensions
    {
        public static Reservation ApplyChanges(this Reservation reservation, Reservation from)
        {
            reservation.ReservationDate = from.ReservationDate;
            reservation.ReservationTime = from.ReservationTime;
            reservation.PartySize = from.PartySize;

            return reservation;
        }
    }
}
