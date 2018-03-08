using FoodieBuddy.API.Controllers;
using FoodieBuddy.Domain.MenuItems;
using FoodieBuddy.Domain.Models.MenuItems;
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
    public class MenuItemControllerTest
    {
        private MenuItem foodItem;
        private Mock<IMenuItemRepository> mockMenuItemRepository;
        private Mock<IMenuItemService> mockMenuItemService;
        private MenuItemsController sut;
        private Guid nonExistingFoodId = Guid.Empty;
        private Guid existingFoodId = Guid.NewGuid();
        private JsonPatchDocument patchedFoodItem;

        [TestInitialize]
        public void Initialize()
        {
            foodItem = new MenuItem
            {
                FoodName = "Regular Burger",
                Ingridients = "Beef Patty, Pickles, Tomato",
                Price = 35
            };

            mockMenuItemRepository = new Mock<IMenuItemRepository>();
            mockMenuItemService = new Mock<IMenuItemService>();

            sut = new MenuItemsController(mockMenuItemRepository.Object, mockMenuItemService.Object);

            patchedFoodItem = new JsonPatchDocument();
            patchedFoodItem.Replace("FoodName", "Burger");

            mockMenuItemRepository
                .Setup(m => m.Retrieve(existingFoodId))
                .Returns(foodItem);

            mockMenuItemRepository
                .Setup(m => m.Retrieve(nonExistingFoodId))
                .Returns<MenuItem>(null);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetMenuItem_WithoutFoodId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetMenuItem(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetMenuItem_WithFoodId_ShouldReturnOkObjectResult()
        {
            // Arrange
            mockMenuItemRepository
                .Setup(m => m.Retrieve(existingFoodId))
                .Returns(foodItem);

            // Act
            var result = sut.GetMenuItem(existingFoodId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Once);
        }

        [TestMethod]
        public void CreateMenuItem_WithValidDetails_ShouldReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateMenuItem(foodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockMenuItemService.Verify(m => m.Save(nonExistingFoodId, foodItem), Times.Once);
        }

        [TestMethod]
        public void CreateMenuItem_WithoutValidDetails_ShoutReturnBadRequestResult()
        {
            // Arrange
            foodItem = null;

            // Act
            var result = sut.CreateMenuItem(foodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMenuItemService.Verify(m => m.Save(nonExistingFoodId, foodItem), Times.Never);
        }

        [TestMethod]
        public void DeleteMenuItem_WithExistingFoodId_ShouldReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteMenuItem(existingFoodId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Once);
            mockMenuItemRepository.Verify(m => m.Delete(existingFoodId), Times.Once);
        }

        [TestMethod]
        public void DeleteMenuItem_WithoutExistingFoodId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.DeleteMenuItem(nonExistingFoodId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(nonExistingFoodId), Times.Once);
            mockMenuItemRepository.Verify(m => m.Delete(nonExistingFoodId), Times.Never);
        }

        [TestMethod]
        public void UpdateMenuItem_WithExistingFoodIdAndValidDetails_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateMenuItem(existingFoodId, foodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Once);
            mockMenuItemService.Verify(m => m.Save(existingFoodId, foodItem), Times.Once);
        }

        [TestMethod]
        public void UpdateMenuItem_WithEmptyFoodItem_ShouldReturnBadRequestResult()
        {
            // Arrange
            foodItem = null;

            // Act
            var result = sut.UpdateMenuItem(existingFoodId, foodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Never);
            mockMenuItemService.Verify(m => m.Save(existingFoodId, foodItem), Times.Never);
        }

        [TestMethod]
        public void UpdateMenuItem_WithNonExistingId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.UpdateMenuItem(nonExistingFoodId, foodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(nonExistingFoodId), Times.Once);
            mockMenuItemService.Verify(m => m.Save(nonExistingFoodId, foodItem), Times.Never);
        }

        [TestMethod]
        public void PatchMenuItem_WithValidDetails_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchMenuItem(existingFoodId, patchedFoodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Once);
            mockMenuItemService.Verify(m => m.Save(existingFoodId, foodItem), Times.Once);
        }

        [TestMethod]
        public void PatchMenuItem_WithEmptyPatchDocument_ShouldReturnBadRequestResult()
        {
            // Arrange
            patchedFoodItem = null;

            // Act
            var result = sut.PatchMenuItem(existingFoodId, patchedFoodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(existingFoodId), Times.Never);
            mockMenuItemService.Verify(m => m.Save(existingFoodId, foodItem), Times.Never);
        }

        [TestMethod]
        public void PatchMenuItem_WithNonExistingFoodId_ShouldReturnNotFoundResult()
        {
            // Act
            var result = sut.PatchMenuItem(nonExistingFoodId, patchedFoodItem);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockMenuItemRepository.Verify(m => m.Retrieve(nonExistingFoodId), Times.Once);
            mockMenuItemService.Verify(m => m.Save(nonExistingFoodId, foodItem), Times.Never);
        }
    }
}
