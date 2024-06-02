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
    public class UpdatedAnimalsController : Controller
    {
        private readonly Dierentuin_AppContext _context;

        public UpdatedAnimalsController(Dierentuin_AppContext context)
        {
            _context = context;
        }

        // GET: UpdatedAnimals
        public async Task<IActionResult> Index()
        {
            return View(await _context.UpdatedAnimal.ToListAsync());
        }

        // GET: UpdatedAnimals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedAnimal = await _context.UpdatedAnimal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updatedAnimal == null)
            {
                return NotFound();
            }

            return View(updatedAnimal);
        }

        // GET: UpdatedAnimals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UpdatedAnimals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Category,Size,Dietary,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement")] UpdatedAnimal updatedAnimal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(updatedAnimal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedAnimal);
        }

        // GET: UpdatedAnimals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedAnimal = await _context.UpdatedAnimal.FindAsync(id);
            if (updatedAnimal == null)
            {
                return NotFound();
            }
            return View(updatedAnimal);
        }

        // POST: UpdatedAnimals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Category,Size,Dietary,ActivityPattern,Prey,Enclosure,SpaceRequirement,SecurityRequirement")] UpdatedAnimal updatedAnimal)
        {
            if (id != updatedAnimal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updatedAnimal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpdatedAnimalExists(updatedAnimal.Id))
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
            return View(updatedAnimal);
        }

        // GET: UpdatedAnimals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedAnimal = await _context.UpdatedAnimal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updatedAnimal == null)
            {
                return NotFound();
            }

            return View(updatedAnimal);
        }

        // POST: UpdatedAnimals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var updatedAnimal = await _context.UpdatedAnimal.FindAsync(id);
            if (updatedAnimal != null)
            {
                _context.UpdatedAnimal.Remove(updatedAnimal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpdatedAnimalExists(int id)
        {
            return _context.UpdatedAnimal.Any(e => e.Id == id);
        }
    }
}
