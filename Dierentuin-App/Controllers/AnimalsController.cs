using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using System.Drawing;

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
        public async Task<IActionResult> Index(string searchString, Animal.AnimalSize? size, Animal.AnimalDietaryClass? dietaryClass, Animal.AnimalActivityPattern? activityPattern, Animal.AnimalSecurityRequirement? securityRequirement)
        {
            ViewBag.Size = size;
            ViewBag.DietaryClass = dietaryClass;
            ViewBag.ActivityPattern = activityPattern;
            ViewBag.SecurityRequirement = securityRequirement;
            ViewBag.SearchString = searchString;

            var animals = from a in _context.Animal
                          select a;

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
            return View();
        }

        // POST: Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Category,Size,DietaryClass,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(animal);
        }

        // POST: Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Category,Size,DietaryClass,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
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
            if (animal != null)
            {
                _context.Animal.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
