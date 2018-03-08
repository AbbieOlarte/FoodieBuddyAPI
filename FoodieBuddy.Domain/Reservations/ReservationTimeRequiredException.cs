using System;

namespace FoodieBuddy.Domain.Reservations
{
    public class ReservationTimeRequiredException: Exception
    {
        public ReservationTimeRequiredException(string message): base(message)
        {

        }
    }
}