using System;
using System.ComponentModel.DataAnnotations;

namespace Dierentuin_App.Models
{
    public class Animal
    {
        [Key] // Primary Key
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }
        public string? Category { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalDietaryClass DietaryClass { get; set; }
        public AnimalActivityPattern ActivityPattern { get; set; }
        public string? Prey { get; set; }
        public string? Enclosure { get; set; }
        public double SpaceRequirement { get; set; }
        public AnimalSecurityRequirement SecurityRequirement { get; set; }

        public enum AnimalSize
        {
            Microscopic,
            VerySmall,
            Small,
            Medium,
            Large,
            VeryLarge
        }

        public enum AnimalDietaryClass
        {
            Carnivore,
            Herbivore,
            Omnivore,
            Insectivore,
            Piscivore
        }

        public enum AnimalActivityPattern
        {
            Diurnal,
            Nocturnal,
            Cathemeral
        }

        public enum AnimalSecurityRequirement
        {
            Low,
            Medium,
            High,
            Critical
        }
    }
}


