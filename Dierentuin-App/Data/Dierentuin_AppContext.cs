using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Models;
using Bogus;

namespace Dierentuin_App.Data
{
    public class Dierentuin_AppContext : DbContext
    {
        public Dierentuin_AppContext(DbContextOptions<Dierentuin_AppContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animal { get; set; } = default!;
        public DbSet<Stall> Stall { get; set; }
        // Add DbSet for Stall
        public void SeedDatabase()
        { // Seed the database with some initial data if stalls and animals are empty 
            try
            {
                Console.WriteLine("Ensuring database is created...");
                Database.EnsureCreated();

                Console.WriteLine("Checking for existing stalls...");
                if (!Stall.Any())
                {
                    Console.WriteLine("No stalls found. Seeding database with stalls...");
                    var stalls = GenerateStalls(2);
                    Stall.AddRange(stalls);
                    SaveChanges();
                    Console.WriteLine("Stalls seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Database already contains stalls. Skipping stall seeding.");
                }

                Console.WriteLine("Checking for existing animals...");
                if (!Animal.Any())
                {
                    Console.WriteLine("No animals found. Seeding database with animals...");
                    var animals = GenerateAnimals(10);
                    Animal.AddRange(animals);
                    SaveChanges();
                    Console.WriteLine("Animals seeded successfully.");
                }
                else
                {
                    Console.WriteLine("Database already contains animals. Skipping animal seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database seeding: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
        // Add method to generate stalls
        private List<Stall> GenerateStalls(int count)
        {// Generate a list of stalls with random data
            var faker = new Faker<Stall>()
                .RuleFor(s => s.Id, f => 0) // Let EF handle the ID
                .RuleFor(s => s.Name, f => f.Address.City())
                .RuleFor(s => s.Climate, f => f.PickRandom(new[] { "Tropical", "Arid", "Temperate", "Continental", "Polar" }))
                .RuleFor(s => s.HabitatType, f => f.PickRandom(new[] { "Forest", "Savanna", "Desert", "Wetlands", "Grassland", "Tundra", "Marine", "Freshwater" }))
                .RuleFor(s => s.SecurityLevel, f => f.PickRandom(new[] { "Low", "Medium", "High" }))
                .RuleFor(s => s.Size, f => Math.Round(f.Random.Double(50, 200), 2)); // Round to 2 decimal places
            // Generate the stalls and print them to the console
            var stalls = faker.Generate(count);
            Console.WriteLine($"Generated {stalls.Count} stalls.");
            foreach (var stall in stalls)
            {
                Console.WriteLine($"Generated stall: {stall.Name}, Climate: {stall.Climate}, Habitat: {stall.HabitatType}");
            }// Return the generated stalls
            return stalls;
        }
        // Add method to generate animals
        private List<Animal> GenerateAnimals(int count)
        {
            // Fetch existing Stall IDs
            var validStallIds = Stall.Select(s => s.Id).ToList();

            // Generate a list of animals with random data
            var faker = new Faker<Animal>()
                .RuleFor(a => a.Id, f => 0) // Let EF handle the ID
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Species, f => f.PickRandom(new[] { "Lion", "Tiger", "Elephant", "Giraffe", "Zebra" }))
                .RuleFor(a => a.Category, f => f.PickRandom(new[] { "Mammal", "Bird", "Reptile", "Amphibian", "Fish" }))
                .RuleFor(a => a.Size, f => f.PickRandom<Animal.AnimalSize>())
                .RuleFor(a => a.DietaryClass, f => f.PickRandom<Animal.AnimalDietaryClass>())
                .RuleFor(a => a.ActivityPattern, f => f.PickRandom<Animal.AnimalActivityPattern>())
                .RuleFor(a => a.Prey, f => f.PickRandom(new[] { "None", "Insects", "Small animals", "Fish", "Plants" }))
                .RuleFor(a => a.Enclosure, f => f.Address.StreetName())
                .RuleFor(a => a.SpaceRequirement, f => Math.Round(f.Random.Double(1, 5), 2)) // Round to 2 decimal places
                .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<Animal.AnimalSecurityRequirement>())
                .RuleFor(a => a.StallId, f => f.PickRandom(validStallIds)); // Ensure valid Stall Ids

            // Generate the animals and print them to the console
            var animals = faker.Generate(count);
            Console.WriteLine($"Generated {animals.Count} animals.");

            // Print the generated animals to the console
            foreach (var animal in animals)
            {
                Console.WriteLine($"Generated animal: {animal.Name}, Species: {animal.Species}, Enclosure: {animal.Enclosure}, Space Requirement: {animal.SpaceRequirement}");
            }

            // Return the generated animals
            return animals;
        }
    }
}
