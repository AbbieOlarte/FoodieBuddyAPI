using System;
using System.Collections.Generic;
using FoodieBuddy.API.Utils;
using FoodieBuddy.Domain.Models.Reservations;
using FoodieBuddy.Domain.Reservations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FoodieBuddy.API.Controllers
{
    [EnableCors("FoodieBuddyApp")]
    [Produces("application/json")]
    [Route("api/Reservations")]
    public class ReservationsController: Controller
    {
        private IReservationRepository reservationRepository;
        private IReservationService reservationService;

        public ReservationsController(IReservationRepository reservationRepository, IReservationService reservationService)
        {
            this.reservationRepository = reservationRepository;
            this.reservationService = reservationService;
        }

        [HttpGet, ActionName("GetReservations")]
        public IActionResult GetReservations(Guid? id)
        {
            var result = new List<Reservation>();
            if (id == null)
            {
                result.AddRange(this.reservationRepository.Retrieve());
            }
            else
            {
                var reservation = this.reservationRepository.Retrieve(id.Value);
                result.Add(reservation);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                if (reservation == null)
                {
                    return BadRequest();
                }
                var result = this.reservationService.Save(Guid.Empty, reservation);
                return CreatedAtAction("GetReservations", new { id = reservation.ReservationId }, reservation);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult DeleteReservation(Guid id)
        {
            var reservationToDelete = this.reservationRepository.Retrieve(id);
            if (reservationToDelete == null)
            {
                return NotFound();
            }
            this.reservationRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateReservation([FromBody] Reservation reservation, Guid id)
        {
            try
            {
                if (reservation == null)
                {
                    return BadRequest();
                }
                var existingReservation = reservationRepository.Retrieve(id);
                if (existingReservation == null)
                {
                    return NotFound();
                }
                existingReservation.ApplyChanges(reservation);
                var result = this.reservationService.Save(id, existingReservation);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IActionResult PatchReservation(JsonPatchDocument patchedReservation, Guid id)
        {
            if (patchedReservation == null)
            {
                return BadRequest();
            }
            var reservation = reservationRepository.Retrieve(id);
            if (reservation == null)
            {
                return NotFound();
            }
            patchedReservation.ApplyTo(reservation);
            reservationService.Save(id, reservation);
            return Ok(reservation);
        }
    }
}