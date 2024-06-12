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
    public class UpdatedStallsController : Controller
    {
        private readonly Dierentuin_AppContext _context;

        public UpdatedStallsController(Dierentuin_AppContext context)
        {
            _context = context;
        }

        // GET: UpdatedStalls
        public async Task<IActionResult> Index()
        {
            return View(await _context.UpdatedStall.ToListAsync());
        }

        // GET: UpdatedStalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedStall = await _context.UpdatedStall
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updatedStall == null)
            {
                return NotFound();
            }

            return View(updatedStall);
        }

        // GET: UpdatedStalls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UpdatedStalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] UpdatedStall updatedStall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(updatedStall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedStall);
        }

        // GET: UpdatedStalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedStall = await _context.UpdatedStall.FindAsync(id);
            if (updatedStall == null)
            {
                return NotFound();
            }
            return View(updatedStall);
        }

        // POST: UpdatedStalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] UpdatedStall updatedStall)
        {
            if (id != updatedStall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updatedStall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpdatedStallExists(updatedStall.Id))
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
            return View(updatedStall);
        }

        // GET: UpdatedStalls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatedStall = await _context.UpdatedStall
                .FirstOrDefaultAsync(m => m.Id == id);
            if (updatedStall == null)
            {
                return NotFound();
            }

            return View(updatedStall);
        }

        // POST: UpdatedStalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var updatedStall = await _context.UpdatedStall.FindAsync(id);
            if (updatedStall != null)
            {
                _context.UpdatedStall.Remove(updatedStall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpdatedStallExists(int id)
        {
            return _context.UpdatedStall.Any(e => e.Id == id);
        }
    }
}
