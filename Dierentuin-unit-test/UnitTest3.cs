using Xunit;
using Dierentuin_App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dierentuin_App.Models;
using System.Linq;

namespace Dierentuin_App.Tests
{
    public class StallsControllerTests
    {
        private StallsController _controller;

        public StallsControllerTests()
        {
            // Initialize the controller before each test
            _controller = new StallsController();
            // Clear the list before each test to ensure no interference between tests
            StallsController._stalls.Clear();
        }

        [Fact]
        public void TestCreate()
        {
            // Arrange
            var stall = new Stall { Id = 1, Name = "Test Stall", Biome = "Test Biome", Climate = "Test Climate" };

            // Act
            var result = _controller.Create(stall) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Contains(StallsController._stalls, s => s.Name == "Test Stall" && s.Id == 1);
        }

        [Fact]
        public void TestEdit()
        {
            // Arrange
            var stall = new Stall { Id = 1, Name = "Existing Stall", Biome = "Existing Biome", Climate = "Existing Climate" };
            StallsController._stalls.Add(stall); // Adding a stall for testing

            var updatedStall = new Stall { Id = 1, Name = "Updated Stall", Biome = "Updated Biome", Climate = "Updated Climate" };

            // Act
            var result = _controller.Edit(1, updatedStall) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            var editedStall = StallsController._stalls.FirstOrDefault(s => s.Id == 1);
            Assert.Equal("Updated Stall", editedStall.Name);
            Assert.Equal("Updated Biome", editedStall.Biome);
            Assert.Equal("Updated Climate", editedStall.Climate);
        }

        [Fact]
        public void TestDelete()
        {
            // Arrange
            var stall = new Stall { Id = 1, Name = "Existing Stall", Biome = "Existing Biome", Climate = "Existing Climate" };
            StallsController._stalls.Add(stall); // Adding a stall for testing

            // Verify the stall is added
            Assert.Contains(StallsController._stalls, s => s.Id == 1);

            // Act
            var result = _controller.Delete(1) as RedirectToActionResult;

            // Verify the action result
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            // Verify the stall is removed
            Assert.DoesNotContain(StallsController._stalls, s => s.Id == 1);
        }
    }
}
