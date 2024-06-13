using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Dierentuin_App.Data;
using Dierentuin_App.Models;

namespace Dierentuin_App.Controllers
{
    public class StallsController : Controller
    {
        private readonly Dierentuin_AppContext _context;
        private readonly ILogger<StallsController> _logger;

        public StallsController(Dierentuin_AppContext context, ILogger<StallsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Stalls/Index
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string climate, string habitatType, string securityLevel)
        {
            // Use IQueryable to build query dynamically
            var stalls = _context.Stall.AsQueryable();

            // Apply filters based on parameters
            if (!string.IsNullOrEmpty(searchString))
            {
                stalls = stalls.Where(s =>
                    s.Name.Contains(searchString) ||
                    s.Climate.Contains(searchString) ||
                    s.HabitatType.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(climate))
            {
                stalls = stalls.Where(s => s.Climate == climate);
            }

            if (!string.IsNullOrEmpty(habitatType))
            {
                stalls = stalls.Where(s => s.HabitatType == habitatType);
            }

            if (!string.IsNullOrEmpty(securityLevel))
            {
                stalls = stalls.Where(s => s.SecurityLevel == securityLevel);
            }

            return View(await stalls.ToListAsync());
        }

        // POST: Stalls/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(string searchString, string climate, string habitatType, string securityLevel)
        {
            // Redirect to the GET action to apply the filtering
            return RedirectToAction(nameof(Index), new { searchString, climate, habitatType, securityLevel });
        }

        // GET: Stalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stall = await _context.Stall
                .Include(s => s.Animals)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stall == null)
            {
                return NotFound();
            }

            // Calculate total space requirement of assigned animals
            double totalSpaceRequirement = stall.Animals.Sum(a => a.SpaceRequirement);

            // Update the stall size in the database
            stall.Size = totalSpaceRequirement;

            // Save changes to database
            await _context.SaveChangesAsync();

            return View(stall);
        }

        // GET: Stalls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stalls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] Stall stall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log the model state errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
            }

            return View(stall);
        }

        // GET: Stalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stall = await _context.Stall.FindAsync(id);
            if (stall == null)
            {
                return NotFound();
            }
            return View(stall);
        }

        // POST: Stalls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] Stall stall)
        {
            if (id != stall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StallExists(stall.Id))
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
            return View(stall);
        }

        // GET: Stalls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stall = await _context.Stall
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stall == null)
            {
                return NotFound();
            }

            return View(stall);
        }

        // POST: Stalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stall = await _context.Stall.FindAsync(id);
            if (stall != null)
            {
                _context.Stall.Remove(stall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StallExists(int id)
        {
            return _context.Stall.Any(e => e.Id == id);
        }
    }
}
