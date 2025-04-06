using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace Dierentuin_App.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly Dierentuin_AppContext _context;
        private readonly IMemoryCache _memoryCache;

        // Update your constructor to accept the cache
        public AnimalsController(Dierentuin_AppContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // GET: Animals
        public async Task<IActionResult> Index(string searchString, Animal.AnimalSize? size, Animal.AnimalDietaryClass? dietaryClass, Animal.AnimalActivityPattern? activityPattern, Animal.AnimalSecurityRequirement? securityRequirement, int? page)
        {
            // Add search parameters and include Stall navigation property
            IQueryable<Animal> animals = _context.Animal.Include(a => a.Stall);

            // Search string values
            if (!String.IsNullOrEmpty(searchString))
            {
                // Convert search string value to lowercase
                var lowerSearchString = searchString.ToLower();
                animals = animals.Where(a =>
                    (a.Name != null && a.Name.ToLower().Contains(lowerSearchString)) ||
                    (a.Species != null && a.Species.ToLower().Contains(lowerSearchString)) ||
                    (a.Category != null && a.Category.ToLower().Contains(lowerSearchString)) ||
                    (a.Prey != null && a.Prey.ToLower().Contains(lowerSearchString)));
            }

            // Search enum dropdown values
            if (size.HasValue || dietaryClass.HasValue || activityPattern.HasValue || securityRequirement.HasValue)
            {
                animals = animals.Where(a =>
                    (!size.HasValue || a.Size == size) &&
                    (!dietaryClass.HasValue || a.DietaryClass == dietaryClass) &&
                    (!activityPattern.HasValue || a.ActivityPattern == activityPattern) &&
                    (!securityRequirement.HasValue || a.SecurityRequirement == securityRequirement)
                );
            }

            // Number of items per page
            int pageSize = 25;
            // Current page number (default is 1)
            int pageNumber = (page ?? 1);

            // Execute query and return paged list to view
            var pagedList = await animals.ToPagedListAsync(pageNumber, pageSize);
            return View(pagedList);
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Stall)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            // Set the animal behavior based on the activity pattern
            switch (animal.ActivityPattern)
            {
                case Animal.AnimalActivityPattern.Diurnal:
                    animal.AnimalBehavior = new DiurnalBehavior();
                    break;
                case Animal.AnimalActivityPattern.Nocturnal:
                    animal.AnimalBehavior = new NocturnalBehavior();
                    break;
                case Animal.AnimalActivityPattern.Cathemeral:
                    animal.AnimalBehavior = new CathemeralBehavior();
                    break;
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            ViewData["StallId"] = new SelectList(_context.Stall, "Id", "Name");
            return View();
        }

        // POST: Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Category,Size,DietaryClass,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement,StallId")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                // Check if the Stall exists
                var stall = await _context.Stall.FindAsync(animal.StallId);
                if (stall == null)
                {
                    // Stall does not exist, return an error
                    ModelState.AddModelError("StallId", "Stall does not exist");
                    ViewData["StallId"] = new SelectList(_context.Stall, "Id", "Name", animal.StallId);
                    return View(animal);
                }

                // Stall exists, insert the Animal
                _context.Add(animal);
                await _context.SaveChangesAsync();

                // Update the stall's Size property immediately after adding the animal
                await UpdateStallSize(animal.StallId);

                return RedirectToAction(nameof(Index));
            }
            ViewData["StallId"] = new SelectList(_context.Stall, "Id", "Name", animal.StallId);
            return View(animal);
        }

        [HttpGet]
        public IActionResult GetAnimalTemplate(string animal)
        {
            try
            {
                Animal template = AnimalFactory.GetFactory(animal).CreateAnimal();
                return Json(template);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["StallId"] = new SelectList(_context.Stall, "Id", "Name", animal.StallId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Category,Size,DietaryClass,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement,StallId")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalAnimal = await _context.Animal.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    if (originalAnimal == null)
                    {
                        return NotFound();
                    }

                    var originalStallId = originalAnimal.StallId;

                    _context.Update(animal);
                    await _context.SaveChangesAsync();

                    // Update stall sizes for both the original and new stalls
                    if (originalStallId != animal.StallId)
                    {
                        await UpdateStallSize(originalStallId);
                        await UpdateStallSize(animal.StallId);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["StallId"] = new SelectList(_context.Set<Stall>(), "Id", "Id", animal.StallId);
            return View(animal);
        }


        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Stall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var stallId = animal.StallId;

            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            // Update the stall's Size property immediately after deleting the animal
            await UpdateStallSize(stallId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AnimalStatistics()
        {
            // Cache key
            const string cacheKey = "AnimalStatistics";

            // Get new statistics with a cache miss
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<string, object> cachedStats))
            {
                var animals = await _context.Animal.Include(a => a.Stall).ToListAsync();

                var grouped = animals
                    .GroupBy(a => a.Stall.Name)
                    .Select(g => new
                    {
                        Stall = g.Key,
                        Carnivores = g.Count(a => a.DietaryClass == Animal.AnimalDietaryClass.Carnivore),
                        Herbivores = g.Count(a => a.DietaryClass == Animal.AnimalDietaryClass.Herbivore),
                        Omnivores = g.Count(a => a.DietaryClass == Animal.AnimalDietaryClass.Omnivore),
                        Insectivores = g.Count(a => a.DietaryClass == Animal.AnimalDietaryClass.Insectivore),
                        Piscivores = g.Count(a => a.DietaryClass == Animal.AnimalDietaryClass.Piscivore),
                    }).ToList();

                // Store arrays in a dictionary
                cachedStats = new Dictionary<string, object>
                {
                    ["StallNames"] = grouped.Select(m => m.Stall).ToArray(),
                    ["Carnivores"] = grouped.Select(m => m.Carnivores).ToArray(),
                    ["Herbivores"] = grouped.Select(m => m.Herbivores).ToArray(),
                    ["Omnivores"] = grouped.Select(m => m.Omnivores).ToArray(),
                    ["Insectivores"] = grouped.Select(m => m.Insectivores).ToArray(),
                    ["Piscivores"] = grouped.Select(m => m.Piscivores).ToArray(),
                    ["LastUpdated"] = DateTime.Now,
                };

                // Store in cache with a 5 min expiration
                _memoryCache.Set(cacheKey, cachedStats, TimeSpan.FromMinutes(5));
            }

            // Use the statistics (cache or newly grabbed)
            ViewBag.StallNames = cachedStats["StallNames"];
            ViewBag.Carnivores = cachedStats["Carnivores"];
            ViewBag.Herbivores = cachedStats["Herbivores"];
            ViewBag.Omnivores = cachedStats["Omnivores"];
            ViewBag.Insectivores = cachedStats["Insectivores"];
            ViewBag.Piscivores = cachedStats["Piscivores"];
            ViewBag.LastUpdated = cachedStats["LastUpdated"];

            return View();
        }

        private async Task UpdateStallSize(int stallId)
        {
            // Calculate total space requirement of animals in the stall
            double totalSpaceRequirement = await _context.Animal
                .Where(a => a.StallId == stallId)
                .SumAsync(a => a.SpaceRequirement);

            // Update the stall's Size property
            var stall = await _context.Stall.FindAsync(stallId);
            if (stall != null)
            {
                stall.Size = totalSpaceRequirement;
                await _context.SaveChangesAsync();
            }
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
