namespace Dierentuin_App.Services
{
    // This service is used to keep track of the day and night cycle in the application.
    public class DayNightService
    {
        private bool _isDay = true;
        public bool IsDay => _isDay;
        // This method is used to toggle the day and night cycle.
        public void ToggleDayNight()
        {
            _isDay = !_isDay;
        }
    }
}
