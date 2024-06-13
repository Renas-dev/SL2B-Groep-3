using System.Diagnostics;
using Dierentuin_App.Models;
using Dierentuin_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Data;

namespace Dierentuin_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly DayNightService _dayNightService;
        private readonly ILogger<HomeController> _logger;
        private readonly Dierentuin_AppContext _context; // Inject Dierentuin_AppContext

        public HomeController(ILogger<HomeController> logger, DayNightService dayNightService, Dierentuin_AppContext context)
        {
            _logger = logger;
            _dayNightService = dayNightService;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["IsDay"] = _dayNightService.IsDay;

            // Fetch the stall with the most animals
            var stallWithMostAnimals = _context.Stall
                                               .Include(s => s.Animals)
                                               .OrderByDescending(s => s.Animals.Count)
                                               .FirstOrDefault();

            return View(stallWithMostAnimals);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
