using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Dierentuin_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Dierentuin_unit_test
{
    // Class containing tests for the UpdatedAnimalsController
    public class UpdatedAnimalsControllerTests
    {
        // Test method: check if adding a valid animal redirects to the Index action.
        [Fact]
        public async Task Add_ValidAnimal_ReturnsRedirectToAction()
        {
            // Set up in-memory database and the controller
            var dbContextOptions = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "AddAnimalDatabase")
                .Options; 
            using var dbContext = new Dierentuin_AppContext(dbContextOptions); 
            var controller = new UpdatedAnimalsController(dbContext); 

            // Create a new animal with all required properties 
            var animal = new UpdatedAnimal
            {
                Name = "Cat",
                Species = "American Shorthair", 
                Category = "Mammal",
                Size = "Small",
                Dietary = "Carnivore",
                ActivityPattern = "Cathemeral", 
                Prey = "Mice",
                Enclosure = "Home",
                SpaceRequirement = "Small",
                SecurityRequirement = "Low" 
            };

            // Call the create method of the controller
            var result = await controller.Create(animal);

            // Verify that the result is a RedirectToActionResult redirecting to the Index action
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            // Check if one animal was added to the database
            Assert.Equal(1, dbContext.UpdatedAnimal.Count()); 
        }

        // Test method: check if editing a valid animal redirects to the Index action
        [Fact]
        public async Task Edit_ValidAnimal_ReturnsRedirectToAction()
        {
            // Set up in-memory database and the controller
            var dbContextOptions = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "EditAnimalDatabase")
                .Options; 
            using var dbContext = new Dierentuin_AppContext(dbContextOptions);
            var controller = new UpdatedAnimalsController(dbContext);

            // Create a new animal and add it to the context
            var animal = new UpdatedAnimal
            {
                Name = "Cat",
                Species = "American Shorthair",
                Category = "Mammal",
                Size = "Small",
                Dietary = "Carnivore",
                ActivityPattern = "Cathemeral",
                Prey = "Mice",
                Enclosure = "Home",
                SpaceRequirement = "Small",
                SecurityRequirement = "Low"
            };

            // Add and save animal to the dbContext
            dbContext.UpdatedAnimal.Add(animal); 
            dbContext.SaveChanges(); 

            // Get the ID of the added animal
            var animalId = animal.Id;

            // Detach the original animal from the context
            dbContext.Entry(animal).State = EntityState.Detached;

            // Create an updated version of the animal
            var updatedAnimal = new UpdatedAnimal
            {
                Id = animalId, // Use the auto-generated ID
                Name = "Updated Cat",
                Species = "American Shorthair",
                Category = "Mammal",
                Size = "Small",
                Dietary = "Carnivore",
                ActivityPattern = "Cathemeral",
                Prey = "Mice",
                Enclosure = "Home",
                SpaceRequirement = "Small",
                SecurityRequirement = "Low"
            };

            // Call the Edit method of the controller with the updated animal
            var result = await controller.Edit(animalId, updatedAnimal);

            // Verify that the result is a RedirectToActionResult redirecting to the Index action
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            // Find and check animal's name was updated in the context
            var findUpdatedAnimal = dbContext.UpdatedAnimal.Find(animalId);
            Assert.Equal("Updated Cat", findUpdatedAnimal.Name);
        }

        // Test method: check if deleting a valid animal redirects to the Index action
        [Fact]
        public async Task Delete_ValidId_ReturnsRedirectToAction()
        {
            // Set up in-memory database and the controller
            var dbContextOptions = new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAnimalDatabase")
                .Options;
            using var dbContext = new Dierentuin_AppContext(dbContextOptions);
            var controller = new UpdatedAnimalsController(dbContext); 

            // Create a new animal 
            var animal = new UpdatedAnimal
            {
                Name = "Cat",
                Species = "American Shorthair",
                Category = "Mammal",
                Size = "Small",
                Dietary = "Carnivore",
                ActivityPattern = "Cathemeral",
                Prey = "Mice",
                Enclosure = "Home",
                SpaceRequirement = "Small",
                SecurityRequirement = "Low"
            };

            // Add and save animal to the dbContext
            dbContext.UpdatedAnimal.Add(animal); 
            dbContext.SaveChanges(); 

            // Get the ID of the added animal
            var animalId = animal.Id;

            // Call the DeleteConfirmed method of the controller with the animal's ID
            var result = await controller.DeleteConfirmed(animalId);

            // Verify that the result is a RedirectToActionResult redirecting to the Index action
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
           
            // Check that the context is empty because there was only 1 animal
            Assert.Empty(dbContext.UpdatedAnimal); 
        }
    }
}
