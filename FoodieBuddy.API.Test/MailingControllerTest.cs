using FoodieBuddy.API.Controllers;
using FoodieBuddy.Domain.MailingList;
using FoodieBuddy.Domain.Models.MailingList;
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
    public class MailingControllerTest
    {
        private Mail mail;
        private Mock<IMailingRepository> mockMailingRepository;
        private Mock<IMailingService> mockMailingService;
        private MailingController sut;
        private Guid existingMailingId = Guid.NewGuid();
        private Guid nonExistingMailingId = Guid.Empty;
        private JsonPatchDocument patchedMail;

        [TestInitialize]
        public void Initialize()
        {
            mail = new Mail
            {
                Email = "abolarte@outlook.com"
            };

            mockMailingRepository = new Mock<IMailingRepository>();
            mockMailingService = new Mock<IMailingService>();

            sut = new MailingController(mockMailingRepository.Object, mockMailingService.Object);

            mockMailingRepository
                .Setup(m => m.Retrieve(existingMailingId))
                .Returns(mail);

            mockMailingRepository
                .Setup(m => m.Retrieve(nonExistingMailingId))
                .Returns<Mail>(null);

            patchedMail = new JsonPatchDocument();
            patchedMail.Replace("Email", "abbieolarte@yahoo.com");
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetMail_WithoutMailingId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetMail(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMailingRepository.Verify(m => m.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetMail_WithMailingId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetMail(existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Once);
        }

        [TestMethod]
        public void CreateMail_WithValidData_ShouldReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateMail(mail);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockMailingService.Verify(m => m.Save(mail.MailingId, mail), Times.Once);
        }

        [TestMethod]
        public void CreateMail_WithNoData_ShoudReturnBadRequestResult()
        {
            // Arrange
            mail = null;

            // Act
            var result = sut.CreateMail(mail);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMailingService.Verify(m => m.Save(Guid.Empty, mail), Times.Never);
        }

        [TestMethod]
        public void DeleteMail_WithMailingId_ShouldReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteMail(existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Once);
            mockMailingRepository.Verify(m => m.Delete(existingMailingId), Times.Once);
        }

        [TestMethod]
        public void DeleteMail_WithNoExistingMailingId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.DeleteMail(nonExistingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Once);
            mockMailingRepository.Verify(m => m.Delete(nonExistingMailingId), Times.Never);
        }

        [TestMethod]
        public void UpdateMail_WithValidDataAndId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateMail(mail, existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Once);
            mockMailingService.Verify(m => m.Save(existingMailingId, mail), Times.Once);
        }

        [TestMethod]
        public void UpdateMail_WithNoValidData_ShouldReturnBadRequestResult()
        {
            // Arrange
            mail = null;

            // Act
            var result = sut.UpdateMail(mail, existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Never);
            mockMailingService.Verify(m => m.Save(existingMailingId, mail), Times.Never);
        }

        [TestMethod]
        public void UpdateMail_WithNoExistingId_ShoulReturnNotFoundResult()
        {
            // Act
            var result = sut.UpdateMail(mail, nonExistingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Once);
            mockMailingService.Verify(m => m.Save(nonExistingMailingId, mail), Times.Never);
        }

        [TestMethod]
        public void PatchMail_WithValidPatchDocument_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchMail(patchedMail, existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Once);
            mockMailingService.Verify(m => m.Save(existingMailingId, mail));
        }

        [TestMethod]
        public void PatchMail_WithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedMail = null;

            // Act
            var result = sut.PatchMail(patchedMail, existingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMailingRepository.Verify(m => m.Retrieve(existingMailingId), Times.Never);
            mockMailingService.Verify(m => m.Save(existingMailingId, mail), Times.Never);
        }

        [TestMethod]
        public void PatchMail_WithNoExistingId_ReturnNotFoundResult()
        {
            // Act
            var result = sut.PatchMail(patchedMail, nonExistingMailingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMailingRepository.Verify(m => m.Retrieve(nonExistingMailingId), Times.Once);
            mockMailingService.Verify(m => m.Save(nonExistingMailingId, mail), Times.Never);
        }
    }
}
