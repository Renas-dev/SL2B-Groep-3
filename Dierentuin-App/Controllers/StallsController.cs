using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dierentuin_App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Dierentuin_App.Data;
using Dierentuin_App.Models;
using X.PagedList;
using Dierentuin_App.Models.Factories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dierentuin_App.Models.Behavior;

namespace Dierentuin_App.Controllers
{
    public class StallsController : Controller
    {
        private readonly DayNightService _dayNightService;
        private readonly Dierentuin_AppContext _context;

        public StallsController(Dierentuin_AppContext context, DayNightService dayNightService)
        {
            _context = context;
            _dayNightService = dayNightService;
        }

        // GET: Stalls/Index
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string climate, string habitatType, string securityLevel, int? page)
        {
            // Use IQueryable to build query dynamically
            var stalls = _context.Stall.AsQueryable();

            // Apply filters based on parameters
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearchString = searchString.ToLower();
                stalls = stalls.Where(s =>
                    (s.Name != null && s.Name.ToLower().Contains(lowerSearchString)) ||
                    (s.Climate != null && s.Climate.ToLower().Contains(lowerSearchString)) ||
                    (s.HabitatType != null && s.HabitatType.ToLower().Contains(lowerSearchString))
                );
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

            // Number of items per page
            int pageSize = 10;
            // Current page number (default is 1)
            int pageNumber = (page ?? 1);

            // Execute query and return paged list to view
            var pagedList = await stalls.ToPagedListAsync(pageNumber, pageSize);
            return View(pagedList);


        }

        // POST: Stalls/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPost(string searchString, string climate, string habitatType, string securityLevel)
        {
            // Redirect to the GET action to apply the filtering
            return RedirectToAction(nameof(Index), new { searchString, climate, habitatType, securityLevel });
        }

        // GET: Stalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["IsDay"] = _dayNightService.IsDay;

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

            // Clean stall based on habitat type
            switch (stall.HabitatType?.ToLower())
            {
                case "rainforest":
                    stall.CleaningStrategy = new RainforestCleaning();
                    break;
                case "tundra":
                    stall.CleaningStrategy = new TundraCleaning();
                    break;
                case "desert":
                    stall.CleaningStrategy = new DesertCleaning();
                    break;
                default:
                    stall.CleaningStrategy = null;
                    break;
            }

            // Store cleaning result for the view
            ViewData["CleaningInstruction"] = stall.PerformCleaning();

            return View(stall);
        }

        // GET: Stalls/Create
        public IActionResult Create(string preset)
        {
            var stall = string.IsNullOrEmpty(preset)
                ? new Stall()
                : StallFactory.CreateStall(preset);

            ViewBag.PresetOptions = new SelectList(StallFactory.GetPresetNames());

            return View(stall);
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

            return View(stall);
        }

        [HttpGet]
        public IActionResult GetStallTemplate(string preset)
        {
            var stall = StallFactory.CreateStall(preset);

            return Json(new
            {
                name = stall.Name,
                climate = stall.Climate,
                habitatType = stall.HabitatType,
                securityLevel = stall.SecurityLevel,
                size = stall.Size
            });
        }


        // GET: Stalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stall = await _context.Stall
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stall == null)
            {
                return NotFound();
            }

            // 👇 DEBUG: Log RowVersion voor controle
            Console.WriteLine("Edit GET RowVersion: " + Convert.ToBase64String(stall.RowVersion ?? new byte[0]));

            return View(stall);
        }

        // POST: Stalls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size,RowVersion")] Stall stall)
        {
            if (id != stall.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Attach(stall);
                    _context.Entry(stall).Property(s => s.RowVersion).OriginalValue = stall.RowVersion;
                    _context.Entry(stall).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    var dbEntry = await _context.Stall.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == stall.Id);

                    if (dbEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Deze stal is verwijderd door een andere gebruiker.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Deze stal is bewerkt door een andere gebruiker terwijl jij deze aanpaste.");
                        stall.RowVersion = dbEntry.RowVersion;
                    }
                }
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StallExists(int id)
        {
            return _context.Stall.Any(e => e.Id == id);
        }
    }
}
