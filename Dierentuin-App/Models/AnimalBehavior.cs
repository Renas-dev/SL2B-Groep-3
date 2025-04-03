namespace Dierentuin_App.Models
{
    // Interface animal activity pattern behaviors
    public interface IAnimalBehavior
    {
        string Behavior();
    }

    // Behaviors based on ActivityPattern
    public class DiurnalBehavior : IAnimalBehavior
    {
        public string Behavior()
        {
            return "Active during daylight hours, rests at night.";
        }

    }

    public class NocturnalBehavior : IAnimalBehavior
    {
        public string Behavior()
        {
            return "Sleeps during the day and becomes active at night.";
        }
    }

    public class CathemeralBehavior : IAnimalBehavior
    {
        public string Behavior()
        {
            return "Active during both day and night, alternating periods of activity.";
        }

    }
}
