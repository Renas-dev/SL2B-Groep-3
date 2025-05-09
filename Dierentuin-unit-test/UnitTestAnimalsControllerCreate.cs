﻿using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Dierentuin_unit_test
{
    public class UnitTestAnimalsControllerCreate
    {
        private AnimalsController _controller;
        private DbContextOptions<Dierentuin_AppContext> _options;
        private Mock<IMemoryCache> _mockMemoryCache;

        public UnitTestAnimalsControllerCreate()
        {
            // Set up in-memory database options for testing
            _options = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            // Set up mock memory cache
            _mockMemoryCache = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task Create_ValidAnimal_RedirectsToIndex()
        {
            using (var context = new Dierentuin_AppContext(_options))
            {
                // Arrange: Add a test stall to the in-memory database
                context.Stall.Add(new Stall { Id = 1, Name = "Test Stall" });
                await context.SaveChangesAsync();
                _controller = new AnimalsController(context, _mockMemoryCache.Object);

                // Arrange: create a new animal to the in-memory database
                var newAnimal = new Animal
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
                };

                // Act: Attempt to create the new animal
                var result = await _controller.Create(newAnimal) as RedirectToActionResult;

                // Assert: Verify that the action redirects to the Index page
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            }
        }
    }
}