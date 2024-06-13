using System;

namespace Dierentuin_App.Services
{
    // This service is used to keep track of the day and night cycle in the application.
    public class DayNightService
    {
        public bool IsDay => DetermineIfDay();

        // This method is used to determine if it's day or night based on the current time in Amsterdam.
        private bool DetermineIfDay()
        {
            TimeZoneInfo amsterdamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime amsterdamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, amsterdamTimeZone);
            int lastDigitOfMinutes = amsterdamTime.Minute % 10;

            return lastDigitOfMinutes <= 4;
        }
    }
}