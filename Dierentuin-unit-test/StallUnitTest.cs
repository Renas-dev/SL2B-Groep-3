using System;
using System.Linq;
using System.Threading.Tasks;
using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dierentuin_unit_test
{
    public class StallUnitTest
    {
        private DbContextOptions<Dierentuin_AppContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<Dierentuin_AppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Create_Stall()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                var controller = new StallsController(context, Mock.Of<ILogger<StallsController>>());
                var newStall = new Stall
                {
                    Id = 1,
                    Name = "Stall naam",
                    Climate = "Tropical",
                    HabitatType = "Forest",
                    SecurityLevel = "Low",
                    Size = 10.05
                };

                // Act
                var result = await controller.Create(newStall);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(1, context.Stall.Count());
            }
        }

        [Fact]
        public async Task Update_Stall()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                var existingStall = new Stall
                {
                    Id = 1,
                    Name = "Stall",
                    Climate = "Tropical",
                    HabitatType = "Forest",
                    SecurityLevel = "High",
                    Size = 5.10
                };
                context.Stall.Add(existingStall);
                context.SaveChanges();

                // Create a mock logger
                var mockLogger = Mock.Of<ILogger<StallsController>>();

                // Initialize controller with context and mocked logger
                var controller = new StallsController(context, mockLogger);

                existingStall.Name = "Updated Stall";

                // Act
                var result = await controller.Edit(existingStall.Id, existingStall);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                var updatedStall = await context.Stall.FindAsync(existingStall.Id);
                Assert.Equal("Updated Stall", updatedStall.Name);
            }
        }


        [Fact]
        public async Task Delete_Stall()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                var existingStall = new Stall
                {
                    Id = 1,
                    Name = "Stall",
                    Climate = "Tropical",
                    HabitatType = "Forest",
                    SecurityLevel = "High",
                    Size = 5.10
                };
                context.Stall.Add(existingStall);
                context.SaveChanges();

                var controller = new StallsController(context, Mock.Of<ILogger<StallsController>>());

                // Act
                var result = await controller.DeleteConfirmed(existingStall.Id);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(0, context.Stall.Count());
            }
        }
    }
}