using FoodieBuddy.Domain.Models.Reservations;
using FoodieBuddy.Domain.Reservations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Domain.Test
{
    [TestClass]
    public class ReservationServiceTest
    {
        private Reservation reservation;
        private Mock<IReservationRepository> mockReservationRepository;
        private ReservationService sut;
        private Guid nonExistingReservationId = Guid.Empty;
        private Guid existingReservationId = Guid.NewGuid();

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
            sut = new ReservationService(mockReservationRepository.Object);
            mockReservationRepository.Setup(r => r.Retrieve(nonExistingReservationId)).Returns<Reservation>(null);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_ReservationWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(reservation.ReservationId, reservation);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(nonExistingReservationId), Times.Once());
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Once());
        }

        [TestMethod]
        public void Save_ReservationWithValidData_ReturnsNewReservationWithReservationId()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Create(reservation))
                .Callback(() => reservation.ReservationId = Guid.NewGuid())
                .Returns(reservation);

            // Act
            var newReservation = sut.Save(reservation.ReservationId, reservation);

            // Assert
            Assert.IsTrue(newReservation.ReservationId != null);
        }

        [TestMethod]
        public void Save_WithExistingReservation_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockReservationRepository.Setup(r => r.Retrieve(existingReservationId)).Returns(reservation);

            // Act
            var existingReservation = sut.Save(existingReservationId, reservation);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(existingReservationId), Times.Once);
            mockReservationRepository.Verify(r => r.Update(reservation.ReservationId, reservation), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankDate_ShouldThrowReservationDateRequiredException()
        {
            // Arrange
            reservation.ReservationDate = DateTime.Now.AddDays(-1);

            // Assert
            Assert.ThrowsException<ReservationDateRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankTime_ShouldThrowReservationTimeRequiredException()
        {
            // Arrange
            reservation.ReservationTime = "";

            // Assert
            Assert.ThrowsException<ReservationTimeRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankPartySize_ShouldThrowPartySizeRequiredException()
        {
            // Arrange
            reservation.PartySize = "";

            // Assert
            Assert.ThrowsException<PartySizeRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

    }
}
