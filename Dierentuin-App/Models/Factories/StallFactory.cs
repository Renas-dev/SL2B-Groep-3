using System.Collections.Generic;

namespace Dierentuin_App.Models.Factories
{
    public static class StallFactory
    {
        private static readonly Dictionary<string, Stall> Presets = new()
        {
            { "Tropical Rainforest", new Stall { Name = "Tropical Paradise", Climate = "Tropical", HabitatType = "Rainforest", SecurityLevel = "High", Size = 150.0 } },
            { "Savannah Enclosure", new Stall { Name = "Savannah Plains", Climate = "Arid", HabitatType = "Grassland", SecurityLevel = "Medium", Size = 120.0 } },
            { "Arctic Zone", new Stall { Name = "Frozen Habitat", Climate = "Polar", HabitatType = "Tundra", SecurityLevel = "High", Size = 100.0 } }
        };

        public static IEnumerable<string> GetPresetNames() => Presets.Keys;

        public static Stall CreateStall(string presetName)
        {
            if (Presets.TryGetValue(presetName, out var preset))
            {
                // Return a new copy to avoid shared references
                return new Stall
                {
                    Name = preset.Name,
                    Climate = preset.Climate,
                    HabitatType = preset.HabitatType,
                    SecurityLevel = preset.SecurityLevel,
                    Size = preset.Size
                };
            }

            return new Stall(); // default empty stall if preset not found
        }
    }
}
