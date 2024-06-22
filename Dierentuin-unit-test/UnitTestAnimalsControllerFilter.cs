using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using X.PagedList;

namespace Dierentuin_unit_test
{
    public class UnitTestAnimalsControllerFilter
    {
        private DbContextOptions<Dierentuin_AppContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsFilteredPagedList()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                // Insert animal data to database
                var testData = new List<Animal>
                {
                    new Animal { Id = 1, Name = "Lion", Species = "Cat", Category = "Mammal", Size = Animal.AnimalSize.Large, DietaryClass = Animal.AnimalDietaryClass.Carnivore, ActivityPattern = Animal.AnimalActivityPattern.Diurnal, SecurityRequirement = Animal.AnimalSecurityRequirement.High, Enclosure = "1", Prey = "Bird"},
                    new Animal { Id = 2, Name = "Tiger", Species = "Cat", Category = "Mammal", Size = Animal.AnimalSize.Large, DietaryClass = Animal.AnimalDietaryClass.Carnivore, ActivityPattern = Animal.AnimalActivityPattern.Nocturnal, SecurityRequirement = Animal.AnimalSecurityRequirement.High, Enclosure = "1", Prey = "Bird"},
                    new Animal { Id = 3, Name = "Elephant", Species = "Proboscidea", Category = "Mammal", Size = Animal.AnimalSize.VeryLarge, DietaryClass = Animal.AnimalDietaryClass.Herbivore, ActivityPattern = Animal.AnimalActivityPattern.Diurnal, SecurityRequirement = Animal.AnimalSecurityRequirement.Medium, Enclosure = "1", Prey = "Ant" }
                };
                context.AddRange(testData);
                context.SaveChanges();

                var controller = new AnimalsController(context);

                // Act
                var result = await controller.Index("lion", Animal.AnimalSize.Large, Animal.AnimalDietaryClass.Carnivore, null, Animal.AnimalSecurityRequirement.High, 1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IPagedList<Animal>>(viewResult.Model);

                //  Verify animal returned filter to match the output (1 animal matches the filter criteria)
                Assert.Equal(1, model.TotalItemCount);
            }
        }
    }
}
