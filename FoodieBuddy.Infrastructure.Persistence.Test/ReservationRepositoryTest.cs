using FoodieBuddy.Domain.Models.Reservations;
using FoodieBuddy.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ReservationRepositoryTest
    {
        private Reservation reservation;
        private DbContextOptions<FoodieBuddyDbContext> dbOptions;
        private FoodieBuddyDbContext dbContext;
        private String connectionString;
        private ReservationRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            reservation = new Reservation
            {
                ReservationDate = DateTime.Today,
                ReservationTime = "8:00am",
                PartySize = "6 people"
            };

            connectionString = @"Server=.;Database=FoodieBuddy;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<FoodieBuddyDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new FoodieBuddyDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new ReservationRepository(dbContext);
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            // Arrange
            var newReservation = sut.Create(reservation);

            // Assert
            Assert.IsNotNull(newReservation);
            Assert.IsTrue(newReservation.ReservationId != null);

            // CleanUp
            sut.Delete(reservation.ReservationId);
        }

        [TestMethod]
        public void Delete_WithExistingReservationData_RemovesRecordFromDatabase()
        {
            // Arrange
            var existingReservation = sut.Create(reservation);

            // Act
            sut.Delete(existingReservation.ReservationId);

            // Assert
            reservation = sut.Retrieve(existingReservation.ReservationId);
            Assert.IsNull(reservation);
        }

        [TestMethod]
        public void Retrieve_WithAnExistingReservationId_ReturnsRecordFromDb()
        {
            // Arrange
            var existingReservation = sut.Create(reservation);

            // Act
            var found = sut.Retrieve(existingReservation.ReservationId);

            // Assert 
            Assert.IsNotNull(found);

            // CleanUp
            sut.Delete(existingReservation.ReservationId);
        }

        [TestMethod]
        public void Update_ReservationWithValidData_SavesUpdateInDatabase()
        {
            // Arrange
            var newReservation = sut.Create(reservation);
            var expectedReservationDate = DateTime.Today.AddDays(1);
            var expectedReservationTime = "10:00am";
            var expectedPartySize = "4 people";

            newReservation.ReservationDate = expectedReservationDate;
            newReservation.ReservationTime = expectedReservationTime;
            newReservation.PartySize = expectedPartySize;

            // Act
            sut.Update(newReservation.ReservationId, newReservation);

            // Assert
            var updatedReservation = sut.Retrieve(newReservation.ReservationId);
            Assert.AreEqual(expectedReservationDate, updatedReservation.ReservationDate);
            Assert.AreEqual(expectedReservationTime, updatedReservation.ReservationTime);
            Assert.AreEqual(expectedPartySize, updatedReservation.PartySize);

            // CleanUp
            sut.Delete(updatedReservation.ReservationId);
        }
    }
}
