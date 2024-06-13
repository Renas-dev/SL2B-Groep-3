using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Data;
using Dierentuin_App.Models;

namespace Dierentuin_App.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly Dierentuin_AppContext _context;

        public AnimalsController(Dierentuin_AppContext context)
        {
            _context = context;
        }

        // GET: Animals
        // GET: Animals
        public async Task<IActionResult> Index(string searchString, Animal.AnimalSize? size, Animal.AnimalDietaryClass? dietaryClass, Animal.AnimalActivityPattern? activityPattern, Animal.AnimalSecurityRequirement? securityRequirement)
        {
            // Add search parameters and include Stall navigation property
            IQueryable<Animal> animals = _context.Animal.Include(a => a.Stall);

            // Search string values
            if (!String.IsNullOrEmpty(searchString))
            {
                animals = animals.Where(a =>
                    a.Name.Contains(searchString) ||
                    a.Species.Contains(searchString) ||
                    a.Category.Contains(searchString) ||
                    a.Prey.Contains(searchString));
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

            return View(await animals.ToListAsync());
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
