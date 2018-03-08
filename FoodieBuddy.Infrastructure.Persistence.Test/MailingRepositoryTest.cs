using FoodieBuddy.Domain.Models.MailingList;
using FoodieBuddy.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Infrastructure.Persistence.Test
{
    [TestClass]
    public class MailingRepositoryTest
    {
        private Mail mail;
        private DbContextOptions<FoodieBuddyDbContext> dbOptions;
        private FoodieBuddyDbContext dbContext;
        private String connectionString;
        private MailingRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            mail = new Mail
            {
                Email = "abolarte@outlook.com"
            };

            connectionString = @"Server=.;Database=FoodieBuddy;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<FoodieBuddyDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new FoodieBuddyDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new MailingRepository(dbContext);
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
            var newMail = sut.Create(mail);

            // Assert
            Assert.IsNotNull(newMail);
            Assert.IsTrue(newMail.MailingId != null);

            // CleanUp
            sut.Delete(mail.MailingId);
        }

        [TestMethod]
        public void Delete_WithExistingMailingData_RemoveRecordFromDb()
        {
            // Arrange
            var existingMail = sut.Create(mail);

            // Act
            sut.Delete(existingMail.MailingId);

            // Assert
            mail = sut.Retrieve(existingMail.MailingId);
            Assert.IsNull(mail);
        }

        [TestMethod]
        public void Retrieve_WithAnExistingMailingId_ReturnsRecordFromDb()
        {
            // Arrange
            var existingMail = sut.Create(mail);

            // Act
            var found = sut.Retrieve(existingMail.MailingId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(existingMail.MailingId);
        }

        [TestMethod]
        public void Update_MailingWithValidData_SavesUpdateInTheDatabase()
        {
            // Arrange
            var newMail = sut.Create(mail);
            var expectedEmail = "abbieolarte@gmail.com";
            newMail.Email = expectedEmail;

            // Act
            sut.Update(newMail.MailingId, newMail);

            // Assert
            var updatedMail = sut.Retrieve(newMail.MailingId);
            Assert.AreEqual(updatedMail.Email, expectedEmail);

            // CleanUp
            sut.Delete(updatedMail.MailingId);
        }
    }
}
