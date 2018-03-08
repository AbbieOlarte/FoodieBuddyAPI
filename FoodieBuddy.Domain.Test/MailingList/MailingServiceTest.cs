using FoodieBuddy.Domain.MailingList;
using FoodieBuddy.Domain.Models.MailingList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Domain.Test.MailingList
{
    [TestClass]
    public class MailingServiceTest
    {
        private Mail email;
        private Mock<IMailingRepository> mockMailingRepository;
        private MailingService sut;
        private Guid nonExistingMailingId = Guid.Empty;
        private Guid existingMailingId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            email = new Mail{
                Email = "abolarte@outlook.com"
            };
            mockMailingRepository = new Mock<IMailingRepository>();
            sut = new MailingService(mockMailingRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_WithValidMailingDetials_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(email.MailingId, email);

            // Assert
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Once);
            mockMailingRepository.Verify(m => m.Create(email), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingMailingDetails_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockMailingRepository
                .Setup(m => m.Retrieve(existingMailingId))
                .Returns(email);

            // Act
            var result = sut.Save(existingMailingId, email);

            // Assert
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Once);
            mockMailingRepository.Verify(m => m.Update(existingMailingId, email), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidMailingDetails_ShouldReturnWithMailingId()
        {
            // Arrange
            mockMailingRepository
                .Setup(m => m.Create(email))
                .Callback(() => email.MailingId = Guid.NewGuid())
                .Returns(email);

            // Act
            var result = sut.Save(email.MailingId, email);

            // Assert
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Once);
            mockMailingRepository.Verify(m => m.Create(email), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankEmail_ShouldThrowEmailRequiredException()
        {
            // Arrange
            email.Email = "";

            // Assert
            Assert.ThrowsException<EmailRequiredException>(() => sut.Save(email.MailingId, email));
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Never);
            mockMailingRepository.Verify(m => m.Create(email), Times.Never);
        }
    }
}
