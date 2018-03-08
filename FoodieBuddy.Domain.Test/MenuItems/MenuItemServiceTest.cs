using FoodieBuddy.Domain.MenuItems;
using FoodieBuddy.Domain.Models.MenuItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Domain.Test.MenuItems
{
    [TestClass]
    public class MenuItemServiceTest
    {
        private MenuItem foodItem;
        private Mock<IMenuItemRepository> mockMenuItemRepository;
        private MenuItemService sut;
        private Guid nonExistingFoodId = Guid.Empty;
        private Guid existingFoodId = Guid.NewGuid();

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
            sut = new MenuItemService(mockMenuItemRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_MenuItemWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(foodItem.FoodId, foodItem);

            // Assert
            mockMenuItemRepository.Verify(f => f.Retrieve(foodItem.FoodId), Times.Once);
            mockMenuItemRepository.Verify(f => f.Create(foodItem), Times.Once);
        }

        [TestMethod]
        public void Save_MenuItemWithValidData_ReturnWithFoodId()
        {
            // Arrange
            mockMenuItemRepository
                .Setup(f => f.Create(foodItem))
                .Callback(() => foodItem.FoodId = Guid.NewGuid())
                .Returns(foodItem);

            // Act
            var newFoodItem = sut.Save(foodItem.FoodId, foodItem);

            // Assert
            mockMenuItemRepository.Verify(f => f.Retrieve(nonExistingFoodId), Times.Once);
            mockMenuItemRepository.Verify(f => f.Create(foodItem), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingMenuItem_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockMenuItemRepository
                .Setup(f => f.Retrieve(existingFoodId))
                .Returns(foodItem);

            // Act
            var existingFoodItem = sut.Save(existingFoodId, foodItem);

            // Assert
            mockMenuItemRepository.Verify(f => f.Retrieve(existingFoodId), Times.Once);
            mockMenuItemRepository.Verify(f => f.Update(existingFoodId, foodItem), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankFoodName_ShouldThrowFoodNameRequiredException()
        {
            // Arrange
            foodItem.FoodName = "";

            // Assert
            Assert.ThrowsException<FoodNameRequiredException>(
                () => sut.Save(foodItem.FoodId, foodItem));
            mockMenuItemRepository.Verify(f => f.Create(foodItem), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankIngredients_ShouldThrowIngredientsRequiredException()
        {
            // Arrange
            foodItem.Ingridients = "";

            // Assert
            Assert.ThrowsException<IngredientsRequiredException>(
                () => sut.Save(foodItem.FoodId, foodItem));
        }

        [TestMethod]
        public void Save_WithZeroPrice_ShouldThrowPriceRequiredException()
        {
            // Arrange
            foodItem.Price = 0;

            // Assert
            Assert.ThrowsException<PriceRequiredException>(
                () => sut.Save(foodItem.FoodId, foodItem));
        }
    }
}
