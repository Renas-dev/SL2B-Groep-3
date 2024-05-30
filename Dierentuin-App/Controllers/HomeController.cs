using System.Diagnostics;
using Dierentuin_App.Models;
using Dierentuin_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dierentuin_App.Controllers
{
    public class HomeController : Controller
    {
        // The DayNightService is used to manage and toggle the day/night state.
        private readonly DayNightService _dayNightService;
        private readonly ILogger<HomeController> _logger;

        // The constructor injects the DayNightService and the logger.
        public HomeController(ILogger<HomeController> logger, DayNightService dayNightService)
        {
            _logger = logger;
            _dayNightService = dayNightService;
        }
        // The Index action method returns the Index view and passes the IsDay property to the view.
        public IActionResult Index()
        {
            ViewData["IsDay"] = _dayNightService.IsDay;
            return View();
        }
        // The Privacy action method returns the Privacy view.
        public IActionResult Privacy()
        {
            return View();
        }
        // The AboutUs action method returns the AboutUs view.
        public IActionResult AboutUs()
        {
            return View();
        }
        // The ToggleDayNight action method toggles the day/night state and redirects to the Index action method.
        public IActionResult ToggleDayNight()
        {
            _dayNightService.ToggleDayNight();
            return RedirectToAction("Index");
        }
        // The Error action method returns the Error view.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}