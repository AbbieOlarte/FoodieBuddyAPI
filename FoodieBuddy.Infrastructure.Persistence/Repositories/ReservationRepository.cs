using System;
using System.Collections.Generic;
using System.Linq;
using FoodieBuddy.Domain.Models.Reservations;
using FoodieBuddy.Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FoodieBuddy.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(IFoodieBuddyDbContext context) : base(context)
        {
        }
    }
}