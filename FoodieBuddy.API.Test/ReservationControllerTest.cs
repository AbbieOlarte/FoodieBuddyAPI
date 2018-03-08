using FoodieBuddy.API.Controllers;
using FoodieBuddy.Domain.Models.Reservations;
using FoodieBuddy.Domain.Reservations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.API.Test
{
    [TestClass]
    public class ReservationControllerTest
    {
        private Reservation reservation;
        private Mock<IReservationRepository> mockReservationRepository;
        private Mock<IReservationService> mockReservationService;
        private ReservationsController sut;
        private Guid nonExistingReservationId = Guid.Empty;
        private Guid existingReservationId = Guid.NewGuid();
        private JsonPatchDocument patchedReservation;

        [TestInitialize]
        public void Initialize()
        {
            reservation = new Reservation
            {
                ReservationDate = DateTime.Today,
                ReservationTime = "8:00am",
                PartySize = "6 people"
            };

            mockReservationRepository = new Mock<IReservationRepository>();
            mockReservationService = new Mock<IReservationService>();

            sut = new ReservationsController(mockReservationRepository.Object, mockReservationService.Object);

            patchedReservation = new JsonPatchDocument();
            patchedReservation.Replace("PartySize", "5 people");

            mockReservationRepository
                .Setup(r => r.Retrieve(existingReservationId))
                .Returns(reservation);

            mockReservationRepository
                .Setup(r => r.Retrieve(nonExistingReservationId))
                .Returns<Reservation>(null);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetReservations_WithoutReservationId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetReservations(null);

            // Assert 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockReservationRepository.Verify(r => r.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetReservations_WithReservationId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetReservations(reservation.ReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
        }

        [TestMethod]
        public void CreateReservation_WithValidData_ShouldReturnCreateAtActionResult()
        {
            // Act
            var result = sut.CreateReservation(reservation);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Once);
        }

        [TestMethod]
        public void CreateReservation_WithNoData_ShouldReturnBadRequestResult()
        {
            // Arrange
            reservation = null;

            // Act
            var result = sut.CreateReservation(reservation);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockReservationService.Verify(r => r.Save(Guid.Empty, reservation), Times.Never);
        }

        [TestMethod]
        public void DeleteReservation_WithReservationId_ShouldReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteReservation(existingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockReservationRepository.Verify(r => r.Delete(existingReservationId), Times.Once);
        }

        [TestMethod]
        public void DeleteReservation_WithoutReservatioId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.DeleteReservation(nonExistingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockReservationRepository.Verify(r => r.Retrieve(nonExistingReservationId), Times.Once);
            mockReservationRepository.Verify(r => r.Delete(nonExistingReservationId), Times.Never);
        }

        [TestMethod]
        public void UpdateReservation_WithExistingDataAndId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateReservation(reservation, existingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockReservationRepository.Verify(r => r.Retrieve(existingReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(existingReservationId, reservation), Times.Once);
        }

        [TestMethod]
        public void UpdateReservation_ReservationWithoutValue_ShouldReturnBadRequestResult()
        {
            // Arrange
            reservation = null;

            // Act
            var result = sut.UpdateReservation(reservation, existingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockReservationRepository.Verify(r => r.Retrieve(existingReservationId), Times.Never);
            mockReservationService.Verify(r => r.Save(existingReservationId, reservation), Times.Never);
        }

        [TestMethod]
        public void UpdateReservation_WithoutExistingReservationId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.UpdateReservation(reservation, nonExistingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockReservationRepository.Verify(r => r.Retrieve(nonExistingReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(nonExistingReservationId, reservation), Times.Never);
        }

        [TestMethod]
        public void PatchReservation_WithExistingReservationAndId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchReservation(patchedReservation, existingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockReservationRepository.Verify(r => r.Retrieve(existingReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(existingReservationId, reservation), Times.Once);
        }

        [TestMethod]
        public void PatchReservation_WithoutExistingReservationId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.PatchReservation(patchedReservation, nonExistingReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockReservationRepository.Verify(r => r.Retrieve(nonExistingReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(nonExistingReservationId, reservation), Times.Never);
        }

        [TestMethod]
        public void PatchReservation_WithEmptyPatchDocument_ShouldReturnBadRequestResult()
        {
            // Arrange
            patchedReservation = null;

            // Act
            var result = sut.PatchReservation(patchedReservation, reservation.ReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Never);
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Never);
        }
    }
}
