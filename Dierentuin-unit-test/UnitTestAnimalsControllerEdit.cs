using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dierentuin_unit_test
{
    public class UnitTestAnimalsControllerEdit
    {
        private AnimalsController _controller;
        private DbContextOptions<Dierentuin_AppContext> _options;

        public UnitTestAnimalsControllerEdit()
        {
            // Set up in-memory database options for testing
            _options = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;
        }

        [Fact]
        public async Task Edit_ValidAnimal_RedirectsToIndex()
        {
            using (var context = new Dierentuin_AppContext(_options))
            {
                // Arrange: Add a test animal to the in-memory database
                context.Animal.Add(new Animal
                {
                    Id = 6,
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

                // Arrange: Initialize the controller after adding the test animal
                _controller = new AnimalsController(context);

                // Act: Retrieve the existing animal to update
                var existingAnimal = await context.Animal.FindAsync(6);

                // Assert: Ensure the existing animal is found
                Assert.NotNull(existingAnimal);

                // Modify the existing animal
                existingAnimal.Name = "Cat"; // Change the name to "Cat"

                // Act: Attempt to edit the animal using the updated data
                var result = await _controller.Edit(6, existingAnimal) as RedirectToActionResult;

                // Assert: Verify that the action redirects to the Index page
                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            }
        }
    }
}