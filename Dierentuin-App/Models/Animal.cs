using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dierentuin_App.Models
{
    public class Animal
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Species is required")]
        public string? Species { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? Category { get; set; }

        public AnimalSize Size { get; set; }

        public AnimalDietaryClass DietaryClass { get; set; }

        public AnimalActivityPattern ActivityPattern { get; set; }

        [Required(ErrorMessage = "Prey is required")]
        public string? Prey { get; set; }

        [Required(ErrorMessage = "Enclosure is required")]
        public string? Enclosure { get; set; }

        [Required(ErrorMessage = "Space requirment is required")]
        public double SpaceRequirement { get; set; }

        public AnimalSecurityRequirement SecurityRequirement { get; set; }

        // Foreign Key to Stall
        public int StallId { get; set; }

        public virtual Stall Stall { get; set; }

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


