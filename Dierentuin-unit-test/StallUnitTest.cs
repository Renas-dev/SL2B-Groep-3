using System;
using System.Linq;
using System.Threading.Tasks;
using Dierentuin_App.Controllers;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var controller = new UpdatedStallsController(context);
                var newStall = new UpdatedStall
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
                Assert.Equal(1, context.UpdatedStall.Count());
            }
        }

        [Fact]
        public async Task Update_Stall()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                var existingStall = new UpdatedStall
                {
                    Id = 1,
                    Name = "Updated stall",
                    Climate = "Tropical",
                    HabitatType = "Forest",
                    SecurityLevel = "High",
                    Size = 5.10
                };
                context.UpdatedStall.Add(existingStall);
                context.SaveChanges();

                var controller = new UpdatedStallsController(context);
                existingStall.Name = "Updated Stall";

                // Act
                var result = await controller.Edit(existingStall.Id, existingStall);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                var updatedStall = context.UpdatedStall.First();
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
                var existingStall = new UpdatedStall
                {
                    Id = 1,
                    Name = "Stall name for deletion",
                    Climate = "Tropical",
                    HabitatType = "Forest",
                    SecurityLevel = "High",
                    Size = 5.10
                };
                context.UpdatedStall.Add(existingStall);
                context.SaveChanges();

                var controller = new UpdatedStallsController(context);

                // Act
                var result = await controller.DeleteConfirmed(existingStall.Id);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(0, context.UpdatedStall.Count());
            }
        }
    }
}
