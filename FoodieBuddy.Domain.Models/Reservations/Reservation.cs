using System;

namespace FoodieBuddy.Domain.Models.Reservations
{
    public class Reservation
    {
        public DateTime? ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public string PartySize { get; set; }
        public Guid ReservationId { get; set; }
    }
}