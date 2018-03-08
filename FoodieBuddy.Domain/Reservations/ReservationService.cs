using FoodieBuddy.Domain.Models.Reservations;
using System;

namespace FoodieBuddy.Domain.Reservations
{
    public class ReservationService: IReservationService
    {
        private IReservationRepository reservationRepository;
        public ReservationService(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }
        public Reservation Save(Guid id, Reservation reservation)
        {
            if (reservation.ReservationDate < DateTime.Today)
            {
                throw new ReservationDateRequiredException("Reservation Date is Required.");
            }
            if (String.IsNullOrEmpty(reservation.ReservationTime))
            {
                throw new ReservationTimeRequiredException("Reservation time is required");
            }
            if (String.IsNullOrEmpty(reservation.PartySize))
            {
                throw new PartySizeRequiredException("Party Size is required.");
            }

            Reservation result = null;

            var foundReservation = reservationRepository.Retrieve(id);

            if (foundReservation == null)
            {
                result = reservationRepository.Create(reservation);
            }
            else
            {
                result = reservationRepository.Update(reservation.ReservationId, reservation);
            }
            return result;
        }
    }
}