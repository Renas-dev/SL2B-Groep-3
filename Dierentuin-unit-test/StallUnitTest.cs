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
using Dierentuin_App.Services;
using X.PagedList;

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
                var mockDayNightService = new Mock<DayNightService>();
                var controller = new StallsController(context, mockDayNightService.Object);
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
                var mockDayNightService = new Mock<DayNightService>();
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

                // Initialize controller with context
                var controller = new StallsController(context, mockDayNightService.Object);

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
                var mockDayNightService = new Mock<DayNightService>();
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

                var controller = new StallsController(context, mockDayNightService.Object);

                // Act
                var result = await controller.DeleteConfirmed(existingStall.Id);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(0, context.Stall.Count());
            }
        }

        // Unit test Filter Stall
        [Fact]
        public async Task Filter_Stall()
        {
            // Arrange
            var options = GetDbContextOptions();
            using (var context = new Dierentuin_AppContext(options))
            {
                var mockDayNightService = new Mock<DayNightService>();

                // Insert stall data to database
                var testData = new List<Stall>
                {
                    new Stall { Id = 1, Name = "Stall 1", Climate = "Tropical", HabitatType = "Forest", SecurityLevel = "High", Size = 5.10 },
                    new Stall { Id = 2, Name = "Stall 2", Climate = "Temperate", HabitatType = "Mountain", SecurityLevel = "Low", Size = 3.50 },
                    new Stall { Id = 3, Name = "Stall 3", Climate = "Tropical", HabitatType = "Jungle", SecurityLevel = "Medium", Size = 7.20 }
                };
                context.Stall.AddRange(testData);
                context.SaveChanges();

                var controller = new StallsController(context, mockDayNightService.Object);

                // Act
                var result = await controller.Index(null, "Tropical", null, null, 1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IPagedList<Stall>>(viewResult.Model);

                // Verify stall returned filter to match the output (2 stalls match the Tropical climate filter)
                Assert.Equal(2, model.TotalItemCount);
            }
        }

    }
}