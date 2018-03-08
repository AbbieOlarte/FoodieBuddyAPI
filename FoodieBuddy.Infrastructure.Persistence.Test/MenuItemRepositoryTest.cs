using FoodieBuddy.Domain.Models.MenuItems;
using FoodieBuddy.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieBuddy.Infrastructure.Persistence.Test
{
    [TestClass]
    public class MenuItemRepositoryTest
    {
        private MenuItem foodItem;
        private DbContextOptions<FoodieBuddyDbContext> dbOptions;
        private FoodieBuddyDbContext dbContext;
        private String connectionString;
        private MenuItemRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            foodItem = new MenuItem
            {
                FoodName = "Regular Burger",
                Ingridients = "Beef Patty, Pickles, Tomato",
                Price = 35
            };

            connectionString = @"Server=.;Database=FoodieBuddy;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<FoodieBuddyDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new FoodieBuddyDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new MenuItemRepository(dbContext);
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
            var newFoodItem = sut.Create(foodItem);

            // Assert
            Assert.IsNotNull(newFoodItem);
            Assert.IsTrue(newFoodItem.FoodId != null);

            // CleanUp
            sut.Delete(foodItem.FoodId);
        }

        [TestMethod]
        public void Delete_WithExistingReservationData_RemovesDataFromDb()
        {
            // Arrange
            var existingMenuItem = sut.Create(foodItem);

            // Act
            sut.Delete(existingMenuItem.FoodId);

            // Assert
            foodItem = sut.Retrieve(existingMenuItem.FoodId);
            Assert.IsNull(foodItem);
        }

        [TestMethod]
        public void Retrieve_WithAnExistingFoodItemId_ReturnsRecordFromDb()
        {
            // Arrange
            var existingMenuItem = sut.Create(foodItem);

            // Act
            var found = sut.Retrieve(existingMenuItem.FoodId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(existingMenuItem.FoodId);
        }

        [TestMethod]
        public void Update_MenuItemWithValidData_SavesUpdateInDatabse()
        {
            // Arrange
            var newMenuitem = sut.Create(foodItem);
            var expectedFoodName = "Cheese Burger";
            var expectedIngredients = "Beef Patty, Pickles, Tomato, Cheese";
            var expectedPrice = 45;

            newMenuitem.FoodName = expectedFoodName;
            newMenuitem.Ingridients = expectedIngredients;
            newMenuitem.Price = expectedPrice;

            // Act
            sut.Update(newMenuitem.FoodId, newMenuitem);

            // Assert
            var updatedMenuItem = sut.Retrieve(newMenuitem.FoodId);
            Assert.AreEqual(expectedFoodName, updatedMenuItem.FoodName);
            Assert.AreEqual(expectedIngredients, updatedMenuItem.Ingridients);
            Assert.AreEqual(expectedPrice, updatedMenuItem.Price);

            // CleanUp
            sut.Delete(updatedMenuItem.FoodId);
        }
    }
}
