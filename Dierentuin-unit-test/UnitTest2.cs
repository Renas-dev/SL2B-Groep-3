using Dierentuin_App.Services;
using Xunit;

namespace Dierentuin_unit_test
{
    public class UnitTest2
    {
        [Fact]
        public void ToggleDayNight_ShouldToggleIsDay()
        {
            // Arrange
            var service = new DayNightService();

            // Act & Assert - Initial State (Day)
            Assert.True(service.IsDay, "Expected initial state to be day.");

            // Act - First Toggle (Day to Night)
            service.ToggleDayNight();
            bool firstToggleResult = service.IsDay;

            // Assert - After First Toggle (Night)
            Assert.False(firstToggleResult, "Expected IsDay to be false after first toggle (day to night).");

            // Act - Second Toggle (Night to Day)
            service.ToggleDayNight();
            bool secondToggleResult = service.IsDay;

            // Assert - After Second Toggle (Day)
            Assert.True(secondToggleResult, "Expected IsDay to be true after second toggle (night to day).");

            // Act - Third Toggle (Day to Night)
            service.ToggleDayNight();
            bool thirdToggleResult = service.IsDay;

            // Assert - After Third Toggle (Night)
            Assert.False(thirdToggleResult, "Expected IsDay to be false after third toggle (day to night).");
        }
    }
}
