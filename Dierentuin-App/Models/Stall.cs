using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dierentuin_App.Models.Behavior;
using Dierentuin_App.Models.Patterns.Composite;

namespace Dierentuin_App.Models
{
    public class Stall : IZooComponent
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        public string Climate { get; set; } = string.Empty;
        public string HabitatType { get; set; } = string.Empty;
        public string SecurityLevel { get; set; } = string.Empty;
        public double Size { get; set; }
        public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
        public DateTime? LastFedAt { get; set; }

        [NotMapped]
        public ICleaningStrategy? CleaningStrategy { get; set; }


        public Stall()
        {
            Animals = new HashSet<Animal>();
            Size = 0.0;
        }

        public string PerformCleaning()
        {
            return CleaningStrategy?.Clean() ?? "No cleaning strategy assigned.";
        }
        public string GetInfo()
        {
            string animalInfo = string.Join("\n", Animals.Select(a => a.GetInfo()));
            return $"Stall: {Name} ({HabitatType})\nAnimals:\n{animalInfo}";
        }

        public double GetSpace()
        {
            return Animals.Sum(a => a.GetSpace());
        }
    }
}
