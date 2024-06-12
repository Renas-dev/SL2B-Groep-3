using Bogus;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dierentuin_App.Seeding
{
    public class DataSeeder
    {
        private readonly Dierentuin_AppContext _context;

        public DataSeeder(Dierentuin_AppContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Stall.Any()) return; // Check if the database has been seeded

            var stallFaker = new Faker<Stall>()
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.Climate, f => f.PickRandom(new[] { "Tropical", "Desert", "Temperate", "Polar" }))
                .RuleFor(s => s.HabitatType, f => f.PickRandom(new[] { "Forest", "Savannah", "Wetlands", "Mountain" }))
                .RuleFor(s => s.SecurityLevel, f => f.PickRandom(new[] { "Low", "Medium", "High" }))
                .RuleFor(s => s.Size, f => Math.Round(f.Random.Double(50, 200), 2));

            var stalls = stallFaker.Generate(10); // Generate 10 stalls
            _context.Stall.AddRange(stalls);
            _context.SaveChanges();

            var random = new Random();
            var animalFaker = new Faker<Animal>()
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Species, f => f.Lorem.Word())
                .RuleFor(a => a.Category, f => f.PickRandom(new[] { "Mammal", "Bird", "Reptile", "Fish", "Amphibian", "Insect" }))
                .RuleFor(a => a.Size, f => f.PickRandom<Animal.AnimalSize>())
                .RuleFor(a => a.DietaryClass, f => f.PickRandom<Animal.AnimalDietaryClass>())
                .RuleFor(a => a.ActivityPattern, f => f.PickRandom<Animal.AnimalActivityPattern>())
                .RuleFor(a => a.Prey, f => f.Lorem.Word())
                .RuleFor(a => a.Enclosure, f => f.Lorem.Word())
                .RuleFor(a => a.SpaceRequirement, f => Math.Round(f.Random.Double(1, 100), 2))
                .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<Animal.AnimalSecurityRequirement>())
                .RuleFor(a => a.StallId, f => random.Next(33, 42)); // Assign a random StallId

            var animals = animalFaker.Generate(50); // Generate 50 animals
            _context.Animal.AddRange(animals);
            _context.SaveChanges();
        }
    }
}
