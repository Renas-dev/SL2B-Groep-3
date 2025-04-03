namespace Dierentuin_App.Models
{
    // Interface for Animal
    public interface IAnimalFactory
    {
        Animal CreateAnimal();
    }

    // Factory implementations
    public class LionFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Lion",
                Species = "Panthera leo",
                Category = "Mammal",
                Size = Animal.AnimalSize.Large,
                DietaryClass = Animal.AnimalDietaryClass.Carnivore,
                ActivityPattern = Animal.AnimalActivityPattern.Cathemeral,
                Prey = "Zebra, Wildebeest, Antelope",
                Enclosure = "Savanna",
                SpaceRequirement = 1000,
                SecurityRequirement = Animal.AnimalSecurityRequirement.High,
            };
        }
    }

    public class MonkeyFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Monkey",
                Species = "Macaca",
                Category = "Mammal",
                Size = Animal.AnimalSize.Medium,
                DietaryClass = Animal.AnimalDietaryClass.Omnivore,
                ActivityPattern = Animal.AnimalActivityPattern.Diurnal,
                Prey = "Fruits, Nuts, Insects",
                Enclosure = "Jungle",
                SpaceRequirement = 250,
                SecurityRequirement = Animal.AnimalSecurityRequirement.Medium,
            };
        }
    }

    public class ElephantFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Elephant",
                Species = "Loxodonta africana",
                Category = "Mammal",
                Size = Animal.AnimalSize.VeryLarge,
                DietaryClass = Animal.AnimalDietaryClass.Herbivore,
                ActivityPattern = Animal.AnimalActivityPattern.Diurnal,
                Prey = "Grass, Leaves, Fruit",
                Enclosure = "Savanna",
                SpaceRequirement = 2000,
                SecurityRequirement = Animal.AnimalSecurityRequirement.Medium,
            };
        }
    }

    public class PenguinFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Penguin",
                Species = "Spheniscidae",
                Category = "Bird",
                Size = Animal.AnimalSize.Medium,
                DietaryClass = Animal.AnimalDietaryClass.Piscivore,
                ActivityPattern = Animal.AnimalActivityPattern.Diurnal,
                Prey = "Fish, Krill, Squid",
                Enclosure = "Cold Water",
                SpaceRequirement = 150,
                SecurityRequirement = Animal.AnimalSecurityRequirement.Low,
            };
        }
    }

    public class TigerFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Tiger",
                Species = "Panthera tigris",
                Category = "Mammal",
                Size = Animal.AnimalSize.Large,
                DietaryClass = Animal.AnimalDietaryClass.Carnivore,
                ActivityPattern = Animal.AnimalActivityPattern.Nocturnal,
                Prey = "Deer, Wild Boar, Buffalo",
                Enclosure = "Jungle",
                SpaceRequirement = 1200,
                SecurityRequirement = Animal.AnimalSecurityRequirement.High,
            };
        }
    }

    public class GiraffeFactory : IAnimalFactory
    {
        public Animal CreateAnimal()
        {
            return new Animal
            {
                Name = "Giraffe",
                Species = "Giraffa camelopardalis",
                Category = "Mammal",
                Size = Animal.AnimalSize.VeryLarge,
                DietaryClass = Animal.AnimalDietaryClass.Herbivore,
                ActivityPattern = Animal.AnimalActivityPattern.Diurnal,
                Prey = "Leaves, Fruits, Seeds",
                Enclosure = "Savanna",
                SpaceRequirement = 1800,
                SecurityRequirement = Animal.AnimalSecurityRequirement.Medium,
            };
        }
    }

    // Factory (provider)
    public static class AnimalFactory
    {
        public static IAnimalFactory GetFactory(string animal)
        {
            return animal switch
            {
                "lion" => new LionFactory(),
                "monkey" => new MonkeyFactory(),
                "elephant" => new ElephantFactory(),
                "penguin" => new PenguinFactory(),
                "tiger" => new TigerFactory(),
                "giraffe" => new GiraffeFactory(),
                _ => throw new ArgumentException($"Unsupported animal type: {animal}"),
            };
        }
    }
}