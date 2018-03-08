using System;

namespace FoodieBuddy.Domain.Reservations
{
    public class ReservationDateRequiredException : Exception
    {
        public ReservationDateRequiredException(string message) : base(message)
        {

        }
    }
}