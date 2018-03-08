using System;
using System.Collections.Generic;
using FoodieBuddy.Domain.Models.Reservations;

namespace FoodieBuddy.Domain.Reservations
{
    public interface IReservationRepository: IRepository<Reservation>
    {
        
    }
}