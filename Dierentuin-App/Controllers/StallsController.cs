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
using Dierentuin_App.Models.Patterns.Composite;

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

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string climate, string habitatType, string securityLevel, int? page)
        {
            var stalls = _context.Stall.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                stalls = stalls.Where(s =>
                    (s.Name != null && s.Name.ToLower().Contains(lowerSearch)) ||
                    (s.Climate != null && s.Climate.ToLower().Contains(lowerSearch)) ||
                    (s.HabitatType != null && s.HabitatType.ToLower().Contains(lowerSearch))
                );
            }

            if (!string.IsNullOrEmpty(climate))
                stalls = stalls.Where(s => s.Climate == climate);
            if (!string.IsNullOrEmpty(habitatType))
                stalls = stalls.Where(s => s.HabitatType == habitatType);
            if (!string.IsNullOrEmpty(securityLevel))
                stalls = stalls.Where(s => s.SecurityLevel == securityLevel);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = await stalls.ToPagedListAsync(pageNumber, pageSize);

            return View(pagedList);
        }

        [HttpPost]
        public IActionResult FeedAllAnimals(int stallId)
        {
            var stall = _context.Stall.FirstOrDefault(s => s.Id == stallId);
            if (stall == null)
            {
                return Json(new { success = false, message = "Stall not found." });
            }

            var now = DateTime.UtcNow;

            if (stall.LastFedAt.HasValue && (now - stall.LastFedAt.Value).TotalMinutes < 10)
            {
                var minutesLeft = 10 - (now - stall.LastFedAt.Value).TotalMinutes;
                var fedAtUnix = new DateTimeOffset(stall.LastFedAt.Value).ToUnixTimeMilliseconds();

                return Json(new
                {
                    success = false,
                    message = $"Animals already fed. Try again in {Math.Ceiling(minutesLeft)} minutes.",
                    fedAt = fedAtUnix
                });
            }

            stall.LastFedAt = now;
            _context.SaveChanges();

            var updatedUnix = new DateTimeOffset(now).ToUnixTimeMilliseconds();

            return Json(new { success = true, fedAt = updatedUnix });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexPost(string searchString, string climate, string habitatType, string securityLevel)
        {
            return RedirectToAction(nameof(Index), new { searchString, climate, habitatType, securityLevel });
        }

        // GET: Stalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["IsDay"] = _dayNightService.IsDay;

            if (id == null)
                return NotFound();

            var stall = await _context.Stall
                .Include(s => s.Animals)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stall == null)
                return NotFound();

            stall.Size = stall.Animals.Sum(a => a.SpaceRequirement);
            await _context.SaveChangesAsync();

            switch (stall.HabitatType?.ToLower())
            {
                case "rainforest": stall.CleaningStrategy = new RainforestCleaning(); break;
                case "tundra": stall.CleaningStrategy = new TundraCleaning(); break;
                case "desert": stall.CleaningStrategy = new DesertCleaning(); break;
                default: stall.CleaningStrategy = null; break;
            }

            ViewData["CleaningInstruction"] = stall.PerformCleaning();

            // ✅ Composite info output
            ViewData["CompositeInfo"] = stall.GetInfo();

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
                return NotFound();

            var stall = await _context.Stall.FindAsync(id);
            if (stall == null)
                return NotFound();

            return View(stall);
        }

        // POST: Stalls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size")] Stall stall)
        {
            if (id != stall.Id)
                return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(stall);
        }

        // GET: Stalls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var stall = await _context.Stall.FirstOrDefaultAsync(m => m.Id == id);
            if (stall == null)
                return NotFound();

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
