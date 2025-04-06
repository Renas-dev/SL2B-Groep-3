using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Dierentuin_unit_test
{
    public class UnitTestAnimalsControllerDelete
    {
        private AnimalsController _controller;
        private DbContextOptions<Dierentuin_AppContext> _options;
        private Mock<IMemoryCache> _mockMemoryCache;

        public UnitTestAnimalsControllerDelete()
        {
            // Set up in-memory database options for testing
            _options = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            // Set up mock memory cache
            _mockMemoryCache = new Mock<IMemoryCache>();
        }

        // Valid Animal Test
        [Fact]
        public async Task Delete_ValidAnimal_RedirectsToIndex()
        {
            using (var context = new Dierentuin_AppContext(_options))
            {
                // Arrange: Add a test animal to the in-memory database
                context.Animal.Add(new Animal
                {
                    Id = 2,
                    Name = "Tiger",
                    Species = "Panthera tigris",
                    Category = "Mammal",
                    Size = Animal.AnimalSize.Large,
                    DietaryClass = Animal.AnimalDietaryClass.Carnivore,
                    ActivityPattern = Animal.AnimalActivityPattern.Nocturnal,
                    Prey = "Deer",
                    Enclosure = "Forest",
                    SpaceRequirement = 150.25,
                    SecurityRequirement = Animal.AnimalSecurityRequirement.High,
                    StallId = 1
                });
                await context.SaveChangesAsync();

                _controller = new AnimalsController(context, _mockMemoryCache.Object);

                // Act: Attempt to delete the test animal
                var result = await _controller.DeleteConfirmed(2) as RedirectToActionResult;

                // Assert: Verify that the action redirects to the Index page
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            }
        }

        // Non-existing Animal Test
        [Fact]
        public async Task Delete_NonExistentAnimal_ReturnsNotFound()
        {
            using (var context = new Dierentuin_AppContext(_options))
            {
                // Arrange: Add a test animal to the in-memory database
                context.Animal.Add(new Animal
                {
                    Name = "Tiger",
                    Species = "Panthera tigris",
                    Category = "Mammal",
                    Size = Animal.AnimalSize.Large,
                    DietaryClass = Animal.AnimalDietaryClass.Carnivore,
                    ActivityPattern = Animal.AnimalActivityPattern.Nocturnal,
                    Prey = "Deer",
                    Enclosure = "Forest",
                    SpaceRequirement = 150.25,
                    SecurityRequirement = Animal.AnimalSecurityRequirement.High,
                    StallId = 1
                });
                await context.SaveChangesAsync();

                _controller = new AnimalsController(context, _mockMemoryCache.Object);

                // Act: Attempt to delete non-existent animal (Id = 999)
                var result = await _controller.DeleteConfirmed(999) as NotFoundResult;

                // Assert: Verify that NotFoundResult is returned
                Assert.NotNull(result);
            }
        }
    }
}